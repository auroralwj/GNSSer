//2015.02.17, czs, create in pengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 64bit.
    /// Contents of the Satellite-Specific Portion of a Type 1009 Message, Each Satellite – Glonass Basic RTK, L1 Only
    /// </summary>
    public class Message1009: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary>
        public Message1009()
        {
           this.Length = 64;
        }
        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint GlonassSatelliteID { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool GlonassL1CodeIndicator { get; set; }
        /// <summary>
        /// uint5 ,5
        /// </summary>
        public uint GlonassSatelliteFrequencyChannelNumber { get; set; }
        /// <summary>
        /// uint,25
        /// </summary>
        public uint GlonassL1Pseudorange { get; set; }
        /// <summary>
        /// int20 ,20
        /// </summary>
        public int GlonassL1PhaseRangeMinusPseudorange { get; set; }
        /// <summary>
        /// uint7 ,7
        /// </summary>
        public uint GlonassL1LocktimeIndicator { get; set; }

        public static Message1009 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1009 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1009 msg = new Message1009();
            msg.GlonassSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6 
            msg.GlonassL1CodeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            msg.GlonassSatelliteFrequencyChannelNumber = BitConvertUtil.GetUInt(sequence.DeQueue(5));//uint5 
            msg.GlonassL1Pseudorange = BitConvertUtil.GetUInt(sequence.DeQueue(25));//uint25 
            msg.GlonassL1PhaseRangeMinusPseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20
            msg.GlonassL1LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7  

            return msg;
        }
    }
}