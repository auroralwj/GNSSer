//2015.01.14, czs, create in namu, 模糊度固定数学四舍五入法


using System;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;


namespace Gnsser
{ 
    /// <summary>
    /// 采用最简单的四舍五入法进行模糊度固定。
    /// </summary>
    public class MathRoundAmbiguityFixer : BaseAmbiguityFixer
    {
        public MathRoundAmbiguityFixer()
        {
        }
        /// <summary>
        /// 计算模糊度整数解。
        /// </summary>
        /// <param name="FloatAmbiguity"></param>
        /// <returns></returns>
        public override WeightedVector GetFixedAmbiguities(WeightedVector FloatAmbiguity)
        {
            Vector vector = new Vector(FloatAmbiguity.Count);
            vector.ParamNames = FloatAmbiguity.ParamNames;
            int i = 0;
            foreach (var item in FloatAmbiguity)
            {
                vector[i] = Math.Round((Double)item);
                i++;
            }
            return new WeightedVector( vector, DefaultRms);
        }
    }
}
