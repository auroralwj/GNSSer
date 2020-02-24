//2018.05.01, czs, create in hmx, 全局星历服务
//2019.01.06, czs, edit in hmx, 一旦获取失败，设置指定小时不必再访问


using System;
using System.IO;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data;
using System.Collections.Generic;
using Gnsser.Correction;
using Geo.Times;
using Gnsser.Times;
using Geo;
using Geo.IO;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 全局星历服务。
    /// </summary>
    public class GlobalIgsEphemerisService : FileEphemerisService
    {
        Log log = new Log(typeof(GlobalIgsEphemerisService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalIgsEphemerisService()
        {
            this.Name = "全局自动 IGS 精密星历服务";
            ServiceHolder = new EphemerisServiceHolder();
        }
        static GlobalIgsEphemerisService instance = new GlobalIgsEphemerisService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalIgsEphemerisService Instance
        {
            get => instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="DataSourceOption"></param>
        public void Init(GnsserConfig DataSourceOption)
        {
            this.GnsserConfig = DataSourceOption;
            IgsEphAutoProvider = new IgsEphemerisServiceAutoProvider();
        }
        #region 核心属性
        /// <summary>
        /// IGS 星历自动提供
        /// </summary>
        public IgsEphemerisServiceAutoProvider IgsEphAutoProvider { get; set; }

        EphemerisServiceHolder ServiceHolder { get; set; }
        #endregion

        /// <summary>
        /// 数据源选项
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }

        #region IEphemerisFile实现 
        private static readonly object locker = new object();
        /// <summary>
        /// 该星历采用的坐标系统,如 IGS08， ITR97
        /// </summary>
        public override string CoordinateSystem { get { if (CurrentService == null) return Data.Rinex.Sp3File.UNDEF; return CurrentService.CoordinateSystem; } }
        /// <summary>
        /// 通过实际数据获取判断是否可用
        /// </summary>
        /// <param name="time"></param>
        /// <param name="satelliteType"></param>
        /// <returns></returns>
        public bool IsAvailable(Time time, SatelliteType satelliteType)
        {
            for (int i = 1; i < 10; i++)
            {
                var testPrn = new SatelliteNumber(i, satelliteType);
                var tryOne = GlobalIgsEphemerisService.Instance.Get(testPrn, time);
                if (tryOne != null)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 当前服务
        /// </summary>
        public IEphemerisService CurrentService { get; set; }
        /// <summary>
        /// 获取星历。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public override Ephemeris Get(SatelliteNumber prn, Time time)
        {
            var services = ServiceHolder.GetService(prn.SatelliteType, time);
            if (services ==null )
            {
                //已经包含，但是为null
                if (ServiceHolder.Contains(prn.SatelliteType, time))
                {
                    return null;
                }
           
                lock (locker)
                {
                    if (services == null)
                    {
                        services = IgsEphAutoProvider.GetService(time, new List<SatelliteType>() { prn.SatelliteType });
                        if (services == null || services.SatelliteTypes == null || services.SatelliteTypes.Count == 0)
                        {
                            SetEmptyServiceWithinSomeHours(prn, time, services);
                            return null;
                        }
                        else
                        { 
                            ServiceHolder.GetOrCreate(prn.SatelliteType)[services.TimePeriod] = services;
                        }
                    }
                }
            }

            this.CurrentService = services;
            return services.Get(prn,time);
        }

        /// <summary>
        /// 设置在一定时间内，不再访问
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <param name="services"></param>
        protected void SetEmptyServiceWithinSomeHours(SatelliteNumber prn, Time time, IEphemerisService services)
        {
            var tp = new BufferedTimePeriod(time, time + TimeSpan.FromHours(this.GnsserConfig.RetryHoursWhenFailedForIgsDownload));
            log.Warn(this.Name + " 获取失败，" + this.GnsserConfig.RetryHoursWhenFailedForIgsDownload + " 小时不再获取。" + tp);
            ServiceHolder[prn.SatelliteType][tp] = services;
        }




        /// <summary>
        /// 时间段。没什么用
        /// </summary>
        public override BufferedTimePeriod TimePeriod
        {
            get { return BufferedTimePeriod.MaxPeriod; }//48个小时外推
        }
        /// <summary>
        /// 没什么用
        /// </summary>
        public override List<SatelliteNumber> Prns
        {
            get
            {
               return SatelliteNumber.AllPrns; 
            }
        }
        /// <summary>
        /// 没什么用
        /// </summary>
        /// <returns></returns>
        public override List<Ephemeris> Gets()
        {
            return new List<Ephemeris>();
        }
        /// <summary>
        /// 没什么用
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public override List<Ephemeris> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        { 
            return new List<Ephemeris>();
        }
        /// <summary>
        /// 没什么用
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="satTime"></param>
        /// <returns></returns>
        public override bool IsAvailable(SatelliteNumber prn, Time satTime)
        { 
            return true;
        }
        #endregion


    }



}