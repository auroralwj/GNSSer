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
    /// 196 bit.
    /// Contents of the Type 1025 Message – Projection Message(Projection Types except LCC2SP, OM)
    /// </summary>
    public class Message1025 : BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1025()
        {
            this.MessageNumber = 1025;
            this.Length = 196;
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
        public int LaNO { get; set; }

        /// <summary>
        /// int35
        /// </summary>
        public int LoNO { get; set; }

        /// <summary>
        /// uint30
        /// </summary>
        public uint addSNO { get; set; }
        /// <summary>
        /// uint36
        /// </summary>
        public uint FE { get; set; }
        /// <summary>
        /// int35
        /// </summary>
        public int FN { get; set; }


        public static Message1025 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1025 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1025 msg = new Message1025();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SystemIdentificationNumber = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.ProjectionType = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.LaNO = BitConvertUtil.GetInt(sequence.DeQueue(34));
            msg.LoNO = BitConvertUtil.GetInt(sequence.DeQueue(35));
            msg.addSNO = BitConvertUtil.GetUInt(sequence.DeQueue(30));
            msg.FE = BitConvertUtil.GetUInt(sequence.DeQueue(36));
            msg.FN = BitConvertUtil.GetInt(sequence.DeQueue(35));

            return msg;
        }
    }
}