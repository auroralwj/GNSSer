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
    /// 报文头内容.共 64 位。
    /// Contents of the Message Header
    /// </summary>
    public class SSRGlonassHeader64 : SSRGpsHeader67
    {
        
        public static SSRGlonassHeader64 Parse(List<byte> data)
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
        public static SSRGlonassHeader64 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            SSRGlonassHeader64 header = new SSRGlonassHeader64();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));
            header.EpochTime1s = BitConvertUtil.GetUInt(sequence.DeQueue(17))+17;
            header.SSRUpdateInterval = BitConvertUtil.GetInt(sequence.DeQueue(4)) == 4;
            header.MultipleMessageIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            header.IODSSR = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.SSRProviderID = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            header.SSRSolutionID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.NoofSatellite = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            if (header.EpochTime1s >= 3 * 3600)
                header.EpochTime1s -= 3 * 3600;
            else header.EpochTime1s += 21 * 3600 ;
            return header;
        }
    }

    /// <summary>
    /// 报文头内容.共 65 位。
    /// Contents of the Message Header
    /// </summary>
    public class SSRGlonassHeader65 : SSRGlonassHeader64
    {

        /// <summary>
        /// bit(1)
        /// </summary>
        public int SatlliteReferenceDatum { get; set; }
        public static SSRGlonassHeader65 Parse(List<byte> data)
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
        public static SSRGlonassHeader65 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            SSRGlonassHeader65 header = new SSRGlonassHeader65();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));
            header.EpochTime1s = BitConvertUtil.GetUInt(sequence.DeQueue(17))+17;
            header.SSRUpdateInterval = BitConvertUtil.GetInt(sequence.DeQueue(4)) == 4;
            header.MultipleMessageIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            header.SatlliteReferenceDatum = BitConvertUtil.GetInt(sequence.DeQueue(1));
            header.IODSSR = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.SSRProviderID = BitConvertUtil.GetUInt(sequence.DeQueue(16));
            header.SSRSolutionID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.NoofSatellite = BitConvertUtil.GetUInt(sequence.DeQueue(6));
            if (header.EpochTime1s >= 3 * 3600)
                header.EpochTime1s -= 3 * 3600;
            else header.EpochTime1s += 21 * 3600 ;
            return header;
        }
    }
}