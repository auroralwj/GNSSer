using System;
using Geo.Coordinates; 
using Gnsser.Times;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gnsser.Service; 

namespace Gnsser
{
    /// <summary>
    /// 轨道参数计算器。
    /// </summary>
    public class EphemerisUtil
    {

        /// <summary>
        /// 判断是否参数表示的星历
        /// </summary>
        /// <param name="satType"></param>
        /// <returns></returns>
        public static bool IsEphemerisParam(SatelliteType satType)
        {
            if (satType == SatelliteType.E
                           || satType == SatelliteType.C
                           || satType == SatelliteType.G
                           || satType == SatelliteType.J
                           || satType == SatelliteType.I)
            {
                return true;
            }

            if (satType == SatelliteType.S
                           || satType == SatelliteType.R)
            {
                return false;
            }
            throw new NotImplementedException("未实现。" + satType);
        }
        /// <summary>
        /// 根据卫星位置，计算可视卫星
        /// </summary>
        /// <param name="stationPos"></param>
        /// <param name="Ephemeries"></param>
        /// <param name="cutOffAngle">高度截止角</param>
        /// <returns></returns>
        public static List<Ephemeris> GetSatsInVisible(XYZ stationPos, IEnumerable<Ephemeris> Ephemeries, double cutOffAngle = 5)
        {
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var item in Ephemeries)
            {
                var polar = CoordTransformer.XyzToGeoPolar( item.XYZ, stationPos);
                if (polar.Elevation > cutOffAngle)
                {
                    list.Add(item);
                } 
            }
            return list; 
        }

        /// <summary>
        /// 根据卫星位置，计算可视卫星
        /// </summary>
        /// <param name="geoCoord"></param>
        /// <param name="Ephemeries"></param>
        /// <param name="cutOffAngle">高度截止角</param>
        /// <returns></returns>
        public static List<Ephemeris> GetSatsInVisible(GeoCoord geoCoord, IEnumerable<Ephemeris> Ephemeries, double cutOffAngle = 5)
        {
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var item in Ephemeries)
            {
                var polar = CoordTransformer.XyzToPolar(item.XYZ, geoCoord);
                if (polar.Elevation > cutOffAngle)
                {
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// 根据卫星位置，计算可视卫星
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="geoCoord"></param>
        /// <param name="Ephemeries"></param>
        /// <param name="cutOffAngle">高度截止角</param>
        /// <returns></returns>
        public static List<Ephemeris> GetSatsInVisible(XYZ xyz, GeoCoord geoCoord, IEnumerable<Ephemeris> Ephemeries, double cutOffAngle = 5)
        {
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var item in Ephemeries)
            {
                var polar = CoordTransformer.XyzToPolar(item.XYZ ,xyz, geoCoord.Lon, geoCoord.Lat);
                if (polar.Elevation > cutOffAngle)
                {
                    list.Add(item);
                }
            }
            return list;
        }


        #region 赋权

        /// <summary>
        /// 根据星历服务类型给定一个初步的精度信息。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static XYZ GetRms(EphemerisServiceType type)
        {
            switch (type)
            {
                case EphemerisServiceType.Composed:
                case EphemerisServiceType.Navigation:
                    return new XYZ(10, 10, 10);
                case EphemerisServiceType.Precise:
                    return new XYZ(0.1, 0.1, 0.1);
                default:
                    return new XYZ(10, 10, 10);
            }
        }

        #endregion



        #region 地球自转效应的改正
        /// <summary>
        /// 卫星坐标的地球自转改正
        /// </summary>
        /// <param name="satPos">卫星位置</param>
        /// <param name="receiverPos">接收机位置</param>
        /// <returns></returns>
        public static XYZ CorrectEarthSagnac(XYZ satPos, XYZ receiverPos)
        {
            double len = (satPos - receiverPos).Length;
            double elapsedTime = len / GnssConst.LIGHT_SPEED;
            return CorrectEarthSagnac(satPos, elapsedTime);
        } 
        /// <summary>
        /// 卫星坐标的地球自转改正
        /// </summary>
        /// <param name="satPos">卫星位置</param>
        /// <param name="elapsedTime">信号传输时间</param>
        /// <returns></returns>
        public static XYZ CorrectEarthSagnac(XYZ satPos, double elapsedTime)
        {
            double sag = GnssConst.EARTH_ROTATE_SPEED * elapsedTime;
            double sx = Math.Cos(sag) * satPos.X + Math.Sin(sag) * satPos.Y;
            double sy = -Math.Sin(sag) * satPos.X + Math.Cos(sag) * satPos.Y;

            return new XYZ(sx, sy, satPos.Z);
        }

        /// <summary>
        /// 利用信号传输时间计算地球自转效应，改正卫星位置。
        /// 地球自转, sag: 调正, 整理; 修正, 位移 
        /// </summary>
        /// <param name="xyz">传输RemoveSat
        /// <param name="transitTime">信号传输时间</param>
        /// <returns></returns>
        public static XYZ CorrectEarthSagnac_Fast(XYZ xyz, double transitTime)
        {
            double sag = GnssConst.EARTH_ROTATE_SPEED * transitTime;
            return new XYZ(
                   xyz.X * Math.Cos(sag) + xyz.Y * Math.Sin(sag),
                   xyz.Y * Math.Cos(sag) - xyz.X * Math.Sin(sag),
                   xyz.Z);
        }

        #endregion

        #region 相对轮效应的改正
        /// <summary>
        /// 相对论改正。
        /// </summary>
        /// <param name="SatXyz">卫星位置</param>
        /// <param name="SatSpeed">卫星速度</param>
        /// <returns></returns>
        public static double GetRelativeCorrection(XYZ SatXyz, XYZ SatSpeed)
        {
            double relativity = -2 * (SatXyz.X / GnssConst.LIGHT_SPEED) * (SatSpeed.X / GnssConst.LIGHT_SPEED)
                - 2 * (SatXyz.Y / GnssConst.LIGHT_SPEED) * (SatSpeed.Y / GnssConst.LIGHT_SPEED)
                - 2 * (SatXyz.Z / GnssConst.LIGHT_SPEED) * (SatSpeed.Z / GnssConst.LIGHT_SPEED);


            //double relativity0 = -2 * (SatXyz.X * SatSpeed.X
            //   + SatXyz.Y * SatSpeed.Y
            //   + SatXyz.Z * SatSpeed.Z) / GnssConst.LIGHT_SPEED / GnssConst.LIGHT_SPEED;
            if (Math.Abs( relativity) > 1)
            {
                new Geo.IO.Log(typeof(EphemerisUtil)).Error("相对论改正也太大了吧！可能是卫星速度计算错误" + SatSpeed);
            }
            return relativity;
        }

        /// <summary>
        /// 定点迭代法
        /// 开普勒方程 for 偏心改正。
        /// solve for eccentric anomaly given mean anomaly and orbital eccentricity
        /// use simple fixed point iteration of kepler's equation
        /// </summary>
        /// <param name="em">M 平近点角</param>
        /// <param name="e">椭圆轨道的偏心率</param>
        /// <returns></returns>
        public static double KeplerEqForEccAnomaly(double em, double e)
        {
            double ecca, ecca0;           //*** iterates of eccentric anomaly
            //*** initialize eccentric anomaly
            ecca = em + e * Math.Sin(em);

            //*** exit only on convergence
            int counter = 0;
            do
            {
                ecca0 = ecca;
                ecca = em + e * Math.Sin(ecca0);
                counter++;
            } while (Math.Abs((ecca - ecca0) / ecca) > 1.0e-14 && counter < 20);
            return ecca;
        }
        #endregion
    }
}