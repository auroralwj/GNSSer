using System;
namespace Geo.Algorithm
{
    /// <summary>
    /// 插值调用器
    /// </summary>
    public interface IInterpolationCaller
    {
        /// <summary>
        /// 插值类型
        /// </summary>
        InterpolationType InterpolationType { get; set; }
        /// <summary>
        /// 阶次
        /// </summary>
        int Order { get; set; }
    }

    /// <summary>
    /// 插值类型。
    /// </summary>
    public enum InterpolationType
    {
        /// <summary>
        /// 拉格朗日插值
        /// </summary>
        LagrangeInterplation,
        /// <summary>
        /// 切比雪夫拟合插值
        /// </summary>
        ChebyshevPolyFit
    }


}
