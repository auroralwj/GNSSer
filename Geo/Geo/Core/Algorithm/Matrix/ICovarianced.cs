//2014.12.02, czs, create in 金信粮贸 双辽, 具有协方差矩阵的对象接口。

using System;

namespace Geo.Algorithm
{
    /// <summary>
    /// 具有协方差矩阵的对象接口
    /// </summary>
    public interface ICovarianced
    {
        /// <summary>
        /// 协方差
        /// </summary>
        IMatrix Covariance { get; }
    }

    /// <summary>
    /// 具有权逆阵的对象接口
    /// </summary>
    public interface IInverseWeighted
    {
        /// <summary>
        /// 权逆阵
        /// </summary>
        IMatrix InverseWeight { get; }
    }
}
