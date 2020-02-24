//2016.08.09, czs, create in fujianyongan, WM星间差分FCB计算器
//2016.08.19, czs, 安徽 黄山 屯溪, 宽项调通，重构
//2016.10.17.13.09, czs, 宽项计算，直接处产品

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;

namespace Gnsser
{ 

    /// <summary>
    /// WM宽项星间差分FCB计算器。
    /// 负责一个测站所有的卫星与指定基准星的差分计算。
    /// </summary>
    public class EpochMwWideLaneSolver : Reviser<EpochInformation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn"></param>
        /// <param name="Name"></param> 
        public EpochMwWideLaneSolver(SatelliteNumber BasePrn, string Name, bool SetWeightWithSat = false, double maxError = 2, bool IsOutputDetails= true)
        {
            this.Name = Name;
            this.BasePrn = BasePrn;
            this.IsSetWeightWithSat = SetWeightWithSat;
            this.IsOutputDetails = IsOutputDetails;
            var outputDirectory = Path.Combine(Gnsser.Setting.GnsserConfig.TempDirectory, "Details");
            this.TableTextManager = new ObjectTableManager(outputDirectory);
            this.TableStorage = this.TableTextManager.GetOrCreate(BasePrn + "_" + Name + "_FloatWL");
               
            this.CyclicalNumerFilterManager = new CyclicalNumerFilterManager(maxError, false);
            this.SatWeightProvider = new SatElevateAndRangeWeightProvider();
            this.LatestEpochSatWmValues = new EpochSatDifferValueManager();
        } 

        #region 属性 
        /// <summary>
        /// 按照卫星（高度角和距离）定权。否为等权处理。
        /// </summary>
        public bool IsSetWeightWithSat { get; set; }
        /// <summary>
        /// 是否输出详细计算信息
        /// </summary>
        public bool IsOutputDetails { get; set; }

        /// <summary>
        /// 管理器,存储最新的历元,若有更新，则直接覆盖之。
        /// </summary>
        public EpochSatDifferValueManager LatestEpochSatWmValues { get; set; }
        /// <summary>
        /// 可能周跳数据管理器。
        /// </summary>
        public CyclicalNumerFilterManager CyclicalNumerFilterManager{get;set;}
         
        /// <summary>
        /// 基准卫星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 表输出
        /// </summary>
        public ObjectTableManager TableTextManager { get; set; }
        /// <summary>
        /// 当前表存储
        /// </summary>
        public ObjectTableStorage TableStorage { get; set; }
        /// <summary>
        /// 卫星定权
        /// </summary>
        SatElevateAndRangeWeightProvider SatWeightProvider { get; set; }  
        #endregion

        #region  方法
        /// <summary>
        /// 是否可以
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsAvailable(EpochInformation obj)
        {
            if (!obj.EnabledPrns.Contains(BasePrn)) { return false; }
            return true;
        }

        /// <summary>
        /// 遍历每一个历元
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation epochInfo)
        {           
            //如果没有基准卫星，则不可计算
            if (!epochInfo.TotalPrns.Contains(BasePrn)) { return false; }            
             
            //1.计算所有的滤波值，并存储
            foreach (var sat in epochInfo)  { CaculateOneSat(sat); }

            //2.构建星间差分对象
            var baseMwValue = this.LatestEpochSatWmValues[BasePrn];  
            foreach (var sat in epochInfo) //只处理本历元具有的卫星
            {
                var val = this.LatestEpochSatWmValues[sat.Prn];
                //计算差分值
                CaculateDifferValue(val, baseMwValue);
            }

            //3.输出
            if (IsOutputDetails)
            {
                TableStorage.NewRow();
                TableStorage.AddItem("Epoch", epochInfo.ReceiverTime.ToShortTimeString()); 

                foreach (var sat in epochInfo) //只处理本历元具有的卫星
                {
                    var val = this.LatestEpochSatWmValues[sat.Prn];
                    var prnKey = val.Prn.ToString();
                    var smoothKey = BuildSmoothKey(prnKey);

                    //表格输出小数部分
                    TableStorage.AddItem(prnKey + "_Raw", val.RawValue);
                    TableStorage.AddItem(smoothKey, val.SmoothValue);
                    TableStorage.AddItem(prnKey + "_MwDiffer", val.DifferValue);
                }
            }
            return true;
        }

        /// <summary>
        /// 计算平滑后的差分值，星间单差。
        /// </summary>
        /// <param name="satValue"></param>
        /// <param name="baseSatValue"></param>
        private void CaculateDifferValue(EpochSatDifferValue satValue, EpochSatDifferValue baseSatValue)
        {
            satValue.DifferValue = satValue.SmoothValue - baseSatValue.SmoothValue;
        }  


        /// <summary>
        ///1 处理一颗卫星
        /// </summary>
        /// <param name="tableRow"></param>
        /// <param name="sat"></param>
        private void CaculateOneSat( EpochSatellite sat)
        {
            var prn = sat.Prn;
            var prnKey = prn.ToString();
            var smoothKey = BuildSmoothKey(prnKey);
           
            var mwCycle = sat.Combinations.MwPhaseCombinationValue; // MW 值 
            var filter = this.CyclicalNumerFilterManager.GetOrCreate(prnKey);

            filter.Buffers = GetMwValues(prn);
            var rms = IsSetWeightWithSat ?  SatWeightProvider.GetStdDev(sat) : 1;
            var input = new RmsedNumeral(mwCycle, rms);
            var smoothAligned = filter.Filter(input);
            
            LatestEpochSatWmValues[sat.Prn] = new EpochSatDifferValue()
            {
                Index = sat.EpochInfo.EpochIndexOfDay,
                Prn = sat.Prn,
                RawValue = mwCycle - filter.IntegerPart,
                SmoothValue = smoothAligned.Value
            };
        }

        /// <summary>
        /// 获取指定卫星的MW原始值列表。缓存。
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public NumeralWindowData GetMwValues(SatelliteNumber prn)
        { 
            List<double> list = new List<double>();
            foreach (var item in this.Buffers)
         	{
                var sat = item.Get(prn);
                if(sat ==null){continue;}

                list.Add(sat.Combinations.MwPhaseCombinationValue);		 
         	}
            return new NumeralWindowData(list);
        }
         
        /// <summary>
        /// 构建平滑关键字
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        private string BuildSmoothKey(Object prn) { return prn + "_Smooth"; }  

        /// <summary>
        /// 结果写入文件
        /// </summary>
        public void WriteToFile()
        {
            TableTextManager.ReductValuesTo();
           TableTextManager.WriteAllToFileAndCloseStream();
        } 
        #endregion
    } 
}