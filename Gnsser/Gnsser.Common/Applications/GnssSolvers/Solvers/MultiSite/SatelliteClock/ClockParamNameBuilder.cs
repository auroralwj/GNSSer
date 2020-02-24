//2016.03.11, czs, create in hongqing, PPP参数命名器

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

namespace Gnsser.Service
{
    /// <summary>
    /// 双差参数命名器
    /// </summary>
    public class ClockParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClockParamNameBuilder( GnssProcessOption option)  :base(option){ }
         
        /// <summary>
        /// 列表
        /// </summary>
        public MultiSiteEpochInfo EpochInfos { get; set; }
        /// <summary>
        /// 卫星
        /// </summary>
        public List<SatelliteNumber> SatelliteNumbers { get; set; }
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>();
            foreach (var epoch in EpochInfos)
            {
                paramNames.Add(GetReceiverClockParamName(epoch));
            }
            foreach (var sat in SatelliteNumbers)
            {
                paramNames.Add(GetParamName(sat));
            }
            foreach (var epoch in EpochInfos)
            {
                paramNames.Add(GetSiteWetTropZpdName(epoch));
            }
            foreach (var epoch in EpochInfos)
            {
                foreach (var s in epoch.EnabledSats)
                {
                    paramNames.Add(GetParamName(s));
                }
            }
            return paramNames;
        }

        public override  string GetParamName(SatelliteNumber sat)
        {
            return GetSatClockParamName(sat);
        }

        public virtual  string GetParamName(EpochSatellite s)
        {
            return GetSiteSatAmbiguityParamName(s);
        }

    }
}