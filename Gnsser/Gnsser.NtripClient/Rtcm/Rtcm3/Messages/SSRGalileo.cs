//2016.12.3, double, create in hongqing, 将Galileo SSR信息整合到一起

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
    public class Message1240 : Message1060
    {
        public Message1240()
        {
           this.Length = 135;
        }
        public new static Message1240 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1240 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1240 msg = new Message1240();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.IODE = BitConvertUtil.GetUInt(sequence.DeQueue(10)); 
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
    public class Message1241 : Message1060
    {
        public Message1241()
        {
            this.Length = 76;
        }
        public new static Message1241 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1241 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1241 msg = new Message1241();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.DeltaClockC0 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.DeltaClockC1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DeltaClockC2 = BitConvertUtil.GetInt(sequence.DeQueue(27));

            return msg;
        }
    }
    /// <summary>
    /// 30bit.
    /// Satellite Specific Part of the SSR Galileo Satellite Code Bias  11bit
    /// Code Specific Part of the SSR Galileo Satellite Code Bias   19bit
    /// </summary>
    public class Message1242 : BaseRtcmMessage
    {
        public Message1242()
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

        public static Message1242 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1242 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1242 msg = new Message1242();
            //Satellite Specific Part of the SSR Galileo Satellite Code Bias 
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.NoofCodeBiasProcess = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            //Code Spoecific Part of the SSR Galileo Satellite Code Bias 
            msg.SignalandTrackingModeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.CodeBias = BitConvertUtil.GetInt(sequence.DeQueue(14));

            return msg;
        }
    }
    /// <summary>
    /// 正常情况下的观测值，没有压缩
    /// </summary>
    public class NormalMessage1243 : NormalSSRMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Message1238"></param>
        public NormalMessage1243(Message1243 Message1243)
        {
            this.Message1243 = Message1243;
        }

        #region 核心属性

        /// <summary>
        /// 报文 1244.
        /// </summary>
        public Message1243 Message1243 { get; set; }
        #endregion

        /// <summary>
        /// 测站编号
        /// </summary>
        public uint ReferenceStationID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        // public Time Time { get { return new Time(DateTime.UtcNow, Header.GalileoEpochTimeInMs / 1000.0); } }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get { return new SatelliteNumber((int)Message1243.SatelliteID, SatelliteType.E); } }
    }
    /// <summary>
    /// 205bit. 
    /// </summary>
    public class Message1243 : Message1060
    {
        public Message1243()
        {
            this.Length = 205;
        }

        public new static Message1243 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public new static Message1243 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1243 msg = new Message1243();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.IODE = BitConvertUtil.GetUInt(sequence.DeQueue(10));
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
    /// Satellite Specific Part of the SSR Galileo URA
    /// </summary>
    public class Message1244 : BaseRtcmMessage
    {
        public Message1244()
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

        public static Message1244 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1244 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1244 msg = new Message1244();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.SSRURA = BitConvertUtil.GetUInt(sequence.DeQueue(6));

            return msg;
        }
    }
    /// <summary>
    /// 28bit. 
    /// Satellite Specific Part of the SSR Galileo High Rate Clock
    /// </summary>
    public class Message1245 : BaseRtcmMessage
    {
        public Message1245()
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

        public static Message1245 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1245 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1245 msg = new Message1245();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.HighRateClockCorrection = BitConvertUtil.GetInt(sequence.DeQueue(22));

            return msg;
        }
    }
}