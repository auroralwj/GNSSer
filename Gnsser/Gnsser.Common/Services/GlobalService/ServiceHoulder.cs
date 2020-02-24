//2018.05.02, czs, create in hmx, 不用考虑系统类型的服务池
//2018.05.27, czs, edit in hmx,增加球谐函数服务池

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
    /// Iono 服务池。
    /// </summary>
    public class GlobalNavEphemerisServiceHolder : ServiceHoulder<IEphemerisService>
    { 
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GlobalNavEphemerisServiceHolder(int MaxHoulderCount = 64) : base(MaxHoulderCount)
        {
            this.Name = "导航星历服务池";
        }
    }

    /// <summary>
    /// Iono 服务池。
    /// </summary>
    public class GlobalKlobucharIonoServiceHolder : ServiceHoulder<IIonoService>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GlobalKlobucharIonoServiceHolder(int MaxHoulderCount = 64) : base(MaxHoulderCount)
        {
            this.Name = "Klobuchar 电离层 服务池";
        }
    }
    /// <summary>
    /// Iono 服务池。
    /// </summary>
    public class GlobalCodeHarmoIonoServiceHolder : ServiceHoulder<IIonoService>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GlobalCodeHarmoIonoServiceHolder(int MaxHoulderCount = 64) : base(MaxHoulderCount)
        {
            this.Name = "球谐函数电离层 服务池";
        }
    }

    /// <summary>
    /// grid Iono 服务池。
    /// </summary>
    public class IgsGridIonoServiceHolder : ServiceHoulder<IGridIonoFileService>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IgsGridIonoServiceHolder(int MaxHoulderCount = 64) : base(MaxHoulderCount)
        {
            this.Name = "格网电离层 服务池";
        }
    }


    /// <summary>
    /// ERP 服务池。
    /// </summary>
    public class ErpServiceHolder : ServiceHoulder<IErpFileService>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ErpServiceHolder(int MaxHoulderCount = 64) : base(MaxHoulderCount)
        {
            this.Name = "ERP 服务池";
        }
    }


    /// <summary>
    /// 不用考虑系统类型的服务池
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class ServiceHoulder<TService> : BaseConcurrentDictionary<BufferedTimePeriod, TService>
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ServiceHoulder(int MaxHoulderCount = 64)
        {
            this.Name = "多系统服务池";
            this.MaxHoulderCount = MaxHoulderCount;
            LastVisitTime = new BaseConcurrentDictionary<BufferedTimePeriod, DateTime>();
            MinExpireTime = TimeSpan.FromMinutes(3);//3分钟，应该足够了
            IsCanBuffered = true;
        }
        /// <summary>
        /// 最大的保有数量，超出后去除第一个。
        /// </summary>
        public int MaxHoulderCount { get; set; }
        /// <summary>
        /// 是否可以跨时段访问,根据时间buffer设置
        /// </summary>
        public bool IsCanBuffered { get; set; }
        /// <summary>
        /// 当前的
        /// </summary>
        public KeyValuePair<BufferedTimePeriod, TService> Current { get; set; }
        /// <summary>
        /// 存储刚刚访问的时间，超过时限才释放资源
        /// </summary>
        BaseConcurrentDictionary<BufferedTimePeriod, DateTime> LastVisitTime { get; set; }
        /// <summary>
        /// 最小的过期时间，超过才能释放资源
        /// </summary>
        public TimeSpan MinExpireTime { get; set; }

        static object getLocker = new object();
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public override void Set(BufferedTimePeriod key, TService val)
        {
            if (this.Count > MaxHoulderCount)
            {
                lock (getLocker)
                {
                    var serviceKey = LastVisitTime.FirstKey;// GetEarlistVisitedTimePeriod();

                    var passedTime = DateTime.Now - (LastVisitTime[serviceKey]);
                    if (passedTime > MinExpireTime)
                    {
                        log.Info("移除缓存服务：" + typeof(TService).Name + ", " + serviceKey);
                        this.Remove(serviceKey);
                        LastVisitTime.Remove(serviceKey);
                    }
                }
            }
            base.Set(key, val);
        }
        ///// <summary>
        ///// 获取最早结束访问的时段
        ///// </summary>
        ///// <returns></returns>
        //public BufferedTimePeriod GetEarlistVisitedTimePeriod()
        //{
        //    KeyValuePair<BufferedTimePeriod, DateTime> kvs = default(KeyValuePair<BufferedTimePeriod, DateTime>);
        //    DateTime dateTime = DateTime.MaxValue;
        //    foreach (var kv in LastVisitTime)
        //    {
        //        if (dateTime < kv.Value)
        //        {
        //            dateTime = kv.Value;
        //            kvs = kv;
        //        }
        //    }

        //    return kvs.Key;
        //}


        static object serviceLocker = new object();
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Contains(Time time)
        {
            foreach (var item in this.Keys)
            {
                if (item.Contains(time)) return true;
            }
            return false;
        }


        /// <summary>
        /// 是否包含该时间
        /// </summary>
        /// <param name="time"></param> 
        /// <returns></returns>
        public TService GetService(Time time)
        {
            lock (serviceLocker)//此过程中，不要修改数据源
            {
                if (Current.Value != null && Current.Key.Contains(time))
                {
                    LastVisitTime[Current.Key] = DateTime.Now;

                    return Current.Value;
                }
                if (!IsCanBuffered) { return default(TService); }

                //var keys = this.Keys;
                foreach (var kv in this.data)
                {
                    if (kv.Key.BufferedContains(time))//为什么Buffered这么大！
                    {
                        Current = kv;
                        LastVisitTime[Current.Key] = DateTime.Now;

                        log.Debug("在 " + typeof(TService) + " 池中获取 "+ time + " 成功! ");
                        return kv.Value;
                    }
                }
                log.Debug("在 " + typeof(TService) + " 池中获取 " + time + " 失败! ");
                return default(TService);
            }
        }
    }



}