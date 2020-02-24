//2015.12.09, czs, create in 成都地铁2号线往犀浦, IClockService,多系统钟差服务

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm;
using Gnsser.Service;
using Geo.Times;
using Geo;
using Gnsser.Data;

namespace Gnsser.Service
{ 
    /// <summary>
    /// 多系统钟差服务
    /// </summary>
    public class MultiSysClockDailyService :  SatBasedMultiSysDailyService<IClockService, AtomicClock>, IClockService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiSysClockDailyService() { }

        public MultiSysClockDailyService(Dictionary<SatelliteType, TimeIntervalService<IClockService>> dic)
            : base(dic)
        {
        }

        /// <summary>
        /// 简单判断是否可用
        /// </summary>
        /// <param name="satelliteType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsAvailable(SatelliteNumber prn, Time time)
        {
            if (!Contains(prn.SatelliteType)) { return false; }

            var two = this[prn.SatelliteType];
            return two.IsAvailable(time);
        }

        public override IClockService GetService()
        {
            return this;
        }


        #region  clockservice
        public AtomicClock Get(string nameOrPrn, Time time)
        {
            // if (!satData.ContainsKey(satelliteType.SatelliteType)) { return false; }
            throw new NotImplementedException();
        }      

        /// <summary>
        /// 时段
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; }
        #endregion



        public List<AtomicClock> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            throw new NotImplementedException();
        }

    }

}