 using System;   

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;

namespace Geo.Data
{

    public class GlobkOrgItem: CoordinateRecord
    {
        /// <summary>
        /// 历元信息
        /// </summary>
        public  DateTime Epoch { get; set; }

        public double Sx { get; set; }
        public double Sy { get; set; }
        public double Sz { get; set; }
    }
    /// <summary>
    /// 通用三维坐标记录
    /// </summary>
    public class CoordinateRecord : StringId, Geo.Coordinates.IXYZ
    {
        public double B{ get; set; }
        public double L{ get; set; }
        public double H{ get; set; }
        public double X{ get; set; }
        public double Y{ get; set; }
        public double Z{ get; set; }
        public double MX{ get; set; }
        public double MY{ get; set; }
        public double MZ{ get; set; }
        public double MB{ get; set; }
        public double ML{ get; set; }
        public double MH{ get; set; }
        public string Description{ get; set; }
        public bool IsKnown { get; set; }


        public bool IsZero { get { return X == 0 && Y == 0 && Z == 0; } }
    }
}
