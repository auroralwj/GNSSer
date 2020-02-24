//2018.05.03, czs, create in hmx, 全局自动格网电离层服务
//2018.05.16, czs, edit in hmx, 修改为DCB服务，不必读取内容
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
    /// 全局自动格网电离层DCB服务，不必读取内容
    /// </summary>
    public class GlobalIgsGridIonoDcbService : IGridIonoFileService
    {
        Log log = new Log(typeof(GlobalIgsGridIonoService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalIgsGridIonoDcbService()
        {
            this.Name = "全局自动格网电离层DCB服务";
            ServiceHolder = new IgsGridIonoServiceHolder();
        }
        static GlobalIgsGridIonoDcbService instance = new GlobalIgsGridIonoDcbService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalIgsGridIonoDcbService Instance
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
            AutoProvider = new IgsGridIonoServiceAutoProvider() { DataSourceOption = DataSourceOption };
        }
        IGridIonoFileService CurrentService;
        static object locker = new object();
        /// <summary>
        /// 获取DCB 对于P1的距离改正。
        /// </summary>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetDcbMeterForP1(Time time, string name)
        {
            var dcb = GetDcb(time, name);
            if (dcb == null) { return 0; }

            double f1f1 = (GnssConst.GPS_FREQUENCY_L1 * GnssConst.GPS_FREQUENCY_L1);
            double f2f2 = (GnssConst.GPS_FREQUENCY_L2 * GnssConst.GPS_FREQUENCY_L2);

            double k2 = -1.0 * f2f2 / (f1f1 - f2f2);

            double val = -1.0 * k2 * dcb.Value * GnssConst.MeterPerNano;

            return val;
        }
        /// <summary>
        /// 获取DCB 对于P1的距离改正。
        /// </summary>
        /// <param name="time"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public double GetDcbMeterForP1(Time time, SatelliteNumber prn)
        {
            var dcb = GetDcb(time, prn);
            if (dcb == null) { return 0; }

            double f1f1 = (GnssConst.GPS_FREQUENCY_L1 * GnssConst.GPS_FREQUENCY_L1);
            double f2f2 = (GnssConst.GPS_FREQUENCY_L2 * GnssConst.GPS_FREQUENCY_L2);

            double k2 = -1.0 * f2f2 / (f1f1 - f2f2);

            double val = -1.0 * k2 * dcb.Value * GnssConst.MeterPerNano;

            return val;
        }

        /// <summary>
        /// 获取当天测站DCB
        /// </summary>
        /// <param name="time"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public RmsedNumeral GetDcb(Time time, string name)
        {
            AutoProvider.DataSourceOption.IsSkipIonoContent = true;

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
            AutoProvider.DataSourceOption.IsSkipIonoContent = true;

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
        /// <param name="geocentricLonlatDeg">地心经纬度，球坐标，单位度</param>
        /// <returns></returns>
        public RmsedNumeral Get(Time time, LonLat geocentricLonlatDeg)
        {
            AutoProvider.DataSourceOption.IsSkipIonoContent = false;
            var services = GetService(time); 

            return services.Get(time, geocentricLonlatDeg); 
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IGridIonoFileService GetService(Time time)
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
        protected void SetEmptyServiceWithinSomeHours(Time time, IGridIonoFileService services)
        {
            var tp = new BufferedTimePeriod(time, time + TimeSpan.FromHours(this.GnsserConfig.RetryHoursWhenFailedForIgsDownload));
            log.Warn(this.Name + " 获取失败，" + this.GnsserConfig.RetryHoursWhenFailedForIgsDownload + " 小时不再获取。" + tp);
            ServiceHolder[tp] = services;
        }


        IgsGridIonoServiceAutoProvider AutoProvider { get; set; }

        IgsGridIonoServiceHolder ServiceHolder { get; set; }

        /// <summary>
        /// 获取倾斜延迟距离
        /// </summary>
        /// <param name="time">历元</param>
        /// <param name="siteXyz">测站坐标</param>
        /// <param name="satXyz">卫星坐标</param>
        /// <param name="freq">频率，单位 10^6</param>
        /// <returns></returns>
        public double GetSlopeDelayRange(Time time, XYZ siteXyz, XYZ satXyz, double freq)
        {
            var tec = GetSlope(time, siteXyz, satXyz);
            return GetIonoDelayRange(tec.Value, freq);
        }
        /// <summary>
        /// 获取倾斜电离层电子数
        /// </summary>
        /// <param name="time">历元</param>
        /// <param name="siteXyz">测站坐标</param>
        /// <param name="satXyz">卫星坐标</param>
        /// <returns></returns>
        public RmsedNumeral GetSlope(Time time, XYZ siteXyz, XYZ satXyz)
        {
            var punctPoint = XyzUtil.GetIntersectionXyz(siteXyz, satXyz, HeightOfModel);
            var geocentricLonLat = Geo.Coordinates.CoordTransformer.XyzToSphere(punctPoint);
            var SpherePolar = CoordTransformer.XyzToSpherePolar(satXyz, siteXyz, AngleUnit.Degree);

            var tec = GetSlope(time, geocentricLonLat, SpherePolar.Elevation);
            return tec;
        }

        /// <summary>
        /// 电离层对于伪距的延迟距离，若是载波则需要换符号。
        /// </summary>
        /// <param name="tec">电子数量，单位 1e16.</param>
        /// <param name="freq">频率，单位 10^6</param>
        /// <returns></returns>
        public static double GetIonoDelayRange(double tec, double freq)
        {
            return tec * 40.28 / (freq * freq) * 1e4;//斜方向延迟             
        }

        /// <summary>
        /// 获取倾斜电离层电子数
        /// </summary>
        /// <param name="receiverTime">历元</param>
        /// <param name="geocentricLonLat">地心经纬度</param>
        /// <param name="elevation">测站卫星地心高度角</param>
        /// <returns></returns>
        public RmsedNumeral GetSlope(Time receiverTime, LonLat geocentricLonLat, double elevation)
        {
            AutoProvider.DataSourceOption.IsSkipIonoContent = false;

            return  GetService(receiverTime).GetSlope(receiverTime, geocentricLonLat, elevation);
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
    }



}