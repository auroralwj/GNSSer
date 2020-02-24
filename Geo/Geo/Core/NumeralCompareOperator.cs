//2017.09.30, czs, create in hongqing, 数字过滤

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 数字操作类型
    /// </summary>
    public enum NumeralOperationType
    {
        加,
        减,
        乘,
        除
    }


    /// <summary>
    /// 比较操作符
    /// </summary>
    public enum NumeralCompareOperator
    {
        等于,
        不等于,
        小于,
        小于等于,
        大于,
        大于等于
    }

    /// <summary>
    /// 比较操作符
    /// </summary>
    public class NumeralFilter
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="referVal"></param>
        /// <param name="NumeralCompareOperator"></param>
        public NumeralFilter(double referVal, NumeralCompareOperator NumeralCompareOperator)
        {
            this.ReferVal = referVal;
            this.NumeralCompareOperator = NumeralCompareOperator;

        }
        /// <summary>
        /// 比较符号
        /// </summary>
        public NumeralCompareOperator NumeralCompareOperator { get; set; }
        /// <summary>
        /// 参考数据
        /// </summary>
        public double ReferVal { get; set; }

        /// <summary>
        /// 过滤，如果满足要求,表示被过滤掉，则返回 true。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool Filter(double val)
        {
            return IsSatisfied(val, ReferVal, NumeralCompareOperator);
        }

        /// <summary>
        /// 过滤，如果满足要求，则返回 true。
        /// </summary>
        /// <param name="val"></param>
        /// <param name="referVal"></param>
        /// <param name="NumeralCompareOperator"></param>
        /// <returns></returns>
        public static bool IsSatisfied(double val, double referVal, NumeralCompareOperator NumeralCompareOperator)
        {
            switch (NumeralCompareOperator)
            {
                case Geo.NumeralCompareOperator.大于: return val > referVal;
                case Geo.NumeralCompareOperator.大于等于: return val >= referVal;
                case Geo.NumeralCompareOperator.等于: return val == referVal;
                case Geo.NumeralCompareOperator.不等于: return val != referVal;
                case Geo.NumeralCompareOperator.小于: return val < referVal;
                case Geo.NumeralCompareOperator.小于等于: return val <= referVal;
                default: return false;
            }
        }
    }
}
