//2018.11.02, czs, create in HMX, 双差网解定位
//2018.11.07, czs, edit in hmx, 递归最小二乘双差网解定位


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
    /// 双差网解定位。具有模糊度。。
    /// </summary>
    public class RecursiveNetDoubleDifferPositionResult : MultiSiteGnssResult//, IFixableParamResult
    {

        /// <summary>
        /// 双差网解定位。具有模糊度。
        /// </summary>
        /// <param name="mInfo">历元观测信息</param>
        /// <param name="Adjustment"></param>
        /// <param name="positioner"></param>
        /// <param name="baseSatPrn"></param>
        /// <param name="baseParamCount"></param>
        public RecursiveNetDoubleDifferPositionResult(
            MultiSiteEpochInfo mInfo,
            AdjustResultMatrix Adjustment,
             GnssParamNameBuilder positioner,
            SatelliteNumber baseSatPrn,
            int baseParamCount = 5
            )
            : base(mInfo, Adjustment, positioner)
        {

        }
        public WeightedVector FixedParams { get; set; }

        public WeightedVector GetFixableVectorInUnit()
        {
            this.ResultMatrix.Estimated.ParamNames = this.ResultMatrix.ParamNames;

            var ambiParamNames = new List<string>();
            foreach (var name in ResultMatrix.ParamNames)
            {
                //if (HasUnstablePrn(name)) { continue; }

                if (name.Contains(Gnsser.ParamNames.DoubleDifferAmbiguity))
                { ambiParamNames.Add(name); }
            }
            var estVector = this.ResultMatrix.Estimated.GetWeightedVector(ambiParamNames);

            return estVector;
        }
    }
}
