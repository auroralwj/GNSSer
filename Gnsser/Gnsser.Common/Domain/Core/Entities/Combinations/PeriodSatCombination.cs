//2017.09.13, czs, create in hongqing,  多历元卫星观测原始值

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
    public class PeriodSatCombination
    {
        /// <summary>
        /// 多历元卫星观测原始值
        /// </summary>
        /// <param name="EpochSat">多历元卫星观测原始值</param>
        public PeriodSatCombination(IBuffer<EpochSatellite> EpochSat)
        {
            this.EpochSats = EpochSats;
        }

        /// <summary>
        /// 多历元卫星观测原始值
        /// </summary>
        public IBuffer<EpochSatellite> EpochSats { get; set; }







    }
}