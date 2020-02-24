//2014.10.24, czs, edit in namu shuangliao, 实现 IClockBiasService 接口
//2016.02.05, czs, edit in hongqing, 时段属性源自File
//2018,.03.17, czs, edit in hmx, 只针对卫星的， 多文件组合钟差数据源
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
    public class SimpelCombinedSatClockService : AbstractSimpleClockService, IComparable<SimpelCombinedSatClockService>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ClockFilePath"></param>
        public SimpelCombinedSatClockService(string ClockFilePath)
        {
            this.Name = System.IO.Path.GetFileName(ClockFilePath);

            this.ClockFile = new SimpleClockFileReader(ClockFilePath, true).ReadAll().GetSatClockCollection();

            this.clockInterpolators = new ConcurrentDictionary<string, SimpleClockInterpolator>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SatClockCollection"></param>
        public SimpelCombinedSatClockService(SatSimpleClockCollection SatClockCollection)
        {
            this.Name = SatClockCollection.Name;

            this.ClockFile = SatClockCollection;
            this.clockInterpolators = new ConcurrentDictionary<string, SimpleClockInterpolator>();
        }

        /// <summary>
        /// 缓存插值器
        /// </summary>
        ConcurrentDictionary<string, SimpleClockInterpolator> clockInterpolators;

        static object locker = new object();
        /// <summary>
        /// 钟差文件，采用懒加载的方式
        /// </summary>
        SatSimpleClockCollection ClockFile { get; set; }
        /// <summary>
        /// 包含的卫星类型
        /// </summary>
        public override List<SatelliteType> SatelliteTypes { get { return ClockFile.SatelliteTypes; } }
        /// <summary>
        /// 获取指定卫星在时段内的序列
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public override List<SimpleClockBias> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            return ClockFile.Get(prn).Values.FindAll(m => m.Time > timeStart && m.Time < timeEnd);
        }
        /// <summary>
        /// 指定卫星的所有钟差序列
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public List<SimpleClockBias> GetClocks(SatelliteNumber prn)
        {
            return ClockFile.Get(prn).Values;
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
            SimpleClockInterpolator clockInterpolator = GetClockInterpolator(nameOrPrn);
            if (clockInterpolator == null || !clockInterpolator.IsAvailable(gpsTime))
                return null;

            #region czs
            SimpleClockBias fittedClock = clockInterpolator.GetAtomicClock(gpsTime);
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
            SimpleClockInterpolator clockInterpolator = GetClockInterpolator(prnStr);
            if (clockInterpolator == null || !clockInterpolator.IsAvailable(time)) return false;
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
                if (SatelliteNumber.IsPrn(prnStr))
                {
                    var prn = SatelliteNumber.Parse(prnStr);
                    var items = ClockFile.Get(prn);
                    if (items == null || items.Count == 0) return null;

                    var inter = new SimpleClockInterpolator(items.Values);
                    clockInterpolators.TryAdd(prnStr, inter);
                    return inter;
                }
            }

            return null;
        }
        /// <summary>
        /// 有效时间
        /// </summary>
        public override BufferedTimePeriod TimePeriod
        {
            get
            {
                return ClockFile.TimePeriod;
            }
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SimpelCombinedSatClockService o = obj as SimpelCombinedSatClockService;
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
        public int CompareTo(SimpelCombinedSatClockService other)
        {
            return this.TimePeriod.CompareTo(other.TimePeriod);
        }
        /// <summary>
        /// 打印输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ", " + this.TimePeriod.ToString() + ", " + Geo.Utils.StringUtil.GetArrayString<SatelliteType>(this.SatelliteTypes.ToArray());
        }
    }

    /// <summary>
    ///只针对卫星的，多文件组合钟差数据源
    /// </summary>
    public class CombinedSatClockService : AbstractClockService, IComparable<CombinedSatClockService>
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ClockFilePath"></param>
        public CombinedSatClockService(string ClockFilePath) 
        { 
            this.Name = System.IO.Path.GetFileName(ClockFilePath);

            this.ClockFile = new ClockFileReader(ClockFilePath, true).ReadAll().GetSatClockCollection();

            this.clockInterpolators = new ConcurrentDictionary<string, ClockInterpolator>();           
        }
          /// <summary>
          /// 构造函数
          /// </summary>
          /// <param name="SatClockCollection"></param>
        public CombinedSatClockService(SatClockCollection SatClockCollection) 
        { 
            this.Name = SatClockCollection.Name;

            this.ClockFile = SatClockCollection;
            this.clockInterpolators = new ConcurrentDictionary<string, ClockInterpolator>();           
        }

        /// <summary>
        /// 缓存插值器
        /// </summary>
      ConcurrentDictionary<string, ClockInterpolator> clockInterpolators;
         
        static object locker = new object();
        /// <summary>
        /// 钟差文件，采用懒加载的方式
        /// </summary>
        SatClockCollection ClockFile { get; set; } 
        /// <summary>
        /// 包含的卫星类型
        /// </summary>
        public List<SatelliteType> SatelliteTypes { get { return ClockFile.SatelliteTypes; } }
        /// <summary>
        /// 获取指定卫星在时段内的序列
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public override List<AtomicClock> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            return ClockFile.Get(prn).Values.FindAll(m => m.Time > timeStart && m.Time < timeEnd);
        }
        /// <summary>
        /// 指定卫星的所有钟差序列
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public List<AtomicClock> GetClocks(SatelliteNumber prn)
        { 
            return ClockFile.Get(prn).Values;
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
            if (clockInterpolator == null || !clockInterpolator.IsAvailable(time)) return false;
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
                if (SatelliteNumber.IsPrn(prnStr))
                {
                    var prn = SatelliteNumber.Parse(prnStr);
                    var items = ClockFile.Get(prn);
                    if (items == null || items.Count == 0) return null;

                    var inter = new ClockInterpolator(items.Values);
                    clockInterpolators.TryAdd(prnStr, inter);
                    return inter; 
                } 
            }

            return null;
        }
        /// <summary>
        /// 有效时间
        /// </summary>
        public override BufferedTimePeriod TimePeriod {
            get
            {
                return ClockFile.TimePeriod;
            }
        } 
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            CombinedSatClockService o = obj as CombinedSatClockService;
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
        public int CompareTo(CombinedSatClockService other)
        {
            return this.TimePeriod.CompareTo(other.TimePeriod);
        }
        /// <summary>
        /// 打印输出
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ", " + this.TimePeriod.ToString() + ", " + Geo.Utils.StringUtil.GetArrayString<SatelliteType>(this.SatelliteTypes.ToArray());
        }
    }
}
