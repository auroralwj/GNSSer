//2014.09.20, czs, create, 依据不同的频率进行距离改正

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using System.Collections.Generic;
using System.Collections;
using Gnsser.Domain;
using Geo.Correction;

namespace Gnsser.Correction
{

    /// <summary>
    /// 基于频率的测站 NEU 改正。
    /// 卫星改正的责任链,是一组改正对象的组合。一般采用此类将各种改正进行组合。
    /// </summary>
    public class FreqBasedNeuCorrectionReviser : GnssCorrectorChain<Dictionary<RinexSatFrequency, NEU>, EpochInformation> , IEpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FreqBasedNeuCorrectionReviser()
        {
            this.Name = "频率NEU改正集合";
            this.CorrectionType = Gnsser.Correction.CorrectionType.FrequenceBasedNeuChain;
        }
        /// <summary>
        /// 改正类型
        /// </summary>
        public CorrectionType CorrectionType { get; protected set; }
        /// <summary>
        /// 执行信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public Geo.IWindowData<EpochInformation> Buffers { get; set; }
 
        /// <summary>
        /// 执行改正
        /// </summary>
        /// <param name="info"></param>
        public override void Correct(EpochInformation info)
        {
            this.Corrections = new Dictionary<string, Dictionary<RinexSatFrequency, NEU>>();
            //执行改正
            foreach (var item in this.Correctors)
            {
                item.Correct(info);

                Corrections.Add(item.Name, item.Correction);
            }

            Dictionary<RinexSatFrequency, NEU> vals = new Dictionary<RinexSatFrequency, NEU>();
            foreach (var item in Correctors)
            {
                if(item == null  || item.Correction ==null) { continue; }

                foreach (var subitem in item.Correction)
                {
                    if (!vals.ContainsKey(subitem.Key)) vals[subitem.Key] = NEU.Zero;

                    vals[subitem.Key] += subitem.Value;
                }
            }
            //可在此设断点查看各个改正情况。
            this.Correction = (vals);
        }

        public bool Revise(ref EpochInformation info)
        { 
            this.Correct(info);
            Dictionary<RinexSatFrequency, NEU> frequencyDicCorrection = this.Correction;

            foreach (var sat in info.EnabledSats)//分别对指定卫星指定频率进行改正
            {
                var satFreqs = sat.RinexSatFrequences;
                //分别执行改正
                foreach (var freqCorretion in frequencyDicCorrection)
                {
                    if (satFreqs.Contains(freqCorretion.Key))//指定类型的卫星，该卫星包含指定频率
                    {
                        //改正等效距离
                      //double rangeCorretion = CoordUtil.GetDirectionLength(freqCorretion.Value, info.ApproxXyz, sat.Ephemeris.XYZ);

                        double rangeCorretion = CoordUtil.GetDirectionLength(freqCorretion.Value, sat.Polar);

                        FrequenceType freqType = ObsCodeConvert.GetFrequenceType(freqCorretion.Key);
                        FreqenceObservation freObs = sat[freqType];

                        if (rangeCorretion == 0) continue;
                  //  //    freObs.AddCommonCorrection(this.Name, -freqCorretion.Value.U);//若为正，其让站星观测值变小了，作为近似值改正数，应该减去
                        
                        freObs.AddCommonCorrection(this.Name,   -rangeCorretion);//若为正，其让站星观测值变小了，作为近似值改正数，应该减去
                        //作为公共距离，还是只作为载波的改正？
                    }
                }
            }
            return true;
        }
    }
}
