using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// ÌìÎÄ×ø±ê¡£
    /// </summary>
    interface IAstroCoord
    {
        /// <summary>
        /// North clockwise
        /// </summary>
        double Azimuth { get; set; }
        /// <summary>
        /// angle between the the vertical and the radius
        /// </summary>
        double Zenith { get; set; }

        double Radius { get; set; }

    }
}
