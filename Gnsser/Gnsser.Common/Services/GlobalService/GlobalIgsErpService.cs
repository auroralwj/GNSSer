//2018.05.02, czs, create in hmx, 全局自动ERP服务
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
    /// 全局自动ERP服务。
    /// </summary>
    public class GlobalIgsErpService : AbstractErpService
    {
        Log log = new Log(typeof(GlobalIgsEphemerisService));

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private GlobalIgsErpService()
        {
            this.Name = "全局自动 IGS ERP 服务";
            ServiceHolder = new ErpServiceHolder();
        }
        static GlobalIgsErpService instance = new GlobalIgsErpService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalIgsErpService Instance
        {
            get => instance;
        }

        /// <summary>
        /// 服务的时段信息
        /// </summary>
        public override BufferedTimePeriod TimePeriod { get { return BufferedTimePeriod.MaxPeriod; } set { } }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="DataSourceOption"></param>
        public void Init(GnsserConfig DataSourceOption)
        {
            this.GnsserConfig = DataSourceOption;
            igsErpAutoProvider = new IgsErpServiceAutoProvider() { DataSourceOption = DataSourceOption };
        }
        static object locker = new object();
        public override ErpItem Get(Time time)
        {
            var services = ServiceHolder.GetService(time);
            if (services == null)
            {  
                lock (locker)
                {
                    if (services == null)
                    {
                        services = igsErpAutoProvider.GetService(time );
                        if (services == null)
                        {
                            SetEmptyServiceWithinSomeHours(time);
                            return null;
                        }
                        else
                        {
                            ServiceHolder[services.TimePeriod] = services;
                        }
                    }
                }
            } 

            return services.Get(time);
        }
        /// <summary>
        /// 设置在一定时间内，不再访问
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        protected void SetEmptyServiceWithinSomeHours( Time time)
        {
            var tp = new BufferedTimePeriod(time - TimeSpan.FromHours(1), time + TimeSpan.FromHours(this.GnsserConfig.RetryHoursWhenFailedForIgsDownload));
            log.Warn(this.Name + " 获取失败，" + this.GnsserConfig.RetryHoursWhenFailedForIgsDownload + " 小时不再获取。" + tp);
            ServiceHolder[tp] = Gnsser.Data.FileErpService.Empty;
        }

        IgsErpServiceAutoProvider igsErpAutoProvider { get; set; }

        ErpServiceHolder ServiceHolder { get; set; }

        /// <summary>
        /// 数据源选项
        /// </summary>
        public GnsserConfig GnsserConfig { get; set; }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ", " + this.TimePeriod.ToString();
        }

    }



}