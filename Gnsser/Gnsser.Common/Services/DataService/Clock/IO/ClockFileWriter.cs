//2016.11.06, double create in zhengzhou, 实现Clk文件输出
//2018.11.18, czs, edit in hmx, 增加一些便捷方法

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Times;
using Geo.IO;
using Geo;

namespace Gnsser.Data.Rinex
{ 
    /// <summary>
    /// IGS 钟差文件输出
    /// </summary>
    public class ClockFileWriter : IDisposable
    {
        ILog log = Log.GetLog(typeof(ClockFileReader));
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="ClockFileHeader">头部信息</param>
        public ClockFileWriter(string filePath, ClockFileHeader ClockFileHeader)
        {
            this.FilePath = filePath;
            this.ClockFileHeader = ClockFileHeader;
            if (ClockFileHeader == null)
            {
                this.ClockFileHeader = new ClockFileHeader
                {
                    //StartTime = DateTime.Now,
                    //VersionId = 3.02.ToString(),
                    //AgencyName = "Gnsser Group"
                };
            }
            this.StringBuilder = new StringBuilder();
            this.StringBuilder.Append(BuildHeaderString(this.ClockFileHeader));
        }
        /// <summary>
        /// clk文件输出
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ClockFile"></param>
        public ClockFileWriter(string filePath, ClockFile ClockFile = null)
        {
            this.FilePath = filePath;
            this.StringBuilder = new StringBuilder();
            if (ClockFile != null)
            {
                this.ClockFileHeader = ClockFile.Header;
                this.StringBuilder.Append(BuidSp3V3String(ClockFile));
            }
            else
            {
                this.ClockFileHeader = new ClockFileHeader
                {
                    //StartTime = DateTime.Now,
                    //VersionId = 3.02.ToString(),
                    //AgencyName = "Gnsser Group"
                };
            }
        }


        /// <summary>
        /// 头部信息
        /// </summary>
        public ClockFileHeader ClockFileHeader { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 数据流
        /// </summary>
        protected StringBuilder StringBuilder { get; set; }


       public void Write(ClockFile flie)
        {
            this.StringBuilder = new StringBuilder();
            if (flie != null)
            {
                this.ClockFileHeader = flie.Header;
                this.StringBuilder.Append(BuidSp3V3String(flie));
                this.Flush();
            }
        }

        public void Write(AtomicClock record)
        {
            StringBuilder.Append(BuildClkRecord(record));
        }

        public void Flush()
        {
            if (StringBuilder.Length > 0)
            {
                File.WriteAllText(FilePath, StringBuilder.ToString());
                StringBuilder.Clear();
            }
        }

        /// <summary>
        /// 保存到文件，并清空缓存。此处采用追加的方式保存，可以多次调用此方法。
        /// </summary>
        public void SaveToFile()
        {
            Flush();
        }

        /// <summary>
        /// 将指定的clk转换成 RINEX V3.0字符串。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string BuidSp3V3String(ClockFile file)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BuildHeaderString(file.Header));
            foreach (var item in file.Data.Values)
            {
                foreach (var clock in item)
                {
                    sb.Append(BuildClkRecord(clock));
                }
            }
            return sb.ToString();
        }
        #region clk文件的写
        #region 头文件
        /// <summary>
        /// 构建头部字符串。
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string BuildHeaderString(ClockFileHeader header, double version = 3.00)
        {
            StringBuilder sb = new StringBuilder();

            // HeaderLabel.RINEX_VERSION_TYPE:
            sb.Append(StringUtil.FillSpace(StringUtil.FillSpaceLeft(version.ToString("0.00"), 9), 20));
            sb.Append(StringUtil.FillSpaceRight("C", 40));
            sb.AppendLine(ClockHeaderLabel.RINEX_VERSION_TYPE);
            
            // HeaderLabel.PGM_RUN_BY_DATE:
            sb.Append(StringUtil.FillSpace("Gnsser", 20));
            sb.Append(StringUtil.FillSpace("GeoSolution", 20));
            sb.Append(StringUtil.FillSpace(DateTime.UtcNow.ToString("yyyyMMdd HHmmss") + " UTC", 20));
            sb.AppendLine(ClockHeaderLabel.PGM_RUN_BY_DATE);

            // HeaderLabel.COMMENT:   
            header.RemoveGnsserComment();
            sb.AppendLine(RinexObsFileWriter.BuildGnsserCommentLines());
            if (header == null)
            {
                sb.AppendLine(StringUtil.FillSpace("", 60) + ClockHeaderLabel.END_OF_HEADER);

                return sb.ToString();
            }
                
            if (header.Comments != null)
            {
                foreach (var item in header.Comments)
                {
                    if (item.Contains("Gnsser")) { continue; } //不重复写Gnsser信息。
                    if (String.IsNullOrWhiteSpace(item)) continue;
                    sb.AppendLine(StringUtil.FillSpace(item, 60) + ClockHeaderLabel.COMMENT);
                }
            }
            if (header.TIME_SYSTEM_ID!=null)
                sb.AppendLine(StringUtil.FillSpace(header.TIME_SYSTEM_ID, 60) + ClockHeaderLabel.TIME_SYSTEM_ID);
            if (header.SYS_PCVS_APPLIED != null)
                sb.AppendLine(StringUtil.FillSpace(header.TIME_SYSTEM_ID, 60) + ClockHeaderLabel.SYS_PCVS_APPLIED);
            if(header .LEAP_SECONDS!=0)
                sb.AppendLine(StringUtil.FillSpace(header.LEAP_SECONDS.ToString(), 6) + StringUtil.FillSpace ("",54)+ ClockHeaderLabel.LEAP_SECONDS);
            if (header.COUNT_OF_TYPES_OF_DATA != 0)
            {
                StringBuilder sb0 = GetTypesOfData(header);
                sb.AppendLine(StringUtil.FillSpace(sb0.ToString(), 60) + ClockHeaderLabel.TYPES_OF_DATA);
            }
            if (header.ANALYSIS_CENTER != null)
                sb.AppendLine(StringUtil.FillSpace(header.ANALYSIS_CENTER, 60) + ClockHeaderLabel.ANALYSIS_CENTER);
            if (header.OF_CLK_REF != null)
                sb.AppendLine(StringUtil.FillSpace(header.OF_CLK_REF, 60) + ClockHeaderLabel.OF_CLK_REF);
            if (header.ANALYSIS_CLK_REF != null)
                sb.AppendLine(StringUtil.FillSpace(header.ANALYSIS_CLK_REF, 60) + ClockHeaderLabel.ANALYSIS_CLK_REF);
            if (header.OF_SOLN_STA_TRF != null)
                sb.AppendLine(StringUtil.FillSpace(header.OF_SOLN_STA_TRF, 60) + ClockHeaderLabel.OF_SOLN_STA_TRF);
            if (header.ClockSolnStations != null)
            {
                if (header.ClockSolnStations.Count != 0)
                {
                    foreach (var item in header.ClockSolnStations)
                    {
                        sb.Append(StringUtil.FillSpace(item.Name, 4) + StringUtil.FillSpace(item.Number, 20));
                        sb.Append(StringUtil.FillSpace(item.XYZ.X.ToString(), 11) + StringUtil.FillSpace(item.XYZ.Y.ToString(), 11) + StringUtil.FillSpace(item.XYZ.Z.ToString(), 11));
                        sb.AppendLine(ClockHeaderLabel.SOLN_STA_NAME_NUM);
                    }
                }
            }
            if (header.OF_SOLN_SATS != 0)
                sb.AppendLine(StringUtil.FillSpace(StringUtil.FillSpaceLeft("",4)+header.OF_SOLN_SATS.ToString(), 60) + ClockHeaderLabel.OF_SOLN_SATS);

            if (header.PrnList != null)
            {
                for (int i = 0; i < header.PrnList.Count; i++)
                {
                    sb.Append(header.PrnList[i].ToString() + " ");
                    if ((i + 1) % 15 == 0)
                        sb.AppendLine(ClockHeaderLabel.PRN_LIST);
                }
                int nn = header.PrnList.Count % 15;
                if (nn != 0)
                    sb.AppendLine(StringUtil.FillSpace("", 60 - 4 * nn) + ClockHeaderLabel.PRN_LIST);
            }
            
                        
            sb.AppendLine(StringUtil.FillSpace("", 60) + ClockHeaderLabel.END_OF_HEADER);
                        
            return sb.ToString();
        }

        private static StringBuilder GetTypesOfData(ClockFileHeader header)
        {
            StringBuilder sb0 = new StringBuilder();
            sb0.Append(StringUtil.FillSpace(header.COUNT_OF_TYPES_OF_DATA.ToString(), 6));
            for (int i = 0; i < header.COUNT_OF_TYPES_OF_DATA; i++)
                sb0.Append(StringUtil.FillSpace(header.TYPES_OF_DATA[i], 6));
            return sb0;
        }
        #endregion
        /// <summary>
        /// 构建历元数据
        /// </summary>
        /// <param name="AtomicClock"></param>
        /// <returns></returns>
        private static string BuildClkRecord(AtomicClock AtomicClock)
        {
            StringBuilder sb = new StringBuilder();
            var OneBlankSpace = " ";
            string clockType = null;
            if (AtomicClock.ClockType == ClockType.Satellite) clockType = "AS";
            else clockType = "AR";
            sb.Append(clockType + OneBlankSpace);
            sb.Append(AtomicClock.Name + OneBlankSpace + OneBlankSpace);
            var epoch = AtomicClock.Time;
            var firstLine = epoch.Year  //四位数的年
                 + OneBlankSpace + epoch.Month.ToString("00")
                + OneBlankSpace + epoch.Day.ToString("00")
                + OneBlankSpace + epoch.Hour.ToString("00")
                + OneBlankSpace + epoch.Minute.ToString("00")
                + OneBlankSpace + epoch.Second.ToString("00.000000")//F11.7
                ;
            sb.Append(firstLine);
            sb.Append(StringUtil.FillSpaceLeft(AtomicClock.StateCode, 3));
            sb.Append(OneBlankSpace);
            sb.Append(OneBlankSpace);
            sb.Append(DoubleUtil.ScientificFomate(AtomicClock.ClockBias, "E20.13", false));
            //sb.Append(OneBlankSpace); 
            sb.Append(DoubleUtil.ScientificFomate(AtomicClock.ClockDrift, "E20.13", false));
            sb.AppendLine();
            return sb.ToString();
        }
        #endregion
        public void Dispose()
        {
            SaveToFile();
        }

        /// <summary>
        /// 便捷写入方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public static void WriteFile(string path, ClockFile file)
        {
            using(ClockFileWriter writer = new ClockFileWriter(path))
            {
                writer.Write(file);
            }
        }
    }
}
