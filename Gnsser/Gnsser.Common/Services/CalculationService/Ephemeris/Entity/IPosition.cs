//2014.10.24, czs,  create in namu shuangliao, 位置状态接口

using System;
using Geo;
using Geo.Coordinates;

namespace Gnsser
{

    /// <summary>
    /// 位置状态接口
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// 空间直角坐标
        /// </summary>
        XYZ XYZ { get; set; }
        /// <summary>
        /// 速度，以空间直角坐标表示。
        /// </summary>
        XYZ XyzDot { get; set; }

        /// <summary>
        /// 大地坐标,以XYZ为基础，不用设置。只用读取。
        /// </summary>
        GeoCoord GeoCoord { get; }        
    }
}
