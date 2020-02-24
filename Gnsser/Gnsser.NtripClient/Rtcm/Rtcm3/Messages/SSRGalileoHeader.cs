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
    /// Contents of the Message Header
    /// </summary>
    public class SSRGalileoHeader67 : SSRGpsHeader67
    {

        public static SSRGalileoHeader67 Parse(List<byte> data)
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
        public static SSRGalileoHeader67 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            SSRGalileoHeader67 header = new SSRGalileoHeader67();
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
    /// Contents of the Message Header
    /// </summary>
    public class SSRGalileoHeader68 : SSRGalileoHeader67
    {
        /// <summary>
        /// bit(1)
        /// </summary>
        public int SatlliteReferenceDatum { get; set; }

        public static SSRGalileoHeader68 Parse(List<byte> data)
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
        public static SSRGalileoHeader68 Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            SSRGalileoHeader68 header = new SSRGalileoHeader68();
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