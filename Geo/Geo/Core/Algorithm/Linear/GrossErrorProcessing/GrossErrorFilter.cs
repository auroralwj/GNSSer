//2014.11.13, czs, create, in namu, 线性粗差过滤器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust {


    /// <summary>
    /// 线性粗差过滤器。
    /// 从粗差列表、数组中，找出粗差。
    /// 在相同的测量条件下的测量值序列中，超过n（3、4或2）倍中误差的测量误差
    /// </summary>
    public class GrossErrorFilter
    {
        /// <summary>
        /// 线性粗差探测器。构造函数。
        /// </summary>
        /// <param name="residuals">数据残差，误差。</param> 
        public GrossErrorFilter(IEnumerable<Double> residuals)
        {
            this.Residuals = residuals; 

             RmsCalculator rmsCalculator = new RmsCalculator(residuals);
             rmsCalculator.Calculate(); 
            this.Rms = rmsCalculator.Rms; 
        }
         
        #region 属性 
        /// <summary>
        /// 数据残差，误差。
        /// </summary>
        public IEnumerable<Double> Residuals { get; set; }
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
        /// 获取过滤粗差后的数据。循环过滤，直到所有粗差数据剔除。
        /// </summary>
        /// <param name="thresholdTimes">几倍中误差？</param>
        /// <returns></returns>
        public List<Double> Filter(double thresholdTimes)
        {
            this.ThresholdTimes = thresholdTimes;
            int resultCount = 0;

            List<Double> results = DoubleUtil.Filter(this.Residuals, LimitError);
            //已经完全满足条件
            if (results.Count == this.Residuals.Count<Double>()) return results;

            do
            {
                resultCount = results.Count;//上一次结果数量。

                GrossErrorFilter filter = new GrossErrorFilter(results);
                results = filter.Filter(thresholdTimes);

            } while (resultCount != results.Count);
            return results;
        } 
          
    }
 

}
