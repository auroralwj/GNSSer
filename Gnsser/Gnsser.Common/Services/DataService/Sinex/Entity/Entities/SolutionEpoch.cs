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
    /// 星历信息。
    /// </summary>
    public class SolutionEpoch : IBlockItem
    {
        public string SiteCode { get; set; }
        public string PointCode { get; set; }
        public string SolutionID { get; set; }
        public string ObservationCode { get; set; }
        public Time DateStart { get; set; }
        public Time DateEnd { get; set; }
        public Time DateMean { get; set; }

        public override bool Equals(object obj)
        {
            SolutionEpoch site = obj as SolutionEpoch;
            return site == null ? false : SiteCode.Equals(site.SiteCode);
        }

        public override int GetHashCode()
        {
            return SiteCode.GetHashCode();
        }
        public override string ToString()
        {
            string line =
             " " + StringUtil.FillSpaceLeft(SiteCode, 4)
           + " " + StringUtil.FillSpaceLeft(PointCode, 2)
           + " " + StringUtil.FillSpaceLeft(SolutionID, 4)
           + " " + ObservationCode
           + " " + DateStart.ToYdsString()
           + " " + DateEnd.ToYdsString()
           + " " + DateMean.ToYdsString()
           ;

            return line;
        }

        public static SolutionEpoch ParseLine(string line)
        {
            SolutionEpoch b = new SolutionEpoch();

            b.SiteCode = line.Substring(1, 4);
            b.PointCode = line.Substring(6, 2).Trim();
            b.SolutionID = line.Substring(9, 4).Trim();
            b.ObservationCode = line.Substring(14, 1);
            if (line.Length > 14)
            {
                b.DateStart = Time.ParseYds(Geo.Utils.StringUtil.SubString(line, 16, 12));
                b.DateEnd = Time.ParseYds(Geo.Utils.StringUtil.SubString(line, 29, 12));
                b.DateMean = Time.ParseYds(Geo.Utils.StringUtil.SubString(line, 42, 12));
            }
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
            this.DateMean = Time.ParseYds(line.Substring(42, 12));

            return this;
        }
    }
}
