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
    /// 多系统星历服务池。
    /// </summary>
    public class EphemerisServiceHolder : MultiSysServiceHoulder<IEphemerisService>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EphemerisServiceHolder(int MaxHoulderCount = 64) : base(MaxHoulderCount)
        {
            this.Name = "多系统星历服务池";
        }
    }

    /// <summary>
    /// 多系统钟差服务池。
    /// </summary>
    public class ClockServiceHolder : MultiSysServiceHoulder<IClockService>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ClockServiceHolder(int MaxHoulderCount = 32) : base(MaxHoulderCount)
        {
            this.Name = "多系统钟差服务池";
        }
    } 

    /// <summary>
    /// 多系统钟差服务池。
    /// </summary>
    public class SimpleClockServiceHolder : MultiSysServiceHoulder<ISimpleClockService>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SimpleClockServiceHolder(int MaxHoulderCount = 32) : base(MaxHoulderCount)
        {
            this.Name = "多系统简单钟差服务池";
        }
    } 

    /// <summary>
    /// 多系统服务池
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class MultiSysServiceHoulder<TService> : BaseConcurrentDictionary<SatelliteType, ServiceHoulder<TService>>
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultiSysServiceHoulder(int MaxHoulderCount = 100)
        {
            this.Name = "多系统服务池";
            this.MaxHoulderCount = MaxHoulderCount;
        }
        /// <summary>
        /// 最大的保有数量，超出后去除第一个。
        /// </summary>
        public int MaxHoulderCount { get; set; }

        /// <summary>
        /// 当前的
        /// </summary>
        public KeyValuePair<SatelliteType, ServiceHoulder<TService>> Current { get; set; }

        static object locker = new object();
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="satType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Contains(SatelliteType satType, Time time)
        {
            if (!this.Contains(satType)) { return false; }
            return this[satType].Contains(time);
        }
        /// <summary>
        /// 是否包含该时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="satType"></param>
        /// <returns></returns>
        public TService GetService(SatelliteType satType, Time time)
        {
            lock (locker)
            {
                if (Current.Value != null && Current.Key == satType)
                {
                    return Current.Value.GetService(time);
                }

                var holder = this.GetOrCreate(satType);
                Current = new KeyValuePair<SatelliteType, ServiceHoulder<TService>>(satType, holder);
                return holder.GetService(time); //;  
            }
        }

        /// <summary>
        /// 创建默认
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override ServiceHoulder<TService> Create(SatelliteType key)
        {
            return new ServiceHoulder<TService>(MaxHoulderCount);
        }

    }

     
}