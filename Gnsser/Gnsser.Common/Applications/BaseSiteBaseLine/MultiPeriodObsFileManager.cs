//2019.01.13, czs, create in hmx, 多时段测站管理器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using System.IO;
using Gnsser.Data.Rinex;

namespace Gnsser
{
    //前提：不同的时段，相同测站文件名称不同。

    /// <summary>
    /// 多时段测站管理器。
    /// </summary>
    public class MultiPeriodObsFileManager : Geo.BaseDictionary<TimePeriod, ObsSiteFileManager>
    {
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="obsFilePaths"></param>
        /// <param name="MinEpochTimeSpan"></param>
        public MultiPeriodObsFileManager(string[] obsFilePaths, TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;

            var periods = ObsFilePeriodDivider.TimePeriodGroup(obsFilePaths, MinEpochTimeSpan.TotalMinutes);

            foreach (var item in periods)
            {
                var site = new ObsSiteFileManager(item.Value, MinEpochTimeSpan);
                this[item.Key] = site;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiPeriodObsFileManager(List<ObsSiteInfo> sites, TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;

            var periods = ObsFilePeriodDivider.TimePeriodGroup(sites, MinEpochTimeSpan.TotalMinutes);

            foreach (var item in periods)
            {
                var site = new ObsSiteFileManager(item.Value, MinEpochTimeSpan);
                this[item.Key] = site;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiPeriodObsFileManager(TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;
        }

        /// <summary>
        /// 最小的同步时段,少于则忽略
        /// </summary>
        public TimeSpan MinEpochTimeSpan { get; set; }
        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount
        {
            get
            {
                int i = 0;
                foreach (var item in this)
                {
                    i += item.Count;
                }
                return i;
            }
        }
        /// <summary>
        /// 测站数量
        /// </summary>
        public int SiteCount => SiteNames.Count;
        /// <summary>
        /// 时段数量
        /// </summary>
        public int PeriodCount => this.Count;
        /// <summary>
        /// 所有测站的名称
        /// </summary>
        public List<string> SiteNames {
            get {
                List<string> list = new List<string>();
                foreach (var item in this.Values)
                {
                    list.AddRange(item.SiteNames);
                }
                return list.Distinct().ToList();
            }
        }
        public override ObsSiteFileManager Create(TimePeriod key)
        {
            return new ObsSiteFileManager(MinEpochTimeSpan);
        }

        /// <summary>
        /// 所有时段的名称
        /// </summary>
        public List<TimePeriod> TimePeriods { get => this.Keys.ToList(); }
        /// <summary>
        /// 获取相交，有效的时段关键字。
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        public List<TimePeriod> GetTimePeriodKeys(TimePeriod timePeriod)
        {
            var result = new List<TimePeriod>();
            foreach (var item in TimePeriods)
            {
                var inter = item.GetIntersect(timePeriod);
                if(inter == null) { continue; }
                if(inter.TimeSpan > MinEpochTimeSpan)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 增加一个,如果已经添加则返回 false。
        /// </summary>
        /// <param name="filePath"></param>
        public bool Add(string filePath)
        {
            var site = new ObsSiteInfo(filePath);
            return Add(site);
        }
        /// <summary>
        /// 增加一个,如果已经添加则返回 false。
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool Add(ObsSiteInfo site)
        {
            if (site.TimePeriod.Span < MinEpochTimeSpan.TotalSeconds)
            {
                log.Warn(site + " 添加失败， 观测时段太短 " + site.TimePeriod);
                return false;
            }
            //是否已经存在 
            if (this.Contains(site))
            {
                log.Warn(site + " 添加失败， 已经存在时段 " + site + ", " + site.TimePeriod);
                return false;
            }
            foreach (var period in this.Keys)
            {
                var inter = period.GetIntersect(site.TimePeriod);
                if (inter != null && inter.Span > MinEpochTimeSpan.TotalSeconds)
                {
                    this[period].Add(site.SiteName, site);
                    site.NetPeriod = period;
                    return true;
                }
            }
            //没有则创建一个
            this.GetOrCreate(site.TimePeriod).Add(site.SiteName, site);
            site.NetPeriod = site.TimePeriod;
            return true;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public void Remove(ObsSiteInfo site)
        {
            foreach (var item in this.Values)
            {
                item.Remove(site);
            }
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool Contains(ObsSiteInfo site)
        {
            var allready = this.GetSites(site.SiteName);
            foreach (var item in allready)
            {
                var common = item.TimePeriod.GetIntersect(site.TimePeriod);
                if (common != null && common.TimeSpan > MinEpochTimeSpan)
                { 
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 测站时段信息
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public List<ObsSiteInfo> GetSites(string siteName)
        {
            List<ObsSiteInfo> result = new List<ObsSiteInfo>();
            foreach (var item in this)
            {
                if (item.Contains(siteName))
                {
                    result.Add(item.Get(siteName));
                }
            }
            return result;
        }
        /// <summary>
        /// 所有的测站
        /// </summary>
        /// <returns></returns>
        public List<ObsSiteInfo> GetAllSites()
        {
            List<ObsSiteInfo> result = new List<ObsSiteInfo>();
            foreach (var item in this)
            {
                result.AddRange(item.Values);
            }
            return result;
        }

        /// <summary>
        /// 提取时段
        /// </summary>
        /// <param name="sites"></param>
        /// <returns></returns>
        public MultiPeriodObsFileManager ExtractPeriodSites(List<ObsSiteInfo> sites)
        {
            var newPeriod = new MultiPeriodObsFileManager(this.MinEpochTimeSpan);
            foreach (var item in this.Keys)
            {
                newPeriod.GetOrCreate(item);
            }
            foreach (var line in sites)
            {
                foreach (var item in newPeriod.KeyValues)
                {
                    var inter = item.Key.GetIntersect(line.TimePeriod);
                    if (inter == null) { continue; }
                    if (inter.TimeSpan > MinEpochTimeSpan)
                    {
                        newPeriod[item.Key].Add(line);
                        break;
                    }
                }
            }
            newPeriod.RemoveEmptyPeriods();
            return newPeriod;
        }


        /// <summary>
        /// 移除空时段，返回移除后的列表。
        /// </summary>
        /// <returns></returns>
        public List<TimePeriod> RemoveEmptyPeriods()
        {
            List<TimePeriod> timePeriods = new List<TimePeriod>();
            foreach (var item in this.KeyValues)
            {
                if (item.Value.Count == 0) { timePeriods.Add(item.Key); }
            }
            this.Remove(timePeriods);
            return timePeriods;
        }

        public ObjectTableStorage GetSiteCoordResultTable()
        {
            ObjectTableStorage result = new ObjectTableStorage("坐标表格");
            int i = 1;
            foreach (var period in this.KeyValues)
            {

                foreach (var item in period.Value.KeyValues)
                {
                    result.NewRow();
                    result.AddItem("Num", i++);
                    result.AddItem("NetPeriod", period.Key); 
                    var site = item.Value;
                    if (site.EstimatedSite != null)
                    {
                        result.AddItem(site.EstimatedSite.GetObjectRow()); 
                    }
                } 
            }
            return result;
        }
    }
     
}
