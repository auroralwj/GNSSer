//2017.06.14, czs, create in hongqing, FCB 数据服务

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Times;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo.Times;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Gnsser.Times;
using Geo;
using Geo.Common;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// 头文件标签
    /// </summary>
    public class FcbFileHeaderLabel
    {
        public const string END_OF_HEADER = "END OF HEADER";
        public const string COMMENT = "COMMENT";
        public const string STA_NAME_LIST = "STA NAME LIST";
        public const string Num_OF_SOLN_STA = "# OF SOLN STA";
        public const string SYS_EXT_PROD_APPLIED = "SYS/EXT PROD APPLIED";
        public const string ANALYSIS_CENTER = "ANALYSIS CENTER";
        public const string RUN_BY_DATE = "RUN BY / DATE";
        public const string VERSION_TYPE = "VERSION / TYPE";
        public const string START_OF_WL_COMMENT = "Widelane Satellite Fractional Cycle Biases"; 


    }

    /// <summary>
    /// FCB 头部文件
    /// </summary>
    public class FcbFileHeader
    {
        public FcbFileHeader()
        {
            this.Comments = new List<string>();
            this.StationNames = new List<string>();
            this.DataType = "FCB DATA"; 
        }
        /// <summary>
        /// 版本
        /// </summary>
        public double Verion { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 参与计算的测站列表。
        /// </summary>
        public List<string> StationNames { get; set; }
        /// <summary>
        /// 测站数量
        /// </summary>
        public int StationCount { get { return StationNames.Count; } }
        /// <summary>
        /// 注释。
        /// </summary>
        public List<string> Comments { get; set; }

        public string RunBy { get; set; }
        public string Date { get; set; }
        public string AnalysisCenter { get; set; }
        public string System { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public SatelliteType SatType { get; set; }
        public string ExtProdApplied { get; set; }

        public WideLaneValue WideLaneValue { get; set; }
    }

    /// <summary>
    /// /WL Value
    /// </summary>
    public class WideLaneValue : BaseDictionary<SatelliteNumber, RmsedNumeral>
    {
        public WideLaneValue(Time time)
        {
            this.Time = time;
        }
        public Time Time { get; set; } 
        /// <summary>
        /// BSD
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public BaseDictionary<SatelliteNumber, RmsedNumeral> GetBsdDic(SatelliteNumber basePrn)
        {
            var result = new BaseDictionary<SatelliteNumber, RmsedNumeral>();
            if (!this.Contains(basePrn)) { return result; }
            var baseVal = this[basePrn];
            foreach (var item in this.KeyValues)
            {
                result[item.Key] = item.Value - baseVal;
            }
            return result;
        }
    }
}