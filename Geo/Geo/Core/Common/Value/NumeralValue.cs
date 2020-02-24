//2014.09.16, czs, create, 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    //2016.08.19, czs, create in 江西上饶火车站, 同时具有平滑数据和原始数据
    /// <summary>
    /// 同时具有平滑数据和原始数据
    /// </summary>
    public class RawSmoothValue : NumeralValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="smoothValue"></param>
        public RawSmoothValue(double value, double smoothValue)
            : base(value)
        {
            this.SmoothValue = smoothValue;
        }
        /// <summary>
        /// 平滑数据
        /// </summary>
        public double SmoothValue { get; set; }

    }
    //2016.10.15, czs, create in hongqing,具有名称的 值与速度
    public class NamedRatedValue : RatedValue, Namable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="rate"></param>
        public NamedRatedValue(string name, double value, double rate)
            : base(value, rate)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }

    //2016.10.15, czs, create in hongqing, 值与速度
    /// <summary>
    /// 值与速度。
    /// </summary>
    public class RatedValue : NumeralValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rate"></param>
        public RatedValue(double value, double rate)
            : base(value)
        {
            this.Rate = rate;
        }

        /// <summary>
        /// 变化
        /// </summary>
        public double Rate { get; set; }
    }

    /// <summary>
    /// 具有一个双精度Value属性。
    /// </summary>
    public  class NumeralValue : BaseValue<Double> , INumeralValue
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="val"></param>
        public NumeralValue(double val= 0):base(val)
        { 

        }

        /// <summary>
        /// 是否值为 0 
        /// </summary>
        public bool IsZero { get { return this.Equals(Zero); } }


        /// <summary>
        /// 值为0。
        /// </summary>
        public static NumeralValue Zero { get { return new NumeralValue(); } }
    }
     
}
