//2014.10.03, czs, create, 静态数学方法

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;

namespace Geo.Utils
{
    /// <summary>
    /// 提供静态数学方法，是系统 Math 的补充。
    /// </summary>
   public static class GeoMath
    {

        const double Pi =  3.1415926535897932384626433832795;
        const double TwoPi = 2 * 3.1415926535897932384626433832795;
       /// <summary>
       /// 求平方
       /// </summary>
       /// <param name="x"></param>
       /// <returns></returns>
        public static double Sqr(double x) { return (x * x); }
        /// <summary>
        /// 将角度归算到 0 - 2 PI 之间。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static double ModTwoPI(double arg)
        {
            double modu = (arg % TwoPi);

            if (modu < 0.0) modu += TwoPi;

            return modu;
        }
        /// <summary>
        /// 本函数优点是能够正确返回象限。
        ///  ArcTangent of sin(x) / cos(x). The advantage of this function over arctan()
        /// is that it returns the correct quadrant of the angle.
        /// </summary>
        /// <param name="sinx"></param>
        /// <param name="cosx"></param>
        /// <returns></returns>
        public static double AcTan(double sinx, double cosx)
        {
            double ret; 

            if (cosx == 0.0)
            {
                if (sinx > 0.0) ret = Pi / 2.0;
                else ret = 3.0 * Pi / 2.0;
            }
            else
            {
                if (cosx > 0.0) ret = Math.Atan(sinx / cosx);
                else ret = Pi + Math.Atan(sinx / cosx);
            }
            return ret;
        }
         

    }
}
