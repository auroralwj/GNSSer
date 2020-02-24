//2016.01.18, double, create in zhengzhou, RTCM 消息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using Geo;


namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 报文头内容.共 46 位。
    /// Contents of the Message Header, Types 1034
    /// </summary>
    public class GlonassNetworkFKPGradientHeader : Geo.IToTabRow
    {
        #region 基本数据属性
        /// <summary>
        /// 报文编号,Uint12,12
        /// </summary>
        public MessageType MessageNumber { get; set; }
        /// <summary>
        /// 参考站ID,Uint12,12
        /// </summary>
        public uint ReferenceStationID { get; set; }
        /// <summary>
        /// uint17
        /// </summary>
        public uint GlonassFKPEpochTime { get; set; }
        /// <summary>
        /// uint5
        /// </summary>
        public uint NoofGlonassSatelliteSignalsProcessed { get; set; }
        #endregion

        #region 扩展属性

      //  public double GpsSeconds { get { return this.GpsEpochTimeInMs / 1000.0; } }

        #endregion


        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MessageNumber");
            sb.Append("\t");
            sb.Append("ReferenceStationID");
            sb.Append("\t");
            sb.Append("GlonassFKPEpochTime");
            sb.Append("\t");
            sb.Append("NoofGlonassSatelliteSignalsProcessed");
            return sb.ToString();
        }
        
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageNumber);
            sb.Append("\t");
            sb.Append(ReferenceStationID);
            sb.Append("\t");
            sb.Append(GlonassFKPEpochTime);
            sb.Append("\t");
            sb.Append(NoofGlonassSatelliteSignalsProcessed);
            return sb.ToString();
        }



        public static GlonassNetworkFKPGradientHeader Parse(List<byte> data)
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
        public static GlonassNetworkFKPGradientHeader Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            GlonassNetworkFKPGradientHeader header = new GlonassNetworkFKPGradientHeader();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));
            header.ReferenceStationID = BitConvertUtil.GetUInt(sequence.DeQueue(12)); 
            header.GlonassFKPEpochTime = BitConvertUtil.GetUInt(sequence.DeQueue(27)); 
            header.NoofGlonassSatelliteSignalsProcessed = BitConvertUtil.GetUInt(sequence.DeQueue(5)) ; 

            return header;
        }
    }
}