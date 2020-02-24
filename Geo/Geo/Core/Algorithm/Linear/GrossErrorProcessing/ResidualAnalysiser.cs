//2014.11.13, czs, create, in namu, 线性粗差过滤器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust {

    /// <summary>
    /// 残差分析结果
    /// </summary>
    public class ResidualAnalysiser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="residuals">数据残差</param>
        public ResidualAnalysiser(double[] residuals)
        {
            this.Residuals = residuals;
            this.Count = this.Residuals.Length;

            //绝对值的平均数
            double averageOfAbs = 0;
            foreach (var item in this.Residuals)
            {
                averageOfAbs += Math.Abs(item) / this.Count;
            }
            this.AverageOfAbs = averageOfAbs;

            RmsCalculator = new RmsCalculator(residuals);
            RmsCalculator.Calculate();
            this.Rms = RmsCalculator.Rms;
        }

        RmsCalculator RmsCalculator;

        /// <summary>
        /// 残差
        /// </summary>
        public double[] Residuals { get; set; }
        /// <summary>
        /// 绝对值的平均值。
        /// </summary>
        public double AverageOfAbs { get; set; }
        /// <summary>
        /// 均方根
        /// </summary>
        public double Rms { get; set; }

        /// <summary>
        /// 分析数据的数量。
        /// </summary>
        public int Count { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Count\t" + Count);
            sb.AppendLine("AverageOfAbs\t" + AverageOfAbs);
            sb.AppendLine("Rms\t" + Rms);

            return sb.ToString();
        }
    }
}
