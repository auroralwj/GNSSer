//2016.03.15, double, create in zhengzhou, 将rtcm中的SSR数据转换成sp3格式的数据。
//2016.10.16, double, edit in hongqing, 实现了GPS SSR的转换
//2016.10.17, double, edit in hongqing, 实现了GLONASS SSR的转换

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Ntrip.Rtcm;
using Gnsser.Data.Rinex;
using Geo.Times;
using Geo.Coordinates;

namespace Gnsser.Ntrip
{
    /// <summary>
    /// RTCM 星历信息转换。
    /// </summary>
   public class RtcmSSRMessageConverter
   {
       #region RTCM 星历信息转换 GPS
       /// <summary>
       /// GPS SSR信息转换。  message1058
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGpsClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1058 msg, SSRGpsHeader67 SSRGpsClockCorrectionHeader)//, double SSRt,XYZ a,XYZ b)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRGpsClockCorrectionHeader.EpochTime1s);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.DriftRate = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = new double();
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// GPS SSR信息转换。  message1057
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGpsClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1057 msg, SSRGpsHeader68 SSRGpsClockCorrectionHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time( Setting.ReceivingTimeOfNtripData.GpsWeek, SSRGpsClockCorrectionHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// GPS SSR信息转换。  message1060
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGpsCombinedHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1060 msg, SSRGpsHeader68 SSRGpsCombinedHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRGpsCombinedHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockBiasRms = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);

           return Sp3Record;

       }
       #endregion

       #region RTCM 星历信息转换 GLONASS
       /// <summary>
       /// GLONASS SSR信息转换。Message1063
      /// </summary>
      /// <param name="msg"></param>
       /// <param name="SSRGlonassOrbitHeader"></param>
      /// <returns></returns>
       public Ephemeris GetSp3Param(Message1063 msg, SSRGlonassHeader65 SSRGlonassOrbitHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.R);
           Sp3Record.Time = new Time(DateTime.Parse(Setting.ReceivingTimeOfNtripData.Date.ToString()), SSRGlonassOrbitHeader.EpochTime1s+1);//是否需要更改，两个系统所采用的时间系统差的存在
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// GLONASS SSR信息转换。Message1064
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGlonassClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1064 msg, SSRGlonassHeader64 SSRGlonassClockCorrectionHeader)//, double SSRt,XYZ a,XYZ b)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.R);
           Sp3Record.Time = new Time(DateTime.Parse(Setting.ReceivingTimeOfNtripData.Date.ToString()), SSRGlonassClockCorrectionHeader.EpochTime1s+1);//是否需要更改，两个系统所采用的时间系统差的存在
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.DriftRate = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = new double();
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// GLONASS SSR信息转换。Message1064
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGlonassClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1066 msg, SSRGlonassHeader65 SSRGlonassClockCorrectionHeader)//, double SSRt,XYZ a,XYZ b)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.R);
           Sp3Record.Time = new Time(DateTime.Parse(Setting.ReceivingTimeOfNtripData.Date.ToString()), SSRGlonassClockCorrectionHeader.EpochTime1s+1);//是否需要更改，两个系统所采用的时间系统差的存在
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.DriftRate = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = new double();
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       
       #endregion

       #region RTCM 星历信息转换 BeiDou
       /// <summary>
       /// BeiDou SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRBeiDouClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1259 msg, SSRBeiDouHeader67 SSRBeiDouClockCorrectionHeader)//, double SSRt,XYZ a,XYZ b)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.C);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRBeiDouClockCorrectionHeader.EpochTime1s+14);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.DriftRate = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = new double();
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// BeiDou SSR信息转换。  message1235
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRBeiDouClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1258 msg, SSRBeiDouHeader68 SSRBeiDouClockCorrectionHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.C);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRBeiDouClockCorrectionHeader.EpochTime1s+14);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// BeiDou SSR信息转换。  message1238
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRBeiDouCombinedHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1261 msg, SSRBeiDouHeader68 SSRBeiDouCombinedHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.C);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRBeiDouCombinedHeader.EpochTime1s+14);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);

           return Sp3Record;

       }
       #endregion

       #region RTCM 星历信息转换 Galileo
       /// <summary>
       /// Galileo SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGalileoClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1241 msg, SSRGalileoHeader67 SSRGalileoClockCorrectionHeader)//, double SSRt,XYZ a,XYZ b)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.E);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRGalileoClockCorrectionHeader.EpochTime1s);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.DriftRate = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = new double();
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// Galileo SSR信息转换。 
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGalileoClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1240 msg, SSRGalileoHeader68 SSRGalileoClockCorrectionHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.E);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRGalileoClockCorrectionHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// Galileo SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRGalileoCombinedHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1243 msg, SSRGalileoHeader68 SSRGalileoCombinedHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID,SatelliteType.E);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRGalileoCombinedHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockBiasRms = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);

           return Sp3Record;

       }
       #endregion

       #region RTCM 星历信息转换 QZSS
       /// <summary>
       /// QZSS SSR信息转换。 
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRQZSSClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1247 msg, SSRQZSSHeader67 SSRQZSSClockCorrectionHeader)//, double SSRt,XYZ a,XYZ b)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRQZSSClockCorrectionHeader.EpochTime1s);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.DriftRate = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = new double();
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// QZSS SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRQZSSClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1246 msg, SSRQZSSHeader68 SSRQZSSClockCorrectionHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRQZSSClockCorrectionHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// QZSS SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRQZSSCombinedHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1249 msg, SSRQZSSHeader68 SSRQZSSCombinedHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRQZSSCombinedHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);

           return Sp3Record;

       }
       #endregion

       #region RTCM 星历信息转换 SBS
       /// <summary>
       /// SBS SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRSBSClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1253 msg, SSRSBSHeader67 SSRSBSClockCorrectionHeader)//, double SSRt,XYZ a,XYZ b)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRSBSClockCorrectionHeader.EpochTime1s);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.DriftRate = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockBiasRms = new double();
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// SBS SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRSBSClockCorrectionHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1252 msg, SSRSBSHeader68 SSRSBSClockCorrectionHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRSBSClockCorrectionHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.Rms = new XYZ();
           return Sp3Record;
       }
       /// <summary>
       /// SBS SSR信息转换。  
       /// </summary>
       /// <param name="msg"></param>
       /// <param name="SSRSBSCombinedHeader"></param>
       /// <returns></returns>
       public Ephemeris GetSp3Param(Message1255 msg, SSRSBSHeader68 SSRSBSCombinedHeader)
       {
           Ephemeris Sp3Record = new Ephemeris();
           Sp3Record.Prn = new SatelliteNumber((int)msg.SatelliteID, SatelliteType.G);
           Sp3Record.Time = new Time(Setting.ReceivingTimeOfNtripData.GpsWeek, SSRSBSCombinedHeader.EpochTime1s);
           Sp3Record.XYZ = new XYZ(msg.DeltaRadial * RtcmConst.DeltaRadial, msg.DeltaAlongTrack * RtcmConst.DeltaAlongTrack, msg.DeltaCrossTrack * RtcmConst.DeltaCrossTrack);
           Sp3Record.XyzDot = new XYZ(msg.DotDeltaRadial * RtcmConst.DotDeltaRadial, msg.DotDeltaAlongTrack * RtcmConst.DotDeltaAlongTrack, msg.DotDeltaCrossTrack * RtcmConst.DotDeltaCrossTrack);
           Sp3Record.ClockBias = msg.DeltaClockC0 * RtcmConst.DeltaClockC0solution / GnssConst.LIGHT_SPEED;//  (3 * 1E8);
           Sp3Record.ClockDrift = msg.DeltaClockC1 * RtcmConst.DeltaClockC1solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);
           Sp3Record.ClockBiasRms = msg.DeltaClockC2 * RtcmConst.DeltaClockC2solution / GnssConst.LIGHT_SPEED;// (3 * 1E8);

           return Sp3Record;

       }
       #endregion

       #region 转换为常用单位的方法

       /// <summary>
       /// 获取绝对的GPS周，从1980.01.06开始计算。
       /// </summary>
       /// <returns></returns>
       public static int GetAbsoluteWeekNumberFromNow(int WeekNumberIn1024 )
       {
           var currentTime = DateTime.UtcNow;
           return GetAbusoluteWeekNumber(WeekNumberIn1024, currentTime);
       }

       public static int GetAbusoluteWeekNumber(int WeekNumberIn1024, DateTime currentTime)
       {
           var passedDays = (currentTime - new DateTime(1980, 1, 6, 0, 0, 0, DateTimeKind.Utc)).TotalDays;
           var daysPer1024Week = 7 * 1024;
           int rollCount = (int)(passedDays / daysPer1024Week);
           return WeekNumberIn1024 + rollCount * 1024;
       }
       #endregion
    }
}
