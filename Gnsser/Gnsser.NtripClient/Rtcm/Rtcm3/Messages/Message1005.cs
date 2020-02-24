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
    /// 测站信息不具有天线高度
    /// 152 bit.
    /// Contents of the Type 1005 Message – Stationary Antenna Reference Point, No Height Information
    /// </summary>
    public class Message1005: BaseRtcmMessage
    {
        public Message1005()
        {
           this.Length = 152;
        }
        /// <summary>
        /// uint12 , 12
        /// </summary>
        //public int MessageNumber { get; set; }

        /// <summary>
        /// uint12 , 12
        /// </summary>
        public uint ReferenceStationID { get; set; }
        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint ReservedforITRFRealizationYear { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool GpsIndicator { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool GLONASSIndicator { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool ReservedforGalileoIndicator { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool ReferenceStationIndicator { get; set; }
        /// <summary>
        /// int38 ,38
        /// </summary>
        public double AntennaReferencePointECEF_X { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool SingleReceiverOscillatorIndicator { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool Reserved1{ get; set; }
        /// <summary>
        /// int38 ,38
        /// </summary>
        public double AntennaReferencePointECEF_Y { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool Reserved2 { get; set; }
        /// <summary>
        /// bit(2) ,2
        /// </summary>
        public uint quartercycleindicator { get; set; }///这样改对不对？是否采用int型，这里应该是怎么考虑？难道应该是写成Reserved2？
        /// <summary>
        /// int38 ,38
        /// </summary>
        public double AntennaReferencePointECEF_Z { get; set; } 

        public static Message1005 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1005 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1005 msg = new Message1005();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReservedforITRFRealizationYear = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6  
            msg.GpsIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            msg.GLONASSIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.ReservedforGalileoIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            msg.ReferenceStationIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.AntennaReferencePointECEF_X = BitConvertUtil.GetLong(sequence.DeQueue(38)) * 0.0001;//int38 
            msg.SingleReceiverOscillatorIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            msg.Reserved1 = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.AntennaReferencePointECEF_Y = BitConvertUtil.GetLong(sequence.DeQueue(38)) * 0.0001;//int38 
            //msg.Reserved2 = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.quartercycleindicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));//bit(2)
            msg.AntennaReferencePointECEF_Z = BitConvertUtil.GetLong(sequence.DeQueue(38)) * 0.0001;//int38 

            return msg;
        }
    }
}
