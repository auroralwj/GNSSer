//2016.10.17, double, create in hongqing, 将MSM文件信息转换为观测文件信息
//2017.05.02, czs, edit in hongqing, 重构观测码信息

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
    /// RTCM MSM文件信息转换为观测文件信息
    /// </summary>
    public class RtcmMsmMessageConverter
    {

        public RtcmMsmMessageConverter(RinexObsFileHeader ObsHeader)
        {
            // TODO: Complete member initialization
            this.ObsHeader = ObsHeader;
        }

        public RinexObsFileHeader ObsHeader { get; set; }

        #region RTCM 星历信息转换 MSM
        /// <summary>
        /// RTCM MSM文件信息转换为观测文件信息.此处返回的是RINEX3.0的数据类型。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="HeaderOfMSM"></param>
        /// <returns></returns>
        public RinexEpochObservation GetRinexEpochObservation(BaseMSM msg, HeaderOfMSM HeaderOfMSM)
        {
            RinexEpochObservation RinexEpochObservation = new RinexEpochObservation();             
            
            uint messageNumber = HeaderOfMSM.MessageNumber;
            double wavelength = 0;
            int frequencyBand = 0;
            string str = null;
            string attribute = null;
            double P = 0, L = 0, D = 0, S = 0;
            int j = 0;
            int index = 0;
            double p = 0;
            SatelliteType satType = SatelliteType.G;
            for (int i = 0; i < HeaderOfMSM.SatCount; i++)
            {
                RinexSatObsData SatObsData = new RinexSatObsData();
                for (int k = 0; k < HeaderOfMSM.Nsig; k++)
                {
                    if (HeaderOfMSM.CellMask[index++] == 0) 
                        continue;
                    if (messageNumber >= 1071 && messageNumber <= 1077)//GPS
                    {
                        satType = SatelliteType.G;
                        SatObsData.Prn = new SatelliteNumber(HeaderOfMSM.SatlliteMask[i], SatelliteType.G);
                        str = msm_sig_gps[HeaderOfMSM.SignalMask[k] - 1];
                        frequencyBand = int.Parse(str.Substring(0, 1));
                        attribute = str.Substring(1, 1);
                        if (frequencyBand == 1) wavelength = Frequence.GpsL1.WaveLength;
                        else if (frequencyBand == 2) wavelength = Frequence.GpsL2.WaveLength;
                        else if (frequencyBand == 5) wavelength = Frequence.GpsL5.WaveLength;
                    }
                    else if (messageNumber >= 1081 && messageNumber <= 1087)//GLONASS
                    {
                        satType = SatelliteType.R;
                        SatObsData.Prn = new SatelliteNumber(HeaderOfMSM.SatlliteMask[i], SatelliteType.R);

                    }
                    else if (messageNumber >= 1091 && messageNumber <= 1097)//GALILEO
                    {
                        satType = SatelliteType.E;
                        SatObsData.Prn = new SatelliteNumber(HeaderOfMSM.SatlliteMask[i], SatelliteType.E);

                    }
                    else if (messageNumber >= 1101 && messageNumber <= 1017)//SBS
                    {
                        satType = SatelliteType.S;
                        SatObsData.Prn = new SatelliteNumber(HeaderOfMSM.SatlliteMask[i], SatelliteType.S);

                    }
                    else if (messageNumber >= 1111 && messageNumber <= 1117)//QZS
                    {
                        satType = SatelliteType.J;
                        SatObsData.Prn = new SatelliteNumber(HeaderOfMSM.SatlliteMask[i], SatelliteType.J);

                    }
                    else if (messageNumber >= 1121 && messageNumber <= 1127)//BDS
                    {
                        satType = SatelliteType.C;
                        SatObsData.Prn = new SatelliteNumber(HeaderOfMSM.SatlliteMask[i], SatelliteType.C);
                        str = msm_sig_gps[HeaderOfMSM.SignalMask[k] - 1];
                        frequencyBand = int.Parse(str.Substring(0, 1));
                        attribute = str.Substring(1, 1);
                        
                        if (frequencyBand == 2) wavelength = Frequence.CompassB1.WaveLength;
                        else if (frequencyBand == 6) wavelength = Frequence.CompassB3.WaveLength;
                        else if (frequencyBand == 7) wavelength = Frequence.CompassB2.WaveLength;

                    }
                    var ObservationCodeOfC = new ObservationCode(ObservationType.C, frequencyBand, attribute);
                    var ObservationCodeOfL = new ObservationCode(ObservationType.L, frequencyBand, attribute);
                    var ObservationCodeOfS = new ObservationCode(ObservationType.S, frequencyBand, attribute);
                    var ObservationCodeOfD = new ObservationCode(ObservationType.D, frequencyBand, attribute);
                    if (msg.NumberOfIntegerMsInSatRoughRange.Count != 0)
                        p = msg.NumberOfIntegerMsInSatRoughRange[i] * 0.001 + msg.SatelliteRoughRangesModulo1ms[i] * Math.Pow(2, -13);
                    else p = msg.SatelliteRoughRangesModulo1ms[i] * Math.Pow(2, -13);
                    
                    if (msg.SignalFinePseudorange.Count!= 0)
                        P = msg.SignalFinePseudorange[j] * Math.Pow(2, -24) + p;
                    else if (msg.SignalFinePseudorangeWithExtendedResolution.Count!= 0)
                        P = msg.SignalFinePseudorangeWithExtendedResolution[j] * Math.Pow(2, -29) + p;
                    P = P * RtcmConst.LightSpeedPerMillisecond;

                    if (msg.SignalFinePhaseRange.Count != 0)
                        L = msg.SignalFinePhaseRange[j] * Math.Pow(2, -29) + p;
                    else if (msg.SignalFinePhaseRangeWithExtendedResolution.Count != 0) 
                        L = msg.SignalFinePhaseRangeWithExtendedResolution[j] * Math.Pow(2, -31) + p;
                    L = L * RtcmConst.LightSpeedPerMillisecond / wavelength;

                    if (msg.PhaseRangeLockTimeIndicator.Count != 0)
                        S = msg.PhaseRangeLockTimeIndicator[j];
                    else if (msg.PhaseRangeLockTimeIndicatorWithExtendedRangeAndResolution[j] != 0)
                        S = msg.PhaseRangeLockTimeIndicatorWithExtendedRangeAndResolution[j];

                    if (messageNumber % 10 == 5 || messageNumber % 10 == 7 && msg.SatRoughPhaseRangeRate.Count != 0)
                    {
                        if (msg.SatRoughPhaseRangeRate[i] > -1638.4)
                        {
                            D = -(msg.SatRoughPhaseRangeRate[i] * 0.001 + msg.SignalFinePhaseRangeRate[j] * 0.0001) / wavelength;                            
                        }
                    }
                    SatObsData.Add(ObservationCodeOfC.ToString(), P);
                    SatObsData.Add(ObservationCodeOfL.ToString(), L);
                    SatObsData.Add(ObservationCodeOfS.ToString(), S);
                    SatObsData.Add(ObservationCodeOfD.ToString(), D);
                    
                    //更新头部信息
                    var obsCodes = new List<string>();
                    obsCodes.Add(ObservationCodeOfC.ToString());
                    obsCodes.Add(ObservationCodeOfL.ToString());
                    obsCodes.Add(ObservationCodeOfS.ToString());
                    obsCodes.Add(ObservationCodeOfD.ToString());
                    if (ObsHeader != null)
                    {
                        ObsHeader.ObsCodes[satType] = obsCodes;
                    }

                    j++;
                }
                
                RinexEpochObservation.Add(SatObsData.Prn,SatObsData);
            }
            
            RinexEpochObservation.Header = ObsHeader;
            RinexEpochObservation.ReceiverTime = HeaderOfMSM.EpochTime;

            return RinexEpochObservation;
        }
        
        public static  List<string> GetObsCodes(){
            return new List<string>() { "C1X", "L1X", "P2X", "L2X" };
        }

        #endregion

        #region 转换为常用单位的方法

        /// <summary>
        /// 获取绝对的GPS周，从1980.01.06开始计算。
        /// </summary>
        /// <returns></returns>
        public static int GetAbsoluteWeekNumberFromNow(int WeekNumberIn1024)
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

        public  List<string> msm_sig_gps = new List<string> 
        {
            /* GPS: ref [13] table 3.5-87, ref [14] table 3.5-91 */
            ""  ,"1C","1P","1W","1Y","1M",""  ,"2C","2P","2W","2Y","2M", /*  1-12 */
            ""  ,""  ,"2S","2L","2X",""  ,""  ,""  ,""  ,"5I","5Q","5X", /* 13-24 */
            ""  ,""  ,""  ,""  ,""  ,"1S","1L","1X"                      /* 25-32 */
        };
        private  List<string> msm_sig_glonass = new List<string> 
        {
            /* GLONASS: ref [13] table 3.5-93, ref [14] table 3.5-97 */
            ""  ,"1C","1P",""  ,""  ,""  ,""  ,"2C","2P",""  ,"3I","3Q",
            "3X",""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,
            ""  ,""  ,""  ,""  ,""  ,""  ,""  ,""
        };
        private  List<string> msm_sig_galileo = new List<string>
        {
            /* Galileo: ref [13] table 3.5-96, ref [14] table 3.5-100 */
            ""  ,"1C","1A","1B","1X","1Z",""  ,"6C","6A","6B","6X","6Z",
            ""  ,"7I","7Q","7X",""  ,"8I","8Q","8X",""  ,"5I","5Q","5X",
            ""  ,""  ,""  ,""  ,""  ,""  ,""  ,""
        };
        private  List<string> msm_sig_qzss = new List<string>
        {
            /* QZSS: ref [13] table 3.5-T+003 */
            ""  ,"1C",""  ,""  ,""  ,"1Z",""  ,""  ,"6S","6L","6X",""  ,
            ""  ,""  ,"2S","2L","2X",""  ,""  ,""  ,""  ,"5I","5Q","5X",
            ""  ,""  ,""  ,""  ,""  ,"1S","1L","1X"
        };
        private  List<string> msm_sig_sbs = new List<string> 
        {
            /* SBAS: ref [13] table 3.5-T+005 */
            ""  ,"1C",""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,
            ""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,"5I","5Q","5X",
            ""  ,""  ,""  ,""  ,""  ,""  ,""  ,""
        };
        private  List<string> msm_sig_bds = new List<string> 
        {
            /* BeiDou: ref [13] table 3.5-T+012 */
            ""  ,"2I","2Q","2X",""  ,""  ,""  ,"6I","6Q","6X",""  ,""  ,
            ""  ,"7I","7Q","7X",""  ,""  ,""  ,""  ,""  ,""  ,""  ,""  ,
            ""  ,""  ,""  ,""  ,""  ,""  ,""  ,""
        };
    }
}
