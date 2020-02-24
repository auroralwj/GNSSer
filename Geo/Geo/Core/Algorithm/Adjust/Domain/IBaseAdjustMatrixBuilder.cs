using System;
using Geo.Algorithm;
using Geo;

namespace Geo.Algorithm.Adjust
{
    public interface IBaseAdjustMatrixBuilder
    {
        /// <summary>
        /// 法方程增量
        /// </summary>
        Matrix CoeffIncrementOfNormalEquation { get; set; }
        /// <summary>
        /// 近似参数
        /// </summary>
        global::Geo.Algorithm.Vector ApproxParam { get; set; }
        /// <summary>
        /// 先验参数
        /// </summary>
        global::Geo.Algorithm.Adjust.WeightedVector AprioriParam { get; }
        void Build();
        bool HasApprox { get; } 
        global::Geo.ParamNameBuilder ParamNameBuilder { get; set; }
        WeightedMatrix  Transfer { get; }

    }
}
