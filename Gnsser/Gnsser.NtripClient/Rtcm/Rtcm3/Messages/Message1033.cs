//2015.02.19, czs, create in pengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 接收机与天线信息描述
    /// 72+8*(M+N+I+J+K) bit.
    /// Contents of the Type 1033 Message – Receiver and Antenna Descriptors
    /// </summary>
    public class Message1033 : BaseRtcmMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Message1033() {
        } 
        /// <summary>
        /// uint12
        /// </summary>
        public uint ReferenceStationID { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint AntennaDescriptorCounterN { get; set; }

        /// <summary>
        /// char(N)
        /// </summary>
        public string AntennaDescriptor { get; set; }

        /// <summary>
        /// uint8
        /// </summary>
        public uint AntennaSetupID { get; set; }

        /// <summary>
        /// uint8
        /// </summary>
        public uint AntennaSerialNumberCounterM { get; set; }
        /// <summary>
        /// char(M)
        /// </summary>
        public string AntennaSerialNumber { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint ReceiverTypeDescriptorCounterI { get; set; }
        /// <summary>
        /// char(I)
        /// </summary>
        public string ReceiverTypeDescriptor { get; set; } 
        /// <summary>
        /// uint8
        /// </summary>
        public uint ReceiverFirmwareVersionCounterJ { get; set; }
        /// <summary>
        /// char(J)
        /// </summary>
        public string ReceiverFirmwareVersion { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint ReceiverSerialNumberCounterK { get; set; }
        /// <summary>
        /// char(K)
        /// </summary>
        public string ReceiverSerialNumber { get; set; }  

        public static Message1033 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1033 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1033 msg = new Message1033();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.AntennaDescriptorCounterN = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  
            msg.AntennaDescriptor = GetString(sequence, (int)msg.AntennaDescriptorCounterN); //char 8*N
            msg.AntennaSetupID = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  
            msg.AntennaSerialNumberCounterM = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  
            msg.AntennaSerialNumber = GetString(sequence, (int)msg.AntennaSerialNumberCounterM);//char 8*M
            msg.ReceiverTypeDescriptorCounterI = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8
            msg.ReceiverTypeDescriptor = GetString(sequence, (int)msg.ReceiverTypeDescriptorCounterI);//char 8*I
            msg.ReceiverFirmwareVersionCounterJ = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8
            msg.ReceiverFirmwareVersion = GetString(sequence, (int)msg.ReceiverFirmwareVersionCounterJ);//char 8*J
            msg.ReceiverSerialNumberCounterK = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8
            msg.ReceiverSerialNumber = GetString(sequence, (int)msg.ReceiverSerialNumberCounterK);//char 8*K

           msg.Length =  72 +
                       ((int)msg.AntennaDescriptorCounterN
                       + (int)msg.AntennaSerialNumberCounterM
                       + (int)msg.ReceiverTypeDescriptorCounterI
                       + (int)msg.ReceiverFirmwareVersionCounterJ
                       + (int)msg.ReceiverSerialNumberCounterK
                       )
                       * 8; 

            return msg;
        }

        private static string GetString(StringSequence sequence, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                var ch = BitConvertUtil.GetChar(sequence.DeQueue(8));//uint12  
                sb.Append(ch);
            }
            var str = sb.ToString();
            return str;
        }

        
    }
     
}