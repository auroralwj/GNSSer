//2014.06.24， czs, create, for RINEX 3.0
//2015.05.10, czs, edit in namu, 分离数据与服务
//2017.07.23, czs, edit in hongqing, 去掉了非GPS、BDS等的屏蔽

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Times;


namespace Gnsser.Data.Rinex
{ 

    /// <summary>
    /// 单导航文件星历服务。导航文件预先加载进来。
        /// </summary>
    public class SingleParamNavFileEphService : FileEphemerisService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public SingleParamNavFileEphService(string path)
            : this( new ParamNavFileReader(path).ReadGnssNavFlie())
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="NavFile"></param>
        public SingleParamNavFileEphService(ParamNavFile NavFile)
        {
            this.NavFile = NavFile;
            if (NavFile.Header != null)
            {
                this.Name = NavFile.Header.Name;
            }
        }

        /// <summary>
        /// 导航星历文件
        /// </summary>
        public  ParamNavFile NavFile { get; set; }


        #region IEphemerisFile实现 


        /// <summary>
        /// 该星历采用的坐标系统,如 IGS08， ITR97
        /// </summary>
        public override string CoordinateSystem { get {  return Data.Rinex.Sp3File.UNDEF; } }

        /// <summary>
        /// 时间段
        /// </summary>
        public override BufferedTimePeriod TimePeriod
        {
            get { return new BufferedTimePeriod(this.NavFile.StartTime, NavFile.EndTime, 48 * 60 * 60); }//48个小时外推
        }
        /// <summary>
        /// 星历服务类型
        /// </summary>
        public override EphemerisServiceType ServiceType { get { return EphemerisServiceType.Navigation; } }
        /// <summary>
        /// 卫星数量
        /// </summary>
        public override int SatCount { get { return Prns.Count; } }
        /// <summary>
        /// 卫星编号
        /// </summary>
        public override List<SatelliteNumber> Prns
        {
            get
            {
                return NavFile.Prns;
            }
        }

        /// <summary>
        /// 返回指定时间段，文件记录的星历信息。
        /// 需要计算，计算太多是否浪费资源？
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public override List<Gnsser.Ephemeris> Gets(SatelliteNumber prn, Time from, Time to)
        {
            List<EphemerisParam> list = NavFile.GetEphemerisParams(prn, from, to);

            List<Gnsser.Ephemeris> infos = new List<Gnsser.Ephemeris>();
            foreach (var item in list)
            {
                //if (key.Prn.SatelliteType == SatelliteType.S || key.Prn.SatelliteType == SatelliteType.R || key.Prn.SatelliteType == SatelliteType.E)
                //    infos.Add(new Ephemeris()
                //    {
                //        Time = key.Time,
                //        XYZ = new XYZ(),
                //        Prn = key.Prn,
                //        ClockBias = key.ClockBias,
                //        ClockDrift = key.ClockDrift,
                //        //XyzDot = text.
                //    });
                //else
                //{
                    XYZ xyz = SatOrbitCaculator.GetSatPos(item, item.Time);
                    infos.Add(new Gnsser.Ephemeris()
                    {
                        Time = item.Time,
                        XYZ = xyz,
                        Prn = item.Prn,
                        ClockBias = item.ClockBias,
                        ClockDrift = item.ClockDrift,
                        //XyzDot = text.
                    });
                //}

            }
            return infos;
        }


        /// <summary>
        /// 获取所有星历信息。
        /// </summary>
        /// <returns></returns>
        public override List<Gnsser.Ephemeris> Gets()
        {
            List<Gnsser.Ephemeris> infos = new List<Gnsser.Ephemeris>();
            foreach (var list in NavFile)
            {
                foreach (var item in list)
                {
                    XYZ xyz = SatOrbitCaculator.GetSatPos(item, item.Time);
                    infos.Add(new Gnsser.Ephemeris()
                    {
                        Time = item.Time,
                        XYZ = xyz,
                        Prn = item.Prn,
                        ClockBias = item.ClockBias,
                        ClockDrift = item.ClockDrift,
                        //XyzDot = text.
                    });

                }
            }
            return infos;
        }

        /// <summary>
        /// 获取卫星位置
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public override Ephemeris Get(SatelliteNumber prn, Time gpsTime)
        {
            if (!this.Prns.Contains(prn)) return null;// throw new Exception("星历数据源中没有包含指定的卫星：" + prn);

            EphemerisParam nearstNavRecord = GetNearstNavRecord(NavFile.GetEphemerisParams(prn), gpsTime, TimeSpan.FromDays(200));
            return nearstNavRecord.GetEphemerisInfo(gpsTime);
        }
       
        /// <summary>
        /// 指定时刻卫星是否健康可用。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public override bool IsAvailable(SatelliteNumber prn, Time gpsTime)
        {
            if (!this.Prns.Contains(prn)) throw new Exception("星历数据源中没有包含指定的卫星：" + prn);

            EphemerisParam nearstNavRecords = GetNearstNavRecord(NavFile.GetEphemerisParams(prn), gpsTime, TimeSpan.FromDays(200));
            return nearstNavRecords.SVHealth == 0;

        }

        /// <summary>
        /// 选择最接近的卫星星历。
        /// </summary>
        /// <returns></returns>
        public static EphemerisParam GetNearstNavRecord(List<EphemerisParam> records, Time gpsTime, TimeSpan TimeLimen)
        {
            EphemerisParam first = records[0];
            EphemerisParam last = records[records.Count - 1];

            List<EphemerisParam> results = new List<EphemerisParam>();
            //小于时段
            if (gpsTime.SecondsOfWeek <= first.Time.SecondsOfWeek)
            {
                if (gpsTime.SecondsOfWeek < first.Time.SecondsOfWeek - TimeLimen.TotalSeconds) //过小
                    throw new Exception("指定的时间已经超出星历时段允许的阈值！" + first.Time);
                else
                {
                    return (first);
                }
            }
            //大于时段。
            if (gpsTime.SecondsOfWeek >= last.Time.SecondsOfWeek)
            {
                if (gpsTime.SecondsOfWeek > last.Time.SecondsOfWeek + TimeLimen.TotalSeconds) //过大
                    throw new Exception("指定的时间已经超出星历时段允许的阈值！" + last.Time);
                else
                {
                    return last;
                }
            }
            //介于时段
            EphemerisParam prev = null;
            foreach (EphemerisParam current in records)
            {
                if (current.Time.SecondsOfWeek == gpsTime.SecondsOfWeek)
                {
                    return current;
                }
                if (prev != null && IsBetween(gpsTime.SecondsOfWeek, prev.Time.SecondsOfWeek, current.Time.SecondsOfWeek))
                {
                    double disA = Math.Abs(gpsTime.SecondsOfWeek - current.Time.SecondsOfWeek);
                    double disB = Math.Abs(gpsTime.SecondsOfWeek - prev.Time.SecondsOfWeek);
                    if (disA < disB) return current;
                    return prev;
                }
                prev = current;
            }
            throw new Exception("你不可能看到我的！");
        } 

        private static bool IsBetween(double val, double boundaryA, double boundaryB)
        {
            return (val > boundaryA && val < boundaryB) || (val > boundaryB && val < boundaryA);
        }

        #endregion

    }
}
