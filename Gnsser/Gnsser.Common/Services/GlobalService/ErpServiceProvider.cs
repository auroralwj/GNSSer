//2017.05.03, czs, create in 洪庆, 星历数据源提供适配器。
//2018.03.15, czs, edit in hmx, 重新设计和封装IGS星历获取,考虑超快星历，考虑多系统
//2018.05.02, czs, edit in hmx, 加入全局自动ERP服务

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
    /// ERP数据源提供适配器。
    /// 提供某一会话的 ERP 服务。
    /// </summary>
    public class ErpServiceProvider: AbstractServiceProvider<IErpFileService>
    {
        ILog log = new Log(typeof(ClockServiceProvider));

        /// <summary>
        /// 构造函数.星历数据源提供适配器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="processOption"></param> 
        public ErpServiceProvider(GnsserConfig option, GnssProcessOption processOption) : base(option, processOption)
        {
        }


        /// <summary>
        /// 获取服务，根据设置自动判断。
        /// </summary> 
        /// <returns></returns>
        public override IErpFileService GetService()
        {
            //ERP
            if (this.CurrentService == null)
            {

                if (ProcessOption.IsIndicatingErpFile)
                {
                    this.CurrentService = new FileErpService(ProcessOption.ErpFilePath);
                    log.Info("成功载入选项指定ERP文件 " + ProcessOption.ErpFilePath);
                }
                else if (!DataSourceOption.EnableAutoFindingFile)
                {
                    var msg = "没有手动设置ERP文件,也没有启用文件自动匹配功能。";
                    log.Info(msg);
                    // throw new ArgumentException(msg);
                }
                else
                {
                    // this.ClockService = new IgsSatClockSourceProvider(DataSourceOption.GetIgsProductSourceOption()).GetDataSourceService();
                    this.CurrentService = GlobalIgsErpService.Instance;
                    // new IgsEphemerisServiceAutoProvider(DataSourceOption.GetIgsProductSourceOption(CurrentSession)).GetEphemerisService();

                    if (this.CurrentService == null || CurrentService.TimePeriod.Span == 0) { log.Info("没有自动匹配上 ERP 文件 " + CurrentService); }
                    else
                    {
                        log.Info("成功自动匹配 ERP 文件 " + CurrentService);
                        return CurrentService;
                    }
                }
            }

            return this.CurrentService;
        }


    }
}

