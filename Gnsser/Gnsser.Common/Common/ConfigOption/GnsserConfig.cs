//2015.01.18, czs, create in namu, Gnsser 设置文件,大部分属性从Setting中迁移过来。
//2017.08.19, czs, add in hongqing, 增加报表输出
//2017.09.04, lly, add in zz, GPT2模型 1度分辨率
//2018.03.15, czs, edit in hmx, 增加超快和快速产品标识。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.IO;
using Gnsser.Times;
using System.Configuration;
using Geo.Times;
using Geo.Common;
using Gnsser.Data;

namespace Gnsser
{


    /// <summary>
    /// Gnsser 设置文件
    /// </summary>
    public class GnsserConfig : Config
    {
        public const string IgsProduct = "IgsProduct";
        public const string Setting = "Setting";
        public const string Exe = "Exe";

        public Action<string> CurrentProjectChanged;

        public Dictionary<SatelliteNumber, int> GlonassSlotFrequences { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public GnsserConfig(Config config) : base(config.Data, config.Comments)
        {
            LoadGnsserProject();
        }
        /// <summary>
        /// 是否忽略电离层文件内容
        /// </summary>
        public bool IsSkipIonoContent { get; set; }

        #region 工程相关
        /// <summary>
        /// 当前工程
        /// </summary>
        public GnsserProject CurrentProject { get; set; }
        /// <summary>
        /// 当前项目路径。
        /// </summary>
        public string CurrentProjectPath { get { return GetPath("CurrentProjectPath"); } set { SetPath(value, "CurrentProjectPath"); } }

        /// <summary>
        /// 根据提供的路径，设置当前工程,并保存到历史记录中。
        /// </summary>
        /// <param name="path"></param>
        public void OpenAndSetCurrentProject(string path)
        {
            this.CurrentProjectPath = path;
            this.LoadGnsserProject();
            this.UpdateProjectHistoryPath(path);
        }

        /// <summary>
        /// 设置为当前工程，根据工程路径，保存工程文件设置，同时保存到历史记录中。
        /// </summary> 
        /// <param name="project"></param>
        public void SetAsCurrentProjectAndSaveToFile(GnsserProject project)
        {
            this.CurrentProjectPath = project.ProjectFilePath;
            this.CurrentProject = project;
            this.UpdateProjectHistoryPath(CurrentProjectPath);
            this.SaveCurrentProject();
        }

        /// <summary>
        /// 更新工程历史记录。最后的始终在第一位。
        /// </summary>
        /// <param name="path"></param>
        public void UpdateProjectHistoryPath(string path)
        {
            if (CurrentProjectChanged != null) CurrentProjectChanged(path);

            var pathes = HistoryProjectPathes;
            if (pathes.Contains(path))
            {
                pathes.Remove(path);
            }
            pathes.Insert(0, path);

            this.HistoryProjectPathes = pathes;
        }

        /// <summary>
        /// 历史工程
        /// </summary>
        public List<string> HistoryProjectPathes
        {
            get
            {
                List<string> pathes = new List<string>();
                var str = GetString("HistoryProjectPathes");
                var strs = str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in strs)
                {
                    pathes.Add(item);
                }
                return pathes;
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0) sb.Append(";");
                    sb.Append(item);
                    i++;
                }
                this.Set("HistoryProjectPathes", sb.ToString());
            }
        }

        #region 保存到文件

        /// <summary>
        /// 保存到文件。
        /// </summary>
        public void SaveCurrentProject()
        {
            this.CurrentProject.SaveToFile(this.CurrentProjectPath);
        }
        /// <summary>
        /// GNSS 工程加载
        /// </summary>
        public void LoadGnsserProject()
        {
            try
            {
                this.CurrentProject = new GnsserProject(CurrentProjectPath);
                this.CurrentProject.ProjectDirectory = Path.GetDirectoryName(CurrentProjectPath);
            }
            catch (Exception ex) { log.Error("读取工程配置文件出错！" + ex.Message); }
        }
        #endregion
        #endregion

        #region 常用属性

        /// <summary>
        /// 当前时刻 UTC 闰秒次数或秒数。
        /// </summary>
        public int LeapSecond { get { return GetInt("LeapSecond", 27); } set { GetInt("LeapSecond", value); } }
        /// <summary>
        /// 卫星系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes
        {
            get
            {
                List<SatelliteType> satellites = new List<SatelliteType>();
                var str = GetString("SatelliteTypes");
                var strs = str.Split(new char[] { ',', ';', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in strs)
                {
                    satellites.Add((SatelliteType)Enum.Parse(typeof(SatelliteType), item));
                }
                return satellites;
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0) sb.Append(";");
                    sb.Append(item);
                    i++;
                }
                this.Set("SatelliteTypes", sb.ToString());
            }
        }
        /// <summary>
        /// 观测文件，必须的，当前只支持一个文件
        /// </summary>
        public string ObsPath { get { return GetPath("SampleOFile"); } set { SetPath("SampleOFile", value); } }
        /// <summary>
        /// 星历文件，非必须的。
        /// </summary>
        public string NavPath { get { return GetPath("SampleNFile"); } set { SetPath("SampleNFile", value); } }
        /// <summary>
        /// 星历文件，非必须的。
        /// </summary>
        public string ClkPath { get { return GetPath("SampleClkFile"); } set { SetPath("SampleClkFile", value); } }
        /// <summary>
        /// GnsserFcbFilePath
        /// </summary>
        public string GnsserFcbFilePath { get { return GetPath("GnsserFcbFilePath", @"Data\GNSS\Common\FcbOfDcb.fcb.txt.xls"); } set { SetPath("GnsserFcbFilePath", value); } }

        /// <summary>
        /// 程序是否处于调试状态,如果是则直接抛出异常，否则继续执行程序
        /// </summary>
        public bool IsDebug { get { return GetBool("IsDebug", true); } set { Set("IsDebug", value.ToString(), Common, "程序是否处于调试状态,如果是则直接抛出异常，否则继续执行程序"); } }
        /// <summary>
        /// Exe 根目录
        /// </summary>
        public string ExeFolder = @".\Exe\";

        /// <summary>
        /// 计算结果是否输出
        /// </summary>
        public bool IsOutputResult
        {
            get { return GetBool("IsOutputResult"); }
            set { Set("IsOutputResult", value.ToString()); }
        }
        /// <summary>
        /// 是否输出平差结果
        /// </summary>
        public bool IsOutputAdjust
        {
            get { return GetBool("IsOutputAdjust"); }
            set { Set("IsOutputAdjust", value.ToString()); }
        }
        /// <summary>
        /// 计算结果是否实时输出在屏幕
        /// </summary>
        public bool IsShowResultOnTime
        {
            get { return GetBool("IsShowResultOnTime"); }
            set { Set("IsShowResultOnTime", value.ToString()); }
        }

        /// <summary>
        /// 是否忽略或过滤粗差
        /// </summary>
        public bool IgnoreCourceError
        {
            get { return GetBool("IgnoreCourceError"); }
            set { Set("IgnoreCourceError", value.ToString()); }
        }
        #endregion

        #region EXE 互操作
        /// <summary>
        /// Rtkrcv 配置
        /// </summary>

        public string RtkrcvConfig
        {
            set { SetConfigVlue("RtkrcvConfig", value, Exe); }
            get { return GetConfigValue("RtkrcvConfig"); }
        }
        /// <summary>
        /// Rtkpost exe文件
        /// </summary>
        public string RtklibPostExe
        {
            set { SetConfigVlue("RtklibPostExe", value, Exe); }
            get { return GetConfigValue("RtklibPostExe"); }
        }
        /// <summary>
        /// RtkRvc 执行文件
        /// </summary>
        public string RtkrcvExe
        {
            set { SetConfigVlue("RtkrcvExe", value, Exe); }
            get { return GetConfigValue("RtkrcvExe"); }
        }
        /// <summary>
        /// rtkpost 配置文件
        /// </summary>
        public string RtklibPostConfig
        {
            set { SetConfigVlue("RtklibPostConfig", value, Exe); }
            get { return GetConfigValue("RtklibPostConfig"); }
        }
        /// <summary>
        /// Teqc 执行文件路径
        /// </summary>
        public string TeqcPath
        {
            set { SetConfigVlue("TeqcExe", value, Exe); }
            get { return GetConfigValue("TeqcExe"); }
        }
        /// <summary>
        /// Gzip 执行文件
        /// </summary>
        public string GzipExe
        {
            set { SetConfigVlue("GzipExe", value, Exe); }
            get { return GetConfigValue("GzipExe"); }
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        public string TarExe
        {
            set { SetConfigVlue("TarExe", value, Exe); }
            get { return GetConfigValue("TarExe"); }
        }
        /// <summary>
        /// 网络下载
        /// </summary>
        public string WgetpExe
        {
            set { SetConfigVlue("WgetpExe", value, Exe); }
            get { return GetConfigValue("WgetpExe"); }
        }
        /// <summary>
        /// 解压缩文件
        /// </summary>
        public string Crx2rnx
        {
            set { SetConfigVlue("Crx2rnx", value, Exe); }
            get { return GetConfigValue("Crx2rnx"); }
        }
        #endregion

        #region 基础目录
        /// <summary>
        /// 是否启用自动匹配文件
        /// </summary>
        public bool EnableAutoFindingFile
        {
            get { return GetBool("EnableAutoFindingFile"); }
            set { SetObj("EnableAutoFindingFile", value, IgsProduct); }
        }
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory
        {
            get { return GetPath("OutputDirectory"); }
            set { SetPath(value, "OutputDirectory"); }
        }
        /// <summary>
        /// 基础数据路径
        /// </summary>
        public string BaseDataPath
        {
            get { return GetPath("BaseDataPath"); }
            set { SetPath(value, "BaseDataPath"); }
        }
        /// <summary>
        /// 临时目录路径
        /// </summary>
        public string TempDirectory
        {
            get { return GetPath("TempDirectory"); }
            set { SetPath(value, "TempDirectory"); }
        }
        /// <summary>
        /// 测站信息路径
        /// </summary>
        public string StationInfoPath
        {
            get { return GetPath("StationInfoPath"); }
            set { SetPath(value, "StationInfoPath"); }
        }
        #endregion

        #region 精密星历数据库
  /// <summary>
        /// 是否采用唯一数据源
        /// </summary>
        public double RetryHoursWhenFailedForIgsDownload
        {
            get { return GetDouble("RetryHoursWhenFailedForIgsDownload", 4); }
            set { SetVal(value, "RetryHoursWhenFailedForIgsDownload", DataSource); }
        }

        /// <summary>
        /// 是否采用唯一数据源
        /// </summary>
        public bool IsUniqueSource
        {
            get { return GetBool("IsUniqueSource", true); }
            set { SetVal(value, "IsUniqueSource", DataSource); }
        }

             /// <summary>
        /// 指定的数据源代码
        /// </summary>
        public string IndicatedSourceCode
        {
            get { return GetString("IndicatedSourceCode", "ig"); }
            set { Set( "IndicatedSourceCode", value, DataSource); }
        }
        /// <summary>
        /// 精密星历允许的断裂次数
        /// </summary>
        public int Sp3EphMaxBreakingCount
        {
            get { return GetInt("Sp3EphMaxBreakingCount", 5); }
            set { this.SetVal(value, "Sp3EphMaxBreakingCount", Setting); }
        }

        /// <summary>
        /// 定位产品模板
        /// </summary>
        public string PositionReportModel
        {
            get { return GetPath("PositionReportModel", @"Data\GNSS\Common\Report.html"); }
            set { SetPath(value, "PositionReportModel"); }
        }
        /// <summary>
        /// 本地IGS产品路径
        /// </summary>
        public string IgsProductLocalDirectory
        {
            get { return GetPath("IgsProductLocalDirectory", @"Data\GNSS\IgsProduct\"); }
            set { SetPath(value, "IgsProductLocalDirectory"); }
        }
        /// <summary>
        /// 本地IGS产品路径
        /// </summary>
        public string IgsProductLocalDirectoriesString
        {
            get { return GetString("IgsProductLocalDirectories", @"Data\GNSS\IgsProduct\"); }
            set { Set(value, "IgsProductLocalDirectories"); }
        }
        /// <summary>
        /// 本地IGS产品路径集合
        /// </summary>
        public List<string> IgsProductLocalDirectories
        {
            get {
                var str = IgsProductLocalDirectoriesString;
                var pathes = str.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var list = new List<string>();
                foreach (var item in pathes)
                {
                    var path = GeoLocalPath(item);
                    list.Add(path);
                }
                return list;
            }
            
            set {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0)
                    {
                        sb.Append(";");
                    }
                    string path = item.Replace(BaseDirectory, "");
                    sb.Append(path);
                    i++;
                }
                IgsProductLocalDirectoriesString = sb.ToString();
            }
        }
        /// <summary>
        /// 电离层文件路径
        /// </summary>
        public string IonoFilePath
        {
            get { return GetPath("IonoFilePath", @"Data\GNSS\IgsProduct\igsg0010.13i"); }
            set { SetPath(value, "IonoFilePath"); }
        }
        /// <summary>
        /// NGA 9DayGPSpreds 预报星历目录
        /// </summary>
        public string Nga9DayGPSpredsDirectory
        {
            get { return GetPath("Nga9DayGPSpredsDirectory", @"Data\GNSS\Nga9DayGPSpreds\"); }
            set { SetPath(value, "Nga9DayGPSpredsDirectory"); }
        }
        /// <summary>
        /// IgsProductUrlModel  ={UrlDirectory}/{Week}/{SourceName}{Week}{DayOfWeek}.{ProductType}.Z
        /// 导航星历下载路径模板。
        /// </summary>
        public string NavEphemerisUrlModel
        {
            get { return GetPath("NavEphemerisUrlModel"); }
            set { SetPath(value, "NavEphemerisUrlModel"); }
        }
        /// <summary>
        /// 以分号分割的IGS数据源模板
        /// </summary>
        public string[] IgsProductUrlModels
        {
            set
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0) sb.Append(";");
                    sb.Append(item.Trim());
                    i++;
                }
                SetConfigVlue("IgsProductUrlModel", sb.ToString(), IgsProduct);
            }
            get
            {
                string str = GetConfigValue("IgsProductUrlModel");
                return str.Split(';');
            }
        }
        /// <summary>
        /// 以分号分割的IGS数据源模板
        /// </summary>
        public string[] IgsProductUrlDirectories
        {
            set
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (var item in value)
                {
                    if (i != 0) sb.Append(";");
                    sb.Append(item.Trim());
                    i++;
                }
                SetConfigVlue("IgsProductUrlDirectory", sb.ToString(), IgsProduct);
            }
            get
            {
                string str = GetConfigValue("IgsProductUrlDirectory");
                return str.Split(';');
            }
        }
        /// <summary>
        /// GPS 星历数据源
        /// </summary>
        public string IgsProductSource
        {
            set { SetConfigVlue("IgsProductSource", value, IgsProduct); }
            get { return GetConfigValue("IgsProductSource"); }
        }
        /// <summary>
        /// GPS 星历数据源
        /// </summary>
        public string GpsEphemerisSource
        {
            set { SetConfigVlue("IgsProductSourceOfGps", value, IgsProduct); }
            get { return GetConfigValue("IgsProductSourceOfGps"); }
        }
        /// <summary>
        /// 北斗星历数据源
        /// </summary>
        public string BeidouEphemerisSource
        {
            set { SetConfigVlue("IgsProductSourceOfBeidou", value, IgsProduct); }
            get { return GetConfigValue("IgsProductSourceOfBeidou"); }
        }
        /// <summary>
        /// 伽利略星历数据源
        /// </summary>
        public string GalieoEphemerisSource
        {
            set { SetConfigVlue("IgsProductSourceOfGalieo", value, IgsProduct); }
            get { return GetConfigValue("IgsProductSourceOfGalieo"); }
        }
        /// <summary>
        /// GLONASS 星历数据源
        /// </summary>
        public string GlonassEphemerisSource
        {
            set { SetConfigVlue("IgsProductSourceOfGlonass", value, IgsProduct); }
            get { return GetConfigValue("IgsProductSourceOfGlonass"); }
        }
        /// <summary>
        /// 基准卫星文件
        /// </summary>
        public string BasePrnFileName
        {
            get { return GetPath("BasePrnFileName", @"Data\PeriodSat.BasePrn.txt.xls"); }
            set { SetPath(value, "BasePrnFileName"); }
        }

        /// <summary>
        /// 卫星示例高度角文件路径
        /// </summary>
        public string SatElevationPath
        {
            get { return GetPath("SatElevationPath", @"Data\Sample\hers0010.18o_SatEle.txt.xls"); }
            set { SetPath(value, "SatElevationPath"); }
        }
        /// <summary>
        /// 是否下载多余的IGS产品
        /// </summary>
        public bool IsDownloadingSurplurseIgsProducts
        {
            get { return GetBool("IsDownloadingSurplurseIgsProducts",false); }
            set { SetVal(value, "IsDownloadingSurplurseIgsProducts", IgsProduct); }
        }
        /// <summary>
        /// 是否采用30秒采样率的钟差
        /// </summary>
        public bool IsUseClk30s
        {
            get { return GetBool("IsUseClk30s", true); }
            set { SetVal(value, "IsUseClk30s", IgsProduct); }
        }


        #endregion

        #region 测试数据
        /// <summary>
        /// 例子Opt文件
        /// </summary>
        public string SampleOptFile
        {
            get { return GetPath("SampleOptFile", @"Data\GNSS\Options\无电离层组合PPP.opt"); }
            set { SetPath(value, "SampleOptFile"); }
        }
        /// <summary>
        /// 测站文件路径
        /// </summary>
        public string SiteFilePath
        {
            get { return GetPath("Site"); }
            set { SetPath(value, "Site"); }
        }

        /// <summary>
        /// SINEX  例子路径
        /// </summary>
        public string SampleSinexFile
        {
            get { return GetPath("SampleSinexFile"); }
            set { SetPath(value, "SampleSinexFile"); }
        }
        /// <summary>
        /// 示例O文件路径
        /// </summary>
        public string SampleOFile
        {
            get { return GetPath("SampleOFile"); }
            set { SetPath(value, "SampleOFile"); }
        }
        /// <summary>
        /// O文件 Rinex V 3.0
        /// </summary>
        public string SampleOFileV3A
        {
            get { return GetPath("SampleOFileV3A"); }
            set { SetPath(value, "SampleOFileV3A"); }
        }
        /// <summary>
        ///  Clock 文件 Rinex V 3.0
        /// </summary>
        public string SampleClkFileV3
        {
            get { return GetPath("SampleClkFileV3"); }
            set { SetPath(value, "SampleClkFileV3"); }
        }
        /// <summary>
        ///  O文件 B Rinex V 3.0
        /// </summary>
        public string SampleOFileV3B
        {
            get { return GetPath("SampleOFileV3B"); }
            set { SetPath(value, "SampleOFileV3B"); }
        }
        /// <summary>
        ///  N 文件 Rinex V 3.0
        /// </summary>
        public string SampleNFileV3
        {
            get { return GetPath("SampleNFileV3"); }
            set { SetPath(value, "SampleNFileV3"); }
        }
        /// <summary>
        ///  O文件 Rinex 
        /// </summary>
        public string SampleOFileA
        {
            get { return GetPath("SampleOFileA"); }
            set { SetPath(value, "SampleOFileA"); }
        }
        /// <summary>
        ///  O文件 Rinex 
        /// </summary>
        public string SampleOTableFile
        {
            get { return GetPath("SampleOTableFile"); }
            set { SetPath(value, "SampleOTableFile"); }
        }
        /// <summary>
        ///  O文件 Rinex 
        /// </summary>
        public string SampleOFileB
        {
            get { return GetPath("SampleOFileB"); }
            set { SetPath(value, "SampleOFileB"); }
        }
        /// <summary>
        ///  N 文件 Rinex 
        /// </summary>
        public string SampleNFile
        {
            get { return GetPath("SampleNFile"); }
            set { SetPath(value, "SampleNFile"); }
        }
        /// <summary>
        ///  Sp3 文件 Rinex 
        /// </summary>
        public string SampleSP3File
        {
            get { return GetPath("SampleSP3File"); }
            set { SetPath(value, "SampleSP3File"); }
        }
        /// <summary>
        ///  ERP 文件 Rinex 
        /// </summary>
        public string SampleErpFile
        {
            get { return GetPath("SampleErpFile",  @"Data\GNSS\igs17217.erp"); }
            set { SetPath(value, "SampleErpFile"); }
        }
        /// <summary>
        ///  clk 文件 Rinex
        /// </summary>
        public string SampleClkFile
        {
            get { return GetPath("SampleClkFile"); }
            set { SetPath(value, "SampleClkFile"); }
        }
        public string SampleLbhFile
        {
            get { return GetPath("SampleLbhFile", @"Data\Sample\Samlpe.lbh"); }
            set { SetPath(value, "SampleLbhFile"); }
        }
        public string SampleXyzFile
        {
            get { return GetPath("SampleXyzFile"); }
            set { SetPath(value, "SampleXyzFile"); }
        }
        public string SampleVectorFile
        {
            get { return GetPath("SampleVectorFile"); }
            set { SetPath(value, "SampleVectorFile"); }
        }
        public string SampleGofFile
        {
            get { return GetPath("SampleGofFile"); }
            set { SetPath(value, "SampleGofFile"); }
        }
        #endregion

        #region 通用数据源

        /// <summary>
        /// VMF1 文件
        /// </summary>
        public string VMF1Directory
        {
            get { return GetPath("VMF1File"); }
            set { SetPath(value, "VMF1File"); }
        }
        /// <summary>
        /// GPT2File 文件
        /// </summary>
        public string GPT2Directory
        {
            get { return GetPath("GPT2File", @"Data\GNSS\Common\gpt2_5.grd"); }
            set { SetPath(value, "GPT2File"); }
        }
        /// <summary>
        /// GPT2File 文件
        /// </summary>
        public string GPT21DegreeDirectory
        {
            get { return GetPath("GPT21DegreeFile", @"Data\GNSS\Common\gpt2_1wA.grd"); }
            set { SetPath(value, "GPT21DegreeFile"); }
        }
        /// <summary>
        /// DCB 文件目录。
        /// </summary>
        public string DcbDirectory
        {
            get { return GetPath("DcbDirectory"); }
            set { SetPath(value, "DcbDirectory"); }
        }
        /// <summary>
        /// GLONASS 频率文件路径。默认。
        /// </summary>
        public string GlonassSlotFreqFile
        {
            get { return GetPath("GlonassSlotFreqFile", @"Data\GNSS\Common\GlonassSlotFreq.txt.xls"); }
            set { SetPath(value, "GlonassSlotFreqFile"); }
        }
        /// <summary>
        /// 天线文件路径。默认。
        /// </summary>
        public string AntennaFile
        {
            get { return GetPath("AntennaFile"); }
            set { SetPath(value, "AntennaFile"); }
        }
        /// <summary>
        /// 天线文件路径 Igs14。
        /// </summary>
        public string AntennaFileIgs14
        {
            get { return GetPath("AntennaFileIgs14", @"Data\GNSS\Common\igs14.atx"); }
            set { SetPath(value, "AntennaFileIgs14"); }
        }
        /// <summary>
        /// 天线文件路径Igs08。
        /// </summary>
        public string AntennaFileIgs08
        {
            get { return GetPath("AntennaFileIgs08", @"Data\GNSS\Common\igs08.atx"); }
            set { SetPath(value, "AntennaFileIgs08"); }
        }
        /// <summary>
        /// 卫星状态文件
        /// </summary>
        public string SatSateFile
        {
            get { return GetPath("SatSateFile"); }
            set { SetPath(value, "SatSateFile"); }
        }
        /// <summary>
        /// 排除卫星的文件记录地址
        /// </summary>
        public string SatExcludeFile
        {
            get { return GetPath("SatExcludeFile"); }
            set { SetPath(value, "SatExcludeFile"); }
        }
        /// <summary>
        /// 海洋潮汐文件
        /// </summary>
        public string OceanTideFile
        {
            get { return GetPath("OceanTideFile"); }
            set { SetPath(value, "OceanTideFile"); }
        }
        /// <summary>
        /// 测站坐标文件
        /// </summary>
        public string SiteCoordFile
        {
            get { return GetPath("SiteCoordFile"); }
            set { SetPath(value, "SiteCoordFile"); }
        }
        /// <summary>
        /// GNSSer 历元电离层文件
        /// </summary>
        public string GnsserEpochIonoPath
        {
            get { return GetPath("GnsserEpochIonoPath", @"Data\Sample\hers0010.18O_Param_Iono.txt.xls"); }
            set { SetPath(value, "GnsserEpochIonoPath"); }
        }
        /// <summary>
        /// PPP定位结果路径，累计保存
        /// </summary>
        public string PppResultFile
        {
            get { return GetPath("PppResultFile"); }
            set { SetPath(value, "PppResultFile"); }
        }
        /// <summary>
        /// PPP定位结果路径，累计保存
        /// </summary>
        public string TempPppResultPath
        {
            get { return GetPath("TempPppResultPath", Path.Combine(Geo.Setting.TempDirectory, "PppResult_GnssResults.xls")); }
            set { SetPath(value, "TempPppResultPath"); }
        }
        #endregion

        #region 分布式数据
        const string Distribute = "Distribute";
        /// <summary>
        /// ServerIp
        /// </summary>
        public string ControlIp
        {
            get { return GetVal("ControlIp"); }
            set { SetVal(value, "ControlIp", Distribute); }
        }
        /// <summary>
        /// ServerPort
        /// </summary>
        public int ControlPort
        {
            get { return GetInt("ControlPort"); }
            set { SetVal(value, "ControlPort", Distribute); }
        }
        /// <summary>
        /// FtpServerUrl
        /// </summary>
        public string FtpServerUrl
        {
            get { return GetVal("FtpServerUrl"); }
            set { SetVal(value, "FtpServerUrl", Distribute); }
        }

        /// <summary>
        /// 计算节点路径
        /// </summary>
        public string ComputeNodeFilePath
        {
            get { return GetPath("ComputeNode"); }
            set { SetPath(value, "ComputeNode", Distribute); }
        }
        /// <summary>
        /// 工作文件路径
        /// </summary>
        public string TaskFilePath
        {
            get { return GetPath("Task"); }
            set { SetPath(value, "Task", Distribute); }
        }
        /// <summary>
        /// 工作文件路径
        /// </summary>
        public string GofTaskFilePath
        {
            get { return GetPath("GofTask"); }
            set { SetPath(value, "GofTask", Distribute); }
        }
        #endregion

        #region 分区平差

        const string Adjustment = "Adjustment";

        /// <summary>
        /// 分区公共文件
        /// </summary>
        public string BlockCommonSite
        {
            get { return GetVal("BlockCommonSite"); }
            set { SetVal(value, "BlockCommonSite", Adjustment); }
        }
        /// <summary>
        /// 分区文件
        /// </summary>
        public string BlockSite
        {
            get { return GetVal("BlockSite"); }
            set { SetVal(value, "BlockSite", Adjustment); }
        }
        #endregion


        /// <summary>
        /// DOP 文件路径
        /// </summary>
        public string DopFilePath
        {
            get { return GetPath("DopFilePath", @"Data\GNSS\Common\G_DOPS_at_2016.01.01.01.00.00.txt.xls"); }
            set { SetPath(value, "DopFilePath"); }
        }
        /// <summary>
        /// IGN 宽巷文件
        /// </summary>
        public string IgnWideLaneFile {
            get { return GetPath("IgnWideLaneFile", @"Data\GNSS\Common\Wide_lane_GPS_satellite_biais.wsb"); }
            set { SetPath(value, "IgnWideLaneFile"); }
        }



        #region 获取


        /// <summary>
        ///获取IGS产品数据源选项。
        /// </summary>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public IgsProductSourceOption GetIgsProductSourceOption(SessionInfo sessionInfo)
        {
            return GetIgsProductSourceOption(sessionInfo.TimePeriod, sessionInfo.SatelliteTypes);
        }

        /// <summary>
        /// 获取IGS星历数据源选项
        /// </summary>
        /// <returns></returns>
        public IgsProductSourceOption GetIgsProductSourceOption(BufferedTimePeriod TimePeriod, List<SatelliteType> SatelliteTypes)
        {
            var option = new IgsProductSourceOption(TimePeriod, SatelliteTypes)
            {
                IgsProductLocalDirectories = IgsProductLocalDirectories,// this.GetIgsProductLocalDirectory().FilePath,
                IgsProductLocalDirectory = IgsProductLocalDirectory,// this.GetIgsProductLocalDirectory().FilePath,
                IgsProductSourceDic = GetSysBasedIgsProductSourceDic(),
                // IgsProductSources = option.IgsProductSourceDic[SatelliteType.U],
                IgsProductUrlDirectories = this.IgsProductUrlDirectories,
                IgsProductUrlModels = this.IgsProductUrlModels,
                IsDownloadingSurplurseIgsProducts = this.IsDownloadingSurplurseIgsProducts,
                IsUniqueSource = this.IsUniqueSource,
                IndicatedSourceCode = this.IndicatedSourceCode,// "ig",
                Sp3EphMaxBreakingCount = this.Sp3EphMaxBreakingCount,
                //MinSequentialSatCount = this.MinSequentialSatCount,
                IsSkipIonoContent = IsSkipIonoContent,
            };
            return option;
        }

        /// <summary>
        /// 从设置文件中读取星历数据源的配置
        /// </summary>
        /// <returns></returns>
        public  Dictionary<SatelliteType, List<string>> GetSysBasedIgsProductSourceDic()
        {
            Dictionary<SatelliteType, List<string>> sp3FileNameDic = new Dictionary<SatelliteType, List<string>>();
            string[] Sources = this.IgsProductSource.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string[] gpsSources = this.GpsEphemerisSource.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] beidouSources = this.BeidouEphemerisSource.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] glonassSources = this.GlonassEphemerisSource.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] galieoSources = this.GalieoEphemerisSource.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            sp3FileNameDic[SatelliteType.U] = new List<string>(Sources);
            sp3FileNameDic[SatelliteType.G] = new List<string>(gpsSources);
            sp3FileNameDic[SatelliteType.C] = new List<string>(beidouSources);
            sp3FileNameDic[SatelliteType.R] = new List<string>(glonassSources);
            sp3FileNameDic[SatelliteType.E] = new List<string>(galieoSources);

            return sp3FileNameDic;
        }
        #endregion

    }
}
