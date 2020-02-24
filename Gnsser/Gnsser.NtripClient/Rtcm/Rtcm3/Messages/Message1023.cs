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
    /// 578 bit.
    /// Contents of the Type 1023 Message – Residual Message, Ellipsoidal Grid Representation
    /// </summary>
    public class Message1023 : BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1023()
        {
            this.MessageNumber = 1023;
            this.Length = 578;
        }

        /// <summary>
        /// uint8
        /// </summary>
        public uint SystemIdentificationNumber { get; set; }

        /// <summary>
        /// bit(1)
        /// </summary>
        public bool HorizontalShiftIndicator { get; set; }

        /// <summary>
        /// bit(1)
        /// </summary>
        public bool VerticalShiftIndicator { get; set; }
        /// <summary>
        /// int21
        /// </summary>
        public int fai0 { get; set; }

        /// <summary>
        /// int22
        /// </summary>
        public int lamda0 { get; set; }

        /// <summary>
        /// uint12
        /// </summary>
        public uint Deltafai { get; set; }
        /// <summary>
        /// uint12
        /// </summary>
        public uint Deltalamda { get; set; }
        /// <summary>
        /// int8
        /// </summary>
        public int MeanDeltafai { get; set; }
        /// <summary>
        /// int8
        /// </summary>
        public int MeanDeltalamda { get; set; }
        /// int15
        /// </summary>
        public int MeanDeltaH { get; set; }
        /// <summary>
        /// int9
        /// </summary>
        public int Segmafai { get; set; }
        /// <summary>
        /// int9
        /// </summary>
        public int Segmalamda { get; set; }
        /// <summary>
        /// int9
        /// </summary>
        public int Segmah { get; set; }
        /// <summary>
        /// uint2
        /// </summary>
        public uint HorizontalInterpolationMethodIndicator { get; set; }

        /// <summary>
        /// uint2
        /// </summary>       
        public uint VerticalInterpolationMethodIndicator { get; set; }
        /// <summary>
        /// uint3
        /// </summary>
        public uint HorizontalGridQualityIndicator { get; set; }
        /// <summary>
        /// uint3
        /// </summary>
        public uint VerticalGridQualityIndicator { get; set; }

        /// <summary>
        /// uint16
        /// </summary>
        public uint MJDNumber { get; set; }


        public static Message1023 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1023 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1023 msg = new Message1023();
            msg.SystemIdentificationNumber = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.HorizontalShiftIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            msg.VerticalShiftIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            msg.fai0 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.lamda0 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.Deltafai = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.Deltalamda = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.MeanDeltafai = BitConvertUtil.GetInt(sequence.DeQueue(8));
            msg.MeanDeltalamda = BitConvertUtil.GetInt(sequence.DeQueue(8));
            msg.MeanDeltaH = BitConvertUtil.GetInt(sequence.DeQueue(15));
            for (int i = 0; i < 16; i++)
            {
                msg.Segmafai = BitConvertUtil.GetInt(sequence.DeQueue(9));
                msg.Segmalamda = BitConvertUtil.GetInt(sequence.DeQueue(9));
                msg.Segmah = BitConvertUtil.GetInt(sequence.DeQueue(9));
            }
            msg.HorizontalInterpolationMethodIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.VerticalInterpolationMethodIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.HorizontalGridQualityIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(3));
            msg.VerticalGridQualityIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(3));
            msg.MJDNumber = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            return msg;
        }
    }
}