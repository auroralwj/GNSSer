//2015.05.14, czs, create in namu，历元单卫星信息构造器
//2015.10.14, czs, refacter in hongqing，继承自有参数的构造器
//2018.07.18，czs, edit in HMX, 自动适配观测类型
//2018.09.24, czs, edit in hmx, 多系统赋值修改，移除ObsCodeManager

using System;
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

namespace Gnsser.Domain
{

    /// <summary>
    /// 采用RINEX数据模型的 历元单卫星信息构造器
    /// </summary>
    public class RinexEpochSatBuilder : AbstractBuilder<EpochSatellite, RinexSatObsData>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RinexEpochSatBuilder()
        {
            this.FreqObsBuilder = new RinexFreqObsBuilder();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FreqObsBuilder"></param>
        public RinexEpochSatBuilder(RinexFreqObsBuilder FreqObsBuilder)
        {
            this.FreqObsBuilder = FreqObsBuilder;
        }
        /// <summary>
        /// 版本
        /// </summary>
        public double Version { get; set; }
        RinexFreqObsBuilder FreqObsBuilder { get; set; }
        SatelliteNumber prn { get; set; }
        // RinexSatObsData observtion;
        EpochInformation EpochInfo { get; set; }
        public Time Time { get; set; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public RinexEpochSatBuilder SetTime(Time Time) { this.Time = Time; return this; }
        /// <summary>
        /// 卫星编号
        /// </summary>
        /// <param name="prn"></param>
        /// <returns></returns>
        public RinexEpochSatBuilder SetPrn(SatelliteNumber prn) { this.prn = prn; return this; }
        /// <summary>
        /// 设置历元信息
        /// </summary>
        /// <param name="EpochInfo"></param>
        /// <returns></returns>
        public RinexEpochSatBuilder SetEpochInfo(EpochInformation EpochInfo) { this.EpochInfo = EpochInfo; this.Time = EpochInfo.ReceiverTime; return this; }

        /// <summary>
        /// 构建，如果可以，默认支持3个频率
        /// </summary>
        /// <returns></returns>
        public override EpochSatellite Build(RinexSatObsData observtion)
        {
            EpochSatellite epochSat = new EpochSatellite(this.EpochInfo, prn);
            //需要重新修改，RINEX 3.0 有的对 BDS 是 C1，C7, C6,3.02 明确规定是 C2， C7, C6
            Dictionary<FrequenceType, List<int>> freqDic = ObsCodeConvert.GetRinexFreqIndexDic(observtion.Prn.SatelliteType);
            //新算法，2018.09.24，hmx
            var freqNums = observtion.GetFrequenceNums();
            foreach (var num in freqNums)
            {
                var prn = observtion.Prn;
                FrequenceType freqType = ObsCodeConvert.GetFrequenceType(freqDic, num);      
                Frequence band = Frequence.GetFrequence(prn, freqType, Time);
                if (band == null)
                {
                    band = Frequence.Default;
                    log.Warn("系统并未设置 " + prn.SatelliteType + " 的第 " + freqType + " 频率， 以 " + band + " 代替 ！");
                }
           
                FreqenceObservation freqObs1 = FreqObsBuilder
                                    .SetFrequenceType(freqType)
                                    .SetFrequence(band)
                                    .SetRinexFrequenceNumber(num)
                                    .Build(observtion);
                epochSat.Set(freqObs1.FrequenceType, freqObs1);
            }

            //次新算法，比较繁琐，2018
            //List<FrequenceType> builded = new List<FrequenceType>();
            //foreach (var item in observtion)
            //{
            //    var num = Geo.Utils.StringUtil.GetNumber(item.Key);
            //    FrequenceType freqType = ObsCodeConvert.GetFrequenceType(freqDic, num);//   (FrequenceType)(builedFreq.IndexOf(num) + 1);

            //    if (!builded.Contains(freqType)) { builded.Add(freqType); }
            //    else { continue; }

            //    FreqenceObservation freqObs1 = FreqObsBuilder
            //                        .SetFrequenceType(freqType)
            //                        .SetRinexFrequenceNumber(num)
            //                        .Build(observtion);
            //    epochSat.Set(freqObs1.FrequenceType, freqObs1);
            //}

            /** //老算法，只支持3个频率
            #region 转换观测值到频率A、频率B、频率C的基本观测值
            //第一频率 
            FreqenceObservation freqObs = FreqObsBuilder
                .SetFrequenceType(FrequenceType.A).Build(observtion);
            epochSat.Set(freqObs.FrequenceType, freqObs);

            //第二频率 
            freqObs = FreqObsBuilder
                .SetFrequenceType(FrequenceType.B).Build(observtion);
            if (freqObs != null) epochSat.Set(freqObs.FrequenceType, freqObs);

            //第三频率
            freqObs = FreqObsBuilder
                .SetFrequenceType(FrequenceType.C).Build(observtion);
            if (freqObs != null) epochSat.Set(freqObs.FrequenceType, freqObs);
            #endregion
            */

            return epochSat;
        }
    }
}