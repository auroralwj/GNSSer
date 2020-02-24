//2017.09.13, czs, create in hongqing,  历元卫星组合值

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Service;
using Gnsser.Correction;
using Gnsser.Data.Rinex;
using Geo;
using Geo.Correction;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Times;

namespace Gnsser.Domain
{
    /// <summary>
    /// 历元卫星组合值
    /// </summary>
    public class EpochSatCombination
    {
        /// <summary>
        /// 历元卫星组合值
        /// </summary>
        /// <param name="EpochSat">历元卫星</param>
        public EpochSatCombination(EpochSatellite EpochSat)
        {
            this.EpochSat = EpochSat;
        }

        /// <summary>
        /// 历元卫星观测原始值
        /// </summary>
        public EpochSatellite EpochSat { get; set; }







    }
}