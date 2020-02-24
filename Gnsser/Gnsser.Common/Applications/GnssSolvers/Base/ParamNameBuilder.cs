//2016.03.11, czs, create in hongqing, PPP参数命名器
//2017.09.05.23, czs, edit in hongqing, 删掉一些，逐渐精简
//2018.10.31, czs, edit in hmx, 增加差分、观测值命名，整理代码

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Data.Sinex;
using Gnsser.Data.Rinex;
using Gnsser.Times;
using Geo.Algorithm;
using Geo.Coordinates;
using  Geo.Algorithm.Adjust;
using Geo;
using Geo.Times;

namespace Gnsser.Service
{
    /// <summary>
    /// 参数名称生成器
    /// </summary>
    public abstract class GnssParamNameBuilder : ParamNameBuilder
    {
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        /// <param name="option"></param>
        public GnssParamNameBuilder(GnssProcessOption option)
        {
            this.Option = option;
            this.SystemCount = option.SatelliteTypes.Count;
            //先默认第一个做基准卫星系统
            this.IsSameTimeSystemInMultiGnss = option.IsSameTimeSystemInMultiGnss;
            this.BaseSatType = option.SatelliteTypes[0];
            this.IsSiteNameIncluded = this.Option.IsSiteNameIncluded;
        }
        /// <summary>
        /// 是否包含测站名称
        /// </summary>
        public bool IsSiteNameIncluded { get; set; }

        #region 赋值操作
        /// <summary>
        /// 设置基准星
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public GnssParamNameBuilder SetBasePrn(SatelliteNumber basePrn)
        {
            this.BasePrn = basePrn;
            return this;
        }
        /// <summary>
        /// 设置基准星
        /// </summary>
        /// <param name="BaseSiteName"></param>
        /// <returns></returns>
        public GnssParamNameBuilder SetBaseSiteName(string BaseSiteName)
        {
            this.BaseSiteName = BaseSiteName;
            return this;
        }
        /// <summary>
        /// 设置历元
        /// </summary>
        /// <param name="Epoches"></param>
        /// <returns></returns>
        public GnssParamNameBuilder SetEpoches(List<Time> Epoches)
        {
            this.Epoches = Epoches;
            return this;
        }
        /// <summary>
        /// 设置卫星名称。
        /// </summary>
        /// <param name="enabledPrns"></param>
        /// <returns></returns>
        public GnssParamNameBuilder SetPrns(List<SatelliteNumber> enabledPrns)
        {
            this.EnabledPrns = enabledPrns;
            return this;
        }
        /// <summary>
        /// 设置原材料
        /// </summary>
        /// <typeparam name="TMaterial"></typeparam>
        /// <param name="currentMaterial"></param>
        public GnssParamNameBuilder SetMaterial<TMaterial>(TMaterial currentMaterial) where TMaterial : ISiteSatObsInfo
        {
            Material = currentMaterial; return this;
        }
        /// <summary>
        /// 设置卫星类型
        /// </summary>
        /// <param name="enabledPrns"></param>
        /// <returns></returns>
        public GnssParamNameBuilder SetSatelliteTypes(List<SatelliteType> enabledPrns)
        {
            this.SatelliteTypes = enabledPrns;
            return this;
        }
        #endregion

        #region 常用基本属性
        /// <summary>
        /// 多系统是否采用相同的时间基准。
        /// </summary>
        public bool IsSameTimeSystemInMultiGnss { get; set; }
        /// <summary>
        /// 系统数量
        /// </summary>
        public int SystemCount { get; set; }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        ///原材料对象
        /// </summary>
        public ISiteSatObsInfo Material { get; set; }
        /// <summary>
        /// 基准卫星系统
        /// </summary>
        public SatelliteType BaseSatType { get; set; }
        /// <summary>
        /// 卫星系统
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get; set; }

        /// <summary>
        /// 历元
        /// </summary>
        public List<Time> Epoches { get; set; }
        /// <summary>
        /// 基准测站名称，用于网解。
        /// </summary>
        public string BaseSiteName { get; set; }

        /// <summary>
        /// 基准卫星编号
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }

        /// <summary>
        /// 基础参数的总数，即除了模糊度的剩余参数的个数
        /// 长基线时是5，即三个坐标参数+两个对流层参数
        /// 短基线时是3，即三个坐标参数
        /// </summary>
        public int BaseParamCount { get; set; }
        /// <summary>
        /// 可用卫星
        /// </summary>
        public List<SatelliteNumber> EnabledPrns { get; set; }
        #endregion

        #region  观测类型名称
        /// <summary>
        /// 双差 P 码名称
        /// </summary> 
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public string GetDoubleDifferObsPCodeName(SatelliteNumber prn, SatelliteNumber basePrn) { return GetDoubleDifferObsCodeNameOf(prn, basePrn, ParamNames.PCode); }

        /// <summary>
        /// 双差 P 码名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="baseSiteName">基准测站</param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public string GetDoubleDifferObsPCodeName(string siteName, string baseSiteName, SatelliteNumber prn, SatelliteNumber basePrn) { return GetDoubleDifferObsCodeNameOf(siteName, baseSiteName, prn, basePrn, ParamNames.PCode); }

        /// <summary>
        /// 双差 L 码名称
        /// </summary> 
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public string GetDoubleDifferObsLCodeName(SatelliteNumber prn, SatelliteNumber basePrn) { return GetDoubleDifferObsCodeNameOf(prn, basePrn, ParamNames.LCode); }

        /// <summary>
        /// 双差 L 码名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="baseSiteName">基准测站</param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public string GetDoubleDifferObsLCodeName(string siteName, string baseSiteName, SatelliteNumber prn, SatelliteNumber basePrn) { return GetDoubleDifferObsCodeNameOf(siteName, baseSiteName, prn, basePrn, ParamNames.LCode); }
        /// <summary>
        /// 差分 P 码名称，星间单差
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn">基准星</param>
        /// <returns></returns>
        public string GetDifferObsPCodeName(string siteName, SatelliteNumber prn, SatelliteNumber basePrn) { return GetDifferObsCodeName(siteName, prn, basePrn, ParamNames.PCode); }
        /// <summary>
        /// 差分 L 码名称,星间单差
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn">基准星</param>
        /// <returns></returns>
        public string GetDifferObsLCodeName(string siteName, SatelliteNumber prn, SatelliteNumber basePrn) { return GetDifferObsCodeName(siteName, prn, basePrn, ParamNames.LCode); }
        /// <summary>
        /// 差分 P 码名称，站间单差
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="baseSiteName">基准站</param>
        /// <returns></returns>
        public string GetDifferObsPCodeName(string siteName, string baseSiteName, SatelliteNumber prn) { return GetDifferObsCodeName(siteName, baseSiteName, prn, ParamNames.PCode); }
        /// <summary>
        /// 差分 L 码名称,站间单差
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="baseSiteName">基准站</param>
        /// <returns></returns>
        public string GetDifferObsLCodeName(string siteName, string baseSiteName, SatelliteNumber prn) { return GetDifferObsCodeName(siteName, baseSiteName, prn, ParamNames.LCode); }
        /// <summary>
        /// P 码名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetObsPCodeName(string siteName, SatelliteNumber prn) { return siteName + ParamNames.Divider + prn +  ParamNames.Divider + ParamNames.PCode; }
        /// <summary>
        /// L 码名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetObsLCodeName(SatelliteNumber prn) { return prn +  ParamNames.Divider + ParamNames.LCode; }
        /// <summary>
        /// P 码名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetObsPCodeName(SatelliteNumber prn) { return prn +  ParamNames.Divider + ParamNames.PCode; }
        /// <summary>
        /// L 码名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetObsLCodeName(string siteName, SatelliteNumber prn) { return siteName + ParamNames.Divider + prn + ParamNames.Divider + ParamNames.LCode; }
        /// <summary>
        /// 双差名称
        /// </summary> 
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetDoubleDifferObsCodeNameOf( SatelliteNumber prn, SatelliteNumber basePrn, string code)
        {
            return   prn + ParamNames.Pointer + basePrn + ParamNames.Divider + ParamNames.DoubleDiffer + code;
        }
        /// <summary>
        /// 载波名称
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="epochIndex"></param>
        /// <returns></returns>
        public static string GetDoubleDifferObsCodeNameOfPhase(SatelliteNumber prn, SatelliteNumber basePrn, int epochIndex)
        {
            return GetDoubleDifferObsCodeNameOf( prn,  basePrn, ParamNames.LCode, epochIndex);
        }
        /// <summary>
        /// 双差名称
        /// </summary> 
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="code"></param>
        /// <param name="epochIndex"></param>
        /// <returns></returns>
        public static string GetDoubleDifferObsCodeNameOf(SatelliteNumber prn, SatelliteNumber basePrn, string code, int epochIndex)
        {
            return prn + ParamNames.Pointer + basePrn + ParamNames.Divider + epochIndex + ParamNames.Divider + ParamNames.DoubleDiffer + code;
        }
        /// <summary>
        /// 双差名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="baseSiteName"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetDoubleDifferObsCodeNameOf(string siteName, string baseSiteName, SatelliteNumber prn, SatelliteNumber basePrn, string code)
        {
            return siteName + ParamNames.Pointer + baseSiteName + ParamNames.Divider + prn + ParamNames.Pointer + basePrn + ParamNames.Divider + ParamNames.DoubleDiffer + code;
        }
        /// <summary>
        /// 星间差分名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetDifferObsCodeName( SatelliteNumber prn, SatelliteNumber basePrn, string code)
        {
            return  prn + ParamNames.Pointer + basePrn + ParamNames.Divider + ParamNames.SingleDiffer + code;
        }
        /// <summary>
        /// 星间差分名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="basePrn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetDifferObsCodeName(string siteName, SatelliteNumber prn, SatelliteNumber basePrn, string code)
        {
            return siteName + ParamNames.Divider + prn + ParamNames.Pointer + basePrn + ParamNames.Divider + ParamNames.SingleDiffer + code;
        }
        /// <summary>
        /// 站间差分名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <param name="baseSiteName"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetDifferObsCodeName(string siteName, string baseSiteName, SatelliteNumber prn, string code)
        {
            return siteName + ParamNames.Pointer + baseSiteName + ParamNames.Divider + prn + ParamNames.Divider + ParamNames.SingleDiffer + code;
        }

        #endregion

        #region 参数名称
        #region 对流层参数      
        /// <summary>
        /// 获取测站对流层湿延迟天顶距参数（Zpd，zenith path delay）名称
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public string GetSiteWetTropZpdName(EpochInformation epoch) { return epoch.SiteName + ParamNames.Divider + ParamNames.WetTropZpd; }
        /// <summary>
        /// 获取测站对流层湿延迟天顶距参数（Zpd，zenith path delay）名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public string GetSiteWetTropZpdName(string siteName) { return siteName + ParamNames.Divider + ParamNames.WetTropZpd; }
        #endregion

        #region 系统钟差
        /// <summary>
        /// 构建系统间钟差参数
        /// </summary>
        /// <returns></returns>
        protected List<string> BuildSysTimeDifferParams()
        {
            //下面考虑多系统时间偏差
            List<string> sysTimeDifferParams = new List<string>();
            if (this.IsSameTimeSystemInMultiGnss) { return sysTimeDifferParams; }

            if (SatelliteTypes.Count >= 2)
            {
                foreach (var item in SatelliteTypes)//系统间时间偏差
                {
                    if (item == BaseSatType) { continue; }
                    var name = ParamNames.SysTimeDistDifferOf + BaseSatType + item;
                    sysTimeDifferParams.Add(name);
                }
            }
            return sysTimeDifferParams;
        }

        /// <summary>
        /// 以第一个系统为基准
        /// </summary>
        /// <param name="satTypes"></param>
        /// <returns></returns>
        public List<String> GetSysTimeRagneDifferName(List<SatelliteType> satTypes)
        {
            if (satTypes == null || satTypes.Count <= 1) { return new List<string>(); }

            var list = new List<String>();
            SatelliteType first = satTypes[0];
            foreach (var type in satTypes)
            {
                if (first == type) { continue; }
                var name = GetSysTimeRagneDifferName(first, type);
                list.Add(name);
            }

            return list;
        }
        public String GetSysTimeRagneDifferName(SatelliteType satA, SatelliteType satB)
        {
            return "" + ParamNames.SysTimeDistDifferOf + satA + satB;
        }

        #endregion

        #region 坐标参数
        /// <summary>
        /// 卫星坐标偏差，G01_Dx，G01_Dy, G01_Dz
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public List<string> GetSatDxyz(SatelliteNumber prn)
        {
            var str = prn.ToString() + ParamNames.Divider;
            return new List<string>()
            {
                str + ParamNames.Dx,
                str + ParamNames.Dy,
                str + ParamNames.Dz,
            };
        }
        /// <summary>
        /// 测站坐标偏差，HERT_Dx，HERT_Dy, HERT_Dz
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public List<string> GetSiteDxyz(string site)
        {
            var str = site.ToString() + ParamNames.Divider;
            return new List<string>()
            {
                str + ParamNames.Dx,
                str + ParamNames.Dy,
                str + ParamNames.Dz,
            };
        }
        /// <summary>
        /// 测站坐标偏差，HERT_Dx 
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public string GetSiteDx(string site)
        {
            var str = site.ToString() + ParamNames.Divider+ ParamNames.Dx;
            return str;
        }
        #endregion

        #region 模糊度
        /// <summary>
        /// 构建单差参数名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetSingleDifferAmbiParamName(String siteName, SatelliteNumber prn)
        {
            return siteName + Gnsser.ParamNames.Pointer + BaseSiteName + Gnsser.ParamNames.Divider + prn + Gnsser.ParamNames.DifferAmbiguitySuffix;
        }
        /// <summary>
        /// 构建双差参数名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetDoubleDifferAmbiParamName(String siteName, SatelliteNumber prn)
        {
            return siteName + Gnsser.ParamNames.Pointer + BaseSiteName + Gnsser.ParamNames.Divider + prn + Gnsser.ParamNames.Pointer + BasePrn + Gnsser.ParamNames.DoubleDifferAmbiguitySuffix;
        }
        /// <summary>
        /// 构建双差参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetDoubleDifferAmbiParamName(SatelliteNumber prn)
        {
            return prn + Gnsser.ParamNames.Pointer + BasePrn + Gnsser.ParamNames.DoubleDifferAmbiguitySuffix;
        }
        /// <summary>
        /// 构建单差模糊度参数名称
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetSingleDifferSatAmbiParamName(SatelliteNumber prn)
        {
            return prn + Gnsser.ParamNames.DifferAmbiguitySuffix;
        }
        /// <summary>
        /// 获取测站卫星模糊度参数名称
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetSiteSatAmbiguityParamName(EpochSatellite s)
        {
            return GetSiteSatAmbiguityParamName(s.EpochInfo.SiteName, s.Prn);
        }

        /// <summary>
        /// 测站-卫星 模糊度
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="prn"></param>
        /// <returns></returns>
        public string GetSiteSatAmbiguityParamName(string siteName, SatelliteNumber prn)
        {
            return siteName + ParamNames.Pointer + BaseSiteName + ParamNames.Divider + prn + ParamNames.AmbiguitySuffix;
        }
        #endregion

        #region 钟差  
        /// <summary>
        /// 建立接收机钟差参数名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="baseSiteName"></param>
        /// <returns></returns>
        public string GetSiteClockDiffer(string siteName, string baseSiteName)
        {
            return siteName + ParamNames.Pointer  + baseSiteName + ParamNames.Divider + ParamNames.RcvClkErrDistance;
        }

        /// <summary>
        /// 建立接收机钟差参数名称
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public string GetReceiverClockParamName(string siteName)
        {
            return siteName + ParamNames.Divider + ParamNames.RcvClkErrDistance;
        }

        /// <summary>
        /// 构建单差钟差参数名称
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetSingleDifferTimedClockParamName(Time item)
        {
            return item.ToShortTimeString() + Gnsser.ParamNames.DifferRcvClkErrDistanceSuffix;
        }

        /// <summary>
        /// 获取卫星钟参数名称, G01_cDt_s
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public string GetSatClockParamName(SatelliteNumber sat)
        {
            return sat.ToString() + ParamNames.Divider + ParamNames.SatClkErrDistance;
        }

        /// <summary>
        /// 建立接收机钟差参数名称
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public string GetReceiverClockParamName(EpochInformation epoch)
        {
            return GetReceiverClockParamName(epoch.SiteName);
        }

        #endregion

        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override string GetParamName(object obj)
        {
            if (obj is SatelliteNumber)
                return GetParamName((SatelliteNumber)obj);
            if (obj is String)
                return obj.ToString();

            return "";
        }

        public abstract string GetParamName(SatelliteNumber prn);

        /// <summary>
        /// 尝试获取卫星编号。
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        internal SatelliteNumber TryGetPrn(string paramName)
        {
            if (paramName == null) { return SatelliteNumber.Default; }
            int index = paramName.IndexOf("->");
            string str = "";
            if (index == -1)
            {
                str = paramName.Substring(0, 3);
            }
            else
            {
                str = paramName.Substring(index + 2, 3);
            }

            return SatelliteNumber.Parse(str);
        }

        #endregion


    }
}