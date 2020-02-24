//2013.01.22, czs,  Rinex 钟差文件
//2015.04.19, czs, edit in hailutu 通辽， 采用字典代替列表，性能提升 10 倍！
//2016.02.05, czs, edit in hongqing, 增加时段属性
//2018.05.08, czs, edit in hmx, 轻量级设计


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Times;
using Geo.Times;
using Geo;


namespace Gnsser.Data.Rinex
{
    /// <summary>
    ///  Rinex 钟差文件
    /// </summary>
    public class ClockFile : ClockFile<AtomicClock>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClockFile()
        {
        }

        /// <summary>
        /// 生成集合
        /// </summary>
        /// <returns></returns>
        public new SatClockCollection  GetSatClockCollection()
        {
            SatClockCollection  satClockCollection = new SatClockCollection(true, SourceCode);

            foreach (var kv in Data)
            {
                if (SatelliteNumber.IsPrn(kv.Key))
                {
                    var prn = SatelliteNumber.Parse(kv.Key);
                    var list = satClockCollection.GetOrCreate(prn);
                    foreach (var item in this.Get(prn.ToString()))
                    {
                        list.Add(item);
                    }
                }
            }
            return satClockCollection;
        }
    }
    /// <summary>
    ///  Rinex 钟差文件
    /// </summary>
    public class SimpleClockFile : ClockFile<SimpleClockBias>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleClockFile()
        {
        }

 /// <summary>
        /// 生成集合
        /// </summary>
        /// <returns></returns>
        public new SatSimpleClockCollection GetSatClockCollection()
        {
            SatSimpleClockCollection satClockCollection = new SatSimpleClockCollection(true, SourceCode);

            foreach (var kv in Data)
            {
                if (SatelliteNumber.IsPrn(kv.Key))
                {
                    var prn = SatelliteNumber.Parse(kv.Key);
                    var list = satClockCollection.GetOrCreate(prn);
                    foreach (var item in this.Get(prn.ToString()))
                    {
                        list.Add(item);
                    }
                }
            }
            return satClockCollection;
        }
    }
    /// <summary>
    /// Rinex 钟差文件 
    /// </summary>
    public class ClockFile<TAtomicClock> : BaseDictionary<string, List<TAtomicClock>>, IIgsProductFile
        where TAtomicClock  : ISimpleClockBias
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClockFile()
        { 
        }

        #region 核心属性
        /// <summary>
        /// 头文件
        /// </summary>
        public ClockFileHeader Header { get; set; }

        #endregion 

        /// <summary>
        /// 代码
        /// </summary>
        public string SourceCode { get { return Header.SourceName.Substring(0, 2); } }
        /// <summary>
        /// 钟采样间隔
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// 钟的数量
        /// </summary>
        public int ClockCount { get { return this.Count; } }

        /// <summary>
        /// 所有的名称列表。
        /// </summary>
        public List<string> Names { get { return new List<string>(Keys); } }
        /// <summary>
        /// 所有的钟差列表
        /// </summary>
        public List<TAtomicClock> AllItems
        {
            get
            {
                var list = new List<TAtomicClock>();
                foreach (var item in this)
                {
                    list.AddRange(item);
                }
                return list;
            }
        }

        /// <summary>
        /// 有效时间
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; } 

        /// <summary>
        ///  从原始文件中筛选
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<TAtomicClock> GetClockItems(SatelliteNumber prn, Time from, Time to)
        {
            return GetClockItems(prn.ToString(), from, to);
        }
        /// <summary>
        /// 从原始文件中筛选
        /// </summary>
        /// <param name="nameOrPrn"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<TAtomicClock> GetClockItems(string nameOrPrn, Time from, Time to)
        {
            var list = GetClockItems(nameOrPrn);
            return list.FindAll(m =>
                        m.Time >= from
                     && m.Time <= to);
        }

        public List<TAtomicClock> GetClockItems(SatelliteNumber prn)
        {
            return GetClockItems(prn.ToString());
        }
        /// <summary>
        /// 返回指定列表引用,若无则创建，并关联。
        /// </summary>
        /// <param name="nameOrPrn"></param>
        /// <returns></returns>
        public List<TAtomicClock> GetClockItems(string nameOrPrn)
        { 
            return   GetOrCreate(nameOrPrn);
        }

        public override List<TAtomicClock> Create(string key)
        {
            return new List<TAtomicClock>();
        }


        public TAtomicClock GetClockItem(string nameOrPrn, Time currentTime)
        {
            var list = GetClockItems(nameOrPrn);
            return list.Find(m => m.Time == currentTime);
        }
        /// <summary>
        /// 生成集合
        /// </summary>
        /// <returns></returns>
        public SatClockCollection<TAtomicClock, ClockFile<TAtomicClock>> GetSatClockCollection()
        {
           var  satClockCollection = new SatClockCollection<TAtomicClock, ClockFile<TAtomicClock>>(true, SourceCode);
            
            foreach (var kv in Data)
            {
                if (SatelliteNumber.IsPrn(kv.Key))
                {
                    var prn = SatelliteNumber.Parse(kv.Key);
                    var list = satClockCollection.GetOrCreate(prn);
                    foreach (var item in this.Get(prn.ToString()))
                    {
                        list.Add(item); 
                    }
                }                
            }
            return satClockCollection; 
        }
        /// <summary>
        /// 移除测站钟差，为文件瘦身
        /// </summary>
        /// <returns></returns>
        public int RemoveSiteColcks()
        {
            this.Header.ClockSolnStations.Clear();
            this.Header.UpdateGnsserInfo();


            List<string> sites = new List<string>();
            foreach (var item in this.Keys)
            {
                if(item.Length > 3) { sites.Add(item); }
            }
            this.Remove(sites);
            return sites.Count;
        }
    }

}
