//2014.10.24， czs, create in namu shuangliao, 数据源配置
//2015.05.12, czs, edit in namu, GNSS 文件类型 采用string 名称描述，利于扩展，命名改为 GnssDataType
//2017.08.23, czs, add in hongqing, 增加电离层文件服务
//2017.09.04, lly, add in zz, GPT2模型 1度分辨率

using System;
using System.Collections.Generic;
using System.Linq;
using Geo;
using System.Text;

namespace Gnsser
{

    /// <summary>
    /// GNSS 数据源类型，注意：这只是一个字符串标识，你可以采用任何字符串而非此变量。
    /// </summary>
    public static class GnssDataType
    {
        /// <summary>
        /// Rinex 到导航文件
        /// </summary>
        public static string RinexNav = "RinexNav";
        /// <summary>
        /// 观测数据源
        /// </summary>
        public static string Observation = "Observation"; 
        /// <summary>
        /// 钟差数据源
        /// </summary>
        public static string IgsAutoClock = "IgsAutoClock";
        /// <summary>
        /// Ephemeris 星历
        /// </summary>
        public static string IgsAutoEphemeris = "IgsAutoEphemeris";
        public static string SecondEphemerisService = "SecondEphemerisService";
        /// <summary>
        /// Ephemeris 星历
        /// </summary>
        public static string IndicatedEphemeris = "IndicatedEphemeris";
        /// <summary>
        /// 海洋潮汐数据源
        /// </summary>
        public static string OceanLoading = "OceanLoading";
        /// <summary>
        /// 天线数据源
        /// </summary>
        public static string Antenna = "Antenna";
        /// <summary>
        /// 排除卫星数据源
        /// </summary>
        public static string ExcludeSat = "ExcludeSat";
        /// <summary>
        /// GPS卫星状态数据源
        /// </summary>
        public static string GpsSatState = "GpsSatState";
        /// <summary>
        /// DCB硬件延迟数据源
        /// </summary>
        public static string DCB = "DCB";
        /// <summary>
        /// 精密星历 SP3 目录。
        /// </summary>
        public static string Sp3Directory = "Sp3Directory";
        /// <summary>
        /// 地球自转信息数据源
        /// </summary>
        public static string IgsAutoErp = "IgsAutoErp";
        /// <summary>
        /// VFM1对流层模型
        /// </summary>
        public static string VMF1 = "VMF1";
        /// <summary>
        /// GPT2对流层模型
        /// </summary>
        public static string GPT2 = "GPT2";
        /// <summary>
        /// GPT2对流层模型 1度分辨率
        /// </summary>
        public static string GPT21Degree = "GPT21Degree";

        /// <summary>
        /// UniverseObject
        /// </summary>
        public static string UniverseObject = "UniverseObject";
        /// <summary>
        /// SatState卫星状态
        /// </summary>
        public static string SatState = "SatState";
        /// <summary>
        /// SatInfo
        /// </summary>
        public static string SatInfo = "SatInfo";
        /// <summary>
        /// SiteCoord
        /// </summary>
        public static string SiteCoord = "SiteCoord";
        /// <summary>
        /// StationInfo
        /// </summary>
        public static string StationInfo = "StationInfo";

        public static string IgsGridIonoFile = "IgsIonoFile";
        public static string IgsGridAutoFile = "IgsGridAutoFile";

        public static string IonoParam = "IonoParam";
        public static string TropAug = "TropAug";
        internal static string AntennaFileIgs08 = "AntennaFileIgs08";
        internal static string AntennaFileIgs14 = "AntennaFileIgs14";
        internal static string IgsAutoSimpleClock = "IgsAutoSimpleClock";
        internal static string IgsGridIonoDcbService = "IgsGridIonoDcbService";
        internal static string IgsCodeIonoHarmoFile= "IgsCodeIonoHarmoFile";
        internal static string IgsIonoFileService= "IgsIonoFileService";
        internal static string IgsKlobucharIonoService = "IgsKlobucharIonoService";
        internal static string IgsNavEphemerisService = "NavEphemerisService";
        internal static string KlobucharIonoService= "KlobucharIonoService";
        internal static string GnsserFcbOfUpdService = "FcbOfUpdService";
    }
}