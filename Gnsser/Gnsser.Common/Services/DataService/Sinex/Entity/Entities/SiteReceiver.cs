using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Geo.Utils;
using Geo.Times; 

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// 接收机信息。
    /// </summary>
    public class SiteReceiver : IBlockItem
    {
        public string SiteCode { get; set; }
        public string PointCode { get; set; }
        public string SolutionID { get; set; }
        public string ObservationCode { get; set; }
        public Time DateStart { get; set; }
        public Time DateEnd { get; set; }
        public string ReceiverType { get; set; }
        public string ReceiverSerialNumber { get; set; }
        public string ReceiverFirmware { get; set; }

        public override bool Equals(object obj)
        {
            SiteReceiver site = obj as SiteReceiver;
            return site == null ? false : SiteCode.Equals(site.SiteCode);
        }

        public override int GetHashCode()
        {
            return SiteCode.GetHashCode();
        }
        /// <summary>
        /// *SITE PT SOLN TProduct DATA_START__ DATA_END____ DESCRIPTION_________ S/N__ FIRMWARE___
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string line =
              " " + StringUtil.FillSpaceLeft(SiteCode, 4)
            + " " + StringUtil.FillSpaceLeft(PointCode, 2)
            + " " + StringUtil.FillSpaceLeft(SolutionID, 4)
            + " " + ObservationCode
            + " " + DateStart.ToYdsString()
            + " " + DateEnd.ToYdsString()
            + " " + StringUtil.FillSpaceLeft(ReceiverType, 20)
            + " " + StringUtil.FillSpaceLeft(ReceiverSerialNumber, 5)
            + " " + StringUtil.FillSpaceLeft(ReceiverFirmware, 11)
            ;

            return line;
        }

        public static SiteReceiver ParseLine(string line)
        {
            SiteReceiver b = new SiteReceiver();
            b.SiteCode = line.Substring(1, 4);
            b.PointCode = line.Substring(6, 2).Trim();
            b.SolutionID = line.Substring(9, 4).Trim();
            b.ObservationCode = line.Substring(14, 1);
            b.DateStart = Time.ParseYds(line.Substring(16, 12));
            b.DateEnd = Time.ParseYds(line.Substring(29, 12));
            b.ReceiverType = line.Substring(42, 20);
            b.ReceiverSerialNumber = line.Substring(63, 5);
            b.ReceiverFirmware = line.Substring(69, 11);

            return b;
        }

        public  IBlockItem Init(string line)
        {
            this.SiteCode = line.Substring(1, 4);
            this.PointCode = line.Substring(6, 2).Trim();
            this.SolutionID = line.Substring(9, 4).Trim();
            this.ObservationCode = line.Substring(14, 1);
            this.DateStart = Time.ParseYds(line.Substring(16, 12));
            this.DateEnd = Time.ParseYds(line.Substring(29, 12));
            this.ReceiverType = Geo.Utils.StringUtil.SubString(line, 42, 20);

            this.ReceiverSerialNumber = Geo.Utils.StringUtil.SubString(line,63, 5);
            this.ReceiverFirmware = Geo.Utils.StringUtil.SubString(line, 69); 

            return this;
        }
    }
}
