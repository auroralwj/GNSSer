//2015.12.09, czs, create in 成都2号线地铁到犀浦, 常用钟差服务

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

namespace Gnsser.Data
{
    /// <summary>
    /// 常用钟差服务
    /// </summary>
    public interface IClockService : IClockService<AtomicClock>
    {

    }
    /// <summary>
    /// 常用钟差服务
    /// </summary>
    public interface ISimpleClockService : IClockService<SimpleClockBias>
    {
        List<SatelliteType> SatelliteTypes { get; }
    }

    public class EmptySimpleClockService : ISimpleClockService 
    {
        public  EmptySimpleClockService(BufferedTimePeriod timePeriod)
        {
            this.TimePeriod = timePeriod;
        }
        public List<SatelliteType> SatelliteTypes => new List<SatelliteType>();
        public string Name => "EmptySimpleClockService";

        public BufferedTimePeriod TimePeriod { get; set; }

        public SimpleClockBias Get(SatelliteNumber prn, Time time)
        {
            return null;
        }

        public SimpleClockBias Get(string nameOrPrn, Time time)
        {
            return null;
        }

        public List<SimpleClockBias> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        {
            return null;
        }
    }

    /// <summary>
    /// 常用钟差服务
    /// </summary>
    public interface IClockService<TAtomicClock> : IMultiSatProductService<TAtomicClock>
        where TAtomicClock : ISimpleClockBias
    {

    }
}