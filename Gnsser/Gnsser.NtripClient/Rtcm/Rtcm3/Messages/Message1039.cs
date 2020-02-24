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
    /// 53bit.
    /// </summary>
    public class Message1039 : Message1038
    {
        public Message1039()
        {
            this.Length = 53;
        }


        public static Message1039 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1039 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1039 msg = new Message1039();
            msg.GlonassSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.GlonassAmbiguityStatusFlag = BitConvertUtil.GetUInt(sequence.DeQueue(2)) ;
            msg.GlonassNonSyncCount = BitConvertUtil.GetUInt(sequence.DeQueue(3));
            msg.GlonassGeometricCarrierPhaseCorrectionDifference = BitConvertUtil.GetInt(sequence.DeQueue(17));
            msg.GlonassIOD = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.GlonassIonosphericCarrierPhaseCorrectionDifference = BitConvertUtil.GetInt(sequence.DeQueue(17)); 

            return msg;
        }
    }
}