using System;
using Gnsser.Times;
using System.Collections.Generic;
using Geo.Times; 
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.IO;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// Rinex 观测文件读取器。
    /// </summary>
    public class ParamNavFileReader
    {
        Log log = new Log(typeof(ParamNavFileReader));
        /// <summary>
        /// 初始化一个读取器。
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public ParamNavFileReader(string fileName)
        {
            this.RinexFileName = fileName;
            if (RinexFileName == null) throw new ArgumentException("路径不可为空！", "RinexFileName");
        }

        /// <summary>
        /// 当前文件路径。
        /// </summary>
        public string RinexFileName { get; set; }

        /// <summary>
        /// 读取RINEX文件的头文件。
        /// </summary>
        /// <param name="rinexFileName">文件路径</param>
        /// <returns></returns>
        public static NavFileHeader ReadHeader(string RinexFileName)
        {
            NavFileHeader header = new NavFileHeader();
            header.Name = Path.GetFileName(RinexFileName);
            using (StreamReader r = new StreamReader(RinexFileName, Encoding.UTF8))
            {
                string line = null;
                string headerLabel = null;
                while ((line = r.ReadLine()) != null && headerLabel != RinexHeaderLabel.END_OF_HEADER)
                {
                    //中文字符支持
                    int nonAscCount = StringUtil.GetNonAscCount(line.Substring(0, 60 > line.Length ? line.Length : 60));
                    headerLabel = line.Substring(60 - nonAscCount).TrimEnd();//header label 61-80
                    if (headerLabel.Contains(RinexHeaderLabel.END_OF_HEADER)) break;
                    switch (headerLabel)
                    {
                        /**
                         *  +--------------------+------------------------------------------+------------+
                            | HEADER LABEL | DESCRIPTION | FORMAT |
                            | (Columns 61-80) | | |
                            +--------------------+------------------------------------------+------------+
                            |RINEX VERSION / TYPE| - Format version : 3.00 | F9.2,11X, |
                            | | - File type: O for Observation Data | A1,19X, |
                            | | - Satellite System: G: GPS | A1,19X |
                            | | R: GLONASS | |
                            | | E: Galileo | |
                            | | S: SBAS payload | |
                            | | M: Mixed | |
                            +--------------------+------------------------------------------+------------+
                         * 
                         */
                        case RinexHeaderLabel.RINEX_VERSION_TYPE:
                            header.Version = double.Parse(line.Substring(0, 9));
                            header.FileType = (RinexFileType)Enum.Parse(typeof(RinexFileType), line.Substring(20, 1));
                            if (line.Substring(40, 1) == " ") header.SatelliteType = RinexUtil.GetSatelliteType(header.FileType);
                            else header.SatelliteType = (SatelliteType)Enum.Parse(typeof(SatelliteType), line.Substring(40, 1));
                            break;
                        case RinexHeaderLabel.PGM_RUN_BY_DATE:
                            header.CreationProgram = line.Substring(0, 20).TrimEnd();
                            header.CreationAgence = line.Substring(20, 20).Trim();
                            header.CreationDate = line.Substring(40, 20).TrimEnd();
                            break;
                        case RinexHeaderLabel.COMMENT:
                            if (header.Comments == null) header.Comments = new List<string>();
                            header.Comments.Add(line.Substring(0, 60 - nonAscCount).Trim());
                            break;
                        case RinexHeaderLabel.RCV_CLOCK_OFFS_APPL:

                            break;
                        case RinexHeaderLabel.LEAP_SECONDS:
                            header.LeapSeconds = int.Parse(line.Substring(0, 6));
                            break;
                        case RinexHeaderLabel.NUM_OF_SATELLITES:
                            break;
                        case RinexHeaderLabel.PRN_NUM_OF_OBS:
                            break;
                        //2X,4D12.4
                        case RinexHeaderLabel.ION_ALPHA:
                            line = line.Replace('D', 'E');
                            header.IonParam.AlfaA0 = double.Parse(line.Substring(2, 12));
                            header.IonParam.AlfaA1 = double.Parse(line.Substring(14, 12));
                            header.IonParam.AlfaA2 = double.Parse(line.Substring(26, 12));
                            header.IonParam.AlfaA3 = double.Parse(line.Substring(38, 12));
                            break;
                        case RinexHeaderLabel.ION_BETA:
                            line = line.Replace('D', 'E');
                            header.IonParam.BetaB0 = double.Parse(line.Substring(2, 12));
                            header.IonParam.BetaB1 = double.Parse(line.Substring(14, 12));
                            header.IonParam.BetaB2 = double.Parse(line.Substring(26, 12));
                            header.IonParam.BetaB3 = double.Parse(line.Substring(38, 12));
                            break;
                        //3X,2D19.12,2I9
                        case RinexHeaderLabel.DELTA_UTC_A0_A1_T_W:
                            line = line.Replace('D', 'E');
                            header.UtcDeltaA0 = double.Parse(line.Substring(3, 19));
                            header.UtcDeltaA1 = double.Parse(line.Substring(22, 19));
                            header.UtcDeltaT = double.Parse(line.Substring(41, 9));
                            header.UtcDeltaW = double.Parse(line.Substring(50, 9));
                            break;
                        case RinexHeaderLabel.PRN_LIST:
                            if (header.PrnList == null) header.PrnList = new List<SatelliteNumber>();
                            string[] prnStrs = line.Substring(0, 60).Trim().Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string prn in prnStrs) header.PrnList.Add(SatelliteNumber.Parse(prn));

                            break;
                        //rinex 3.0 nav
                        case RinexHeaderLabel.TIME_SYSTEM_CORR:

                            break;
                        //rinex 3.0 nav
                        case RinexHeaderLabel.IONOSPHERIC_CORR:

                            break;

                        case RinexHeaderLabel.END_OF_HEADER:
                        default: break;
                    }
                }
            }
            return header;
        }

        #region 导航文件的的读取
        /// <summary>
        /// GNSS导航文件的读取
        /// </summary> 
        /// <returns></returns>
        public ParamNavFile ReadGnssNavFlie()
        {
            ParamNavFile f = new ParamNavFile();
            f.Header = ReadHeader(RinexFileName);
            //测试版本
            if (f.Header.Version == 0)
            {
                using (StreamReader sr = new StreamReader(RinexFileName, UnicodeEncoding.UTF8))
                {
                    RinexUtil.SkipHeader(sr);
                }
            }

            using (StreamReader sr = new StreamReader(RinexFileName, UnicodeEncoding.UTF8))
            {
                RinexUtil.SkipHeader(sr);

                if (f.Header.Version < 3.0)
                {

                    while (sr.Peek() != -1)
                    {
                        EphemerisParam record = ReadRecordV2(sr, f.Header);
                        f.Add(record);
                    }
                }
                else if (f.Header.Version >= 3.0 && f.Header.Version < 4.0)
                {
                    while (sr.Peek() != -1)
                    {
                        SatClockBias recordHeader = null;
                        string line = null;
                        try
                        {
                            line = sr.ReadLine();
                            recordHeader = ParseFirstLineV3(line, f.Header);
                            var record = new EphemerisParam(recordHeader);
                            ReadRecordBodyV3(sr, record);
                            f.Add(record);
                        }
                        catch(Exception ex)
                        {
                            log.Error("导航星历解析错误！将继续尝试 " + ex.Message + line + ", Path:" + RinexFileName);
                        }
                    }
                }
            }
            return f;
        }

        #region RINEX 3.0s
        /// <summary>
        /// 读取 RINEX 3.0s GNSS 导航文件。
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="header"></param>
        /// <returns></returns> 
        public static EphemerisParam ReadRecordBodyV3(StreamReader sr,   EphemerisParam record)
        {
            string line = null;
            if (record.Prn.SatelliteType == SatelliteType.U) { return record; }
            //采用坐标
            while (record.Prn.SatelliteType == SatelliteType.S || record.Prn.SatelliteType == SatelliteType.R)
            {
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                line = sr.ReadLine();
                //ParseFirstLineV3(line, header, record);
                // GlonassNaviFileReader.ReadRecordBodyV3(record )  
            }

            //轨道参数
            if (record.Prn.SatelliteType == SatelliteType.E
                || record.Prn.SatelliteType == SatelliteType.C
                || record.Prn.SatelliteType == SatelliteType.G
                || record.Prn.SatelliteType == SatelliteType.J
                || record.Prn.SatelliteType == SatelliteType.I)
            {

                line = sr.ReadLine();
                line = line.Replace('D', 'E');
                record.IODE = Geo.Utils.StringUtil.ParseDouble(line, 4, 19);
                record.Crs = Geo.Utils.StringUtil.ParseDouble(line, 23, 19);
                record.DeltaN = Geo.Utils.StringUtil.ParseDouble(line, 42, 19);
                record.MeanAnomaly = Geo.Utils.StringUtil.ParseDouble(line, 61, 19);

                line = sr.ReadLine();
                line = line.Replace('D', 'E');
                record.Cuc = Geo.Utils.StringUtil.ParseDouble(line, 4, 19);
                record.Eccentricity = Geo.Utils.StringUtil.ParseDouble(line, 23, 19);
                record.Cus = Geo.Utils.StringUtil.ParseDouble(line, 42, 19);
                record.SqrtA = Geo.Utils.StringUtil.ParseDouble(line, 61, 19);

                line = sr.ReadLine();
                line = line.Replace('D', 'E');
                record.Toe = Geo.Utils.StringUtil.ParseDouble(line, 4, 19);
                record.Cic = Geo.Utils.StringUtil.ParseDouble(line, 23, 19);
                record.LongOfAscension = Geo.Utils.StringUtil.ParseDouble(line, 42, 19);
                record.Cis = Geo.Utils.StringUtil.ParseDouble(line, 61, 19);

                line = sr.ReadLine();
                line = line.Replace('D', 'E');
                record.Inclination = Geo.Utils.StringUtil.ParseDouble(line, 4, 19);
                record.Crc = Geo.Utils.StringUtil.ParseDouble(line, 23, 19);
                record.ArgumentOfPerigee = Geo.Utils.StringUtil.ParseDouble(line, 42, 19);
                record.OmegaDot = Geo.Utils.StringUtil.ParseDouble(line, 61, 19);

                line = sr.ReadLine();
                line = line.Replace('D', 'E');
                record.EyeDot = Geo.Utils.StringUtil.ParseDouble(line, 4, 19);
                record.CodesL2 = Geo.Utils.StringUtil.ParseDouble(line, 23, 19);
                record.GPSWeeks = (int)Geo.Utils.StringUtil.ParseDouble(line, 42, 19);
                record.L2PDataFlag = Geo.Utils.StringUtil.ParseDouble(line, 61, 19) == 1;

                line = sr.ReadLine();
                line = line.Replace('D', 'E');
                record.SVAccuracy = Geo.Utils.StringUtil.ParseDouble(line, 4, 19);
                record.SVHealth = Geo.Utils.StringUtil.ParseDouble(line, 23, 19);
                record.TGD = Geo.Utils.StringUtil.ParseDouble(line, 42, 19);
                record.IODC = Geo.Utils.StringUtil.ParseDouble(line, 61, 19);

                //if (header.Version == 1.0) return record;

                line = sr.ReadLine();
                line = line.Replace('D', 'E');
                record.TTM = Geo.Utils.StringUtil.ParseDouble(line, 4, 19);
            }
            return record;
        }
        /// <summary>
        /// 分解第一行。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="header"></param>
        /// <param name="record"></param>
        public static SatClockBias ParseFirstLineV3(string line, NavFileHeader header)
        {
            SatClockBias record = new SatClockBias();
            line = line.Replace('D', 'E');
            record.Prn = SatelliteNumber.Parse(line.Substring(0, 3), header.SatelliteType);
            record.Time = Time.Parse(line.Substring(4, 19));
            string val = line.Substring(23, 19);
            record.ClockBias = double.Parse(val);
            record.ClockDrift = double.Parse(line.Substring(42, 19));
            record.DriftRate = double.Parse(line.Substring(61, 19));
            return record;
        }
        #endregion

        #region RINEX 2.0
        /// <summary>
        /// 读取 RINEX 2.0s GPS导航文件。
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static EphemerisParam ReadRecordV2(StreamReader sr, NavFileHeader header)
        {
            EphemerisParam record = new EphemerisParam();

            string line = sr.ReadLine();
            ParseFirstLineV2(line, header, record);

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            record.IODE = double.Parse(line.Substring(3, 19));
            record.Crs = double.Parse(line.Substring(22, 19));
            record.DeltaN = double.Parse(line.Substring(41, 19));
            record.MeanAnomaly = double.Parse(line.Substring(60, 19));

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            record.Cuc = double.Parse(line.Substring(3, 19));
            record.Eccentricity = double.Parse(line.Substring(22, 19));
            record.Cus = double.Parse(line.Substring(41, 19));
            record.SqrtA = double.Parse(line.Substring(60, 19));

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            record.Toe = double.Parse(line.Substring(3, 19));
            record.Cic = double.Parse(line.Substring(22, 19));
            record.LongOfAscension = double.Parse(line.Substring(41, 19));
            record.Cis = double.Parse(line.Substring(60, 19));

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            record.Inclination = double.Parse(line.Substring(3, 19));
            record.Crc = double.Parse(line.Substring(22, 19));
            record.ArgumentOfPerigee = double.Parse(line.Substring(41, 19));
            record.OmegaDot = double.Parse(line.Substring(60, 19));

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            record.EyeDot = double.Parse(line.Substring(3, 19));
            record.CodesL2 = double.Parse(line.Substring(22, 19));
            record.GPSWeeks = (int)double.Parse(line.Substring(41, 19));
            record.L2PDataFlag = double.Parse(line.Substring(60, 19)) == 1;

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            record.SVAccuracy = double.Parse(line.Substring(3, 19));
            record.SVHealth = double.Parse(line.Substring(22, 19));
            record.TGD = double.Parse(line.Substring(41, 19));
            record.IODC = double.Parse(line.Substring(60, 19));

            if (header.Version == 1.0) return record;

            line = sr.ReadLine();
            line = line.Replace('D', 'E');
            record.TTM = double.Parse(line.Substring(3, 19));

            return record;
        }
        /// <summary>
        /// 分解第一行。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="header"></param>
        /// <param name="record"></param>
        public static void ParseFirstLineV2(string line, NavFileHeader header, SatClockBias record)
        {
            line = line.Replace('D', 'E');
            record.Prn = SatelliteNumber.Parse(line.Substring(0, 2), header.SatelliteType);
            record.Time = Time.Parse(line.Substring(2, 20));
            string val = line.Substring(22, 19);
            record.ClockBias = double.Parse(val);
            record.ClockDrift = double.Parse(line.Substring(41, 19));
            record.DriftRate = double.Parse(line.Substring(60, 19));
        }


        #endregion
        #endregion

        #region 工具
        /// <summary>
        /// 读取并返回 RINEX 内容行。如果有注释行，则读取注释，继续读取，返回新的内容行。
        /// </summary>
        /// <param name="r">StreamReader</param>
        /// <param name="comments">注释</param>
        /// <returns></returns>
        private static string ReadContentLine(StreamReader r, List<String> comments)
        {
            string line = r.ReadLine();
            if (line == null) return line;
            //判断是否是注释行

            while (IsCommentLine(line))
            {
                comments.Add(GetCommenValue(line));
                line = r.ReadLine();
            }
            return line;
        }
        /// <summary>
        /// 判断本行是否是注释行。即，在60列时，具有COMMENT标识。
        /// </summary>
        /// <param name="line">输入行</param>
        /// <returns></returns>
        private static bool IsCommentLine(string line)
        {
            if (line.Length >= 67)
            {
                if (line.Substring(60, 7) == RinexHeaderLabel.COMMENT)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取注释行的内容。
        /// </summary>
        /// <param name="line">输入行</param>
        /// <returns></returns>
        private static string GetCommenValue(string line)
        {
            return StringUtil.SubString(line, 0, 60).Trim();
        }


        private static string SubString(string line, int nonAscCount, int fromIndex, int len)
        {
            return line.Substring(fromIndex, len - nonAscCount).Trim();
        }
        #endregion
    }
}
