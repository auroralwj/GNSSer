using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Gnsser.Times;
using Geo.Times; 

namespace Gnsser.Data.Sinex
{


    /// <summary>
    /// 天线信息。
    /// +SITE/ANTENNA
    /// *SITE PT SOLN TProduct DATA_START__ DATA_END____ DESCRIPTION_________ S/N__
    /// </summary>
    public class SiteAntenna : IBlockItem
    {
        public string SiteCode { get; set; }
        public string PointCode { get; set; }
        public string SolutionID { get; set; }
        public string ObservationCode { get; set; }
        public Time DateStart { get; set; }
        public Time DateEnd { get; set; }
        public string AntennaType { get; set; }
        public string AntennaSerialNumber { get; set; }

        public override bool Equals(object obj)
        {
            SiteAntenna site = obj as SiteAntenna;
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
            + " " + StringUtil.FillSpaceLeft(AntennaType, 20)
            + " " + StringUtil.FillSpaceLeft(AntennaSerialNumber, 5)
            ;

            return line;
        }

        public static SiteAntenna ParseLine(string line)
        {
            SiteAntenna b = new SiteAntenna();
            b.SiteCode = line.Substring(1, 4);
            b.PointCode = line.Substring(6, 2).Trim();
            b.SolutionID = line.Substring(9, 4).Trim();
            b.ObservationCode = line.Substring(14, 1);
            b.DateStart = Time.ParseYds(line.Substring(16, 12));
            b.DateEnd = Time.ParseYds(line.Substring(29, 12));
            b.AntennaType = line.Substring(42, 20);
            b.AntennaSerialNumber = line.Substring(63, 5);

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
            this.AntennaType = line.Substring(42, 20);
            this.AntennaSerialNumber = Geo.Utils.StringUtil.SubString( line, 63);

            return this;
        }
    }



}
