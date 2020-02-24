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
    /// 参数值，通常包括方差先验值或方差估计值。
    /// </summary>
    public class SolutionValue : IBlockItem
    {
        public SolutionValue() { }
        public SolutionValue(
            int Index,
            string SiteCode,
            string ParameterType,
            double ParameterValue,
            Time RefEpoch,
            double StdDev = 0,
            string SolutionID = "0001",
            string ParameterUnits = "m",
            string ConstraintCode = "0",
            string PointCode = "A"
            )
        {
            this.Index = Index;
            this.SiteCode = SiteCode;
            this.ParameterType = ParameterType;
            this.ParameterValue = ParameterValue;
            this.StdDev = StdDev;
            this.RefEpoch = RefEpoch;
            this.SolutionID = SolutionID;
            this.ParameterUnits = ParameterUnits;
            this.ConstraintCode = ConstraintCode;
            this.PointCode = PointCode;
        }

        public int Index { get; set; }
        public string ParameterType { get; set; }
        public string SiteCode { get; set; }
        public string PointCode { get; set; }
        public string SolutionID { get; set; }
        public Time RefEpoch { get; set; }

        public string ParameterUnits { get; set; }
        public string ConstraintCode { get; set; }
        public double ParameterValue { get; set; }

        /// <summary>
        /// Estimated standard deviation for the parameter.
        /// </summary>
        public double StdDev { get; set; }

        public override bool Equals(object obj)
        {
            SolutionValue o = obj as SolutionValue;
            if (o == null) return false;

            return this.SiteCode.Equals(o.SiteCode) && this.ParameterType == o.ParameterType;
        }
        public override int GetHashCode()
        {
            return SiteCode.GetHashCode() + ParameterType.GetHashCode();
        }

        public override string ToString()
        {
            string line =
              " " + StringUtil.FillSpaceLeft(Index, 5)
           + " " + StringUtil.FillSpaceLeft(ParameterType, 6)
           + " " + StringUtil.FillSpaceLeft(SiteCode, 4)
           + " " + StringUtil.FillSpaceLeft(PointCode, 2)
           + " " + StringUtil.FillSpaceLeft(SolutionID, 4)
           + " " + RefEpoch.ToYdsString()
           + " " + StringUtil.FillSpace(ParameterUnits, 4)
           + " " + ConstraintCode

           + " " + DoubleUtil.ScientificFomate(ParameterValue, "E21.15")
           + " " + DoubleUtil.ScientificFomate(StdDev, "E11.6", false)
           ;

            return line;
        }

        public  IBlockItem Init(string line)
        {
            //SolutionValue b = new SolutionValue();
            this.Index = int.Parse(line.Substring(1, 5));
            this.ParameterType = line.Substring(7, 6).Trim();
            this.SiteCode = line.Substring(14, 4).Trim();
            this.PointCode = line.Substring(19, 2).Trim();
            this.SolutionID = line.Substring(22, 4).Trim();
            this.RefEpoch = Time.ParseYds(line.Substring(27, 12));
            this.ParameterUnits = line.Substring(40, 4).Trim();
            this.ConstraintCode = line.Substring(45, 1).Trim();
            this.ParameterValue = double.Parse(line.Substring(47, 21).Trim());
            this.StdDev = double.Parse(Geo.Utils.StringUtil.SubString(line, 69, 11).Trim());

            return this;
        }
        public static SolutionValue ParseLine(string line)
        {
            SolutionValue b = new SolutionValue();
            b.Index = int.Parse(line.Substring(1, 5));
            b.ParameterType = line.Substring(7, 6).Trim();
            b.SiteCode = line.Substring(14, 4).Trim();
            b.PointCode = line.Substring(19, 2).Trim();
            b.SolutionID = line.Substring(22, 4).Trim();
            b.RefEpoch = Time.ParseYds(line.Substring(27, 12));
            b.ParameterUnits = line.Substring(40, 4).Trim();
            b.ConstraintCode = line.Substring(45, 1).Trim();
            b.ParameterValue = double.Parse(line.Substring(47, 21).Trim());
            b.StdDev = double.Parse(line.Substring(69, 11).Trim());

            return b;
        }

    }
}
