
//2016.12.4, double, create in hongqing, 将QZSS SSR信息整合到一起

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
    public class Message1246 : Message1060
    {
        public Message1246()
        {
           this.Length = 133;
        }
        public new static Message1246 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1246 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1246 msg = new Message1246();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
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
    /// 74bit.
    /// </summary>
    public class Message1247 : Message1060
    {
        public Message1247()
        {
            this.Length = 76;
        }
        public new static Message1247 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1247 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1247 msg = new Message1247();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.DeltaClockC0 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.DeltaClockC1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DeltaClockC2 = BitConvertUtil.GetInt(sequence.DeQueue(27));

            return msg;
        }
    }
    /// <summary>
    /// 30bit.
    /// Satellite Specific Part of the SSR QZSS Satellite Code Bias  11bit
    /// Code Specific Part of the SSR QZSS Satellite Code Bias   19bit
    /// </summary>
    public class Message1248 : BaseRtcmMessage
    {
        public Message1248()
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

        public static Message1248 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1248 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1248 msg = new Message1248();
            //Satellite Specific Part of the SSR QZSS Satellite Code Bias 
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.NoofCodeBiasProcess = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            //Code Spoecific Part of the SSR QZSS Satellite Code Bias 
            msg.SignalandTrackingModeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.CodeBias = BitConvertUtil.GetInt(sequence.DeQueue(14));

            return msg;
        }
    }
    /// <summary>
    /// 正常情况下的观测值，没有压缩
    /// </summary>
    public class NormalMessage1249 : NormalSSRMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Message1261"></param>
        public NormalMessage1249(Message1249 Message1249)
        {
            this.Message1249 = Message1249;
        }

        #region 核心属性

        /// <summary>
        /// 报文 1261.
        /// </summary>
        public Message1249 Message1249 { get; set; }
        #endregion

        /// <summary>
        /// 测站编号
        /// </summary>
        public uint ReferenceStationID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        // public Time Time { get { return new Time(DateTime.UtcNow, Header.QZSSEpochTimeInMs / 1000.0); } }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get { return new SatelliteNumber((int)Message1249.SatelliteID, SatelliteType.J); } }
    }
    /// <summary>
    /// 203bit. 
    /// </summary>
    public class Message1249 : Message1060
    {
        public Message1249()
        {
            this.Length = 205;
        }

        public new static Message1249 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public new static Message1249 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1249 msg = new Message1249();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
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
    /// 10bit. 
    /// Satellite Specific Part of the SSR QZSS URA
    /// </summary>
    public class Message1250 : BaseRtcmMessage
    {
        public Message1250()
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

        public static Message1250 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1250 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1250 msg = new Message1250();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.SSRURA = BitConvertUtil.GetUInt(sequence.DeQueue(6));

            return msg;
        }
    }
    /// <summary>
    /// 26bit. 
    /// Satellite Specific Part of the SSR QZSS High Rate Clock
    /// </summary>
    public class Message1251 : BaseRtcmMessage
    {
        public Message1251()
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

        public static Message1251 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1251 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1251 msg = new Message1251();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.HighRateClockCorrection = BitConvertUtil.GetInt(sequence.DeQueue(22));

            return msg;
        }
    }
}