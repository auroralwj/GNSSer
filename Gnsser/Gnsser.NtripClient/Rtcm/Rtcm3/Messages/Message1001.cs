﻿//2015.02.16, czs, create in pengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 58bit.
    /// Contents of the Satellite-Specific Portion of a Type 1001 Message, Each Satellite – GPS Basic RTK, L1 Only
    /// Type 1001 Message supports single-frequency RTK operation. It does not include an indication of the satellite 
    /// carrier-to noise ratio as measured by the reference station.
    /// </summary>
    public class Message1001 : BaseRtcmMessage
    {
        public Message1001()
        {
           this.Length = 58;
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

        public static Message1001 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1001 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1001 msg = new Message1001();
            msg.GpsSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6
            msg.GpsL1CodeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit（1） 
            msg.GpsL1Pseudorange = BitConvertUtil.GetUInt(sequence.DeQueue(24));//uint24 
            msg.GpsL1PhaseRangeMinusPseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//uint20 
            msg.GpsL1LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7  

            return msg;
        }
    }
}