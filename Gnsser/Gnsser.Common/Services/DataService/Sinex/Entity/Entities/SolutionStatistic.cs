using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// 统计信息。
    /// the following is from standard v2.
    /// 4) The block SOLUTION/STATISTICS is now RECOMMENDED if the requested values are 
    /// available because for coeffOfParams further combination of solutions it is necessary to have 
    /// the complete statistical information.
    /// The preference is given to the original values like 'NUMBER OF OBSERVATIONS' 
    /// and 'NUMBER OF UNKNOWNS' instead of 'DEGREE OF FREEDOM'.
    /// The 'NUMBER OF OBSERVATIONS' should represent only the number of 'real'   observations.
    /// A new value became necessary if unconstrained normal equations are stored 
    /// because the variance factor contains the constraints of the solution. Therefore 
    /// the weighted square sum of the vector 'observed minus computed' should be given 
    /// in the SOLUTION/STATISTICS block to become independent of the influence of the 
    /// constraints on the variance factor: (o-c)' P (o-c), where (o-c) represents the 
    /// vector 'observed minus computed' and P denotes the weigth matrix. This new value 
    /// can be stored under the name WEIGHTED SQUARE SUM OF O-C
    /// </summary>
    public class SolutionStatistic : IBlockItem
    {
        public string Name { get; set; }
        public Double Val { get; set; }      

        public override string ToString()
        {
            return " " + StringUtil.FillSpace(Name, 30) + " " + StringUtil.FillSpaceLeft(Val.ToString(), 22);
        }


        public  IBlockItem Init(string line)
        {
            this.Name = line.Substring(1, 30).Trim();
            this.Val = double.Parse(line.Substring(31).Trim());
            return this;
        }
    }
}
