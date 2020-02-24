//2018.09.02, czs, create in hmx, 参数固定的二次平差

using System;
using System.Collections.Generic; 
using System.Text;  
using Geo.Algorithm.Adjust; 
using Geo.Utils;
using Geo;
using Gnsser;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 参数固定的二次平差，用于模糊度固定，坐标固定等
    /// </summary>
    public class ParamFixedAdjuster   : MatrixAdjuster
    {
        /// <summary>
        /// 手动矩阵生成器
        /// </summary>
        public ParamFixedAdjuster()
        {
            ConditionalAdjuster = new ConditionalAdjuster();
        }
        /// <summary>
        /// 条件平差
        /// </summary>
        ConditionalAdjuster ConditionalAdjuster { get; set; }
        /// <summary>
        /// 浮点解
        /// </summary>
        public WeightedVector FloatParams { get; set; }
        /// <summary>
        /// 固定解
        /// </summary>
        public WeightedVector FixedParams { get; set; }
        /// <summary>
        /// 控制矩阵
        /// </summary>
        public IMatrix ControlMatrix { get; set; }

        public override AdjustResultMatrix Run(AdjustObsMatrix input)
        {




            var result = ConditionalAdjuster.Run(input);





            throw new NotImplementedException();
        }
    }
}