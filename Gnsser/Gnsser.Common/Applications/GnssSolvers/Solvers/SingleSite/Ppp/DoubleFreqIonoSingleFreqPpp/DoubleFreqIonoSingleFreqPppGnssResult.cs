//2018.07.05, czs, create in HMX, 双频电离层延迟改正，单频PPP


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
    ///  双频电离层延迟改正，单频PPP
    /// </summary>
    public class DoubleFreqIonoSingleFreqPppGnssResult : SingleSiteGnssResult
    {
        /// <summary>
        /// 精 通用单站单频计算，高度可配置构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="positioner">定位器</param>
        public DoubleFreqIonoSingleFreqPppGnssResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment, GnssParamNameBuilder positioner
            )
            : base(receiverInfo, Adjustment, positioner)
        {
                
        } 
    }
}
