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
    /// 40+8*N  bit.
    ///Contents of the Type 1007 Message – Antenna Descriptor 
    /// </summary>
    public class Message1007 : BaseRtcmMessage
    {
        public Message1007()
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

        public static Message1007 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1007 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1007 msg = new Message1007();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12 
            msg.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));//uint12  
            msg.DescriptorCounterN = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < (int)msg.DescriptorCounterN; i++)
            {
                var ch = BitConvertUtil.GetChar(sequence.DeQueue(8));//char8  
                sb.Append(ch);
            }//char8(N)
            msg.AntennaDescriptor = sb.ToString();

            msg.AntennaSetupID = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8   
            msg.Length =  40 + (int)msg.DescriptorCounterN * 8;
            return msg;
        }
    }
}