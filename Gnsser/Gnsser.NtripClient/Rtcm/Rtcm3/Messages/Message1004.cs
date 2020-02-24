//2015.02.16, czs, create in pengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using Geo.Times;
using Gnsser;

namespace Gnsser.Ntrip.Rtcm
{
    public class NormalMessage
    {

    }

    /// <summary>
    /// GPS 头部信息，具有时间变量。
    /// </summary>
    public class NormalHeader
    {
        public NormalHeader(GpsMessageHeader header)
        {

            this.Header = header;
        }
        /// <summary>
        /// 信息 1004.
        /// </summary>
        public GpsMessageHeader Header { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public Time Time
        {
            get
            {
                var gpsWeek = new Time(Setting.ReceivingTimeOfNtripData.DateTime).GpsWeek;
                return new Time(gpsWeek, Header.GpsEpochTimeInMs / 1000.0);
            }
        }

    }

    /// <summary>
    /// 正常情况下的观测值，没有压缩
    /// </summary>
    public class NormalMessage1004 :NormalMessage, Geo.IToTabRow
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="header"></param>
        /// <param name="Message1004"></param>
        public NormalMessage1004(GpsMessageHeader header,  Message1004 Message1004)
        {
            this.Message1004 = Message1004;
            this.Header = header;
        }  
             
        #region 核心属性
        /// <summary>
        /// 报文 头文件.
        /// </summary>
        public GpsMessageHeader Header { get; set; }
        /// <summary>
        /// 报文 1004.
        /// </summary>
        public Message1004 Message1004 { get; set; }
        #endregion

        /// <summary>
        /// 测站编号
        /// </summary>
        public uint ReferenceStationID { get; set; }
         
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time
        {
            get
            {
                var gpsWeek = new Time(Setting.ReceivingTimeOfNtripData.DateTime).GpsWeek;
                return new Time(gpsWeek, Header.GpsEpochTimeInMs / 1000.0);
            }
        }

        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber Prn { get { return new SatelliteNumber((int)Message1004.GpsSatelliteID, SatelliteType.G); } }

        /// <summary>
        /// 
        /// </summary>
        public double GpsL1PhaseRangeMinusPseudorange { get { return Message1004.GpsL1PhaseRangeMinusPseudorange; }   }
        
        /// <summary>
        /// 
        /// </summary>
        public double GpsL2PhaseRangeMinusL1Pseudorange { get { return Message1004.GpsL2PhaseRangeMinusL1Pseudorange; } }
        /// <summary>
        /// 
        /// </summary>
        public double GpsL2MinusL1PseudorangeDifference { get { return Message1004.GpsL2MinusL1PseudorangeDifference; }  }
        /// <summary>
        /// L1 伪距
        /// </summary>
        public double GpsL1Pseudorange { get { return Message1004.GpsL1Pseudorange; }  }
        /// <summary>
        /// L2 相位距离
        /// </summary>
        public double GpsL1PhaseRange { get; set; }
        /// <summary>
        /// L2 伪距
        /// </summary>
        public double GpsL2Pseudorange { get; set; }
        /// <summary>
        /// L2 相位距离
        /// </summary>
        public double GpsL2PhaseRange { get; set; }
        /// <summary>
        /// 模型化模糊度
        /// </summary>
        public double GpsIntegerL1PseudorangeModulusAmbiguity { get { return Message1004.GpsIntegerL1PseudorangeModulusAmbiguity; } }
    


        #region Tab
        /// <summary>
        /// 以Tab键分开的属性名称
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Time");
            sb.Append("\t");
            sb.Append("ReferenceStationID");
            sb.Append("\t");
            sb.Append("Prn");
            sb.Append("\t");
            sb.Append("GpsL1Pseudorange");
            sb.Append("\t");
            sb.Append("GpsL1PhaseRange");
            sb.Append("\t");
            sb.Append("GpsL2Pseudorange");
            sb.Append("\t");
            sb.Append("GpsL2PhaseRange"); 
            return sb.ToString();
        }
        /// <summary>
        /// 以Tab键分开的属性值
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Time);
            sb.Append("\t");
            sb.Append(ReferenceStationID);
            sb.Append("\t");
            sb.Append(Prn);
            sb.Append("\t");
            sb.Append(GpsL1Pseudorange);
            sb.Append("\t");
            sb.Append(GpsL1PhaseRange);
            sb.Append("\t");
            sb.Append(GpsL2Pseudorange);
            sb.Append("\t");
            sb.Append(GpsL2PhaseRange); 
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    ///  125 bit.
    /// Contents of the Satellite-Specific Portion of a Type 1004 Message, Each Satellite – GPS Extended RTK, L1 & L2
    /// </summary>
    public class Message1004 : BaseRtcmMessage, Geo.IToTabRow
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Message1004()
        {
            this.Length = 125;
        }
        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint GpsSatelliteID { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool GpsL1CodeIndicator { get; set; }
        /// <summary>
        /// uint24 ,24
        /// </summary>
        public uint GpsL1Pseudorange { get; set; }
        /// <summary>
        /// int20 ,20
        /// </summary>
        public int GpsL1PhaseRangeMinusPseudorange { get; set; }
        /// <summary>
        /// uint7 ,7
        /// </summary>
        public uint GpsL1LocktimeIndicator { get; set; }
        /// <summary>
        /// uint8 ,8
        /// </summary>
        public uint GpsIntegerL1PseudorangeModulusAmbiguity { get; set; }
        /// <summary>
        /// uint8 ,8
        /// </summary>
        public uint GpsL1Cnr { get; set; }
        /// <summary>
        /// bit(2) ,2
        /// </summary>
        public uint GpsL2CodeIndicator { get; set; }
        /// <summary> 
        /// int14 ,14
        /// </summary>
        public int GpsL2MinusL1PseudorangeDifference { get; set; }
        /// <summary>
        /// int20 ,20
        /// </summary>
        public int GpsL2PhaseRangeMinusL1Pseudorange { get; set; }
        /// <summary>
        /// uint7 ,7
        /// </summary>
        public uint GpsL2LocktimeIndicator { get; set; }
        /// <summary>
        /// uint8 ,8
        /// </summary>
        public uint GpsL2Cnr { get; set; }
        

        public static Message1004 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }
        #region Tab
        /// <summary>
        /// 以Tab键分开的属性名称
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("GpsSatelliteID");
            sb.Append("\t");
            sb.Append("GpsL1CodeIndicator");
            sb.Append("\t");
            sb.Append("GpsL1Pseudorange");
            sb.Append("\t");
            sb.Append("GpsL1PhaseRangeMinusPseudorange");
            sb.Append("\t");
            sb.Append("GpsL1LocktimeIndicator");
            sb.Append("\t");
            sb.Append("GpsIntegerL1PseudorangeModulusAmbiguity");
            sb.Append("\t");
            sb.Append("GpsL1Cnr");
            sb.Append("\t");
            sb.Append("GpsL2CodeIndicator");
            sb.Append("\t");
            sb.Append("GpsL2MinusL1PseudorangeDifference");
            sb.Append("\t");
            sb.Append("GpsL2PhaseRangeMinusL1Pseudorange");
            sb.Append("\t");
            sb.Append("GpsL2LocktimeIndicator");
            sb.Append("\t");
            sb.Append("GpsL2Cnr");
            return sb.ToString();
        }
        /// <summary>
        /// 以Tab键分开的属性值
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GpsSatelliteID);
            sb.Append("\t");
            sb.Append(GpsL1CodeIndicator);
            sb.Append("\t");
            sb.Append(GpsL1Pseudorange);
            sb.Append("\t");
            sb.Append(GpsL1PhaseRangeMinusPseudorange);
            sb.Append("\t");
            sb.Append(GpsL1LocktimeIndicator);
            sb.Append("\t");
            sb.Append(GpsIntegerL1PseudorangeModulusAmbiguity);
            sb.Append("\t");
            sb.Append(GpsL1Cnr);
            sb.Append("\t");
            sb.Append(GpsL2CodeIndicator);
            sb.Append("\t");
            sb.Append(GpsL2MinusL1PseudorangeDifference);
            sb.Append("\t");
            sb.Append(GpsL2PhaseRangeMinusL1Pseudorange);
            sb.Append("\t");
            sb.Append(GpsL2LocktimeIndicator);
            sb.Append("\t");
            sb.Append(GpsL2Cnr);
            return sb.ToString();
        }
        #endregion

        public static Message1004 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);
            Message1004 msg = new Message1004();
            msg.GpsSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6 
            msg.GpsL1CodeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            msg.GpsL1Pseudorange = BitConvertUtil.GetUInt(sequence.DeQueue(24));//uint24 
            msg.GpsL1PhaseRangeMinusPseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20
            msg.GpsL1LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7 
            msg.GpsIntegerL1PseudorangeModulusAmbiguity = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8 
            msg.GpsL1Cnr = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8
            msg.GpsL2CodeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));//bit(2)
            msg.GpsL2MinusL1PseudorangeDifference = BitConvertUtil.GetInt(sequence.DeQueue(14));//int14
            msg.GpsL2PhaseRangeMinusL1Pseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20 
            msg.GpsL2LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7  
            msg.GpsL2Cnr = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8

            return msg;
        }
    }
}