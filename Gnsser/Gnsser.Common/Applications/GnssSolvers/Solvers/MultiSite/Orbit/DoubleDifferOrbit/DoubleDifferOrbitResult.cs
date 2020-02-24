//2018.10.28, czs, create in HMX, 无电离层组合双差定轨


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
    /// 无电离层组合双差定轨。具有模糊度。。
    /// </summary>
    public class DoubleDifferOrbitResult : MultiSiteGnssResult, IFixableParamResult
    {

        /// <summary>
        /// 双差差分定位结果。具有模糊度。
        /// </summary>
        /// <param name="mInfo">历元观测信息</param>
        /// <param name="Adjustment"></param>
        /// <param name="positioner"></param>
        /// <param name="baseSatPrn"></param>
        /// <param name="baseParamCount"></param>
        public DoubleDifferOrbitResult(
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
