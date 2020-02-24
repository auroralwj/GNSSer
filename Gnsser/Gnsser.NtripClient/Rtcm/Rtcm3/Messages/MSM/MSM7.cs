//2016.10.17, double, create in hongqing, RTCM MSM7

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    ///  Compact Pseudoranges , PhaseRanges, PhaseRangeRate and cnr (high resolution)
    /// </summary>
    public class MSM7 : BaseMSM
    {
        public MSM7()
        {
        }


        public static MSM7 Parse(List<byte> data, int Nsat, int Ncell)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr,  Nsat,  Ncell);
        }

        public static MSM7 Parse(string binStr, int Nsat, int Ncell)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            MSM7 msg = new MSM7();

            for (int i = 0; i < Nsat; i++)
                msg.NumberOfIntegerMsInSatRoughRange.Add(BitConvertUtil.GetUInt(sequence.DeQueue(8)));
            for (int i = 0; i < Nsat; i++)
                msg.ExtendedSatInfo.Add(BitConvertUtil.GetUInt(sequence.DeQueue(4)));
            for (int i = 0; i < Nsat; i++)
                msg.SatelliteRoughRangesModulo1ms.Add(BitConvertUtil.GetUInt(sequence.DeQueue(10)));
            for (int i = 0; i < Nsat; i++)
                msg.SatRoughPhaseRangeRate.Add(BitConvertUtil.GetInt(sequence.DeQueue(14)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalFinePseudorangeWithExtendedResolution.Add(BitConvertUtil.GetInt(sequence.DeQueue(20)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalFinePhaseRangeWithExtendedResolution.Add(BitConvertUtil.GetInt(sequence.DeQueue(24)));
            for (int i = 0; i < Ncell; i++)
                msg.PhaseRangeLockTimeIndicatorWithExtendedRangeAndResolution.Add(BitConvertUtil.GetUInt(sequence.DeQueue(10)));
            for (int i = 0; i < Ncell; i++)
                msg.HalfCycleAmbiguityIndicator.Add(BitConvertUtil.GetInt(sequence.DeQueue(1)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalCnrWithExtendedResolution.Add(BitConvertUtil.GetUInt(sequence.DeQueue(10)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalFinePhaseRangeRate.Add(BitConvertUtil.GetInt(sequence.DeQueue(15)));
            return msg;
        }
    }
}