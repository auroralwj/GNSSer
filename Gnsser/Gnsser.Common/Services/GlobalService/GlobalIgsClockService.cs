//2018.05.02, czs, create in hmx, 全局自动钟差服务
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
    /// 全局自动钟差服务。
    /// </summary>
    public class GlobalIgsSimpleClockService : AbstractSimpleClockService
    {
        Log log = new Log(typeof(GlobalIgsSimpleClockService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalIgsSimpleClockService()
        {
            this.Name = "全局自动 IGS 精密钟差服务";
            ServiceHolder = new SimpleClockServiceHolder();
        }
        static GlobalIgsSimpleClockService instance = new GlobalIgsSimpleClockService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalIgsSimpleClockService Instance
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
            igsClkAutoProvider = new IgsSimpleClockServiceAutoProvider() { DataSourceOption = DataSourceOption };
        }

        IgsSimpleClockServiceAutoProvider igsClkAutoProvider { get; set; }

        SimpleClockServiceHolder ServiceHolder { get; set; }

        /// <summary>
        /// 数据源选项
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }

        #region IEphemerisFile实现 
        static object locker = new object();
        /// <summary>
        /// 获取星历。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public override SimpleClockBias Get(SatelliteNumber prn, Time time)
        { 
            var services = ServiceHolder.GetService(prn.SatelliteType, time);
            if (services ==null || services.SatelliteTypes == null || services.SatelliteTypes.Count ==0)
            {
                if(services is EmptySimpleClockService)
                {
                    return null;
                }   
                //已经包含，但是为null
                if (ServiceHolder.Contains(prn.SatelliteType, time))
                {
                    return null;
                } 
                lock (locker)
                {
                    if (services == null)
                    {
                        services = igsClkAutoProvider.GetService(time, new List<SatelliteType>() { prn.SatelliteType });
                        if (services == null ||  services.SatelliteTypes == null || services.SatelliteTypes.Count == 0)
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
            return services.Get(prn, time);
        }
        /// <summary>
        /// 设置在一定时间内，不再访问
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <param name="services"></param>
        protected void SetEmptyServiceWithinSomeHours(SatelliteNumber prn, Time time, ISimpleClockService services)
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

        public override SimpleClockBias Get(string nameOrPrn, Time time)
        {
            throw new NotImplementedException();
        }

        public override List<SimpleClockBias> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
     






    /// <summary>
    /// 全局自动钟差服务。
    /// </summary>
    public class GlobalIgsClockService : AbstractClockService
    {
        Log log = new Log(typeof(GlobalIgsEphemerisService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalIgsClockService()
        {
            this.Name = "全局自动 IGS 精密钟差服务";
            ServiceHolder = new ClockServiceHolder();
        }
        static GlobalIgsClockService instance = new GlobalIgsClockService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalIgsClockService Instance
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
            igsClkAutoProvider = new IgsClockServiceAutoProvider() { DataSourceOption = DataSourceOption };
        }

        IgsClockServiceAutoProvider igsClkAutoProvider { get; set; }

        ClockServiceHolder ServiceHolder { get; set; }

        /// <summary>
        /// 数据源选项
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }

        #region IEphemerisFile实现 
        static object locker = new object();
        /// <summary>
        /// 获取星历。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public override AtomicClock Get(SatelliteNumber prn, Time time)
        {
            var services = ServiceHolder.GetService(prn.SatelliteType, time);
            if (services == null)
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
                        services = igsClkAutoProvider.GetService(time, new List<SatelliteType>() { prn.SatelliteType });
                        if (services == null)
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
            return services.Get(prn, time);
        }
        /// <summary>
        /// 设置在一定时间内，不再访问
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <param name="services"></param>
        protected void SetEmptyServiceWithinSomeHours(SatelliteNumber prn, Time time, IClockService services)
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

        public override AtomicClock Get(string nameOrPrn, Time time)
        {
            throw new NotImplementedException();
        }

        public override List<AtomicClock> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            throw new NotImplementedException();
        }
        #endregion


    }





}