//2018.05.27, czs, create in hmx, 全局自动CODE球谐函数电离层服务
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
    /// 全局自动CODE球谐函数电离层服务。
    /// </summary>
    public class GlobalCodeHarmoIonoService : IIonoService
    {
        Log log = new Log(typeof(GlobalCodeHarmoIonoService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalCodeHarmoIonoService()
        {
            this.Name = "全局自动 CODE 球谐函数电离层服务";
            ServiceHolder = new GlobalCodeHarmoIonoServiceHolder();
        }
        static GlobalCodeHarmoIonoService instance = new GlobalCodeHarmoIonoService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalCodeHarmoIonoService Instance
        {
            get => instance;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } 
        /// <summary>
        /// 服务的时段信息
        /// </summary>
        public  BufferedTimePeriod TimePeriod { get { return BufferedTimePeriod.MaxPeriod; }set { } }

        IgsCodeHarmoIonoServiceAutoProvider AutoProvider { get; set; }

        GlobalCodeHarmoIonoServiceHolder ServiceHolder { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="DataSourceOption"></param>
        public void Init(GnsserConfig DataSourceOption)
        {
            this.GnsserConfig = DataSourceOption;
            AutoProvider = new IgsCodeHarmoIonoServiceAutoProvider() { DataSourceOption = DataSourceOption };
        }
        IIonoService CurrentService;
        static object locker = new object();

  
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geocentricLonlatDeg"></param>
        /// <returns></returns>
        public RmsedNumeral Get(Time time, LonLat geocentricLonlatDeg)
        {
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
        public IIonoService GetService(Time time)
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
            protected void SetEmptyServiceWithinSomeHours(Time time, IIonoService services)
            {
                var tp = new BufferedTimePeriod(time, time + TimeSpan.FromHours(this.GnsserConfig.RetryHoursWhenFailedForIgsDownload));
                log.Warn(this.Name + " 获取失败，" + this.GnsserConfig.RetryHoursWhenFailedForIgsDownload + " 小时不再获取。" + tp);
                ServiceHolder[tp] = services;
            }


            /// <summary>
            /// 获取倾斜电离层电子数
            /// </summary>
            /// <param name="receiverTime"></param>
            /// <param name="geocentricLonLat"></param>
            /// <param name="elevation"></param>
            /// <returns></returns>
            public RmsedNumeral GetSlope(Time receiverTime, LonLat geocentricLonLat, double elevation)
        {
            var services = GetService(receiverTime);
            if (services == null)
            {
                return RmsedNumeral.Zero;
            }
            return services.GetSlope(receiverTime, geocentricLonLat, elevation);
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
            var services = GetService(time);
            if (services == null)
            {
                return 0;
            }
            return services.GetSlopeDelayRange(time, siteXyz, satXyz, freq);
        }

        public RmsedNumeral GetSlope(Time time, XYZ siteXyz, XYZ satXyz)
        {
            var services = GetService(time);
            if (services == null)
            {
                return RmsedNumeral.Zero;
            }
            return services.GetSlope(time, siteXyz, satXyz);
        }
    }



}