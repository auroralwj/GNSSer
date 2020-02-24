//2017.02.09, czs, create in hongqing, FCB 宽巷计算器
//2017.03.08, czs, edit in hongqing, MwTableBuilder单独提出，便于后续组建产品
//2018.08.30, czs, edit in hmx,去除卫星高度角文件，以星历服务代替，DCB改正适应所有日期
//2018.09.02, czs, create in hmx, 全球测站MW快速提取。
//2018.09.09, czs, create in hmx, 测站时段数据存储。

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
    /// 多颗卫星，站星时段数值存储器。
    /// 键为卫星编号 PRN，分组为时段 TimePeriod，数据为 RmsedNumeral。
    /// 假定：时段内所有的卫星数值不变！
    /// </summary>
    public class MultiSatPeriodRmsNumeralStorage : PeriodRmsNumeralStorage<SatelliteNumber>// BaseDictionary<SatelliteNumber, BaseDictionary<TimePeriod, RmsedNumeral>> 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiSatPeriodRmsNumeralStorage(string name)
        {
            this.Name = name;
            this.CasheOfRawDiffers = new BaseDictionary<SatelliteNumber, MultiSatPeriodRmsNumeralStorage>();

        }

        /// <summary>
        /// 对本测站，各卫星对应时段作差，固定模糊度。星间单差。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fractions"></param>
        /// <param name="MaxDifferForAmbi"></param>
        /// <param name="MaxRms"></param>
        /// <returns></returns>
        public MultiSatPeriodRmsNumeralStorage GetRoundInt(string name, Dictionary<SatelliteNumber, RmsedNumeral> fractions, double MaxDifferForAmbi, double MaxRms)
        {
            int okCount = 0;
            int badCount = 0;
            var newSite = new MultiSatPeriodRmsNumeralStorage(name);//当前测站
            foreach (var satKv in this.Data)
            {
                var prn = satKv.Key;
                if (!fractions.ContainsKey(prn)) { continue; } //浮点解没有，则跳过

                var fraction = fractions[prn];
                var newSatPeriod = newSite.GetOrCreate(prn);
                foreach (var period in satKv.Value.Data)         //针对每颗卫星的计算
                {
                    if (period.Value.Rms > MaxRms) { badCount++; log.Warn("RMS太大，忽略 " + name + " " + prn + " " + period.Key + ", " + period.Value.Rms); continue; }

                    int intAmbiguity;
                    //问题就在这里
                    var flatAmbi = period.Value.Value - fraction.Value; //模糊度浮点解
                    if (Geo.Utils.DoubleUtil.TryRoundToFixAmbiguiy(flatAmbi, out intAmbiguity, MaxDifferForAmbi))
                    {
                        okCount++;
                        newSatPeriod[period.Key] = new RmsedNumeral(intAmbiguity, period.Value.Rms);//保留RMS用于分析
                       // newSatPeriod[period.Key] = new RmsedNumeral(intAmbiguity, 0.0000001);//模糊度固定后，认为是真值
                    }
                    else
                    {
                        badCount++;
                        log.Warn("偏差太大，模糊度固定失败，" + name + " " + prn + period.Key + ", " + period.Value.Value);
                    }
                }
            }
            var totalCount = (okCount + badCount);
            log.Info(name + ", 匹配时段数： " + totalCount + ", 固定成功时段数： " + okCount + ", 失败时段数： " + badCount + ", 成功率：" + okCount * 1.0 / totalCount);
            return newSite;
        }
        /// <summary>
        /// 存储差分值，一次计算就存储。避免重复计算。
        /// </summary>
        BaseDictionary<SatelliteNumber, MultiSatPeriodRmsNumeralStorage> CasheOfRawDiffers { get; set; }



        /// <summary>
        /// 获取指定历元所有数值与基准的差分值。
        /// 首先进行时段差分，再获取。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <param name="epoch"></param>
        /// <param name="maxRms"></param>
        /// <returns></returns>
        public BaseDictionary<SatelliteNumber, RmsedNumeral> GetRawDiffer(SatelliteNumber basePrn, Time epoch, double maxRms = 0.5)
        {
            var result = new BaseDictionary<SatelliteNumber, RmsedNumeral>(basePrn + "_" +epoch.ToString());
            MultiSatPeriodRmsNumeralStorage differs = GetRawDiffer(basePrn, maxRms);
            return differs.GetEpochValues(epoch);
        }

        static object locker = new object();

        /// <summary>
        /// 直接与基准星对应时段作差，通常是为了消除接收机硬件延迟。
        /// 说明：对应时段作差，可以消除部分接收机硬件延迟变化。
        /// </summary>
        /// <param name="basePrn">基准星</param>
        /// <param name="maxRms">忽略掉误差太大的数据,包括基准数据</param>
        /// <returns>结果RMS不包括基准星影响</returns>
        public MultiSatPeriodRmsNumeralStorage GetRawDiffer(SatelliteNumber basePrn, double maxRms = 0.5)
        {
            if (CasheOfRawDiffers.Contains(basePrn)) { log.Debug("本基准卫星的差分表 RawDiffers 已经计算过一次，直接返回。"); return CasheOfRawDiffers[basePrn]; }
            lock (locker)
            {
                MultiSatPeriodRmsNumeralStorage newSite = new MultiSatPeriodRmsNumeralStorage(this.Name);
                CasheOfRawDiffers[basePrn] = newSite;
                if (!this.Contains(basePrn)) { return newSite; }//如果不包含，基准星，则直接返回

                //基准数据与质量控制
                var baseMw = this[basePrn];
                var basePeriods = new List<TimePeriod>();// =  baseMw.Keys ;
                foreach (var item in baseMw.KeyValues)
                {
                    if(item.Value.Rms <= maxRms)
                    {
                        basePeriods.Add(item.Key);
                    }
                }

                //遍历差分所有对应时段
                foreach (var satKv in this.Data)
                {
                    var prn = satKv.Key;
                    foreach (var satP in satKv.Value.Data)                       //当前卫星时段
                    {
                        if(satP.Value.Rms > maxRms) {  continue;  }
                        foreach (var basePeriod in basePeriods)                  //基准卫星时段遍历比较
                        {
                            if (!basePeriod.IsIntersect(satP.Key)) { continue; } //无交集，则略过
                            var interPeriod = basePeriod.GetIntersect(satP.Key);
                            var differVal = satP.Value - baseMw[basePeriod];     //星间单差计算
                            newSite.GetOrCreate(prn)[new TimePeriod(interPeriod)] = differVal;
                        }
                    }
                }
                return newSite;
            }
        }

        /// <summary>
        /// 直接获取数据，如果没有，返回 null
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns>如果没有，返回 null</returns>
        internal RmsedNumeral GetValue(SatelliteNumber prn, Time time)
        {
            if (!this.Contains(prn)) { return null; }

            var periods = this[prn];

            foreach (var kv in periods.KeyValues)
            {
                if (kv.Key.Contains(time)) { return kv.Value; }
            }
            return null;
        }
    }

}