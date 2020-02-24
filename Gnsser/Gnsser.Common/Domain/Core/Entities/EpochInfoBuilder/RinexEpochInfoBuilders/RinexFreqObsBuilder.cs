//2015.05.14, czs, refactor, 频率观测文件构造器
//2015.10.14, czs, refacter in hongqing，继承自有参数的构造器
//2018.09.24, czs, edit in hmx, 多系统赋值修改，移除ObsCodeManager

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Correction;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Correction;
using Geo.IO;

namespace Gnsser.Domain
{

    /// <summary>
    /// 频率观测文件构造器。基于RINEX观测文件，构建GNSSer内部存储器。
    /// </summary>
    public class RinexFreqObsBuilder : AbstractBuilder<FreqenceObservation, RinexSatObsData>
    {
        Log log = new Log(typeof(RinexFreqObsBuilder));
        /// <summary>
        /// 构造函数
        /// </summary>
        public RinexFreqObsBuilder()
        {
        }

        FrequenceType FrequenceType { get; set; }
        RinexSatObsData observtion { get; set; }
        /// <summary>
        /// RINEX 编号，注意：有的并不一致，如北斗 L1 频率有的数字为 2，因此需要指定。
        /// </summary>
        public int RinexFrequenceNumber { get; set; }
        /// <summary>
        /// 频率
        /// </summary>
        public Frequence Frequence { get; set; }
        /// <summary>
        /// 设置频率类型
        /// </summary>
        /// <param name="FrequenceType"></param>
        /// <returns></returns>
        public RinexFreqObsBuilder SetFrequenceType(FrequenceType FrequenceType) { this.FrequenceType = FrequenceType; return this; }
        /// <summary>
        /// RINEX 频率数字，必须设置！
        /// </summary>
        /// <param name="RinexFrequenceNumber"></param>
        /// <returns></returns>
        public RinexFreqObsBuilder SetRinexFrequenceNumber(int RinexFrequenceNumber) { this.RinexFrequenceNumber = RinexFrequenceNumber; return this; }
        /// <summary>
        /// 设置观测数据
        /// </summary>
        /// <param name="SatObsData"></param>
        /// <returns></returns>
        public RinexFreqObsBuilder SetSatObsData(RinexSatObsData SatObsData) { this.observtion = SatObsData; return this; }
        /// <summary>
        /// 设置Frequence
        /// </summary>
        /// <param name="Frequence"></param>
        /// <returns></returns>
        public RinexFreqObsBuilder SetFrequence(Frequence Frequence) { this.Frequence = Frequence; return this; }
        /// <summary>
        /// 创建
        /// </summary> 
        /// <returns></returns>
        public override FreqenceObservation Build(RinexSatObsData observtion)
        {
            if (observtion == null) return null;

            var prn = observtion.Prn; 
             
            var freqObservation = new FreqenceObservation(Frequence, FrequenceType);

            //RINEX观测文件的区别为数字频率编号
            foreach (var kv in observtion)
            {
                var v = kv.Value;
                if (v.ObservationCode.BandOrFrequency != RinexFrequenceNumber) { continue; } //过滤

                var obsType = v.ObservationCode.ObservationType;
                switch (obsType)
                {
                    case ObservationType.L:
                        var PhaseRange = new PhaseRangeObservation(v, Frequence);
                        freqObservation.Set(obsType, PhaseRange);
                        break; 
                    default: 
                        var d = new Observation(v.Value, v.ObservationCode);
                        freqObservation.Set(obsType, d);
                        break; 
                }
            }


            return freqObservation;
        }
    }

}