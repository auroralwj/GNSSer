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
    /// 234 bit.
    /// Contents of the Type 1026 Message – Projection Message(Projection Types LCC2SP-Lambert Conic Conformal(2 SP))
    /// </summary>
    public class Message1026 : BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1026()
        {
            this.MessageNumber = 1026;
            this.Length = 234;
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
        /// int34
        /// </summary>
        public int LaFO { get; set; }

        /// <summary>
        /// int35
        /// </summary>
        public int LoFO { get; set; }

        /// <summary>
        /// int34
        /// </summary>
        public int LaSP1 { get; set; }
        /// <summary>
        /// int34
        /// </summary>
        public int LaSP2 { get; set; }
        /// <summary>
        /// uint36
        /// </summary>
        public uint EFO { get; set; }      
        /// <summary>
        /// int35
        /// </summary>
        public int NFO { get; set; }


        public static Message1026 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1026 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1026 msg = new Message1026();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SystemIdentificationNumber = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.ProjectionType = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.LaFO = BitConvertUtil.GetInt(sequence.DeQueue(34));
            msg.LoFO = BitConvertUtil.GetInt(sequence.DeQueue(35));
            msg.LaSP1 = BitConvertUtil.GetInt(sequence.DeQueue(34));
            msg.LaSP2 = BitConvertUtil.GetInt(sequence.DeQueue(34));
            msg.EFO = BitConvertUtil.GetUInt(sequence.DeQueue(36));
            msg.NFO = BitConvertUtil.GetInt(sequence.DeQueue(35));

            return msg;
        }
    }
}