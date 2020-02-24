//2016.08.11, czs, create in 福建永安, 小数截取器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Utils;
using Geo;
using Geo.Algorithm.Adjust;

namespace Geo.Algorithm
{
    /// <summary>
    /// 小数截取器。
    /// </summary>
    public static class FractionTruncater 
    { 
        /// <summary>
        /// 获取小数部分。
        /// 整数项无关获取小数法。
        /// </summary>
        /// <param name="vector">带权向量</param>
        /// <returns></returns>
        public static double GetIntFreeFraction(WeightedVector vector)
        {
            return Geo.Utils.DoubleUtil.GetIntFreeFraction(vector.OneDimArray, vector.GetWeightVector());


            double totalUp = 0;
            double totalDown = 0;
            double weight = 1.0;
            int length = vector.Count;
            bool hasWeight = vector.IsWeighted;

            for (int i = 0; i < length; i++)
            {
                if (hasWeight)
                {
                    weight = vector.GetWeithValue(i);
                }
                double floatNum = vector[i];

                double twoPiMulti = 2.0 * GeoConst.PI * floatNum;

                double up = weight * Math.Sin(twoPiMulti);
                double down = weight * Math.Cos(twoPiMulti);

                totalUp += up;
                totalDown += down;
            }
            double val = Math.Atan2(totalUp, totalDown);
            return val;
        }
    }
}
