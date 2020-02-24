//2015.12.09, czs, create in 成都地铁2号线往犀浦, IClockService, 按天组织的GNSS服务
//2018.03.15, czs, edti in hmx, 增加超快星历支持

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm;
using Gnsser.Service;
using Geo.Times;
using Geo;
using Gnsser.Data;

namespace Gnsser.Service
{     
    /// <summary>
    /// 按整型时间组织的GNSS服务，只提供单系统服务。
    /// </summary>
    public class TimeIntervalService<TService> : BaseDictionary<int, List<TService>>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TimeIntervalSeconds"></param>
        public TimeIntervalService(  int TimeIntervalSeconds)
        { 
            this.TimeIntervalSeconds = TimeIntervalSeconds;
        }
        /// <summary>
        /// 时段信息
        /// </summary>
        public int TimeIntervalSeconds { get; set; }

        /// <summary>
        /// 当前服务
        /// </summary>
        public TService CurrentService { get; protected set; }

        /// <summary>
        /// 服务时段,计算得来，不保证连续性。
        /// </summary>
        public BufferedSuccessiveTimePeriod TimePeriod
        {
            get
            {
                BufferedSuccessiveTimePeriod timeperiod = new BufferedSuccessiveTimePeriod();
                if (TimeIntervalSeconds == 86400)
                {
                    var secondsPerDay = 86400;

                    foreach (var item in Keys)
                    {
                        int gpsWeek = item / 10;
                        double secondOfWeek = item % 10 * secondsPerDay;
                        var startTime = new Time(gpsWeek, secondOfWeek);
                        var endTime = new Time(gpsWeek, secondOfWeek + secondsPerDay);


                        BufferedTimePeriod period = new BufferedTimePeriod(startTime, endTime);
                        timeperiod.Add(period);
                    }
                    return timeperiod;
                }                

                double secondsPerWeek = 86400.0 * 7;

                foreach (var item in Keys)
                {
                    int gpsWeek = item / 10;
                    var startTime = new Time(gpsWeek, 0.0);
                    var endTime = new Time(gpsWeek, secondsPerWeek);
                    BufferedTimePeriod period = new BufferedTimePeriod(startTime, endTime);
                    timeperiod.Add(period);
                }
                return timeperiod;
            }
        }

        /// <summary>
        /// 添加一个
        /// </summary>
        /// <param name="time"></param>
        /// <param name="services"></param>
        public void Add(Time time, List<TService> services)
        {
            if (TimeIntervalSeconds < 86400) //按小时计
            {
                this.Add(time.GetGpsWeekAndDay(), services);
            }
            else if (TimeIntervalSeconds == 86400)
            {
                this.Add(time.GetGpsWeekAndDay(), services);
            }
            else { this.Add(time.GpsWeek, services); }
        }
         

        /// <summary>
        /// 获取第一个服务
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TService GetService(Time time)
        {
            var ss = GetServices(time);
            if (ss != null && ss.Count != 0)
                return ss[0];
            return default(TService);
        } 
        /// <summary>
        /// 获取备选服务列表
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<TService> GetServices(Time time)
        {
            var day = time.GetGpsWeekAndDay();
            if (this.Contains(day))
            {
               return  this[day]; 
            }
            return null;
        }


        /// <summary>
        /// 简单判断是否具有该服务
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsAvailable(Time time)
        { 
            var day = time.GetGpsWeekAndDay();
            if (!this.Contains(day)) return false;

            var services = Get(day);

            if (services.Count == 0) return false;

            return true;
        }
        /// <summary>
        /// 获取可能可用的日服务列表。服务将从中选取。找过一次就不必再找了。
        /// </summary>
        /// <param name="SatelliteType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual List<TService> GetMayAvailableServices(Time time)
        {
            var GpsWeek = time.GpsWeek;
            var service = this.Get(GpsWeek);
            if (service == null && time.IsNearToWeekEdge())//可能在边界区域
            {
                GpsWeek = Time.GetNearstWeek(time);
                service = this.Get(GpsWeek);
            }

            return service;
        }
    }
}
