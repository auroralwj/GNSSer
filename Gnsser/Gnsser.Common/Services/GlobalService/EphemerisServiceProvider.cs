//2017.05.03, czs, create in 洪庆, 星历数据源提供适配器。
//2018.03.15, czs, edit in hmx, 重新设计和封装IGS星历获取,考虑超快星历，考虑多系统
//2018.04.27, czs, create in hmx, 配合Option配置，增加指定路径的多系统星历服务，
//2018.05.02, czs, create in hmx, 提取抽象IGS服务提供者

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Times;
using Geo.Coordinates;
using Geo.IO;
using Gnsser.Data;
using Gnsser.Service;
using Gnsser.Times;
using Gnsser.Correction;

namespace Gnsser
{ 


    /**
     * 星历加载逻辑
    1、先判断指定星历是否可用，指定了或可用，则直接加载返回
    2、查找本地星历
    2.1若只需要导航星历，
    2.1.1则先查找本地导航星历，
    2.1.2如果没有找到，则转入步骤3；
    2.2若需要精密星历，
    2.2.1查找本地精密星历；
    2.2.2若没有找到，则转入步骤3
    3.查找网络星历
    3.1若只需要导航星历，
    3.1.1则联网下载导航星历
    3.1.2若不可联网或联网查找失败，则以精密星历代替
    3.1.3若精密星历也不可用，则返回null或抛出异常。
    3.2若需要精密星历，
    3.2.1则联网下载精密星历
    2.2.3若不可联网或联网查找失败，则返回null或抛出异常。
     * */

    /// <summary>
    /// 星历数据源提供适配器。
    /// </summary>
    public class EphemerisServiceProvider : AbstractServiceProvider<IEphemerisService>
    {
        ILog log = new Log(typeof(EphemerisServiceProvider));

        /// <summary>
        /// 构造函数.星历数据源提供适配器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="processOption"></param>
        /// <param name="ObservationDataSources"></param>
        public EphemerisServiceProvider(GnsserConfig option, GnssProcessOption processOption, MultiSiteObsStream ObservationDataSources = null)
            :base(option, processOption)
        {
            this.ObservationDataSources = ObservationDataSources; 
            this.CurrentSession = new SessionInfo(ObservationDataSources.TimePeriod, processOption.SatelliteTypes);
        }

        #region 属性 
        /// <summary>
        /// 当前会话
        /// </summary>
        public SessionInfo CurrentSession { get; set; }
        /// <summary>
        /// 观测数据源，是一个集合
        /// </summary>
        public MultiSiteObsStream ObservationDataSources { get; set; } 
        #endregion

        #region  方法



        /// <summary>
        /// 获取服务，根据设置自动判断。
        /// </summary> 
        /// <returns></returns>
        public override IEphemerisService GetService()
        {
            //检查当前，避免重复加载
            if (IsCurrentEphemerisServiceAvailable())
            {
                log.Info("返回已有星历服务 " + CurrentService);
                return CurrentService;
            }

            if (ProcessOption.IsUseUniqueEphemerisFile)
            {
                this.CurrentService =  EphemerisDataSourceFactory.Create(ProcessOption.EphemerisFilePath);      
                log.Info("成功载入选项指定星历文件:  " + ProcessOption.EphemerisFilePath);
                return CurrentService;
            }

            if (ProcessOption.IsIndicatedEphemerisPathes) //优先指定的星历文件
            {
                var ephePathes = ProcessOption.GetIndicatedEphemerisPathes();

                this.CurrentService = new IndicatedEphemerisService(ephePathes, ProcessOption, DataSourceOption.GetIgsProductSourceOption(CurrentSession)) ;
                //EphemerisDataSourceFactory.Create(ProcessOption.EphemerisFilePath);
                StringBuilder sb = new StringBuilder();
                foreach (var item in ephePathes)
                {
                    sb.AppendLine(item.Key + ":" + item.Value);
                }
                log.Info("成功载入选项指定星历文件:\r\n " + sb.ToString());
                return CurrentService;
            }

            

            //如果不需要精密星历，且数据文件夹中有导航文件，则加载之
            if (ObservationDataSources != null && !this.ProcessOption.IsPreciseEphemerisFileRequired)
            {
                ObservationDataSources.TryLoadEphemerisServices();
                if (ObservationDataSources.EphemerisServices.Count > 0)
                {
                    this.CurrentService = ObservationDataSources.EphemerisServices.First;

                    var msg = "设置没有指定星历，也不需要精密星历，发现了导航星历，因此采用此一个导航星历。" + this.CurrentService;
                    log.Info(msg);
                    return CurrentService;
                }
            }

            //自动匹配精密星历
            if (DataSourceOption.EnableAutoFindingFile)
            {
                // new IgsEphemerisServiceAutoProvider(DataSourceOption.GetIgsProductSourceOption(CurrentSession)).GetEphemerisService();
                GlobalIgsEphemerisService.Instance.IgsEphAutoProvider.IsConnectIgsProduct = this.ProcessOption.IsConnectIgsDailyProduct;

                //尝试获取一个，如果失败则表示不可用  
                if (CurrentSession.SatelliteTypes.Count == 0)
                {
                    log.Error("请指定GNSS系统类型");
                }else if (GlobalIgsEphemerisService.Instance.IsAvailable(this.CurrentSession.TimePeriod.Start, this.CurrentSession.SatelliteTypes[0]))
                {
                    this.CurrentService = GlobalIgsEphemerisService.Instance;

                    if (this.CurrentService == null || CurrentService.SatCount == 0) { log.Info("没有匹配上星历文件 " + CurrentService); }
                    else
                    {
                        //设置拟合阶次，放在此处不是很合理，2019.05.15，czs
                        GlobalIgsEphemerisService.Instance.IgsEphAutoProvider.InerpolateOrder = this.ProcessOption.EphemerisInterpolationOrder;

                        log.Info("成功自动匹配星历文件 " + CurrentService);
                        return CurrentService;
                    }
                }
            }
            //自动匹配精密星历
            //if (DataSourceOption.EnableAutoFindingFile)
            //{
            //    this.EphemerisService = new IgsEphemerisServiceAutoProvider(DataSourceOption.GetIgsProductSourceOption(CurrentSession)).GetEphemerisService();

            //    if (this.EphemerisService == null || EphemerisService.SatCount == 0) { log.Info("没有匹配上星历文件 " + EphemerisService); }
            //    else
            //    {
            //        log.Info("成功自动匹配星历文件 " + EphemerisService);
            //        return EphemerisService;
            //    }
            //}

            //NGA星历匹配
            if (ProcessOption.IsEnableNgaEphemerisSource && DataSourceOption.EnableAutoFindingFile && Setting.EnableNet)
            {
                Time obsTime = CurrentSession.TimePeriod.Start;
                Time time = Time.UtcNow;
                var span = (time - obsTime);//与当前的时间差
                var isWithTwoDays = span < 48 * 3600;//IGS快速星历服务 
                                                     //NGA只有预报星历，取消之。应急采用。
                TryAddNgaEphemeris(obsTime, isWithTwoDays);
                if (this.CurrentService == null || CurrentService.SatCount == 0) { log.Info("没有匹配上星历文件 " + CurrentService); }
                else
                {
                    log.Info("成功自动匹配 NGA 星历文件 " + CurrentService);
                    return CurrentService;
                }
            }


            //还没有匹配上，寻找导航文件
            if ((CurrentService == null || CurrentService.SatCount == 0))
            {
                ObservationDataSources.TryLoadEphemerisServices();
                if (ObservationDataSources.EphemerisServices.Count > 0)
                {
                    this.CurrentService = ObservationDataSources.EphemerisServices.First;

                    var msg = "最后时刻，采用了一个导航星历。" + this.CurrentService;
                    log.Info(msg);
                }
            }

            if ((CurrentService == null || CurrentService.SatCount == 0) )
            {
                this.CurrentService = GlobalIgsEphemerisService.Instance;
                var msg = "星历服务匹配失败！！！，仍然尝试用全局星历！";
                log.Error(msg);
           //     throw new ArgumentException(msg);
            }

            return this.CurrentService;
        }

        /// <summary>
        /// 尝试加载NGA星历
        /// </summary>
        /// <param name="obsTime"></param>
        /// <param name="isWithTwoDays"></param>
        public void TryAddNgaEphemeris(Time obsTime, bool isWithTwoDays)
        {
            //NGA 预报星历
            if (ObservationDataSources != null && (CurrentService == null || CurrentService.SatCount == 0))
            {

                if (isWithTwoDays)//两天内
                {
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("p");
                    sb.Append("NGA");
                    sb.Append(obsTime.GpsWeek);
                    sb.Append((int)obsTime.DayOfWeek);
                    sb.Append("1");
                    sb.Append(".sp3");
                    var fileName = sb.ToString();

                    string localDirectory = Setting.GnsserConfig.Nga9DayGPSpredsDirectory;
                    //首先检查本地是否已有星历，或者今天(24小时内)已经下载过
                    var fs = Directory.GetFiles(localDirectory, "*.sp3");
                    foreach (var item in fs)
                    {
                        if (item.Contains(fileName) && new FileInfo(item).CreationTimeUtc > (DateTime.UtcNow - TimeSpan.FromDays(1)))
                        {
                            this.CurrentService = EphemerisDataSourceFactory.Create(item);
                            var msg = "采用了NGA预报星历。" + this.CurrentService;
                            log.Info(msg);
                            break;
                        }
                    }

                    if (isWithTwoDays && (CurrentService == null || CurrentService.SatCount == 0))
                    {       //每天UTC正午12点前更新当天和后8天的数据
                        string url = "ftp://ftp.nga.mil/pub2/gps/predictions/9DayGPSpreds/";
                        var files = Geo.Utils.NetUtil.DownloadFtpDirecotryOrFile(url, "*.*", localDirectory, "anonymous", "");
                        var sp3s = new List<string>();
                        foreach (var item in files)
                        {
                            if (!File.Exists(item)) { continue; }
                            var destPath = item + ".sp3";
                            Geo.Utils.FileUtil.MoveFile(item, destPath, true);
                            sp3s.Add(destPath);
                        }

                        foreach (var item in sp3s)
                        {
                            if (item.Contains(fileName))
                            {
                                this.CurrentService = EphemerisDataSourceFactory.Create(item);
                                var msg = "新下载并采用了NGA预报星历。" + this.CurrentService;
                                log.Info(msg);
                                break;
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 当前的星历服务是否 已有、可用。
        /// </summary>
        /// <returns></returns>
        private bool IsCurrentEphemerisServiceAvailable()
        { 
            return IsEphemerisServiceAvailable(CurrentService, CurrentSession.TimePeriod.Start, CurrentSession.SatelliteTypes);
        }

        /// <summary>
        /// 星历服务是否可用。
        /// </summary>
        /// <param name="service"></param>
        /// <param name="obsTime"></param>
        /// <param name="satTypes">多系统支持</param>
        /// <returns></returns>
        public bool IsEphemerisServiceAvailable(IEphemerisService service, Time obsTime, List<SatelliteType> satTypes = null)
        {
            bool result = (service != null && service.SatCount != 0);
            if (result)
            {
                if (satTypes != null) { return service.IsAvailable(satTypes, obsTime); }

                if (!service.TimePeriod.Contains(obsTime))
                {
                    return false;
                }
            }
            else
            {
                log.Debug("所检查 " +  service + " 星历服务为空，或没有数据。");
            }
            return result;
        }

        #endregion
    }
}

