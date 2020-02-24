//2014.11.07, czs, create in numu, 分离XY静态算法部分。

using System;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// 3维平面坐标接口，以X、Y、Z分量表示。
    /// </summary>
    public class XyzUtil
    {
        /// <summary>
        /// 获取相交点、穿刺点，如电离层
        /// </summary>
        /// <param name="siteXyz"></param>
        /// <param name="satXyz"></param>
        /// <param name="geoHeight">距离地球平均表面的距离 m, 默认 450 000 m</param>
        /// <returns></returns>
        public static XYZ GetIntersectionXyz(XYZ siteXyz, XYZ satXyz, double geoHeight = 450000)
        {
            return new XYZ(MathUtil.GetPuncturePoint(
               siteXyz.ToArray(),
                satXyz.ToArray(),
                 Geo.Referencing.Ellipsoid.MeanRaduis + geoHeight
            ));
        }


        /// <summary>
        /// 两条线段的交点。
        /// </summary>
        /// <param name="lineAxy1"></param>
        /// <param name="lineAxy2"></param>
        /// <param name="lineBxy1"></param>
        /// <param name="lineBxy2"></param>
        /// <returns></returns>
        public static XYZ GetIntersectionPt(XY lineAxy1, XY lineAxy2, XY lineBxy1, XY lineBxy2)
        {
            XY xy = XyUtil.GetIntersectionPtOfTwoLineSegment(lineAxy1, lineAxy2, lineBxy1, lineBxy2);
            if (xy == null) return null;
            return new XYZ(xy.X, xy.Y);
        }
    }
}
