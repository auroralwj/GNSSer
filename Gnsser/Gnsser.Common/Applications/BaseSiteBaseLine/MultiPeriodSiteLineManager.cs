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

    /// <summary>
    /// 多时段基线管理器
    /// </summary>
    public class MultiPeriodSiteLineManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TimeSpan"></param>
        public MultiPeriodSiteLineManager(TimeSpan TimeSpan)
        {
            SiteManager = new MultiPeriodObsFileManager(TimeSpan);
            BaseLineManager = new MutliPeriodBaseLineManager(TimeSpan);
        }
        /// <summary>
        /// 测站管理器
        /// </summary>
        public MultiPeriodObsFileManager SiteManager { get; set; }
        /// <summary>
        /// 基线与基线网管理器
        /// </summary>
        public MutliPeriodBaseLineManager BaseLineManager { get; set; }

        #region 增加移除
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="name"></param>
        public void Remove(GnssBaseLineName name)
        {
          //  this.ObsFileManager.Remove(name);
            //移除基线 
            BaseLineManager.Remove(name);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="name"></param>
        public void Remove(SiteObsBaseline name)
        {
          //  this.ObsFileManager.Remove(name);
            //移除基线 
            BaseLineManager.Remove(name);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="site"></param>
        public void Remove(ObsSiteInfo site)
        {
            this.SiteManager.Remove(site);
            //移除基线 
            BaseLineManager.Remove(site);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="timePeiod"></param>
        public void Remove(TimePeriod timePeiod)
        {
            this.SiteManager.Remove(timePeiod); 
            //移除基线 
            BaseLineManager.Remove(timePeiod);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="siteName"></param>
        public void Remove(string siteName)
        {
            foreach (var item in this.SiteManager)
            {
                item.Remove(siteName);
            } 
            //移除基线 
            BaseLineManager.Remove(siteName);
        }

        /// <summary>
        /// 添加，返回成功的对象
        /// </summary>
        /// <param name="filePathes"></param>
        /// <param name="newAddedBaseLines"></param>
        /// <returns></returns>
        public List<ObsSiteInfo> Add(IEnumerable<string> filePathes, out List<SiteObsBaseline> newAddedBaseLines)
        {
            var oks = new List<ObsSiteInfo>();
            List<SiteObsBaseline> newBaseLines = new List<SiteObsBaseline>();
            foreach (string path in filePathes)
            {
                var info = new ObsSiteInfo(path);
                List<SiteObsBaseline> newBaseLine = null;
                if (AddAndBuildLines(info, out newBaseLine))
                {
                    oks.Add(info);

                    if (newBaseLine.Count == 0) { continue; }
                    info.NetPeriod = newBaseLine[0].NetPeriod;

                    newBaseLines.AddRange(newBaseLine);
                }
            }
            newAddedBaseLines = newBaseLines;

            return oks;
        }

        /// <summary>
        /// 添加，返回成功的对象
        /// </summary>
        /// <param name="filePathes"></param> 
        /// <returns></returns>
        public List<ObsSiteInfo> Add(IEnumerable<string> filePathes)
        {
            var oks = new List<ObsSiteInfo>();
            List<SiteObsBaseline> newBaseLines = new List<SiteObsBaseline>();
            foreach (string path in filePathes)
            {
                var site = new ObsSiteInfo(path);
                var result = this.SiteManager.Add(site);
                if (result)
                {
                    oks.Add(site);
                }
            }
            return oks;
        }

        /// <summary>
        /// 添加测站
        /// </summary>
        /// <param name="site"></param>
        /// <param name="newBaseLines"></param>
        /// <returns></returns>
        public bool AddAndBuildLines(ObsSiteInfo site, out List<SiteObsBaseline> newBaseLines)
        {
            var lines = new List<SiteObsBaseline>();
            var result = this.SiteManager.Add(site);
            if (result) //添加对应的基线
            {
                //组成同步基线
                var periods = this.SiteManager.GetTimePeriodKeys(site.TimePeriod);
                foreach (var periodKey in periods)
                {
                    var siteManager = this.SiteManager[periodKey];
                    var newLines = siteManager.GenerateBaseLines(site);
                    var oks = BaseLineManager.Add(newLines);
                    lines.AddRange(oks);
                }
            }
            newBaseLines = lines;
            return result;
        }
        /// <summary>
        /// 重新构建基线
        /// </summary>
        /// <param name="BaseLineSelectionType">基线选择类型</param>
        /// <param name="centerSiteName">中心站名称</param>
        /// <param name="outerLineFilePath">外部基线文件路径</param>
        /// <returns></returns>
        public List<SiteObsBaseline> RebuildBaseLineManager(BaseLineSelectionType BaseLineSelectionType, string centerSiteName, string outerLineFilePath)
        {
            List<SiteObsBaseline> result = new List<SiteObsBaseline>();
            var dic =  this.GenerateObsBaseLines(BaseLineSelectionType, centerSiteName,   outerLineFilePath);

            this.BaseLineManager.Clear();
            this.BaseLineManager.Add(dic);

            foreach (var item in dic)
            {
                result.AddRange(item.Value);
            }
            return result;
        }

        /// <summary>
        /// 构建基线
        /// </summary>
        /// <param name="BaseLineSelectionType">基线选择类型</param>
        /// <param name="centerSiteName">中心站名称</param>
        /// <param name="outerLineFilePath">外部基线文件路径</param>
        /// <returns></returns>
        public Dictionary<TimePeriod, List<SiteObsBaseline>> GenerateObsBaseLines(BaseLineSelectionType BaseLineSelectionType, string centerSiteName, string outerLineFilePath)
        {
            var result = new Dictionary<TimePeriod, List<SiteObsBaseline>>();
            foreach (var kv in this.SiteManager.KeyValues)
            {
                result[kv.Key] = kv.Value.GenerateObsBaseLines(BaseLineSelectionType, centerSiteName, outerLineFilePath);
            } 
            return result;
        }


        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.SiteManager.Clear();
            this.BaseLineManager.Clear();
        }
        #endregion


        #region 异步环闭合差
        /// <summary>
        /// 生成所有可能的三角形，然后根据不同时段提取网络，计算闭合差 
        /// </summary>
        /// <returns></returns>
        public AsyncTriguilarNetQualitiyManager BuildAsyncTriangularClosureQualies(GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy, int differPeriodCount)
        {
            //获取所有基线
             var lineNames = this.BaseLineManager.GetLineNamesWithEstValue();
            //由所有基线生成可能的三角网组合 
            var  triNetNames = TriangularNetName.BuildTriangularNetNames(lineNames, SiteManager.SiteNames);


            var differs = TimePeriod.GetDifferPeriods(this.BaseLineManager.Keys, differPeriodCount);

            //保存结果
            var CurrentQualityManager = new AsyncTriguilarNetQualitiyManager();
            foreach (var triNetName in triNetNames)
            {
                List<BaseLineNet> subNets = this.GetAsyncTriguilarNet(triNetName, differs);
                if (subNets == null || subNets.Count == 0) { continue; }
                List<QualityOfTriAngleClosureError> list = new List<QualityOfTriAngleClosureError>();
                foreach (var subNet in subNets)
                {
                    var qulity = new QualityOfTriAngleClosureError(subNet, GnssReveiverNominalAccuracy);
                    list.Add(qulity);
                }
                CurrentQualityManager.Add(triNetName, list);
            }
            return CurrentQualityManager;
        }
        /// <summary>
        /// 异步环闭合差
        /// </summary>
        /// <param name="triNetName"></param>
        /// <param name="differs">算法量很大，因此用参数表示</param>
        /// <returns></returns>
        public List<BaseLineNet> GetAsyncTriguilarNet(TriangularNetName triNetName, List<List<TimePeriod>> differs)
        {
            List<BaseLineNet> result = new List<BaseLineNet>();
            foreach (var periods in differs)
            {
                var nets = GetAsyncTriguilarNet(triNetName, periods);
                result.AddRange(nets);
            }
            return result;
        }

        /// <summary>
        /// 获取所有可能的异步环网络
        /// </summary>
        /// <param name="timePeriods">不超过3时段</param>
        /// <param name="triNetName"></param>
        /// <returns></returns>
        public List<BaseLineNet> GetAsyncTriguilarNet(TriangularNetName triNetName, List<TimePeriod> timePeriods)
        {
            List<BaseLineNet> result = new List<BaseLineNet>();
            if (timePeriods.Count <= 1) { return result; }

            if (timePeriods.Count == 2)
            {
                var lineA = triNetName.LineA;
                var lineB = triNetName.LineB;
                var lineC = triNetName.LineC;
                var period1 = timePeriods[0];
                var period2 = timePeriods[1];
                var period1Lines = this.BaseLineManager.Get(period1);
                var period2Lines = this.BaseLineManager.Get(period2);
                //6种组合 
                var p1LineA = period1Lines.GetOrReversed(lineA);
                var p2LineA = period2Lines.GetOrReversed(lineA);
                var p1LineB = period1Lines.GetOrReversed(lineB);
                var p2LineB = period2Lines.GetOrReversed(lineB);
                var p1LineC = period1Lines.GetOrReversed(lineC);
                var p2LineC = period2Lines.GetOrReversed(lineC);

                BaseLineNet net1 = new BaseLineNet(new List<SiteObsBaseline>() { p1LineA, p1LineB, p2LineC });
                BaseLineNet net2 = new BaseLineNet(new List<SiteObsBaseline>() { p1LineA, p2LineB, p2LineC });
                BaseLineNet net3 = new BaseLineNet(new List<SiteObsBaseline>() { p1LineA, p2LineB, p1LineC });
                BaseLineNet net4 = new BaseLineNet(new List<SiteObsBaseline>() { p2LineA, p1LineB, p1LineC });
                BaseLineNet net5 = new BaseLineNet(new List<SiteObsBaseline>() { p2LineA, p2LineB, p1LineC });
                BaseLineNet net6 = new BaseLineNet(new List<SiteObsBaseline>() { p2LineA, p1LineB, p2LineC });

                var all = new List<BaseLineNet> { net1, net2, net3, net4, net5, net6 };
                foreach (var item in all)
                {
                    if (item.Count < 3) { continue; }
                    result.Add(item);
                }
                return result;
            }
            else
            {
                //3个时段
                var lineA = triNetName.LineA;
                var lineB = triNetName.LineB;
                var lineC = triNetName.LineC;
                var period1 = timePeriods[0];
                var period2 = timePeriods[1];
                var period3 = timePeriods[2];
                var period1Lines = this.BaseLineManager.Get(period1);
                var period2Lines = this.BaseLineManager.Get(period2);
                var period3Lines = this.BaseLineManager.Get(period3);

                var p1LineA = period1Lines.GetOrReversed(lineA);
                var p2LineA = period2Lines.GetOrReversed(lineA);
                var p3LineA = period3Lines.GetOrReversed(lineA);
                var p1LineB = period1Lines.GetOrReversed(lineB);
                var p2LineB = period2Lines.GetOrReversed(lineB);
                var p3LineB = period3Lines.GetOrReversed(lineB);
                var p1LineC = period1Lines.GetOrReversed(lineC);
                var p2LineC = period2Lines.GetOrReversed(lineC);
                var p3LineC = period3Lines.GetOrReversed(lineC);

                //6种组合 
                BaseLineNet net1 = new BaseLineNet(new List<SiteObsBaseline>() { p1LineA, p2LineB, p3LineC });
                BaseLineNet net2 = new BaseLineNet(new List<SiteObsBaseline>() { p1LineA, p3LineB, p2LineC });
                BaseLineNet net3 = new BaseLineNet(new List<SiteObsBaseline>() { p2LineA, p1LineB, p3LineC });
                BaseLineNet net4 = new BaseLineNet(new List<SiteObsBaseline>() { p2LineA, p3LineB, p1LineC });
                BaseLineNet net5 = new BaseLineNet(new List<SiteObsBaseline>() { p3LineA, p2LineB, p1LineC });
                BaseLineNet net6 = new BaseLineNet(new List<SiteObsBaseline>() { p3LineA, p1LineB, p2LineC });

                var all = new List<BaseLineNet> { net1, net2, net3, net4, net5, net6 };
                foreach (var item in all)
                {
                    if (item.Count < 3) { continue; }
                    result.Add(item);
                }
                return result;
            }
        }
        #endregion

    }
}
