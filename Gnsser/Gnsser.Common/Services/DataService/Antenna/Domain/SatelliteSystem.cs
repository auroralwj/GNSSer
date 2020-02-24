using System;
using System.Collections.Generic;

using System.Text;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// RINEX卫星系统类型。
    /// 与 SatelliteType 不同。
    /// </summary>
    public enum SatelliteSystem
    {
        /// <summary>
        /// GPS
        /// </summary>
        G,
        /// <summary>
        /// GLONASS
        /// </summary>
        R,
        /// <summary>
        /// Galileo
        /// </summary>
        E,
        /// <summary>
        /// SBAS payload
        /// </summary>
        S,
        /// <summary>
        /// Mixed
        /// </summary>
        M,
        /// <summary>
        /// Compass
        /// </summary>
        C
    }
    
}
