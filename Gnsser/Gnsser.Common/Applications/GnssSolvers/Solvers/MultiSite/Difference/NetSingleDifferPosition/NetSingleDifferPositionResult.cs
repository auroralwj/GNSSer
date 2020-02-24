//2018.11.05, czs, create in HMX, 单差网解定位

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;


namespace Gnsser.Service
{
    /// <summary>
    /// 单差网解定位。具有模糊度。。
    /// </summary>
    public class NetSingleDifferPositionResult : MultiSiteGnssResult
    {

        /// <summary>
        /// 双差网解定位。具有模糊度。
        /// </summary>
        /// <param name="mInfo">历元观测信息</param>
        /// <param name="Adjustment"></param>
        /// <param name="positioner"></param>
        /// <param name="baseSatPrn"></param>
        /// <param name="baseParamCount"></param>
        public NetSingleDifferPositionResult(
            MultiSiteEpochInfo mInfo,
            AdjustResultMatrix Adjustment,
             GnssParamNameBuilder positioner,
            SatelliteNumber baseSatPrn,
            int baseParamCount = 5
            )
            : base(mInfo, Adjustment, positioner)
        {

        }
    }
}
