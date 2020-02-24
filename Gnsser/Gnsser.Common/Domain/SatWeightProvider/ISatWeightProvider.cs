// 2014.09.12, czs, create, 卫星定权。 
//2018.09.27, czs, edit in HMX， 相位和伪距单独获取
 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 卫星精度方差提供器
    /// </summary>
    public interface ISatWeightProvider
    { 
        /// <summary>
        /// 返回伪距权逆阵，或方差
        /// </summary>
        /// <param name="satellite"></param>
        /// <returns></returns>
        double  GetInverseWeightOfRange(EpochSatellite satellite);

    }
}