//2016.06.05, czs, add in hongqing, 提取IPlanePolar平面极坐标

using System;
namespace Geo.Coordinates
{
    /// <summary>
    /// 平面极坐标
    /// </summary>
    public interface IPlanePolar : IAngleUnit
    {   /// <summary>
        /// 角度单位
        /// </summary>
        AngleUnit Unit { get; }

        /// <summary>
        /// 方位角
        /// </summary>
        double Azimuth { get; set; } 
        /// <summary>
        /// 半径
        /// </summary>
        double Range { get; set; } 

    }

    /// <summary>
    /// 三维极坐标。
    /// </summary>
    public interface IPolar : IPlanePolar
    { 
        /// <summary>
        /// 高度角
        /// </summary>
        double Elevation { get; set; } 
        /// <summary>
        /// 天顶距角
        /// </summary>
        double Zenith { get; }
    }
}
