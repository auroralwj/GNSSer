//2014.06.12, czs, created 

using System;
namespace Geo.Referencing
{
    /// <summary>
    /// 坐标参考系工厂接口
    /// </summary>
    public interface ICrsFactory
    {
        /// <summary>
        /// 创建一个坐标参考系。
        /// </summary>
        /// <param name="coordinateSystem">坐标系统</param>
        /// <param name="datum">基准</param>
        /// <returns></returns>
        ICoordinateReferenceSystem Create(ICoordinateSystem coordinateSystem, IDatum datum);
    }
}
