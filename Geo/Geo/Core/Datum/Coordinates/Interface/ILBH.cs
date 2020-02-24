//2015.06.25, czs, create in namu, 增加大地坐标 LBH 接口。

using System;
namespace Geo.Coordinates
{ 
    /// <summary>
    /// 三维大地坐标标接口，以L、B、H 表示。 
    /// </summary>
    public interface ILBH
    {
        /// <summary>
        /// 经度。
        /// </summary>
        double L { get; set; }
        /// <summary>
        /// 经度。
        /// </summary>
        double B { get; set; }
        /// <summary>
        /// 高程。
        /// </summary>
        double H { get; set; }
    } 
}
