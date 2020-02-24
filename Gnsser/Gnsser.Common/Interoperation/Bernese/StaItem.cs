using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// STATION NAME          FLG          FROM                   TO         OLD STATION NAME      REMARK
    /// ****************      ***  YYYY MM DD HH MM SS  YYYY MM DD HH MM SS  ********************  ************************
    /// </summary>
    public class StaItem 
    {
        /// <summary>
        /// 标记名称，如 AIRA
        /// </summary>
        public string MakerName { get; set; }
        /// <summary>
        /// 测站编号，如 21742S001
        /// </summary>
        public string MakerNumber { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 从时间
        /// </summary>
        public Time From { get; set; }
        /// <summary>
        /// 到时间
        /// </summary>
        public Time To { get; set; }
        /// <summary>
        /// 老站名
        /// </summary>
        public string OldStaName { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 从日
        /// </summary>
        public static Time fromDate = new Time(1980, 1, 6);
        /// <summary>
        /// 到日
        /// </summary>
        public static Time toDate = new Time(2099, 12, 31);
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return MakerName.GetHashCode();
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            StaItem o = obj as StaItem;
            if (o == null) return false;

            return MakerName.Equals(o.MakerName);
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MakerName;
        }
        /// <summary>
        /// 测站字符串
        /// </summary>
        /// <returns></returns>
        public string ToStaString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                StringUtil.FillSpace(MakerName + " " + MakerNumber, 22)
                + StringUtil.FillZeroLeft(Flag, 3) + "  "
                + GetGpsTimeString(From) + "  "
                + GetGpsTimeString(To) + "  "
                 + StringUtil.FillSpace(OldStaName, 20) + "  "
                 + StringUtil.FillSpace(Remark, 24));
            return sb.ToString();
        }
        /// <summary>
        /// 获取时间字符串
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public static string GetGpsTimeString(Time gpsTime)
        {
            return
                gpsTime.Year + " "
                + gpsTime.Month.ToString("00") + " "
                + gpsTime.Day.ToString("00") + " "
                + gpsTime.Hour.ToString("00") + " "
                + gpsTime.Minute.ToString("00") + " "
                + gpsTime.Seconds.ToString("00");

        }
    }

    /// <summary>
    /// RECEIVER TYPE         ANTENNA TYPE          REC #   ANT #    NORTH      EAST      UP      DESCRIPTION             REMARK
    /// ********************  ********************  ******  ******  ***.****  ***.****  ***.****  **********************  ************************ 
    /// </summary>
    public class StaInfoItem : StaItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StaInfoItem() { }

        
        /// <summary>
        /// 从 Rinex Header 中创建。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="h"></param>
        /// <param name="path"></param>
        public StaInfoItem(string name,ISiteInfo   h, string path)
        {
            int recNum = 0;
            int.TryParse(h.ReceiverNumber, out recNum);
            int antNum = 0;
            int.TryParse(h.AntennaNumber, out antNum);

            MakerName = name;
            MakerNumber = (h.MarkerNumber == null || h.MarkerNumber == "") ? "" : h.MarkerNumber.ToUpper();
            Flag = 1;
            From = fromDate;
            To = toDate;
            OldStaName = name + "*";
            Remark = "From " + Path.GetFileName(path);
            ReceiverType = h.ReceiverType == null ? " " : h.ReceiverType.ToUpper();
            AntennaType = h.AntennaType == null ? " " : h.AntennaType.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            RecNum = recNum > 999999 ? 0 : recNum;
            AntNum = antNum > 999999 ? 0 : antNum;
            NEU = CoordTransformer.HenToNeu(h.Hen);
            Discription = " ";
        }


        /// <summary>
        /// 接收类型
        /// </summary>
        public string ReceiverType { get; set; }
        /// <summary>
        /// 天线类型
        /// </summary>
        public string AntennaType { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public int RecNum { get; set; }
        /// <summary>
        /// 天线编号
        /// </summary>
        public int AntNum { get; set; }
        /// <summary>
        /// 东北天
        /// </summary>
        public NEU NEU { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Discription { get; set; }

        /// <summary>
        /// 测站信息
        /// </summary>
        /// <returns></returns>
        public string ToStaInfoString() { return ToString(); }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                StringUtil.FillSpace(MakerName + " " + MakerNumber, 22)
                + StringUtil.FillZeroLeft(Flag, 3) + "  "
                + GetGpsTimeString(From) + "  "
                + GetGpsTimeString(To) + "  "
                 + StringUtil.FillSpace(ReceiverType, 20) + "  "
                 + StringUtil.FillSpace(AntennaType, 20) + "  "
                 + StringUtil.FillSpaceLeft(RecNum, 6) + "  "
                 + StringUtil.FillSpaceLeft(AntNum, 6) + "  "
                 + GetNEUString(NEU) + "  "
                 + StringUtil.FillSpace(Discription, 24) + "  "
                 + StringUtil.FillSpace(Remark, 24));
            return sb.ToString();
        }
        /// <summary>
        /// STATION NAME          FLG          FROM                   TO         RECEIVER TYPE         ANTENNA TYPE          REC #   ANT #    NORTH      EAST      UP      DESCRIPTION             REMARK
        /// ****************      ***  YYYY MM DD HH MM SS  YYYY MM DD HH MM SS  ********************  ********************  ******  ******  ***.****  ***.****  ***.****  **********************  ************************
        /// AIRA 21742S001        001  1980 01 06 00 00 00  2099 12 31 00 00 00  TRIMBLE 4000SSI       TRM23903.00            17424       0    0.0000    0.0000    0.0000                            From AIRA1430.02O       
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static StaInfoItem ParseLine(string line)
        {
            StaInfoItem item = new StaInfoItem();
            item.MakerName = line.Substring(0, 4);
            item.MakerNumber = line.Substring(5, 11).Trim();
            item.Flag = int.Parse(line.Substring(22, 3).Trim());
            item.From = Time.Parse(line.Substring(27, 19));
            item.To = Time.Parse(line.Substring(48, 19));
            item.OldStaName = item.MakerName + "*";
            item.ReceiverType =  line.Substring(69, 20).Trim();
            item.AntennaType = line.Substring(91, 20).Trim();
            item.RecNum = int.Parse(line.Substring(114, 7).Trim());
            item.AntNum = int.Parse(line.Substring(121, 7).Trim());
            item.NEU = new NEU(double.Parse(line.Substring(129, 9).Trim()),
                double.Parse(line.Substring(139, 9).Trim()),
                 double.Parse(line.Substring(149, 9).Trim()));
            item.Discription = line.Substring(159, 22).Trim();
            item.Remark = line.Substring(183, 24).Trim(); 
            return item;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetNEUString(NEU n)
        {
            return
                StringUtil.FillSpaceLeft(n.N.ToString("0.0000"), 8) + "  "
                 + StringUtil.FillSpaceLeft(n.E.ToString("0.0000"), 8) + "  "
                 + StringUtil.FillSpaceLeft(n.U.ToString("0.0000"), 8);
        }
    }

}