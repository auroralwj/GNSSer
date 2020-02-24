using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// 角度转换
    /// </summary>
    public static class AngularTransformer
    {

        public const double DegToRadMultiplier = 0.017453292519943295769236907684886;
        public const double RadToDegMultiplier = 57.295779513082320876798154814105;

        /// <summary>
        /// 度转换为弧度
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static double DegToRad(double deg)
        {
            return deg * DegToRadMultiplier;
        }

        /// <summary>
        /// 弧度转换为度.
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static double RadToDeg(double rad)
        {
            return rad * RadToDegMultiplier;
        }

        /// <summary>
        /// 度分秒转换为度。1093000表示109°30′00″
        /// </summary>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double Dms_sToDeg(double dms_s)
        {
            double deg = (int)(dms_s / 10000);
            double min = (int)((dms_s % 10000) / 100);//120.3030
            double sec = dms_s % 100;
            double lonDeg = deg + min / 60.0 + sec / 3600.0;
            return lonDeg;
        }
        /// <summary>
        /// 度分秒转换为度。
        /// </summary>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double D_msToDeg(double d_ms)
        {
            double deg = (int)(d_ms);
            double min = (int)((d_ms * 100) % 100);//120.3030
            double sec = (d_ms * 10000) % 100;
            double lonDeg = deg + min / 60.0 + sec / 3600.0;
            return lonDeg;
        }

    }
}
