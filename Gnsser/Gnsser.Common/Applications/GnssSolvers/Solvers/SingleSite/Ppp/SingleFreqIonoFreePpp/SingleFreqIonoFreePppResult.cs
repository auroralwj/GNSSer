//2017.09.14, czs, create in hongqing, 单频消电离层组合


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Gnsser.Domain; 
using Geo.Algorithm.Adjust;
using Geo;


namespace Gnsser.Service
{
    /// <summary>
    ///  单频消电离层组合
    /// </summary>
    public class SingleFreqIonoFreePppResult :SingleSiteGnssResult
    {
        /// <summary>
        /// 单频消电离层组合 构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="positioner"> </param>
        public SingleFreqIonoFreePppResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment, GnssParamNameBuilder positioner
            )
            : base(receiverInfo, Adjustment, positioner)
        {
                
        } 
    }
}
