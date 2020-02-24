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
    /// 偏心信息。
    /// *SITE PT SOLN TProduct DATA_START__ DATA_END____ AXE ARP->BENCHMARK(M)_________
    /// AJAC  A 0001 P 02:143:00000 02:143:86370 UNE   0.0000   0.0000   0.0000
    /// </summary>
    public class SiteEccentricity : IBlockItem
    {
        public string SiteCode { get; set; }
        public string PointCode { get; set; }
        public string SolutionID { get; set; }
        public string ObservationCode { get; set; }
        public Time DateStart { get; set; }
        public Time DateEnd { get; set; }
        public string EccentricityReferenceSystem { get; set; }
        public Geo.Coordinates.HEN Une { get; set; }

        public override bool Equals(object obj)
        {
            SiteEccentricity site = obj as SiteEccentricity;
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
           + " " + StringUtil.FillSpaceLeft(EccentricityReferenceSystem, 3)
           + " " + Une.ToRnxString(8.4)
           ;

            return line;
        }

        public static SiteEccentricity ParseLine(string line)
        {
            SiteEccentricity b = new SiteEccentricity();

            b.SiteCode = line.Substring(1, 4);
            b.PointCode = line.Substring(6, 2).Trim();
            b.SolutionID = line.Substring(9, 4).Trim();
            b.ObservationCode = line.Substring(14, 1);
            b.DateStart = Time.ParseYds(line.Substring(16, 12));
            b.DateEnd = Time.ParseYds(line.Substring(29, 12));
            b.EccentricityReferenceSystem = line.Substring(42, 3);
            string[] strs = line.Substring(46, 26).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            b.Une = new HEN(double.Parse(strs[i++]), double.Parse(strs[i++]), double.Parse(strs[i++]));

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
            this.EccentricityReferenceSystem = line.Substring(42, 3);
            string[] strs = line.Substring(46, 26).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            this.Une = HEN.TryParse(strs[i++], strs[i++], strs[i++]);

            return this;
        }
    }

}
