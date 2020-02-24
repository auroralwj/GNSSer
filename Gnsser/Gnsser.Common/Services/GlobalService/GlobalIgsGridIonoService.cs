//2018.05.03, czs, create in hmx, 全局自动格网电离层服务
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
    /// 电离层服务类型
    /// </summary>
    public enum IonoSerivceType
    {
        /// <summary>
        /// IGS 格网电离层
        /// </summary>
        IgsGrid,
        /// <summary>
        /// CODE 球谐函数模型
        /// </summary>
        SphericalHarmonics,
        /// <summary>
        /// GPS 导航 Klobuchar
        /// </summary>
        GpsKlobuchar
    }



    /// <summary>
    /// 全局自动格网电离层服务。
    /// </summary>
    public class GlobalIgsGridIonoService : IGridIonoFileService
    {
        Log log = new Log(typeof(GlobalIgsGridIonoService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalIgsGridIonoService()
        {
            this.Name = "全局自动格网电离层服务";
            ServiceHolder = new IgsGridIonoServiceHolder();
        }
        static GlobalIgsGridIonoService instance = new GlobalIgsGridIonoService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalIgsGridIonoService Instance
        {
            get => instance;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否忽略体文件的读取
        /// </summary>
        public bool IsSkipContent { get; set; }
        /// <summary>
        /// 服务的时段信息
        /// </summary>
        public  BufferedTimePeriod TimePeriod { get { return BufferedTimePeriod.MaxPeriod; }set { } }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="DataSourceOption"></param>
        public void Init(GnsserConfig DataSourceOption)
        {
            this.GnsserConfig = DataSourceOption;
            igClkAutoProvider = new IgsGridIonoServiceAutoProvider() { DataSourceOption = DataSourceOption };
        }
        IGridIonoFileService CurrentService;
        static object locker = new object();

        /// <summary>
        /// 获取当天测站DCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public RmsedNumeral GetDcb(Time time, string name)
        {
            igClkAutoProvider.DataSourceOption.IsSkipIonoContent = true;

            var services = GetService(time);
            if(services == null)
            {
                return RmsedNumeral.Zero;
            }

            return services.GetDcb(time, name);
        }

        /// <summary>
        /// 获取当天卫星DCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public RmsedNumeral GetDcb(Time time, SatelliteNumber prn)
        {
            igClkAutoProvider.DataSourceOption.IsSkipIonoContent = true;

            var services = GetService(time);
            if (services == null)
            {
                return RmsedNumeral.Zero;
            }
            return services.GetDcb(time, prn); 
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geocentricLonlatDeg"></param>
        /// <returns></returns>
        public RmsedNumeral Get(Time time, LonLat geocentricLonlatDeg)
        {
            igClkAutoProvider.DataSourceOption.IsSkipIonoContent = false;
            var services = GetService(time);
            if (services == null)
            {
                return RmsedNumeral.Zero;
            }
            return services.Get(time, geocentricLonlatDeg); 
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IGridIonoFileService GetService(Time time)
        {
            IGridIonoFileService services = null;
          
            services = ServiceHolder.GetService(time);
            if (services == null && !ServiceHolder.Contains(time))
            {
                lock (locker)
                {
                    var latency = IgsProductTimeAvailable.GetLatency(time);
                    if (latency != IgsProductLatency.Final)
                    {
                        log.Warn("当前无法获得IGS格网电离层产品,将不再尝试 " + latency); 
                    }
                    else
                    {
                        services = igClkAutoProvider.GetService(time);
                    }
                    //记录
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
        protected void SetEmptyServiceWithinSomeHours(Time time, IGridIonoFileService services)
        {
            //格网电离层按照天计算
            var tp = new BufferedTimePeriod(time.Date, time.Date + TimeSpan.FromDays(1));
            log.Warn(this.Name + " 获取失败，今天不再获取。" + tp);
            ServiceHolder[tp] = services;
        }

        IgsGridIonoServiceAutoProvider igClkAutoProvider { get; set; }

        IgsGridIonoServiceHolder ServiceHolder { get; set; }
        /// <summary>
        /// 获取倾斜电离层电子数
        /// </summary>
        /// <param name="receiverTime"></param>
        /// <param name="geocentricLonLat"></param>
        /// <param name="elevation"></param>
        /// <returns></returns>
        public RmsedNumeral GetSlope(Time receiverTime, LonLat geocentricLonLat, double elevation)
        {
            igClkAutoProvider.DataSourceOption.IsSkipIonoContent = false;
            var service = GetService(receiverTime);
            if(service == null) { return RmsedNumeral.Zero; }
            return service.GetSlope(receiverTime, geocentricLonLat, elevation);
        }
        /// <summary>
        /// 数据源选项
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }
        /// <summary>
        /// 模型高度
        /// </summary>
        public double HeightOfModel { get { return CurrentService ==null? 450000 : CurrentService. HeightOfModel; } }
        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name + ", " + TimePeriod;
        }
        /// <summary>
        /// 获取斜距对应的伪距的电离层延迟误差
        /// </summary>
        /// <param name="time"></param>
        /// <param name="siteXyz"></param>
        /// <param name="satXyz"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public double GetSlopeDelayRange(Time time, XYZ siteXyz, XYZ satXyz, double freq)
        {
            igClkAutoProvider.DataSourceOption.IsSkipIonoContent = false;
            var services = GetService(time);
            if (services == null)
            {
                return 0;
            }
            return services.GetSlopeDelayRange(time, siteXyz, satXyz, freq);
        }

        public RmsedNumeral GetSlope(Time time, XYZ siteXyz, XYZ satXyz)
        {
            igClkAutoProvider.DataSourceOption.IsSkipIonoContent = false;

            var services = GetService(time);
            if (services == null)
            {
                return RmsedNumeral.Zero;
            }
            return services.GetSlope(time, siteXyz, satXyz);
        }
    }



}