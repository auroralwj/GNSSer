//2016.08.09, czs, create in fujianyongan, WM星间差分FCB计算器
//2016.08.19, czs, 安徽 黄山 屯溪, 宽项调通，重构
//2016.10.17.13.09, czs, 宽项计算，直接处产品
//2016.10.20, czs, edit in hongqing, 窄巷计算
//2016.10.19, czs, 宽项计算，更名为 MultiSiteMwWideLaneSolver
//2016.10.21, czs, 在宽项的基础上，修改为窄巷计算器

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
    /// 多站窄巷项计算器
    /// </summary>
    public class MultiSiteNarrowLaneSolver : BaseDictionary<string, EpochNarrowLaneSolver>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn"></param>
        /// <param name="SetWeightWithSat"></param>
        /// <param name="maxError"></param>
        /// <param name="IsOutputDetails"></param>
        public MultiSiteNarrowLaneSolver(SatelliteNumber BasePrn,DifferFcbManager DifferFcbManager, string directory, int skipCount,  bool SetWeightWithSat = false, double maxError = 2, bool IsOutputDetails = true)
        {
            this.BasePrn = BasePrn;
            this.MaxError = maxError;
            this.IsOutputDetails = IsOutputDetails;
            this.IsSetWeightWithSat = SetWeightWithSat;
            this.DifferFcbManager = DifferFcbManager;
            this.TableManager = new ObjectTableManager(directory);
            this.SkipCount = skipCount;
        }

        #region 属性
        public int SkipCount { get; set; }
        /// <summary>
        /// 负责数据汇总输出
        /// </summary>
        ObjectTableManager TableManager { get; set; }
        /// <summary>
        /// 已解出的宽项值
        /// </summary>
        DifferFcbManager DifferFcbManager { get; set; } 
        /// <summary>
        /// 是否输出详细计算信息
        /// </summary>
        public bool IsOutputDetails { get; set; }
        /// <summary>
        /// 基准卫星
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 按照卫星（高度角和距离）定权。否为等权处理。
        /// </summary>
        public bool IsSetWeightWithSat { get; set; }
        /// <summary>
        /// 最大误差
        /// </summary>
        public double MaxError { get; set; }
        #endregion

        #region  方法
        public override EpochNarrowLaneSolver Create(string key)
        {
            return new EpochNarrowLaneSolver(BasePrn, key, SkipCount, IsSetWeightWithSat, MaxError, IsOutputDetails);
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="results"></param>
        /// <param name="mEpochInfo"></param>
        /// <param name="mSiteBuffers"></param>
        public void Solve(
            Dictionary<string, PppResult> results,
            IWindowData<Dictionary<string, PppResult>> resultBuffer,
            MultiSiteEpochInfo mEpochInfo,
            IWindowData<MultiSiteEpochInfo> mSiteBuffers)
        {
            this.CurrentMaterial = mEpochInfo;

            foreach (var item in mEpochInfo)
            {
                var key = item.Name;
                var info = item;
                var buffer = GetEpochInfoBuffer(key, mSiteBuffers);//单站缓存
                var reviser = GetOrCreate(item.Name);
                reviser.DifferFcbManager = DifferFcbManager;
                reviser.Buffers = buffer;
                reviser.PppResult = results[key];
                reviser.PppResultBuffers = GetEpochResultBuffer(key, resultBuffer);//结果缓存
                reviser.Revise(ref info);
            }

            AddCurrentResultToOutputTable();
        }

        MultiSiteEpochInfo CurrentMaterial;

        #region  星间单差窄巷逐历元详细输出
        /// <summary>
        /// 综合输出实时网解结果。同一颗卫星的差分值，网解输出。
        /// 如 G02-G01，则内容是所有站的该数值列表，此处只输出平滑后的MW差分值。
        /// </summary>
        /// <param name="CurrentMultiEpochInfo"></param> 
        private void AddCurrentResultToOutputTable()
        {
            //考虑所有的卫星
            foreach (var prn in CurrentMaterial.EnabledPrns)
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
            var differkey = BuildDifferKey(prn) + "_NarrowLane";
            var differTable = TableManager.GetOrCreate(differkey);
            differTable.NewRow();
            differTable.AddItem("Epoch", CurrentMaterial.ReceiverTime.ToShortTimeString());

            //遍历当前历元的每一个测站，添加到输出表格，一个测站一列
            foreach (var epoch in CurrentMaterial)
            {
                var epochRolver = this[epoch.Name];
                if (epochRolver.LatestEpochNarrowLaneValues.Contains(prn))
                {
                    var differValue = epochRolver.LatestEpochNarrowLaneValues[prn];//获取最新的解算
                    //只考虑本历元
                    if (differValue.Index == epochIndex) { differTable.AddItem(epoch.Name, differValue.SmoothValue); }
                }
            }
        }
        #endregion
        #region 获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mSiteBuffers"></param>
        /// <returns></returns>
        public IWindowData<PppResult> GetEpochResultBuffer(string key, IWindowData<Dictionary<string, PppResult>> mSiteBuffers)
        {
            WindowData<PppResult> buffer = new WindowData<PppResult>(mSiteBuffers.Count);
            foreach (var item in mSiteBuffers)
            {
                if (!item.ContainsKey(key)) { continue; }

                var epochInfo = item[key];
                if (epochInfo == null) { continue; }
                buffer.Add(epochInfo);
            }
            return buffer;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mSiteBuffers"></param>
        /// <returns></returns>
        public IWindowData<EpochInformation> GetEpochInfoBuffer(string key, IWindowData<MultiSiteEpochInfo> mSiteBuffers)
        {
            WindowData<EpochInformation> buffer = new WindowData<EpochInformation>(mSiteBuffers.Count);
            foreach (var item in mSiteBuffers)
            {
                var epochInfo = item.Get(key);
                if (epochInfo == null) { continue; }
                buffer.Add(epochInfo);
            }
            return buffer;
        }
        #endregion

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
            var PeriodFilterManager = new PeriodPipeFilterManager(1, 0.5);
            foreach (var oneSiteSolver in this)
            {
                table.NewRow();
                table.AddItem("Name", oneSiteSolver.Name);// 第一列为名称

                var latestValues = oneSiteSolver.LatestEpochNarrowLaneValues;
                foreach (var item in latestValues.Keys) //每个测站对应一排
                {
                    var key = BuildDifferKey(item);
                    var initVal = latestValues[item].SmoothValue;
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


        /// <summary>
        /// 全部写入文件
        /// </summary>
        public void WriteAllToFiles()
        {
            log.Info("写入结果到文件！");
            foreach (var item in this)
            {
                item.WriteToFile();
            }
         
            //写最后的结果
            var table = BuildLatestResultTable();

            if (table.RowCount > 1)
            {
                table.ReductValuesTo();
                table.GetAveragesWithStdDevAndAppendToTable();
                this.TableManager.AddTable(this.BasePrn + "_LastNarrowLaneOfSatDcbDiffer", table);
            }
            TableManager.WriteAllToFileAndCloseStream();

        }
        #endregion
    }
}