//2017.07.19, czs, create in hongqing, 增加平差类型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Algorithm
{
    /// <summary>
    /// 平差类型
    /// </summary>
    public enum AdjustmentType
    {
        /// <summary>
        /// 参数平差，普通最小二乘法。
        /// </summary>
        参数平差,
        /// <summary>
        /// 条件平差
        /// </summary>
        条件平差,
        /// <summary>
        /// 具有参数的条件平差
        /// </summary>
        具有参数的条件平差,
        /// <summary>
        /// 具有条件的参数平差
        /// </summary>
        具有条件的参数平差,
        /// <summary>
        /// 卡尔曼滤波
        /// </summary>
        卡尔曼滤波,
        /// <summary>
        /// 均方根滤波
        /// </summary>
        均方根滤波,
        /// <summary>
        /// 序贯平差
        /// </summary>
        序贯平差,
        /// <summary>
        /// 参数加权平差
        /// </summary>
        参数加权平差,
        单期递归最小二乘,
        递归最小二乘,
    }
}
