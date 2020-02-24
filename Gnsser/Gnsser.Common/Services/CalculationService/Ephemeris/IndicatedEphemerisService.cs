//2018.04.27, czs, create in hmx, 指定路径的多系统星历服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Geo;
using Geo.IO;
using Geo.Common;
using Gnsser.Service;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    /// 指定路径的多系统星历服务
    /// </summary>
    public class IndicatedEphemerisService : FileEphemerisService
    {
        Log log = new Log(typeof(IndicatedEphemerisService));
        /// <summary>
        /// 构造函数
        /// </summary>
        public IndicatedEphemerisService() { this.Name = "指定路径的多系统星历服务"; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathes"></param>
        public IndicatedEphemerisService(Dictionary<SatelliteType, string> pathes, GnssProcessOption processOption, IgsProductSourceOption option)
        {
            this.Name = "指定路径的多系统星历服务"; 
            data = new BaseDictionary<SatelliteType, FileEphemerisService>();
            foreach (var item in  pathes)
            {
                if (!File.Exists(item.Value)){ log.Error("指定的 " + item.Key + " 星历路径不存在"); continue; }

                data[item.Key] = EphemerisDataSourceFactory.Create(item.Value,  FileEphemerisType.Unkown, true, option.MinSequentialSatCount, option.Sp3EphMaxBreakingCount, processOption.EphemerisInterpolationOrder);
            }
        }

        BaseDictionary<SatelliteType, FileEphemerisService> data;

        #region IEphemerisFile实现 
        /// <summary>
        /// 该星历采用的坐标系统,如 IGS08， ITR97
        /// </summary>
        public override string CoordinateSystem { get { return data.FirstOrDefault().CoordinateSystem; } }

        /// <summary>
        /// 时间段
        /// </summary>
        public override BufferedTimePeriod TimePeriod
        {
            get { return data.First.TimePeriod; }//48个小时外推
        }

        public override List<SatelliteNumber> Prns
        {
            get
            {
                List<SatelliteNumber> list = new List<SatelliteNumber>();
                foreach (var item in data)
                {
                    list.AddRange(item.Prns);
                }
                return list;
            }
        }

        public override Ephemeris Get(SatelliteNumber prn, Time time)
        {
            if (data.Contains(prn.SatelliteType))
            {
                return data[prn.SatelliteType].Get(prn, time);
            }
            return null;
        }

        public override List<Ephemeris> Gets()
        {
            List<Ephemeris> list = new List<Ephemeris>();
            foreach (var item in data)
            {
                list.AddRange(item.Gets());
            } 
            return list;
        }

        public override List<Ephemeris> Gets(SatelliteNumber prn, Time timeStart, Time timeEnd)
        { 
            if (data.Contains(prn.SatelliteType))
            {
                return data[prn.SatelliteType].Gets(prn, timeStart, timeEnd);
            }
            return new List<Ephemeris>();
        }

        public override bool IsAvailable(SatelliteNumber prn, Time satTime)
        {
            if (data.Contains(prn.SatelliteType))
            {
                return data[prn.SatelliteType].IsAvailable(prn, satTime);
            }
            return false;
        }
        #endregion

    }

}