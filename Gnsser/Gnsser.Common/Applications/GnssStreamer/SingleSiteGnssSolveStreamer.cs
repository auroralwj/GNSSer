//2016.08.20, czs, create in 福建永安, 宽项计算器，多站观测数据遍历器
//2016.08.29, czs, edit in 西安洪庆, 重构多站观测数据遍历器
//2017.05.11, czs, edit in hongqing, 增加ENU的输出
//2017.09.06, czs, edit in honqing, 更名为 SingleSiteGnssSolveStreamer
//2018.07.27, czs, edit in HMX, 单站与多种进行多函数合并

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;


namespace Gnsser
{
    /// <summary>
    /// 单站计算数据流解算器
    /// </summary>
    public class SingleSiteGnssSolveStreamer : SingleSiteObsAdjustStreamer, IFileGnssSolver
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleSiteGnssSolveStreamer(string outDirectory = null)
            : base(outDirectory ?? Gnsser.Setting.TempDirectory)
        {
        }
        #region 属性
        IGnssSolver IFileGnssSolver.Solver { get { return Solver; } }
        /// <summary>
        /// 是否具有表格数据
        /// </summary>
        public bool HasTableData
        {
            get { return (this.TableTextManager != null && this.TableTextManager.Count != 0); }
        }
        /// <summary>
        /// 单站多历元
        /// </summary>
        PeriodInformationBuilder PeriodInformationBuilder { get; set; }
        /// <summary>
        /// 单站单星多历元
        /// </summary>
        PeriodSatelliteBuilder PeriodSatelliteBuilder { get; set; }
        /// <summary>
        /// 用于单星计算
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            this.GnssResultBuilder = new GnssResultBuilder(this.TableTextManager, this.AioAdjustFileBuilder, AdjustEquationFileBuilder, Option, Context);
            this.PeriodInformationBuilder = new PeriodInformationBuilder(true,this.Option.MultiEpochCount);
            this.PeriodSatelliteBuilder = new PeriodSatelliteBuilder(this.Option.MultiEpochCount, this.Option.IsSmoothMoveInMultiEpoches);
            this.Solver = BuildRolver(Context, Option);
        }
        public override void Complete()
        {
            base.Complete();
            GnssResultBuilder.Complete();
        }
        /// <summary>
        /// 处理一个历元
        /// </summary>
        /// <param name="epoch"></param>
        public override SimpleGnssResult Produce(EpochInformation epoch)
        {
            if (!this.EphemerisDataSource.TimePeriod.Contains(epoch.ReceiverTime)) { this.IsCancel = true; log.Warn("星历结束，结束计算！" + this.EphemerisDataSource.TimePeriod); }
            if (epoch.Count == 0) { log.Error(epoch.ToShortString() + ", 没有可用卫星，是否系统选错了？"); return null; }

            ISiteSatObsInfo material = null;
            SimpleGnssResult result = null;

            if (Solver is SingleSiteGnssSolver)
            {
                material = epoch;
                result = ((SingleSiteGnssSolver)Solver).Get(epoch);
            }
            else if (Solver is SingleSitePeriodSolver) //单站多历元
            {

                PeriodInformationBuilder.Add(epoch);
                var period = PeriodInformationBuilder.Build();
                if (period == null || !period.Enabled) { return null; }
                material = period;
                result = ((SingleSitePeriodSolver)Solver).Get(period);

                PeriodInformationBuilder.Data.Clear();
            }
            else if (Solver is CommonSingeSatGnssSolver || Solver is CommonSingePeriodSatGnssSolver) //单站多历元
            {
                material = epoch;

                if (!epoch.Contains(this.Prn))
                {//如果星历不存在则重新选星
                    if (this.Option.IsIndicatedPrn)
                    {
                        this.Prn = this.Option.IndicatedPrn;
                    }
                    else
                    {
                        //this.Prn = epoch.FirstKey;
                        this.Prn = epoch.GetMaxElevationPrn();
                        log.Info(this.Name + ", 最大高度角选星：" + this.Prn);
                    }
                }

                var sat = epoch.Get(this.Prn);
                if (sat == null)
                {
                    return null;
                }
                if (Solver is CommonSingeSatGnssSolver)
                {
                    result = ((CommonSingeSatGnssSolver)Solver).Get(sat);
                }
                else if (Solver is CommonSingePeriodSatGnssSolver) //单站多历元
                {
                    if (PeriodSatelliteBuilder.Add(sat))
                    {
                        var period = PeriodSatelliteBuilder.Build();
                        if (period == null || !period.Enabled) { return null; }
                        //material = period;
                        result = ((CommonSingePeriodSatGnssSolver)Solver).Get(period);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}