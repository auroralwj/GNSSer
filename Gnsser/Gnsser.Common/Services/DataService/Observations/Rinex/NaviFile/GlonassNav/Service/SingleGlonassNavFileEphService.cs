//2016.10.17,czs & double, edit in hongqing, 重构为字典模式

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
    /// RINEX GLONASS 导航文件。
    /// 一个类的实例代表一个文件。
    /// 纠结的GLONASS导航文件，保留了GPS导航文件的头部，记录方式又和SP3大致相同，但是通常采样率很低。
    /// </summary>
    public class SingleGlonassNavFileEphService : FileEphemerisService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SingleGlonassNavFileEphService(GlonassNavFile GlonassNavFile) { this.NavFile = GlonassNavFile; }

        /// <summary>
        /// 导航文件
        /// </summary>
        public GlonassNavFile NavFile { get; set; }

        #region IEphemerisFile实现

        /// <summary>
        /// 该星历采用的坐标系统,如 IGS08， ITR97
        /// </summary>
        public override string CoordinateSystem { get {  return Data.Rinex.Sp3File.UNDEF;   } }


        /// <summary>
        /// 星历服务类型
        /// </summary>
        public override EphemerisServiceType ServiceType { get { return EphemerisServiceType.Navigation; } }
        /// <summary>
        /// 文件记录开始时间
        /// </summary>
        /// <remarks>文件记录开始时间.</remarks>
        /// <value>now</value>
        public Time StartTime { get { return NavFile.First[0].Time; } }
        /// <summary>
        /// 文件记录结束时间
        /// </summary>
        /// <remarks>文件记录结束时间.</remarks>
        /// <value>now</value>
        public Time EndTime { get { return NavFile.Last[NavFile.Last.Count - 1].Time; } } 
        /// <summary>
        /// 时间段
        /// </summary>
        public override BufferedTimePeriod TimePeriod
        {
            get { return new BufferedTimePeriod(this.StartTime, EndTime); }
            protected set { }
        } 

        //public override int SatCount { get { return Prns.Count; } }
        public override List<SatelliteNumber> Prns
        {
            get
            {
                if (NavFile.Header.PrnList != null)
                    return NavFile.Header.PrnList;
                else
                {
                    List<SatelliteNumber> prns = new List<SatelliteNumber>();
                    foreach (var item in NavFile.Keys)
                    {
                        if (item != null && !prns.Contains(item)) prns.Add(item);
                    }
                    return prns;
                }
            }
        }


        /// <summary>
        /// 返回指定时间段，文件记录的星历信息。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public override List<Gnsser.Ephemeris> Gets(SatelliteNumber prn, Time from, Time to)
        {
            List<GlonassNavRecord> list = this.NavFile.Get(prn).FindAll(m => m.Time.DateTime >= from.DateTime
                               && m.Time.DateTime <= to.DateTime);
            return GetEphemerisInfos(list);
        }

        /// <summary>
        /// 获取所有星历信息。
        /// </summary>
        /// <returns></returns>
        public override List<Gnsser.Ephemeris> Gets()
        {
            return GetEphemerisInfos(this.NavFile.NavRecords);
        }

        /// <summary>
        /// 转换为通用的 EphemerisInfo 列表。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Gnsser.Ephemeris> GetEphemerisInfos(List<GlonassNavRecord> list)
        {
            List<Gnsser.Ephemeris> infos = new List<Gnsser.Ephemeris>();
            if (list == null) { return infos; }

            foreach (var item in list)
            {
                infos.Add(new Gnsser.Ephemeris()
                {
                    Time = item.Time,
                    XYZ = item.XYZ, 
                    Prn = item.Prn,
                    ClockBias = item.ClockBias,
                    ClockDrift = item.ClockDrift,
                    //XyzDot = text.
                });
            }
            return infos;
        }


        /// <summary>
        /// 返回指定时刻卫星的星历信息。
        /// 拟合生成。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public override Ephemeris Get(SatelliteNumber prn, Time gpsTime)
        {
            List<Gnsser.Ephemeris> entities = GetEphemerisInfos(this.NavFile.Get(prn));
            Gnsser.Ephemeris info = entities.Find(m => m.Time.Equals(gpsTime));
            if (info != null) return info;
            //拟合
            return new EphemerisInterpolator(entities).GetEphemerisInfo(gpsTime);
        }       
        #endregion

        public override bool IsAvailable(SatelliteNumber prn, Time satTime)
        {
            log.Warn("Glonass 的 IsHealth 方法并没有实现！");
            return true;
            throw new NotImplementedException();
        }

    }
}
