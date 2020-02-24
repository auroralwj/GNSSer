//2014.06.06, czs, create

using System;
namespace Geo.Coordinates
{
    /// <summary>
    /// 二维经纬度坐标。
    /// </summary>
    public interface ILonLat : IAngleUnit
    {
        /// <summary>
        /// 维度
        /// </summary>
        double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        double Lon { get; set; }
    }

    public interface IAngleUnit
    {
        /// <summary>
        /// 角度的单位
        /// </summary>
        AngleUnit Unit { get; set; }

    }
}
