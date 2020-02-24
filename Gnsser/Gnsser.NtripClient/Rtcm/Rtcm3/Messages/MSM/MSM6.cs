//2016.10.17, double, create in hongqing, RTCM MSM6

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    ///  Compact Pseudoranges , PhaseRanges and cnr (high resolution)
    /// </summary>
    public class MSM6 : BaseMSM
    {
        public MSM6()
        {
        }
        
        public static MSM6 Parse(List<byte> data, int Nsat, int Ncell)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr,  Nsat,  Ncell);
        }

        public static MSM6 Parse(string binStr, int Nsat, int Ncell)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            MSM6 msg = new MSM6();

            for (int i = 0; i < Nsat; i++)
                msg.NumberOfIntegerMsInSatRoughRange.Add(BitConvertUtil.GetUInt(sequence.DeQueue(8)));
            for (int i = 0; i < Nsat; i++)
                msg.SatelliteRoughRangesModulo1ms.Add(BitConvertUtil.GetUInt(sequence.DeQueue(10)));
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

            return msg;
        }
    }
}