//2016.01.18, double, create in zhengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 报文头内容.共 73 位。
    /// Contents of the Message Header, Types 1037,1038,1039
    /// </summary>
    public class GlonassNetworkRTKHeader : Geo.IToTabRow
    {
        #region 基本数据属性
        /// <summary>
        /// 报文编号,Uint12,12
        /// </summary>
        public MessageType MessageNumber { get; set; }
        /// <summary>
        /// uint8
        /// </summary>
        public uint NetworkID { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public uint SubnetworkID { get; set; }
        /// <summary>
        /// uint20
        /// </summary>
        public uint GlonassNetworkEpochTime { get; set; }        
        /// <summary>
        /// bit(1)
        /// </summary>
        public bool MultipleMessageIndicator { get; set; }
        /// <summary>
        /// uint12
        /// </summary>
        public uint MasterReferenceStationID { get; set; }
        /// <summary>
        /// uint12
        /// </summary>
        public uint AuxiliaryReferenceStationID { get; set; }
        /// <summary>
        /// uint4
        /// </summary>
        public uint ofGlonassDataEntries { get; set; }
        #endregion

        #region 扩展属性

        //  

        #endregion


        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MessageNumber");
            sb.Append("\t");
            sb.Append("NetworkID");
            sb.Append("\t");
            sb.Append("SubnetworkID");
            sb.Append("\t");
            sb.Append("GlonassNetworkEpochTime");
            sb.Append("\t");
            sb.Append("MultipleMessageIndicator");
            sb.Append("\t");
            sb.Append("MasterReferenceStationID");
            sb.Append("\t");
            sb.Append("AuxiliaryReferenceStationID");
            sb.Append("\t");
            sb.Append("ofGlonassDataEntries");
            return sb.ToString();
        }

        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageNumber);
            sb.Append("\t");
            sb.Append(NetworkID);
            sb.Append("\t");
            sb.Append(SubnetworkID);
            sb.Append("\t");
            sb.Append(GlonassNetworkEpochTime);
            sb.Append("\t");
            sb.Append(MultipleMessageIndicator);
            sb.Append("\t");
            sb.Append(MasterReferenceStationID);
            sb.Append("\t");
            sb.Append(AuxiliaryReferenceStationID);
            sb.Append("\t");
            sb.Append(ofGlonassDataEntries);
            return sb.ToString();
        }



        public static GlonassNetworkRTKHeader Parse(List<byte> data)
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
        public static GlonassNetworkRTKHeader Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            GlonassNetworkRTKHeader header = new GlonassNetworkRTKHeader();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));
            header.NetworkID = BitConvertUtil.GetUInt(sequence.DeQueue(8));
            header.SubnetworkID = BitConvertUtil.GetUInt(sequence.DeQueue(4));
            header.GlonassNetworkEpochTime = BitConvertUtil.GetUInt(sequence.DeQueue(20)) ;
            header.MultipleMessageIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;
            header.MasterReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            header.AuxiliaryReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12));
            header.ofGlonassDataEntries = BitConvertUtil.GetUInt(sequence.DeQueue(4));

            return header;
        }
    }
}