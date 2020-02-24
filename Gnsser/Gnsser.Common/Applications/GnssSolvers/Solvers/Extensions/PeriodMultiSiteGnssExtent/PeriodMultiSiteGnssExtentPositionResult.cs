//2017.08.28, czs & kyc, create in hongqing, 多站多历元GNSS计算预留测试类

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Algorithm;

namespace Gnsser.Service
{
    /// <summary>
    /// 双差差分定位结果。具有模糊度。。
    /// </summary>
    public class PeriodMultiSiteGnssExtentPositionResult : TwoSitePeriodDifferPositionResult
    {
        /// <summary>
        /// 双差差分定位结果。具有模糊度。
        /// </summary>
        /// <param name="Adjustment"></param>
        /// <param name="BasePrn"></param>
        /// <param name="DifferPositionOption"></param>
        /// <param name="PeriodDifferInfo"></param>
        /// <param name="PositionResult"></param>
        public PeriodMultiSiteGnssExtentPositionResult(
            MultiSitePeriodInfo PeriodDifferInfo,
            AdjustResultMatrix Adjustment,
            SatelliteNumber BasePrn,
            GnssProcessOption DifferPositionOption, GnssParamNameBuilder nameBuilder)
            : base(PeriodDifferInfo, Adjustment, BasePrn, nameBuilder)
        {
            this.DifferPositionOption = DifferPositionOption;
        }
        GnssProcessOption DifferPositionOption { get; set; }

     
        /// <summary>
        /// 固定后的模糊度,单位周，值应该为整数。
        /// </summary>
        public Vector FixedIntAmbiguities { get; set; }

    }
}