//2015.12.09, czs, create in 成都地铁2号线往犀浦, IClockService, 按天组织的GNSS服务

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
    /// 多系统按天服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public abstract class MultiSysDailyService<TService> : BaseDictionary<SatelliteType, TimeIntervalService<TService>>

    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultiSysDailyService() { }
        /// <summary>
        /// 带数据的构造函数
        /// </summary>
        /// <param name="dic"></param>
        public MultiSysDailyService(Dictionary<SatelliteType, TimeIntervalService<TService>> dic, int TimeIntervalSeconds) :base(dic){
            this.TimeIntervalSeconds = TimeIntervalSeconds;
         }

        /// <summary>
        /// 当前的服务。如果当前任务失败，则重新查找服务。
        /// </summary>
        public TService CurrentService { get; protected set; }

        /// <summary>
        /// 时段信息
        /// </summary>
        public int TimeIntervalSeconds { get; set; }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public abstract TService GetService();


        /// <summary>
        /// 获取可能可用的日服务列表。服务将从中选取。
        /// </summary>
        /// <param name="SatelliteType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public virtual List<TService> GetMayAvailableServices(SatelliteType SatelliteType, Time time)
        {
            if (!Contains(SatelliteType)) { return new List<TService>() ; }

            if (this.TimeIntervalSeconds == 86400)
            {
                var GpsWeekAndDay = time.GetGpsWeekAndDay();

                var service = this[SatelliteType].Get(GpsWeekAndDay);
                if (service == null && time.IsNearToDayEdge())//可能在边界区域
                {
                    GpsWeekAndDay = Time.GetNearstWeekAndDay(time);
                    service = this[SatelliteType].Get(GpsWeekAndDay);
                }
                return service;
            }
            else
            {
                var GpsWeek = time.GpsWeek;
                var service = this.Get(SatelliteType).Get( GpsWeek);
                if (service == null && time.IsNearToWeekEdge())//可能在边界区域
                {
                    GpsWeek = Time.GetNearstWeek(time);
                    service = this.Get(SatelliteType).Get(GpsWeek);
                }
                return service;
            }
        }
    }

}
