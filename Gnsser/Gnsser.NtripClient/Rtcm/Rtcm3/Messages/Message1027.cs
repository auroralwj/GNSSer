//2016.01.16, double, create in zhengzhou, RTCM 消息
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 258 bit.
    /// Contents of the Type 1027 Message – Projection Message(Projection Types OM-Oblique Merator)
    /// </summary>
    public class Message1027: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1027() {
            this.MessageNumber = 1027;
            this.Length = 258;
        }

        /// <summary>
        /// uint8
        /// </summary>
        public uint SystemIdentificationNumber { get; set; } 

        /// <summary>
        /// uint6
        /// </summary>
        public uint ProjectionType { get; set; }

        /// <summary>
        /// bit(1)
        /// </summary>
        public bool RectificationFlag { get; set; } 
        /// <summary>
        /// int34
        /// </summary>
        public int LaPC { get; set; } 

        /// <summary>
        /// int35
        /// </summary>
        public int LoPC { get; set; }

        /// <summary>
        /// uint35
        /// </summary>
        public uint AzIL { get; set; }
        /// <summary>
        /// int26
        /// </summary>
        public int DiffARSG { get; set; }
        /// <summary>
        /// uint30
        /// </summary>
        public uint AddSIL { get; set; }
        /// <summary>
        /// uint36
        /// </summary>
        public uint EPC { get; set; }
        /// <summary>
        /// int35
        /// </summary>
        public int NPC { get; set; }

               
        public static Message1027 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1027 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1027 msg = new Message1027();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SystemIdentificationNumber = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.ProjectionType = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.RectificationFlag = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            msg.LaPC = BitConvertUtil.GetInt(sequence.DeQueue(34));
            msg.LoPC = BitConvertUtil.GetInt(sequence.DeQueue(35));
            msg.AzIL = BitConvertUtil.GetUInt(sequence.DeQueue(35));
            msg.DiffARSG = BitConvertUtil.GetInt(sequence.DeQueue(26));
            msg.AddSIL = BitConvertUtil.GetUInt(sequence.DeQueue(30));
            msg.EPC = BitConvertUtil.GetUInt(sequence.DeQueue(36));
            msg.NPC = BitConvertUtil.GetInt(sequence.DeQueue(35));
           
            return msg;
        }        
    } 
}