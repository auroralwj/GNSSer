//2017.05.03, czs, create in 洪庆, 星历数据源提供适配器。
//2018.03.15, czs, edit in hmx, 重新设计和封装IGS星历获取,考虑超快星历，考虑多系统
//2018.05.02, czs, edit in hmx, 加入全局自动钟差服务

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
    /// 钟差数据源提供适配器。
    /// 提供某一会话的星历服务。
    /// </summary>
    public class SimpleClockServiceProvider : AbstractServiceProvider<ISimpleClockService>
    {
        ILog log = new Log(typeof(ClockServiceProvider));

        /// <summary>
        /// 构造函数.星历数据源提供适配器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="processOption"></param> 
        public SimpleClockServiceProvider(GnsserConfig option, GnssProcessOption processOption) : base(option, processOption)
        {
        }


        /// <summary>
        /// 获取服务，根据设置自动判断。
        /// </summary> 
        /// <returns></returns>
        public override ISimpleClockService GetService()
        {
            //钟差
            if (this.CurrentService == null && ProcessOption.IsPreciseClockFileRequired)
            {

                if (ProcessOption.IsIndicatingClockFile)
                {
                    this.CurrentService = new SimpleClockService(ProcessOption.ClockFilePath);
                    log.Info("成功载入选项指定钟差文件 " + ProcessOption.ClockFilePath);
                }
                else if (!DataSourceOption.EnableAutoFindingFile)
                {
                    var msg = "没有手动设置钟差文件,也没有启用文件自动匹配功能。";
                    log.Info(msg);
                    // throw new ArgumentException(msg);
                }
                else
                {
                    // this.ClockService = new IgsSatClockSourceProvider(DataSourceOption.GetIgsProductSourceOption()).GetDataSourceService();
                    this.CurrentService = GlobalIgsSimpleClockService.Instance;
                    // new IgsEphemerisServiceAutoProvider(DataSourceOption.GetIgsProductSourceOption(CurrentSession)).GetEphemerisService();

                    if (this.CurrentService == null || CurrentService.TimePeriod.Span == 0) { log.Info("没有自动匹配上星历文件 " + CurrentService); }
                    else
                    {
                        log.Info("成功自动匹配钟差文件 " + CurrentService);
                        return CurrentService;
                    }
                }
            }

            return this.CurrentService;
        }


    }







    /// <summary>
    /// 钟差数据源提供适配器。
    /// 提供某一会话的星历服务。
    /// </summary>
    public class ClockServiceProvider : AbstractServiceProvider<IClockService>
    {
        ILog log = new Log(typeof(ClockServiceProvider));

        /// <summary>
        /// 构造函数.星历数据源提供适配器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="processOption"></param> 
        public ClockServiceProvider(GnsserConfig option, GnssProcessOption processOption) : base(option, processOption)
        {
        }


        /// <summary>
        /// 获取服务，根据设置自动判断。
        /// </summary> 
        /// <returns></returns>
        public override IClockService GetService()
        {
            //钟差
            if (this.CurrentService == null && ProcessOption.IsPreciseClockFileRequired)
            {

                if (ProcessOption.IsIndicatingClockFile)
                {
                    this.CurrentService = new ClockService(ProcessOption.ClockFilePath);
                    log.Info("成功载入选项指定钟差文件 " + ProcessOption.ClockFilePath);
                }
                else if (!DataSourceOption.EnableAutoFindingFile)
                {
                    var msg = "没有手动设置钟差文件,也没有启用文件自动匹配功能。";
                    log.Info(msg);
                    // throw new ArgumentException(msg);
                }
                else
                {
                    // this.ClockService = new IgsSatClockSourceProvider(DataSourceOption.GetIgsProductSourceOption()).GetDataSourceService();
                    this.CurrentService = GlobalIgsClockService.Instance;
                    // new IgsEphemerisServiceAutoProvider(DataSourceOption.GetIgsProductSourceOption(CurrentSession)).GetEphemerisService();

                    if (this.CurrentService == null || CurrentService.TimePeriod.Span == 0) { log.Info("没有自动匹配上星历文件 " + CurrentService); }
                    else
                    {
                        log.Info("成功自动匹配钟差文件 " + CurrentService);
                        return CurrentService;
                    }
                }
            }

            return this.CurrentService;
        }


    }


}

