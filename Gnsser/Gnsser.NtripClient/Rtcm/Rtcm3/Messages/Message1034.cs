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
    public class Message1034 : BaseRtcmMessage
    {
        public Message1034()
        {
           this.Length = 66;
        }

        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint GpsSatelliteID { get; set; }
        /// <summary>
        /// bit(8) 
        /// </summary>
        public uint GpsIODE { get; set; }
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

        public static Message1034 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1034 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1034 msg = new Message1034();
            msg.GpsSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.GpsIODE = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.N0 = BitConvertUtil.GetInt(sequence.DeQueue(12));
            msg.E0 = BitConvertUtil.GetInt(sequence.DeQueue(12));
            msg.NI = BitConvertUtil.GetInt(sequence.DeQueue(14));
            msg.EI = BitConvertUtil.GetInt(sequence.DeQueue(14));

            return msg;
        }
    }
}