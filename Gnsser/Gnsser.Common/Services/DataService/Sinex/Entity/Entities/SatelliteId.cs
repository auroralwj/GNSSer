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
    ///+SATELLITE/ID
    ///G063 01 --------- P 11:197:00000 00:000:00000 BLOCK IIF 
    /// </summary>
    public class SatelliteId : IBlockItem
    {
        public string SatelliteCode { get; set; }
        public string PRN { get; set; }
        public string CosparId { get; set; }
        public string ObservationCode { get; set; }
        public Time LaunchTime { get; set; }
        public Time DecommissionTime { get; set; }
        public string AntennaType { get; set; }

        public override bool Equals(object obj)
        {
            SatelliteId site = obj as SatelliteId;
            return site == null ? false : SatelliteCode.Equals(site.SatelliteCode);
        }

        public override int GetHashCode()
        {
            return SatelliteCode.GetHashCode();
        }
        public override string ToString()
        {
            string line = "";
            line += " " + StringUtil.FillSpace(SatelliteCode, 4);
            line += " " + StringUtil.FillSpace(PRN, 2);
            line += " " + StringUtil.FillSpace(CosparId, 9);
            line += " " + StringUtil.FillSpace(ObservationCode, 1);
            line += " " + LaunchTime.ToYdsString();
            line += " " + DecommissionTime.ToYdsString();
            line += " " + StringUtil.FillSpace(AntennaType, 20);

            return line;
        }

        public static SatelliteId ParseLine(string line)
        {
            SatelliteId b = new SatelliteId();
            b.SatelliteCode = line.Substring(1, 4);
            b.PRN = line.Substring(6, 2);
            b.CosparId = line.Substring(9, 9);
            b.ObservationCode = line.Substring(19, 1);
            b.LaunchTime = Time.ParseYds(line.Substring(21, 12));
            b.DecommissionTime = Time.ParseYds(line.Substring(34, 12));
            b.AntennaType = line.Substring(47, 20);

            return b;
        }

        public  IBlockItem Init(string line)
        { 
            this.SatelliteCode = line.Substring(1, 4);
            this.PRN = line.Substring(6, 2);
            this.CosparId = line.Substring(9, 9);
            this.ObservationCode = line.Substring(19, 1);
            this.LaunchTime = Time.ParseYds(line.Substring(21, 12));
            this.DecommissionTime = Time.ParseYds(line.Substring(34, 12));
            this.AntennaType = StringUtil.SubString( line,47, 20);

            return this;
        }
    }
   
}
