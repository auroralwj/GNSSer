//2016.08.09, czs, create in fujianyongan, WM星间差分FCB计算器
//2016.08.19, czs, 安徽 黄山 屯溪, 宽项调通，重构
//2016.10.17.13.09, czs, 宽项计算，直接处产品
//2016.10.20, czs, edit in hongqing, 窄巷计算

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
    /// 负责一个测站所有的卫星与指定基准星的差分计算。WM宽项星间差分FCB计算器。
    /// </summary>
    public class EpochNarrowLaneSolver : Reviser<EpochInformation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn"></param>
        /// <param name="Name"></param> 
        public EpochNarrowLaneSolver(SatelliteNumber BasePrn, string Name,int skipCount, bool SetWeightWithSat = false, double maxError = 2, bool IsOutputDetails = true)
        {
            this.Name = Name;
            this.BasePrn = BasePrn;
            this.IsSetWeightWithSat = SetWeightWithSat;
            this.IsOutputDetails = IsOutputDetails;
            var outputDirectory = Path.Combine(Gnsser.Setting.GnsserConfig.TempDirectory, "Details");
            this.TableTextManager = new ObjectTableManager(outputDirectory);
            this.TableStorage = this.TableTextManager.GetOrCreate(OutputTableName);
              
            this.PeriodFilterManager = new PeriodPipeFilterManager(1, 0);
            this.WideLaneFilterManager = new CyclicalNumerFilterManager(maxError, false);
            this.SatWeightProvider = new SatElevateAndRangeWeightProvider();
            this.LatestEpochSatWmValues = new EpochSatDifferValueManager();
            this.LatestEpochNarrowLaneValues = new EpochSatValueManager();
            this.LatestEpochFloatAmbiguityValues = new EpochSatDifferValueManager();
            this.NarrowLaneFilterManager = new CyclicalNumerFilterManager(maxError, false);
            this.NarrowBufferManager = new NumeralWindowDataManager(10);
        } 

        #region 属性  
        #region 数据源
        /// <summary>
        /// 精密单点定位结果。
        /// </summary>
        public PppResult PppResult { get; set; }
        /// <summary>
        /// PPP结果缓存。
        /// </summary>
        public IWindowData<PppResult> PppResultBuffers { get; set; }
        #endregion

        /// <summary>
        /// 按照卫星（高度角和距离）定权。否为等权处理。
        /// </summary>
        public bool IsSetWeightWithSat { get; set; }
        /// <summary>
        /// 是否输出详细计算信息
        /// </summary>
        public bool IsOutputDetails { get; set; }
        
        #region 产品
        /// <summary>
        /// 管理器,存储最新的历元宽项,若有更新，则直接覆盖之。
        /// </summary>
        public EpochSatDifferValueManager LatestEpochSatWmValues { get; set; }
        /// <summary>
        /// 最新的浮点解信息
        /// </summary>
        public EpochSatDifferValueManager LatestEpochFloatAmbiguityValues { get; set; }
        /// <summary>
        /// 最新的窄巷解。
        /// </summary>
        public EpochSatValueManager LatestEpochNarrowLaneValues { get; set; }
        #endregion

        /// <summary>
        /// 可能周跳数据管理器。
        /// </summary>
        public CyclicalNumerFilterManager WideLaneFilterManager{get;set;}

        /// <summary>
        /// 可能周跳数据管理器。
        /// </summary>
        public CyclicalNumerFilterManager NarrowLaneFilterManager { get; set; }
        /// <summary>
        /// 周期滤波器，将原始值进行周期滤波，使其在0附近。
        /// </summary>
        public PeriodPipeFilterManager PeriodFilterManager { get; set; }
        /// <summary>
        /// 输出表格名称，一个文件一个输出，利于调试和查找问题。
        /// </summary>
        public string OutputTableName { get { return BasePrn + "_" + Name + "_Detail"; } } 
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

        /// <summary>
        /// 已解出的宽项值
        /// </summary>
        public DifferFcbManager DifferFcbManager { get; set; }
        /// <summary>
        /// 窗口数据。用于判断是周跳，还是粗差。窄巷
        /// </summary>
        public NumeralWindowDataManager NarrowBufferManager { get; set; }

        #region 计算细节
        /// <summary>
        /// 卫星
        /// </summary>
        public EpochSatellite Sat { get; set; }
        /// <summary>
        /// 第一频率
        /// </summary>
        public Frequence FrequenceA { get { return Sat.FrequenceA.Frequence; } }
        /// <summary>
        /// 第二频率
        /// </summary>
        public Frequence FrequenceB { get { return Sat.FrequenceB.Frequence; } }
        /// <summary>
        /// 之于浮模糊度点解的无电离层乘法因子。
        /// 对于特定系统这是一个常量。应该提前赋值。
        /// 2016.10.20, czs, 
        /// </summary>
        public double FloatAmbiguityMultiFactor { get { return (FrequenceA.Value + FrequenceB.Value) / FrequenceA.Value; } }

        /// <summary>
        /// 之于宽项模糊度整数解的乘法因子。
        /// 对于特定系统这是一个常量。应该提前赋值。
        /// 2016.10.20, czs, 
        /// </summary>
        public double WideLaneMultiFactor { get { return FrequenceB.Value / (FrequenceA.Value - FrequenceB.Value); } }
        /// <summary>
        /// 计算窄巷模糊度浮点解
        /// </summary>
        /// <param name="Sat"></param>
        /// <param name="FloatAmbiguityDiffer"></param>
        /// <param name="wideLaneInteger"></param>
        /// <returns></returns>
        public double GetNarrowLaneValue(EpochSatellite Sat, double FloatAmbiguityDiffer, int wideLaneInteger)
        {
            this.Sat = Sat;
            //求乘数因子
            var narrowLane =
                (FloatAmbiguityMultiFactor * FloatAmbiguityDiffer
                - WideLaneMultiFactor * wideLaneInteger);

            return narrowLane;
        }



        #endregion


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
            if (!epochInfo.EnabledPrns.Contains(BasePrn)) { return false; }

            //1.计算宽项滤波值，并存储
            foreach (var sat in epochInfo) { SolveAndUpdateWideLane(sat); }

            //2.计算平滑宽项星间差分值
            var baseMwValue = this.LatestEpochSatWmValues[BasePrn];
            foreach (var sat in epochInfo) //只处理本历元具有的卫星
            {
                var val = this.LatestEpochSatWmValues[sat.Prn];
                //计算差分值
                SovleAndUpdateDifferValue(val, baseMwValue);
            }

            //3.提取PPP计算结果，模糊度差分结果,先不做平滑
            foreach (var prn in epochInfo.EnabledPrns)
            {
                var floatVal = PppResult.GetFloatAmbiguityCycle(prn);
                LatestEpochFloatAmbiguityValues[prn] = new EpochSatDifferValue()
                {
                    Index = epochInfo.EpochIndexOfDay,
                    Prn = prn,
                    RawValue = floatVal,
                    SmoothValue = floatVal,
                };
            }
            //3.1 星间单差值，更新到结果
            var baseFloat = this.LatestEpochFloatAmbiguityValues[BasePrn];
            foreach (var prn in epochInfo.EnabledPrns)
            {
                var val = LatestEpochFloatAmbiguityValues[prn];
                val.DifferValue = val.SmoothValue - baseFloat.SmoothValue;
            }

            //4.计算窄巷值
            foreach (var sat in epochInfo.EnabledSats)
            {
                var prn = sat.Prn;
                var prnKey = prn.ToString();
                var val = LatestEpochFloatAmbiguityValues[prn];
                var wideLane = this.LatestEpochSatWmValues[sat.Prn];

                double fcbOfWideLaneDiffer = 0;
                var differkey = BuildDifferKey(prn);
                if (DifferFcbManager != null && DifferFcbManager.Contains(differkey))
                {
                    var dcb = DifferFcbManager.Get(differkey).Last;
                    fcbOfWideLaneDiffer = dcb.WideLaneValue;
                    if (Math.Abs(dcb.Time - sat.ReceiverTime) > 7 * 3600 * 24)
                    {
                        log.Warn("提供的宽项星间单差已经超过一周！");
                    }
                }
                var intWideLane = wideLane.DifferValue - fcbOfWideLaneDiffer;//此处应该减去星间单差的小数部分。2016.10.20.
                var wideLaneInt = (int)Math.Round(intWideLane);
                var narrowValue = GetNarrowLaneValue(sat, val.DifferValue, wideLaneInt); //此处应该固定模糊度，不应该直接取整。

                this.LatestEpochNarrowLaneValues[prn] = new EpochSatValue()
                {
                    Index = epochInfo.EpochIndexOfDay,
                    Prn = prn,
                    RawValue = narrowValue,
                    Tag = wideLaneInt // 存储整型宽项模糊度
                };

                //获取小数部分
                var narrowBuffer = NarrowBufferManager.GetOrCreate(prnKey);
                if (narrowBuffer.IsFull)
                {
                    var filter = this.NarrowLaneFilterManager.GetOrCreate(prnKey);
                    filter.Buffers = narrowBuffer;
                    var rms = IsSetWeightWithSat ? SatWeightProvider.GetStdDev(sat) : 1;
                    var input = new RmsedNumeral(narrowValue, rms);
                    var smoothAligned = filter.Filter(input);
                    this.LatestEpochNarrowLaneValues[prn].SmoothValue = smoothAligned.Value;
                }

                narrowBuffer.Add(narrowValue);
            }

            //3.输出
            if (IsOutputDetails)
            {               
                TableStorage.NewRow();
                TableStorage.AddItem("Epoch", epochInfo.ReceiverTime.ToShortTimeString());

                foreach (var sat in epochInfo.EnabledSats) //只处理本历元具有的卫星
                {

                    var val = this.LatestEpochSatWmValues[sat.Prn];
                    var prnKey = val.Prn.ToString();
                    var wideLaneKey = BuildWideLaneKey(prnKey);

                    //宽项
                    TableStorage.AddItem(wideLaneKey + "_Raw", val.RawValue);
                    TableStorage.AddItem(wideLaneKey + "_Smooth", val.SmoothValue);
                    TableStorage.AddItem(wideLaneKey + "_MwDiffer", val.DifferValue);

                    //浮点解
                    var floatVal = this.LatestEpochFloatAmbiguityValues[sat.Prn];
                    var floatKey = prnKey;
                    var differKey = BuildDifferKey(prnKey);

                    TableStorage.AddItem(floatKey + "_FloatAmbi_Raw", floatVal.RawValue);
                    //TableStorage.AddItem(floatKey + "_Smooth", floatVal.SmoothValue);
                    TableStorage.AddItem(differKey, floatVal.DifferValue);

                    //窄巷
                    var narrowVal = this.LatestEpochNarrowLaneValues[sat.Prn];
                    var narrowLaneKey = BuildNarrowLaneKey(prnKey);
                    TableStorage.AddItem(narrowLaneKey + "_Raw", narrowVal.RawValue);
                    TableStorage.AddItem(narrowLaneKey + "_WideLaneInt", narrowVal.Tag);
                    TableStorage.AddItem(narrowLaneKey + "_Smooth", narrowVal.SmoothValue);
                    //TableStorage.AddItem(narrowLaneKey + "_MwDiffer", narrowVal.DifferValue); 
                }
            }
            return true;
        }

        /// <summary>
        /// 计算平滑后的差分值，星间单差。
        /// </summary>
        /// <param name="satValue"></param>
        /// <param name="baseSatValue"></param>
        private void SovleAndUpdateDifferValue(EpochSatDifferValue satValue, EpochSatDifferValue baseSatValue)
        {
            satValue.DifferValue = satValue.SmoothValue - baseSatValue.SmoothValue;
        }

        #region  计算宽项
        /// <summary>
        /// 1. 处理一颗卫星的宽项值，包含和基准卫星的差分值。
        /// </summary>
        /// <param name="tableRow"></param>
        /// <param name="sat"></param>
        private void SolveAndUpdateWideLane( EpochSatellite sat)
        {
            var prn = sat.Prn;
            var prnKey = prn.ToString();
            var smoothKey = BuildWideLaneKey(prnKey);
           
            var mwCycle = sat.Combinations.MwPhaseCombinationValue; // MW 值 
            var filter = this.WideLaneFilterManager.GetOrCreate(prnKey);

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
        /// 构建宽项关键字
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        private string BuildWideLaneKey(Object prn) { return prn + "_WL"; }
        /// <summary>
        /// 构建窄巷关键字
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        private string BuildNarrowLaneKey(Object prn) { return prn + "_NL"; }

        private string BuildDifferKey(Object prn) { return prn + "-" + BasePrn; }
        #endregion


        #region 计算模糊度浮点解
        /// <summary>
        /// 浮点解缓存
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public NumeralWindowData GetFloatAmbiguities(SatelliteNumber prn)
        {
            List<double> list = new List<double>();
            foreach (var item in this.PppResultBuffers)
            {
                if (item.HasAmbituityValue(prn))
                {
                    var sat = item.GetFloatAmbiguityCycle(prn);
                    list.Add(sat); 
                }           
            }
            return new NumeralWindowData(list);
        } 

        #endregion  



        /// <summary>
        /// 结果写入文件
        /// </summary>
        public void WriteToFile()
        {
           TableTextManager.WriteAllToFileAndCloseStream();
        } 
        #endregion
    } 
}