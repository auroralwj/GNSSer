//2015.02.04, czs, create in pengzhou, 处理 RTCM 3.X 版本的数据读取。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gnsser.Domain;
using Gnsser.Data.Rinex;

namespace Gnsser.Ntrip.Rtcm
{ 
    /// <summary>
    /// RTCM 3 信息转换
    /// </summary>
    public class Rtcm3Converter
    {

        public Rtcm3Converter()
        {
        }

        //保存上一个数据
        Dictionary<SatelliteNumber, Dictionary<FrequenceType, double>> PreviousData = new Dictionary<SatelliteNumber, Dictionary<FrequenceType, double>>();
        #region GPS
        public RinexEpochObservation GetEpochObservation(EpochMessage EpochMessage)
        { 
            RinexEpochObservation rinex = new RinexEpochObservation();
            rinex.ReceiverTime = EpochMessage.Header.Time;

            foreach (var item in EpochMessage)
            {
                var msg = item as NormalMessage1004;

                 RinexSatObsData satData = GetSatObsData(  msg);

                 rinex.Add(satData.Prn, satData);
            }

            return rinex;
        }
     
        /// <summary>
        /// 对上面的函数重新编写
        /// </summary>
        /// <param name="msg1004"></param>
        /// <returns></returns>
        public RinexSatObsData GetSatObsData(NormalMessage1004 msg1004)
        {
            RinexSatObsData obsData = new RinexSatObsData();

            double L1Pseudorange = 0;
            double L1Phase = 0;
            double L2Pseudorange = 0;
            double L2Phase = 0;

            obsData.Prn = msg1004.Prn;

            L1Pseudorange = GetL1PseudoRange(msg1004.GpsL1Pseudorange, msg1004.GpsIntegerL1PseudorangeModulusAmbiguity);

            if (msg1004.GpsL1PhaseRangeMinusPseudorange != RtcmConst.MaxGpsL1PhaseRangeMinusPseudorange)
            {
                double L1PhaseMinusPseudorange = msg1004.GpsL1PhaseRangeMinusPseudorange * RtcmConst.PhaseRangeResolution / Frequence.GpsL1.WaveLength;
                L1PhaseMinusPseudorange = RevisePhaseRange(obsData.Prn, FrequenceType.A, L1PhaseMinusPseudorange);

                L1Phase = L1Pseudorange / Frequence.GpsL1.WaveLength + L1PhaseMinusPseudorange;
            }

            if (msg1004.GpsL2MinusL1PseudorangeDifference != RtcmConst.MaxL2MinusL1PseudorangeDifference)
            {
                L2Pseudorange = GetL2PseudoRange(L1Pseudorange, msg1004.GpsL2MinusL1PseudorangeDifference);
            }

            if (msg1004.GpsL2PhaseRangeMinusL1Pseudorange != RtcmConst.MaxGpsL1PhaseRangeMinusPseudorange)
            {
                double L2PhaseMinusL1Pseudorange = msg1004.GpsL2PhaseRangeMinusL1Pseudorange * RtcmConst.PhaseRangeResolution / Frequence.GpsL2.WaveLength;
                L2PhaseMinusL1Pseudorange = RevisePhaseRange(obsData.Prn, FrequenceType.B, L2PhaseMinusL1Pseudorange);
                L2Phase = L1Pseudorange / Frequence.GpsL2.WaveLength + L2PhaseMinusL1Pseudorange;
            }
            //obsData.

            //ObservationCode ObservationCodeOfC = new ObservationCode();
            //ObservationCode ObservationCodeOfL = new ObservationCode();
            //ObservationCode ObservationCodeOfS = new ObservationCode();
            //ObservationCode ObservationCodeOfD = new ObservationCode();
            obsData.Add("C1X", L1Pseudorange);
            obsData.Add("L1X", L1Phase);
            obsData.Add("P2X", L2Pseudorange);
            obsData.Add("L2X", L2Phase);

            return obsData;
        }
        #endregion
        #region GLONASS
        public RinexEpochObservation GetEpochObservation(GlonassEpochMessage EpochMessage)
        {
            RinexEpochObservation rinex = new RinexEpochObservation();
            rinex.ReceiverTime = EpochMessage.Header.Time; 
            foreach (var item in EpochMessage)
            {
                var msg = item as GlonassNormalMessage1012;

                RinexSatObsData satData = GetSatObsData(msg);

                rinex.Add(satData.Prn, satData);
            }

            return rinex;
        }

        /// <summary>
        /// 对上面的函数重新编写
        /// </summary>
        /// <param name="msg1012"></param>
        /// <returns></returns>
        public RinexSatObsData GetSatObsData(GlonassNormalMessage1012 msg1012)
        {
            RinexSatObsData obsData = new RinexSatObsData();

            double L1Pseudorange = 0;
            double L1Phase = 0;
            double L2Pseudorange = 0;
            double L2Phase = 0;

            obsData.Prn = msg1012.Prn;

            L1Pseudorange = GetL1PseudoRange(msg1012.GlonassL1Pseudorange, msg1012.GlonassIntegerL1PseudorangeModulusAmbiguity);

            if (msg1012.GlonassL1PhaseRangeMinusPseudorange < RtcmConst.MaxGpsL1PhaseRangeMinusPseudorange)
            {
                double L1PhaseMinusPseudorange = msg1012.GlonassL1PhaseRangeMinusPseudorange * RtcmConst.PhaseRangeResolution / Frequence.GpsL1.WaveLength;
                L1PhaseMinusPseudorange = RevisePhaseRange(obsData.Prn, FrequenceType.A, L1PhaseMinusPseudorange);

                L1Phase = L1Pseudorange / Frequence.GpsL1.WaveLength + L1PhaseMinusPseudorange;
            }

            if (msg1012.GlonassL2MinusL1PseudorangeDifference < RtcmConst.MaxL2MinusL1PseudorangeDifference)
            {
                L2Pseudorange = GetL2PseudoRange(L1Pseudorange, msg1012.GlonassL2MinusL1PseudorangeDifference);
            }

            if (msg1012.GlonassL2PhaseRangeMinusL1Pseudorange < RtcmConst.MaxGpsL1PhaseRangeMinusPseudorange)
            {
                double L2PhaseMinusL1Pseudorange = msg1012.GlonassL2PhaseRangeMinusL1Pseudorange * RtcmConst.PhaseRangeResolution / Frequence.GpsL2.WaveLength;
                L2PhaseMinusL1Pseudorange = RevisePhaseRange(obsData.Prn, FrequenceType.B, L2PhaseMinusL1Pseudorange);
                L2Phase = L1Pseudorange / Frequence.GpsL2.WaveLength + L2PhaseMinusL1Pseudorange;
            }
            //obsData.

            obsData.Add("C1X", L1Pseudorange);
            obsData.Add("L1X", L1Phase);
            obsData.Add("P2X", L2Pseudorange);
            obsData.Add("L2X", L2Phase);

            return obsData;
        }
        #endregion
        /// <summary>
        /// 将RTCM L1 伪距复原。
        /// </summary>
        /// <param name="L1Pseudorange"></param>
        /// <param name="GpsL2MinusL1PseudorangeDifference"></param>
        /// <returns></returns>
        private double GetL2PseudoRange(double L1Pseudorange, double GpsL2MinusL1PseudorangeDifference)
        {
            return L1Pseudorange + GpsL2MinusL1PseudorangeDifference * RtcmConst.PseoudoRangeResolution;
        }

        /// <summary>
        /// 将RTCM L1 伪距复原。
        /// </summary>
        /// <param name="rtcmPseudorange"></param>
        /// <param name="PseudorangeModulusAmbiguity"></param>
        /// <returns></returns>
        private double GetL1PseudoRange(double rtcmPseudorange, double PseudorangeModulusAmbiguity)
        {
            return rtcmPseudorange * RtcmConst.PseoudoRangeResolution
              + PseudorangeModulusAmbiguity * GnssConst.LightSpeedPerMillisecond;
        }
        
        /// <summary>
        /// 对载波距离进行修正，若超出一定的距离。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="freqType"></param>
        /// <param name="PhaseRangeMinusPseudorange"></param>
        /// <returns></returns>
        private double RevisePhaseRange(SatelliteNumber prn, FrequenceType freqType, double PhaseRangeMinusPseudorange)
        {
            if (!PreviousData.ContainsKey(prn)) { PreviousData[prn] = new Dictionary<FrequenceType, double>(); }

            if (!PreviousData[prn].ContainsKey(freqType)) { PreviousData[prn][freqType] = 0; }
            else
            {
                var prevValue = PreviousData[prn][freqType];

                if (PhaseRangeMinusPseudorange < prevValue - 750.0) { PhaseRangeMinusPseudorange += 1500.0; }
                else if (PhaseRangeMinusPseudorange > prevValue + 750.0) { PhaseRangeMinusPseudorange -= 1500.0; }
            }

            PreviousData[prn][freqType] = PhaseRangeMinusPseudorange;

            return PhaseRangeMinusPseudorange;
        }

         
        //private int LossOfLock(ref int [,] GpsL1LocktimeIndicator, SatelliteNumber prn, int freq, int lossoflock)
        //{
        //    int lli = (!lossoflock && GpsL1LocktimeIndicator[prn.PRN - 1, freq]) || GpsL1LocktimeIndicator[prn.PRN - 1, freq];
        //    GpsL1LocktimeIndicator[prn.PRN - 1, freq] = lossoflock;
        //    return lli;
        //}

    } 
}