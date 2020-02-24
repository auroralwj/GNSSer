//2015.04.16, czs, edit in namu, 重构命名，接口提取

using System;

namespace Geo.Coordinates
{
    /// <summary>
    /// 转换接口
    /// </summary>
    public interface ICoordTransformer : ITransformer<XYZ>
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="old">原坐标</param>
        /// <returns></returns>
        XYZ Trans(XYZ old);
    }

    /// <summary>
    /// 转换接口
    /// </summary>
    public interface ITransformer <T> 
    {
        /// <summary>
        /// 执行转换
        /// </summary>
        /// <param name="old">原对象</param>
        /// <returns></returns>
        T Trans(T old);
    }
}
