//2018.07.31, czs, create in hmx, 提取模糊度固定结果的接口
//2018.10.16, czs, edit in hmx, IAmbiguityResult 更名为 IFixableParamResult


using Geo.Algorithm;
using Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
    /// <summary>
    /// 模糊度/固定参数结果的接口
    /// </summary>
    public interface IFixableParamResult: IAdjustmentResult
    {
        /// <summary>
        ///已固定的模糊度，固定参数等
        /// </summary>
        WeightedVector FixedParams { get; set; }

        /// <summary>
        /// 提取可用于固定的参数加权向量，且转换为用于固定的单位，如原始载波以周为单位的模糊度，用于载波计算，如果是xyz，则不必转换。
        /// </summary>
        /// <returns></returns>
        WeightedVector GetFixableVectorInUnit();
    }
}