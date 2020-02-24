using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Text;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Gnsser;
using Geo.Referencing;
using Gnsser.Service;
using Geo.Times;
using Geo.Algorithm;

namespace Gnsser
{
    /// <summary>
    /// 卫星信息计算和统计。
    /// </summary>
    public class SatInfoCaculator
    {
        public FileEphemerisService EphemerisService { get; set; }

        public XYZ StationPos { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int Count { get; set; }

        public double EleAngle { get; set; }

        public double SpanMinutes { get; set; }

        public List<AbstractVector> LonLats { get; set; }

        public SatInfoCaculator(FileEphemerisService navFile, XYZ stationPos)
        {
            this.EphemerisService = navFile;
            this.StationPos = stationPos;
            this.EleAngle = 15;
            this.LonLats = new List<AbstractVector>();
        }

        public void SetPeriod(DateTime from, DateTime to, double spanMinutes)
        {
            this.From = from;
            this.To = to;
            this.SpanMinutes = spanMinutes;
            this.Count = (int)((To - From).TotalMinutes / SpanMinutes);
        }
        /// <summary>
        /// 指定时段内，可以看见的卫星。
        /// </summary>
        /// <returns></returns>
        public List<VisibilityOfSat> GetPeriodSatAppearTimes()
        {
            this.LonLats.Clear();
            List<VisibilityOfSat> sats = new List<VisibilityOfSat>();
            List<StationSatInfo> infos = new List<StationSatInfo>();
            List<SatelliteNumber> prns = new List<SatelliteNumber>();
            //  EphemerisService.EphemerisDataSource.GetEphemerisInfos(
            foreach (SatelliteNumber rec in EphemerisService.Prns)//卫星一颗一颗的算
            {
                if (prns.Contains(rec)) continue;
                prns.Add(rec);

                VisibilityOfSat satA = new VisibilityOfSat() { PRN = rec, VisibleTimes = new List<BufferedTimePeriod>() };
                sats.Add(satA);
                Polar lastP = null;

                DateTime f = DateTime.MinValue;
                for (int i = 0; i < Count; i++)
                {
                    DateTime time = From + TimeSpan.FromMinutes(i * SpanMinutes);
                    Time g = new Time(time);
                    Geo.Coordinates.XYZ xyz = EphemerisService.Get(rec, g).XYZ;
                    GeoCoord coord = CoordTransformer.XyzToGeoCoord(xyz);
                    Polar p = CoordTransformer.XyzToGeoPolar(xyz, StationPos);

                    if (lastP == null) { lastP = p; continue; }

                    if (p.Elevation >= EleAngle)
                    {
                        if (f.Equals(DateTime.MinValue) || lastP.Elevation < EleAngle)
                        {
                            f = time;
                            lastP = p;
                            continue;
                        }
                    }
                    if (p.Elevation < EleAngle || i == Count - 1) //当前小于指定高度角
                    {
                        if (!f.Equals(DateTime.MinValue) && lastP.Elevation > EleAngle)//且上一个时刻是大于的，则终端出现了。
                        {
                            BufferedTimePeriod s = new BufferedTimePeriod(f, time);
                            satA.VisibleTimes.Add(s);
                            this.LonLats.Add(new Vector(coord.Lon, coord.Lat) { Tag = rec.PRN.ToString() });
                        }
                    }
                    lastP = p;
                }
            }
            sats.Sort();

            return sats;
        }
        /// <summary>
        /// 所有卫星，在时间段中的相对位置信息。
        /// </summary> 
        /// <returns></returns>
        public Dictionary<SatelliteNumber, List<StationSatInfo>> GetPeriodAllSatInfos()
        {
            this.LonLats.Clear();
            Dictionary<SatelliteNumber, List<StationSatInfo>> dics = new Dictionary<SatelliteNumber, List<StationSatInfo>>();

            foreach (SatelliteNumber rec in EphemerisService.Prns)
            {
                if (dics.ContainsKey(rec)) continue;
                List<StationSatInfo> satList = new List<StationSatInfo>();
                dics.Add(rec, satList);

                for (int i = 0; i < Count; i++)
                {
                    DateTime time = From + TimeSpan.FromMinutes(i * SpanMinutes);
                    StationSatInfo info = GetInstantSatInfo(EphemerisService, rec, new Time(time));
                    if (info == null) continue;
                    satList.Add(info);
                }
            }
            return dics;
        }
        /// <summary>
        /// 时段单星
        /// </summary>
        /// <param name="stationPos"></param>
        /// <param name="navFilePath"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="spanMinutes"></param>
        /// <returns></returns>
        public List<StationSatInfo> GetPeriodSatInfos(SatelliteNumber rec)
        {
            this.LonLats.Clear();
            List<StationSatInfo> satList = new List<StationSatInfo>();
            for (int i = 0; i < Count; i++)
            {
                DateTime time = From + TimeSpan.FromMinutes(i * SpanMinutes);

                StationSatInfo info = GetInstantSatInfo(EphemerisService, rec, new Time(time));
                if (info == null) continue;
                satList.Add(info);
            }
            return satList;
        }


        public List<StationSatInfo> GetAllInstantSatInfos(DateTime time)
        {
            return GetAllInstantSatInfos(new Time(time));
        }

        /// <summary>
        /// 在指定时刻，所有卫星相对于测站的信息
        /// </summary>
        /// <param name="stationPos"></param>
        /// <param name="navFilePath"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<StationSatInfo> GetAllInstantSatInfos(Time time)
        {
            this.LonLats.Clear();

            List<StationSatInfo> sats = new List<StationSatInfo>();
            List<SatelliteNumber> prns = new List<SatelliteNumber>();
            foreach (SatelliteNumber rec in EphemerisService.Prns)
            {
                if (prns.Contains(rec)) continue;
                prns.Add(rec);

                StationSatInfo info = GetInstantSatInfo(EphemerisService, rec, time);
                if (info == null) continue;
                sats.Add(info);
            }
            return sats;
        }


        /// <summary>
        /// 获取卫星在指定时刻，相对于测站的信息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="prn"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public StationSatInfo GetInstantSatInfo(IEphemerisService service, SatelliteNumber prn, Time time)
        {
            Geo.Coordinates.XYZ satPos = service.Get(prn, time).XYZ;
            GeoCoord coord = CoordTransformer.XyzToGeoCoord(satPos);

            Geo.Coordinates.Polar p = CoordTransformer.XyzToGeoPolar(satPos, StationPos);
            if (p.Elevation < EleAngle) return null;

            this.LonLats.Add(new Vector(coord.Lon, coord.Lat) { Tag = prn.ToString() });

            //显示到表格
            return new StationSatInfo()
            {
                PRN = prn,
                Time = time,
                Azimuth = p.Azimuth,
                ElevatAngle = p.Elevation,
                Distance = p.Range,
                Lat = coord.Lat,
                Lon = coord.Lon,
                Height = coord.Height,
                X = satPos.X,
                Y = satPos.Y,
                Z = satPos.Z
            };
        }

        /// <summary>
        /// 卫星在指定时段内出现的数量。要求星历文件卫星数量必须足够。
        /// </summary>
        /// <returns></returns>
        public Dictionary<DateTime, int> GetPeriodSatAppearCounts()
        {
            this.LonLats.Clear();

            Dictionary<DateTime, int> dics = new Dictionary<DateTime, int>();
            for (int i = 0; i < Count; i++)
            {
                int count = 0;
                DateTime time = From + TimeSpan.FromMinutes(i * SpanMinutes);
                foreach (SatelliteNumber rec in EphemerisService.Prns)
                {
                    StationSatInfo info = GetInstantSatInfo(EphemerisService, rec, new Time(time));
                    if (info == null) continue;
                    count++;
                }
                dics.Add(time, count);
            }
            return dics;
        }

    }

    /// <summary>
    /// 卫星在一段时间内的可视性。
    /// </summary>
    public class VisibilityOfSat : IComparable<VisibilityOfSat>
    {
        /// <summary>
        /// PRN
        /// </summary>
        public SatelliteNumber PRN { get; set; }
        /// <summary>
        /// 可见性时段
        /// </summary>
        public List<BufferedTimePeriod> VisibleTimes { get; set; }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(VisibilityOfSat other)
        {
            return this.PRN.CompareTo(other.PRN);
        }
    }
    /// <summary>
    /// 卫星相对于测站的信息
    /// </summary>
    public class StationSatInfo
    {
        /// <summary>
        /// PRN
        /// </summary>
        public SatelliteNumber PRN { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 距离
        /// </summary>

        public double Distance { get; set; }
        public double Azimuth { get; set; }
        public double ElevatAngle { get; set; }

        public double Lon { get; set; }
        public double Lat { get; set; }
        public double Height { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

}
