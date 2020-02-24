using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标分量常用类型组合。
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Other,
        /// <summary>
        /// 具有 X 和 Y　坐标组合
        /// </summary>
        XY,
        /// <summary>
        /// X Y Z
        /// </summary>
        XYZ,
        /// <summary>
        /// Lon Lat
        /// </summary>
        LonLat,
        /// <summary>
        /// Lon Lat Height
        /// </summary>
        LonLatHeight,
        /// <summary>
        /// North East up
        /// </summary>
        NEU,
        /// <summary>
        /// Lon Lat Radius 球心坐标
        /// </summary>
        LonLatRadius,
        /// <summary>
        /// Height East North
        /// </summary>
        HEN,
        /// <summary>
        /// Radius Azimuth ZenithAngle
        /// </summary>
        RadiusAzimuthElevation,
        /// <summary>
        /// Radius Azimuth ZenithAngle
        /// </summary>
        RadiusAzimuth,
        /// <summary>
        /// UP East North
        /// </summary>
        UEN,
        ENU,
    }
}
