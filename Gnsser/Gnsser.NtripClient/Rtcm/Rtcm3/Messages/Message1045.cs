//2016.11.18, double,create in hongqing, Galileo星历数据message1045


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// Galileo Ephemerides
    /// </summary>
    public class Message1045: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1045() {
            this.MessageNumber = 1044;
            this.Length = 496;
        }

        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// uint12
        /// </summary>
        public uint WeekNumber { get; set; }
        /// <summary>
        /// uint10
        /// </summary>
        public uint Iodn { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint GalileoSVSISA { get; set; }
        /// <summary>
        /// int14
        /// </summary>
        public int Idot { get; set; }
        /// <summary>
        /// uint14
        /// </summary>
        public uint Toc { get; set; }
        /// <summary>
        /// int6
        /// </summary>
        public int af2 { get; set; }
        /// <summary>
        /// int21
        /// </summary>
        public int af1 { get; set; }
        /// <summary>
        /// int31
        /// </summary>
        public int af0 { get; set; }
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
        /// uint14
        /// </summary>
        public uint Toe { get; set; }
        /// <summary>
        /// int16
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
        /// int10
        /// </summary>
        public int GalileoBGD { get; set; }
        /// <summary>
        /// int10
        /// </summary>
        public int E5aSignalHealth { get; set; }
        /// <summary>
        /// int10
        /// </summary>
        public int E5aDataUse { get; set; }
        /// <summary>
        /// bit(7)
        /// </summary>
        public int Reserved  { get; set; } 
       
        public static Message1045 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1045 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1045 msg = new Message1045();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.WeekNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.Iodn = BitConvertUtil.GetUInt(sequence.DeQueue(10));
            msg.GalileoSVSISA = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.Idot = BitConvertUtil.GetInt(sequence.DeQueue(14));
            msg.Toc = BitConvertUtil.GetUInt(sequence.DeQueue(14));
            msg.af2 = BitConvertUtil.GetInt(sequence.DeQueue(6));
            msg.af1 = BitConvertUtil.GetInt(sequence.DeQueue(21));
            msg.af0 = BitConvertUtil.GetInt(sequence.DeQueue(31));
            //??
            msg.Crs = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.DeltaN = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.M0 = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            //??
            msg.Cuc = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.Eccentricity = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            msg.Cus = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.SqrtA = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            msg.Toe = BitConvertUtil.GetUInt(sequence.DeQueue(14));
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
            msg.GalileoBGD = BitConvertUtil.GetInt(sequence.DeQueue(10));
            msg.E5aSignalHealth = BitConvertUtil.GetInt(sequence.DeQueue(2));
            msg.E5aDataUse = BitConvertUtil.GetInt(sequence.DeQueue(1));
            msg.Reserved = BitConvertUtil.GetInt(sequence.DeQueue(7));
           
            return msg;
        }        
    } 
}