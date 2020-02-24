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
    /// 590 bit.
    /// Contents of the Type 1024 Message – Residual Message, Plane Grid Representation
    /// </summary>
    public class Message1024: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1024() {
            this.MessageNumber = 1024;
            this.Length = 590;
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
        /// int25
        /// </summary>
        public int N0 { get; set; } 

        /// <summary>
        /// uint26
        /// </summary>
        public uint E0 { get; set; }

        /// <summary>
        /// uint12
        /// </summary>
        public uint DeltaN { get; set; }
        /// <summary>
        /// uint12
        /// </summary>
        public uint DeltaE { get; set; }
        /// <summary>
        /// int10
        /// </summary>
        public int MeanDeltaN { get; set; }
        /// int10
        /// </summary>
        public int MeanDeltaE { get; set; }
        /// <summary>
        /// int15
        /// </summary>
        public int MeanDeltah { get; set; }

        /// <summary>
        /// int9
        /// </summary>
        public int SegmaN { get; set; }
        /// <summary>
        /// int9
        /// </summary>
        public int SegmaE { get; set; }
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
        
       
        public static Message1024 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1024 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1024 msg = new Message1024();
            msg.SystemIdentificationNumber = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.HorizontalShiftIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1))==1;
            msg.VerticalShiftIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1))==1;
            msg.N0 = BitConvertUtil.GetInt(sequence.DeQueue(25));
            msg.E0 = BitConvertUtil.GetUInt(sequence.DeQueue(26));
            msg.DeltaN = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.DeltaE = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.MeanDeltaN = BitConvertUtil.GetInt(sequence.DeQueue(10));
            msg.MeanDeltaE = BitConvertUtil.GetInt(sequence.DeQueue(10));
            msg.MeanDeltah = BitConvertUtil.GetInt(sequence.DeQueue(15));       
           for(int i=0;i<16;i++)
            {
                msg.SegmaN = BitConvertUtil.GetInt(sequence.DeQueue(9));
                msg.SegmaE = BitConvertUtil.GetInt(sequence.DeQueue(9));
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