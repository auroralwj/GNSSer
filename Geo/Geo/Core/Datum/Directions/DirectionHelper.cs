//2015.04.15, czs, create in numu shuangliao, 增加注释

using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// 方向判断。可以重构 2015.04.16, czs
    /// </summary>
    public class DirectionHelper
    {
        const double D22_5 = 22.5;
        const double D45 = 45.0;
      //  const double PIPart8 = 0.39269908169872415480783042290994;

        /// <summary>
        /// 得到两个坐标点的相对方向。
        /// 第二个相对于第一个。
        /// </summary>
        /// <param name="me"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static string GetDirectionString(LonLat me, LonLat other)
        {
            Direction dir = GetDirection(me, other);
            return GetDirectionString(dir);
        }
        /// <summary>
        /// 获取字符串表示的方向
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string GetDirectionString(Direction dir)
        { 
            switch (dir)
            {
                case Direction.East:
                    return "东";
                case Direction.North:
                    return "北";
                case Direction.NorthEast:
                    return "东北";
                case Direction.NorthWest:
                    return "西北";
                case Direction.South:
                    return "南";
                case Direction.SouthEast:
                    return "东南";
                case Direction.SouthWest:
                    return "西南";
                case Direction.West:
                    return "西";
                default:
                    return "东";
            }
        }
        /// <summary>
        /// 驾驶方向
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="now"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static DrivenDirection GetDrivenDirection(XYZ pre, XYZ now, XYZ next)
        {
            return  GetDrivenDirection(pre.GetLonLat(), now.GetLonLat(), next.GetLonLat());
        }
        /// <summary>
        /// 驾驶方向
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="now"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static DrivenDirection GetDrivenDirection(LonLat pre, LonLat now, LonLat next)
        {
            double deg1 = GetAngleDeg( pre, now);
            double deg2 = GetAngleDeg( now, next);
            double degree = deg2 - deg1;

            //0<=degree<360
            if (degree < 0)
            {
                degree += 360;
            } 
            if (degree >= 360)
            {
                degree -= 360;
            }

            if (-D45 < degree && degree <= D45)
            {
                return DrivenDirection.Ahead;
            }
            int i = 1;

            if (D45 * (i) < degree && degree <= D45 * (i += 2))
            {
                return DrivenDirection.Left;
            }
            if (D45 * (i) < degree && degree <= D45 * (i += 2))
            {
                return DrivenDirection.Back;
            }
            if (D45 * (i) < degree && degree <= D45 * (i += 2))
            {
                return DrivenDirection.Right;
            }
            if (D45 * (i) < degree && degree <= D45 * (i += 2))
            {
                return DrivenDirection.Ahead;
            }
            return DrivenDirection.Ahead;
        }
        /// <summary>
        /// 方向获取
        /// </summary>
        /// <param name="me"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Direction GetDirection(LonLat me, LonLat other)
        {
            return GetDirection(new XYZ(me.Lon, me.Lat), new XYZ(other.Lon, other.Lat));
        }
        /// <summary>
        /// 方向获取
        /// </summary>
        /// <param name="me"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Direction GetDirection(XYZ me, XYZ other)
        {
            double degree = GetAngleDeg(me, other);

            //0<=degree<360
            if (degree < D22_5)
            {
                degree += 360;
            }
            if (degree >= 360)
            {
                degree -= 360;
            }


            if (-D22_5 < degree &&  degree <= D22_5)
            {
                return Direction.East;
            }
            int i = 1;

            if (D22_5 * (i) < degree && degree <= D22_5 * (i += 2))
            {
                return Direction.NorthEast;
            }
            if (D22_5 * (i) < degree && degree <= D22_5 * (i += 2))
            {
                return Direction.North;
            }
            if (D22_5 * (i) < degree && degree <= D22_5 * (i += 2))
            {
                return Direction.NorthWest;
            }
            if (D22_5 * (i) < degree && degree <= D22_5 * (i += 2))
            {
                return Direction.West;
            }
            if (D22_5 * (i) < degree && degree <= D22_5 * (i += 2))
            {
                return Direction.SouthWest;
            }
            if (D22_5 * (i) < degree && degree <= D22_5 * (i += 2))
            {
                return Direction.South;
            }
            if (D22_5 * (i) < degree && degree <= D22_5 * (i += 2))
            {
                return Direction.SouthEast;
            }
            return Direction.East;
        }
      
        /// <summary>
        /// 获取两点连线的角度，逆时针，以X轴起算,单位全为度。
        /// </summary>
        /// <param name="me"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double GetAngleDeg(XYZ me, XYZ other)
        {
            double latDifferDeg = (other.Y - me.Y);
            double lonDifferDeg = (other.X - me.X);

            return GetAngleDeg(latDifferDeg, lonDifferDeg);
        }
        /// <summary>
        /// 两个坐标的角度
        /// </summary>
        /// <param name="me"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double GetAngleDeg(LonLat me, LonLat other)
        {
            double latDifferDeg = (other.Lat - me.Lat);
            double lonDifferDeg = (other.Lon - me.Lon);

            return GetAngleDeg(latDifferDeg, lonDifferDeg);
        }
        /// <summary>
        /// 逆时针角度
        /// </summary>
        /// <param name="latDifferDeg"></param>
        /// <param name="lonDifferDeg"></param>
        /// <returns></returns>
        public static double GetAngleDeg(double latDifferDeg, double lonDifferDeg)
        {
            double latDiffer = latDifferDeg * ScreenCoordTransformer.DegToRadMultiplier;
            double lonDiffer = lonDifferDeg * ScreenCoordTransformer.DegToRadMultiplier;

            //  double sin = latDiffer / Math.Sqrt(latDiffer*latDiffer + lonDiffer*lonDiffer);
            //0- 2 * PI
            // 返回结果:
            //     角 θ，以弧度为单位，满足 -π≤θ≤π，且 tan(θ) = y / x，其中 (x, y) 是笛卡儿平面中的点。请看下面： 如果 (x, y)
            //     在第 1 象限，则 0 < θ < π/2。如果 (x, y) 在第 2 象限，则 π/2 < θ≤π。如果 (x, y) 在第 3 象限，则 -π
            //     < θ < -π/2。如果 (x, y) 在第 4 象限，则 -π/2 < θ < 0。
            double rad = GetAngleRad(latDiffer, lonDiffer);
            // Console.WriteLine(rad.ToString());
            double degree = rad * ScreenCoordTransformer.RadToDegMultiplier;
            return degree;
        }

        /// <summary>
        /// 通过坐标差求角度。单位为弧度。
        /// </summary>
        /// <param name="yDiffer"></param>
        /// <param name="xDiffer"></param>
        /// <returns></returns>
        public static double GetAngleRad(double yDiffer, double xDiffer)
        {
            double rad = Math.Atan2(yDiffer, xDiffer);
            rad = rad < 0 ? rad + CoordConsts.PI * 2 : rad;
            return rad;
        }

    }

}
