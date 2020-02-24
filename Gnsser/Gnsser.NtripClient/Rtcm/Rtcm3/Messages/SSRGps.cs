//2016.01.16, double, create in zhengzhou, RTCM 消息
//2016.11.19, double, edit in hongqing, 将Gps SSR信息整合到一起
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
    public class Message1057 : Message1060
    {
        public Message1057()
        {
           this.Length = 135;
        }
        public new static Message1057 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        public new static Message1057 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1057 msg = new Message1057();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
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
    /// 76bit.
    /// </summary>
    public class Message1058 : Message1060
    {
        public Message1058()
        {
            this.Length = 76;
        }
        public new static Message1058 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public new static Message1058 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1058 msg = new Message1058();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.DeltaClockC0 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.DeltaClockC1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.DeltaClockC2 = BitConvertUtil.GetInt(sequence.DeQueue(27));
            if (msg.DeltaClockC1 != 0 || msg.DeltaClockC2 != 0)
            { 
            }
            return msg;
        }
    }
    /// <summary>
    /// 30bit.
    /// Satellite Specific Part of the SSR GPS Satellite Code Bias  11bit
    /// Code Specific Part of the SSR GPS Satellite Code Bias   19bit
    /// </summary>
    public class Message1059 : BaseRtcmMessage
    {
        public Message1059()
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

        public static Message1059 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1059 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1059 msg = new Message1059();
            //Satellite Specific Part of the SSR GPS Satellite Code Bias 
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.NoofCodeBiasProcess = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            //Code Spoecific Part of the SSR GPS Satellite Code Bias 
            for (int i = 0; i < (int)msg.NoofCodeBiasProcess; i++)
            {

            }
            msg.SignalandTrackingModeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.CodeBias = BitConvertUtil.GetInt(sequence.DeQueue(14));

            return msg;
        }
    }
    public class SSRMessage : Geo.BaseDictionary<SatelliteNumber, NormalSSRMessage>
    {
        public SSRMessage(){ }
        public NormalHeader Header { get; set; }


    }
    public class NormalSSRMessage{}
    /// <summary>
    /// 正常情况下的观测值，没有压缩
    /// </summary>
    public class NormalMessage1060 : NormalSSRMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Message1060"></param>
        public NormalMessage1060(Message1060 Message1060)
        {
            this.Message1060 = Message1060;
        }

        #region 核心属性
        /// <summary>
        /// 报文 1060.
        /// </summary>
        public Message1060 Message1060 { get; set; }
        #endregion

        /// <summary>
        /// 测站编号
        /// </summary>
        public uint ReferenceStationID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        // public Time Time { get { return new Time(DateTime.UtcNow, Header.GpsEpochTimeInMs / 1000.0); } }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get { return new SatelliteNumber((int)Message1060.SatelliteID, SatelliteType.G); } }
    }
    /// <summary>
    /// 205bit. 
    /// </summary>
    public class Message1060 : BaseRtcmMessage
    {
        public Message1060()
        {
            this.Length = 205;
        }
        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint IODE { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public int DeltaRadial { get; set; }
        /// <summary>
        /// int20
        /// </summary>
        public int DeltaAlongTrack { get; set; }
        /// <summary>
        /// int20
        /// </summary>
        public int DeltaCrossTrack { get; set; }
        /// <summary>
        /// int21
        /// </summary>
        public int DotDeltaRadial { get; set; }
        /// <summary>
        /// int19
        /// </summary>
        public int DotDeltaAlongTrack { get; set; }
        /// <summary>
        /// int19
        /// </summary>
        public int DotDeltaCrossTrack { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public int DeltaClockC0 { get; set; }
        /// <summary>
        /// int21
        /// </summary>
        public int DeltaClockC1 { get; set; }
        /// <summary>
        /// int27
        /// </summary>
        public int DeltaClockC2 { get; set; }
        /// <summary>
        /// uint24
        /// </summary>
        public uint IodCrc { get; set; }
        public static Message1060 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1060 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1060 msg = new Message1060();
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
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
    /// 12bit. 
    /// Satellite Specific Part of the SSR GPS URA
    /// </summary>
    public class Message1061 : BaseRtcmMessage
    {
        public Message1061()
        {
            this.Length = 12;
        }

        /// <summary>
        /// uint6
        /// </summary>
        public uint GpsSatelliteID { get; set; }
        /// <summary>
        /// bit(6)
        /// </summary>
        public uint SSRURA { get; set; }

        public static Message1061 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1061 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1061 msg = new Message1061();
            msg.GpsSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.SSRURA = BitConvertUtil.GetUInt(sequence.DeQueue(6));

            return msg;
        }
    }
    /// <summary>
    /// 28bit. 
    /// Satellite Specific Part of the SSR GPS High Rate Clock
    /// </summary>
    public class Message1062 : BaseRtcmMessage
    {
        public Message1062()
        {
            this.Length = 28;
        }

        /// <summary>
        /// uint6
        /// </summary>
        public uint GpsSatelliteID { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public int HighRateClockCorrection { get; set; }

        public static Message1062 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1062 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1062 msg = new Message1062();
            msg.GpsSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.HighRateClockCorrection = BitConvertUtil.GetInt(sequence.DeQueue(22));

            return msg;
        }
    }
}