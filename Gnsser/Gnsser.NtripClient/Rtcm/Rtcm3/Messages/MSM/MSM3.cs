//2016.10.17, double, create in hongqing, RTCM MSM3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// Compact Pseudoranges and PhaseRanges
    /// </summary>
    public class MSM3 : BaseMSM
    {
        public MSM3()
        {
        }
        

        public static MSM3 Parse(List<byte> data, int Nsat, int Ncell)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr, Nsat, Ncell);
        }

        public static MSM3 Parse(string binStr, int Nsat, int Ncell)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            MSM3 msg = new MSM3();

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
            



            return msg;
        }
    }
}