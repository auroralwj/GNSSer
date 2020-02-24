//2017.07.23,czs, create in hongqing,  混合类型的导航文件

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
    public class MixedNavFileEphService : FileEphemerisService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public MixedNavFileEphService(string path)
            : this(new MixedNavFileReader(path).ReadGnssNavFlie())
        { 
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="NavFile"></param>
        public MixedNavFileEphService(MixedNavFile NavFile)
        {
            this.NavFile = NavFile;
            if (NavFile.Header != null)
            {
                this.Name = NavFile.Header.Name;
            }
            ParamNavFileEphService = new SingleParamNavFileEphService(NavFile.ParamNavFile);
            GlonassNavFileEphService = new SingleGlonassNavFileEphService(NavFile.GlonassNavFile);

        }

        SingleParamNavFileEphService ParamNavFileEphService { get; set; }
        SingleGlonassNavFileEphService GlonassNavFileEphService { get; set; }

        /// <summary>
        /// 导航星历文件
        /// </summary>
        public MixedNavFile NavFile { get; set; }

        /// <summary>
        /// 该星历采用的坐标系统,如 IGS08， ITR97.
        /// 导航文件不需要这么精确
        /// </summary>
        public override string CoordinateSystem { get {   return Data.Rinex.Sp3File.UNDEF;  } }

        #region IEphemerisFile实现 
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
        /// 卫星编号
        /// </summary>
        public override List<SatelliteNumber> Prns   {  get  {   return NavFile.Prns;  }   }

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
            if (EphemerisUtil.IsEphemerisParam(prn.SatelliteType))
            {
                return this.ParamNavFileEphService.Gets(prn, from, to);
            } 
            return this.GlonassNavFileEphService.Gets(prn, from, to);
        }


        /// <summary>
        /// 获取所有星历信息。
        /// </summary>
        /// <returns></returns>
        public override List<Gnsser.Ephemeris> Gets()
        {
            var ephs = this.ParamNavFileEphService.Gets();

            var others = this.GlonassNavFileEphService.Gets();
            ephs.AddRange(others);

            return ephs;
        }

        /// <summary>
        /// 获取卫星位置
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public override Ephemeris Get(SatelliteNumber prn, Time gpsTime)
        {
            if (EphemerisUtil.IsEphemerisParam(prn.SatelliteType))
            {
                return this.ParamNavFileEphService.Get(prn, gpsTime);
            }
            return this.GlonassNavFileEphService.Get(prn, gpsTime);
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

            if (EphemerisUtil.IsEphemerisParam(prn.SatelliteType))
            {
                return this.ParamNavFileEphService.IsAvailable(prn, gpsTime);
            }
            return this.GlonassNavFileEphService.IsAvailable(prn, gpsTime);

        }

        #endregion
    }
}
