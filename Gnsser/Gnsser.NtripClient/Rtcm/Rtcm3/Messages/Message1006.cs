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
    /// 测站信息具有天线高度
    /// 168 bit.
    ///Contents of the Type 1006 Message – Stationary Antenna Reference Point, with Height Information
    ///测站信息
    /// </summary>
    public class Message1006 : Message1005 
    {
        public Message1006()
        {
           this.Length = 168;
        }
        /// <summary>
        /// uint16,16
        /// </summary>
        public double AntennaHeight { get; set; } 

        public static Message1006 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1006 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1006 msg = new Message1006();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReservedforITRFRealizationYear = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6 
            msg.GpsIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.GLONASSIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.ReservedforGalileoIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.ReferenceStationIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            //msg.AntennaReferencePointECEF_X = BitConvertUtil.GetLong(sequence.DeQueue(38));//int38 
            msg.AntennaReferencePointECEF_X = BitConvertUtil.GetLong(sequence.DeQueue(38)) * 0.0001;//int38 
            msg.SingleReceiverOscillatorIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            msg.Reserved1 = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            //msg.AntennaReferencePointECEF_Y = BitConvertUtil.GetLong(sequence.DeQueue(38));//int38
            msg.AntennaReferencePointECEF_Y = BitConvertUtil.GetLong(sequence.DeQueue(38)) * 0.0001;//int38
            //msg.Reserved2 = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.quartercycleindicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));//bit(2)
            //msg.AntennaReferencePointECEF_Z = BitConvertUtil.GetLong(sequence.DeQueue(38));//int38 
            msg.AntennaReferencePointECEF_Z = BitConvertUtil.GetLong(sequence.DeQueue(38)) * 0.0001;//int38 
            msg.AntennaHeight = BitConvertUtil.GetUInt(sequence.DeQueue(16)) * 0.0001 ;//uint16  

            return msg;
        }
    }
}