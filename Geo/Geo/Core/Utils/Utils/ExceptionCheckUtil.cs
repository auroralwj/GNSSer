using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Utils
{
    /// <summary>
    /// 用于异常检查，直接抛出异常。
    /// </summary>
    public static class ExceptionCheckUtil
    {
        /// <summary>
        /// 检查是否在范围内(含边界)，否则抛出参数异常。
        /// </summary>
        /// <param name="val">参数值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="valName">参数名称</param>
        public static void Scope(double val, double min, double max = Double.MaxValue, string valName = "valName")
        {
            if (val < min || val > max)
                throw new ArgumentException("参数不在给定范围内:[" + min + "," + max + "]", valName);
        }
        /// <summary>
        /// 是否小于
        /// </summary>
        /// <param name="val">参数值</param>
        /// <param name="max">最大值</param>
        /// <param name="valName">参数名称</param>
        public static void Smaller(double val, double max, string valName = "valName")
        {
            if (val >= max)
                throw new ArgumentException("参数应该小于: " + max, valName);
        }  
        
        /// <summary>
        /// 是否小于等于
        /// </summary>
        /// <param name="val">参数值</param>
        /// <param name="max">最大值</param>
        /// <param name="valName">参数名称</param>
        public static void EqualOrSmaller(double val, double max, string valName = "valName")
        {
            if (val > max)
                throw new ArgumentException("参数应该小于或等于: " + max, valName);
        }


        /// <summary>
        /// 是否大于
        /// </summary>
        /// <param name="val">参数值</param>
        /// <param name="min">最小值</param>
        /// <param name="valName">参数名称</param>
        public static void Larger(double val, double min, string valName = "valName")
        {
            if (val >= min)
                throw new ArgumentException("参数应该大于: " + min, valName);
        }
        /// <summary>
        /// 是否大于或等于
        /// </summary>
        /// <param name="val">参数值</param>
        /// <param name="min">最小值</param>
        /// <param name="valName">参数名称</param>
        public static void EqualOrLarger(double val, double min, string valName = "valName")
        {
            if (val > min)
                throw new ArgumentException("参数应该大于或等于: " + min, valName);
        }  
    
    }
}
