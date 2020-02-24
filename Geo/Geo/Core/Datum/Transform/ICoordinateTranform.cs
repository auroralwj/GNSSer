//2014.06.10,czs,edit

using System;
using System.Collections.Generic;

namespace Geo.Coordinates
{
    /// <summary>
    /// 坐标转换顶层接口,为轻量级坐标转换设计。
    /// </summary>
    /// <typeparam name="TSourceCoord">输入类型</typeparam>
    /// <typeparam name="TTargetCoord">输出类型</typeparam>
    public interface ICoordinateTranform<TSourceCoord, TTargetCoord>
        where TSourceCoord :  ICoordinate
        where TTargetCoord :  ICoordinate 
    {
        /// <summary>
        /// 将旧坐标转换成新坐标
        /// </summary>
        /// <param name="oldCoord">待转换坐标</param>
        /// <returns></returns>
        ICoordinate Trans(TSourceCoord oldCoord);
        /// <summary>
        /// 将旧坐标数组转换成新坐标数组
        /// </summary>
        /// <param name="oldCoords">待转换坐标数组</param>
        /// <returns></returns>
        IEnumerable<ICoordinate> Trans(IEnumerable<TSourceCoord> oldCoords);
    } 
}
