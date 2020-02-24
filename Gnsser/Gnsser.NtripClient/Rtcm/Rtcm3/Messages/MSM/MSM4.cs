//2016.10.17, double, create in hongqing, RTCM MSM4

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// Compact Pseudoranges and PhaseRanges plus cnr
    /// </summary>
    public class MSM4 : BaseMSM
    {
        public MSM4()
        {
        }

        

        public static MSM4 Parse(List<byte> data, int Nsat, int Ncell)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr,  Nsat,  Ncell);
        }

        public static MSM4 Parse(string binStr, int Nsat, int Ncell)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            MSM4 msg = new MSM4();
            for (int i = 0; i < Nsat; i++)
                msg.NumberOfIntegerMsInSatRoughRange.Add(BitConvertUtil.GetUInt(sequence.DeQueue(8)));
            for (int i = 0; i < Nsat; i++)
                msg.SatelliteRoughRangesModulo1ms.Add(BitConvertUtil.GetUInt(sequence.DeQueue(10)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalFinePseudorange.Add(BitConvertUtil.GetInt(sequence.DeQueue(15)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalFinePhaseRange.Add(BitConvertUtil.GetInt(sequence.DeQueue(22)));
            for (int i = 0; i < Ncell; i++)
                msg.PhaseRangeLockTimeIndicator.Add(BitConvertUtil.GetUInt(sequence.DeQueue(4)));
            for (int i = 0; i < Ncell; i++)
                msg.HalfCycleAmbiguityIndicator.Add(BitConvertUtil.GetInt(sequence.DeQueue(1)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalCnr.Add(BitConvertUtil.GetUInt(sequence.DeQueue(6)));

            return msg;
        }
    }
}