using System;
using System.Collections.Generic;

using System.Text;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Data.Rinex
{    
    /// <summary>
    /// 依据卫星轨道参数获取卫星位置。
    /// </summary>
    public static class SatOrbitCaculator
    {
        /// <summary>
        ///  依据卫星轨道参数获取卫星位置。
        /// </summary>
        /// <param name="record"></param>
        /// <param name="gpstime"></param>
        /// <returns></returns>
        public static  XYZ GetSatPos(EphemerisParam record, Time gpstime)
        {   
          return  OrbitUtil.GetSatXyz(record,gpstime.SecondsOfWeek); 
        }

       
    }
}
