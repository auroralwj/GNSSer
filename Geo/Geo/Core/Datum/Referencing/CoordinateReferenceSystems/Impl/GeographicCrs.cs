//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 基于大地水准面的地理坐标系的参照系统，由于是曲面上，因此不适用于制图，制图用平面坐标系。
    /// </summary>
    public class GeographicCrs : GeodeticCrs
    {
        /// <summary>
        /// 椭球地理坐标系统
        /// </summary>
        EllipsoidalCs EllipsoidalCs { get; set; }
    }
}