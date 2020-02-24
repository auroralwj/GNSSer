//2014.09.04, czs, create, 进一步对平差进行封装

using System;
using System.Collections.Generic; 
using System.Text; 
using Geo.Algorithm;
using Geo.Algorithm.Adjust; 
using Geo.Utils;
using Geo;
using Gnsser; 

namespace Geo.Algorithm.Adjust
{
    public abstract class BaseAdjustMatrixBuilder< TProduct,TMaterial> : BaseAdjustMatrixBuilder
    {

    } 
    /// <summary>
    /// 平差计算中需要转换的矩阵。
    /// </summary>
    public abstract class BaseAdjustMatrixBuilder : SimpleAdjustMatrixBuilder, IBaseAdjustMatrixBuilder
    {
        /// <summary>
        /// 平差计算中需要转换的矩阵，构造函数
        /// </summary>
        public BaseAdjustMatrixBuilder()
        {

        }
        /// <summary>
        /// 法方程增量
        /// </summary>
        public virtual Matrix CoeffIncrementOfNormalEquation { get; set; }
        /// <summary>
        /// 参数是否具有初始值。
        /// </summary>
        public bool HasApprox { get { return this.ApproxParam != null; } }
        /// <summary>
        /// 参数是否具有先验值。
        /// </summary>
        public bool HasApriori { get { return this.AprioriParam != null; } }
        /// <summary>
        /// 创建状态转移矩阵和噪声,注意：大多数状态转移模型为两个对角线矩阵。
        /// </summary>  
        public virtual WeightedMatrix Transfer { get; protected set; }
        /// <summary>
        /// 第二创建状态转移矩阵和噪声,注意：大多数状态转移模型为两个对角线矩阵。
        /// </summary>  
        public virtual WeightedMatrix SecondTransfer { get; protected set; }

        /// <summary>
        /// 参数近似值，是参数实体，非改正项，可以为空
        /// </summary>
        public virtual Vector ApproxParam { get; set; }
        /// <summary>
        ///参数先验值，具有权阵。
        ///如果以改正数作为参数进行计算，则数值向量通常为 0 
        /// </summary>
        public abstract WeightedVector AprioriParam { get; }
        /// <summary>
        /// 观测方程中第二系数阵，如在具有参数的条件平差
        /// </summary>
        public virtual Matrix SecondCoefficient { get => null; }
        /// <summary>
        /// 第二参数近似向量
        /// </summary>
        public virtual Vector SecondApproxVector { get => null; }
        /// <summary>
        /// 扩展平差误差方程的常数项，自由项，维度与系数阵行数相同，只有数值没有权。
        /// 如果 FreeVector 不够用，则用这个
        /// </summary>
        public Vector SecondFreeVector { get => null; }

        /// <summary>
        /// 参数名称生成器
        /// </summary>
        public virtual ParamNameBuilder ParamNameBuilder { get; set; }

        /// <summary>
        /// 生成
        /// </summary>
        public virtual void Build() {
        
        
        }

    }
}
