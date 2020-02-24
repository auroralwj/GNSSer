//2015.04.16, czs, edit in namu, ���� GeodeticX ���㣬����ԭ Bessl

using System;
using System.Collections.Generic;
using System.Text;
 

namespace Geo.Coordinates
{
    /// <summary>
    /// ���ñ���������㷨�������߳�
    /// </summary>
    public class GeoDistance
    {
        /// <summary>
        /// ���ñ���������㷨�������߳���
        /// </summary>
        /// <param name="coord1"></param>
        /// <param name="coord2"></param>
        /// <returns></returns>
        public static double GetDistanceInMeter(XYZ coord1, XYZ coord2)
        {
            return GetDistanceInMeter(new LonLat(coord1.X, coord1.Y), new LonLat(coord2.X, coord2.Y));        
        }

        /// <summary>
        /// ���ñ���������㷨�������߳���
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
