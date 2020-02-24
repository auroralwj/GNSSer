//2016.11.18, double,create in hongqing, QZSS星历数据message1044


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// QZSS Ephemerides
    /// </summary>
    public class Message1044: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1044() {
            this.MessageNumber = 1044;
            this.Length = 485;
        }

        /// <summary>
        /// uint4
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// uint16
        /// </summary>
        public uint Toc { get; set; }
        /// <summary>
        /// int8
        /// </summary>
        public int af2 { get; set; }
        /// <summary>
        /// int16
        /// </summary>
        public int af1 { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public int af0 { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint Iode { get; set; }
        /// <summary>
        /// int16
        /// </summary>
        public int Crs { get; set; }
        /// <summary>
        /// int16
        /// </summary>
        public int DeltaN { get; set; }
        /// <summary>
        /// uint32
        /// </summary>
        public uint M0 { get; set; }
        /// <summary>
        /// int16
        /// </summary>
        public int Cuc { get; set; }
        /// <summary>
        /// uint32
        /// </summary>
        public uint Eccentricity { get; set; }
        /// <summary>
        /// int16
        /// </summary>
        public int Cus { get; set; }
        /// <summary>
        /// uint32
        /// </summary>
        public uint SqrtA { get; set; }
        /// <summary>
        /// uint16
        /// </summary>
        public uint Toe { get; set; }
        /// <summary>
        /// int18
        /// </summary>
        public int Cic { get; set; }
        /// <summary>
        /// int32
        /// </summary>
        public int Omega0 { get; set; }
        /// <summary>
        /// int16
        /// </summary>
        public int Cis { get; set; }
        /// <summary>
        /// int32
        /// </summary>
        public int I0 { get; set; }
        /// <summary>
        /// int16
        /// </summary>
        public int Crc { get; set; }
        /// <summary>
        /// int32
        /// </summary>
        public int ArgumentOfPerigee { get; set; }
        /// <summary>
        /// int24 
        /// </summary>
        public int OmegaDot { get; set; }
        /// <summary>
        /// bit(7)
        /// </summary>
        public int i0DOT { get; set; }
        /// <summary>
        /// L2 码类型
        /// </summary>
        public int CodeOnL2 { get; set; }
        /// <summary>
        /// uint12
        /// </summary>
        public uint WeekNumber { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public uint URA { get; set; }
        /// <summary>
        ///uint6
        /// </summary>
        public uint SVHealthState { get; set; }
        /// <summary>
        /// int8
        /// </summary>
        public int Tgd { get; set; }
        /// <summary>
        /// uint10
        /// </summary>
        public uint Iodc { get; set; }
        /// <summary>
        /// bit1, 0 curve 4hours, 1 greater than 4h
        /// </summary>
        public bool FitInterval { get; set; }
        public static Message1044 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1044 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1044 msg = new Message1044();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.WeekNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.Toc = BitConvertUtil.GetUInt(sequence.DeQueue(14));
            msg.af2 = BitConvertUtil.GetInt(sequence.DeQueue(6));
            msg.af1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.af0 = BitConvertUtil.GetInt(sequence.DeQueue(31));
            msg.Iode = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            //??
            msg.Crs = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.DeltaN = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.M0 = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            //??
            msg.Cuc = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.Eccentricity = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            msg.Cus = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.SqrtA = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            msg.Toe = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            //??
            msg.Cic = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.Omega0 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            //??
            msg.Cis = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.I0 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            msg.Crc = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.ArgumentOfPerigee = BitConvertUtil.GetInt(sequence.DeQueue(32));
            //??
            msg.OmegaDot = BitConvertUtil.GetInt(sequence.DeQueue(24));

            msg.i0DOT = BitConvertUtil.GetInt(sequence.DeQueue(14));
            msg.CodeOnL2 = BitConvertUtil.GetInt(sequence.DeQueue(2));
            msg.WeekNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.URA = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.SVHealthState = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.Tgd = BitConvertUtil.GetInt(sequence.DeQueue(8));
            msg.Iodc = BitConvertUtil.GetUInt(sequence.DeQueue(10));
            msg.FitInterval = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            return msg;
        }
    } 
}