//2014.11.08, czs, create in mamu, 滑动平均插值器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 滑动平均插值器。
    /// </summary>
    public class MovingAverageInterpolater : YGetter
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ys"></param>
        /// <param name="order"></param>
        public MovingAverageInterpolater(double[] ys, int order = 10)
        {
            this.Ys = ys;
            this.Order = order;
            this.Currents = new List<double>();
        }
        /// <summary>
        /// 滑动个数。
        /// </summary>
        public int Order { get; protected set; }

        double[] Ys { get; set; }

        List<double> Currents { get; set; }
        /// <summary>
        /// 获取Y值
        /// </summary>
        /// <param name="xValue"></param>
        /// <returns></returns>
        public override double GetY(double xValue)
        {
            var nexts = GetNexts((int)xValue);
            double temp = DoubleUtil.Average(nexts);
            return temp;
        }
        /// <summary>
        /// 获取接下来的
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public List<double> GetNexts(int from)
        {
            List<double> ys = new List<double>();

            for (int i = from; i < from + Order && i < Ys.Length; i++)
            {
                ys.Add( Ys[i]);
            }
            return ys;
        }
    }
}
