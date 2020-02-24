using System;
using Gnsser.Times;
using System.Collections.Generic;

using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// 一个GPS卫星导航信息记录,
    /// 包含卫星编号，时刻，轨道根数等。具有摄动改正参数。
    /// </summary>
    public class EphemerisParam : KeplerEphemerisParam
    {
        public EphemerisParam() { }
        public EphemerisParam(SatClockBias record)
        {
            this.Time = record.Time;
            this.Prn = record.Prn;
            this.ClockBias = record.ClockBias;
            this.ClockDrift = record.ClockDrift;
            this.DriftRate = record.DriftRate;
        }

        /// <summary>
        /// RTCM 3 参数， //2016.01.20, double, add, 
        /// </summary>
        public double Toc { get; set; }
       
        #region 轨道摄动参数 perturbation parameters

        /// <summary>
        /// 平均角速度n的改正值。 mean motion difference from computed value (r/s)
        /// </summary> 
        public double DeltaN { get; set; }
        /// <summary>
        /// 轨道倾角i的变化率， rate of change of inclination              (r/s)
        /// </summary>
        public double EyeDot { get; set; }
        /// <summary>
        ///升交点赤经Omega的变化率， rate of change of right ascension          (r/s)
        /// </summary>
        public double OmegaDot { get; set; }

        /// <summary>
        /// 升交距角 u = w + f 的余弦的调和改正振幅。 cosine harmonic correction to arg. of lat. (r)
        /// </summary>
        public double Cuc { get; set; }
        /// <summary>
        ///  升交距角 u = w + f 的正弦的调和改正振幅。 csine harmonic correction to arg. of lat.   (r)
        /// </summary>
        public double Cus { get; set; }
        /// <summary>
        /// 卫星到地心距离r的余弦调和改正项。cosine harmonic correction to radius       (m)
        /// </summary>
        public double Crc { get; set; }
        /// <summary>
        /// 卫星到地心距离r的正弦调和改正项。sine harmonic correction to radius         (m)单位m
        /// </summary>
        public double Crs { get; set; }
        /// <summary>
        ///  轨道倾角i的余弦调和改正项。cosine harmonic correction to inclination  (r)
        /// </summary>
        public double Cic { get; set; }
        /// <summary>
        ///  轨道倾角i的正弦调和改正项。sine harmonic correction to inclination    (r)
        /// </summary>
        public double Cis { get; set; }
        #endregion

        #region other satData

        /// <summary>
        /// Issue of Data，Ephemeris/数据、星历发布时间。
        /// </summary>
        public double IODE { get; set; }

        /// <summary>
        /// L2上的码codes on l2 channel (binary, 01-info, 10-coeff/matrix)
        /// </summary>
        public double CodesL2 { get; set; }
        /// <summary>
        /// GPS周，与Toe一同表示时间 gps week # (to go with toe)               (wk)
        /// </summary>
        public int GPSWeeks { get; set; }
        /// <summary>
        /// BeiDou周  
        /// </summary>
        public int BeiDouWeeks { get; set; }
        /// <summary>
        /// Galileo周  
        /// </summary>
        public int GalileoWeeks { get; set; }
        /// <summary>
        /// BDS卫星B1I星上设备时延差
        /// </summary>
        public double Tgd1OfBeidDou { get; set; }
        /// <summary>
        /// BDS卫星B2I星上设备时延差
        /// </summary>
        public double Tgd2OfBeidDou { get; set; }
        /// <summary>
        /// BDS卫星健康状况
        /// </summary>
        public double SatH1 { get; set; }

        /// <summary>
        /// P码的数据标记
        /// l2 info satData flag (binary, 0-norm, 1-set off)
        /// </summary>
        public bool L2PDataFlag { get; set; }

        /// <summary>
        /// 卫星精度（m）URA index (converted)                      (m)
        /// </summary>
        public double SVAccuracy { get; set; }
        /// <summary>
        /// 卫星健康状态 sv health    (6 bits, 0-ok, 1-not)
        /// </summary>
        public double SVHealth { get; set; }
        /// <summary>
        ///  est. group delay   (tsv-tgd)               (s)
        /// </summary>
        public double TGD { get; set; }
        /// <summary>
        /// 钟的数据龄期issue of satData svClock
        /// </summary>
        public double IODC { get; set; }

        /// <summary>
        /// bit1,Resolution:
        /// RTCM 3 参数， //2016.01.20, double, add, 
        /// </summary>
        public bool FitInterval { get; set; }
        /// <summary>
        /// 电文发送时刻（单位为GPS周内的秒，通过交接字（HOW）中的Z计算得出）
        ///  transmission time of message  
        /// </summary>
        public double TTM { get; set; }
        #endregion


        public override string ToString()
        {
            return Prn.ToString() + " " + Time.ToString();
        }
        /// <summary>
        /// 获取卫星钟差。
        /// </summary>
        /// <param name="gpsTime">时间</param>
        /// <returns></returns>
        public double GetClockOffset(Time gpsTime) { return GetClockOffset(this, gpsTime); }

        /// <summary>
        /// 获取卫星钟差
        /// </summary>
        /// <param name="record">星历参数</param>
        /// <param name="gpsTime">时间</param>
        /// <returns></returns>
        public static double GetClockOffset(EphemerisParam record, Time gpsTime)
        {
            double toc = record.Time.SecondsOfWeek;
            double t = gpsTime.SecondsOfWeek;
            double differTime = t - record.Toe;
            //GPS卫星以钟为周秒，考虑到一个星期的开始或结束
            if (differTime > 302400) differTime -= 604800;
            else if (differTime < -302400) differTime += 604800;

            return record.GetOffset(differTime); 
        }
         

        #region 核心计算方法

        private static double GM = 3.986005e14;
        private static double we = 7.2921151467e-5;        //*** rad/sec
        //private static double gpspi = 3.1415926535898e0;
        private static double bigf = -4.442807633e-10;
        /// <summary> 
        /// 计算星历信息。
        /// compute pos of broadcast orbit (BC) for matrix single time
        /// sense of the corrector (with relativity) is:   t=tsv-dt-dtr
        /// BC is in GPS system time (use svClock corr, time in sec.of.week)
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <param name="myModes"></param>
        /// <returns></returns>
        public Ephemeris GetEphemerisInfo(Time gpsTime, bool IsDualFreq = false)
        {
            double dt, dtr, clockOffSet;

            //*** correct satellite time
            dt = SatSvClock_NonRelative(gpsTime.SecondsOfWeek);                       //*** icd-200 20.3.3.3.3.1
            dtr = SatSvClock_Relative(gpsTime.SecondsOfWeek, dt);
            clockOffSet = dt + dtr;

            //*** icd-200 20.3.3.3.3.2  (not dual freq)
            //*** old version below
            //*** if( (!myModes.IsDualFreq()) && (!myModes.IsIon())) svClock = svClock - tgd;

            //*** 2006july28 notes
            //*** DOD does NOT apply sat P1P2 DCB correction (Tgd) to broadcast clocks
            //*** IGS does NOT apply sat P1P2 DCB corrections to their clocks "by agreed convention"
            //*** thus, sat P1P2 DCB must be applied to DoD/IGS clocks if single freq. 
            //*** this is same convention as icd-200 20.3.3.3.3.2
            //*** this has NOTHING to do with use of any kind of normals ionosphere model
            //*** its purpose is undoing the disseminated svClock biases by means of
            //***    uncorrected (by DCB's) pseudorange ionosphere estimates

            if (!IsDualFreq) clockOffSet = clockOffSet - TGD;

            //*** compute broadcast position at corrected time

            Geo.Coordinates.XYZ xyz = GetSatXyz(gpsTime.SecondsOfWeek - clockOffSet, this.Prn);

            var state = OrbitUtil.GetPosXYZ(this, gpsTime.SecondsOfWeek - clockOffSet);
            return new Ephemeris
            {
                XYZ = state.Value,
                ClockBias = clockOffSet,
                RelativeCorrection = dtr,
                Prn = this.Prn,
                Time = gpsTime,
                ClockDrift = ClockDrift,
                XyzDot = state.Rms
            };
        }

        public Ephemeris GetEphemerisInfo(double gpsTime, bool IsDualFreq = false)
        {
            double dt, dtr, clockOffSet;

            //*** correct satellite time
            dt = SatSvClock_NonRelative(gpsTime);                       //*** icd-200 20.3.3.3.3.1
            dtr = SatSvClock_Relative(gpsTime, dt);
            clockOffSet = dt + dtr;

            //*** icd-200 20.3.3.3.3.2  (not dual freq)
            //*** old version below
            //*** if( (!myModes.IsDualFreq()) && (!myModes.IsIon())) svClock = svClock - tgd;

            //*** 2006july28 notes
            //*** DOD does NOT apply sat P1P2 DCB correction (Tgd) to broadcast clocks
            //*** IGS does NOT apply sat P1P2 DCB corrections to their clocks "by agreed convention"
            //*** thus, sat P1P2 DCB must be applied to DoD/IGS clocks if single freq. 
            //*** this is same convention as icd-200 20.3.3.3.3.2
            //*** this has NOTHING to do with use of any kind of normals ionosphere model
            //*** its purpose is undoing the disseminated svClock biases by means of
            //***    uncorrected (by DCB's) pseudorange ionosphere estimates

            if (!IsDualFreq) clockOffSet = clockOffSet - TGD;

            //*** compute broadcast position at corrected time

            Geo.Coordinates.XYZ xyz = GetSatXyz(gpsTime - clockOffSet, this.Prn);
            return new Ephemeris
            {
                XYZ = xyz,
                ClockBias = clockOffSet,
                RelativeCorrection = dtr,
                Prn = this.Prn,
               // Time = gpsTime,
                ClockDrift = ClockDrift, 
                XyzDot = new XYZ()
            };
        }

        public Ephemeris GetEphemerisInfo(Time gpsTime, XYZ staXyz, bool IsDualFreq = false)
        {
            double dt, dtr, clockOffSet;

            //*** correct satellite time
            dt = SatSvClock_NonRelative(gpsTime.SecondsOfWeek);                       //*** icd-200 20.3.3.3.3.1
            dtr = SatSvClock_Relative(gpsTime.SecondsOfWeek, dt);
            clockOffSet = dt + dtr;

            //*** icd-200 20.3.3.3.3.2  (not dual freq)
            //*** old version below
            //*** if( (!myModes.IsDualFreq()) && (!myModes.IsIon())) svClock = svClock - tgd;

            //*** 2006july28 notes
            //*** DOD does NOT apply sat P1P2 DCB correction (Tgd) to broadcast clocks
            //*** IGS does NOT apply sat P1P2 DCB corrections to their clocks "by agreed convention"
            //*** thus, sat P1P2 DCB must be applied to DoD/IGS clocks if single freq. 
            //*** this is same convention as icd-200 20.3.3.3.3.2
            //*** this has NOTHING to do with use of any kind of normals ionosphere model
            //*** its purpose is undoing the disseminated svClock biases by means of
            //***    uncorrected (by DCB's) pseudorange ionosphere estimates

            if (!IsDualFreq) clockOffSet = clockOffSet - TGD;

            //*** compute broadcast position at corrected time

            Geo.Coordinates.XYZ xyz = GetSatXyz(gpsTime.SecondsOfWeek - clockOffSet, this.Prn);



             var state = OrbitUtil.GetPosXYZ(this, gpsTime.SecondsOfWeek - clockOffSet);


            return new Ephemeris
            {
                XYZ = state.Value,
                ClockBias = clockOffSet,
                RelativeCorrection = dtr,
                Prn = this.Prn,
                Time = gpsTime,
                ClockDrift = ClockDrift,
                XyzDot = state.Rms
            };
        }

        public Ephemeris GetEphemerisInfo(double gpsTime, XYZ staXyz, bool IsDualFreq = false)
        {
            double dt, dtr, clockOffSet;

            //*** correct satellite time
            dt = SatSvClock_NonRelative(gpsTime);                       //*** icd-200 20.3.3.3.3.1
            dtr = SatSvClock_Relative(gpsTime, dt);
            clockOffSet = dt + dtr;

            //*** icd-200 20.3.3.3.3.2  (not dual freq)
            //*** old version below
            //*** if( (!myModes.IsDualFreq()) && (!myModes.IsIon())) svClock = svClock - tgd;

            //*** 2006july28 notes
            //*** DOD does NOT apply sat P1P2 DCB correction (Tgd) to broadcast clocks
            //*** IGS does NOT apply sat P1P2 DCB corrections to their clocks "by agreed convention"
            //*** thus, sat P1P2 DCB must be applied to DoD/IGS clocks if single freq. 
            //*** this is same convention as icd-200 20.3.3.3.3.2
            //*** this has NOTHING to do with use of any kind of normals ionosphere model
            //*** its purpose is undoing the disseminated svClock biases by means of
            //***    uncorrected (by DCB's) pseudorange ionosphere estimates

            if (!IsDualFreq) clockOffSet = clockOffSet - TGD;

            //*** compute broadcast position at corrected time

            Geo.Coordinates.XYZ xyz = GetSatXyz(gpsTime - clockOffSet, this.Prn);
            return new Ephemeris
            {
                XYZ = xyz,
                ClockBias = clockOffSet,
                RelativeCorrection = dtr,
                Prn = this.Prn,
              //  Time = gpsTime,
                ClockDrift = ClockDrift, 
                XyzDot = new XYZ()
            };
        }

        /// <summary>
        /// 二阶多项式归算钟差。经过改正后，各卫星间的同步误差可保持在20ns以内，这个影响可在相对定位中通过求差中消除。 
        /// compute satellite svClock correction (non-relativity term)
        /// sense of the corrector is:   t=tsv-dt
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public double SatSvClock_NonRelative(double t)
        {
            double dt;
            double span = t - Time.SecondsOfWeek;

            if (span > 302400.0) span = span - 604800.0;        //*** handle week rollover
            if (span < -302400.0) span = span + 604800.0;        //*** valid icd200

            dt = ClockBias + ClockDrift * span + DriftRate * span * span;       //*** valid icd200
            return dt;
        }
        /// <summary>
        /// compute satellite svClock correction (relativity term)
        /// sense of the corrector is:   t=tsv-dt-dtr
        /// </summary>
        /// <param name="t"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public double SatSvClock_Relative(double t, double dt)
        {
            double dtr;       //*** relativity term
            double ek;

            if ((t - Time.SecondsOfWeek) > 302400.0) t = t - 604800.0;        //*** handle week rollover
            if ((t - Time.SecondsOfWeek) < -302400.0) t = t + 604800.0;        //*** valid icd200

            //*** relativity term

            ek = EccentricAnomaly(t - dt);
            dtr = bigf * Eccentricity * SqrtA * Math.Sin(ek);           //*** valid icd200
            return dtr;
        }
        /// <summary>
        /// 根据轨道根数计算卫星位置。
        /// compute ECBF XYZ at time t from broadcast elements
        ///               units: fraction, meters, radians
        /// note: t corrected for SV svClock error, also passed in svClock
        /// note: also passing relativitistic correction
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public Geo.Coordinates.XYZ GetSatXyz(Time gpsTime)
        {
            return GetSatXyz(gpsTime.SecondsOfWeek, this.Prn);
        }


  
        public Geo.Coordinates.XYZ GetSatXyz(double secOfWeek, SatelliteNumber prn)
       {
           return OrbitUtil.GetSatXyz(this,secOfWeek, prn.SatelliteType, prn.PRN <= 5 && prn.SatelliteType == SatelliteType.C);

       }
       

        /// <summary>
        /// compute eccentric anomaly at time TProduct from broadcast elements
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double EccentricAnomaly(double time)
        {
            double a, en0, tk, en, emk;

            if ((time - Toe) > 302400.0) time = time - 604800.0;      //*** handle week rollover
            if ((time - Toe) < -302400.0) time = time + 604800.0;

            a = SqrtA * SqrtA;               //*** semimajor axis
            en0 = Math.Sqrt(GM / (a * a * a));   //*** mean motion
            tk = time - Toe;                    //*** time since orbit reference epoch
            en = en0 + DeltaN;                 //*** corrected mean motion
            emk = MeanAnomaly + en * tk;               //*** mean anomaly, M
            return OrbitUtil.KeplerEqForEccAnomaly(emk, Eccentricity);           //*** solve kepler's eq for ecc anomaly, E
        }
 
 
        #endregion


        public override bool Equals(object obj)
        {
            EphemerisParam p = obj as EphemerisParam;
            if (p == null) return false;

            return this.Prn.Equals(p.Prn) && this.Time.Equals(p.Time);
        }
        public override int GetHashCode()
        {
            return this.Prn.GetHashCode() + 13 * this.Time.GetHashCode();
        }
    }


}
