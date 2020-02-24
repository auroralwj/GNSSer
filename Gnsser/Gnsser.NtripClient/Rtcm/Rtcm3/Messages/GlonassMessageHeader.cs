//2015.02.17, czs, create in pengzhou, RTCM 消息
//2016.10.15 double edit in hongqing 按照GpsMessageHeader进行修改
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 同一时段的信息
    /// </summary>
    public class GlonassEpochMessage : Geo.BaseDictionary<SatelliteNumber, GlonassNormalMessage>
    {
        /// <summary>
        /// 闰秒
        /// </summary>
        public static double LeapSecondOfUtcToGps{get;set;}

        public GlonassEpochMessage()
        {
            //1980.01.06，utc leap 9s
            LeapSecondOfUtcToGps = Winform.Setting.GnsserConfig.LeapSecond - 9.0;
        }

        public GlonassNormalHeader Header { get; set; }


    }
    /// <summary>
    /// 消息头内容.共 61 位。
    /// Contents of the Message Header, Types 1009 through 1012: GLONASS RTK Messages
    /// </summary>
    public class GlonassMessageHeader:Geo.IToTabRow
    {
        /// <summary>
        /// 闰秒
        /// </summary>
        public static double LeapSecondOfUtcToGps{get;set;}

        public GlonassMessageHeader()
        {
            //1980.01.06，utc leap 9s
            LeapSecondOfUtcToGps = Winform.Setting.GnsserConfig.LeapSecond - 9.0;
        }
        #region 基本数据属性
        /// <summary>
        /// 消息编号,Uint,12
        /// </summary>
        public MessageType MessageNumber { get; set; }
        /// <summary>
        /// 参考站ID,Uint,12
        /// </summary>
        public int ReferenceStationID { get; set; }
        /// <summary>
        ///  Glonass 历元时刻,Uint,27
        /// </summary>
        public double  GlonassEpochTimeTk { get; set; }
        /// <summary>
        /// 同步GNSS标识，Bit,1
        /// </summary>
        public bool SyncGnssMessageFlag { get; set; }
        /// <summary>
        /// 消息条数，Uint,5
        /// </summary>
        public int MessageCount { get; set; }
        /// <summary>
        /// GPS 散度平滑标识，Bit,1 ， GPS Divergencefree Smoothing Indicator
        ///  if false : Divergence-free smoothing not used
        /// </summary>
        public bool GlonassDivergencefreeSmoothingIndicator { get; set; }
        /// <summary>
        /// GPS平滑间隔数，Bit,3
        /// </summary>
        public int GlonassSmoothingInterval { get; set; }///采用int型？
        #endregion

        public string GetTabTitles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MessageNumber");
            sb.Append("\t");
            sb.Append("ReferenceStationID");
            sb.Append("\t");
            sb.Append("GlonassEpochTimeTk");
            sb.Append("\t");
            sb.Append("SyncGnssMessageFlag");
            sb.Append("\t");
            sb.Append("MessageCount");
            sb.Append("\t");
            sb.Append("GlonassDivergencefreeSmoothingIndicator");
            sb.Append("\t");
            sb.Append("GlonassSmoothingInterval");
            return sb.ToString();
        }

        public string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageNumber);
            sb.Append("\t");
            sb.Append(ReferenceStationID);
            sb.Append("\t");
            sb.Append(GlonassEpochTimeTk);
            sb.Append("\t");
            sb.Append(SyncGnssMessageFlag);
            sb.Append("\t");
            sb.Append(MessageCount);
            sb.Append("\t");
            sb.Append(GlonassDivergencefreeSmoothingIndicator);
            sb.Append("\t");
            sb.Append(GlonassSmoothingInterval);
            return sb.ToString();
        }

        public static GlonassMessageHeader Parse(List<byte> data)
        {
            //首先化为二进制字符串
            string binStr = BitConvertUtil.GetBinString(data);
            return Parse(binStr);
        }

        public static GlonassMessageHeader Parse(string binStr)
        {
            StringSequence sequence = new StringSequence();
            sequence.EnQuence(binStr);

            GlonassMessageHeader header = new GlonassMessageHeader();
            header.MessageNumber = (MessageType)BitConvertUtil.GetInt(sequence.DeQueue(12));//uint12 
            header.ReferenceStationID = BitConvertUtil.GetInt(sequence.DeQueue(12));//uint12 
            header.GlonassEpochTimeTk = BitConvertUtil.GetInt(sequence.DeQueue(27)) * 0.001 + LeapSecondOfUtcToGps;//uint27 ,question：这里是加17还是18还是19？
            header.SyncGnssMessageFlag = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1) 
            header.MessageCount = BitConvertUtil.GetInt(sequence.DeQueue(5));//uint5 
            header.GlonassDivergencefreeSmoothingIndicator = BitConvertUtil.GetInt(sequence.DeQueue(1)) == 1;//bit(1)
            header.GlonassSmoothingInterval = BitConvertUtil.GetInt(sequence.DeQueue(3));//bit(3)  
            //if (header.GlonassEpochTimeTk > 10800)
            //    header.GlonassEpochTimeTk -= 10800;
            //else header.GlonassEpochTimeTk += 21 * 3600;
            header.GlonassEpochTimeTk -= 10800;
            return header;
        }
    }
}