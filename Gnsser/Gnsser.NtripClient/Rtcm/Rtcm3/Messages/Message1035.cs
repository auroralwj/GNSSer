//2016.01.18, double, create in zhengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 66bit.
    /// </summary>
    public class Message1035 : BaseRtcmMessage
    {
        public Message1035()
        {
           this.Length = 66;
        }

        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint GlonassSatelliteID { get; set; }
        /// <summary>
        /// bit(8) 
        /// </summary>
        public uint GlonassIOD { get; set; }
        /// <summary>
        /// int12
        /// </summary>
        public int N0 { get; set; }
        /// <summary>
        /// int12
        /// </summary>
        public int E0 { get; set; }
        /// <summary>
        /// int14
        /// </summary>
        public int NI { get; set; }
        /// <summary>
        /// int14
        /// </summary>
        public int EI { get; set; }

        public static Message1035 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1035 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1035 msg = new Message1035();
            msg.GlonassSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.GlonassIOD = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.N0 = BitConvertUtil.GetInt(sequence.DeQueue(12));
            msg.E0 = BitConvertUtil.GetInt(sequence.DeQueue(12));
            msg.NI = BitConvertUtil.GetInt(sequence.DeQueue(14));
            msg.EI = BitConvertUtil.GetInt(sequence.DeQueue(14));

            return msg;
        }
    }
}