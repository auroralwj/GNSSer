//2018.09.13, czs, create in hmx, 历元数据存储。

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
    /// 历元卫星测站数据列表，所有的测站数据集合到卫星这里
    /// </summary>
    public class EpochSatSiteValueList : EpochValueListStorage<SatelliteNumber, string, RmsedNumeral>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public EpochSatSiteValueList(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 周期内加权平均
        /// </summary>
        /// <param name="minDataCount">小于此，则忽略</param>
        /// <param name="maxRmsTimes">最大运行的RMS倍数</param>
        /// <returns></returns>
        public MultiSatEpochRmsNumeralStorage GetAverage(int minDataCount, double maxRmsTimes)
        {
            MultiSatEpochRmsNumeralStorage result = new MultiSatEpochRmsNumeralStorage("平均_" +  this.Name);
            foreach (var epochVals in this.Data)
            {
                var epcohResult = new BaseDictionary<SatelliteNumber, RmsedNumeral>();
                result[epochVals.Key] = epcohResult;
                foreach (var kv in epochVals.Value.Data)
                {
                    if(kv.Value.Count < minDataCount) { continue; }
                    
                    var ave = Geo.Utils.DoubleUtil.GetAverageInOnePeriod(kv.Value.Values.ToList(), 1, maxRmsTimes);
                    epcohResult[kv.Key] = ave;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取表格
        /// </summary>
        /// <param name="outDirectory"></param>
        /// <returns></returns>
        public ObjectTableManager GetTable(string outDirectory, SatelliteNumber basePrn)
        {
            log.Info("开始提取表格 " + this.Name);
            DateTime start = DateTime.Now;
            ObjectTableManager result = new ObjectTableManager(outDirectory);
            foreach (var epochVals in this.Data)
            {
                foreach (var satVals in epochVals.Value.Data)
                {
                    var prn = satVals.Key;
                    if (satVals.Value.Count == 0) { continue; }

                    var talbe = result.GetOrCreate(prn + "-" +basePrn);
                    talbe.NewRow();
                    talbe.AddItem("Epoch", epochVals.Key);
                    talbe.AddItem(satVals.Value);
                }
            }
            var span = DateTime.Now - start;
            log.Info("提取表格完毕，耗时： " + span);
            return result;
        }
         
    }


    /// <summary>
    /// 多颗卫星，站星历元数值存储器。
    /// 键为卫星编号 PRN，分组为历元，数据为 RmsedNumeral。
    /// 假定：历元数据容易变化！
    /// </summary>
    public class MultiSatEpochRmsNumeralStorage : EpochRmsNumeralStorage<SatelliteNumber>// BaseDictionary<SatelliteNumber, BaseDictionary<Time, RmsedNumeral>> 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiSatEpochRmsNumeralStorage(string name)
        {
            this.Name = name;
        }

        static object locker = new object();

        /// <summary>
        /// 直接获取数据，如果没有，返回 null
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns>如果没有，返回 null</returns>
        public RmsedNumeral GetValue(SatelliteNumber prn, Time time)
        {
            if (!this.Contains(time)) { return null; }

            var periods = this[time];

            if (!periods.Contains(prn)) { return null; }

            return periods[prn];
        }

        /// <summary>
        /// 原始数据差分，返回新对象。
        /// </summary>
        /// <param name="basePrn"></param>
        /// <returns></returns>
        public MultiSatEpochRmsNumeralStorage GetRawDiffer(SatelliteNumber basePrn)
        {
            MultiSatEpochRmsNumeralStorage result = new MultiSatEpochRmsNumeralStorage("原始差分");

            foreach (var kv in this.Data)
            {
                if (!kv.Value.Contains(basePrn))
                {
                    continue;
                }
                var baseVals = kv.Value[basePrn];
                foreach (var row in kv.Value.KeyValues)
                {
                    var val = row.Value - baseVals;
                    result.GetOrCreate(kv.Key)[row.Key] = val;
                }
            }
            return result;
        }

        /// <summary>
        /// 计算窄巷模糊度浮点解
        /// </summary>
        /// <param name="wideIntVals"></param>
        /// <param name="funcToSolve"></param>
        /// <returns></returns>
        public MultiSatEpochRmsNumeralStorage GetNarrowLaneFcbs(MultiSatPeriodRmsNumeralStorage wideIntVals, Func<RmsedNumeral, RmsedNumeral, RmsedNumeral> funcToSolve)
        {
            MultiSatEpochRmsNumeralStorage result = new MultiSatEpochRmsNumeralStorage(this.Name + "窄巷模糊度");

            foreach (var kv in this.Data)
            {
                var time = kv.Key;
                var resultRow = result.GetOrCreate(time);
                foreach (var row in kv.Value.KeyValues)
                {
                    var prn = row.Key;
                    var wideInt = wideIntVals.GetValue(prn, time);

                    if (wideInt == null) { continue; }

                    //计算窄巷模糊度
                    var resultVal = funcToSolve(row.Value, wideInt);

                    resultRow[prn] = resultVal;// new RmsedNumeral
                }
            }
            return result;
        }


        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="basePrn">基准卫星，并不做计算，只是赋值</param>
        /// <returns></returns>
        public List<FcbOfUpd> GetFcbProduct(SatelliteNumber basePrn, double maxRms= 0.6)
        {
            List<FcbOfUpd> result = new List<FcbOfUpd>();
            foreach (var epcochRow in this.Data)
            {
                var resultItem = new FcbOfUpd(basePrn, epcochRow.Key, false);
                result.Add(resultItem);

                foreach (var item in epcochRow.Value.KeyValues)
                {
                    if(item.Value.Rms > maxRms) { continue; }

                    resultItem.Add(item.Key, item.Value);
                }
            }
            return result;
        }


        /// <summary>
        /// 返回四舍五入小数部分
        /// </summary>
        /// <returns></returns>
        public MultiSatEpochRmsNumeralStorage GetRoundFraction()
        {
            MultiSatEpochRmsNumeralStorage result = new MultiSatEpochRmsNumeralStorage("正小数_" + this.Name);

            foreach (var kv in this.Data)
            {
                var time = kv.Key;
                var resultRow = result.GetOrCreate(time);
                foreach (var row in kv.Value.KeyValues)
                {
                    var prn = row.Key;
                    resultRow[prn] = Geo.Utils.DoubleUtil.GetRoundFraction(row.Value);// new RmsedNumeral 
                }
            }
            return result;
        }
    }

}