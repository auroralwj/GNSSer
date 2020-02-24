//2015.02.16, czs, create in pengzhou, RTCM 消息

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
    /// 同一历元的信息
    /// </summary>
    public class EpochMessage : Geo.BaseDictionary<SatelliteNumber, NormalMessage>
    {
        public EpochMessage()
        { 
        }
        /// <summary>
        ///  GPS 头部信息，具有时间变量。
        /// </summary>
        public NormalHeader Header { get; set; }


    }


    /// <summary>
    /// 报文头内容.共 64 位。
    /// Contents of the Message Header, Types 1001, 1002, 1003, 1004: GPS RTK Messages
    /// </summary>
    public class GpsMessageHeader :Geo.IToTabRow
    {
        #region 基本数据属性
        /// <summary>
        /// 报文编号,Uint12,12
        /// </summary>
        public MessageType MessageNumber { get; set; }
        /// <summary>
        /// 参考站ID,Uint12,12
        /// </summary>
        public int ReferenceStationID { get; set; }
        /// <summary>
        /// GPS 历元时刻,Uint30,30,GPS Epoch Time(TOW) 0-604,799,999 ms 1 ms uint30
        /// </summary>
        public uint GpsEpochTimeInMs { get; set; }
        /// <summary>
        /// 同步GNSS标识，Bit（1）,1
        /// </summary>
        public bool SyncGnssMessageFlag { get; set; }
        /// <summary>
        /// 报文条数，Uint5,5
        /// </summary>
        public int MessageCount { get; set; }
        /// <summary>
        /// GPS 散度平滑标识，Bit（1）,1 ， GPS Divergencefree Smoothing Indicator
        ///  if false : Divergence-free smoothing not used
        /// </summary>
        public bool GpsDivergencefreeSmoothingIndicator { get; set; }
        /// <summary>
        /// GPS平滑间隔数，Bit（3）,3
        /// </summary>
        public int GpsSmoothingInterval { get; set; }
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
            sb.Append("GpsEpochTimeInMs");
            sb.Append("\t");
            sb.Append("SyncGnssMessageFlag");
            sb.Append("\t");
            sb.Append("MessageCount");
            sb.Append("\t");
            sb.Append("GpsDivergencefreeSmoothingIndicator");
            sb.Append("\t");
            sb.Append("GpsSmoothingInterval");  
            return sb.ToString();
        }
        
        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageNumber);
            sb.Append("\t");
            sb.Append(ReferenceStationID);
            sb.Append("\t");
            sb.Append(GpsEpochTimeInMs);
            sb.Append("\t");
            sb.Append(SyncGnssMessageFlag);
            sb.Append("\t");
            sb.Append(MessageCount);
            sb.Append("\t");
            sb.Append(GpsDivergencefreeSmoothingIndicator);
            sb.Append("\t");
            sb.Append(GpsSmoothingInterval);
            return sb.ToString();
        }



        public static GpsMessageHeader Parse(List<byte> data)
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
        public static GpsMessageHeader Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            GpsMessageHeader header = new GpsMessageHeader();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));//uint12 
            header.ReferenceStationID = BitConvertUtil.GetInt(sequence.DeQueue(12));//uint12 
            //var GpsEpochTimeInMs = BitConvertUtil.GetUInt(sequence.DeQueue(30));//uint30 
            header.GpsEpochTimeInMs = BitConvertUtil.GetUInt(sequence.DeQueue(30));//uint30 
            header.SyncGnssMessageFlag = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit（1） 
            header.MessageCount = BitConvertUtil.GetInt(sequence.DeQueue(5));//uint5 
            header.GpsDivergencefreeSmoothingIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit（1） 
            header.GpsSmoothingInterval = BitConvertUtil.GetInt(sequence.DeQueue(3));//bit（3）   

            return header;
        }
    }
}