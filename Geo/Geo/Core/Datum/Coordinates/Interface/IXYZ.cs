using System;
namespace Geo.Coordinates
{ 
    /// <summary>
    /// 三维坐标标接口，以 X、 Y、 Z 表示。 
    /// </summary>
    public interface IXYZ : IXY
    { 
        /// <summary>
        /// Z 轴坐标值。
        /// </summary>
        double Z { get; set; }
    } 
}
