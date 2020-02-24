//2018.10.16, czs, create in hmx, 参数数值提取
//2018.10.19, czs, edit in hmx， 模糊度增加RMS信息
//2018.11.07, czs, create in hmx, 单独的类提取时段固定参数值

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

namespace Gnsser.Winform
{
    /// <summary>
    /// 提取时段固定参数值
    /// </summary>
    public class PeriodParamExtractor : AbstractBuilder<PeriodRmsedNumeralStoarge>
    {
        Log log = new Log(typeof(PeriodParamExtractor));
        /// <summary>
        /// 默认构造函数提取时段固定参数值
        /// </summary>
        /// <param name="epochParamPath"></param>
        /// <param name="ParamRmsFilePath"></param>
        /// <param name="maxBreakCount"></param>
        /// <param name="DefaultRmsValue"></param>
        public PeriodParamExtractor(string epochParamPath, string ParamRmsFilePath, int maxBreakCount, double DefaultRmsValue = 1e-8)
        {
            this.DefaultRmsValue = DefaultRmsValue;
            this.MaxBreakCount = maxBreakCount;
            if (File.Exists(ParamRmsFilePath))
            {
                EpochParamRmsTable = ObjectTableReader.Read(ParamRmsFilePath);
            }
            else
            {
                log.Warn("没有 RMS 文件， 将采用默认 RMS");
            }
            this.EpochParamTable = ObjectTableReader.Read(epochParamPath);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="EpochParamTable"></param>
        /// <param name="EpochParamRmsTable"></param>
        /// <param name="maxBreakCount"></param>
        /// <param name="DefaultRmsValue"></param>
        public PeriodParamExtractor(ObjectTableStorage EpochParamTable, ObjectTableStorage EpochParamRmsTable, int maxBreakCount, double DefaultRmsValue = 1e-8)
        {
            this.DefaultRmsValue = DefaultRmsValue;
            this.MaxBreakCount = maxBreakCount;
            this.EpochParamRmsTable = EpochParamRmsTable; 
            this.EpochParamTable = EpochParamTable;
        }

        public int MaxBreakCount { get; set; }
        public double DefaultRmsValue { get; set; } 
        public ObjectTableStorage EpochParamTable { get; set; }
        public ObjectTableStorage EpochParamRmsTable { get; set; }
        public PeriodRmsedNumeralStoarge CurrentResult { get; set; }

        public override PeriodRmsedNumeralStoarge Build()
        {
            PeriodRmsedNumeralStoarge result = new PeriodRmsedNumeralStoarge();
            double interval = EpochParamTable.GetInterval<Time>();
            double maxBreakSpan = interval * (MaxBreakCount + 1);
            BaseDictionary<string, TimePeriod> currents = new BaseDictionary<string, TimePeriod>();
            foreach (var row in EpochParamTable.BufferedValues)
            {
                var time = (Time)row["Epoch"];

                foreach (var kv in row)
                {
                    if (!currents.Contains(kv.Key))
                    {
                        currents[kv.Key] = new TimePeriod(time, time);
                    }
                    var currentPeriod = currents[kv.Key];

                    var span = time - currentPeriod.End;
                    if (span > maxBreakSpan) // 断裂，重新赋值
                    {
                        //获取RMS并更新
                        double rmsVal = DefaultRmsValue;
                        if (EpochParamRmsTable != null)
                        {
                            var rms = EpochParamRmsTable.GetValue<double>(currentPeriod.End, kv.Key, DefaultRmsValue);
                            var stdDev = EpochParamRmsTable.GetValue<double>(currentPeriod.End, "StdDev", 1);
                            rmsVal = rms / stdDev;
                        }
                        ((RmsedNumeral)currentPeriod.Object).Rms = rmsVal;
                        result.GetOrCreate(kv.Key)[currentPeriod] = (RmsedNumeral)currentPeriod.Object;

                        //新建
                        currentPeriod = new TimePeriod(time, time);
                        currents[kv.Key] = currentPeriod;
                    }
                    currentPeriod.End = time;
                    currentPeriod.Object = new RmsedNumeral(Geo.Utils.ObjectUtil.GetNumeral(kv.Value), DefaultRmsValue);
                }
            }
            //最后更新一次，查缺补漏
            foreach (var kv in currents.KeyValues)
            { //获取RMS
                double rmsVal = DefaultRmsValue;
                if (EpochParamRmsTable != null)
                {
                    var rms = EpochParamRmsTable.GetValue<double>(kv.Value.End, kv.Key, DefaultRmsValue);
                    var stdDev = EpochParamRmsTable.GetValue<double>(kv.Value.End, "StdDev", 1);
                    rmsVal = rms / stdDev;
                }
                var val = (RmsedNumeral)kv.Value.Object;
                val.Rms = rmsVal; //恢复为协因数阵
                result.GetOrCreate(kv.Key)[kv.Value] = val;
            }
            this.CurrentResult = result;
            return (result);
        } 
         
    }

}