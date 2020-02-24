//2016.11.18, double,create in hongqing, 北斗星历数据message1046


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// BeiDou Ephemerides
    /// </summary>
    public class Message63: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message63() {
            this.MessageNumber = 63;
            this.Length = 537;
        }

        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; }
        /// <summary>
        /// uint13
        /// </summary>
        public uint WeekNumber { get; set; }
        /// <summary>
        /// bit(4)
        /// </summary>
        public int BeiDOuURAI { get; set; }
        /// <summary>
        /// int14
        /// </summary>
        public int Idot { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint Aode { get; set; }
        /// <summary>
        /// uint17
        /// </summary>
        public uint Toc { get; set; }
        /// <summary>
        /// int11
        /// </summary>
        public int a2 { get; set; }
        /// <summary>
        /// int22
        /// </summary>
        public int a1 { get; set; }
        /// <summary>
        /// int24
        /// </summary>
        public int a0 { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint Aodc { get; set; }
        /// <summary>
        /// int18
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
        /// int18
        /// </summary>
        public int Cuc { get; set; }
        /// <summary>
        /// uint32
        /// </summary>
        public uint Eccentricity { get; set; }
        /// <summary>
        /// int18
        /// </summary>
        public int Cus { get; set; }
        /// <summary>
        /// uint32
        /// </summary>
        public uint SqrtA { get; set; }
        /// <summary>
        /// uint17
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
        public int Tgd1 { get; set; }
        /// <summary>
        /// int10
        /// </summary>
        public int Tgd2 { get; set; }
        /// <summary>
        /// bit(9),卫星健康信息
        /// </summary>
        public int SvHealth { get; set; }
        /// <summary>
        /// bit(1),卫星自主健康状态
        /// </summary>
        public int SatH1 { get; set; }
        /// <summary>
        /// bit(17)
        /// </summary>
        public int Reserved  { get; set; }
        
       
        public static Message63 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message63 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message63 msg = new Message63();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.WeekNumber = BitConvertUtil.GetUInt(sequence.DeQueue(13));
            msg.BeiDOuURAI = BitConvertUtil.GetInt(sequence.DeQueue(4));
            msg.Idot = BitConvertUtil.GetInt(sequence.DeQueue(14));
            msg.Aode = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.Toc = BitConvertUtil.GetUInt(sequence.DeQueue(17));
            msg.a2 = BitConvertUtil.GetInt(sequence.DeQueue(11));
            msg.a1 = BitConvertUtil.GetInt(sequence.DeQueue(22));
            msg.a0 = BitConvertUtil.GetInt(sequence.DeQueue(24));
            msg.Aodc = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            //??
            msg.Crs = BitConvertUtil.GetInt(sequence.DeQueue(18));
            msg.DeltaN = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.M0 = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            //??
            msg.Cuc = BitConvertUtil.GetInt(sequence.DeQueue(18));
            msg.Eccentricity = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            msg.Cus = BitConvertUtil.GetInt(sequence.DeQueue(18));
            msg.SqrtA = BitConvertUtil.GetUInt(sequence.DeQueue(32));
            msg.Toe = BitConvertUtil.GetUInt(sequence.DeQueue(17));
            //??
            msg.Cic = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.Omega0 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            //??
            msg.Cis = BitConvertUtil.GetInt(sequence.DeQueue(18));
            msg.I0 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            msg.Crc = BitConvertUtil.GetInt(sequence.DeQueue(18));
            msg.ArgumentOfPerigee = BitConvertUtil.GetInt(sequence.DeQueue(32));
            //??
            msg.OmegaDot = BitConvertUtil.GetInt(sequence.DeQueue(24));
            msg.Tgd1 = BitConvertUtil.GetInt(sequence.DeQueue(10));
            msg.Tgd2 = BitConvertUtil.GetInt(sequence.DeQueue(10));
            //msg.SvHealth = BitConvertUtil.GetInt(sequence.DeQueue(9));
            msg.SatH1 = BitConvertUtil.GetInt(sequence.DeQueue(1));
            //msg.Reserved = BitConvertUtil.GetInt(sequence.DeQueue(17));
            
            return msg;
        }        
    } 
}