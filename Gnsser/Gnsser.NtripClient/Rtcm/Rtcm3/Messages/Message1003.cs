//2015.02.16, czs, create in pengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 101 bit.
    /// Contents of the Satellite-Specific Portion of a Type 1003 Message, Each Satellite – GPS Extended RTK, L1 Only    /// </summary>
    public class Message1003: BaseRtcmMessage
    {
        public Message1003()
        {
           this.Length = 101;
        }
        /// <summary>
        /// uint6 , 6 
        /// </summary>
        public uint GpsSatelliteID { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool GpsL1CodeIndicator { get; set; }
        /// <summary>
        /// uint24 ,24
        /// </summary>
        public uint GpsL1Pseudorange { get; set; }
        /// <summary>
        /// int20 ,20
        /// </summary>
        public int GpsL1PhaseRangeMinusPseudorange { get; set; }
        /// <summary>
        /// uint7 ,7
        /// </summary>
        public uint GpsL1LocktimeIndicator { get; set; }
         
        /// <summary>
        /// bit2 ,2
        /// </summary>
        public uint GpsL2CodeIndicator { get; set; } 
        /// <summary>
        /// int14 ,14
        /// </summary>
        public int GpsL2MinusL1PseudorangeDifference { get; set; }
        /// <summary>
        /// int20 ,20
        /// </summary>
        public int GpsL2PhaseRangeMinusL1Pseudorange { get; set; }
        /// <summary>
        /// uint7 ,7
        /// </summary>
        public uint GpsL2LocktimeIndicator { get; set; }

        public static Message1003 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1003 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1003 msg = new Message1003();
            msg.GpsSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6
            msg.GpsL1CodeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.GpsL1Pseudorange = BitConvertUtil.GetUInt(sequence.DeQueue(24));//uint24 
            msg.GpsL1PhaseRangeMinusPseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20 
            msg.GpsL1LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7 
            msg.GpsL2CodeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));//bit(2)
            msg.GpsL2MinusL1PseudorangeDifference = BitConvertUtil.GetInt(sequence.DeQueue(14));//int14 
            msg.GpsL2PhaseRangeMinusL1Pseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20 
            msg.GpsL2LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7  

            return msg;
        }
    }
}