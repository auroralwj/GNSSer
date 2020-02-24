//2018.12.26, czs, create in  ryd, 卫星数据掐头去尾

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;
using Gnsser.Domain;
using Geo.Times;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    /// 卫星数据掐头去尾
    /// </summary>
    public class BreakOffBothEndsReviser : EpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BreakOffBothEndsReviser(SiteSatAppearenceService SiteSatAppearenceService, double minuteOfBreakOffBothEnds)
        {
            this.Name = "卫星数据掐头去尾";
            log.Info("启用 " + Name);
            CurrentIndex = -1;
            this.SecondOfBreakOffBothEnds = minuteOfBreakOffBothEnds * 60;
            this.SiteSatAppearenceService = SiteSatAppearenceService;
            if (SiteSatAppearenceService == null)
            {
                log.Warn(Name + "需要时段分析服务！" + typeof(SiteSatAppearenceService));
            }
        }
        /// <summary>
        /// 掐头去尾数量
        /// </summary>
        public double SecondOfBreakOffBothEnds { get; set; }
        /// <summary>
        /// 当前历元编号
        /// </summary>
        public int CurrentIndex { get; set; }


        SiteSatAppearenceService SiteSatAppearenceService { get; set; }
        /// <summary>
        /// 第一个历元
        /// </summary>
        public Time FirstEpoch { get; set; }


        public override bool Revise(ref EpochInformation epochInfo)
        {
            if (SiteSatAppearenceService == null)
            {
                return true;
            }

            CurrentIndex++;
            //首历元忽略
            if (CurrentIndex <= 1) { FirstEpoch = epochInfo.ReceiverTime; return true; }

            var buffer = this.Buffers;
            var time = epochInfo.ReceiverTime;

            var service = SiteSatAppearenceService.Get(epochInfo.Name);
            if(service == null) {
                log.Warn("时段服务器为null" + epochInfo.Name);
                return true;
            }
             
            List<SatelliteNumber> tobeCuts = new List<SatelliteNumber>();
            foreach (var sat in epochInfo)
            {
                var period = service.Get(sat.Prn);
                if (period.Contains(FirstEpoch) || period.Contains(service.LastRegistTime))//包含首尾历元，则不必掐头
                {
                    continue;
                }
                var StartTime = period.Start;

                bool isCut = IsInCutRegion(time, period.Start) || IsInCutRegion(time, period.End);
                if (isCut) { 
                     tobeCuts.Add(sat.Prn);  
                }
            }

            epochInfo.Remove(tobeCuts, true, this.Name);

            return true;
        }

        public bool IsInCutRegion(Time time, Time endTime)
        {
            var passed = Math.Abs(time - endTime);
            var isIn = passed < SecondOfBreakOffBothEnds;
            return isIn;
        }
    }
}