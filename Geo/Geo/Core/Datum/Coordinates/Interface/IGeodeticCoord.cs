//2014.06.06, czs, create

using System;
namespace Geo.Coordinates
{
    /// <summary>
    /// 3 维大地坐标。
    /// </summary>
    public interface IGeodeticCoord: ILonLatCoord
    {
        /// <summary>
        /// 高程，包括大地高，正高等。
        /// </summary>
        double Height { get; set; }
    }
}
