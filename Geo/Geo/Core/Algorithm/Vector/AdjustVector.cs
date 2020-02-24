//2014.10.02， czs, create, 海鲁吐嘎查 通辽, 创建向量接口，为后继实现自我的、快速的、大规模的矩阵计算做准备
//2014.10.08， czs, edit in hailutu, 将核心变量数组改成了列表，这样可以方便更改维数。
//2016.03.23, czs, edit in hongqing, 进行了简单的梳理和调整

using System;
using System.Text;
using System.Collections.Generic;
using Geo.Common;
using Geo.Algorithm;
using Geo.Utils;
//using Geo.Algorithm;

namespace Geo.Algorithm
{ 

    /// <summary>
    /// Value = Obs - Approx
    /// 平差用向量包含了观测值，近似值和残差。
    /// 本向量为残差向量。如果没有近似值，则为观测值向量。
    /// </summary>
    public class AdjustVector : Vector
    {
        /// <summary>
        /// 采用观测值和近似值初始化。
        /// </summary>
        /// <param name="Observation">观测值</param>
        /// <param name="Approx">近似值</param>
        public AdjustVector(IVector Observation, IVector Approx)
            : base((Observation.Minus(Approx)))
        {
            this.Observation = Observation;
            this.Approx = Approx;
        } 

        /// <summary>
        /// 观测值
        /// </summary>
        public IVector Observation { get; private set; }

        /// <summary>
        /// 近似值
        /// </summary>
        public IVector Approx { get; private set; }
    }     
}
