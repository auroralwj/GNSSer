//2015.05.06, czs, create in namu, RTCM 星历信息转换。
//2016.03.24, double, edit in zhengzhou, 实现RTCM GPS星历信息转换。
//2016.10.17, double, edit in hongqing, 实现RTCM GLONASS星历信息转换。
//2016.11.17&18, double, edit in hongqing, 初步实现RTCM BeiDou，Galileo，QZSS星历信息转换。
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Ntrip.Rtcm;
using Gnsser.Data.Rinex;
using Geo.Times; 

namespace Gnsser.Ntrip
{
    /// <summary>
    /// RTCM 星历信息转换。
    /// </summary>
   public class RtcmEphMessageConverter
    {
       
       /// <summary>
       /// GPS 实时星历信息转换。
       /// </summary>
       /// <param name="msg"></param>
       /// <returns></returns>
       public EphemerisParam GetEphemerisParam(Message1019 msg)
       {
           EphemerisParam para = new EphemerisParam();
           para.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);

           #region 开普勒轨道根数 kepler elements
           para.Toc = msg.Toc * 16;
           para.Toe = msg.Toe * 16;
           para.SqrtA = msg.SqrtA * RtcmConst.P2_19;
           para.Eccentricity = msg.Eccentricity * RtcmConst.P2_33;
           para.Inclination = msg.I0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.LongOfAscension = msg.Omega0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.ArgumentOfPerigee = msg.ArgumentOfPerigee * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.MeanAnomaly = msg.M0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           #endregion

           #region 轨道摄动参数 perturbation parameters
           para.DeltaN = msg.DeltaN * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           para.EyeDot = msg.Idot * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           //??
           para.OmegaDot = msg.OmegaDot * RtcmConst.SemiCircleToRad * RtcmConst.P2_43;
           para.Cuc = msg.Cuc * RtcmConst.P2_29;
           //??
           para.Cus = msg.Cus * RtcmConst.P2_29;
           para.Crc = msg.Crc * RtcmConst.P2_5;
           //??
           para.Crs = msg.Crs * RtcmConst.P2_5;
           //??
           para.Cic = msg.Cic * RtcmConst.P2_29;
           //??
           para.Cis = msg.Cis * RtcmConst.P2_29;
           #endregion

           #region other satData
           para.IODE = msg.Iode;
           para.CodesL2 = msg.CodeOnL2;
           para.GPSWeeks = GetAbsoluteWeekNumber((int)msg.WeekNumber);
           para.L2PDataFlag = msg.L2PDataFlag;
           para.SVAccuracy = msg.SvAccuracy;
           para.SVHealth = msg.SvHealth;
           para.TGD = msg.Tgd * RtcmConst.P2_31;
           para.IODC = msg.Iodc;
           //??
           para.TTM = 0;
           //??
           para.FitInterval = msg.FitInterval;/* 0:4hr,1:>4hr */
           #endregion

           #region clock
           para.ClockBias = msg.Af0 * RtcmConst.P2_31;
           para.ClockDrift = msg.Af1 * RtcmConst.P2_43;
           para.DriftRate = msg.Af2 * RtcmConst.P2_55;
           para.Time = new Time(para.GPSWeeks, para.Toe);
           #endregion
           return para;

       }
       /// <summary>
       /// BeiDou 实时星历信息转换。
       /// </summary>
       /// <param name="msg"></param>
       /// <returns></returns>
       public EphemerisParam GetEphemerisParam(Message63 msg)
       {
           EphemerisParam para = new EphemerisParam();
           para.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.C);

           #region 开普勒轨道根数 kepler elements
           para.Toc = msg.Toc * 8;
           para.Toe = msg.Toe * 8;
           para.SqrtA = msg.SqrtA * RtcmConst.P2_19;
           para.Eccentricity = msg.Eccentricity * RtcmConst.P2_33;
           para.Inclination = msg.I0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.LongOfAscension = msg.Omega0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.ArgumentOfPerigee = msg.ArgumentOfPerigee * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.MeanAnomaly = msg.M0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           #endregion

           #region 轨道摄动参数 perturbation parameters
           para.DeltaN = msg.DeltaN * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           para.EyeDot = msg.Idot * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           //??
           para.OmegaDot = msg.OmegaDot * RtcmConst.SemiCircleToRad * RtcmConst.P2_43;
           para.Cuc = msg.Cuc * RtcmConst.P2_31;
           //??
           para.Cus = msg.Cus * RtcmConst.P2_31;
           para.Crc = msg.Crc * RtcmConst.P2_6;
           //??
           para.Crs = msg.Crs * RtcmConst.P2_6;
           //??
           para.Cic = msg.Cic * RtcmConst.P2_31;
           //??
           para.Cis = msg.Cis * RtcmConst.P2_31;
           #endregion

           #region other satData
           para.IODE = msg.Aode;
           //para.CodesL2 = msg.CodeOnL2;
           //para.BeiDouWeeks = GetAbsoluteBeiDouWeekNumber((int)msg.WeekNumber);
           para.GPSWeeks = Setting.ReceivingTimeOfNtripData.GpsWeek;//para.BeiDouWeeks + 1356; 
           //para.L2PDataFlag = msg.L2PDataFlag;
           if (msg.BeiDOuURAI >= 0 && msg.BeiDOuURAI < 6) para.SVAccuracy = Math.Pow(2, msg.BeiDOuURAI / 2 + 1);
           else if (msg.BeiDOuURAI < 15) para.SVAccuracy = Math.Pow(2, msg.BeiDOuURAI - 2);
           else para.SVAccuracy = 9999999.9999;
           para.SVHealth = msg.SvHealth;
           para.Tgd1OfBeidDou = msg.Tgd1*0.1;
           para.Tgd2OfBeidDou = msg.Tgd2*0.1;
           para.IODC = msg.Aodc;
           para.SatH1 = msg.SatH1;            
           #endregion

           #region clock
           para.ClockBias = msg.a0 * RtcmConst.P2_33;
           para.ClockDrift = msg.a1 * RtcmConst.P2_50;
           para.DriftRate = msg.a2 * RtcmConst.P2_66;
           para.Time = new Time(para.GPSWeeks, para.Toe);
           #endregion

           return para;

       }
       /// <summary>
       /// Glonass 实时星历信息转换。
       /// </summary>
       /// <param name="msg"></param>
       /// <returns></returns>
       public GlonassNavRecord GlonassNavRecord(Message1020 msg)
       {
           GlonassNavRecord para = new GlonassNavRecord();
           para.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.R);

           #region XYZ相关信息
           para.XYZ = new Geo.Coordinates.XYZ(msg.Xn,msg.Yn ,msg.Zn );
           para.XyzVelocity = new Geo.Coordinates.XYZ(msg.XnfFirstDerivative,msg.YnfFirstDerivative,msg.XnfFirstDerivative);
           para.XyzAcceleration = new Geo.Coordinates.XYZ(msg.XnSecondDerivative,msg.YnSecondDerivative,msg.ZnSecondDerivative);
           #endregion

           para.Health = msg.MsbOfBnWord;
           para.FrequencyNumber = msg.SatelliteFrequencyChannelNumber;
           para.ClockBias = msg.TaoN;
           para.RelativeFrequencyBias = msg.GamaN;
           double secondsOfDay;
           if(msg.Tkh>=3)
               secondsOfDay=(msg.Tkh-3)*3600+msg.Tkm *60+msg.Tks;
           else secondsOfDay=(msg.Tkh-3+24)*3600+msg.Tkm *60+msg.Tks;
           para.Time= new Time(Setting.ReceivingTimeOfNtripData.DateTime, secondsOfDay);
           para.MessageTime = msg.Tb;
           para.AgeOfOper = new double();
           return para;

       }
       /// <summary>
       /// QZSS 实时星历信息转换。
       /// </summary>
       /// <param name="msg"></param>
       /// <returns></returns>
       public EphemerisParam GetEphemerisParam(Message1044 msg)
       {
           EphemerisParam para = new EphemerisParam();
           para.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.S);

           #region 开普勒轨道根数 kepler elements
           para.Toc = msg.Toc * 16;
           para.Toe = msg.Toe * 16;
           para.SqrtA = msg.SqrtA * RtcmConst.P2_19;
           para.Eccentricity = msg.Eccentricity * RtcmConst.P2_33;
           para.Inclination = msg.I0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.LongOfAscension = msg.Omega0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.ArgumentOfPerigee = msg.ArgumentOfPerigee * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.MeanAnomaly = msg.M0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           #endregion

           #region 轨道摄动参数 perturbation parameters
           para.DeltaN = msg.DeltaN * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           para.EyeDot = msg.i0DOT * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           //??
           para.OmegaDot = msg.OmegaDot * RtcmConst.SemiCircleToRad * RtcmConst.P2_43;
           para.Cuc = msg.Cuc * RtcmConst.P2_29;
           //??
           para.Cus = msg.Cus * RtcmConst.P2_29;
           para.Crc = msg.Crc * RtcmConst.P2_5;
           //??
           para.Crs = msg.Crs * RtcmConst.P2_5;
           //??
           para.Cic = msg.Cic * RtcmConst.P2_29;
           //??
           para.Cis = msg.Cis * RtcmConst.P2_29;
           #endregion

           #region other satData
           para.IODE = msg.Iode;
           para.CodesL2 = msg.CodeOnL2;
           para.GPSWeeks = GetAbsoluteWeekNumber((int)msg.WeekNumber);
           //para.L2PDataFlag = msg.L2PDataFlag;
           para.SVAccuracy = msg.URA;
           para.SVHealth = msg.SVHealthState;
           para.TGD = msg.Tgd * RtcmConst.P2_31;
           para.IODC = msg.Iodc;
           //??
           para.TTM = 0;
           //??
           para.FitInterval = msg.FitInterval;/* 0:4hr,1:>4hr */
           #endregion

           #region clock
           para.ClockBias = msg.af0 * RtcmConst.P2_31;
           para.ClockDrift = msg.af1 * RtcmConst.P2_43;
           para.DriftRate = msg.af2 * RtcmConst.P2_55;
           para.Time = new Time(para.GPSWeeks, para.Toe);
           #endregion
           return para;

       }
       /// <summary>
       /// Galileo 实时星历信息转换。
       /// </summary>
       /// <param name="msg"></param>
       /// <returns></returns>
       public EphemerisParam GetEphemerisParam(Message1045 msg)
       {
           EphemerisParam para = new EphemerisParam();
           para.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.E);

           #region 开普勒轨道根数 kepler elements
           para.Toc = msg.Toc * 60;
           para.Toe = msg.Toe * 60;
           para.SqrtA = msg.SqrtA * RtcmConst.P2_19;
           para.Eccentricity = msg.Eccentricity * RtcmConst.P2_33;
           para.Inclination = msg.I0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.LongOfAscension = msg.Omega0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.ArgumentOfPerigee = msg.ArgumentOfPerigee * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           para.MeanAnomaly = msg.M0 * RtcmConst.P2_31 * RtcmConst.SemiCircleToRad;
           #endregion

           #region 轨道摄动参数 perturbation parameters
           para.DeltaN = msg.DeltaN * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           para.EyeDot = msg.Idot * RtcmConst.P2_43 * RtcmConst.SemiCircleToRad;
           //??
           para.OmegaDot = msg.OmegaDot * RtcmConst.SemiCircleToRad * RtcmConst.P2_43;
           para.Cuc = msg.Cuc * RtcmConst.P2_29;
           //??
           para.Cus = msg.Cus * RtcmConst.P2_29;
           para.Crc = msg.Crc * RtcmConst.P2_5;
           //??
           para.Crs = msg.Crs * RtcmConst.P2_5;
           //??
           para.Cic = msg.Cic * RtcmConst.P2_29;
           //??
           para.Cis = msg.Cis * RtcmConst.P2_29;
           #endregion

           #region other satData
           para.IODE = msg.Iodn;
           para.TTM = 0;
           para.GalileoWeeks = GetAbsoluteWeekNumber((int)msg.WeekNumber);
           para.SVAccuracy =msg.GalileoSVSISA;
           #endregion

           #region clock
           para.ClockBias = msg.af0 * RtcmConst.P2_34;
           para.ClockDrift = msg.af1 * RtcmConst.P2_46;
           para.DriftRate = msg.af2 * RtcmConst.P2_59;
           para.Time = new Time(para.GPSWeeks, para.Toe);
           #endregion
           return para;

       }
       #region 转换为常用单位的方法

       /// <summary>
       /// 获取绝对的GPS周，从1980.01.06开始计算。
       /// </summary>
       /// <returns></returns>
       public static int GetAbsoluteWeekNumber(int WeekNumberIn1024 )
       {
           var currentTime = Setting.ReceivingTimeOfNtripData.DateTime;
           return GetAbusoluteWeekNumber(WeekNumberIn1024, currentTime);
       }

       public static int GetAbusoluteWeekNumber(int WeekNumberIn1024, DateTime currentTime)
       {
           var passedDays = (currentTime - new DateTime(1980, 1, 6, 0, 0, 0, DateTimeKind.Utc)).TotalDays;
           var daysPer1024Week = 7 * 1024;
           int rollCount = (int)(passedDays / daysPer1024Week);
           return WeekNumberIn1024 + rollCount * 1024;
       }
       /// <summary>
       /// 获取绝对的BeiDou周，从2006.01.01开始计算。
       /// </summary>
       /// <returns></returns>
       public static int GetAbsoluteBeiDouWeekNumber(int WeekNumberIn8192)
       {
           var currentTime = Setting.ReceivingTimeOfNtripData.DateTime;
           return GetAbusoluteBeiDouWeekNumber(WeekNumberIn8192, currentTime);
       }
       public static int GetAbusoluteBeiDouWeekNumber(int WeekNumberIn8192, DateTime currentTime)
       {
           var passedDays = (currentTime - new DateTime(2006, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalDays;
           var daysPer8192Week = 7 * 8192;
           int rollCount = (int)(passedDays / daysPer8192Week);
           return WeekNumberIn8192 + rollCount * 8192;
       }

       #endregion

    }
}
