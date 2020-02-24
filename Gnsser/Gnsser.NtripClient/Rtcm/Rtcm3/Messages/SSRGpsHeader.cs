//2016.11.18, double, create in hongqing, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 报文头内容.共 67 位。
    /// Contents of the Message Header, Types 1058
    /// </summary>
    public class SSRGpsHeader67
    {
        #region 基本数据属性
        /// <summary>
        /// 报文编号,Uint12,12
        /// </summary>
        public MessageType MessageNumber { get; set; }
        /// <summary>
        /// uint20
        /// </summary>
        public uint EpochTime1s { get; set; }
        /// <summary>
        /// bit(4)
        /// </summary>
        public bool SSRUpdateInterval { get; set; }
        /// <summary>
        /// bit(1)
        /// </summary>
        public bool MultipleMessageIndicator { get; set; }        
        /// <summary>
        /// uint4
        /// </summary>
        public uint IODSSR { get; set; }
        /// <summary>
        /// uint16
        /// </summary>
        public uint SSRProviderID { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public uint SSRSolutionID { get; set; }
        /// <summary>
        /// uint6
        /// </summary>
        public uint NoofSatellite { get; set; }
        #endregion

        
        
        public static SSRGpsHeader67 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        /// <summary>
        /// 解析报文头文件
        /// </summary>
        /// <param name="binStr"></param>
        /// <returns></returns>
        public static SSRGpsHeader67 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            SSRGpsHeader67 header = new SSRGpsHeader67();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));
            header.EpochTime1s = BitConvertUtil.GetUInt(sequence.DeQueue(20));
            header.SSRUpdateInterval = BitConvertUtil.GetInt(sequence.DeQueue(4)) == 4;
            header.MultipleMessageIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            header.IODSSR = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.SSRProviderID = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            header.SSRSolutionID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.NoofSatellite = BitConvertUtil.GetUInt(sequence.DeQueue(6));

            return header;
        }
    }

    /// <summary>
    /// 报文头内容.共 67 位。
    /// Contents of the Message Header, Types 1058
    /// </summary>
    public class SSRGpsHeader68:SSRGpsHeader67
    {
        /// <summary>
        /// bit(1)
        /// </summary>
        public int SatlliteReferenceDatum { get; set; }
        public static SSRGpsHeader68 Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        /// <summary>
        /// 解析报文头文件
        /// </summary>
        /// <param name="binStr"></param>
        /// <returns></returns>
        public static SSRGpsHeader68 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            SSRGpsHeader68 header = new SSRGpsHeader68();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));
            header.EpochTime1s = BitConvertUtil.GetUInt(sequence.DeQueue(20));
            header.SSRUpdateInterval = BitConvertUtil.GetInt(sequence.DeQueue(4)) == 4;
            header.MultipleMessageIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            header.SatlliteReferenceDatum = BitConvertUtil.GetInt(sequence.DeQueue(1));
            header.IODSSR = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.SSRProviderID = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            header.SSRSolutionID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.NoofSatellite = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            

            return header;
        }
    }
}