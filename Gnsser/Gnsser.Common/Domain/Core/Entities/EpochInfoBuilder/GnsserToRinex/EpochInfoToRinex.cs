//2015.10.16, czs, create in D5181达州到成都东， Gnsser To Rinex
//2018.07.18，czs, edit in HMX, 自动适配观测类型
//2018.09.06，czs, edit in HMX, 增加可选观测值改正输出

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
    /// 由EpochInformation转换为RINEX对象
    /// </summary>
    public class EpochInfoToRinex : AbstractBuilder<RinexEpochObservation, EpochInformation>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rinexVersion"></param>
        /// <param name="ignoreDisabled"></param>
        /// <param name="IsUseCorrectedOfRange"></param>
        public EpochInfoToRinex(double rinexVersion, bool IsUseCorrectedOfRange, bool ignoreDisabled = true)
        {
            this.RinexVersion = rinexVersion;
            this.IgnoreDisabled = ignoreDisabled;
            this.IsUseCorrectedOfRange = IsUseCorrectedOfRange;
        }

        /// <summary>
        /// 标记为未启用的，如数据不全、断断续续等，则不用输出。
        /// </summary>
        public bool IgnoreDisabled { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public double RinexVersion { get; set; }
        /// <summary>
        /// 头部信息
        /// </summary>
        public RinexObsFileHeader Header { get; set; }
        /// <summary>
        /// 是否采用伪距改正数，若有，需改正观测码
        /// </summary>
        public bool IsUseCorrectedOfRange { get; set; }
        /// <summary>
        /// 创建观测信息对象
        /// </summary>
        /// <returns></returns>
        public override RinexEpochObservation Build(EpochInformation epcohInfo)
        {
            RinexEpochObservation epochRinex = new RinexEpochObservation();

            epochRinex.EpochFlag = (int)epcohInfo.EpochState; //历元标志，钟跳探测用  
            epochRinex.ReceiverTime = epcohInfo.ReceiverTime;
            if (Header == null)
            {
                Header = BuildHeader(epcohInfo);
            }

            epochRinex.Header = Header;
            foreach (var item in epcohInfo)
            {
                if (IgnoreDisabled && !item.Enabled)
                {
                    continue;
                }

                var to = new EpochSatToRinex(RinexVersion);
                var obj = to.Build(item);

                epochRinex.Add(item.Prn, obj);
            }

            return epochRinex;
        }

        /// <summary>
        /// 生成头部文件。此处采用一个观测历元生成是有缺陷的！！后续观测可能会增加卫星！czs, 2015.12.25
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <returns></returns>
        public RinexObsFileHeader BuildHeader(EpochInformation epochInfo)
        {
            RinexObsFileHeader header = new RinexObsFileHeader(
                epochInfo.ObsInfo.Clone() as IObsInfo,
                epochInfo.SiteInfo.Clone() as ISiteInfo
                );
            header.ObsCodes = BuildObsCodes(epochInfo);
            header.Comments.Add("obs codes built from the epoch " + epochInfo.ReceiverTime);
            return header;
        }

        /// <summary>
        /// 由Gnsser历元观测类型创建RINEX观测码。由这一个历元的观测数据生成。
        /// </summary>
        /// <param name="epcohInfo"></param> 
        /// <returns></returns>
        private Dictionary<SatelliteType, List<string>> BuildObsCodes(EpochInformation epcohInfo)
        {
            var dic = new Dictionary<SatelliteType, List<string>>();
            foreach (var sat in epcohInfo)
            {
                var satType = sat.Prn.SatelliteType;
                if (!dic.ContainsKey(satType)) { dic[satType] = new List<string>(); }

                var list = dic[satType];
                var codes = sat.GetObservationCodes();

                foreach (var code in codes)
                {
                    var rinexCode = code.GetRinexCode(RinexVersion);
                    if (!list.Contains(rinexCode))
                    {
                        list.Add(rinexCode);
                    }
                }
            }
            return dic;
        }
    }

}
