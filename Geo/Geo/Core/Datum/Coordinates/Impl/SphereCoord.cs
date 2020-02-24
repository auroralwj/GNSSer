using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// Sphere Coordinate
    /// </summary>
    public class SphereCoord : LonLat, IAngleUnit
    {  
        /// <summary>
        /// Çò×ø±ê
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="radius"></param>
        public SphereCoord(double lon, double lat, double radius, AngleUnit Unit=  AngleUnit.Degree)
            : base(lon, lat, Unit)
        {
            this.Radius = radius;
        }
        /// <summary>
        /// ×Ö·û´®
        /// </summary>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public string ToString(string spliter)
        {
            return Lon.ToString("0.000000") + spliter + Lat.ToString("0.000000") + spliter + Radius.ToString("0.000000");
        }
        #region attribute 
        /// <summary>
        /// °ë¾¶
        /// </summary>
        public new double Radius { get; set; }
        #endregion


        /// <summary>
        /// ½Ç¶Èµ¥Î»
        /// </summary>
        public AngleUnit Unit { get; set; }
    }
}
