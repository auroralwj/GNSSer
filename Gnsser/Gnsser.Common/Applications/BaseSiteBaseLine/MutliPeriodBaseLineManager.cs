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
using Geo.Coordinates;

namespace Gnsser
{ 
    /// <summary>
    /// 多时段基线管理器
    /// </summary>
    public class MutliPeriodBaseLineManager : BaseDictionary<TimePeriod, SiteObsBaselineManager>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MinEpochTimeSpan"></param>
        public MutliPeriodBaseLineManager(TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MinEpochTimeSpan"></param>
        /// <param name="lines"></param>
        public MutliPeriodBaseLineManager(List<SiteObsBaseline> lines, TimeSpan MinEpochTimeSpan)
        {
            this.MinEpochTimeSpan = MinEpochTimeSpan;
            Add(lines);
        }
        /// <summary>
        /// 最小的同步时段,少于则忽略
        /// </summary>
        public TimeSpan MinEpochTimeSpan { get; set; }
        /// <summary>
        /// 不重复的基线名称
        /// </summary>
        public List<GnssBaseLineName > LineNames {
             get
            {
                var list = new List<GnssBaseLineName>();
                foreach (var item in this.Values)
                {
                    list.AddRange(item.Keys);
                }
                return list.Distinct().ToList();
            }
        }
        /// <summary>
        /// 获取具有平差值的名称
        /// </summary>
        /// <returns></returns>
        public List<GnssBaseLineName> GetLineNamesWithEstValue()
        {
            var list = new List<GnssBaseLineName>();
            foreach (var item in this.Values)
            {
                list.AddRange(item.GetLineNamesWithEstValue());
            }
            return list.Distinct().ToList();
        }


        public List<string> GetSiteNames()
        {
            List<string> siteNames = new List<string>();
            foreach (var item in this)
            {
                siteNames.AddRange(item.GetSiteNames());
            }

            return siteNames.Distinct().ToList();
        }

        /// <summary>
        /// 所有基线
        /// </summary>
        public List<SiteObsBaseline> AllLineObjs
        {
             get
             {
                var list = new List<SiteObsBaseline>();
                foreach (var collection in this.Values)
                { 
                   list.AddRange(collection.Values); 
                }
                return list.Distinct().ToList();
             }
        }
        /// <summary>
        /// 所有基线
        /// </summary>
        /// <returns></returns>
        public List<SiteObsBaseline> GetAllBaseLines() { return AllLineObjs; }
        /// <summary>
        /// 已经移除的集合
        /// </summary>
        public List<SiteObsBaseline> RemovedLines
        {
            get
            {
                List<SiteObsBaseline> removedLines = new List<SiteObsBaseline>();
                foreach (var item in this)
                {
                    removedLines.AddRange(item.RemovedLines.Values);
                }

                return removedLines;
            }
        }

        /// <summary>
        /// 获取所有具有平差结果的基线
        /// </summary>
        /// <returns></returns>
        public List<EstimatedBaseline> GetAllEstLines()
        {
            List<SiteObsBaseline> lst = GetAllBaseLines();
            var result = new List<EstimatedBaseline>();
            foreach (var item in lst)
            {
                var est = item.EstimatedResult;
                if (est == null) { continue; }

                result.Add((EstimatedBaseline)(est));
            }
            return result;
        }
        /// <summary>
        /// 名称不同的基线数量
        /// </summary>
        public int CountOfDifferLineName => LineNames.Count;
        /// <summary>
        /// 所有基线数量。
        /// </summary>
        public int TotalLineCount => this.GetAllBaseLines().Count;
        /// <summary>
        /// 时段
        /// </summary>
        public List<TimePeriod> TimePeriods => Keys;

        /// <summary>
        /// 当前基线网，通过 GetOrBuildPeriodBaseLineNet 获取
        /// </summary>
        private MultiPeriodBaseLineNet BaseLineNets { get; set; }
        /// <summary>
        /// 时段三角闭合差检核，通过 GetOrCreatePeriodSyncTriguilarNetQualitiyManager  获取
        /// </summary>
        private PeriodTriguilarNetQualitiyManager PeriodTriguilarNetQualitiyManager { get; set; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override SiteObsBaselineManager Create(TimePeriod key)
        {
            return new SiteObsBaselineManager();
        }
        /// <summary>
        /// 获取所有时段的同名基线对象
        /// </summary>
        /// <param name="lineName"></param>
        /// <returns></returns>
        public List<SiteObsBaseline> GetLines(GnssBaseLineName lineName)
        {
            List<SiteObsBaseline> result = new List<SiteObsBaseline>();

            foreach (var item in this)
            {
                if (item.Contains(lineName))
                {
                    result.Add(item.Get(lineName));
                }
            }
            return result;
        }
        /// <summary>
        /// 重新添加。覆盖添加。
        /// </summary>
        /// <param name="dic"></param>
        public void Add(Dictionary<TimePeriod, List<SiteObsBaseline>> dic)
        {
            foreach (var item in dic)
            {
                this[item.Key] = new SiteObsBaselineManager( item.Value);
            } 
        }

        /// <summary>
        /// 提取时段
        /// </summary>
        /// <param name="sitebaseLines"></param>
        /// <returns></returns>
        public MutliPeriodBaseLineManager ExtractPeriodLines(List<SiteObsBaseline> sitebaseLines)
        {
            var newPeriod = new MutliPeriodBaseLineManager(this.MinEpochTimeSpan);
            foreach (var item in this.Keys)
            {
                newPeriod.GetOrCreate(item);
            }
            foreach (var line in sitebaseLines)
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
        /// 最大匹配的时段键.没则返回null
        /// </summary>
        /// <param name="linePeriod"></param>
        /// <returns></returns>
        public TimePeriod GetPeriodKey(TimePeriod linePeriod)
        {
            return TimePeriod.GetMaxCommon(this.Keys, linePeriod); 
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool Contains(SiteObsBaseline line)
        {
            var allready = this.GetLines(line.LineName);
            foreach (var item in allready)
            {
                var common = item.TimePeriod.GetIntersect(line.TimePeriod);
                if (common != null && common.TimeSpan > MinEpochTimeSpan)
                {
                    return true;
                }
            }
            return false;
        }

        #region 增加移除
        /// <summary>
        /// 返回成功的
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<SiteObsBaseline> Add(List<SiteObsBaseline> lines)
        {
            List<SiteObsBaseline> oks = new List<SiteObsBaseline>();
            foreach (var item in lines)
            {
               if(this.Add(item))
                {
                    oks.Add(item);
                }
            }
            return oks;
        }
        /// <summary>
        /// 增加一个,如果已经添加则返回 false。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool Add(SiteObsBaseline line)
        {
            if (line.TimePeriod.Span < MinEpochTimeSpan.TotalSeconds)
            {
                log.Warn(line + " 添加失败， 观测时段太短 " + line.TimePeriod);
                return false;
            }
            //是否已经存在 
            if (this.Contains(line))
            {
                log.Warn(line + " 添加失败， 已经存在时段 " + line + ", " + line.TimePeriod);
                return false;
            }

            var key = GetPeriodKey(line.TimePeriod);

            if(key != null) //有则直接添加
            {
                var net = this[key];
                if (net.Contains(line.LineName)) //如果已经存在，则比较时段长短
                {
                    var old = net.Get(line.LineName);
                    if(old.TimeSpan > line.TimeSpan)
                    {
                        log.Warn(" 时段 " + key.ToDefualtPathString() + " 已经具有 " + line + ",且时段更长，因此忽略本基线 " 
                            + old.TimePeriod.ToDefualtPathString() + "(旧) > " + line.TimePeriod.ToDefualtPathString() + "(新)");
                        return false;
                    }
                    else
                    {
                        net[line.LineName] = line;
                        log.Warn(" 时段 " + key.ToDefualtPathString() + " 已经具有 " + line + ",但本时段更长，因此旧基线被替换 "
                            + old.TimePeriod.ToDefualtPathString() + "(旧)  < " + line.TimePeriod.ToDefualtPathString() + "(新)");
                    }
                }
                else
                {
                    net.Add(line.LineName, line); 
                }
                line.NetPeriod = key;
            }
            else
            {
                //没有，则创建一个,并以之为时段关键字
                this.GetOrCreate(line.TimePeriod).Add(line.LineName, line); 
                line.NetPeriod = line.TimePeriod;
            }
             
            return true;
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
                if(item.Value.Count == 0) { timePeriods.Add(item.Key); }
            } 
            this.Remove(timePeriods);
            return timePeriods;
        }
        /// <summary>
        /// 移除基线
        /// </summary>
        /// <param name="lines"></param>
        public void Remove(List<SiteObsBaseline> lines)
        {
            foreach (var line in lines)
            {
                Remove(line);
            }
        }
        /// <summary>
        /// 移除基线
        /// </summary>
        /// <param name="line"></param>
        public void Remove(SiteObsBaseline line)
        {
            var key = this.GetPeriodKey(line.TimePeriod);
            this.Get(key).Remove(line.LineName);
        }
        /// <summary>
        /// 移除基线
        /// </summary>
        /// <param name="line"></param>
        public void Remove(GnssBaseLineName line)
        {
            foreach (var item in this.KeyValues)
            {
                item.Value.Remove(line);
            } 
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="siteName"></param>
        public void Remove(string siteName)
        {
            foreach (var item in this)
            {
                item.Remove(siteName, true);
            }
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="site"></param>
        public void Remove(ObsSiteInfo site)
        { 
            foreach (var item in this.KeyValues)
            {
                if (item.Value.Contains(site))
                {
                    item.Value.Remove(site, true); 
                } 
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        public override void Clear()
        {
            foreach (var item in this)
            {
                item.Clear();
            }
            base.Clear(); 
        }
        #endregion



        /// <summary>
        /// 所有的表格
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetBaselineTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("基线列表");
            foreach (var periodKv in this.KeyValues)
            { 
                foreach (var kv in periodKv.Value.KeyValues)
                {
                    if (kv.Value.EstimatedResult == null) { continue; }

                    table.NewRow();
                    table.AddItem("Period", periodKv.Key);
                    table.AddItem(kv.Value.EstimatedResult.GetObjectRow());
                }
            }
            return table;
        }
        /// <summary>
        /// 时段基线网
        /// </summary>
        /// <param name="isRebuit"></param>
        /// <returns></returns>
        public MultiPeriodBaseLineNet GetOrBuildPeriodBaseLineNet(bool isRebuit)
        {
            if (BaseLineNets == null || isRebuit)
            {
                BaseLineNets = new MultiPeriodBaseLineNet(); 
                foreach (var item in this.KeyValues)
                {
                    BaseLineNets[item.Key] = item.Value.BuidBaseLineNet();
                }
            }

            return BaseLineNets;
        }

        /// <summary>
        /// 获取各时段同步三角闭合差。
        /// </summary>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        /// <param name="isRebuit"></param>
        /// <returns></returns>
        public PeriodTriguilarNetQualitiyManager GetOrCreatePeriodSyncTriguilarNetQualitiyManager(GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy, bool isRebuit)
        {
            if (PeriodTriguilarNetQualitiyManager == null || isRebuit)
            {
                PeriodTriguilarNetQualitiyManager = new PeriodTriguilarNetQualitiyManager();
                var nets = GetOrBuildPeriodBaseLineNet(isRebuit);
                foreach (var item in nets.KeyValues)
                {
                    PeriodTriguilarNetQualitiyManager[item.Key] = item.Value.BuildTriangularClosureQualies(GnssReveiverNominalAccuracy);
                }
            }
            return PeriodTriguilarNetQualitiyManager;
        }
        /// <summary>
        /// 获取当前时段三角形闭合差
        /// </summary>
        /// <param name="timePeriodKey"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        /// <param name="isRebuit"></param>
        /// <returns></returns>
        public TriguilarNetQualitiyManager GetTriguilarNetQualitiyManager(TimePeriod timePeriodKey, GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy, bool isRebuit)
        {
            var dic = GetOrCreatePeriodSyncTriguilarNetQualitiyManager(GnssReveiverNominalAccuracy, isRebuit);

            var max = TimePeriod.GetMaxCommon(dic.Keys, timePeriodKey);
            if (max == null) { return new TriguilarNetQualitiyManager(); }

            return dic[max];
        }
        RepeatErrorOfBaseLineManager RepeatErrorOfBaseLineManager { get; set; }
        /// <summary>
        /// 获取或创建复测基线较差
        /// </summary>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        /// <param name="isRebuit"></param>
        /// <returns></returns>
        public RepeatErrorOfBaseLineManager GetOrCreateRepeatErrorOfBaseLineManager(GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy, bool isRebuit)
        {
            if (RepeatErrorOfBaseLineManager == null || isRebuit)
            {
                RepeatErrorOfBaseLineManager = CreateRepeatErrorOfBaseLineManager(GnssReveiverNominalAccuracy);
            }
            return RepeatErrorOfBaseLineManager;
        }
        /// <summary>
        /// 获取复测基线较差
        /// </summary>
        /// <param name="GnssReveiverNominalAccuracy"></param> 
        /// <returns></returns>
        public RepeatErrorOfBaseLineManager CreateRepeatErrorOfBaseLineManager(GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        {
            RepeatErrorOfBaseLineManager result = new RepeatErrorOfBaseLineManager(GnssReveiverNominalAccuracy);

            var allObjLines = this.GetAllBaseLines();
            foreach (var item in allObjLines)
            {
                List<SiteObsBaseline> sameNameLines = GetLines(item.LineName);

                var repeatError = result.GetOrCreate(item);
                repeatError.Init(sameNameLines);
            }
            return result;
        }
        /// <summary>
        /// 构建独立基线
        /// </summary>
        /// <param name="IndependentLineSelectType"></param>
        /// <returns></returns>
        public MultiPeriodBaseLineNet BuidIndependentNets(IndependentLineSelectType IndependentLineSelectType)
        { 
            MultiPeriodBaseLineNet result = new MultiPeriodBaseLineNet();
            foreach (var kvs in this.KeyValues)
            {
                var BaseLineNet = kvs.Value.BuidBaseLineNet();
                //注意这里返回的是不同时段的独立基线集合
                var independentLineNet = BaseLineNet.GetIndependentNet(IndependentLineSelectType);

                result[kvs.Key] = (independentLineNet);
            }
            return result;
        }
         
    } 
}
