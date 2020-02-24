//2015.01.10, czs, create in namu shangliao, 卫星可见性提取处理器


using System;
using System.Text;
using System.IO;
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
using Geo.Utils;
using Geo.Common;
using Gnsser.Checkers;  

namespace Gnsser
{
    /// <summary>
    /// 卫星周跳提取处理器，提取到 SatPeriodInfoManager 中。
    /// </summary>
    public class BaseLineSatCycleSlipAnalyst : TwinsReviser<EpochInformation>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="SatelliteTypes">卫星类型</param>
        /// <param name="interval">采样间隔</param>
        public BaseLineSatCycleSlipAnalyst(DataSourceContext DataSourceContext, GnssProcessOption SatelliteTypes, double interval = 30.0)
        {
            this.SatPeriodMarker = new SatPeriodInfoManager(interval);
            this.SatelliteTypes = SatelliteTypes.SatelliteTypes;
            this.RefObsBuilder = new EpochInfoReviseManager(DataSourceContext, SatelliteTypes);
            this.RovObsBuilder = new EpochInfoReviseManager(DataSourceContext, SatelliteTypes);
            this.DifferPositionOption = SatelliteTypes;
            //RefObsBuilder.CaculatingProcessor.AddProcessor(CycleSlipDetectorChainProcessor.Default());
            //RovObsBuilder.CaculatingProcessor.AddProcessor(CycleSlipDetectorChainProcessor.Default());



            
            //RefObsBuilder.CaculatingProcessor.AddProcessor(new ReverseCycleSlipeReviser());
            //RovObsBuilder.CaculatingProcessor.AddProcessor(new ReverseCycleSlipeReviser());
        }

        /// <summary>
        /// 参考站历元信息构建器
        /// </summary>
        public EpochInfoReviseManager RefObsBuilder { get; set; }
        /// <summary>
        /// 流动站历元信息构建器
        /// </summary>
        public EpochInfoReviseManager RovObsBuilder { get; set; }
        /// <summary>
        /// 提取结果
        /// </summary>
        public SatPeriodInfoManager SatPeriodMarker { get; set; }

        /// <summary>
        /// 卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }

        public GnssProcessOption DifferPositionOption { get; set; }

        /// <summary>
        /// 处理过程
        /// </summary>
        /// <param name="obsA">观测数据 A</param>
        /// <param name="obsB">观测数据 B</param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation epochInfoA , ref EpochInformation epochInfoB )
        {
              RefObsBuilder.Revise(ref epochInfoA);
              RefObsBuilder.Revise(ref epochInfoA);

            if (epochInfoA == null || epochInfoB == null) return false;

            foreach (var sat in epochInfoA)
            {
                if (epochInfoB.Contains(sat.Prn))
                {
                    if (sat.IsUnstable || epochInfoB[sat.Prn].IsUnstable)
                    {
                        SatPeriodMarker.AddTimePeriod(sat.Prn, sat.Time.Value);
                    }
                }
            }
            return true;
        }
    }
}