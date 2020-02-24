
//2016.11.19, double, create in hongqing, 将BeiDou SSR信息整合到一起

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 135bit. 
    /// </summary>
    public class Message1258 : Message1060
    {
        public Message1258()
        {
           this.Length = 135;
        }
        public new static Message1258 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1258 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1258 msg = new Message1258();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.IODE = BitConvertUtil.GetUInt(sequence.DeQueue(10));
            msg.IodCrc = BitConvertUtil.GetUInt(sequence.DeQueue(24));
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
    /// 76bit.
    /// </summary>
    public class Message1259 : Message1060
    {
        public Message1259()
        {
            this.Length = 76;
        }
        public new static Message1259 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1259 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1259 msg = new Message1259();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.DeltaClockC0 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.DeltaClockC1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DeltaClockC2 = BitConvertUtil.GetInt(sequence.DeQueue(27));

            return msg;
        }
    }
    /// <summary>
    /// 30bit.
    /// Satellite Specific Part of the SSR BeiDou Satellite Code Bias  11bit
    /// Code Specific Part of the SSR BeiDou Satellite Code Bias   19bit
    /// </summary>
    public class Message1260 : BaseRtcmMessage
    {
        public Message1260()
        {
            this.Length = 30;
        }
        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint NoofCodeBiasProcess { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint SignalandTrackingModeIndicator { get; set; }
        /// <summary>
        /// int14
        /// </summary>
        public int CodeBias { get; set; }

        public static Message1260 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1260 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1260 msg = new Message1260();
            //Satellite Specific Part of the SSR BeiDou Satellite Code Bias 
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.NoofCodeBiasProcess = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            //Code Spoecific Part of the SSR BeiDou Satellite Code Bias 
            msg.SignalandTrackingModeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.CodeBias = BitConvertUtil.GetInt(sequence.DeQueue(14));

            return msg;
        }
    }
    /// <summary>
    /// 正常情况下的观测值，没有压缩
    /// </summary>
    public class NormalMessage1261 : NormalSSRMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Message1261"></param>
        public NormalMessage1261(Message1261 Message1261)
        {
            this.Message1261 = Message1261;
        }

        #region 核心属性

        /// <summary>
        /// 报文 1261.
        /// </summary>
        public Message1261 Message1261 { get; set; }
        #endregion

        /// <summary>
        /// 测站编号
        /// </summary>
        public uint ReferenceStationID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        // public Time Time { get { return new Time(DateTime.UtcNow, Header.BeiDouEpochTimeInMs / 1000.0); } }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get { return new SatelliteNumber((int)Message1261.SatelliteID, SatelliteType.C); } }
    }
    /// <summary>
    /// 205bit. 
    /// </summary>
    public class Message1261 : Message1060
    {
        public Message1261()
        {
            this.Length = 205;
        }

        public new static Message1261 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1261 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1261 msg = new Message1261();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.IODE = BitConvertUtil.GetUInt(sequence.DeQueue(10));
            msg.IodCrc = BitConvertUtil.GetUInt(sequence.DeQueue(24));
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
    /// 12bit. 
    /// Satellite Specific Part of the SSR BeiDou URA
    /// </summary>
    public class Message1262 : BaseRtcmMessage
    {
        public Message1262()
        {
            this.Length = 12;
        }
        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// bit(6)
        /// </summary>
        public uint SSRURA { get; set; }

        public static Message1262 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1262 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1262 msg = new Message1262();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.SSRURA = BitConvertUtil.GetUInt(sequence.DeQueue(6));

            return msg;
        }
    }
    /// <summary>
    /// 28bit. 
    /// Satellite Specific Part of the SSR BeiDou High Rate Clock
    /// </summary>
    public class Message1263 : BaseRtcmMessage
    {
        public Message1263()
        {
            this.Length = 28;
        }
        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public int HighRateClockCorrection { get; set; }

        public static Message1263 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1263 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1263 msg = new Message1263();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.HighRateClockCorrection = BitConvertUtil.GetInt(sequence.DeQueue(22));

            return msg;
        }
    }
}