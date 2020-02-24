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
    /// 36bit.
    /// </summary>
    public class Message1038 : Message1037
    {
        public Message1038()
        {
            this.Length = 36;
        }

        /// <summary>
        /// int17
        /// </summary>
        public int GlonassGeometricCarrierPhaseCorrectionDifference { get; set; }
        /// <summary>
        /// bit(8)
        /// </summary>
        public uint GlonassIOD { get; set; }

        public static Message1038 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1038 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1038 msg = new Message1038();
            msg.GlonassSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.GlonassAmbiguityStatusFlag = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.GlonassNonSyncCount = BitConvertUtil.GetUInt(sequence.DeQueue(3));
            msg.GlonassGeometricCarrierPhaseCorrectionDifference = BitConvertUtil.GetInt(sequence.DeQueue(17));
            msg.GlonassIOD = BitConvertUtil.GetUInt(sequence.DeQueue(8)); 

            return msg;
        }
    }
}