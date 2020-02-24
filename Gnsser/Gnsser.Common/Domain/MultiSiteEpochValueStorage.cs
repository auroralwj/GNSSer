//2018.09.13, czs, create in hmx, 测站历元数据存储。

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Gnsser
{
    /// <summary>
    /// 测站历元数据存储
    /// </summary>
    public class MultiSiteEpochValueStorage : BaseConcurrentDictionary<string, MultiSatEpochRmsNumeralStorage>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="name"></param>
        public MultiSiteEpochValueStorage(string name)
        {
            this.Name = name;
            log.MsgProducer = this.GetType(); 
        }
        public override MultiSatEpochRmsNumeralStorage Create(string key)
        {
            return new MultiSatEpochRmsNumeralStorage(key);
        } 

        /// <summary>
        /// 时段
        /// </summary>
        public TimePeriod TimePeriod
        {
            get
            {
                TimePeriod timePeriod = new TimePeriod(this.First.TimePeriod);
                foreach (var item in this)
                {
                    timePeriod = timePeriod.Exppand(item.TimePeriod);
                }
                return timePeriod;
            }
        }

        /// <summary>
        /// 时段小数表。一行为一个测站，所有卫星的平均值的小数部分。
        /// </summary>
        public ObjectTableStorage FractionTable { get; set; }
        /// <summary>
        /// 详细表,完整的信息都包含于此，所有产品皆可由此衍生.
        /// 一行为一个测站，一个时段的值。
        /// </summary>
        public ObjectTableStorage DetailTable { get; set; }

        /// <summary>
        /// 获取数值
        /// </summary>
        /// <param name="site"></param>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public RmsedNumeral  GetValue(string site, SatelliteNumber prn, Time time)
        {
            if (!this.Contains(site)) { return null; }
            return this[site].GetValue(prn, time);            
        }

        /// <summary>
        /// 原始数据差分当前星，如获取PPP浮点数星间单差。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public MultiSiteEpochValueStorage GetRawDiffer(SatelliteNumber basePrn)
        {
            log.Info("开始生成 星间单差浮点数，基准星：" + basePrn);
            MultiSiteEpochValueStorage sitePeriodDifferFloat = new MultiSiteEpochValueStorage("所有站星间单差浮点数模糊度");
            foreach (var siteKv in this.Data)
            {
                string siteName = siteKv.Key;
                var periodVals = siteKv.Value;
                var newSite = periodVals.GetRawDiffer(basePrn);
                sitePeriodDifferFloat.Add(siteName, newSite);
            }
            log.Info("星间单差浮点数生成完毕，基准星：" + basePrn);
            return sitePeriodDifferFloat;
        }


        /// <summary>
        /// 所有站星间单差窄巷浮点数模糊度
        /// </summary>
        /// <param name="intValueOfDifferWL"></param>
        /// <param name="funcToSolvNlAmbi"></param>
        /// <returns></returns>
        public MultiSiteEpochValueStorage GetNarrowLaneFcbs(MultiSitePeriodValueStorage intValueOfDifferWL, Func<RmsedNumeral, RmsedNumeral, RmsedNumeral> funcToSolvNlAmbi)
        {
            log.Info("开始生成 所有站星间单差窄巷浮点数模糊度");//，基准星：" + basePrn);
            MultiSiteEpochValueStorage narrrowFcbs = new MultiSiteEpochValueStorage("所有站星间单差窄巷浮点数模糊度");
            foreach (var siteKv in this.Data)
            {
                string siteName = siteKv.Key;
                var periodVals = siteKv.Value;
                var wideIntVals = intValueOfDifferWL.Get(siteKv.Key);
                if(wideIntVals == null || wideIntVals.Count == 0) { continue; }

                var newSite = periodVals.GetNarrowLaneFcbs(wideIntVals, funcToSolvNlAmbi);
                narrrowFcbs.Add(siteName, newSite);
            }
            log.Info("所有站星间单差窄巷浮点数模糊度生成完毕");//，基准星：" + basePrn);
            return narrrowFcbs;
        }

        /// <summary>
        /// 组合相同卫星数值
        /// </summary>
        /// <returns></returns>
        public EpochSatSiteValueList GetSameSatValues()
        {
            EpochSatSiteValueList result = new EpochSatSiteValueList("组合相同卫星数值");

            foreach (var siteKv in this.Data)
            {
                string siteName = siteKv.Key;
                var periodVals = siteKv.Value;
                if (periodVals == null || periodVals.Count == 0) { continue; }

                foreach (var epochKv in siteKv.Value.KeyValues)
                {
                   var epochResult = result.GetOrCreate(epochKv.Key);
                    foreach (var item in epochKv.Value.KeyValues)
                    {
                        epochResult.GetOrCreate(item.Key).Add(siteName, item.Value);
                    }
                } 
            }
            log.Info("组合相同卫星数值完成");//，基准星：" + basePrn);
            return result;


        } 

        /// <summary>
        /// 获取四舍五入小数部分。
        /// </summary>
        /// <returns></returns>
        public MultiSiteEpochValueStorage GetRoundFraction()
        {
            MultiSiteEpochValueStorage result = new MultiSiteEpochValueStorage("返回正小数部分");
            foreach (var siteKv in this.Data)
            {
                string siteName = siteKv.Key;
                var periodVals = siteKv.Value; 
                if (periodVals == null || periodVals.Count == 0) { continue; }

                var newSite = periodVals.GetRoundFraction();
                result.Add(siteName, newSite);
            }
            log.Info("小数部分提取完成");//，基准星：" + basePrn);
            return result;             
        }

         
    }

}