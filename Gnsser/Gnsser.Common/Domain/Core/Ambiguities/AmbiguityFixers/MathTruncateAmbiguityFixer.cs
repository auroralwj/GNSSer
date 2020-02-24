//2015.01.14, czs, create in namu, 采用最简单的数字截断法进行模糊度固定
//2018.10.19, czs, edit in hmx， 模糊度增加RMS信息


using System;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;


namespace Gnsser
{ 
    /// <summary>
    /// 采用最简单的数字截断法进行模糊度固定。
    /// </summary>
    public class MathTruncateAmbiguityFixer : BaseAmbiguityFixer
    {
        public MathTruncateAmbiguityFixer()
        { 
        }

        /// <summary>
        /// 默认的RMS
        /// </summary>
        public double DefaultRms { get; set; }

        public override WeightedVector GetFixedAmbiguities(WeightedVector FloatAmbiguity)
        {
            Vector vector = new Vector(FloatAmbiguity.Count);
            vector.ParamNames = FloatAmbiguity.ParamNames;
            int i = 0;
            foreach (var item in FloatAmbiguity)
            {
                vector[i] = Math.Truncate((Double)item);
                i++;
            } return new WeightedVector( vector, DefaultRms);
        }
    }
}
