//2014.09.20, czs, create, 依据不同的频率进行距离改正
//2018.08.02, czs, edit in hmx, 增加空改正数的判断

using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Geo.Correction;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Algorithm;

namespace Gnsser.Correction
{

    /// <summary>
    /// 按照频率分类的距离改正。
    /// 卫星改正的责任链,是一组改正对象的组合。一般采用此类将各种改正进行组合。
    /// </summary>
    public class FreqBasedRangeCorrectionReviser : GnssCorrectorChain<Dictionary<RinexSatFrequency, double>, EpochSatellite>, IEpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FreqBasedRangeCorrectionReviser()
        {
            this.Name = "频率距离改正集合";
            this.CorrectionType = Gnsser.Correction.CorrectionType.FrequenceBasedRangChain;
        }
        /// <summary>
        /// 执行信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public Geo.IWindowData<EpochInformation> Buffers { get; set; }
        /// <summary>
        /// 改正类型
        /// </summary>
        public CorrectionType CorrectionType { get; protected set; }

        /// <summary>
        /// 改正
        /// </summary>
        /// <param name="TInput"></param>
        public override void Correct(EpochSatellite TInput)
        {
            this.Corrections = new Dictionary<string, Dictionary<RinexSatFrequency, double>>();
            //执行改正
            foreach (var item in this.Correctors)
            {
                item.Correct(TInput);
                this.Corrections.Add(item.Name, item.Correction);
            }

            var vals = new Dictionary<RinexSatFrequency, double>();
            foreach (var item in Correctors)
            {
                if(item == null || item.Correction == null) { continue; }

                foreach (var subitem in item.Correction)
                {
                    if (!vals.ContainsKey(subitem.Key)) { vals[subitem.Key] = 0.0; }
                     
                    vals[subitem.Key]  +=   subitem.Value;
                }
            }
            //可在此设断点查看各个改正情况。
            this.Correction = (vals);
        }

        /// <summary>
        /// 矫正
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Revise(ref EpochInformation info)
        { 
            foreach (var sat in info)
            {
                if (!sat.Enabled || !sat.HasEphemeris) continue;

                this.Correct(sat);

                var satFreqs = sat.RinexSatFrequences;
                //分别执行改正
                foreach (var frequencyCorretor in this.Correction)
                {
                    if (satFreqs.Contains(frequencyCorretor.Key))//指定类型的卫星类型和频率 如 GPS L1， GPS L2
                    {
                        //只改正指定观测频率的等效距离
                        double rangeCorretion = frequencyCorretor.Value;
                        if (rangeCorretion == 0) continue;

                        var freqType = ObsCodeConvert.GetFrequenceType(frequencyCorretor.Key);
                        FreqenceObservation freObs = sat[freqType];
                        if (freObs.CommonCorrection.ContainsCorrection(Name))
                        {
                            continue;
                        }
                        freObs.AddCommonCorrection(this.Name, rangeCorretion);
                    }
                }
            }
            return true;
        }
    }
}
