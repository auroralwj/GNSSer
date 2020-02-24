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
//2018.05.02, czs, create in HMX, 封装全局 GNSS 数据源服务。
//2019.01.06, czs, edit in hmx, 一旦获取失败，设置指定小时不必再访问

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

namespace Gnsser
{

    /// <summary>
    /// 封装全局 GNSS 数据源服务，如天线文件服务，海潮等，应该包括星历。
    /// </summary>
    public class GlobalDataSourceService : BaseDictionary<string, IService>
    {
        new ILog log = Log.GetLog(typeof(DataSourceContext));

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        private GlobalDataSourceService()
        {
            this.Name = "全局数据源"; 
        }
        static GlobalDataSourceService instance = new GlobalDataSourceService();
        /// <summary>
        /// 单例模式
        /// </summary>
        public static GlobalDataSourceService Instance { get { return instance; } }

          

        /// <summary>
        /// 加载所有数据源.//2015.05.12, czs, 应该采用懒加载模式，用则加载，不用则不加载，可以提高效率
        /// </summary> 
        public void Init(GnsserConfig GlobalDataOption)
        {
            this.Add(GnssDataType.UniverseObject, UniverseObjectProvider.Instance);
            this.Add(GnssDataType.Antenna, new AntennaFileService(GlobalDataOption.AntennaFile));
            this.Add(GnssDataType.AntennaFileIgs08, new AntennaFileService(GlobalDataOption.AntennaFileIgs08));
            this.Add(GnssDataType.AntennaFileIgs14, new AntennaFileService(GlobalDataOption.AntennaFileIgs14));
            this.Add(GnssDataType.SatState, new SatExcludeFileService(GlobalDataOption.SatExcludeFile));
            this.Add(GnssDataType.SatInfo, new SatInfoService(GlobalDataOption.SatSateFile));
            this.Add(GnssDataType.OceanLoading, new OceanLoadingHarmonicsService(GlobalDataOption.OceanTideFile));

            GlobalIgsEphemerisService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsAutoEphemeris, GlobalIgsEphemerisService.Instance);

            GlobalIgsClockService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsAutoClock, GlobalIgsClockService.Instance);

            GlobalIgsSimpleClockService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsAutoSimpleClock, GlobalIgsSimpleClockService.Instance);

            GlobalIgsErpService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsAutoErp, GlobalIgsErpService.Instance);

            GlobalIgsGridIonoService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsGridAutoFile, GlobalIgsGridIonoService.Instance);
            IgsGridIonoFileService = GlobalIgsGridIonoService.Instance;
             
            GlobalKlobucharIonoService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsKlobucharIonoService, GlobalKlobucharIonoService.Instance);
            this.IgsKlobucharIonoService = GlobalKlobucharIonoService.Instance;

            GlobalNavEphemerisService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsNavEphemerisService, GlobalNavEphemerisService.Instance);
            this.IgsNavEphemerisService = GlobalNavEphemerisService.Instance;

            GlobalCodeHarmoIonoService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsCodeIonoHarmoFile, GlobalCodeHarmoIonoService.Instance);

            GlobalIgsGridIonoDcbService.Instance.Init(GlobalDataOption);
            this.Add(GnssDataType.IgsGridIonoDcbService, GlobalIgsGridIonoDcbService.Instance);

            this.Add(GnssDataType.DCB, new DcbDataService(GlobalDataOption.DcbDirectory));
            if (File.Exists(GlobalDataOption.VMF1Directory)) { this.Add(GnssDataType.VMF1, new Vmf1DataService(GlobalDataOption.VMF1Directory)); }
            if (File.Exists(GlobalDataOption.GPT2Directory)) { this.Add(GnssDataType.GPT2, new Gpt2DataService(GlobalDataOption.GPT2Directory)); }
            if (File.Exists(GlobalDataOption.GPT21DegreeDirectory)) { this.Add(GnssDataType.GPT21Degree, new Gpt2DataService1Degree(GlobalDataOption.GPT21DegreeDirectory)); }         
        }

        #region object override
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
        #endregion

        #region 常用数据源,如果不用，则空置

        /// <summary>
        /// 历元电离层参数服务
        /// </summary>
        public IonoEpochParamService IonoEpochParamService { get; set; }
        /// <summary>
        /// GPT2模型
        /// </summary>
        public Gpt2DataService gpt2DataService { get { return Get(GnssDataType.GPT2) as Gpt2DataService; } }

        /// <summary>
        /// GPT2模型 1度分辨率
        /// </summary>
        public Gpt2DataService1Degree gpt2DataService1Degree { get { return Get(GnssDataType.GPT21Degree) as Gpt2DataService1Degree; } }
        /// <summary>
        /// 坐标服务
        /// </summary>
        public SiteCoordService SiteCoordService { get { return Get(GnssDataType.SiteCoord) as SiteCoordService; } }

        /// <summary>
        /// 坐标服务
        /// </summary>
        public StaionInfoService StaionInfoService { get { return Get(GnssDataType.StationInfo) as StaionInfoService; } }

    
        /// <summary>
        /// SatInfoService  数据
        /// </summary>
        public SatInfoService SatInfoService { get { return Get(GnssDataType.SatInfo) as SatInfoService; } }
        /// <summary>
        /// DCB 数据
        /// </summary>
        public DcbDataService DcbDataService { get { return Get(GnssDataType.DCB) as DcbDataService; } }

        /// <summary>
        /// ERP 数据
        /// </summary>
        public IErpFileService ErpDataService
        {
            get { return Get(GnssDataType.IgsAutoErp) as IErpFileService; }
            set { Set(GnssDataType.IgsAutoErp, value); }
        }

        /// <summary>
        /// VMF1模型
        /// </summary>
        public Vmf1DataService Vmf1DataService { get { return Get(GnssDataType.VMF1) as Vmf1DataService; } }


        /// <summary>
        /// 必须指定卫星提供者。多星历服务的第一个系统的星历服务。
        /// </summary>
        public IEphemerisService IgsAutoEphemerisService
        {
            get { return Get(GnssDataType.IgsAutoEphemeris) as IEphemerisService; }
            set { Set(GnssDataType.IgsAutoEphemeris, value); }
        }
        /// <summary>
        /// 必须指定卫星提供者。多星历服务的第一个系统的星历服务。
        /// </summary>
        public IEphemerisService IgsNavEphemerisService
        {
            get { return Get(GnssDataType.IgsNavEphemerisService) as IEphemerisService; }
            set { Set(GnssDataType.IgsNavEphemerisService, value); }
        }
        /// <summary>
        /// 是否具有星历服务
        /// </summary>
        public bool HasEphemerisService { get { return IgsAutoEphemerisService != null; } }
        /// <summary>
        /// 是否具有钟差服务
        /// </summary>
        public bool HasClockService { get { return ClockService != null; } }
        /// <summary>
        /// 是否具有ERP服务
        /// </summary>
        public bool HasErpService { get { return ErpDataService != null; } }
        /// <summary>
        /// 钟差文件，精密钟差。可选。
        /// </summary>
        public IClockService ClockService
        {
            get { return Get(GnssDataType.IgsAutoClock) as IClockService; }
            set { Set(GnssDataType.IgsAutoClock, value); }
        }
        /// <summary>
        /// 天线数据源
        /// </summary>
        public AntennaFileService AntennaDataSource { get { return Get(GnssDataType.Antenna) as AntennaFileService; } }
        /// <summary>
        /// 天线数据源 IGS 14
        /// </summary>
        public AntennaFileService AntennaDataSourceIgs14 { get { return Get(GnssDataType.AntennaFileIgs14) as AntennaFileService; } }
        /// <summary>
        /// 天线数据源 IGS 08
        /// </summary>
        public AntennaFileService AntennaDataSourceIgs08 { get { return Get(GnssDataType.AntennaFileIgs08) as AntennaFileService; } }
        /// <summary>
        /// 海洋潮汐数据源
        /// </summary>
        public OceanLoadingHarmonicsService OceanLoadingDataSource { get { return Get(GnssDataType.OceanLoading) as OceanLoadingHarmonicsService; } }
        /// <summary>
        /// 卫星状态数据源
        /// </summary>
        public SatExcludeFileService SatStateDataSource { get { return Get(GnssDataType.SatState) as SatExcludeFileService; } }
        /// <summary>
        /// 宇宙目标数据源
        /// </summary>
        public UniverseObjectProvider UniverseObjectProvider { get { return Get(GnssDataType.UniverseObject) as UniverseObjectProvider; } }

        #region 电离层
        /// <summary>
        /// IGS 电离层文件服务
        /// </summary>
        public IIonoService  IgsCodeHarmoIonoFileService { get { return Get(GnssDataType.IgsCodeIonoHarmoFile) as IIonoService; } set { Set(GnssDataType.IgsCodeIonoHarmoFile, value); } }

        /// <summary>
        /// IGS   电离层文件服务
        /// </summary>
        public IIonoService IgsGridIonoFileService { get { return Get(GnssDataType.IgsGridIonoFile) as IIonoService; } set { Set(GnssDataType.IgsGridIonoFile, value); } }
        /// <summary>
        /// IGS Klobuchar 电离层文件服务
        /// </summary>
        public IIonoService IgsKlobucharIonoService { get { return Get(GnssDataType.IgsKlobucharIonoService) as IIonoService; } set { Set(GnssDataType.IgsKlobucharIonoService, value); } }

        /// <summary>
        /// 电离层参数服务
        /// </summary>
        public IonoParamService IonoParamModelService { get { return Get(GnssDataType.IonoParam) as IonoParamService; } set { Set(GnssDataType.IonoParam, value); } }


        /// <summary>
        /// 对流层增强服务
        /// </summary>
        public TropAugService TropAugService { get { return Get(GnssDataType.TropAug) as TropAugService; } set { Set(GnssDataType.TropAug, value); } }

        /// <summary>
        /// IGS电离层DCB改正
        /// </summary>
        public IGridIonoFileService GridIonoDcbDataService { get { return Get(GnssDataType.IgsGridIonoDcbService) as IGridIonoFileService; } set { Set(GnssDataType.IgsGridIonoDcbService, value); } }

        #endregion
        #endregion

    }


}
