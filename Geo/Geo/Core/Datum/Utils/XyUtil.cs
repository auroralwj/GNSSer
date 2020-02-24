//2014.11.07, czs, create in numu, 分离XY静态算法部分。

using System;

namespace Geo.Coordinates
{
    /// <summary>
    /// 二维平面坐标接口，以X、Y分量表示。
    /// </summary>
    public class XyUtil
    {

        #region 静态计算方法  
        //public static INumeralIndexing GetIntersectionPtOfTwoLineSegment(
        //    TwoDLineSegment<INumeralIndexing> line1,
        //    TwoDLineSegment<INumeralIndexing> line2) 
        //{
        //    return GetIntersectionPtOfTwoLineSegment(line1.CoordA, line1.CoordB, line2.CoordA, line2.CoordB);
        //}
        public static INumeralIndexing GetIntersectionPtOfTwoLineSegment(
            INumeralIndexing lineAxy1,
            INumeralIndexing lineAxy2,
            INumeralIndexing lineBxy1,
            INumeralIndexing lineBxy2)
        {
            return GetIntersectionPtOfTwoLineSegment(
                new XY(lineAxy1[0], lineAxy1[1]),
                new XY(lineAxy2[0], lineAxy2[1]),
                new XY(lineBxy1[0], lineBxy1[1]),
                new XY(lineBxy2[0], lineBxy2[1]));
        }
        /// <summary>
        /// 计算两线段的交点。如果不相交，则返回null。
        /// </summary>
        /// <param name="lineAxy1">线段 1 的第 1 个点</param>
        /// <param name="lineAxy2">线段 1 的第 2 个点</param>
        /// <param name="lineBxy1">线段 2 的第 1 个点</param>
        /// <param name="lineBxy2">线段 2 的第 2 个点</param>
        /// <returns></returns>
        public static XY GetIntersectionPtOfTwoLineSegment(XY lineAxy1, XY lineAxy2, XY lineBxy1, XY lineBxy2)
        {
            //首先判断其Box是否相交。
            Envelope enA = new Envelope(lineAxy1, lineAxy2);
            Envelope enB = new Envelope(lineBxy1, lineBxy2);


            if (!enA.IntersectsWith(enB)) return null;

            //计算直线相交点，并比交是否在线段内。
            XY xy = GetIntersectionPtOfTwoBeeline(lineAxy1, lineAxy2, lineBxy1, lineBxy2);
            if (xy == null) return null;

            if (enA.Contains(xy) && enB.Contains(xy)) return xy;

            return null;
        }

        /// <summary>
        /// 求两直线交点。
        /// </summary>
        /// <param name="lineAXy1"></param>
        /// <param name="lineAxy2"></param>
        /// <param name="lineBxy1"></param>
        /// <param name="lineBxy2"></param>
        /// <returns>若有则直接返回改点，若平行则返回 NULL</returns>
        public static XY GetIntersectionPtOfTwoBeeline(XY lineAxy1, XY lineAxy2, XY lineBxy1, XY lineBxy2)
        {
            //首先计算斜率
            double slopeA = GetSlope(lineAxy1, lineAxy2);
            double slopeB = GetSlope(lineBxy1, lineBxy2);
            if (slopeA.Equals(slopeB)) { return null; }

            if (slopeA.Equals(Double.PositiveInfinity))
            {
                double bB0 = GetPointSlopeLineConstant(lineBxy1, lineBxy2);
                double x0 = lineAxy1.X;
                double y0 = slopeB * x0 + bB0;
                return new XY(x0, y0);
            }
            if (slopeB.Equals(Double.PositiveInfinity))
            {
                double bA0 = GetPointSlopeLineConstant(lineAxy1, lineAxy2);
                double x0 = lineBxy2.X;
                double y0 = slopeA * x0 + bA0;
                return new XY(x0, y0);
            }

            double bA = GetPointSlopeLineConstant(lineAxy1, lineAxy2);
            double bB = GetPointSlopeLineConstant(lineBxy1, lineBxy2);

            double x = (bA - bB) / (slopeB - slopeA);
            double y = slopeA * x + bA;
            XY xy = new XY(x, y);

            if (xy.X.Equals(Double.NaN) || xy.Y.Equals(double.NaN))
            {
                throw new Exception("出错" + xy.ToString());
            }

            return xy;
        }

        /// <summary>
        /// 点斜式的常数项。
        /// </summary>
        /// <param name="lineAxy1"></param>
        /// <param name="lineAxy2"></param>
        /// <returns></returns>
        public static double GetPointSlopeLineConstant(XY lineAxy1, XY lineAxy2)
        {
            double slopeA = GetSlope(lineAxy1, lineAxy2);
            double b = ((lineAxy1.Y + lineAxy2.Y) - slopeA * (lineAxy1.X + lineAxy2.X)) * 0.5;
            return b;
        }

        /// <summary>
        /// 计算斜率
        /// </summary>
        /// <param name="lineAxy1"></param>
        /// <param name="lineAxy2"></param>
        /// <returns>斜率，如果与X垂直，则返回正无穷</returns>
        public static double GetSlope(XY lineAxy1, XY lineAxy2)
        {
            double aDeltaX = lineAxy2.X - lineAxy1.X;
            double aDeltaY = lineAxy2.Y - lineAxy1.Y;
            //double slopeA = Math.Atan2(aDeltaY, aDeltaX);
            if (aDeltaX == 0) return Double.PositiveInfinity;

            double slopeA = aDeltaY / aDeltaX;
            return slopeA;
        }
        /// <summary>
        /// 通过坐标差求角度。单位为弧度。
        /// </summary>
        /// <param name="yDiffer"></param>
        /// <param name="xDiffer"></param>
        /// <returns></returns>
        public static double GetAngleRad(double yDiffer, double xDiffer)
        {
            //  double sin = latDiffer / Math.Sqrt(latDiffer*latDiffer + lonDiffer*lonDiffer);
            //0- 2 * PI
            // 返回结果:
            //     角 θ，以弧度为单位，满足 -π≤θ≤π，且 tan(θ) = y / x，其中 (x, y) 是笛卡儿平面中的点。请看下面： 如果 (x, y)
            //     在第 1 象限，则 0 < θ < π/2。如果 (x, y) 在第 2 象限，则 π/2 < θ≤π。如果 (x, y) 在第 3 象限，则 -π
            //     < θ < -π/2。如果 (x, y) 在第 4 象限，则 -π/2 < θ < 0。
            double rad = Math.Atan2(yDiffer, xDiffer);
            rad = rad < 0 ? rad + CoordConsts.PI * 2 : rad;
            return rad;
        }
        /// <summary>
        /// 已知一条直线上的X坐标，求值Y坐标。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double GetYOnLine(double x, XY pt1, XY pt2)
        {
            return GetYOnLine(x, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        /// <summary>
        /// 已知一条直线上的X坐标，求值Y坐标。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static double GetYOnLine(double x, double x1, double y1, double x2, double y2)
        {
            double up = (x - x1) * (y2 - y1);
            double down = x2 - x1;
            if (down == 0) throw new ArgumentException("两点的斜率无穷大！");
            return up / down + y1;
        }

        /// <summary>
        /// //判断点是否落在三角形外接圆的内部
        /// xc, yc, r;//外接圆的圆心和半径
        ///  //Return TRUE if the point (xp,yp) lies inside the circumcircle 
        ///made up by points (x1,y1) (x2,y2) (x3,y3)
        ///The circumcircle centre is returned in (xc,yc) and the radius r
        ///NOTE: A point on the edge is inside the circumcircle
        /// </summary>
        /// <param name="xp"></param>
        /// <param name="yp"></param>
        /// <param name="x1">三角形</param>
        /// <param name="y1">三角形</param>
        /// <param name="x2">三角形</param>
        /// <param name="y2">三角形</param>
        /// <param name="x3">三角形</param>
        /// <param name="y3">三角形</param>
        /// <param name="xc">外接圆的圆心</param>
        /// <param name="yc">外接圆的圆心</param>
        /// <param name="r">外接圆半径</param>
        /// <returns></returns>
        public static bool InCircle(double xp, double yp, double x1, double y1, double x2, double y2, double x3, double y3, double xc, double yc, double r)
        {
            double eps;
            double m1, m2, mx1, mx2, my1, my2, dx, dy, rsqr, drsqr;
            eps = 0.000001;
            bool InCircle = false;
            if (Math.Abs(y2 - y1) < eps)
            {
                m2 = -(x3 - x2) / (y3 - y2);
                mx2 = (x2 + x3) / 2;
                my2 = (y2 + y3) / 2;
                xc = (x2 + x1) / 2;
                yc = m2 * (xc - mx2) + my2;
            }
            else if (Math.Abs(y3 - y2) < eps)
            {
                m1 = -(x2 - x1) / (y2 - y1);
                mx1 = (x1 + x2) / 2;
                my1 = (y1 + y2) / 2;
                xc = (x3 + x2) / 2;
                yc = m1 * (xc - mx1) + my1;
            }
            else
            {
                m1 = -(x2 - x1) / (y2 - y1);
                m2 = -(x3 - x2) / (y3 - y2);
                mx1 = (x1 + x2) / 2;
                mx2 = (x2 + x3) / 2;
                my1 = (y1 + y2) / 2;
                my2 = (y2 + y3) / 2;
                xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                yc = m1 * (xc - mx1) + my1;
            }
            dx = x2 - xc;
            dy = y2 - yc;
            rsqr = dx * dx + dy * dy;
            r = Math.Sqrt(rsqr);
            dx = xp - xc;
            dy = yp - yc;
            drsqr = dx * dx + dy * dy;
            if (drsqr <= rsqr)
            { InCircle = true; }
            return InCircle;
        }

        /// <summary>
        /// 判断点在边的哪个旁边，-1代表左边，0代表边上，1代表右边
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static int WhichSide(XY pt, XY ptA, XY ptB)
        {
            return WhichSide(pt.X, pt.Y, ptA.X, ptA.Y, ptB.X, ptB.Y);
        }

        /// <summary>
        /// 判断点在边的哪个旁边，-1代表左边，0代表边上，1代表右边
        /// </summary>
        /// <param name="xp"></param>
        /// <param name="yp"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static int WhichSide(double xp, double yp, double x1, double y1, double x2, double y2)
        {
            int WhichSide;
            double equation = ((yp - y1) * (x2 - x1)) - ((y2 - y1) * (xp - x1));
            if (equation > 0)
            {
                WhichSide = -1;
            }
            else if (equation == 0)
            {
                WhichSide = 0;
            }
            else
            {
                WhichSide = 1;
            }
            return WhichSide;
        }

        #endregion


    }
}
