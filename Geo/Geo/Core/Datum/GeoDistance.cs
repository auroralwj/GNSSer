//2015.04.16, czs, edit in namu, 采用 GeodeticX 计算，精简原 Bessl

using System;
using System.Collections.Generic;
using System.Text;
 

namespace Geo.Coordinates
{
    /// <summary>
    /// 采用贝塞尔大地算法计算大地线长
    /// </summary>
    public class GeoDistance
    {
        /// <summary>
        /// 采用贝塞尔大地算法计算大地线长。
        /// </summary>
        /// <param name="coord1"></param>
        /// <param name="coord2"></param>
        /// <returns></returns>
        public static double GetDistanceInMeter(XYZ coord1, XYZ coord2)
        {
            return GetDistanceInMeter(new LonLat(coord1.X, coord1.Y), new LonLat(coord2.X, coord2.Y));        
        }

        /// <summary>
        /// 采用贝塞尔大地算法计算大地线长。
        /// </summary>
        /// <param name="lonLat1"></param>
        /// <param name="lonLat2"></param>
        /// <returns></returns>
        public static double GetDistanceInMeter(LonLat lonLat1, LonLat lonLat2)
        {
            double distance = GeodeticX.Geodetic.Bessel_BL_S(lonLat1.Lat, lonLat1.Lon, lonLat2.Lat, lonLat2.Lon, 6378137, 298.257223563);             
            return distance;
        }
    }
}
