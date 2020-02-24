//2016.04.19, czs, create in hongqing, 多个观测文件管理分析器
//2018.12.12, czs, edit in hmx, 单独提出

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using System.IO;

namespace Gnsser
{
    /// <summary>
    /// 管理器
    /// </summary>
    public class SiteObsBaselineManager : BaseDictionary<GnssBaseLineName, SiteObsBaseline>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteObsBaselineManager()
        {
            IsOrderedKeyEnabled = false;
            RemovedLines = new Dictionary<GnssBaseLineName, SiteObsBaseline>();
        }
       
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lines"></param>
        public SiteObsBaselineManager(List<SiteObsBaseline> lines) : this()
        {
            foreach (var line in lines)
            {
                this[line.LineName] = line;
            }
        }
        /// <summary>
        /// 移除的基线
        /// </summary>
        public Dictionary<GnssBaseLineName, SiteObsBaseline> RemovedLines { get; set; }

        #region 增减
        /// <summary>
        /// 移除基线
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(GnssBaseLineName key)
        {
            Remove(key, true);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isRecord"></param>
        public void Remove(GnssBaseLineName key, bool isRecord)
        {
            if (isRecord)
            {
                var line = Get(key);
                RemovedLines[key] = line;
            }

            base.Remove(key);
        }


        /// <summary>
        /// 获取具有平差值的名称
        /// </summary>
        /// <returns></returns>
        public List<GnssBaseLineName> GetLineNamesWithEstValue()
        {
            var list = new List<GnssBaseLineName>();
            foreach (var item in this.KeyValues)
            {
                if(item.Value.EstimatedResult != null)
                {
                    list.Add(item.Key); 
                }
            }
            return list;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public SiteObsBaseline Get(string siteName, string other)
        {
            List<SiteObsBaseline> lines = new List<SiteObsBaseline>();
            foreach (var kv in this.KeyValues)
            {
                if (kv.Key.Contains(siteName) && kv.Key.Contains(other))
                {
                    return kv.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public List<SiteObsBaseline> Get(string siteName)
        {
            List<SiteObsBaseline> lines = new List<SiteObsBaseline>();
            foreach (var kv in this.KeyValues)
            {
                if (kv.Key.Contains(siteName))
                {
                    lines.Add(kv.Value);
                }
            }
            return lines;
        }
        /// <summary>
        /// 移除包含指定测站的基线，返回移除数量
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="isRecord "></param>
        /// <returns></returns>
        public int Remove(string siteName,bool isRecord)
        {
            List<GnssBaseLineName> lines = new List<GnssBaseLineName>();
            foreach (var kv in this.KeyValues)
            {
                if (kv.Key.Contains(siteName))
                {
                    lines.Add(kv.Key);
                }
            }
            foreach (var item in lines.Distinct().ToList())
            {
                this.Remove(item, isRecord); 
            }
            return lines.Count;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="line"></param>
        public void Add(SiteObsBaseline line)
        {
            this.Add(line.LineName, line);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="siteObsBaselines"></param>
        /// <param name="isOverWrite"></param>
        public int Add(List<SiteObsBaseline> siteObsBaselines, bool isOverWrite = false)
        {
            int count = 0;
            IsOrderedKeyEnabled = false;
            foreach (var item in siteObsBaselines)
            {
                if (isOverWrite)
                {
                    count++;
                    this[item.LineName] = item;
                }
                else
                {
                    if (!this.Contains(item.LineName))
                    {
                        count++;
                        this.Add(item.LineName, item);
                    }
                }
            }
            return count;
        }
        #endregion

        /// <summary>
        /// 短基线
        /// </summary>
        /// <param name="MaxShortLineLength"></param>
        /// <returns></returns>
        public List<SiteObsBaseline> GetShortLines(double MaxShortLineLength)
        {
            return this.Values.FindAll(m => m.GetLength() <= MaxShortLineLength);
        }
        /// <summary>
        /// 长基线
        /// </summary>
        /// <param name="MaxShortLineLength"></param>
        /// <returns></returns>
        public List<SiteObsBaseline> GetLongLines(double MaxShortLineLength)
        {
            return this.Values.FindAll(m => m.GetLength() > MaxShortLineLength);
        } 
        /// <summary>
        /// 测站名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetSiteNames()
        {
            List<string> sites = new List<string>();
            foreach (var item in this)
            {
                sites.Add(item.RefName);
                sites.Add(item.RovName);
            }
            return sites.Distinct().ToList();
        }

        /// <summary>
        /// 获取所有的测量基线
        /// </summary>
        /// <returns></returns>
        public List<EstimatedBaseline> GetEstimatedBaselines()
        {
            var result = new List<EstimatedBaseline>();
            foreach (var item in this.Values)
            {
                var line = item.EstimatedResult;
                if(line == null) { continue; }

                result.Add( (EstimatedBaseline)line);
            }
            return result;
        }

        /// <summary>
        /// 构建基线网
        /// </summary>
        /// <returns></returns>
        public  BaseLineNet BuidBaseLineNet()
        {
            return new BaseLineNet(GetEstimatedBaselines());
        }

        /// <summary>
        /// 获取基线表格
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetBaselineTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("基线结果");
            foreach (var kv in this.KeyValues)
            {
                if (kv.Value.EstimatedResult == null) { continue; }

                table.NewRow();
                table.AddItem(kv.Value.EstimatedResult.GetObjectRow());
            }
            return table;
        }
         
        /// <summary>
        /// 是否是独立基线，如果所有基线包含，其中一个测站，只有一个，则为独立基线。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsIndependent(GnssBaseLineName name)
        {

            int refCount = 0;
            int rovCount = 0;
            foreach (var item in this.Keys)
            { 
                if (item.Contains(name.RefName)) { refCount++; }
                if (item.Contains(name.RovName)) { rovCount++; } 
            }
            return refCount <= 1 || rovCount <= 1;
        }
    /// <summary>
        /// 获取
        /// </summary>
        /// <param name="siteA"></param>
        /// <param name="siteB"></param>
        /// <returns></returns>
        public SiteObsBaseline GetOrReversed(string siteA, string siteB)
        {
            var lineName = new GnssBaseLineName(siteA, siteB);
            return GetOrReversed(lineName);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="lineName"></param>
        /// <returns></returns>
        public SiteObsBaseline GetOrReversed(GnssBaseLineName lineName)
        {
            if (this.Contains(lineName))
            {
                return this[lineName];
            }
            var reverseLine = lineName.ReverseBaseLine;
            if (this.Contains(reverseLine))
            {
                return this[reverseLine];
            }
            return null;
        }
        /// <summary>
        /// 是否包含测站对象
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool Contains(ObsSiteInfo site)
        {
            foreach (var item in this.Values)
            {
                if (item.Start.Equals(site) ||item.End.Equals(site)) { return true; }
            }
            return false;
        } 
        /// <summary>
        /// 移除，是否记录。
        /// </summary>
        /// <param name="site"></param>
        /// <param name="isRecord"></param>
        public void Remove(ObsSiteInfo site, bool isRecord)
        {
            List<GnssBaseLineName> todelte = new List<GnssBaseLineName>();
            foreach (var item in this.Values)
            {
                if (item.Contains(site)) { todelte.Add(item.LineName); }
            }
            foreach (var item in todelte)
            { 
                this.Remove(item, isRecord);
            }
        }
        /// <summary>
        /// 清理
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            RemovedLines.Clear();
        }
    }

    /// <summary>
    /// 结果状态
    /// </summary>
    public enum ResultState
    {
        Unknown,
        Good,
        Acceptable,
        Warning,
        Bad    
    }


    /// <summary>
    /// 测站基线
    /// </summary>
    public class SiteObsBaseline : Segment<ObsSiteInfo>, IComparable<SiteObsBaseline>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteObsBaseline( ObsSiteInfo rovSite, ObsSiteInfo refSite)
            : base(refSite, rovSite)
        {
            this.LineName = new GnssBaseLineName( rovSite.SiteName, refSite.SiteName)
            {
                RefFilePath = refSite.FilePath,
                RovFilePath = rovSite.FilePath
            };
        }
        /// <summary>
        /// 所在网络的时段
        /// </summary>
        public TimePeriod NetPeriod { get; set; }
        /// <summary>
        /// 结果状态
        /// </summary>
        public ResultState ResultState { get; set; }
        /// <summary>
        /// 基线名称
        /// </summary>
        public GnssBaseLineName LineName { get; set; }
        /// <summary>
        /// 当前解算的结果
        /// </summary>
        public IEstimatedBaseline EstimatedResult { get; set; }

        /// <summary>
        ///  参考站名称
        /// </summary>
        public string RefName => LineName.RefName;
        /// <summary>
        ///  流动站名称
        /// </summary>
        public string RovName => LineName.RovName;

        /// <summary>
        /// 时段，两个观测的共有时段。
        /// </summary>
        public TimePeriod TimePeriod
        {
            get
            {
                var first = Start.TimePeriod;
                var second = End.TimePeriod;

                var Segment = first.GetIntersect(second);
                if (Segment == null)
                {
                    return TimePeriod.Zero;
                }
                return new TimePeriod(Segment.Start, Segment.End);
            }
        }

        /// <summary>
        /// 共同时段信息。
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                var first = Start.TimePeriod;
                var second = End.TimePeriod;

                var Segment = first.GetIntersect(second);
                if (Segment == null)
                {
                    return TimeSpan.Zero;
                }

                return Segment.End.DateTime - Segment.Start.DateTime;
            }
        }

        /// <summary>
        /// 过滤获取指定时段的基线集合
        /// </summary>
        /// <param name="siteObsBaselines"></param>
        /// <param name="netPeriod"></param>
        /// <param name="minCommonSpan"></param>
        /// <returns></returns>
        static public List<SiteObsBaseline> GetLinesInPeriod(List<SiteObsBaseline> siteObsBaselines, TimePeriod netPeriod, TimeSpan minCommonSpan)
        {
            var currentPeriodLines = new List<SiteObsBaseline>();

            foreach (var line in siteObsBaselines)
            {
                var inter = (line.TimePeriod.GetIntersect(netPeriod));
                if (inter == null || inter.TimeSpan < minCommonSpan) { continue; }

                currentPeriodLines.Add(line);
            }
            return currentPeriodLines;
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <returns></returns>
        public double GetLength()
        {
            if(this.EstimatedResult != null) { return EstimatedResult.EstimatedVector.Length; }

            var vector = (this.End.SiteObsInfo.ApproxXyz - this.Start.SiteObsInfo.ApproxXyz);

            return vector.Length;
        }
        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override bool Contains(ObsSiteInfo val)
        {
            return this.Start.Equals(val) || End.Equals(val);
        }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return LineName.Name;
        }
        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var o = obj as SiteObsBaseline;
            if(o == null) { return false; }
            return o.LineName.Equals(this.LineName) && this.TimePeriod.Equals(o.TimePeriod);
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return LineName.GetHashCode();
        }

        public int CompareTo(SiteObsBaseline other)
        {
            return TimePeriod.CompareTo(other.TimePeriod);
        }
    }
}