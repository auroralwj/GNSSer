//2014.11.13, czs, create, in namu, 线性粗差过滤器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{

    /// <summary>
    /// 线性粗差过滤器。
    /// 从粗差列表、数组中，找出粗差。
    /// 在相同的测量条件下的测量值序列中，超过n（3、4或2）倍中误差的测量误差
    /// </summary>
    public class DicGrossErrorFilter
    {
        /// <summary>
        /// 线性粗差探测器。构造函数。
        /// </summary>
        /// <param name="residuals">数据残差，误差。</param> 
        public DicGrossErrorFilter(Dictionary<Double, Double> residuals)
        {
            this.Residuals = residuals;

            RmsCalculator rmsCalculator = new RmsCalculator(residuals.Values);
            rmsCalculator.Calculate();
            this.Rms = rmsCalculator.Rms;
        }

        #region 属性
        /// <summary>
        /// 数据残差，误差。
        /// </summary>
        public Dictionary<Double, Double> Residuals { get; set; }
        //1.limit error
        //2.在一定观测条件下偶然误差的绝对值不应超过的限值 
        /// <summary>
        ///  限差。一般为 2、3、4 倍中误差。
        /// </summary>
        public double LimitError { get { return Rms * ThresholdTimes; } }
        /// <summary>
        /// 中误差。
        /// </summary>
        public double Rms { get; set; }
        /// <summary>
        /// 中误差倍数
        /// </summary>
        public double ThresholdTimes { get; set; }
        #endregion

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="thresholdTimes"></param>
        /// <returns></returns>
        public Dictionary<Double, Double> Filter(double thresholdTimes)
        {
            this.ThresholdTimes = thresholdTimes;
            int resultCount = 0;

            Dictionary<Double, Double> results = DoubleUtil.GetAbsFiltedDic(this.Residuals, LimitError);
            //已经完全满足条件
            if (results.Count == this.Residuals.Keys.Count) return results;

            do
            {
                resultCount = results.Count;//上一次结果数量。

                DicGrossErrorFilter filter = new DicGrossErrorFilter(results);
                results = filter.Filter(thresholdTimes);

            } while (resultCount != results.Count);
            return results;
        }
    }
}
