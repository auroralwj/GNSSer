//2014.06.04，czs, create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Referencing
{
    /// <summary>
    /// 坐标轴名称。
    /// 这是一个可以常常需要变化的文件，如果未包含制定类型，可以从这里添加。
    /// </summary>
    public enum Ordinate
    {
        /// <summary>
        /// 未指定坐标。
        /// </summary>
        Other,
        /// <summary>
        /// X 坐标
        /// </summary>
        X,
        /// <summary>
        /// Y 坐标
        /// </summary>
        Y,
        /// <summary>
        /// Z 坐标
        /// </summary>
        Z,
        /// <summary>
        /// Lon 坐标
        /// </summary>
        Lon,
        /// <summary>
        /// Lat 坐标
        /// </summary>
        Lat,
        /// <summary>
        /// Height 坐标
        /// </summary>
        Height,
        /// <summary>
        /// Time 坐标
        /// </summary>
        Time,
        /// <summary>
        /// 半径 坐标。
        /// </summary>
        Radius,
        /// <summary>
        ///北方向 常用于站心坐标
        /// </summary>
        North,
        /// <summary>
        ///东方向 常用于站心坐标
        /// </summary>
        East,
        /// <summary>
        ///上方向 常用于站心坐标
        /// </summary>
        Up,
        /// <summary>
        /// 方位角
        /// </summary>
        Azimuth, 
        /// <summary>
        /// 高度角 同 Lat ？
        /// </summary>
        ElevatAngle
    }
}
