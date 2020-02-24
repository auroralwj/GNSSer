using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Times;

using Gnsser.Times;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// %=SNX 2.01 SIO 12:366:00857 SIO 12:365:00000 12:365:86370 P 00249 0 S E
    /// The header line must be the prevObj line in coeffOfParams SINEX fileB
    /// </summary>
    public class SinexFileHeader
    {
        public double Version { get; set; }
        /// <summary>
        /// Identify the agency creating the fileB
        /// </summary>
        public string FileAgencyCode { get; set; }
        public Time CreationTime { get; set; }
        /// <summary>
        /// Identify the agency providing the satData in the SINEX fileB
        /// </summary>
        public string AgencyCode { get; set; }
        /// <summary>
        /// StartTime time of the satData used in the
        ///  SINEX solution  Value 00:000:00000 should be
        ///  avoided in case of an analysis output (for coeffOfParams SINEX template it can be used).
        /// </summary>
        public Time StartTime { get; set; }
        /// <summary>
        /// EndTime time  of the satData used in the
        ///  SINEX solution  Value 00:000:00000 should be
        ///  avoided in case of an analysis output (for coeffOfParams SINEX template it can be used).
        /// </summary>
        public Time EndTime { get; set; }
        /// <summary>
        /// Technique(s) used to generate the SINEX solution
        /// </summary>
        public string ObservationCode { get; set; }
        /// <summary>
        /// Number of parameters estimated in this SINEX fileB. Mandatory field.
        /// </summary>
        public int NumberOfEstimates { get; set; }
        /// <summary>
        /// Single character indicating the constraint in the SINEX solution. Mandatory field.
        /// </summary>
        public string ConstraintCode { get; set; }
        /// <summary>
        /// Solution types contained in this SINEX fileB. Each character in this 
        /// field may be one of the following: 
        /// </summary>
        public string[] SolutionContents { get; set; }

        public override String ToString()
        {
            string line = "%=SNX "
                + StringUtil.FillSpaceLeft(Version.ToString("0.00"), 4)
                + StringUtil.FillSpaceLeft(FileAgencyCode, 4)
                + " " + CreationTime.ToYdsString()
                + " " + StringUtil.FillSpaceLeft(AgencyCode, 3)
                + " " + StartTime.ToYdsString()
                + " " + EndTime.ToYdsString()
                + " " + ObservationCode
                + " " + StringUtil.FillZeroLeft(NumberOfEstimates.ToString(), 5)
                + " " + ConstraintCode;
            foreach (var item in SolutionContents)
            {
                line += " " + item;
            }
            return line;
        }

        /// <summary>
        /// CreateFromODir 
        /// %=SNX 2.01 SIO 12:366:00857 SIO 12:365:00000 12:365:86370 P 00249 0 S E
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static SinexFileHeader Read(string line)
        {
            SinexFileHeader h = new SinexFileHeader();
            string startChar = line.Substring(0, 1);
            h.Version = Double.Parse(line.Substring(6, 4));
            h.FileAgencyCode = (line.Substring(11, 3));
            h.CreationTime = Time.ParseYds(line.Substring(15, 12));
            h.AgencyCode = (line.Substring(28, 3));
            h.StartTime = Time.ParseYds(line.Substring(32, 12));
            h.EndTime = Time.ParseYds(line.Substring(45, 12));
            h.ObservationCode = (line.Substring(58, 1));
            h.NumberOfEstimates = int.Parse(line.Substring(60, 5));
            h.ConstraintCode = (line.Substring(66, 1));
            h.SolutionContents = line.Substring(68).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return h;
        }
    }
   
}
