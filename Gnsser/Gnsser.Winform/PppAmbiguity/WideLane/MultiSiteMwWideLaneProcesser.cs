//2016.08.09, czs, create in fujian yongan, WM星间差分FCB计算器
//2016.08.09, czs, edit in hongqing, 多测站，多历元 WM星间差分FCB计算器
//2016.08.19, czs, edit in huangshang, 宽项收敛稳定
//2016.08.20, czs, edit in 福建永安, 重构输出
//2016.10.18, czs, edit in hongqing, 重命名为 MultiSiteMwWideLaneProcesser

using System;
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
    /// 多测站，多历元 WM星间差分FCB计算器
    /// </summary>
    public class MultiSiteMwWideLaneProcesser : Reviser<MultiSiteEpochInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn"></param>
        /// <param name="EpochCount"></param>
        public MultiSiteMwWideLaneProcesser(SatelliteNumber BasePrn, string directory, bool SetWeightWithSat, double MaxError, bool isOutputDetail)
        {
            this.Name = "无电离层模糊度宽项网解"; 
            this.BasePrn = BasePrn;
            this.OutputDirectory = directory;
            this.IsOutputDetail = isOutputDetail;
            this.OutputProductInterval = 10 * 60;
            this.TableManager = new ObjectTableManager(directory);
            this.MwWideLaneSolvers = new MultiSiteMwWideLaneSolver(BasePrn, SetWeightWithSat, MaxError, isOutputDetail);
            this.DifferFcbManager = new DifferFcbManager();
            this.PeriodFilterManager = new PeriodPipeFilterManager(1, 0.5);
            this.LastProductTime = Time.MinValue;
        } 

        #region 属性  
        /// <summary>
        /// 产品输出采样间隔,单位：秒。
        /// </summary>
        public int OutputProductInterval { get; set; }
        /// <summary>
        /// 最后一次输出产品的时间。
        /// </summary>
        public Time LastProductTime { get; set; }
        /// <summary>
        /// 导出详细信息。
        /// </summary>
        public bool IsOutputDetail { get; set; }
        /// <summary>
        /// 将结果统一到一个周期内，利于求平均。
        /// </summary>
        PeriodPipeFilterManager PeriodFilterManager { get; set; } 
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// 计算产品的存储、调用、输出
        /// </summary>
        public DifferFcbManager DifferFcbManager { get; set; }
        /// <summary>
        /// 多站的历元处理器
        /// </summary>
        public MultiSiteMwWideLaneSolver MwWideLaneSolvers { get; set; }
        
        /// <summary>
        /// 基准卫星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 表输出
        /// </summary>
        public ObjectTableManager TableManager { get; set; }  
        /// <summary>
        /// 当前观测数据
        /// </summary>
        public MultiSiteEpochInfo CurrentMaterial { get; set; }
        #endregion
         
        /// <summary>
        /// 遍历每一个历元
        /// </summary>
        /// <param name="multiEpochInfo"></param>
        /// <returns></returns>
        public override bool Revise(ref MultiSiteEpochInfo multiEpochInfo)
        {
            if (multiEpochInfo == null) { return false; }

            this.CurrentMaterial = multiEpochInfo;
             
            //挨个计算
            foreach (var epochInfo in multiEpochInfo)
            {
                var info = epochInfo;
                var epochRolver = MwWideLaneSolvers.GetOrCreate(epochInfo.Name);
                if (!epochRolver.IsAvailable(info)) { continue; }

                epochRolver.Buffers = GetEpochBuffers(epochInfo.Name);
                epochRolver.Revise(ref info);
            }

            //实时输出，每一个卫星差分对应一个文件，如C02-C01   
            if (IsOutputDetail)
            {
                AddCurrentResultToOutputTable(); 
            }

            //输出产品
            if (IsTimeToOutputProduct())
            {
                //应该先计算平均值
                AddCurrentProductToFcbManager(CurrentMaterial.ReceiverTime);
                this.LastProductTime = CurrentMaterial.ReceiverTime;
            }          

            return true;
        }
        /// <summary>
        /// 构建指定测站的缓存
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public WindowData<EpochInformation> GetEpochBuffers(string name)
        {
            List<EpochInformation> list = new List<EpochInformation>();
            foreach (var item in this.Buffers)
            {
                var epoch = item.Get(name);
                if (epoch == null) { continue; }
                list.Add(epoch);
            }
            return new WindowData<EpochInformation>(list);
        }

        #region  结果生成与输出
        #region Wm宽项星间单差逐历元详细输出
        /// <summary>
        /// 综合输出实时网解结果。同一颗卫星的差分值，网解输出。
        /// 如 G02-G01，则内容是所有站的该数值列表，此处只输出平滑后的MW差分值。
        /// </summary>
        /// <param name="CurrentMultiEpochInfo"></param> 
        private void AddCurrentResultToOutputTable()
        {
            //考虑所有的卫星
            foreach (var prn in CurrentMaterial.TotalPrns)
            {
                if (prn == BasePrn) { continue; } //忽略基础卫星
                AddRowToOutputTable(prn);
            }
        }

        /// <summary>
        /// 添加这颗卫星的差分值作为一行到输出表格
        /// </summary> 
        /// <param name="prn"></param>
        /// <returns></returns>
        private void AddRowToOutputTable(SatelliteNumber prn)
        {
            int epochIndex = CurrentMaterial.EpochIndexOfDay;
            var differkey = BuildDifferKey(prn) + "_WideLane";
            var differTable = TableManager.GetOrCreate(differkey);
            differTable.NewRow();
            differTable.AddItem("Epoch", CurrentMaterial.ReceiverTime.ToShortTimeString());

            //遍历当前历元的每一个测站，添加到输出表格，一个测站一列
            foreach (var epoch in CurrentMaterial)
            {
                var epochRolver = MwWideLaneSolvers[epoch.Name];
                if (epochRolver.LatestEpochSatWmValues.Contains(prn))
                {
                    var differValue = epochRolver.LatestEpochSatWmValues[prn];//获取最新的解算
                    //只考虑本历元
                    if (differValue.Index == epochIndex) { differTable.AddItem(epoch.Name, differValue.DifferValue); }
                }
            }
        }
        #endregion

        #region 产品输出
        /// <summary>
        /// 生成产品，此处直接取平均值。？？应该剔除粗差！2016.10.19， czs
        /// </summary>
        /// <param name="ReceiverTime"></param>
        private void AddCurrentProductToFcbManager(Time ReceiverTime)
        {  
            var tableValues = BuildLatestResultTable();
            var aveDic = tableValues.GetAverages();

            //输出实时结果文件,多个测站的平滑值
            foreach (var val in aveDic)
            {
                var key = BuildDifferKey(val.Key);
                 
                DifferFcbManager.Add(new DifferFcbOfSatDcbItem(key, ReceiverTime, val.Value));
            }
        }
        /// <summary>
        /// 是否是产品输出时间。
        /// </summary>
        /// <returns></returns>
        private bool IsTimeToOutputProduct()
        {
            if (CurrentMaterial == null) return true;

            return  (this.CurrentMaterial.ReceiverTime - LastProductTime >= OutputProductInterval) ;
        }
        #endregion

        /// <summary>
        /// 写入文件
        /// </summary>
        public void WriteToFiles()
        {
            log.Info("写入结果到文件！");
            //写最后的结果
            var table = BuildLatestResultTable();
            TableManager.ReductValuesTo();
            if (table.RowCount > 1)
            {
                table.GetAveragesWithStdDevAndAppendToTable();
                this.TableManager.AddTable(this.BasePrn + "_LastWideLaneSatDiffer", table); 
            }
            TableManager.WriteAllToFileAndCloseStream();
            
            foreach (var item in MwWideLaneSolvers)
            {
                item.WriteToFile();                
            }
            if (CurrentMaterial != null)
            {
                AddCurrentProductToFcbManager(this.CurrentMaterial.ReceiverTime);
            }
            var path = Path.Combine(OutputDirectory, "DcbProducts.sdfcb.xls");
            DifferFcbManager.WriteToFile(path);
        } 

        /// <summary>
        /// 构建汇总文档写最后的结果.可以作为当日产品。
        /// 把所有的测站计算的最后结果，写入一个表格。
        /// 列为每一颗卫星的差分标题，如 G02-G01。
        /// 行为各颗卫星的数值。
        /// 本表可以作为计算平均数用。
        /// </summary>
        public ObjectTableStorage BuildLatestResultTable()
        { 
            var table = new ObjectTableStorage();
             
            foreach (var oneSiteSolver in MwWideLaneSolvers)
            {
                table.NewRow();
                table.AddItem("Name", oneSiteSolver.Name);// 第一列为名称

                var latestValues = oneSiteSolver.LatestEpochSatWmValues;
                foreach (var item in latestValues.Keys) //每个测站对应一排
                {
                    var key = BuildDifferKey(item);
                    var initVal = latestValues[item].DifferValue;
                    //结果统一到一个周期区间
                    var revisedVal = PeriodFilterManager.GetOrCreate(key).Filter(initVal);
                    table.AddItem(key, revisedVal); 
                }
                table.EndRow();
            }

            return table;
        }

        
        /// <summary>
        /// 构建差分key
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        private string BuildDifferKey(Object sat) { return sat + "-" + BasePrn; }
        #endregion
    } 
}