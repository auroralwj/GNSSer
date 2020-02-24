//2014.10.06, czs, edit in hailutu,  新增了 GnssDataSourceProvider 准备替换此类，而此类功能用于提供访问的便利。
//2014.12.19, czs, edit in namu, 遍历每一个星历，并读取一个历元，检查是否具有指定类型的星历
//2015.05.10, lly, add in zz , 添加 VMF1DataService
//2015.05.12, czs, edit in namu, 重构，继承自 BaseDictionary， 名称DataSouceProvider改为 DataSourceContext
//2015.09.25, czs, edit in xian, 名称DataSouceProvider改为 DataSourceContext 数据源上下文，参照Android开发中的上下文类，增加注册判断函数
//2016.04.20, czs & cuiyang & double, add in hongqing, 多观测数据源
//2016.04.24, czs, edit in hongqing, 为所有测站附加天线信息
//2016.11.01, czs, edit in hongqing, 采用单例模式实现太阳月亮计算
//2017.08.23, czs, add in hongqing, 增加电离层文件服务
//2017.09.04, lly, add in zz, GPT2模型 1度分辨率
//2017.10.23, czs, add in hongqing, 增加多种电离层文件服务
//2017.11.09, lly, add in zz, 增加对流层增强文件服务
//2018.05.02, czs, edit in HMX, 分离出 封装全局 GNSS 数据源服务。
//2018.05.03, czs, edit in HMX, 根据星历选择天线文件，首先依据星历指定值，其次按照时间分隔

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Times;
using Gnsser.Data.Rinex;
using Geo.IO;
using Geo;
using Geo.Times;
using Gnsser.Data.Sinex;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 封装，提供更便利的数据源。所有的数据源服务，如星历、钟差等，都应该在此类注册和使用。
    /// 每次任务都具有一个数据上下文，因而不应该定义为单利模式。
    /// 数据源首先应该在此注册，使用时先判断是否注册了，再使用。
    /// 应该分开考虑：2016.10.02.07.58, czs
    /// 1、与时域相关；
    /// 2、与测站相关；
    /// 3、与系统相关。
    /// </summary>
    public class DataSourceContext : BaseDictionary<string, IService>
    {
        new ILog log = Log.GetLog(typeof(DataSourceContext));

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public DataSourceContext()
        {
        }
        /// <summary>
        /// 通用构造函数
        /// </summary>
        /// <param name="GlobalDataOption"></param>
        /// <param name="option"></param>
        /// <param name="session"></param>
        public DataSourceContext(GnsserConfig GlobalDataOption, GnssProcessOption option, SessionInfo session)
            :this()
        {
            this.GlobalDataOption = GlobalDataOption;
            this.Option = option;
            this.Session = session;
        }

        #region 属性
        /// <summary>
        /// 全局数据源
        /// </summary>
        public GlobalDataSourceService GlobalDataSource { get { return GlobalDataSourceService.Instance; } }

        /// <summary>
        /// 数据源选项
        /// </summary>
        public GnsserConfig GlobalDataOption { get; set; }
        /// <summary>
        /// 处理选项。
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 当前会话信息
        /// </summary>
        public SessionInfo Session { get; set; }
        #endregion

        /// <summary>
        /// 加载所有数据源.//2015.05.12, czs, 应该采用懒加载模式，用则加载，不用则不加载，可以提高效率
        /// </summary> 
        public void LoadDataSource()
        {
            //如果没有设置输入源，则判断是否有路径
            if (ObservationDataSources == null && this.Option.IsObsDataRequired) { AddObservationDataSources(); }

            if (Session == null)
            {
                Session = new SessionInfo(ObservationDataSources.TimePeriod, Option.SatelliteTypes);
            }
            
            if (this.Option.IsSiteCoordServiceRequired) { AddSiteCoordService(); }
            if (this.Option.IsStationInfoRequired) { AddStaionInfoService(); }

            SetAntennaDataSource();

            //为所有测站附加天线信息
            if (this.AntennaDataSource != null)
            {
                foreach (var item in ObservationDataSources.DataSources)
                {
                    item.SiteInfo.Antenna = this.AntennaDataSource.Get(item.SiteInfo.AntennaType);
                }
            }

            if ((EphemerisService == null || EphemerisService.SatCount == 0) && this.Option.IsEphemerisRequired)
            {
                this.EphemerisService = new EphemerisServiceProvider(this.GlobalDataOption, Option, ObservationDataSources).GetService();
                //第二星历
                ObservationDataSources.TryLoadEphemerisServices();
                if (ObservationDataSources.EphemerisServices.Count > 0)
                {
                    this.SecondEphemerisService = ObservationDataSources.EphemerisServices.First;

                    var msg = "自带了导航星历，加入备用星历服务之中" + this.SecondEphemerisService;
                    log.Info(msg);
                }
            }

            if (!HasClockService && Option.IsPreciseClockFileRequired && Option.IsUsingFullClockService)
            {
                this.ClockService = new ClockServiceProvider(GlobalDataOption, Option).GetService();
            }

            if (!HasClockService && Option.IsPreciseClockFileRequired)
            {
                this.SimpleClockService = new SimpleClockServiceProvider(GlobalDataOption, Option).GetService();
            }

            var isIgsProductAwailable = IgsProductTimeAvailable.IsFinalAvailableGps(Session.TimePeriod.Start);


            if (isIgsProductAwailable && ErpDataService == null && Option.IsErpFileRequired)
            {
                var msg = "没有手动设置ERP文件,也没有启用文件自动匹配功能。";
                if (!this.GlobalDataOption.EnableAutoFindingFile)
                {
                    log.Info(msg);
                    //  throw new ArgumentException(msg);
                }
                else
                {
                    this.ErpDataService = new ErpServiceProvider(GlobalDataOption, Option).GetService();
                    //  this.ErpDataService = new CommonErpServiceProvider(IgsProductSourceOption).GetDataSourceService();
                    //this.ErpDataService = new ErpServiceProvider(GlobalDataOption, Option).GetService();
                    //this.Add(GnssDataType.ERP, new ErpFileService(Option.GetErpFileOption()));

                }
            }
            LoadIonoDataSourceService(isIgsProductAwailable);

            if (this.Option.IsTropAugmentEnabled && File.Exists(Option.TropAugmentFilePath))
            {
                log.Info("即将加载对流层增强文件 " + Option.TropAugmentFilePath);
                this.TropAugService = new TropAugService(Option.TropAugmentFilePath);
            }

            //钟跳
            if (Option.IsOpenClockJumpSwitcher && File.Exists(Option.OuterClockJumpFile))
            {
                ClockJumpFile = new ObjectTableReader(Option.OuterClockJumpFile).Read();
            }
        }
        /// <summary>
        /// 根据星历选择天线文件，首先依据星历指定值，其次按照时间分隔
        /// </summary>
        private void SetAntennaDataSource()
        {
            //根据星历选择天线文件
            if (this.EphemerisService != null 
                && this.EphemerisService.CoordinateSystem != null
                && this.EphemerisService.CoordinateSystem != Sp3File.UNDEF)
            {
                if (this.EphemerisService.CoordinateSystem.Contains("14"))
                {
                    this.AntennaDataSource = GlobalDataSourceService.Instance.AntennaDataSourceIgs14;
                }
                else if (this.EphemerisService.CoordinateSystem.Contains("08"))
                {
                    this.AntennaDataSource = GlobalDataSourceService.Instance.AntennaDataSourceIgs08;
                }
            }


            if (this.AntennaDataSource == null)
            {
                if (Session.TimePeriod.Start >= Time.StartOfIgs14)
                {
                    this.AntennaDataSource = GlobalDataSourceService.Instance.AntennaDataSourceIgs14;
                }
                else //if (Session.TimePeriod.Start >= Time.StartOfIgs08)
                {
                    this.AntennaDataSource = GlobalDataSourceService.Instance.AntennaDataSourceIgs08;
                }
                //默认要有一个
                if (this.AntennaDataSource == null)
                {
                    this.AntennaDataSource = GlobalDataSourceService.Instance.AntennaDataSource;
                }
            }
        }
        /// <summary>
        /// 检查并更新
        /// </summary>
        /// <param name="ephFilePath"></param>
        /// <param name="clkFilePath"></param>
        /// <param name="isForceToUpdate"></param>
        public void CheckOrUpdateEphAndClkService(string ephFilePath, string clkFilePath, bool isForceToUpdate =false)
        {
            if ((this.EphemerisService == null || this.EphemerisService.SatCount == 0)
                || isForceToUpdate)
            {
                FileEphemerisService ephemerisDataSource = null;
                if (File.Exists(ephFilePath))
                {
                    ephemerisDataSource = EphemerisDataSourceFactory.Create(ephFilePath, FileEphemerisType.Unkown, true, Option.MinSuccesiveEphemerisCount);
                    this.EphemerisService = ephemerisDataSource;
                }
            }

            if ((this.SimpleClockService == null || this.EphemerisService.SatCount == 0)
               || isForceToUpdate)
            { 
                ISimpleClockService ClockService = null;
                if (File.Exists(clkFilePath))
                {
                    ClockService = new SimpleClockService(clkFilePath);
                }
                this.SimpleClockService = ClockService;
            }
        }

        /// <summary>
        /// 加载电离层服务，主要是检查并加载手动的电离层服务。具体用哪一个需要在Option和使用中决定！！
        /// </summary>
        /// <param name="isIgsProductAwailable">是否可用</param>
        private void LoadIonoDataSourceService(bool isIgsProductAwailable)
        {
            //是否有输入数据
            if (Option.IsIndicatingGridIonoFile && File.Exists(Option.IonoGridFilePath))
            {
                this.IgsGridIonoFileService = new GridIonoFileService(Option.IonoGridFilePath);
                log.Info("成功载入选项指定电离层文件 " + Option.IonoGridFilePath);
            }
            if(this.IgsGridIonoFileService == null)
            {
                this.IgsGridIonoFileService = GlobalIgsGridIonoService.Instance;
                log.Info("成功采用自动 IGS 格网电离层服务 ");
            }
             
            if (Option.IsGnsserEpochIonoFileRequired && File.Exists(Option.GnsserEpochIonoFilePath))
            {
                log.Info("将加载GNSSer历元电离层文件 " + Option.GnsserEpochIonoFilePath);
                this.IonoEpochParamService = new IonoEpochParamService(Option.GnsserEpochIonoFilePath);
            }
            //存在则添加
            if (Option.IsNavIonoModelCorrectionRequired && File.Exists(Option.NavIonoModelPath))
            {
                log.Info("请注意时段是否正确，即将加载导航文件作为电离层参数 " + Option.NavIonoModelPath);
                this.IonoKlobucharParamService = new IonoParamService(Option.NavIonoModelPath);
            }
            if (Option.IsGnsserFcbOfDcbRequired && File.Exists(Option.GnsserFcbFilePath))
            {
                log.Info("即将加载 GNSSer FCB DCB 文件 " + Option.GnsserFcbFilePath);
                this.GnsserFcbOfUpdService = new FcbOfUpdService(Option.GnsserFcbFilePath);
            }
        }

        private void AddObservationDataSources()
        {
            if (!String.IsNullOrEmpty(GlobalDataOption.ObsPath))
            {
                var pathes = GlobalDataOption.ObsPath.Split(';');
                this.Add(GnssDataType.Observation, new MultiSiteObsStream(pathes, Option.BaseSiteSelectType, Option.IsSameSatRequired, this.Option.IndicatedBaseSiteName));

                log.Info("加载了观测数据源！" + String.Format(new EnumerableFormatProvider(), "{0:,}", pathes));
            }
            else
            {
                throw new ArgumentNullException("请设置观测数据源！ ObservationDataSource 或路径 FilePath");
            }
        }

        private void AddStaionInfoService()
        {
            if (this.Option.IsIndicatingStationInfoFile && File.Exists(this.Option.StationInfoPath))
            {
                this.Add(GnssDataType.StationInfo, new StaionInfoService(new FileOption(this.Option.StationInfoPath)));
                log.Info("加载了指定的测站信息文件！" + this.Option.StationInfoPath);
            }
            else
            {
                this.Add(GnssDataType.StationInfo, new StaionInfoService(GlobalDataOption.StationInfoPath));
                log.Info("加载了系统默认测站文件！" + GlobalDataOption.StationInfoPath);
            }
        }
        /// <summary>
        /// 增加坐标服务
        /// </summary>
        private async void AddSiteCoordService()
        {

            if (this.Option.IsIndicatingCoordFile && File.Exists(this.Option.CoordFilePath))
            {
                this.Add(GnssDataType.SiteCoord, new SiteCoordService(new FileOption(this.Option.CoordFilePath)));
                log.Info("加载了指定的坐标文件！" + this.Option.CoordFilePath);
            }
            else
            {
                this.Add(GnssDataType.SiteCoord, new SiteCoordService(GlobalDataOption.SiteCoordFile));
                log.Info("加载了系统默认坐标文件！" + GlobalDataOption.SiteCoordFile);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var o = obj as DataSourceContext;
            if (o == null) return false;

            return o.ToString().Equals(this.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #region 常用数据源,如果不用，则空置
        /// <summary>
        /// 测站卫星出现时段记录数据与服务
        /// </summary>
        public SiteSatPeriodDataService SiteSatPeriodDataService { get; set; }
        /// <summary>
        /// 测站卫星出现时段记录与服务
        /// </summary>
        public SiteSatAppearenceService SiteSatAppearenceService { get; set; }

        /// <summary>
        /// 历元电离层参数服务
        /// </summary>
        public IonoEpochParamService IonoEpochParamService { get; set; }
        /// <summary>
        /// 坐标服务
        /// </summary>
        public SiteCoordService SiteCoordService { get { return Get(GnssDataType.SiteCoord) as SiteCoordService; } }

        /// <summary>
        /// 坐标服务
        /// </summary>
        public StaionInfoService StaionInfoService { get { return Get(GnssDataType.StationInfo) as StaionInfoService; } }

        /// <summary>
        /// 观测数据源,观测数据集合的第一个。
        /// </summary>
        public ISingleSiteObsStream ObservationDataSource
        {
            get { if (ObservationDataSources == null) return null; return ObservationDataSources.BaseDataSource; }
            set { ObservationDataSources = new MultiSiteObsStream(value); }
        }

        /// <summary>
        /// 观测数据源，是一个集合
        /// </summary>
        public MultiSiteObsStream ObservationDataSources
        {
            get { return Get(GnssDataType.Observation) as MultiSiteObsStream; }
            set { Set(GnssDataType.Observation, value); }
        }

        /// <summary>
        /// DCB 数据
        /// </summary>
        public DcbDataService DcbDataService { get { return GlobalDataSource.DcbDataService; } }

        /// <summary>
        /// ERP 数据
        /// </summary>
        public IErpFileService ErpDataService
        {
            get { return Get(GnssDataType.IgsAutoErp) as IErpFileService; }
            set { Set(GnssDataType.IgsAutoErp, value); }
        }
        /// <summary>
        /// FCB 服务
        /// </summary>
        public FcbOfUpdService GnsserFcbOfUpdService
        {
            get { return Get(GnssDataType.GnsserFcbOfUpdService) as FcbOfUpdService; }
            set { Set(GnssDataType.GnsserFcbOfUpdService, value); }
        } 

        /// <summary>
        /// 必须指定卫星提供者。多星历服务的第一个系统的星历服务。
        /// </summary>
        public IEphemerisService EphemerisService
        {
            get { return Get(GnssDataType.IgsAutoEphemeris) as IEphemerisService; }
            set { Set(GnssDataType.IgsAutoEphemeris, value); }
        }
        /// <summary>
        /// 第二星历，通常为自身的导航文件。多星历服务的第一个系统的星历服务。
        /// </summary>
        public IEphemerisService SecondEphemerisService
        {
            get { return Get(GnssDataType.SecondEphemerisService) as IEphemerisService; }
            set { Set(GnssDataType.SecondEphemerisService, value); }
        }
        /// <summary>
        /// 是否具有星历服务
        /// </summary>
        public bool HasEphemerisService { get { return EphemerisService != null; } }
        /// <summary>
        /// 是否具有钟差服务
        /// </summary>
        public bool HasClockService { get { return SimpleClockService != null; } }
        /// <summary>
        /// 是否具有ERP服务
        /// </summary>
        public bool HasErpService { get { return ErpDataService != null; } }
        /// <summary>
        /// 钟差文件，精密钟差。可选。
        /// </summary>
        public ISimpleClockService SimpleClockService
        {
            get { return Get(GnssDataType.IgsAutoSimpleClock) as ISimpleClockService; }
            set { Set(GnssDataType.IgsAutoSimpleClock, value); }
        }

        /// <summary>
        /// 钟差文件，精密钟差。可选。
        /// </summary>
        public IClockService ClockService
        {
            get { return Get(GnssDataType.IgsAutoClock) as IClockService; }
            set { Set(GnssDataType.IgsAutoClock, value); }
        }
        #region 全局数据源
        /// <summary>
        /// SatInfoService  数据
        /// </summary>
        public SatInfoService SatInfoService { get => GlobalDataSource.SatInfoService; }
        /// <summary>
        /// GPT2模型
        /// </summary>
        public Gpt2DataService gpt2DataService { get => GlobalDataSource.gpt2DataService; }

        /// <summary>
        /// GPT2模型 1度分辨率
        /// </summary>
        public Gpt2DataService1Degree gpt2DataService1Degree { get => GlobalDataSource.gpt2DataService1Degree; }
        /// <summary>
        /// VMF1模型
        /// </summary>
        public Vmf1DataService Vmf1DataService { get { return GlobalDataSource.Vmf1DataService; } }
        /// <summary>
        /// 海洋潮汐数据源
        /// </summary>
        public OceanLoadingHarmonicsService OceanLoadingDataSource { get => GlobalDataSource.OceanLoadingDataSource; }
        /// <summary>
        /// 卫星状态数据源
        /// </summary>
        public SatExcludeFileService SatStateDataSource { get => GlobalDataSource.SatStateDataSource; }
        /// <summary>
        /// 宇宙目标数据源
        /// </summary>
        public UniverseObjectProvider UniverseObjectProvider { get => GlobalDataSource.UniverseObjectProvider; }
        #endregion

        /// <summary>
        /// 天线数据源
        /// </summary>
        public AntennaFileService AntennaDataSource    {
            get {
                return Get(GnssDataType.Antenna) as AntennaFileService;
            }
            set { Set(GnssDataType.Antenna, value); }
        }// { get => GlobalDataSource.AntennaDataSource; }
        #region 电离层
        /// <summary>
        /// IGS 电离层文件服务
        /// </summary>
        public IIonoService IgsGridIonoFileService { get { return Get(GnssDataType.IgsGridIonoFile) as IIonoService; } set { Set(GnssDataType.IgsGridIonoFile, value); } }
        /// <summary>
        /// IGS Klobuchar 电离层文件服务
        /// </summary>
        public IIonoService IgsKlobucharIonoService { get => GlobalDataSource.IgsKlobucharIonoService; set { Set(GnssDataType.IgsKlobucharIonoService, value); } }
        /// <summary>
        /// 电离层服务
        /// </summary>
        public KlobucharIonoService KlobucharIonoService   { get { return Get(GnssDataType.KlobucharIonoService) as KlobucharIonoService; } set { Set(GnssDataType.KlobucharIonoService, value); } }
       
        /// <summary>
        /// 电离层参数服务
        /// </summary>
        public IonoParamService IonoKlobucharParamService { get { return Get(GnssDataType.IonoParam) as IonoParamService; } set { Set(GnssDataType.IonoParam, value); } }
        
        /// <summary>
        /// IGS CODE 球谐函数 电离层文件服务
        /// </summary>
        public IIonoService IgsCodeHarmoIonoFileService { get => GlobalDataSource.IgsCodeHarmoIonoFileService; } 

        
        /// <summary>
        /// 对流层增强服务
        /// </summary>
        public TropAugService TropAugService { get { return Get(GnssDataType.TropAug) as TropAugService; } set { Set(GnssDataType.TropAug, value); } }
        /// <summary>
        /// IGS电离层DCB改正
        /// </summary>
        public IGridIonoFileService GridIonoDcbDataService { get => GlobalDataSource.GridIonoDcbDataService; }
        /// <summary>
        /// 钟跳文件
        /// </summary>
        public ObjectTableStorage ClockJumpFile { get; set; }
        #endregion
        #endregion

        #region 加载
        /// <summary>
        /// 加载方法
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="obsPath"></param>
        /// <param name="navFile"></param>
        /// <param name="clockFile"></param>
        /// <returns></returns>
        public static DataSourceContext LoadDefault(GnssProcessOption Option,
            string obsPath=null, IEphemerisService navFile = null,
            Data.ISimpleClockService clockFile = null)
        {
            DataSourceContext DataSourceContext = new Gnsser.DataSourceContext();
            RinexFileObsDataSource obsSource = null;
            if (obsPath != null)
            {
                obsSource = new RinexFileObsDataSource(obsPath);
            }else if( File.Exists(Option.ObsFilePath))
            {
                obsSource = new RinexFileObsDataSource(Option.ObsFilePath);
            }
            else
            {
                throw new Exception("加载Context失败！请设置观测文件路径后再试。");
            }
            return LoadDefault(Option, obsSource, navFile, clockFile);
        }

        /// <summary>
        /// 默认加载，用于定位计算。
        /// </summary>
        /// <param name="option"></param>
        /// <param name="obsSource"></param>
        /// <param name="navFile"></param>
        /// <param name="clockFile"></param>
        /// <returns></returns>
        public static DataSourceContext LoadDefault(GnssProcessOption option,
            ISingleSiteObsStream obsSource,
            IEphemerisService navFile = null,
            Data.ISimpleClockService clockFile = null)
        {
            SessionInfo session = new SessionInfo(obsSource.ObsInfo.TimePeriod, option.SatelliteTypes);
            DataSourceContext DataSourceContext = new Gnsser.DataSourceContext(Setting.GnsserConfig, option, session);

            if (obsSource != null)
            {
                DataSourceContext.ObservationDataSource = obsSource;
            }
            if (navFile != null)
            {
                DataSourceContext.EphemerisService = navFile;
            }

            if (clockFile != null)
            {
                DataSourceContext.SimpleClockService = clockFile;
            }


            //加载数据源
            DataSourceContext.LoadDataSource();

            return DataSourceContext;
        }
        /// <summary>
        /// 加载默认
        /// </summary>
        /// <param name="PositionOption"></param>
        /// <param name="obsSource"></param>
        /// <param name="navFile"></param>
        /// <param name="clockFile"></param>
        /// <returns></returns>
        public static DataSourceContext LoadDefault(GnssProcessOption PositionOption,
            MultiSiteObsStream obsSource,
            IEphemerisService navFile = null,
            Data.ISimpleClockService clockFile = null)
        {
            SessionInfo session = new SessionInfo(obsSource.BaseDataSource.ObsInfo.TimePeriod, PositionOption.SatelliteTypes);

            var DataSourceOption = Setting.GnsserConfig;

            DataSourceContext DataSourceContext = new Gnsser.DataSourceContext(DataSourceOption, PositionOption, session);

            if (obsSource != null)
            {
                DataSourceContext.ObservationDataSources = obsSource;
            }
            if (navFile != null)
            {
                DataSourceContext.EphemerisService = navFile;
            }

            if (clockFile != null)
            {
                DataSourceContext.SimpleClockService = clockFile;
            }
            //加载数据源
            DataSourceContext.LoadDataSource();

            return DataSourceContext;
        }


        /// <summary>
        /// 加载默认
        /// </summary>
        /// <param name="PositionOption"></param>
        /// <param name="obsSource"></param>
        /// <param name="navFile"></param>
        /// <param name="clockFile"></param>
        /// <returns></returns>
        public static DataSourceContext LoadDefault(GnssProcessOption PositionOption,
            IService obsSource,
            IEphemerisService navFile = null,
            Data.ISimpleClockService clockFile = null)
        {

            if (obsSource is MultiSiteObsStream)
            {
                return LoadDefault(PositionOption, (MultiSiteObsStream)obsSource, navFile, clockFile);
            }
            if (obsSource is ISingleSiteObsStream)
            {
                return LoadDefault(PositionOption, (ISingleSiteObsStream)obsSource, navFile, clockFile);
            }
            
            if (obsSource is ReversedMultiSiteObsStream)
            {
                return LoadDefault(PositionOption, ((ReversedMultiSiteObsStream)obsSource).OriginalSource, navFile, clockFile);
            }
            if (obsSource is ReversedSingleSiteObsStream)
            {
                return LoadDefault(PositionOption, ((ReversedSingleSiteObsStream)obsSource).OriginalSource, navFile, clockFile);
            }


            //   return LoadDefault(PositionOption);
            throw new Exception("加载Context失败！");
            return null;
        }

        #endregion
    }
}
