// 2014.09.19, czs, create, in hailutu, 数据源统一设计
//2016.11.01, czs, edit in hongqing, 采用缓存存储日月位置，PPP效率得到极大提高

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Geo.Times; 
using Gnsser.Times;
using Geo;

namespace Gnsser.Data
{
    /// <summary>
    /// 存储太阳月亮时间和位置
    /// </summary>
    public class SunMoonTimeStorage : BaseConcurrentDictionary<long, SunMoonTime>
    {

    }
    /// <summary>
    /// 以分钟采用率为单位。
    /// </summary>
    public class SunMoonTime
    {
        public Time Time { get; set; }
        public XYZ SunXyz { get; set; }
        public XYZ MoonXyz { get; set; }

        public double Gmst { get; set; }
    }

    /// <summary>
    /// 宇宙星体信息供应者
    /// </summary>
    public class UniverseObjectProvider : GnssFileService<XYZ>
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        private UniverseObjectProvider()
            : base("")
        {
            SunMoonTimeStorage = new SunMoonTimeStorage();
        }
        private static UniverseObjectProvider instance = new UniverseObjectProvider();
        /// <summary>
        /// 单例模式
        /// </summary>
        static public UniverseObjectProvider Instance { get { return instance; } }

        //Objects to compute Sun and Moon positions
        SunPosition sunPosition = new SunPosition();
        MoonPosition moonPosition = new MoonPosition();

        SunMoonPosition sunmoonPosition = new SunMoonPosition();
        SunMoonTimeStorage SunMoonTimeStorage { get; set; }

        /// <summary>
        /// 构建关键字，将时间划分为以分钟或半分钟为序列。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public long BuildKey(Time time)
        {
            return time.TickTime.SecondTicks / 30;
        }

        /// <summary>
        /// 获取指定时刻太阳的位置。
        /// </summary>
        /// <param name="Time">时间</param>
        /// <returns></returns>
        public XYZ GetSunPosition(Time Time)
        {
            return sunPosition.GetPosition(Time);
        }

        /// <summary>
        /// 获取指定时刻太阳与月亮的位置
        /// </summary>
        /// <param name="time"></param>
        /// <param name="erpv"></param>
        /// <param name="rsun"></param>
        /// <param name="rmoon"></param>
        /// <param name="gmst"></param>
        public void GetSunPosition(Time time, ErpItem erpv, ref XYZ rsun, ref XYZ rmoon, ref double gmst)
        {
            var item2 = GetOrCreate(time, erpv);

            rmoon = item2.MoonXyz;
            rsun = item2.SunXyz;
            gmst = item2.Gmst;
        }
        static object locker = new object();
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="time"></param>
        /// <param name="erpv"></param>
        /// <returns></returns>
        public SunMoonTime GetOrCreate(Time time, ErpItem erpv)
        {
            var key = BuildKey(time);

            if (!SunMoonTimeStorage.Contains(key))
            {
                lock (locker)
                {
                    if (!SunMoonTimeStorage.Contains(key))
                    {
                        SunMoonTime item = new SunMoonTime();
                        sunmoonPosition.GetPosition(time, erpv);
                        item.SunXyz = sunmoonPosition.rSun;
                        item.MoonXyz = sunmoonPosition.rMoon;
                        item.Gmst = sunmoonPosition.gmst;
                        //return key;

                        SunMoonTimeStorage.Add(key, item);
                    }
                }
            }
            var item2 = SunMoonTimeStorage.Get(key);
            return item2;
        }
        /// <summary>
        /// 获取指定时刻太阳的位置。
        /// </summary>
        /// <param name="Time">时间</param>
        /// <returns></returns>
        public void GetSunPosition(Time time, ErpItem erpv, ref XYZ rsun)
        {
            var item2 = GetOrCreate(time, erpv);

            //   sunmoonPosition.GetPosition(Time, erpv);
            rsun = item2.SunXyz;//.rSun;

        }


        /// <summary>
        /// 获取指定时刻月亮的位置。
        /// </summary>
        /// <param name="Time">时间</param>
        /// <returns></returns>
        public XYZ GetMoonPosition(Time Time)
        {
            return moonPosition.GetPosition(Time);
        }
        /// <summary>
        /// 地球极移，单位：弧度。
        /// pole displacement x y parameters, in arcseconds
        /// </summary>
        /// <returns></returns>
        public XY GetEarthPoleDisplacement()
        {
            return new XY(0.076263, 0.289897);
        }
    }
}
