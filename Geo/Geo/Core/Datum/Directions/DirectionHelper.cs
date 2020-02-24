//2015.04.15, czs, create in numu shuangliao, ����ע��

using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// �����жϡ������ع� 2015.04.16, czs
    /// </summary>
    public class DirectionHelper
    {
        const double D22_5 = 22.5;
        const double D45 = 45.0;
      //  const double PIPart8 = 0.39269908169872415480783042290994;

        /// <summary>
        /// �õ�������������Է���
        /// �ڶ�������ڵ�һ����
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
        /// ��ȡ�ַ�����ʾ�ķ���
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string GetDirectionString(Direction dir)
        { 
            switch (dir)
            {
                case Direction.East:
                    return "��";
                case Direction.North:
                    return "��";
                case Direction.NorthEast:
                    return "����";
                case Direction.NorthWest:
                    return "����";
                case Direction.South:
                    return "��";
                case Direction.SouthEast:
                    return "����";
                case Direction.SouthWest:
                    return "����";
                case Direction.West:
                    return "��";
                default:
                    return "��";
            }
        }
        /// <summary>
        /// ��ʻ����
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
        /// ��ʻ����
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
        /// �����ȡ
        /// </summary>
        /// <param name="me"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Direction GetDirection(LonLat me, LonLat other)
        {
            return GetDirection(new XYZ(me.Lon, me.Lat), new XYZ(other.Lon, other.Lat));
        }
        /// <summary>
        /// �����ȡ
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
        /// ��ȡ�������ߵĽǶȣ���ʱ�룬��X������,��λȫΪ�ȡ�
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
        /// ��������ĽǶ�
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
        /// ��ʱ��Ƕ�
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
            // ���ؽ��:
            //     �� �ȣ��Ի���Ϊ��λ������ -�СܦȡܦУ��� tan(��) = y / x������ (x, y) �ǵѿ���ƽ���еĵ㡣�뿴���棺 ��� (x, y)
            //     �ڵ� 1 ���ޣ��� 0 < �� < ��/2����� (x, y) �ڵ� 2 ���ޣ��� ��/2 < �ȡܦС���� (x, y) �ڵ� 3 ���ޣ��� -��
            //     < �� < -��/2����� (x, y) �ڵ� 4 ���ޣ��� -��/2 < �� < 0��
            double rad = GetAngleRad(latDiffer, lonDiffer);
            // Console.WriteLine(rad.ToString());
            double degree = rad * ScreenCoordTransformer.RadToDegMultiplier;
            return degree;
        }

        /// <summary>
        /// ͨ���������Ƕȡ���λΪ���ȡ�
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
