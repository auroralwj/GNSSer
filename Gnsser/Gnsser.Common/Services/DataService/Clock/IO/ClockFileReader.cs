//2013.01.22, czs , Rinex 钟差文件
//2016.03.06, czs, 流式重构，增强鲁棒性
//2018.05.08, czs, extract in hmx, IGS 简易钟差文件读取

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
    /// IGS 简易钟差文件读取
    /// </summary>
    public class SimpleClockFileReader : ClockFileReader<SimpleClockBias, SimpleClockFile>
    {
        ILog log = Log.GetLog(typeof(SimpleClockFileReader));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="IsSkipSite"></param>
        public SimpleClockFileReader(string filePath, bool IsSkipSite = false) : base(filePath, IsSkipSite)
        {
        }
         ClockType CurrentClockType { get; set; }

        string CurrentName { get; set; }
        /// <summary>
        /// 整体读取，可以精确探知时刻。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override SimpleClockFile ReadFile(string path)
        {
            this.Reset();

            SimpleClockFile file = new SimpleClockFile();
            file.Name = System.IO.Path.GetFileName(path);
            file.Header = Header;
            file.Interval = Interval;

            Time start = Time.MaxValue;
            Time end = Time.MinValue;

            while (this.MoveNext())
            {
                var item = this.Current;
                if (IsSkipSite && CurrentClockType == ClockType.Receiver) { continue; }

                if (item != null)
                {
                    file.GetClockItems(CurrentName).Add(item);

                    if (item.Time < start) start = item.Time;
                    if (item.Time > end) end = item.Time;
                }
            }

            file.TimePeriod = new BufferedTimePeriod(start, end);

            log.Debug("完全加载了钟差文件到内存 " + Name);
            return file;
        }
        #region 解析方法
        /// <summary>
        /// AR AMC2 2012 12 30 00 00  0.000000  2    0.350317686691E-08  0.112840527013E-09  
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected override SimpleClockBias ParseRinexLine(string line)
        {
            //try
            //{
            SimpleClockBias clock = new SimpleClockBias(); 
            string mark = line.Substring(0, 2);
            if (mark == "AR") CurrentClockType = Data.Rinex.ClockType.Receiver;
            if (mark == "AS") CurrentClockType = Data.Rinex.ClockType.Satellite;

            if (IsSkipSite && CurrentClockType == ClockType.Receiver) { return clock; }

            CurrentName = line.Substring(3, 4).Trim();          

            clock.Time = Time.Parse(line.Substring(8, 26));
            clock.ClockBias = StringUtil.ParseDouble(line, 40, 19);

            return clock;
            //}
            //catch (Exception ex)
            //{
            //    log.Error( "解析错误：" + line + ", "+ ex.Message);
            //    return null;
            //}
        }


        #endregion

    } 



    /// <summary>
    /// IGS 钟差文件读取
    /// </summary>
    public class ClockFileReader : ClockFileReader<AtomicClock, ClockFile>
    {
        ILog log = Log.GetLog(typeof(ClockFileReader));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="IsSkipSite"></param>
        public ClockFileReader(string filePath, bool IsSkipSite = false) : base(filePath, IsSkipSite)
        {
        }
        /// <summary>
        /// 快捷访问方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="IsSkipSite">是否忽略测站</param>
        /// <returns></returns>
        public static ClockFile ReadFile(string path, bool IsSkipSite)
        {
            ClockFileReader reader = new ClockFileReader(path, IsSkipSite);
           return  reader.ReadAll();
        }
        /// <summary>
        /// 整体读取，可以精确探知时刻。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override ClockFile ReadFile(string path)
        {
            this.Reset();

            ClockFile file = new ClockFile();
            file.Name = System.IO.Path.GetFileName(path);
            file.Header = Header;
            file.Interval = Interval;

            Time start = Time.MaxValue;
            Time end = Time.MinValue;

            while (this.MoveNext())
            {
                var item = this.Current;
                if (IsSkipSite && item.ClockType == ClockType.Receiver) { continue; }

                if (item != null)
                {
                    file.GetClockItems(item.Name).Add(item);

                    if (item.Time < start) start = item.Time;
                    if (item.Time > end) end = item.Time;
                }
            }

            file.TimePeriod = new BufferedTimePeriod(start, end);

            log.Debug("完全加载了钟差文件到内存 " + Name);
            return file;
        }
        #region 解析方法
        /// <summary>
        /// AR AMC2 2012 12 30 00 00  0.000000  2    0.350317686691E-08  0.112840527013E-09  
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected override AtomicClock ParseRinexLine(string line)
        {
            //try
            //{
            AtomicClock clock = new AtomicClock();
            clock.Source = this.Header.SourceName;
            string mark = line.Substring(0, 2);
            if (mark == "AR") clock.ClockType = Data.Rinex.ClockType.Receiver;
            if (mark == "AS") clock.ClockType = Data.Rinex.ClockType.Satellite;

            if (IsSkipSite && clock.ClockType == ClockType.Receiver) { return clock; }

            clock.Name = line.Substring(3, 4).Trim();
            clock.Prn = TryGetPrn(clock.Name); 

            clock.Time = Time.Parse(line.Substring(8, 26));
            clock.StateCode = int.Parse(line.Substring(36, 1));

            clock.ClockBias = StringUtil.ParseDouble(line, 40, 19);
            if (line.Length >= 60)
            {
                clock.ClockDrift = StringUtil.ParseDouble(line, 60, 19);
                if (Math.Abs(clock.ClockDrift) > 0.1)
                {
                    clock.ClockDrift = 0;
                }
            }
            return clock;
            //}
            //catch (Exception ex)
            //{
            //    log.Error( "解析错误：" + line + ", "+ ex.Message);
            //    return null;
            //}
        }



        public SatelliteNumber TryGetPrn(string CurrentName)
        {
            if (CurrentName.Trim().Length == 3 && Char.IsNumber(CurrentName[1]) && Char.IsNumber(CurrentName[2]))
            {
                return SatelliteNumber.Parse(CurrentName);
            }

            return SatelliteNumber.Default;
        }
        #endregion

    }




    /// <summary>
    /// IGS 钟差文件读取
    /// </summary>
    public abstract class ClockFileReader<TAtomicClock, TClockFile> : Geo.IO.AbstractTimedStreamReader<TAtomicClock>
        where  TAtomicClock : ISimpleClockBias, new()
        where TClockFile : ClockFile<TAtomicClock>, new()
    {
        ILog log = Log.GetLog(typeof(ClockFileReader));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="IsSkipSite"></param>
        public ClockFileReader(string filePath, bool IsSkipSite = false) : base(filePath)
        {
            Header = ReadHeader(filePath);
            this.IsSkipSite = IsSkipSite;
            string threechar = filePath.Substring(filePath.Length - 3).TrimEnd();
            if (threechar == "clk") Interval = 300;
            if (threechar == "30s") Interval = 30;
            if (threechar == "05s") Interval = 5;
        }
        /// <summary>
        /// 读取原子钟文件。
        /// </summary>
        /// <returns></returns>
        public TClockFile ReadAll()
        {
            return ReadFile(InputPath);
        }
        /// <summary>
        /// 头部信息
        /// </summary>
        public ClockFileHeader Header { get; set; }
        /// <summary>
        /// 是否略过测站钟差，这样可以节约内存
        /// </summary>
        public bool IsSkipSite { get; set; }
        /// <summary>
        /// 采样间隔
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 整体读取，可以精确探知时刻。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract TClockFile ReadFile(string path);
        #region 解析方法
        /// <summary>
        /// AR AMC2 2012 12 30 00 00  0.000000  2    0.350317686691E-08  0.112840527013E-09  
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        protected abstract TAtomicClock ParseRinexLine(string line);

        /// <summary>
        /// 读取RINEX文件的头文件。
        /// </summary>
        /// <param name="rinexFileName">文件路径</param>
        /// <returns></returns>
        public static ClockFileHeader ReadHeader(string rinexFileName = null)
        {
            if (rinexFileName == null) throw new ArgumentException("路径不可为空！", "RinexFileName");

            ClockFileHeader header = new ClockFileHeader();

            header.SourceName = Path.GetFileName(rinexFileName);
            header.TYPES_OF_DATA = new List<string>();
            using (StreamReader r = new StreamReader(rinexFileName, Encoding.UTF8))
            {
                string line = null;
                string headerLabel = null;
                while ((line = r.ReadLine()) != null && headerLabel != ClockHeaderLabel.END_OF_HEADER)
                {
                    //中文字符支持
                    int nonAscCount = StringUtil.GetNonAscCount(line.Substring(0, 60 > line.Length ? line.Length : 60));
                    headerLabel = line.Substring(60 - nonAscCount).TrimEnd();//header label 61-80
                    if (headerLabel.Contains(ClockHeaderLabel.END_OF_HEADER)) break;
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
                        case ClockHeaderLabel.RINEX_VERSION_TYPE:
                            header.Version = double.Parse(line.Substring(0, 9));
                            header.FileType = (RinexFileType)Enum.Parse(typeof(RinexFileType), line.Substring(20, 1));
                            if (line.Substring(40, 1) == " ") header.SatelliteSystem = RinexUtil.GetSatelliteType(header.FileType);
                            else header.SatelliteSystem = (SatelliteType)Enum.Parse(typeof(SatelliteType), line.Substring(40, 1));
                            break;
                        case ClockHeaderLabel.PGM_RUN_BY_DATE:
                            header.CreationProgram = line.Substring(0, 20).TrimEnd();
                            header.CreationAgence = line.Substring(20, 20).Trim();
                            header.CreationDate = line.Substring(40, 20).TrimEnd();
                            break;
                        case ClockHeaderLabel.COMMENT:
                            if (header.Comments == null) header.Comments = new List<string>();
                            header.Comments.Add(line.Substring(0, 60 - nonAscCount).Trim());
                            break;

                        //以下是钟差文件头 
                        case ClockHeaderLabel.TIME_SYSTEM_ID:
                            header.TIME_SYSTEM_ID = line.Substring(0, 60).Trim();

                            break;
                        case ClockHeaderLabel.SYS_PCVS_APPLIED:
                            header.SYS_PCVS_APPLIED = line.Substring(0, 60).Trim();
                            break;
                        case ClockHeaderLabel.SYS_DCBS_APPLIED:
                            header.SYS_DCBS_APPLIED = line.Substring(0, 60).Trim();
                            break;
                        case ClockHeaderLabel.LEAP_SECONDS://2016.11.06,double add
                            header.LEAP_SECONDS = int.Parse(line.Substring(0, 60).Trim());
                            break;
                        case ClockHeaderLabel.TYPES_OF_DATA:
                            header.COUNT_OF_TYPES_OF_DATA = int.Parse(line.Substring(0, 6));
                            for (int i = 0; i < header.COUNT_OF_TYPES_OF_DATA; i++)
                                header.TYPES_OF_DATA.Add(line.Substring(6 + 6 * i, 6).Trim());
                            break;
                        case ClockHeaderLabel.ANALYSIS_CENTER:
                            header.ANALYSIS_CENTER = line.Substring(0, 60).Trim();
                            break;
                        case ClockHeaderLabel.OF_CLK_REF:
                            header.OF_CLK_REF = line.Substring(0, 60).Trim();
                            break;
                        case ClockHeaderLabel.ANALYSIS_CLK_REF:
                            header.ANALYSIS_CLK_REF = line.Substring(0, 60).Trim();
                            break;
                        case ClockHeaderLabel.OF_SOLN_STA_TRF:
                            header.OF_SOLN_STA_TRF = line.Substring(0, 60).Trim();
                            break;
                        //AMC2 40472S004           -1248596310 -4819428207  3976505962SOLN STA NAME / NUM 
                        case ClockHeaderLabel.SOLN_STA_NAME_NUM:
                            if (header.ClockSolnStations == null) header.ClockSolnStations = new List<ClockSolnStation>();
                            header.ClockSolnStations.Add(ParseClockSolnStation(line.Substring(0, 60)));
                            break;
                        case ClockHeaderLabel.OF_SOLN_SATS:
                            header.OF_SOLN_SATS = int.Parse(line.Substring(0, 60).Trim());
                            break;
                        case ClockHeaderLabel.PRN_LIST:
                            if (header.PrnList == null) header.PrnList = new List<SatelliteNumber>();
                            var prnStrs = Geo.Utils.StringUtil.Split(line.Substring(0, 60).Trim() + " ", 4);//.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string prn in prnStrs) header.PrnList.Add(SatelliteNumber.Parse(prn));

                            break;
                        //rinex 3.0 nav
                        case ClockHeaderLabel.TIME_SYSTEM_CORR:

                            break;
                        //rinex 3.0 nav
                        case ClockHeaderLabel.IONOSPHERIC_CORR:

                            break;

                        case ClockHeaderLabel.END_OF_HEADER:
                            return header;
                        default: break;
                    }
                }
            }
            return header;
        }

        /// <summary>
        /// AMC2 40472S004           -1248596310 -4819428207  3976505962
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static ClockSolnStation ParseClockSolnStation(string line)
        {
            var x = StringUtil.ParseDouble(line, 25, 11);
            var y = StringUtil.ParseDouble(line, 37, 11);
            var z = StringUtil.ParseDouble(line, 49, 11);

            ClockSolnStation sol = new ClockSolnStation()
            {
                Name = line.Substring(0, 4),
                Number = line.Substring(5, 20).Trim(),
                XYZ = new XYZ(x, y, z)// XYZ.Parse(  line.Substring(25, 39), new char[] { ' ' })
            };
            sol.GeoCoord = CoordTransformer.XyzToGeoCoord(sol.XYZ);
            return sol;
        }
        #endregion

        /// <summary>
        /// 从文件开始获取时间
        /// </summary>
        /// <returns></returns>
        public override Time TryGetStartTime()
        {
            this.Reset();
            while (this.MoveNext())
            {
                return Current.Time;
            }
            return Time.MaxValue;
        }
        /// <summary>
        /// 从文件末尾获取时间
        /// </summary>
        /// <returns></returns>
        public override Time TryGetEndTime()
        {
            this.Reset();

            StreamReader.BaseStream.Position = StreamReader.BaseStream.Length - 500;
            var time = Time.MinValue;
            while (this.MoveNext())
            {
                time = Current.Time;
            }
            return time;
        }


        /// <summary>
        /// 移动到下一个，错误则返回false
        /// </summary>
        /// <returns></returns>
        public override bool MoveNext()
        {
            #region 流程控制
            CurrentIndex++;
            if (CurrentIndex == StartIndex) { log.Debug("数据流 " + this.Name + " 开始读取数据。"); }
            if (this.IsCancel) { log.Info("数据流 " + this.Name + " 已被手动取消。"); return false; }
            if (CurrentIndex > this.MaxEnumIndex) { log.Info("数据流 " + this.Name + " 已达指定的最大编号 " + this.MaxEnumIndex); return false; }
            while (CurrentIndex < this.StartIndex) { this.MoveNext(); }
            #endregion
            if (StreamReader.EndOfStream)
            { return false; }

            string line = null;
            while ((line = StreamReader.ReadLine()) != null)
            {
                if (String.IsNullOrWhiteSpace(line) || line.Length < 50)
                {
                    continue;
                }
                try
                {
                    string mark = line.Substring(0, 2);
                    if (mark != "AR" && mark != "AS")
                        continue;
                    var item = ParseRinexLine(line);
                    this.Current = item;
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error("读取钟差文件" + Name + "出错：" + ex.Message + line + ", 将继续尝试");
                    continue;
                }
            }
            return false;
        }
        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            StreamReader.BaseStream.Position = 0;
            StreamReader.BaseStream.Seek(0, SeekOrigin.Begin);

            RinexUtil.SkipHeader(StreamReader);
        }
    }


    


}
