//2016.01.16, double, create in zhengzhou, RTCM 消息
//2016.11.19, double, edit in hongqing, 将Glonass SSR信息整合到一起

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 134bit. 
    /// </summary>
    public class Message1063 : Message1060
    {
        public Message1063()
        {
           this.Length = 134;
        }


        public new static Message1063 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public new static Message1063 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1063 msg = new Message1063();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.IODE = BitConvertUtil.GetUInt(sequence.DeQueue(8)); 
            msg.DeltaRadial = BitConvertUtil.GetInt(sequence.DeQueue(22)); 
            msg.DeltaAlongTrack = BitConvertUtil.GetInt(sequence.DeQueue(20));
            msg.DeltaCrossTrack = BitConvertUtil.GetInt(sequence.DeQueue(20));  
            msg.DotDeltaRadial = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DotDeltaAlongTrack = BitConvertUtil.GetInt(sequence.DeQueue(19));
            msg.DotDeltaCrossTrack = BitConvertUtil.GetInt(sequence.DeQueue(19)); 

            return msg;
        }
    }
    /// <summary>
    /// 75bit.
    /// </summary>
    public class Message1064 : Message1060
    {
        public Message1064()
        {
            this.Length = 75;
        }


        public new static Message1064 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public new static Message1064 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1064 msg = new Message1064();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.DeltaClockC0 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.DeltaClockC1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DeltaClockC2 = BitConvertUtil.GetInt(sequence.DeQueue(27));

            return msg;
        }
    }
    /// <summary>
    /// 29bit.
    /// Satellite Specific Part of the SSR Glonass Satellite Code Bias  10bit
    /// Code Specific Part of the SSR Glonass Satellite Code Bias   19bit
    /// </summary>
    public class Message1065 : BaseRtcmMessage
    {
        public Message1065()
        {
            this.Length = 29;
        }

        /// <summary>
        /// uint5
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint NoofCodeBiasProcess { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint GlonassSignalandTrackingModeIndicator { get; set; }
        /// <summary>
        /// int14
        /// </summary>
        public int CodeBias { get; set; }

        public static Message1065 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1065 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1065 msg = new Message1065();
            //Satellite Specific Part of the SSR Glonass Satellite Code Bias 
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.NoofCodeBiasProcess = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            //Code Spoecific Part of the SSR Glonass Satellite Code Bias 
            msg.GlonassSignalandTrackingModeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.CodeBias = BitConvertUtil.GetInt(sequence.DeQueue(14));

            return msg;
        }
    }
    /// <summary>
    /// 204bit. 
    /// </summary>
    public class Message1066 : Message1060
    {
        public Message1066()
        {
            this.Length = 204;
        }

        public new static Message1066 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public new static Message1066 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1066 msg = new Message1066();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.IODE = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.DeltaRadial = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.DeltaAlongTrack = BitConvertUtil.GetInt(sequence.DeQueue(20));
            msg.DeltaCrossTrack = BitConvertUtil.GetInt(sequence.DeQueue(20));
            msg.DotDeltaRadial = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DotDeltaAlongTrack = BitConvertUtil.GetInt(sequence.DeQueue(19));
            msg.DotDeltaCrossTrack = BitConvertUtil.GetInt(sequence.DeQueue(19));
            msg.DeltaClockC0 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.DeltaClockC1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DeltaClockC2 = BitConvertUtil.GetInt(sequence.DeQueue(27));

            return msg;
        }
    }
    /// <summary>
    /// 11bit. 
    /// Satellite Specific Part of the SSR Glonass URA
    /// </summary>
    public class Message1067 : BaseRtcmMessage
    {
        public Message1067()
        {
            this.Length = 11;
        }

        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// bit(6)
        /// </summary>
        public uint SSRURA { get; set; }

        public static Message1067 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1067 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1067 msg = new Message1067();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.SSRURA = BitConvertUtil.GetUInt(sequence.DeQueue(6));

            return msg;
        }
    }
    /// <summary>
    /// 27bit. 
    /// Satellite Specific Part of the SSR Glonass High Rate Clock
    /// </summary>
    public class Message1068 : BaseRtcmMessage
    {
        public Message1068()
        {
            this.Length = 27;
        }

        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public int HighRateClockCorrection { get; set; }

        public static Message1068 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1068 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1068 msg = new Message1068();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.HighRateClockCorrection = BitConvertUtil.GetInt(sequence.DeQueue(22));

            return msg;
        }
    }
}