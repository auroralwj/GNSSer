//2018.03.15, czs, create in hmx,  IGS 星历服务提供器，提供无间断，多系统的星历。
//2018.05.02, czs, create in hmx, IGS 钟差服务提供器

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


    /// <summary>
    /// IGS 钟差服务提供器，提供无间断，多系统的星历。
    /// 自动匹配提供。
    /// </summary>
    public class IgsSimpleClockServiceAutoProvider : IgsServiceAutoProvider<ISimpleClockService>
    {
        protected Log log = new Log(typeof(IgsClockServiceAutoProvider));

        /// <summary>
        /// 默认构造函数
        /// </summary> 
        public IgsSimpleClockServiceAutoProvider()
        {  
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="opt"></param>
        public IgsSimpleClockServiceAutoProvider(IgsProductSourceOption opt):base(opt)
        { 
        } 

        /// <summary>
        /// 通过给定的时间创建服务
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        protected override ISimpleClockService CreateService(Time epoch)
        {
            var startLatency = IgsProductTimeAvailable.GetLatency(epoch);
            log.Info("当前历元"+ epoch +"可以获得IGS " + startLatency + " 产品");
            ISimpleClockService startService = null;
            switch (startLatency)
            {
                case IgsProductLatency.Final: 
                    IgsProductType igsProduct = IgsProductType.Clk;
                    if (DataSourceOption.IsUseClk30s)
                    {
                        igsProduct = IgsProductType.Clk_30s;
                    }
                    startService = new IgsSatSimpleClockSourceProvider(Option, igsProduct).GetDataSourceService();


                    if (DataSourceOption.IsUseClk30s && (startService == null || startService.TimePeriod.Span == 0))
                    {
                        igsProduct = IgsProductType.Clk;
                        log.Info("自动匹配钟差文件失败！ " + igsProduct + ", " + epoch + ", 尝试 " + igsProduct);
                        startService = new IgsSatSimpleClockSourceProvider(Option, igsProduct).GetDataSourceService();
                    }
                    break;
                case IgsProductLatency.Rapid:
                    startService = new IgsSatSimpleClockSourceProvider(Option, IgsProductType.igr_Clk).GetDataSourceService();
                    break;
                default:
                    log.Warn("由于时间太早，IGS提供的钟差在超快星历文件SP3中，忽略钟差文件的加载。。。");
                    startService = null;// new IgsSatClockSourceProvider(option, IgsProductType.igu_Clk).GetDataSourceService();
                    break;
            }
            //如果最终产品没有获取到，则尝试快速产品
            if ((startService == null || ((SimpelCombinedSatClockService)startService).SatelliteTypes.Count ==0) && startLatency == IgsProductLatency.Final)
            {
                log.Warn("自动钟差 最终产品 获取失败！ 尝试快速产品 " + epoch);
                startService = new IgsSatSimpleClockSourceProvider(Option, IgsProductType.igr_Clk).GetDataSourceService();
            }
            //最后尝试预报产品,没有单独的预报产品！！！！！！！
            if (startService == null)
            {
                log.Warn("自动钟差服务获取失败！"+ epoch);
               // startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igu_Sp3).GetDataSourceService();
            }

            return startService;
        }
         
    }
    




    /// <summary>
    /// IGS 钟差服务提供器，提供无间断，多系统的星历。
    /// 自动匹配提供。
    /// </summary>
    public class IgsClockServiceAutoProvider : IgsServiceAutoProvider<IClockService>
    {
        protected Log log = new Log(typeof(IgsClockServiceAutoProvider));

        /// <summary>
        /// 默认构造函数
        /// </summary> 
        public IgsClockServiceAutoProvider()
        {  
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="opt"></param>
        public IgsClockServiceAutoProvider(IgsProductSourceOption opt):base(opt)
        { 
        } 

        /// <summary>
        /// 通过给定的时间创建服务
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        protected override IClockService CreateService(Time epoch)
        {
            var startLatency = IgsProductTimeAvailable.GetLatency(epoch);
            log.Info("当前历元"+ epoch +"可以获得IGS " + startLatency + " 产品");
            IClockService startService = null;
            switch (startLatency)
            {
                case IgsProductLatency.Final: 
                    IgsProductType igsProduct = IgsProductType.Clk;
                    if (DataSourceOption.IsUseClk30s)
                    {
                        igsProduct = IgsProductType.Clk_30s;
                    }
                    startService = new IgsSatClockSourceProvider(Option, igsProduct).GetDataSourceService();


                    if (DataSourceOption.IsUseClk30s && (startService == null || startService.TimePeriod.Span == 0))
                    {
                        igsProduct = IgsProductType.Clk;
                        log.Info("自动匹配钟差文件失败！ " + igsProduct + ", " + epoch + ", 尝试 " + igsProduct);
                        startService = new IgsSatClockSourceProvider(Option, igsProduct).GetDataSourceService();
                    }
                    break;
                case IgsProductLatency.Rapid:
                    startService = new IgsSatClockSourceProvider(Option, IgsProductType.igr_Clk).GetDataSourceService();
                    break;
                default:
                    log.Warn("由于时间太早，IGS提供的钟差在超快星历文件SP3中，忽略钟差文件的加载。。。");
                    startService = null;// new IgsSatClockSourceProvider(option, IgsProductType.igu_Clk).GetDataSourceService();
                    break;
            }
            //如果最终产品没有获取到，则尝试快速产品
            if (startService == null && startLatency == IgsProductLatency.Final)
            {
                log.Warn("自动钟差 最终产品 获取失败！ 尝试快速产品 " + epoch);
                startService = new IgsSatClockSourceProvider(Option, IgsProductType.igr_Clk).GetDataSourceService();
            }
            //最后尝试预报产品,没有单独的预报产品！！！！！！！
            if (startService == null)
            {
                log.Warn("自动钟差服务获取失败！"+ epoch);
               // startService = new IgsEphemerisSourceProvider(Option, IgsProductType.igu_Sp3).GetDataSourceService();
            }

            return startService;
        }
         
    }


}
