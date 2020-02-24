//2018.10.27, czs, create in hmx, 简易伪距轨道确定


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
    /// 简易伪距轨道确定参数命名器
    /// </summary>
    public class RangeOrbitParamNameBuilder : GnssParamNameBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RangeOrbitParamNameBuilder( GnssProcessOption option)  :base(option){ }
         
        /// <summary>
        /// 对象
        /// </summary>
        public MultiSiteEpochInfo MultiSiteEpochInfo { get => (MultiSiteEpochInfo)Material; } 
        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override List<string> Build()
        {
            List<string> paramNames = new List<string>();
            var mInfo = MultiSiteEpochInfo;
            //卫星坐标
            foreach (var prn in this.EnabledPrns)
            {
                paramNames.AddRange(GetSatDxyz(prn));
            }
            //卫星钟差
            foreach (var prn in this.EnabledPrns)
            {
                paramNames.Add(this.GetSatClockParamName(prn));
            }
            //接收机钟差
            foreach (var site in mInfo)
            {
                paramNames.Add(GetReceiverClockParamName(site.SiteName));
            }
            return paramNames;
        } 

        public override  string GetParamName(SatelliteNumber sat)
        {
            return GetSatClockParamName(sat);
        }

    }
}