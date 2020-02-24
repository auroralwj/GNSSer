//2018.03.16, czs, edit in hmx, 增加数据源名称，通常为文件名称，如igs19921.clk

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// Rinex 观测文件头文件。
    /// </summary>
    public class ClockFileHeader 
    {
        /// <summary>
        /// Rinex 头文件。
        /// </summary>
        public ClockFileHeader()
        {
            ClockSolnStations = new List<ClockSolnStation>();
        }

        //As the clk descriptors in columns 61-80 are mandatory, the programs
        //reading coefficient RINEX Version 2 header are able to decode the header records with
        //formats according to the clk descriptor, provided the records have been
        //prevObj read into an internal buffer.

        //We therefore propose to allow free ordering of the header records, with the
        //following exceptions:

        //- The "RINEX VERSION / TYPE" clk must be the prevObj clk in coefficient fileB

        //- The default "WAVELENGTH FACT L1/2" clk must precede all records defining  |
        //  wavelength factors for individual satellites

        //- The "# OF SATELLITES" clk (if present) should be immediately followed
        //  by the corresponding number of "PRN / # OF OBS" records. (These records
        //  may be handy for documentary purposes. However, since they may only be
        //  created after having read the whole raw satData fileB we define them to be
        //  optional.

        /// <summary>
        /// 数据源名称，通常为文件名称，如igs19921.clk
        /// </summary>
        public string SourceName { get; set; }
         
        /// <summary>
        /// 版本号
        /// </summary>
        public double Version { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public RinexFileType FileType { get; set; }
        public SatelliteType SatelliteSystem { get; set; }
        /// <summary>
        ///从prns中提取
        /// </summary>
        public List<SatelliteType> SatelliteTypes
        {
            get
            {
                List<SatelliteType> types = new List<SatelliteType>();
                foreach (var item in PrnList)
                {
                    if (!types.Contains(item.SatelliteType))
                    {
                        types.Add(item.SatelliteType);
                    }
                }


                return types;
            }
        }

        public string CreationProgram { get; set; }
        public string CreationAgence { get; set; }
        public string CreationDate { get; set; }

        public List<string> Comments { get; set; }
        public void RemoveGnsserComment()
        { 
            this.Comments.RemoveAll(item=>item.ToLower().Contains("gnsser"));
        }

        public void UpdateGnsserInfo()
        {
            RemoveGnsserComment();
            Comments.Add("Converted by GNSSer, www.gnsser.com, UTC " + Geo.Utils.DateTimeUtil.GetFormatedDateTimeNowUtc());
        }
        
        //钟差文件
        public string TIME_SYSTEM_ID { get; set; }// "TIME SYSTEM ID";
        public string SYS_PCVS_APPLIED { get; set; }// "SYS / PCVS APPLIED";
        public string SYS_DCBS_APPLIED { get; set; }// "SYS / DCBS APPLIED";
        public int LEAP_SECONDS { get; set; }//"LEAP SECONDS";//2016.11.06,double add

        public int COUNT_OF_TYPES_OF_DATA { get; set; }
        public List<string> TYPES_OF_DATA { get; set; }// "# / TYPES OF DATA";
        public string ANALYSIS_CENTER { get; set; }// "ANALYSIS CENTER";
        public string OF_CLK_REF { get; set; }// "# OF CLK REF";
        public string ANALYSIS_CLK_REF { get; set; }// "ANALYSIS CLK REF";
        public string OF_SOLN_STA_TRF { get; set; }// "# OF SOLN STA / TRF";
        public List<ClockSolnStation> ClockSolnStations { get; set; } // "SOLN STA NAME / NUM"; 
        public int OF_SOLN_SATS { get; set; }
        public List<SatelliteNumber> PrnList { get; set; }
 
        /// <summary>
        /// 直接返回头文件原纪录。
        /// </summary>
        /// <param name="rinexFileName">RINEX 文件路径</param>
        /// <returns></returns>
        public static string ReadText(string rinexFileName)
        {
            using (TextReader r = new StreamReader(rinexFileName, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                string line = null;
                while ((line = r.ReadLine()) != null)
                {
                    //中文字符支持
                    int nonAscCount = StringUtil.GetNonAscCount(line.Substring(0, 60 > line.Length ? line.Length : 60));
                    string headerLabel = line.Substring(60 - nonAscCount).TrimEnd();//header label 61-80
                    if (headerLabel.Contains(RinexHeaderLabel.END_OF_HEADER)) break;

                    sb.AppendLine(line);
                }
               return  sb.ToString();
            } 
        }
         


        public override string ToString()
        {
            List<AttributeItem> list = ObjectUtil.GetAttributes(this, false);
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendLine(item.ToString());                
            }
            return sb.ToString();
        }   
    }

}
