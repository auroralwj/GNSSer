//2017.09.09, czs, create in hongqing, 通用单站单频计算，高度可配置


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
    ///  通用单站单频计算，高度可配置结果。 
    /// </summary>
    public class CommonSingeFreqGnssResult :SingleSiteGnssResult
    {
        /// <summary>
        /// 精 通用单站单频计算，高度可配置构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="paramNames">参数名称</param>
        public CommonSingeFreqGnssResult(
            EpochInformation receiverInfo,
            AdjustResultMatrix Adjustment, GnssParamNameBuilder positioner
            )
            : base(receiverInfo, Adjustment, positioner)
        {
                
        } 
    }
}
