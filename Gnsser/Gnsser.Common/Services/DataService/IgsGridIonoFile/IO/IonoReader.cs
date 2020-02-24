//2017.08.16, czs, create in hongqing, 电离层文件的读取

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Times;
using Geo;
using Geo.IO;
using Geo.Utils;
using Geo.Coordinates;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{
    /// <summary>
    /// 电离层 记录部分，不适用于遍历，更适用于一次性读取。
    /// 遍历的话，先全读取数据部分，遍历的只是最后RMS部分。
    /// </summary>
    public class IonoReader : Geo.IO.AbstractTimedStreamReader<IonoSection>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        public IonoReader(string filePath)
            : base(filePath)
        {
            Header = ReadHeader(InputPath);
            IonoFile = new IonoFile();
        } 
        /// <summary>
        /// 头部信息。
        /// </summary>
        public IonoHeader Header { get; set; }  

        /// <summary>
        ///  读取RINEX文件的头文件。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public IonoHeader ReadHeader(string path)
        {
            if (!Geo.Utils.FileUtil.IsValid(path)) { return null; }
            IonoHeader header = new IonoHeader();

            header.FileName = Path.GetFileName(path);
            using (StreamReader r = new StreamReader(path, true))
            {
                int lineIndex = 0;
                string line = null;
                while ((line = ReadNextNoNullLine(r)) != null)
                {
                    lineIndex++;
                    //中文字符支持
                    int nonAscCount = StringUtil.GetNonAscCount(line.Substring(0, 60 > line.Length ? line.Length : 60));
                    string headerLabel = line.Substring(60 - nonAscCount).TrimEnd();//header label 61-80
                    if (headerLabel.Contains(RinexHeaderLabel.END_OF_HEADER)) break;
                    string content = line.Substring(0, 60);
                    if (String.IsNullOrWhiteSpace(content)) { continue; }//没有内容

                    switch (headerLabel)
                    {
                        case RinexHeaderLabel.IONEX_VERSION_TYPE:
                            header.Version = double.Parse(line.Substring(0, 8));
                            header.FileType = line.Substring(20, 1);
                            header.SatSysOrTheoModel = line.Substring(40, 3);
                            break;
                        case RinexHeaderLabel.PGM_RUN_BY_DATE:
                            header.FileInfo.CreationProgram = line.Substring(0, 20).TrimEnd();
                            header.FileInfo.CreationAgence = line.Substring(20, 20).Trim();
                            header.FileInfo.CreationDate = line.Substring(40, 20).TrimEnd();
                            break;
                        case RinexHeaderLabel.COMMENT:
                            if (header.Comments == null) header.Comments = new List<string>();
                            header.Comments.Add(line.Substring(0, 60 - nonAscCount).Trim());
                            break;
                        case RinexHeaderLabel.DESCRIPTION:
                            header.Description = (line.Substring(0, 60 - nonAscCount).Trim());
                            break;
                        case RinexHeaderLabel.EPOCH_OF_FIRST_MAP:
                            header.EpochOfFirstMap = Time.Parse(line.Substring(0, 60 - nonAscCount).Trim());
                            break;
                        case RinexHeaderLabel.EPOCH_OF_LAST_MAP:
                            header.EpochOfLastMap = Time.Parse(line.Substring(0, 60 - nonAscCount).Trim());
                            break;
                        case RinexHeaderLabel.INTERVAL:
                            header.Interval = double.Parse(line.Substring(0, 10));
                            break;
                        case RinexHeaderLabel.OF_MAPS_IN_FILE:
                            var str = line.Substring(0, 60);
                            if (String.IsNullOrWhiteSpace(str)) break;
                            header.NumOfTotalMaps = IntUtil.TryParse(str);
                            break;
                        case RinexHeaderLabel.MAPPING_FUNCTION:
                            header.Description = (line.Substring(0, 60 - nonAscCount).Trim());
                            break;
                        case RinexHeaderLabel.ELEVATION_CUTOFF:
                            header.ElevatonCutOff = StringUtil.ParseDouble(line.Substring(0, 10));
                            break;
                        case RinexHeaderLabel.OBSERVABLES_USED:
                            header.ObservablesUsed = (line.Substring(0, 60 - nonAscCount).Trim());
                            break;

                        case RinexHeaderLabel.OF_STATIONS:
                            var str2 = line.Substring(0, 6);
                            if (String.IsNullOrWhiteSpace(str2)) break;
                            header.NumOfStations = IntUtil.TryParse(str2);
                            break;
                        case RinexHeaderLabel.OF_SATELLITES:
                            var str3 = line.Substring(0, 6);
                            if (String.IsNullOrWhiteSpace(str3)) break;
                            header.NumOfSatellites = IntUtil.TryParse(str3);
                            break;
                        case RinexHeaderLabel.BASE_RADIUS:
                            header.MeanEarthRadius = double.Parse(line.Substring(0, 10));
                            break;
                        case RinexHeaderLabel.MAP_DIMENSION:
                            var str4 = line.Substring(0, 6);
                            if (String.IsNullOrWhiteSpace(str4)) break;
                            header.MapDimension = IntUtil.TryParse(str4);
                            break;
                        case RinexHeaderLabel.HGT1_HGT2_DHGT:
                            header.HeightRange = ParseIncreaseValue(line);
                            break;
                        case RinexHeaderLabel.LON1_LON2_DLON:
                            header.HeightRange = ParseIncreaseValue(line);
                            break;
                        case RinexHeaderLabel.LAT1_LAT2_DLAT:
                            header.HeightRange = ParseIncreaseValue(line);
                            break;
                        case RinexHeaderLabel.EXPONENT:
                            header.Exponent = int.Parse(line.Substring(0, 6));
                            break;
                        case RinexHeaderLabel.START_OF_AUX_DATA:
                            header.StartOfAuxData = line.Substring(0, 60).Trim();
                            break;
                        case RinexHeaderLabel.END_OF_AUX_DATA:
                            header.EndOfAuxData = line.Substring(0, 60).Trim();
                            break;
                        case RinexHeaderLabel.STATION_BIAS_RMS:
                            var satFlag = line.Substring(3, 1);//ＧＮＳＳ系统标识 如 G
                            var siteName = satFlag + "_" + line.Substring(6, 4).Trim().ToLower();//IGS code
                            var val = new RmsedNumeral()
                            {                                
                                Value = Double.Parse(line.Substring(26, 10)),
                                Rms = Double.Parse(line.Substring(36, 10)),
                            };
                            header.StationsWithBiasRms[siteName] = val;
                            break;
                        case RinexHeaderLabel.PRN_BIAS_RMS:
                            var Name = SatelliteNumber.Parse(line.Substring(0, 6).Trim());
                            var val2 = new RmsedNumeral()
                        {
                            Value = Double.Parse(line.Substring(6, 10)),
                            Rms = Double.Parse(line.Substring(16, 10)),
                        };
                            header.SatellitesWithBiasRms[Name] = val2;
                            break;

                        case RinexHeaderLabel.END_OF_HEADER:
                            return header;
                        default: break;
                    }
                    header.LineNumber = lineIndex + 1;
                }
            }

            return header;
        }

        public IonoFile IonoFile { get; set; }
        /// <summary>
        /// 读取文件
        /// </summary> 
        /// <returns></returns>
        public IonoFile ReadAll(bool isSkipContent = false)
        {
            this.Reset();

            IonoFile file = new IonoFile();
            file.Name = Path.GetFileName(InputPath);
            //   this.ReadHeader(InputPath);
            file.Header = Header;

            startTime = Header.EpochOfFirstMap;// Time.MaxValue;
            endTime = Header.EpochOfLastMap;// Time.MinValue;

            if (!isSkipContent)
            {
                while (this.MoveNext())
                {
                    var section = this.Current;

                    if (section != null && section.Count != 0)
                    {
                        file.Add(section);

                        if (section.Time < startTime) startTime = section.Time;
                        if (section.Time > endTime) endTime = section.Time;
                    }
                }
                log.Info("完全加载了电离层文件到内存 " + Name);
            }
            file.TimePeriod = new BufferedTimePeriod(startTime, endTime);

            return file;
        }
        Time startTime = Time.MaxValue;
        /// <summary>
        /// 从文件开始获取时间
        /// </summary>
        /// <returns></returns>
        public override Time TryGetStartTime()
        {
            if (startTime != Time.MaxValue)
            { return startTime; }

            this.Reset();
            while (this.MoveNext())
            {
                startTime = Current.Time;
                return startTime;
            }
            //StreamReader.BaseStream.Position = pos;
            return startTime;
        }

        Time endTime = Time.MinValue;
        /// <summary>
        /// 从文件末尾获取时间
        /// </summary>
        /// <returns></returns>
        public override Time TryGetEndTime()
        {
            if (endTime != Time.MinValue)    {  return endTime;   }

            try
            {
                endTime = this.Header.EpochOfLastMap;
                return endTime;
            }
            catch (Exception ex)
            {
                log.Error("获取结束时间失败！将尝试其他方法。" + ex.Message);
            } 
            this.Reset();

            //30颗卫星 5000有2个多历元
            //var differ = StreamReader.BaseStream.Length > 5000 ? 5000 : StreamReader.BaseStream.Length;
            //StreamReader.BaseStream.Seek(-differ, SeekOrigin.End);

            string line = null;
            while ((line = StreamReader.ReadLine()) != null)
            {
                if (IsFirstLineOfEpochMapSectionWithTime(line))
                {
                    endTime = ParseTime(line);
                }
            }

            return endTime;
        }

        private Time ParseTime(string line)
        {
            if (line.Length < 80) { return Time.Default; }

            var lable = line.Substring(0, 60);
            return Time.Parse(lable);
        }
        /// <summary>
        /// 是否是数据的开始
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsStartLineOfEpochMapSection(string line)
        {
            if (line.Length < 76) { return false; }
            var lable = line.Substring(60).Trim();
            return lable == RinexHeaderLabel.START_OF_TEC_MAP;
        }
        private bool IsStartLineOfRmsEpochMapSection(string line)
        {
            if (line.Length < 76) { return false; }
            var lable = line.Substring(60).Trim();
            return lable == RinexHeaderLabel.START_OF_RMS_MAP;
        }

        /// <summary>
        /// 第一行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsFirstLineOfEpochMapSectionWithTime(string line)
        {
            if (line ==null || line.Length < 80) { return false; }
            var lable = line.Substring(60, 20);
            return lable == RinexHeaderLabel.EPOCH_OF_CURRENT_MAP;
        }
        private bool IsEndLineOfEpochMapSection(string line)
        {
            if (line.Length < 74) { return false; }
            var lable = line.Substring(60).Trim();
            return lable == RinexHeaderLabel.END_OF_TEC_MAP;
        }
        /// <summary>
        /// 移动到下一个，尝试解析，如果到末尾了，则返回false
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            if (StreamReader.EndOfStream) { return false; }
            #region 流程控制
            CurrentIndex++;
            if (CurrentIndex == StartIndex) { log.Debug("数据流 " + this.Name + " 开始读取数据。"); }
            if (this.IsCancel) { log.Info("数据流 " + this.Name + " 已被手动取消。"); return false; }
            if (CurrentIndex > this.MaxEnumIndex) { log.Info("数据流 " + this.Name + " 已达指定的最大编号 " + this.MaxEnumIndex); return false; }
            while (CurrentIndex < this.StartIndex) { this.MoveNext(); }
            #endregion
             
            //try
            { 
                //第一句为时间
                var val = ReadSection( IonoFile, Header, StreamReader);
                if (val != null)
                {
                    this.Current = val;
                    return true;
                }

            }
            //catch (Exception ex)
            //{
            //    log.Error("读取星历文件" + Name + "出错：" + ex.Message + line + ", 将继续尝试");
            //    return MoveNext();
            //}
            return false;
        }

        bool IsCurrentValueOrRms;

        /// <summary>
        /// 读取历元对象
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="StreamReader"></param>
        /// <returns></returns>
        private IonoSection ReadSection(IonoFile ionoFile, IonoHeader Header, StreamReader StreamReader)
        { 
            string line = StreamReader.ReadLine();
            var isValueOrRms = (IsStartLineOfEpochMapSection(line));
            if (isValueOrRms)
            {
                IsCurrentValueOrRms = true;
                return  ReadSection(ionoFile, Header, StreamReader);
            }
            isValueOrRms = !(IsStartLineOfRmsEpochMapSection(line));


            while (!IsFirstLineOfEpochMapSectionWithTime(line))
            {
                line = StreamReader.ReadLine();
                if (line == null) { return null; }
                if (line.Length > 60 && line.Substring(60).Trim() == RinexHeaderLabel.END_OF_FILE) { return null; }
            }
            IonoSection IonoSection =  null; 
            var time = this.ParseTime(line); 
            if(ionoFile.Contains(time)){
                IonoSection =ionoFile[time]; 
            }else{
                IonoSection = new IonoSection() { Header = Header, Time=time }; 
            } 
            while ((line = StreamReader.ReadLine()) != null && IsStartOfRecord(line) &&  !IsEndLineOfEpochMapSection(line))
            {
                var lat = double.Parse(line.Substring(2, 6));
                IonoRecord record = null;
                if (IonoSection.Contains(lat))
                {
                    record = IonoSection[lat];
                }
                else
                {
                    record = new IonoRecord()
                    {
                        LonRange = new IncreaseValue
                        {
                            Start = double.Parse(line.Substring(8, 6)),
                            End = double.Parse(line.Substring(14, 6)),
                            Increament = double.Parse(line.Substring(20, 6)),
                        },
                        Height = double.Parse(line.Substring(26, 6)),
                        Lat = lat,
                    };
                }
                double lineCount = Math.Ceiling(record.LonRange.Count / 16.0);
                List<double> vals = new List<double>();
                for (int i = 0; i < lineCount; i++)
                {
                    line = StreamReader.ReadLine();
                    var items = StringUtil.Split(line, 5);
                    foreach (var item in items)
                    {
                        if (String.IsNullOrWhiteSpace(item)) continue;
                        vals.Add(StringUtil.ParseDouble(item));
                    }
                }
                for (int i = 0; i < vals.Count; i++)
                {
                    //TEC values in 0.1 TECU(10^16)
                    double val = vals[i];
                    if (Header.Exponent == 0)
                    {
                        val *= 0.1;
                    }
                    else
                    {
                        val *=  Math.Pow(10, Header.Exponent); 

                    }
                    var alat = record.LonRange.Start + i * record.LonRange.Increament;
                    if (record.Contains(alat))
                    {
                        record[alat].Rms = val;
                    }
                    else
                    {
                        record.Add(alat, new RmsedNumeral(val, Double.NaN));
                    }
                }
                
                IonoSection[lat] = record;
            }

            if (isValueOrRms)
            {
                ionoFile[time] = IonoSection;
                return  ReadSection(ionoFile, Header, StreamReader);
            }

            return IonoSection; 
        }

        private static bool IsStartOfRecord(string line)
        {
            if (line.Length < 80) return false;
            return line.Substring(60, 20) == RinexHeaderLabel.LAT_LON1_LON2_DLON_H;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset()
        {
            StreamReader.BaseStream.Position = 0;
            StreamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            RinexUtil.SkipLines(StreamReader, Header.LineNumber);
        }


        private static IncreaseValue ParseIncreaseValue(string line)
        {
            var inc = new IncreaseValue()
            {
                Start = double.Parse(line.Substring(2, 6)),
                End = double.Parse(line.Substring(8, 6)),
                Increament = double.Parse(line.Substring(14, 6))
            };
            return inc;
        }

        #region 静态工具方法 
        /// <summary>
        /// 读取下一行有内容的行，非空行，空白行。
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string ReadNextNoNullLine(StreamReader reader)
        {
            string line = reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(line))
            {
                return line;
            }

            if (line == null) { return "";  }
            return ReadNextNoNullLine(reader);
        }

        /// <summary>
        /// 读取第一个内容行。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetFirstContentLine(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                RinexUtil.SkipHeader(reader);
                return ReadContentLine(reader);
            }
        }

        /// <summary>
        /// 读取并返回 RINEX 内容行。如果有注释行，则读取注释，继续读取，返回新的内容行。
        /// </summary>
        /// <param name="r">StreamReader</param>
        /// <param name="comments">注释</param>
        /// <returns></returns>
        private static string ReadContentLine(StreamReader r, List<String> comments = null)
        {
            string line = r.ReadLine();
            if (line == null) return line;
            //判断是否是注释行

            while (IsCommentLine(line))
            {
                if (comments != null) comments.Add(GetCommenValue(line));

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

        /// <summary>
        /// 截取包含非ascii码的字符串，如汉字
        /// </summary>
        /// <param name="line"></param>
        /// <param name="nonAscCount"></param>
        /// <param name="fromIndex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static string SubString(string line, int nonAscCount, int fromIndex, int len)
        {
            return StringUtil.SubString(line, fromIndex, len - nonAscCount).Trim();
            // return line.Substring(fromIndex, len - nonAscCount).Trim();
        }
        #endregion
    }     
}
