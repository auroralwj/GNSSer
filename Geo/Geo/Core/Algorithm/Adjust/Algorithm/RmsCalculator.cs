//2014.09.18, czs, create, in hailutu, 均方根计算器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    
    /// <summary>
    /// 均方根计算器
    /// </summary>
    public class RmsCalculator
    {
        /// <summary>
        /// 线性粗差探测器。构造函数。
        /// </summary>
        /// <param name="errors"></param>
        public RmsCalculator(IEnumerable<Double> errors)
        {
            this.errors = errors; 
        }

        IEnumerable<Double> errors;

        /// <summary>
        /// 均方根
        /// </summary>
        public double Rms { get; private set; }
        /// <summary>
        /// 计算赋值。
        /// </summary>
        /// <param name="errors">误差序列</param>
        public void Calculate()
        {
            double meanSquare = 0;
            double allSquare = 0;
            int count = 0;
            foreach (var item in errors)
            {
                double squre = item * item;
                allSquare += squre;
                count++;
            }
            meanSquare = allSquare / count;
             
            this.Rms = Math.Sqrt( meanSquare);
        } 

          
    }
}
