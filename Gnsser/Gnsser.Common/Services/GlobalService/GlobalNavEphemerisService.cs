//2018.07.07, czs, create in hmx, 全局自动导航星历服务
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
using Gnsser.Data.Rinex;

namespace Gnsser
{
    /// <summary>
    ///全局自动导航星历服务
    /// </summary>
    public class GlobalNavEphemerisService : FileEphemerisService, IEphemerisService
    {
        Log log = new Log(typeof(GlobalNavEphemerisService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalNavEphemerisService()
        {
            this.Name = "全局自动导航星历服务";
            ServiceHolder = new GlobalNavEphemerisServiceHolder();
        }
        static GlobalNavEphemerisService instance = new GlobalNavEphemerisService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalNavEphemerisService Instance
        {
            get => instance;
        } 
        /// <summary>
        /// 服务的时段信息
        /// </summary>
        public override BufferedTimePeriod TimePeriod { get { return BufferedTimePeriod.MaxPeriod; }   }

        IgsNavEphemerisAutoProvider AutoProvider { get; set; }

        GlobalNavEphemerisServiceHolder ServiceHolder { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="DataSourceOption"></param>
        public void Init(GnsserConfig DataSourceOption)
        {
            this.GnsserConfig = DataSourceOption;
            AutoProvider = new IgsNavEphemerisAutoProvider() { DataSourceOption = DataSourceOption };
        }
        IEphemerisService CurrentService;
        static object locker = new object();

         
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEphemerisService GetService(Time time)
        {
            var services = ServiceHolder.GetService(time);
            if (services == null)
            {
                lock (locker)
                {
                    services = AutoProvider.GetService(time);
                    if (services == null)
                    {
                        SetEmptyServiceWithinSomeHours(time, services);
                        return null;
                    }
                    else
                    {
                        ServiceHolder[services.TimePeriod] = services;
                    }
                }
            }
            CurrentService = services;

            if (services == null)
            {
                return null;
            }
            return services;
        }
        /// <summary>
        /// 设置在一定时间内，不再访问
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <param name="services"></param>
        protected void SetEmptyServiceWithinSomeHours(Time time, IEphemerisService services)
        {
            var tp = new BufferedTimePeriod(time, time + TimeSpan.FromHours(this.GnsserConfig.RetryHoursWhenFailedForIgsDownload));
            log.Warn(this.Name + " 获取失败，" + this.GnsserConfig.RetryHoursWhenFailedForIgsDownload + " 小时不再获取。" + tp);
            ServiceHolder[tp] = services;
        }


        /// <summary>
        /// 数据源选项
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }

        public EphemerisServiceType ServiceType => CurrentService.ServiceType;

        public int SatCount => CurrentService.SatCount;

        public override string CoordinateSystem => CurrentService.CoordinateSystem;

        static List<SatelliteNumber> prns = SatelliteNumber.AllPrns;

        public override List<SatelliteNumber> Prns => prns;// CurrentService.Prns;

        public List<SatelliteType> SatelliteTypes => CurrentService.SatelliteTypes;
         
         

        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name + ", " + TimePeriod;
        }

        public List<SatelliteNumber> GetPrns(SatelliteType type)
        {
            return CurrentService.GetPrns(type);
        }

        public override List<Ephemeris> Gets(SatelliteNumber prn, Time from, Time to, double interval)
        {
            return GetService(from).Gets(prn, from, to, interval);
        }

        public override List<Ephemeris> Gets(SatelliteNumber prn)
        {
            return CurrentService.Gets(prn );
        }

        public override List<Ephemeris> Gets()
        {
            return CurrentService.Gets();
        }

        public override bool IsAvailable(SatelliteNumber prn, Time satTime)
        {
            return GetService(satTime).IsAvailable(prn,   satTime);
        }

        public   bool IsAvailable(SatelliteType satType, Time satTime)
        {
            return GetService(satTime).IsAvailable(satType, satTime);
        }

        public   bool IsAvailable(List<SatelliteType> satType, Time satTime)
        {
            return GetService(satTime).IsAvailable(satType, satTime);
        }

        public override Ephemeris Get(SatelliteNumber prn, Time time)
        {
            var service = GetService(time);
            return service.Get(prn, time);
        }

        public override List<Ephemeris> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            var service = GetService(timeStart);
            return service.Gets(prn, timeStart, timeEnd);
        }
    }
}