using System;
namespace Geo.Coordinates
{
    /// <summary>
    /// 二维平面坐标接口，以X、Y分量表示。
    /// </summary>
    public interface IXY 
    {

        /// <summary>
        /// X 坐标分量
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// Y 坐标分量
        /// </summary>
        double Y { get; set; }

        /// <summary>
        /// 坐标值是否全为 0。
        /// </summary>
        bool IsZero { get; }
    } 
    
}
