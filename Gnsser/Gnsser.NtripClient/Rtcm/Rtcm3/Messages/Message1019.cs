//2015.05.06, czs, create in namu, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 488 GPS Ephemerides
    /// </summary>
    public class Message1019: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1019() {
            this.MessageNumber = 1019;
            this.Length = 488;// 448;
        }
        #region Tab
        /// <summary>
        /// 以Tab键分开的属性名称
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MessageNumber");
            sb.Append("\t");
            sb.Append("GpsSatelliteID");
            sb.Append("\t");
            sb.Append("WeekNumber");
            sb.Append("\t");
            sb.Append("SvAccuracy");
            sb.Append("\t");
            sb.Append("CodeOnL2");
            sb.Append("\t");
            sb.Append("Idot");
            sb.Append("\t");
            sb.Append("Iode");
            sb.Append("\t");
            sb.Append("Toc");
            sb.Append("\t");
            sb.Append("Af2");
            sb.Append("\t");
            sb.Append("Af1");
            sb.Append("\t");
            sb.Append("Af0");
            sb.Append("\t");
            sb.Append("Iodc");
            sb.Append("\t");
            sb.Append("Crs");
            sb.Append("\t");
            sb.Append("DeltaN");
            sb.Append("\t");
            sb.Append("M0");
            sb.Append("\t");
            sb.Append("Cuc");
            sb.Append("\t");
            sb.Append("Eccentricity");
            sb.Append("\t");
            sb.Append("Cus");
            sb.Append("\t");
            sb.Append("SqrtA");
            sb.Append("\t");
            sb.Append("Toe");
            sb.Append("\t");
            sb.Append("Cic");
            sb.Append("\t");
            sb.Append("Omega0");
            sb.Append("\t");
            sb.Append("Cis");
            sb.Append("\t");
            sb.Append("I0");
            sb.Append("\t");
            sb.Append("Crc");
            sb.Append("\t");
            sb.Append("ArgumentOfPerigee");
            sb.Append("\t");
            sb.Append("OmegaDot");
            sb.Append("\t");
            sb.Append("Tgd");
            sb.Append("\t");
            sb.Append("SvHealth");
            sb.Append("\t");
            sb.Append("L2PDataFlag");
            sb.Append("\t");
            sb.Append("FitInterval");
            return sb.ToString();
        }
        /// <summary>
        /// 以Tab键分开的属性值
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageNumber);
            sb.Append("\t");
            sb.Append(SatelliteID);
            sb.Append("\t");
            sb.Append(WeekNumber);
            sb.Append("\t");
            sb.Append(SvAccuracy);
            sb.Append("\t");
            sb.Append(CodeOnL2);
            sb.Append("\t");
            sb.Append(Idot);
            sb.Append("\t");
            sb.Append(Iode);
            sb.Append("\t");
            sb.Append(Toc);
            sb.Append("\t");
            sb.Append(Af2);
            sb.Append("\t");
            sb.Append(Af1);
            sb.Append("\t");
            sb.Append(Af0);
            sb.Append("\t");
            sb.Append(Iodc);
            sb.Append("\t");
            sb.Append(Crs);
            sb.Append("\t");
            sb.Append(DeltaN);
            sb.Append("\t");
            sb.Append(M0);
            sb.Append("\t");
            sb.Append(Cuc);
            sb.Append("\t");
            sb.Append(Eccentricity);
            sb.Append("\t");
            sb.Append(Cus);
            sb.Append("\t");
            sb.Append(SqrtA);
            sb.Append("\t");
            sb.Append(Toe);
            sb.Append("\t");
            sb.Append(Cic);
            sb.Append("\t");
            sb.Append(Omega0);
            sb.Append("\t");
            sb.Append(Cis);
            sb.Append("\t");
            sb.Append(I0);
            sb.Append("\t");
            sb.Append(Crc);
            sb.Append("\t");
            sb.Append(ArgumentOfPerigee);
            sb.Append("\t");
            sb.Append(OmegaDot);
            sb.Append("\t");
            sb.Append(Tgd);
            sb.Append("\t");
            sb.Append(SvHealth);
            sb.Append("\t");
            sb.Append(L2PDataFlag);
            sb.Append("\t");
            sb.Append(FitInterval);
            return sb.ToString();
        }
        #endregion

        #region 原始记录数据
        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; } 
        /// <summary>
        /// uint10，0 -1023， 1 gpsWeek，Roll-over every 1024 weeks starting from Midnight on the night of January 5/Morning January 6, 1980
        /// </summary>
        public uint WeekNumber { get; set; }
        /// <summary>
        /// uint4，Resolution:1, Units: meters; see GPS SPS Signal Spec, 2.4.3.2
        /// </summary>
        public uint SvAccuracy { get; set; } 
        /// <summary>
        /// bit2,00 - Reserved,
        /// 01 - P code on,
        /// 10 - C/A code on,
        /// 11 - L2C on
        /// </summary>
        public uint CodeOnL2 { get; set; }
        //Note 1 - Determining Loss of Lock: In normal operation, a cycle slip will be evident when the Minimum Lock Time (MLT) has
        //decreased in value. For long time gaps between messages, such as from a radio outage, extra steps should be taken on the rover to
        //safeguard against missed cycle slips
        /// <summary>
        /// int14，GPS IDOT See Note 1， 2^-43 semicircles/sec。 int14
        /// </summary>
        public int Idot { get; set; }
        /// <summary>
        /// uint8,Issue of Data Ephemeris,see GPS SPS Signal Spec, 2.4.4.2
        /// </summary>
        public uint Iode { get; set; }
        /// <summary>
        /// uint16,Resolution=2^4s
        /// </summary>
        public uint Toc { get; set; }
        /// <summary>
        /// int8,Resolution:2^-55 sec/sec2
        /// </summary>
        public int Af2 { get; set; }
        /// <summary>
        /// int16,Resolution:2^-43 sec/sec
        /// </summary>
        public int Af1 { get; set; }
        /// <summary>
        /// int22,Resolution: 2^-31 sec
        /// </summary>
        public int Af0 { get; set; }
        /// <summary>
        /// uint10,The 8 LSBs of IODC contains the same bits and sequence as those in IODE; see GPS SPS Signal Spec, 2.4.3.4.
        /// </summary>
        public uint Iodc { get; set; }
        /// <summary>
        /// int16,Resolution:2^-5 m
        /// </summary>
        public int Crs { get; set; }
        /// <summary>
        /// int16，Resolution:2^-43 semicircles/sec
        /// </summary>
        public int DeltaN { get; set; }
        /// <summary>
        /// int32，Resolution:2^-31 semicircles
        /// </summary>
        public int M0 { get; set; }
        /// <summary>
        /// int16，Resolution:2^-29 rad
        /// </summary>
        public int Cuc { get; set; }
        /// <summary>
        /// uint32,Resolution:2^-33
        /// </summary>
        public uint Eccentricity { get; set; }
        /// <summary>
        /// int16,Resolution:2^-29 rad
        /// </summary>
        public int Cus { get; set; }
        /// <summary>
        /// uint32,Resolution:2^-19 m1/2
        /// </summary>
        public uint SqrtA { get; set; }
        /// <summary>
        /// uint16,Resolution:2^4 sec
        /// </summary>
        public uint Toe { get; set; }
        /// <summary>
        /// int16,Resolution:2^-29 rad
        /// </summary>
        public int Cic { get; set; }
        /// <summary>
        /// int32,Resolution:2^-31 semicircles
        /// </summary>
        public int Omega0 { get; set; }
        /// <summary>
        /// int16,Resolution:2^-29 rad
        /// </summary>
        public int Cis { get; set; }
        /// <summary>
        /// int32,Resolution:2^-31 semicircles
        /// </summary>
        public int I0 { get; set; }
        /// <summary>
        /// int16,Resolution:2^-5 m
        /// </summary>
        public int Crc { get; set; }
        /// <summary>
        /// int32,Resolution:2^-31 semicircles
        /// </summary>
        public int ArgumentOfPerigee { get; set; }
        /// <summary>
        /// int24 OMEGADOT,OmegaDot, RateOfRightAscension,Resolution:2^-43 semicircles/sec
        /// </summary>
        public int OmegaDot { get; set; }
        /// <summary>
        /// int8,Resolution:2^-31 sec
        /// </summary>
        public int Tgd { get; set; }        
        /// <summary>
        /// uint6,Resolution:1
        /// </summary>
        public uint SvHealth { get; set; }
        /// <summary>
        /// bit1,Resolution:1
        /// </summary>
        public bool L2PDataFlag { get; set; }
        #endregion

        /// <summary>
        /// bit1, 0 curve 4hours, 1 greater than 4h
        /// </summary>
        public bool FitInterval { get; set; }
 
        public static Message1019 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1019 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1019 msg = new Message1019();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.WeekNumber = BitConvertUtil.GetUInt(sequence.DeQueue(10));
            msg.SvAccuracy = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.CodeOnL2 = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.Idot = BitConvertUtil.GetInt(sequence.DeQueue(14)) ;
            msg.Iode = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.Toc = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            msg.Af2 = BitConvertUtil.GetInt(sequence.DeQueue(8)) ;
            msg.Af1 = BitConvertUtil.GetInt(sequence.DeQueue(16)) ;
            msg.Af0 = BitConvertUtil.GetInt(sequence.DeQueue(22)) ;
            msg.Iodc = BitConvertUtil.GetUInt(sequence.DeQueue(10));
            //??
            msg.Crs = BitConvertUtil.GetInt(sequence.DeQueue(16)) ;
            msg.DeltaN = BitConvertUtil.GetInt(sequence.DeQueue(16));
            msg.M0 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            //??
            msg.Cuc = BitConvertUtil.GetInt(sequence.DeQueue(16)) ;
            msg.Eccentricity = BitConvertUtil.GetUInt(sequence.DeQueue(32)) ;
            msg.Cus = BitConvertUtil.GetInt(sequence.DeQueue(16)) ;
            msg.SqrtA = BitConvertUtil.GetUInt(sequence.DeQueue(32)) ;
            msg.Toe = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            //??
            msg.Cic = BitConvertUtil.GetInt(sequence.DeQueue(16)) ;
            msg.Omega0 = BitConvertUtil.GetInt(sequence.DeQueue(32));
            //??
            msg.Cis = BitConvertUtil.GetInt(sequence.DeQueue(16)) ;
            msg.I0 = BitConvertUtil.GetInt(sequence.DeQueue(32)) ;
            msg.Crc = BitConvertUtil.GetInt(sequence.DeQueue(16)) ;
            msg.ArgumentOfPerigee = BitConvertUtil.GetInt(sequence.DeQueue(32));
            //??
            msg.OmegaDot = BitConvertUtil.GetInt(sequence.DeQueue(24)) ;
            msg.Tgd = BitConvertUtil.GetInt(sequence.DeQueue(8)) ;
            msg.SvHealth = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.L2PDataFlag = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            msg.FitInterval = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;

            return msg;
        }        
    } 
}