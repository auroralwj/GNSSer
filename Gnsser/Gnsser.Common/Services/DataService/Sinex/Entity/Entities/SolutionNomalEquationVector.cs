using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Times; 

namespace Gnsser.Data.Sinex
{
    /// SOLUTION/NORMAL_EQUATION_VECTOR Block (Mandatory for normal equations)
    /// Description:
    /// If the SINEX fileB shall provide the normal equation directly this block is mandatory
    /// and contains the vector of the right hand side of the unconstrained (reduced) normal equation.
    /// Comment:
    /// The indices correspond to the indices of the SOLUTION/ESTIMATE block
    /// <summary>
    /// 如果SINEX文件直接提供法方程，则必须提供本模块，包含了此法方程的右手边。
    /// </summary>
    public class SolutionNomalEquationVector : IBlockItem
    {
        public int Index { get; set; }
        public string ParameterType { get; set; }
        public string SiteCode { get; set; }
        public string PointCode { get; set; }
        public string SolutionID { get; set; }
        public Time RefEpoch { get; set; }

        public string ParameterUnits { get; set; }
        public string ConstraintCode { get; set; }
        public double ValOfRightHand { get; set; } 

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
           + " " + StringUtil.FillSpaceLeft(ConstraintCode, 1)
           + " " + DoubleUtil.ScientificFomate(ValOfRightHand, "E21.15") 
           ;

            return line;
        }

        public  IBlockItem Init(string line)
        {
          //  SolutionValue b = new SolutionValue();
            this.Index = int.Parse(line.Substring(1, 5));
            this.ParameterType = line.Substring(7, 6);
            this.SiteCode = line.Substring(14, 4);
            this.PointCode = line.Substring(19, 2).Trim();
            this.SolutionID = line.Substring(22, 4).Trim();
            this.RefEpoch = Time.ParseYds(line.Substring(27, 12));
            this.ParameterUnits = line.Substring(40, 4).Trim();
            this.ConstraintCode = line.Substring(45, 1).Trim();
            this.ValOfRightHand = double.Parse(line.Substring(47, 21).Trim()); 

            return this;
        }
    }

}
