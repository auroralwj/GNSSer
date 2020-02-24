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
    /// 48+8*(M+N) bit.
    /// Contents of the Type 1008 Message – Antenna Descriptor &Serial Number
    /// </summary>
    public class Message1008 : BaseRtcmMessage
    {
        public Message1008()
        {
           //this.Length = 58;
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
        /// uint8 , 8
        /// </summary>
        public uint DescriptorCounterN { get; set; }
        /// <summary>
        /// char8(N),8*N, N ≤ 31 ,8 bit characters, ISO 8859-1 (not limited to ASCII)
        /// </summary>
        public string AntennaDescriptor  { get; set; }
         
        /// <summary>
        /// uint8 ,8
        /// </summary>
        public uint AntennaSetupID  { get; set; } 

        /// <summary>
        /// uint8 ,8
        /// </summary>
        public uint SerialNumberCounterM { get; set; }
        /// <summary>
        /// char8(N),8*N, N ≤ 31 ,8 bit characters, ISO 8859-1 (not limited to ASCII)
        /// </summary>
        public string AntennaSerialNumber { get; set; }

        public static Message1008 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1008 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1008 msg = new Message1008();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12  
            msg.DescriptorCounterN = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  

           int len =  (int)msg.DescriptorCounterN;
           var str = BitConvertUtil.GetCharString(sequence, len);//char8(N) 
            msg.AntennaDescriptor = str;

            msg.AntennaSetupID = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8   
            msg.SerialNumberCounterM = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8   


            msg.AntennaSerialNumber = BitConvertUtil.GetCharString(sequence, (int)msg.SerialNumberCounterM); //char8(M)   

            msg.Length = 48 + (int)msg.DescriptorCounterN * 8 + (int)msg.SerialNumberCounterM * 8;

            return msg;
        }

    }
}