//2015.02.16, czs, create in pengzhou, RTCM 消息
//2015.10.14, double, Add "msg.GpsL1LocktimeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(7))". Modify some comments. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 74 bit.
    /// Contents of the Satellite-Specific Portion of a Type 1002 Message, Each Satellite – GPS Extended RTK, L1 Only   
    /// </summary>
    public class Message1002: BaseRtcmMessage
    {
        public Message1002()
        {
           this.Length = 74;
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
        /// uint8 ,8
        /// </summary>
        public uint GpsIntegerL1PseudorangeModulusAmbiguity { get; set; }
        /// <summary>
        /// uint8 ,8
        /// </summary>
        public uint GpsL1Cnr { get; set; }

        public static Message1002 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1002 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1002 msg = new Message1002();
            msg.GpsSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6
            msg.GpsL1CodeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            msg.GpsL1Pseudorange = BitConvertUtil.GetUInt(sequence.DeQueue(24));//uint24 
            msg.GpsL1PhaseRangeMinusPseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20 
            //msg.GpsL1LocktimeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(7));//uint7
            msg.GpsIntegerL1PseudorangeModulusAmbiguity = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  
            msg.GpsL1Cnr = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  

            return msg;
        }
    }
}