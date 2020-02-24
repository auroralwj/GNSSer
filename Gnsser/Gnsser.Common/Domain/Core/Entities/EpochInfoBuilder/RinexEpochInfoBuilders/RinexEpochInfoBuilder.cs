//2015.05.14, czs, refacter in 双辽，RinexEpochInformationBuilder 创建
//2015.10.14, czs, refacter in hongqing，继承自有参数的构造器，参数 RinexEpochObservation 
//2018.09.25，czs, edit in hmx, 重构
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
    /// 标准RINEX对象创建EpochInformation
    /// </summary>
    public class RinexEpochInfoBuilder : AbstractBuilder<EpochInformation, RinexEpochObservation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="header"></param> 
        public RinexEpochInfoBuilder(RinexObsFileHeader header)
        {
            this.SatelliteTypes = header.SatelliteTypes;
            this.RinexVersion = header.Version;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SatelliteTypes"></param>
        public RinexEpochInfoBuilder(List<SatelliteType> SatelliteTypes)
        {
            this.SatelliteTypes = SatelliteTypes;
        }
        bool SkipError = true;
        /// <summary>
        /// RINEX 版本
        /// </summary>
        public double RinexVersion { get; set; }
        RinexEpochObservation obsSection { get; set; }
        List<SatelliteType> SatelliteTypes { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        /// <param name="SatelliteTypes"></param>
        /// <returns></returns>
        public RinexEpochInfoBuilder SetSatelliteTypes(List<SatelliteType> SatelliteTypes) { this.SatelliteTypes = SatelliteTypes; return this; }

        /// <summary>
        /// 创建观测信息对象
        /// </summary>
        /// <returns></returns>
        public override EpochInformation Build(RinexEpochObservation obsSection)
        {
            if (obsSection == null) return null;

            EpochInformation EpochInformation = new Domain.EpochInformation();
            EpochInformation.Name = obsSection.Name;
            EpochInformation.SiteInfo = obsSection.Header.SiteInfo;
            EpochInformation.ObsInfo = obsSection.Header.ObsInfo;
            EpochInformation.EpochState = (EpochState)obsSection.EpochFlag; //历元标志，周跳探测用  

            var time = new CorrectableTime(obsSection.ReceiverTime);
            EpochInformation.Time = time;

            //扩展
            RinexFreqObsBuilder FreqObsBuilder = new RinexFreqObsBuilder();
            var epochSatBuilder = new RinexEpochSatBuilder(FreqObsBuilder);
            //添加原始观测量。
            var prns = obsSection.Prns;
            prns.Sort();
            foreach (var prn in prns)
            {
                //过滤掉不需要处理的卫星类型
                if (!SatelliteTypes.Contains(SatelliteType.M) && !SatelliteTypes.Contains(prn.SatelliteType))
                { continue; }
                EpochSatellite epochSat = null;
                var observtion = obsSection[prn];
                if (observtion != null)
                {
                    epochSatBuilder
                            .SetEpochInfo(EpochInformation)
                            .SetPrn(prn);
                    epochSat = epochSatBuilder.Build(observtion);
                }
                if (epochSat != null)
                {
                    EpochInformation.Add(prn, epochSat);
                }

            }
            return EpochInformation;
        }
    }
}
