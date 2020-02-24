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
    /// 28bit.
    /// </summary>
    public class Message1037 : BaseRtcmMessage
    {
        public Message1037()
        {
           this.Length = 28;
        }

        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint GlonassSatelliteID { get; set; }
        /// <summary>
        /// bit(2)
        /// </summary>
        public uint GlonassAmbiguityStatusFlag { get; set; }
        /// <summary>
        /// uint3
        /// </summary>
        public uint GlonassNonSyncCount { get; set; }
        /// <summary>
        /// int17
        /// </summary>
        public int GlonassIonosphericCarrierPhaseCorrectionDifference { get; set; }

        public static Message1037 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1037 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1037 msg = new Message1037();
            msg.GlonassSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.GlonassAmbiguityStatusFlag = BitConvertUtil.GetUInt(sequence.DeQueue(2)); 
            msg.GlonassNonSyncCount = BitConvertUtil.GetUInt(sequence.DeQueue(3));
            msg.GlonassIonosphericCarrierPhaseCorrectionDifference = BitConvertUtil.GetInt(sequence.DeQueue(17)); 

            return msg;
        }
    }
}