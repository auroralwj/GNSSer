//2014.10.24, czs,  edit in namu shuangliao,  分解为位置和钟差
//2014.12.26, czs,  edit in namu shuangliao,  增加精度信息实现  IRmsed<XYZ> 

using System;
using Gnsser.Times;
using Geo;
using Geo.Coordinates;

namespace Gnsser
{
    /// <summary>
    /// 星历接口。主要包含卫星的位置、钟差，及其变化量。
    /// </summary>
    public interface IEphemeris : IPosition, IClockBias, IComparable<IEphemeris>, Geo.IToTabRow, IRmsed<XYZ> 
    {

        /// <summary>
        /// 钟差的相对论改正，单位为秒。通过卫星相对速度计算而来。
        /// </summary>
        double RelativeCorrection { get; set; }    
    }
}
