  //2015.05.06, czs, create in namu, RTCM 消息
//2016.01.18, double,将SatelliteID修改为GlonassSatelliteID 又改回去为SatelliteID

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 360 glonass Ephemerides
    /// </summary>
    public class Message1020: BaseRtcmMessage
    {
        /// <summary>
        /// 消息构造函数
        /// </summary> 
        public Message1020() {
            this.MessageNumber = 1020;
            this.Length = 360;
        }

        /// <summary>
        /// uint6
        /// </summary>
        public uint SatelliteID { get; set; } 

        /// <summary>
        /// uint5
        /// </summary>
        public uint SatelliteFrequencyChannelNumber { get; set; }

        /// <summary>
        /// bit(1)
        /// </summary>
        public bool AlmanacHealth { get; set; } 
        /// <summary>
        /// bit(1)
        /// </summary>
        public int AlmanacHealthAvailabilityIndicator { get; set; } 

        /// <summary>
        /// P1 bit(2)
        /// </summary>
        public uint P1 { get; set; }

        /// <summary>
        /// bit(12)
        /// </summary>
        //public int Tk { get; set; }
        /// <summary>
        /// bit(6)
        /// </summary>
        public uint Tkh { get; set; }
        /// <summary>
        /// bit(5)
        /// </summary>
        public uint Tkm { get; set; }
        /// <summary>
        /// bit(1)
        /// </summary>
        public double Tks { get; set; }
        /// <summary>
        /// bit1
        /// </summary>
        public int MsbOfBnWord { get; set; }

        /// <summary>
        /// P2 bit(1)
        /// </summary>
        public bool P2 { get; set; }

        /// <summary>
        /// uint7
        /// </summary>
        public uint Tb { get; set; }

        /// <summary>
        /// intS24
        /// </summary>
        //public int XnfFirstDerivative { get; set; }
        public double XnfFirstDerivative { get; set; }
        /// <summary>
        /// intS27
        /// </summary>
        //public int Xn { get; set; }
        public double Xn { get; set; }
        /// <summary>
        /// intS5
        /// </summary>
        //public int XnSecondDerivative { get; set; }
        public double XnSecondDerivative { get; set; }

        /// <summary>
        /// intS24
        /// </summary>
        //public int YnfFirstDerivative { get; set; }
        public double YnfFirstDerivative { get; set; }
        /// <summary>
        /// intS27
        /// </summary>
        //public int Yn { get; set; }
        public double Yn { get; set; }
        /// <summary>
        /// intS5
        /// </summary>
        //public int YnSecondDerivative { get; set; }
        public double YnSecondDerivative { get; set; }

        /// <summary>
        /// intS24
        /// </summary>
        //public int ZnfFirstDerivative { get; set; }
        public double ZnfFirstDerivative { get; set; }
        /// <summary>
        /// intS27
        /// </summary>
        //public int Zn { get; set; }
        public double Zn { get; set; }
        /// <summary>
        /// intS5
        /// </summary>
        //public int ZnSecondDerivative { get; set; }
        public double ZnSecondDerivative { get; set; }

        /// <summary>
        /// bit(1)
        /// </summary>
        public bool P3 { get; set; }
        /// <summary>
        /// intS111
        /// </summary>
        //public int GamaN { get; set; }
        public double GamaN { get; set; }

        /// <summary>
        /// bit(2)
        /// </summary>
        public uint Mp { get; set; }
        /// <summary>
        /// bit 1
        /// </summary>
        public bool Mln { get; set; }
        /// <summary>
        /// intS22
        /// </summary>
        //public int TaoN { get; set; }
        public double TaoN { get; set; }
        /// <summary>
        /// intS5
        /// </summary>
        public long MDeltaTaoN { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint En { get; set; }
        /// <summary>
        /// bit1
        /// </summary>
        public bool Mp4 { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public uint Mft { get; set; }
        /// <summary>
        /// uint11
        /// </summary>
        public uint Mnt { get; set; }
        /// <summary>
        /// bit(2)
        /// </summary>
        public uint Mm { get; set; }
        /// <summary>
        /// bit(1)
        /// </summary>
        public bool TheAvailabilityOfAdditionalData { get; set; }
        /// <summary>
        /// uint11
        /// </summary>
        public uint Na { get; set; }
        /// <summary>
        /// intS32
        /// </summary>
        public long TaoC { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint Mn4 { get; set; }
        /// <summary>
        /// intS22
        /// </summary>
        public long MtaoGps { get; set; }
        /// <summary>
        /// bit(1)
        /// </summary>
        public bool MlnFifthString { get; set; }
        /// <summary>
        /// bit(7)
        /// </summary>
        public uint Reserved { get; set; }
       
        public static Message1020 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static Message1020 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            Message1020 msg = new Message1020();
            msg.MessageNumber = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            msg.SatelliteID = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.SatelliteFrequencyChannelNumber = BitConvertUtil.GetUInt(sequence.DeQueue(5))-7;
            msg.AlmanacHealth = BitConvertUtil.GetInt(sequence.DeQueue(1))==1;
            msg.AlmanacHealthAvailabilityIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1));
            msg.P1 = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            //Tk 分为时、分、秒，各自所占字符数不等
            msg.Tkh = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            msg.Tkm = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.Tks = BitConvertUtil.GetInt(sequence.DeQueue(1)) * 30;
            msg.MsbOfBnWord = BitConvertUtil.GetInt(sequence.DeQueue(1));
            msg.P2 = BitConvertUtil.GetInt(sequence.DeQueue(1))==1;
            msg.Tb = BitConvertUtil.GetUInt(sequence.DeQueue(7)) * 900;
            msg.XnfFirstDerivative = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(24)) * GnssConst.P2_20 * 1E3;
            msg.Xn = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(27)) * GnssConst.P2_11 * 1E3;
            msg.XnSecondDerivative = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(5)) * GnssConst.P2_30 * 1E3;
            msg.YnfFirstDerivative = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(24)) * GnssConst.P2_20 * 1E3;
            msg.Yn = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(27)) * GnssConst.P2_11 * 1E3;
            msg.YnSecondDerivative = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(5)) * GnssConst.P2_30 * 1E3;
            msg.ZnfFirstDerivative = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(24)) * GnssConst.P2_20 * 1E3;
            msg.Zn = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(27)) * GnssConst.P2_11 * 1E3;
            msg.ZnSecondDerivative = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(5)) * GnssConst.P2_30 * 1E3;
            msg.P3 = BitConvertUtil.GetInt(sequence.DeQueue(1))==1;
            msg.GamaN = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(11)) * GnssConst.P2_40;
            msg.Mp = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.Mln = BitConvertUtil.GetInt(sequence.DeQueue(1))==1;
            msg.TaoN = BitConvertUtil.GetInt(sequence.DeQueue(22)) * GnssConst.P2_30;
            msg.MDeltaTaoN = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(5));
            msg.En = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.Mp4 = BitConvertUtil.GetInt(sequence.DeQueue(1)) ==1;
            msg.Mft = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            msg.Mnt = BitConvertUtil.GetUInt(sequence.DeQueue(11));
            msg.Mm = BitConvertUtil.GetUInt(sequence.DeQueue(2));
            msg.TheAvailabilityOfAdditionalData = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            msg.Na = BitConvertUtil.GetUInt(sequence.DeQueue(11));
            msg.TaoC = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(32));
            msg.Mn4 = BitConvertUtil.GetUInt(sequence.DeQueue(5));
            msg.MtaoGps = BitConvertUtil.ConvertToInt64WithInverseCode(sequence.DeQueue(22));
            msg.MlnFifthString = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            msg.Reserved = BitConvertUtil.GetUInt(sequence.DeQueue(7));
           
            return msg;
        }        
    } 
}