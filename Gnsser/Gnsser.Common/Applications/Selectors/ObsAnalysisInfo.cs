//2016.11.25, czs & cuiyang, create  in hongqing, 观测文件分析专家

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Geo.Common;
using Geo.Coordinates;
using Geo;
using Geo.IO;
using Geo.Times;
using Gnsser.Data.Rinex;
using Gnsser.Models;
using Gnsser.Domain;

namespace Gnsser
{
    /// <summary>
    /// 观测文件分析结果,可以在测站选择，基线等组合中使用。
    /// 是分析的结果，也可以直接从观测文件头部读取。
    /// </summary>
    public class ObsAnalysisInfo
    {        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ObsAnalysisInfo(string rinexFile):this( new RinexObsFileReader(rinexFile, false).GetHeader() )
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="header"></param>
        public ObsAnalysisInfo(RinexObsFileHeader header)
        {
            this.SiteObsInfo = header;
            WidthFixedParamReader reader = new WidthFixedParamReader();
            var paramDic = reader.GetParamFromHeader(header);

            this.MultipathFactors = new Dictionary<FrequenceType, double>();
            if (paramDic.ContainsKey("A_MP"))
            {
                this.MultipathFactors.Add(FrequenceType.A, Double.Parse(paramDic["A_MP"]));
                this.MultipathFactors.Add(FrequenceType.B, Double.Parse(paramDic["B_MP"]));
            }
        }
        #region 属性
        /// <summary>
        /// 文件基本信息
        /// </summary>
        public SiteObsInfo SiteObsInfo { get; set; }

        /// <summary>
        /// 多路径分析结果
        /// </summary>
        public Dictionary<FrequenceType, double> MultipathFactors { get; set; }
        /// <summary>
        /// 是否具有多路径计算参数
        /// </summary>
        public bool HasMultipathFactor { get { return MultipathFactors != null && MultipathFactors.Count == 2; } }
        #endregion

        /// <summary>
        /// 待输出的参数。
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> BuildParamDics()
        {
            Dictionary<string, string> paramDic = new Dictionary<string, string>();
            //多路径
            foreach (var kv in MultipathFactors)
            {
                paramDic.Add(kv.Key + "_MP", kv.Value.ToString("0.000"));
            }
            //paramDic.Add("EpochCount", this.Count + "");
            return paramDic;
        }

        #region  IO
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        public void WriteAsRinexCommentFile(string path)
        {
            var builder = new WidthFixedParamLineBuilder();
            Dictionary<string, string> paramDic = BuildParamDics();

            builder.Params = paramDic;
            var result = builder.ToMarkedLineString();
            File.WriteAllText(path, result);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="rinexFile"></param>
        public static ObsAnalysisInfo ParseRinexCommentFile(string rinexFile)
        {
            WidthFixedParamReader reader = new WidthFixedParamReader();
            var paramDic = reader.ParseFromRinexOFile(rinexFile);

            ObsAnalysisInfo info = new ObsAnalysisInfo(rinexFile);

            info.MultipathFactors = new Dictionary<FrequenceType, double>();
            if (paramDic.ContainsKey("A_MP"))
            {
                info.MultipathFactors.Add(FrequenceType.A, Double.Parse(paramDic["A_MP"]));
                info.MultipathFactors.Add(FrequenceType.B, Double.Parse(paramDic["B_MP"]));

            }
            return info;
        }

        #endregion
        /// <summary>
        /// 将统计结果更新写入到观测文件中。
        /// </summary>
        /// <param name="existOFilePath"></param>
        /// <param name="newOfilePath"></param>
        internal void UpdateToRinexOFileHeader(string existOFilePath, string newOfilePath)
        {
            WidthFixedParamLineBuilder manager = new WidthFixedParamLineBuilder();
            manager.Params = BuildParamDics();
            manager.UpdateToRinexOFileHeader(existOFilePath, newOfilePath);
        }

    }


    /// <summary>
    /// 观测文件分析结果，逐个历元分析的集合
    /// </summary>
    public class ObsAnalysisInfoCollection : BaseDictionary<Time, EpochAnlysisInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ObsAnalysisInfoCollection(string FileName)
        {
            this.FilePath = FileName;
            this.SiteObsInfo = new RinexObsFileReader(FileName, false).GetHeader();
        }
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="header"></param>
        public ObsAnalysisInfoCollection(RinexObsFileHeader header, string FileName = null)
        {
            this.FilePath = FileName;
            this.SiteObsInfo = header;
        }
        #region 属性
        /// <summary>
        /// 文件基本信息
        /// </summary>
        public SiteObsInfo SiteObsInfo { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get { return Path.GetFileName(FilePath); } }
        #endregion
        /// <summary>
        /// 创建结果。如统计多路径。
        /// </summary>
        public ObsAnalysisInfo BuildResult()
        { 
            return new ObsAnalysisInfo(FilePath)
            {
                 MultipathFactors = GetMultipathFactors()
            };
        }

        /// <summary>
        /// 多路径效应因子
        /// </summary>
        private Dictionary<FrequenceType, double> GetMultipathFactors()
        {
            ObjectTableStorage table = BuildObjectTable();
            var aveRms = table.GetAveragesWithStdDev();

            var table2 = BuildObjectTable2();
            var OutputPath = Path.Combine(Setting.GnsserConfig.TempDirectory, FileName + "_Mp.xls");
            var writer = new ObjectTableWriter(OutputPath);
            writer.Write( table2);

            Dictionary<FrequenceType, double> data = new Dictionary<FrequenceType, double>();
            foreach (var val in aveRms)
            {
               var type =   Geo.Utils.EnumUtil.Parse<FrequenceType>( val.Key);
               data.Add(type, val.Value[1]); 
            }

            return data;
        }

        private ObjectTableStorage BuildObjectTable2()
        {
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var item in this)
            {
                var epcoh = item;
                table.NewRow();
                foreach (var sat in epcoh)
                {
                    foreach (var facs in sat.MultipathFactors)
                    {
                        table.AddItem(sat.Prn + "_" + facs.Key, facs.Value);
                    }
                }
                table.EndRow();
            }
            return table;
        } 
        private ObjectTableStorage BuildObjectTable()
        {
            ObjectTableStorage table = new ObjectTableStorage();
            foreach (var item in this)
            {
                var epcoh = item;
                foreach (var sat in epcoh)
                {
                    table.NewRow();
                    foreach (var facs in sat.MultipathFactors)
                    {
                        table.AddItem(facs.Key, facs.Value);
                    }
                    table.EndRow();
                }
            }
            return table;
        } 

    }



    /// <summary>
    /// 历元统计分析结果。
    /// </summary>
    public class EpochAnlysisInfo : BaseDictionary<SatelliteNumber, EpochSatAnlysisInfo>
    {

    }
    
    /// <summary>
    ///当前历元的单卫星统计分析结果 
    /// </summary>
    public class EpochSatAnlysisInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sat"></param>
        public EpochSatAnlysisInfo(EpochSatellite sat)
        {
            this.Prn = sat.Prn;
            //计算多路径因子
            var freA = sat.FrequenceA;
            var freB = sat.FrequenceB;
            var alfaTemp = (freA.Frequence.Value / freB.Frequence.Value);
            var alfa = alfaTemp * alfaTemp;
            var factorL1 = GetMultpathFactorL1(freA.PseudoRange.Value, freA.PhaseRange.Value, freB.PhaseRange.Value, alfa);
            var factorL2 = GetMultpathFactorL2(freA.PseudoRange.Value, freA.PhaseRange.Value, freB.PhaseRange.Value, alfa);

            this.MultipathFactors = new Dictionary<FrequenceType, double>();

            MultipathFactors.Add(freA.FrequenceType, factorL1);
            MultipathFactors.Add(freB.FrequenceType, factorL2);

            //判断是否有效
            InvalidCodes = new Dictionary<FrequenceType, List<GnssCodeType>>();
            foreach (var freq in sat)
            {
                var list = new List<GnssCodeType>();
                InvalidCodes[freq.FrequenceType] = list;
                foreach (var item in freq)
                {
                    foreach (var item2 in item)
                    {
                        if (!IsValid(item2.Value))
                        {
                            list.Add(item2.GnssCodeType);
                        }
                    }
                }
            }
        }

        #region 属性，分析结果
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get; set; }

        /// <summary>
        /// 多路径效应因子
        /// </summary>
        public Dictionary<FrequenceType, double> MultipathFactors { get; set; }

        /// <summary>
        /// 无效的观测类型集合
        /// </summary>
        public Dictionary<FrequenceType, List<GnssCodeType>> InvalidCodes { get; set; }
        #endregion

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Prn + " MultipathFactors: " + MultipathFactors.Count + " InvalidCodes:" + InvalidCodes.Count;
        }

        #region 工具方法
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        static public bool IsValid(double val)
        {
            if (val == 0) { return false; }

            return  Geo.Utils.DoubleUtil.IsValid(val);
        }

        /// <summary>
        /// 计算载波1的多路径因子
        /// </summary>
        /// <param name="rangeA"></param>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <param name="alfa"></param>
        /// <returns></returns>
        public static double GetMultpathFactorL1(double rangeA, double L1, double L2, double alfa)
        {
            var temp = 2 / (alfa - 1);
            return rangeA - (1 + temp) * L1 + temp * L2;
        }
        /// <summary>
        /// 计算载波2的多路径因子
        /// </summary>
        /// <param name="rangeB"></param>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <param name="alfa"></param>
        /// <returns></returns>
        public static double GetMultpathFactorL2(double rangeB, double L1, double L2, double alfa)
        {
            var temp = 2 * alfa / (alfa - 1);
            return rangeB - temp * L1 + (temp - 1)* L2;
        }

        #endregion



    }
}
