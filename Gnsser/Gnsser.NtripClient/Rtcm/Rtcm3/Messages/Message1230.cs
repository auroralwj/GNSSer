//2016.10.19, double,create in hongqing

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using Geo.Times;
namespace Gnsser.Ntrip.Rtcm
{
        
    /// <summary>
    /// 32+16*N
    /// Contents of GLONASS L1 and L2 Code-Phase Bias Message1230
    /// </summary>
    public class Message1230 : BaseRtcmMessage, Geo.IToTabRow
    {
        /// <summary>
        /// 消息构造函数
        /// </summary>
        public Message1230()
        {
           
        }

        /// <summary>
        /// uint6 , 6
        /// </summary>
        public uint ReferenceStationID { get; set; }
        /// <summary>
        /// bit(1) ,1
        /// </summary>
        public bool GlonassCodePhaseBiasIndicator { get; set; }
        /// <summary>
        /// uint5 ,5
        /// </summary>
        public uint Reserved { get; set; }
        /// <summary>
        /// uint,25
        /// </summary>
        public double GlonassFDMASignalMask { get; set; }
        /// <summary>
        /// int20 ,20
        /// </summary>
        public int GlonassL1CACodePhaseBias { get; set; }
        /// <summary>
        /// uint7 ,7
        /// </summary>
        public uint GlonassL1LocktimeIndicator { get; set; }
        /// <summary>
        /// uint7
        /// </summary>
        public uint GlonassIntegerL1PseudorangeModulusAmbiguity { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint GlonassL1Cnr { get; set; } 

        /// <summary>
        /// bit(2) 
        /// </summary>
        public uint GlonassL2CodeIndicator { get; set; }
        /// <summary>
        /// uint14
        /// </summary>
        public uint GlonassL2MinusL1PseudorangeDifference { get; set; }
        /// <summary>
        /// int20
        /// </summary>
        public int GlonassL2PhaseRangeMinusL1Pseudorange { get; set; }
        /// <summary>
        /// uint7 ,7
        /// </summary>
        public uint GlonassL2LocktimeIndicator { get; set; } 
        /// <summary>
        /// uint8
        /// </summary>
        public uint GlonassL2Cnr { get; set; }
        /// <summary>
        /// double型
        /// </summary>
        public double GlonassL1PhaseRange { get; set; }
        /// <summary>
        /// double型
        /// </summary>
        public double GlonassL2Pseudorange { get; set; }
        /// <summary>
        /// double型
        /// </summary>
        public double GlonassL2PhaseRange { get; set; }
        /// <summary>
        /// double型
        /// </summary>
        public double GlonassL1PhaseRangeMinusPseudorange1 { get; set; }
        /// <summary>
        /// double型
        /// </summary>
        public double GlonassL2PhaseRangeMinusL1Pseudorange1 { get; set; }


        #region Tab
        /// <summary>
        /// 以Tab键分开的属性名称
        /// </summary>
        /// <returns></returns>
        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("GlonassSatelliteID");
            sb.Append("\t");
            sb.Append("GlonassL1CodeIndicator");
            sb.Append("\t");
            sb.Append("GlonassSatelliteFrequencyChannelNumber");
            sb.Append("\t");
            sb.Append("GlonassL1Pseudorange");
            sb.Append("\t");
            sb.Append("GlonassL1PhaseRangeMinusPseudorange");
            sb.Append("\t");
            sb.Append("GlonassL1LocktimeIndicator");
            sb.Append("\t");
            sb.Append("GlonassIntegerL1PseudorangeModulusAmbiguity");
            sb.Append("\t");
            sb.Append("GlonassL1Cnr");
            sb.Append("\t");
            sb.Append("GlonassL2CodeIndicator");
            sb.Append("\t");
            sb.Append("GlonassL2MinusL1PseudorangeDifference");
            sb.Append("\t");
            sb.Append("GlonassL2PhaseRangeMinusL1Pseudorange");
            sb.Append("\t");
            sb.Append("GlonassL2LocktimeIndicator");
            sb.Append("\t");
            sb.Append("GlonassL2Cnr");
            sb.Append("\t");
            sb.Append("GlonassL1Pseudorange");
            return sb.ToString();
        }
        /// <summary>
        /// 以Tab键分开的属性值
        /// </summary>
        /// <returns></returns>
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ReferenceStationID);
            sb.Append("\t");
            sb.Append(GlonassCodePhaseBiasIndicator);
            sb.Append("\t");
            sb.Append(Reserved);
            sb.Append("\t");
            sb.Append(GlonassFDMASignalMask);
            sb.Append("\t");
            sb.Append(GlonassL1CACodePhaseBias);
            sb.Append("\t");
            sb.Append(GlonassL1LocktimeIndicator);
            sb.Append("\t");
            sb.Append(GlonassIntegerL1PseudorangeModulusAmbiguity);
            sb.Append("\t");
            sb.Append(GlonassL1Cnr);
            sb.Append("\t");
            sb.Append(GlonassL2CodeIndicator);
            sb.Append("\t");
            sb.Append(GlonassL2MinusL1PseudorangeDifference);
            sb.Append("\t");
            sb.Append(GlonassL2PhaseRangeMinusL1Pseudorange);
            sb.Append("\t");
            sb.Append(GlonassL2LocktimeIndicator);
            sb.Append("\t");
            sb.Append(GlonassL2Cnr);
            sb.Append("\t");
            sb.Append(GlonassFDMASignalMask);
            return sb.ToString();
        }
        #endregion
        public static Message1012 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1012 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1012 msg = new Message1012();
            msg.GlonassSatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));//uint6
            msg.GlonassL1CodeIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            msg.GlonassSatelliteFrequencyChannelNumber = BitConvertUtil.GetUInt(sequence.DeQueue(5));//uint5
            msg.GlonassL1Pseudorange = BitConvertUtil.GetInt(sequence.DeQueue(25));//uint25 
            msg.GlonassL1PhaseRangeMinusPseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20 
            msg.GlonassL1LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7  
            msg.GlonassIntegerL1PseudorangeModulusAmbiguity = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7 
            msg.GlonassL1Cnr = BitConvertUtil.GetUInt(sequence.DeQueue(8));//uint8  


            msg.GlonassL2CodeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(2));//bit(2)  
            msg.GlonassL2MinusL1PseudorangeDifference = BitConvertUtil.GetUInt(sequence.DeQueue(14));//uint14  
            msg.GlonassL2PhaseRangeMinusL1Pseudorange = BitConvertUtil.GetInt(sequence.DeQueue(20));//int20 
            msg.GlonassL2LocktimeIndicator = BitConvertUtil.GetUInt(sequence.DeQueue(7));//uint7  
            msg.GlonassL2Cnr = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            msg.GlonassL1Pseudorange = msg.GlonassL1Pseudorange * 0.02 + msg.GlonassIntegerL1PseudorangeModulusAmbiguity * GnssConst.LightSpeedPerMillisecond;
            uint a1 = 0xFFF80000;
            if (msg.GlonassL1PhaseRangeMinusPseudorange != a1)
            {
                msg.GlonassL1PhaseRangeMinusPseudorange1 = msg.GlonassL1PhaseRangeMinusPseudorange * 0.0005 / Frequence.GetGlonassG1(1).WaveLength;
            }
                msg.GlonassL1PhaseRange = msg.GlonassL1Pseudorange + msg.GlonassL1PhaseRangeMinusPseudorange * 0.0005;
            uint a2 = 0xFFFFE000;
            if (msg.GlonassL2MinusL1PseudorangeDifference != a2)
                msg.GlonassL2Pseudorange = msg.GlonassL1Pseudorange + msg.GlonassL2MinusL1PseudorangeDifference * 0.02;
            if (msg.GlonassL2PhaseRangeMinusL1Pseudorange != a1)
            {
                msg.GlonassL2PhaseRangeMinusL1Pseudorange1 = msg.GlonassL2PhaseRangeMinusL1Pseudorange * 0.0005 / Frequence.GetGlonassG2(1).WaveLength;
            }
                msg.GlonassL2PhaseRange = msg.GlonassL1Pseudorange + msg.GlonassL2PhaseRangeMinusL1Pseudorange * 0.0005;


             
            return msg;
        }
    }
}