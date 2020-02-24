//2015.12.17, czs, create in hongqing, 基于卫星编号和时间的服务

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
    ///  基于卫星编号和时间的服务。
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TProduct"></typeparam>
    public abstract class SatBasedMultiSysDailyService<TService, TProduct> : MultiSysDailyService<TService>
        where TService : IMultiSatProductService<TProduct>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SatBasedMultiSysDailyService()
        {
            NulledServices = new List<int>();
        }
        /// <summary>
        /// 狗战术
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="TimeIntervalSeconds"></param>
        public SatBasedMultiSysDailyService(Dictionary<SatelliteType, TimeIntervalService<TService>> dic, bool IsSwitchWhenEphemerisNull = false, int TimeIntervalSeconds = 86400)
            : base(dic, TimeIntervalSeconds)
        {
            NulledServices = new List<int>();
            this.IsSwitchWhenEphemerisNull = IsSwitchWhenEphemerisNull;
        }
        /// <summary>
        /// 获取失败后是否切换当前星历
        /// </summary>
        public bool IsSwitchWhenEphemerisNull { get; set; }
        /// <summary>
        /// 试图获取过，但是未成功的服务。不必再次尝试获取了。
        /// </summary>
        private List<int> NulledServices { get; set; }
        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public TProduct Get(SatelliteNumber prn, Time time)
        {
            if (CurrentService != null)
            {
                var result = CurrentService.Get(prn, time);
                if (result == null)
                {
                    //重新选择服务。
                    log.Warn(this.Name +  " " + prn + " 在 " + time + " 获取 " + typeof(TProduct).Name + " 失败！");
                    if (IsSwitchWhenEphemerisNull)
                    { 
                        log.Info( typeof(TProduct).Name + " " + this.Name +  "尝试重新选择服务");
                        return LoopGetAndSetCurrentService(prn, time);
                    }
                }
                return result;
            }
            if (NulledServices.Contains(time.GetGpsWeekAndDay())) return default(TProduct);

            TProduct rst = LoopGetAndSetCurrentService(prn, time);
            return rst;
        }

        /// <summary>
        /// 如果失败则返回 null。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        protected TProduct LoopGetAndSetCurrentService(SatelliteNumber prn, Time time)
        {
            TProduct rst = default(TProduct);
            List<TService> dayServices = GetMayAvailableServices(prn.SatelliteType, time);
            if (dayServices == null || dayServices.Count == 0)
            {
                log.Info("没有找到服务！" + typeof(TService).ToString());

                NulledServices.Add(time.GetGpsWeekAndDay());

                this.CurrentService = default(TService);
                return rst;
            }

            foreach (var item in dayServices)
            {
                rst = item.Get(prn, time);

                if (rst != null)
                {
                    this.CurrentService = item;
                    this.Name = this.CurrentService.ToString();

                    log.Info( "匹配成功！ " + typeof(TProduct).Name + " 服务设置为:" + item.ToString());
                    return rst;
                }
            }
            return rst;
        }
    }

}
