//2015.02.04, czs, create in pengzhou, 处理 RTCM 3.X 版本的数据读取。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        L1_Only_GPS_RTK_Observables = 1001,
        Extended_L1_Only_GPS_RTK_Observables = 1002,
        L1_L2_GPS_RTK_Observables = 1003,
        Extended_L1_L2_GPS_RTK_Observables = 1004,
        Stationary_RTK_Reference_Station_ARP = 1005,
        Stationary_RTK_Reference_Station_ARP_with_Antenna_Height = 1006,
        Antenna_Descriptor = 1007,
        Antenna_Descriptor_Serial_Number = 1008,
        L1_Only_GLONASS_RTK_Observables = 1009,
        Extended_L1_Only_GLONASS_RTK_Observables = 1010,
        L1_L2_GLONASS_RTK_Observables = 1011,
        Extended_L1_L2_GLONASS_RTK_Observables = 1012,
        System_Parameters = 1013,
        Network_Auxiliary_Station_Data = 1014,
        GPS_Ionospheric_Correction_Differences = 1015,
        GPS_Geometric_Correction_Differences = 1016,
        GPS_Combined_Geometric_and_Ionospheric_Correction_Differences = 1017,
        RESERVED_for_Alternative_Ionospheric_Correction_Difference_Message = 1018,
        GPS_Ephemerides = 1019,
        GLONASS_Ephemerides = 1020
    }
    
    /// <summary>
    /// RTCM 3.X 的帧
    /// </summary>
    public class Rtcm3Frame : DataFrame
    {
        public Rtcm3Frame(byte preamble) : base(preamble)
        {

        }

        /// <summary>
        /// 6 位的保留字。
        /// </summary>
        public byte Reserved { get; set; }

        /// <summary>
        /// 信息长度,单位：字节，不含头部和校验码。0-1023。
        /// </summary>
        public int MessageLength { get; set; }
        /// <summary>
        /// 字节形式的数据内容
        /// </summary>
        public List<byte> Messages { get { return new List<byte>(this.Content.GetRange(2, MessageLength));} }

    }    
    /// <summary>
    /// 网络数据帧，由报文头部，数据体和验证区组成。
    /// 以8位的字节为基本单元。
    /// </summary>
    public class DataFrame
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="preamble">报文头标识</param>
        public DataFrame(byte preamble)
        {
            this.Preamble = preamble;
            this.Content = new List<byte>();
        }

        /// <summary>
        /// 报文头部标识。
        /// </summary>
        public byte Preamble { get; set; }
        /// <summary>
        /// 数据体
        /// </summary>
        public List<byte> Content { get; set; }
        /// <summary>
        /// 追加一个字节
        /// </summary>
        /// <param name="bt">一个字节</param>
        public void AddContent(byte bt)
        {
            this.Content.Add(bt);
        }
        /// <summary>
        /// 追加一串数据。
        /// </summary>
        /// <param name="bts">一串数据</param>
        public void AddContent(List<byte> bts)
        {
            Content.AddRange(bts); 
        }
        /// <summary>
        /// 所有的数据，含头部和数据体，不含CRC校验码。
        /// </summary>
        public List<byte> Data
        {
            get
            {
                var list = new List<byte>();
                list.Add(Preamble);
                list.AddRange(Content);
                return list;
            }
        }

        /// <summary>
        /// CRC 校验码
        /// </summary>
        public int CrcCode { get; set; }
    }

   
}