//2012.05, czs, create, 卫星的出现概率。 主要用于卫星选择。
//20140.12.11, czs, edit in jinxinliaomao shuangliao, 更名为 SatelliteNumberRatio

using System;
using System.Collections.Generic;
using System.Text; 

namespace Gnsser
{
    /// <summary>
    /// 卫星出现的比率或者次数。 主要用于卫星选择。
    /// </summary>
    public class SatelliteNumberRatio : IComparable
    {
        public SatelliteNumberRatio(){ }
        public SatelliteNumberRatio( SatelliteNumber PRN ){  this.PRN = PRN;}
        /// <summary>
        /// 卫星编号
        /// </summary>
        public SatelliteNumber PRN { get; set; }
        /// <summary>
        /// 出现的比率或者次数
        /// </summary>
        public double Ratio { get; set; } 

        public override bool Equals(object obj)
        {
            SatelliteNumberRatio other = obj as SatelliteNumberRatio;
            if (obj == null) return false;
            return PRN.Equals(other.PRN);
        }
        public override int GetHashCode()
        {
            return PRN.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            SatelliteNumberRatio other = obj as SatelliteNumberRatio;
            if (Ratio < 1)
              return (int)(1 / (other.Ratio - Ratio));
            return (int)(other.Ratio - Ratio);
        }
    }
}
