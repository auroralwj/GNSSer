//2017.08.31, czs, create in hongqing, 平差器构造工厂

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Algorithm.Adjust;

namespace Geo.Algorithm
{   
    /// <summary>
    ///平差器构造工厂
    /// </summary>
    public class AdjusterFactory
    {
        /// <summary>
        /// 生产平差器
        /// </summary> 
        /// <param name="type"></param>
        /// <returns></returns>
        public static MatrixAdjuster Create(AdjustmentType type)
        {
            MatrixAdjuster Adjustment = null;
            switch (type)
            {
                case AdjustmentType.参数平差:
                    Adjustment = new ParamAdjuster();
                    break;
                case AdjustmentType.条件平差:
                    Adjustment = new ConditionalAdjuster();
                    break;
                case AdjustmentType.具有参数的条件平差:
                    Adjustment = new ConditionalAdjusterWithParam();
                    break;
                case AdjustmentType.具有条件的参数平差:
                    Adjustment = new ParamAdjusterWithCondition();
                    break;
                case AdjustmentType.卡尔曼滤波:
                   Adjustment = new SimpleKalmanFilter();
                  // Adjustment = new KalmanFilter();
                    break;
                case AdjustmentType.均方根滤波:
                    Adjustment = new SquareRootInformationFilter();
                    break;
                case AdjustmentType.序贯平差:
                    Adjustment = new SequentialAdjuster();
                    break;
                case AdjustmentType.参数加权平差:
                    Adjustment = new WeightedParamAdjuster();
                    break;
                case AdjustmentType.递归最小二乘:
                    Adjustment = new RecursiveAdjuster();
                    break;
                case AdjustmentType.单期递归最小二乘:
                    Adjustment = new SingleRecursiveAdjuster();
                    break;
                default:
                    //Adjustment = new ParamAdjuster();
                    break;
            }
            if(Adjustment == null)
            {
                throw new Exception("尚未实现 " + type);
            }

            return Adjustment;
        }       

    }
}
