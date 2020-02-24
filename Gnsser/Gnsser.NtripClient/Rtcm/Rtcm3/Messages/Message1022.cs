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
    /// 517+8*(M+N) bit.
    /// Contents of the Type 1022 Message – Molodenski-Badekas Transformation 
    /// </summary>
    public class Message1022: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1022() {
            this.MessageNumber = 1022;
        }

        /// <summary>
        /// uint5
        /// </summary>
        public uint SourceNameCounter { get; set; }
        /// <summary>
        /// char8(N)
        /// </summary>
        public string SourceName { get; set; } 

        /// <summary>
        /// uint5
        /// </summary>
        public uint TargetNameCounter { get; set; }

        /// <summary>
        /// char8(M)
        /// </summary>
        public string TargetName { get; set; } 
        /// <summary>
        /// uint8
        /// </summary>
        public uint SystemIdentificationNumber { get; set; } 

        /// <summary>
        ///bit(10)
        /// </summary>
        public uint UtilizedTransformationMessageIndicator { get; set; }

        /// <summary>
        /// uint5
        /// </summary>
        public uint PlateNumber { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public uint ComputationIndicator { get; set; }
        /// <summary>
        /// uint2
        /// </summary>
        public uint HeightIndicator { get; set; }
        /// <summary>
        /// int19
        /// </summary>
        public int LatitudeOfOrigin { get; set; }
        /// <summary>
        /// int20
        /// </summary>
        public int LongitudeOfOrigin { get; set; }

        /// <summary>
        /// uint14
        /// </summary>
        public uint deltaLatitudeOfOrigin { get; set; }

        /// <summary>
        /// uint14
        /// </summary>
        public uint deltaLongitudeOfOrigin { get; set; }

        /// <summary>
        /// int23
        /// </summary>        
        public int dX { get; set; }
        /// <summary>
        /// int23
        /// </summary>
        public int dY { get; set; }
        /// <summary>
        /// int23
        /// </summary>
        public int dZ { get; set; }

        /// <summary>
        /// int32
        /// </summary>
        public int R1 { get; set; }
        /// <summary>
        /// int32
        /// </summary>
        public int R2 { get; set; }
        /// <summary>
        /// int32
        /// </summary>
        public int R3 { get; set; }

        /// <summary>
        /// int25
        /// </summary>
        public int dS { get; set; }
        /// <summary>
        /// int35
        /// </summary>
        public int XP { get; set; }
        /// <summary>
        /// int35
        /// </summary>
        public int YP { get; set; }

        /// <summary>
        /// int35
        /// </summary>
        public int ZP { get; set; }
        /// <summary>
        /// uint24
        /// </summary>
        public uint addaS { get; set; }

        /// <summary>
        /// uint25
        /// </summary>
        public uint addbS { get; set; }
        /// <summary>
        /// uint24
        /// </summary>
        public uint addaT { get; set; }
        /// <summary>
        /// uint25
        /// </summary>
        public uint addbT { get; set; }
        /// <summary>
        /// uint3
        /// </summary>
        public uint HorizontalHelmertMolodenskiQualityIndicator { get; set; }
        /// <summary>
        /// uint3
        /// </summary>
        public uint VerticalHelmertMolodenskiQualityIndicator { get; set; }
       
        public static Message1022 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1022 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1022 msg = new Message1022();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SourceNameCounter = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.SourceName = BitConvertUtil.GetCharString(sequence, (int)msg.SourceNameCounter);
            msg.TargetNameCounter = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.TargetName = BitConvertUtil.GetCharString(sequence, (int)msg.TargetNameCounter);
            msg.SystemIdentificationNumber = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.UtilizedTransformationMessageIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(10));
            msg.PlateNumber = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.ComputationIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.HeightIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.LatitudeOfOrigin = BitConvertUtil.GetInt(sequence.DeQueue(19));
            msg.LongitudeOfOrigin = BitConvertUtil.GetInt(sequence.DeQueue(20));
            msg.deltaLatitudeOfOrigin = BitConvertUtil.GetUInt(sequence.DeQueue(14));
            msg.deltaLongitudeOfOrigin = BitConvertUtil.GetUInt(sequence.DeQueue(14));
            msg.dX = BitConvertUtil.GetInt(sequence.DeQueue(23));
            msg.dY = BitConvertUtil.GetInt(sequence.DeQueue(23));
            msg.dZ = BitConvertUtil.GetInt(sequence.DeQueue(23));
            msg.R1 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            msg.R2 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            msg.R3 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            msg.dS = BitConvertUtil.GetInt(sequence.DeQueue(25));
            msg.XP = BitConvertUtil.GetInt(sequence.DeQueue(35));
            msg.YP = BitConvertUtil.GetInt(sequence.DeQueue(35));
            msg.ZP = BitConvertUtil.GetInt(sequence.DeQueue(35));
            msg.addaS = BitConvertUtil.GetUInt(sequence.DeQueue(24));
            msg.addbS = BitConvertUtil.GetUInt(sequence.DeQueue(25));
            msg.addaT = BitConvertUtil.GetUInt(sequence.DeQueue(24));
            msg.addbT = BitConvertUtil.GetUInt(sequence.DeQueue(25));
            msg.HorizontalHelmertMolodenskiQualityIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(3));
            msg.VerticalHelmertMolodenskiQualityIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(3));

            msg.Length = 517 + (int)msg.SourceNameCounter * 8 + (int)msg.TargetNameCounter * 8;
            return msg;
        }        
    } 
}