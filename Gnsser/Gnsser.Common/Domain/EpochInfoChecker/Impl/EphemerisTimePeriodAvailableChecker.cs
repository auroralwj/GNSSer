//2016.05.01, czs, edit in hongqing, 更名为 EphemerisTimePeriodAvailableChecker

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Domain; 
using Gnsser.Excepts;


namespace Gnsser.Checkers
{
    /// <summary>
    /// 观测时间是否有效
    /// </summary>
    public class EphemerisTimePeriodAvailableChecker : EpochInfoChecker, IChecker<MultiSiteEpochInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="EphemerisService">星历服务</param>
        public EphemerisTimePeriodAvailableChecker(IEphemerisService EphemerisService)
        {
            this.Name = "星历检核";
            this.EphemerisService = EphemerisService;
        }
        IEphemerisService EphemerisService;

        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="epochInfo"></param>
        public override bool Check(EpochInformation epochInfo)
        {
            //多系统设计式有待改进 ！！！ 2014.12.17 czs
            foreach (var item in epochInfo.SatelliteTypes)
            {

                if (EphemerisService.TimePeriod.Contains(epochInfo.ReceiverTime) ||
                   (epochInfo.ReceiverTime >= EphemerisService.TimePeriod.BufferedStart && epochInfo.ReceiverTime <= EphemerisService.TimePeriod.BufferedEnd))
                {
                    return true;
                }
                else
                {
                    log.Error("无法获取星历，观测历元(" + epochInfo.ReceiverTime + ")不在星历有效时间内！" + EphemerisService.TimePeriod);
                    return false;
                }
            }
            foreach (var item in epochInfo)
            {
                if (!EphemerisService.Prns.Contains(item.Prn))
                {
                    item.Enabled = false;
                    
                   // noEphemerisPrns.Add(key.Prn);//可删除？？？2014.12.17
                }

            }

            return true;
        }

        public bool Check(MultiSiteEpochInfo t)
        {
            foreach (var item in t)
            {
                if (!Check(item)) { return false; }
            }
            return true;
        }
    }
}
