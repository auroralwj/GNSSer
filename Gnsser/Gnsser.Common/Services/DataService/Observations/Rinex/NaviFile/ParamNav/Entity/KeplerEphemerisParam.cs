
//2016.05.31 , czs, ceate in hongqing, 简单6参数轨道星历


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Geo.Coordinates;

namespace Gnsser
{
    /// <summary>
    /// 简单的开普勒轨道6根数
    /// </summary>
    public class KeplerEphemerisParam : SatClockBias
    {

        #region 开普勒轨道根数 kepler elements
        /// <summary>
        /// 星历的参考时刻（GPS周内的秒数）
        /// </summary>
        public double Toe { get; set; }
        /// <summary>
        /// Toe时刻的轨道长半径平方根。square root of semimajor axis           Sqrt(m)  
        /// </summary>
        public double SqrtA { get; set; }
        /// <summary>
        /// e, Toe时刻的偏心率，eccentricity
        /// </summary>
        public double Eccentricity { get; set; }
        /// <summary>
        /// i, Toe时刻的轨道倾角，inclination at ref. epoch  rad
        /// </summary>
        public double Inclination { get; set; }
        /// <summary>
        /// Ω， Toe时刻的升交点赤经。right ascension of asc. node at ref. epoch (r)
        /// </summary>
        public double LongOfAscension { get; set; }
        /// <summary>
        /// 小Ω，argument of perigee                        (r)
        /// </summary>
        public double ArgumentOfPerigee { get; set; }
        /// <summary>
        /// M, Toe时刻的平近点角， mean anomaly at ref. epoch                 (r)
        /// </summary>
        public double MeanAnomaly { get; set; }
        #endregion
    }

}