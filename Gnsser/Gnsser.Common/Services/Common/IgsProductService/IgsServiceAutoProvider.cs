//2018.03.15, czs, create in hmx,  IGS 星历服务提供器，提供无间断，多系统的星历。
//2018.05.02, czs, create in hmx, 提取抽象IGS服务提供者

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Common;
using Gnsser.Service;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times; 
using Geo.IO;
using System.IO;


namespace Gnsser.Data
{
    //2018.05.02, czs, create in hmx, 提取抽象IGS服务提供者
    /// <summary>
    /// 抽象IGS服务提供者
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public abstract class IgsServiceAutoProvider<TService> where TService : ITimePeriod<BufferedTimePeriod>
    {
        protected Log log = new Log(typeof(IgsServiceAutoProvider<TService> ));

        /// <summary>
        /// 默认构造函数
        /// </summary> 
        public IgsServiceAutoProvider()
        {
            Name = typeof(TService).Name;
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="opt"></param>
        public IgsServiceAutoProvider(IgsProductSourceOption opt)
        {
            this.Option = opt;
            Name = typeof(TService).Name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 一些额外信息的获取
        /// </summary>
       public  GnsserConfig DataSourceOption { get; set; }

        /// <summary>
        /// 数据源选项
        /// </summary>
        protected IgsProductSourceOption Option { get; set; }
        /// <summary>
        /// 卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get { return Option.SatelliteTypes; } }


        /// <summary>
        /// 获取星历
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        public TService GetService(IgsProductSourceOption Option)
        {
            this.Option = Option;
            return GetService();
        }
        /// <summary>
        /// 获取星历
        /// </summary>
        /// <param name="time"></param>
        /// <param name="SatelliteTypes"></param>
        /// <returns></returns>
        public TService GetService(Time time, List<SatelliteType> SatelliteTypes)
        {
            //若当前时间在两日交界处，30s内，如2018-01-01 23:59:30 ，则考虑下一日
            if(time.SecondsOfDay  > 86400 - 30)
            {
                time = time + 30;
            }
            //时间修改为0时，和下一日的0时
            TimePeriod timePeriod = new TimePeriod(time.Date, time.Date + TimeSpan.FromDays(1));
            SessionInfo session = new SessionInfo(timePeriod, SatelliteTypes);
            return GetService(session);
        }
        /// <summary>
        /// 获取星历
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public TService GetService(SessionInfo session)
        {
            this.Option = Setting.GnsserConfig.GetIgsProductSourceOption(session);
            return GetService();
        }
        /// <summary>
        /// 获取星历,与系统无关
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TService GetService(Time time)
        {
            SessionInfo session = new SessionInfo(time, new List<SatelliteType>());
            this.Option = Setting.GnsserConfig.GetIgsProductSourceOption(session);
            return GetService();
        }

        /// <summary>
        /// 获取星历
        /// </summary>
        /// <returns></returns>
        public TService GetService()
        {
            var epoch = Option.TimePeriod.Start;
            TService startService = CreateService(epoch);

            if(startService == null)
            { 
                log.Warn("不包含该时间的 " + this.Name + " 服务！" + epoch);
                return startService;
            }
            if (startService.TimePeriod.Contains(Option.TimePeriod.End))
            {
                return startService;
            }
            else
            {
                log.Warn("不包含结束时间的 "+this.Name+" 服务！" + Option.TimePeriod.End);
            }
            return startService;
        }
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        protected abstract TService CreateService(Time epoch);
    }

}