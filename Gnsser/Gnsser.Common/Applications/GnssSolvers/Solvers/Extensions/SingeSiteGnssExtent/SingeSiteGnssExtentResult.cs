//2017.08.28, czs & kyc, create in hongqing, 单站单历元GNSS计算预留测试类

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Gnsser.Domain;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 单站单历元GNSS计算预留测试类
    /// </summary>
    public class SingeSiteGnssExtentResult : SingleSiteGnssResult// PhasePositionResult
    {
        /// <summary>
        /// 单站单历元GNSS计算预留测试类
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="positioner">定位器</param>
        /// <param name="baseParamCount">基础参数数量</param>
        public SingeSiteGnssExtentResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment,
            GnssParamNameBuilder positioner,
            int baseParamCount = 5
            )
            : base(receiverInfo, Adjustment,  positioner)
        { 

        } 

    }
}