using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Ntrip.Rtcm
{ 
    /// <summary>
    /// 头部信息的第一个 30 位
    /// </summary>
    public class Rtcm2Header1 : Rtcm2Byte
    {

        /// <summary>
        /// 序文；电报报头；先兆
        /// </summary> 
        public static string Preamble_2 = "01100110";
        /// <summary>
        /// 帧ID 6 bit
        /// </summary>
        public byte Id { get; set; }
        /// <summary>
        /// 引导字 8 bit
        /// </summary>
        public byte Head { get; set; }
        /// <summary>
        /// 基准站ID 10 bit
        /// </summary>
        public int SiteId { get; set; }


        static public Rtcm2Header1 Parse(string binString)
        {
            if (binString.Length != 30) { throw new Exception("应该是30位。"); }
            Rtcm2Header1 RtcmFrame = new Rtcm2Header1();
            RtcmFrame.CharString = binString;
            var header = binString.Substring(0, 8);
            if (header != Rtcm2Header1.Preamble_2) { throw new Exception("header应该是 " + Rtcm2Header1.Preamble_2 + "。实际是：" + header); }
            RtcmFrame.Head = (byte)Convert.ToInt32(header, 2);//转回十进制
            RtcmFrame.Id = (byte)Convert.ToInt32(binString.Substring(8, 6), 2);//转回十进制
            RtcmFrame.SiteId = Convert.ToInt32(binString.Substring(14, 10), 2);//转回十进制
            RtcmFrame.CheckBit = Convert.ToInt32(binString.Substring(24, 6), 2);//转回十进制
            return RtcmFrame;
        }
    }

    /// <summary>
    ///  头部信息的第二个 30 位
    /// </summary>
    public class Rtcm2Header2 : Rtcm2Byte
    {
        //以下为第二个字
        /// <summary>
        /// 修正Z 计数
        /// </summary>
        public int FixZCount { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 帧长度
        /// </summary>
        public int FrameLength { get; set; }
        /// <summary>
        /// 基准站健康状态3 bit
        /// </summary>
        public int HealthStatus { get; set; }


        static public Rtcm2Header2 Parse(string binString)
        {
            Rtcm2Header2 RtcmHeader = new Rtcm2Header2();
            if (binString.Length != 30) { throw new Exception("应该是30位。"); }
            RtcmHeader.CharString = binString;
            RtcmHeader.FixZCount = Convert.ToInt32(binString.Substring(0, 13), 2);//转回十进制
            RtcmHeader.SerialNumber = Convert.ToInt32(binString.Substring(13, 3), 2);//转回十进制
            RtcmHeader.FrameLength = Convert.ToInt32(binString.Substring(16, 5), 2);//转回十进制
            RtcmHeader.HealthStatus = Convert.ToInt32(binString.Substring(21, 3), 2);//转回十进制
            RtcmHeader.CheckBit = Convert.ToInt32(binString.Substring(24, 6), 2);//转回十进制
            return RtcmHeader;
        }  
    }



    /// <summary>
    /// 帧
    /// </summary>
    public class Rtcm2Header
    {   
        public  Rtcm2Header()
        { 
        }
        public Rtcm2Header(Rtcm2Header1 RtcmByte1, Rtcm2Header2 RtcmByte2)
        {
            this.RtcmByte1 = RtcmByte1;
            this.RtcmByte2 = RtcmByte2;
        }

        public Rtcm2Header1 RtcmByte1 { get; set; }
        public Rtcm2Header2 RtcmByte2 { get; set; } 
    }
}
