//2016.10.17, double, create in hongqing, RTCM MSM1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// Compact Pseudoranges
    /// </summary>
    public class MSM1 : BaseMSM
    {
        public MSM1()
        {
        }


        public static MSM1 Parse(List<byte> data,int Nsat,int Ncell)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr, Nsat, Ncell);
        }

        public static MSM1 Parse(string binStr,int Nsat,int Ncell)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            MSM1 msg = new MSM1();
            for (int i = 0; i < Nsat;i++ )
                msg.SatelliteRoughRangesModulo1ms.Add( BitConvertUtil.GetUInt(sequence.DeQueue(10)));
            for (int i = 0; i < Ncell; i++)
                msg.SignalFinePseudorange.Add(BitConvertUtil.GetInt(sequence.DeQueue(15)));
            
            return msg;
        }
    }
}