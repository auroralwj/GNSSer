//2014.10.24, czs, edit in namu shuangliao, 实现 IClockBiasService 接口
//2016.02.05, czs, edit in hongqing, 时段属性源自File
//2018.05.02, czs, edit in hmx, 提取抽象钟差服务类

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm;
using Gnsser.Service;
using Geo.Times;
using Geo;

namespace Gnsser.Data
{ 
     


    /// <summary>
    /// 钟差数据源。接收钟差文件或列表进行插值计算。
    /// </summary>
    public class SimpleClockService : AbstractSimpleClockService, IComparable<SimpleClockService>
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ClockFilePath"></param>
        public SimpleClockService(string ClockFilePath, bool isSatOnly =true) 
        {
            this.InputPath = ClockFilePath;
            this.Name = System.IO.Path.GetFileName(ClockFilePath);

            this.Reader = new SimpleClockFileReader(ClockFilePath, isSatOnly);

            this.clockInterpolators = new ConcurrentDictionary<string, SimpleClockInterpolator>();           
        }

        /// <summary>
        /// 头部信息
        /// </summary>
        public ClockFileHeader Header { get; set; }
        /// <summary>
        /// 读取器
        /// </summary>
        public SimpleClockFileReader Reader { get; set; }

        /// <summary>
        /// 输入路径
        /// </summary>
        public string InputPath { get; set; } 

        /// <summary>
        /// 缓存插值器
        /// </summary>
      ConcurrentDictionary<string, SimpleClockInterpolator> clockInterpolators;

        SimpleClockFile _clockFile;
        static object locker = new object();
        /// <summary>
        /// 钟差文件，采用懒加载的方式
        /// </summary>
        SimpleClockFile ClockFile
        {
            get
            {
                lock (locker)
                {
                    if (IsLoadedFile) { return _clockFile; }
                    else { _clockFile = Reader.ReadAll(); }
                    return _clockFile;
                }
            }
        }
        /// <summary>
        /// 是否完全加载到内存
        /// </summary>
        public bool IsLoadedFile { get { return _clockFile != null; } }
        /// <summary>
        /// 包含的卫星类型
        /// </summary>
        public override List<SatelliteType> SatelliteTypes { get { return ClockFile.Header.SatelliteTypes; } }
        /// <summary>
        /// 获取指定卫星在时段内的序列
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public override List<SimpleClockBias> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        { 
            return ClockFile.GetClockItems( prn, timeStart, timeEnd);
        }
        /// <summary>
        /// 指定卫星的所有钟差序列
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public List<SimpleClockBias> GetClocks(SatelliteNumber prn)
        {
            string prnStr = prn.ToString();
            return ClockFile.GetClockItems(prnStr);
        }
        /// <summary>
        /// 采样间隔
        /// </summary>
        /// <returns></returns>
        public double GetInterval()
        {
            return ClockFile.Interval;
        }

        /// <summary> 
        /// 根据接收机时间和位置，获取计算卫星发射时刻的位置。不计算相对地面的延迟。
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <param name="gpsTime">时间</param>
        /// <returns>如果返回 null，在表示计算失败</returns>
        public override SimpleClockBias Get(SatelliteNumber prn, Time gpsTime)
        {
            string prnStr = prn.ToString();
            return Get(prnStr, gpsTime);
        }

        /// <summary> 
        /// 根据接收机时间和位置，获取计算卫星发射时刻的位置。不计算相对地面的延迟。
        /// </summary>
        /// <param name="nameOrPrn">卫星编号</param>
        /// <param name="gpsTime">时间</param>
        /// <returns>如果返回 null，在表示计算失败</returns>
        public override SimpleClockBias Get(string nameOrPrn, Time gpsTime)
        { 
            var clockInterpolator = GetClockInterpolator(nameOrPrn);
            if (clockInterpolator ==null || !clockInterpolator.IsAvailable(gpsTime))
                return null;

            #region czs
            var fittedClock = clockInterpolator.GetAtomicClock(gpsTime);
            #endregion

            return fittedClock;
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsAvailable(SatelliteNumber prn, Time time)
        {
            string prnStr = prn.ToString();
            var clockInterpolator = GetClockInterpolator(prnStr);
            if (!clockInterpolator.IsAvailable(time)) return false;
            return true;
        }
        static object locker2 = new object();
        /// <summary>
        /// 插值器
        /// </summary>
        /// <param name="prnStr"></param>
        /// <returns></returns>
        private SimpleClockInterpolator GetClockInterpolator(string prnStr)
        {
            if (clockInterpolators.ContainsKey(prnStr))
            {
                return clockInterpolators[prnStr];
            }
            lock (locker2)
            {
                if (clockInterpolators.ContainsKey(prnStr))
                {
                    return clockInterpolators[prnStr];
                }

                var items = ClockFile.GetClockItems(prnStr);
                if (items == null || items.Count == 0) return null;

                var inter = new SimpleClockInterpolator(items);
                clockInterpolators.TryAdd(prnStr, inter);
                return inter;
            }
        }
        /// <summary>
        /// 有效时间
        /// </summary>
        public override BufferedTimePeriod TimePeriod {
            get
            {
                if (IsLoadedFile &&  _clockFile.TimePeriod != null) { return ClockFile.TimePeriod; }
                else
                {
                    var endTime = Reader.TryGetEndTime();
                    var startTime = Reader.TryGetStartTime();
                    return new BufferedTimePeriod(startTime, endTime, 1);
                }
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        { 
            return Name ;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ClockService o = obj as ClockService;
            if (o == null)
            {
                return false;
            }
            return o.TimePeriod.Equals(TimePeriod) && Name == o.Name;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        /// <summary>
        /// 时段比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(SimpleClockService other)
        {
            return this.TimePeriod.CompareTo(other.TimePeriod);
        }
         
    }




    /// <summary>
    /// 钟差数据源。接收钟差文件或列表进行插值计算。
    /// </summary>
    public class ClockService : AbstractClockService, IComparable<ClockService>
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ClockFilePath"></param>
        public ClockService(string ClockFilePath, bool isSatOnly =true) 
        {
            this.InputPath = ClockFilePath;
            this.Name = System.IO.Path.GetFileName(ClockFilePath);

            this.Reader = new ClockFileReader(ClockFilePath, isSatOnly);

            this.clockInterpolators = new ConcurrentDictionary<string, ClockInterpolator>();           
        }

        /// <summary>
        /// 头部信息
        /// </summary>
        public ClockFileHeader Header { get; set; }
        /// <summary>
        /// 读取器
        /// </summary>
        public ClockFileReader Reader { get; set; }

        /// <summary>
        /// 输入路径
        /// </summary>
        public string InputPath { get; set; } 

        /// <summary>
        /// 缓存插值器
        /// </summary>
      ConcurrentDictionary<string, ClockInterpolator> clockInterpolators;

        ClockFile _clockFile;
        static object locker = new object();
        /// <summary>
        /// 钟差文件，采用懒加载的方式
        /// </summary>
        ClockFile ClockFile
        {
            get
            {
                lock (locker)
                {
                    if (IsLoadedFile) { return _clockFile; }
                    else { _clockFile = Reader.ReadAll(); }
                    return _clockFile;
                }
            }
        }
        /// <summary>
        /// 是否完全加载到内存
        /// </summary>
        public bool IsLoadedFile { get { return _clockFile != null; } }
        /// <summary>
        /// 包含的卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get { return ClockFile.Header.SatelliteTypes; } }
        /// <summary>
        /// 获取指定卫星在时段内的序列
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public override List<AtomicClock> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        { 
            return ClockFile.GetClockItems( prn, timeStart, timeEnd);
        }
        /// <summary>
        /// 指定卫星的所有钟差序列
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public List<AtomicClock> GetClocks(SatelliteNumber prn)
        {
            string prnStr = prn.ToString();
            return ClockFile.GetClockItems(prnStr);
        }
        /// <summary>
        /// 采样间隔
        /// </summary>
        /// <returns></returns>
        public double GetInterval()
        {
            return ClockFile.Interval;
        }

        /// <summary> 
        /// 根据接收机时间和位置，获取计算卫星发射时刻的位置。不计算相对地面的延迟。
        /// </summary>
        /// <param name="prn">卫星编号</param>
        /// <param name="gpsTime">时间</param>
        /// <returns>如果返回 null，在表示计算失败</returns>
        public override AtomicClock Get(SatelliteNumber prn, Time gpsTime)
        {
            string prnStr = prn.ToString();
            return Get(prnStr, gpsTime);
        }

        /// <summary> 
        /// 根据接收机时间和位置，获取计算卫星发射时刻的位置。不计算相对地面的延迟。
        /// </summary>
        /// <param name="nameOrPrn">卫星编号</param>
        /// <param name="gpsTime">时间</param>
        /// <returns>如果返回 null，在表示计算失败</returns>
        public override AtomicClock Get(string nameOrPrn, Time gpsTime)
        { 
            ClockInterpolator clockInterpolator = GetClockInterpolator(nameOrPrn);
            if (clockInterpolator ==null || !clockInterpolator.IsAvailable(gpsTime))
                return null;

            #region czs
            AtomicClock fittedClock = clockInterpolator.GetAtomicClock(gpsTime);
            #endregion

            return fittedClock;
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsAvailable(SatelliteNumber prn, Time time)
        {
            string prnStr = prn.ToString();
            ClockInterpolator clockInterpolator = GetClockInterpolator(prnStr);
            if (!clockInterpolator.IsAvailable(time)) return false;
            return true;
        }
        static object locker2 = new object();
        /// <summary>
        /// 插值器
        /// </summary>
        /// <param name="prnStr"></param>
        /// <returns></returns>
        private ClockInterpolator GetClockInterpolator(string prnStr)
        {
            if (clockInterpolators.ContainsKey(prnStr))
            {
                return clockInterpolators[prnStr];
            }
            lock (locker2)
            {
                if (clockInterpolators.ContainsKey(prnStr))
                {
                    return clockInterpolators[prnStr];
                }

                var items = ClockFile.GetClockItems(prnStr);
                if (items == null || items.Count == 0) return null;

                var inter = new ClockInterpolator(items);
                clockInterpolators.TryAdd(prnStr, inter);
                return inter;
            }
        }
        /// <summary>
        /// 有效时间
        /// </summary>
        public override BufferedTimePeriod TimePeriod {
            get
            {
                if (IsLoadedFile &&  _clockFile.TimePeriod != null) { return ClockFile.TimePeriod; }
                else
                {
                    var endTime = Reader.TryGetEndTime();
                    var startTime = Reader.TryGetStartTime();
                    return new BufferedTimePeriod(startTime, endTime, 1);
                }
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        { 
            return Name ;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ClockService o = obj as ClockService;
            if (o == null)
            {
                return false;
            }
            return o.TimePeriod.Equals(TimePeriod) && Name == o.Name;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        /// <summary>
        /// 时段比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ClockService other)
        {
            return this.TimePeriod.CompareTo(other.TimePeriod);
        }
         
    }


}
