//2016.04.02, czs, create in 洪庆, 一个数字，分成了整数部分和小数部分
//2016.08.20, czs, edit in fujian yongan, 小数部分不一定在0到1之间。可以手动指定。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{ 
    /// <summary>
    /// 一个数字，分成了整数部分和小数部分。
    /// 小数部分不一定在0到1之间。可以手动指定。
    /// </summary>
    public class IntFractionNumber : NumeralValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fraction"></param>
        /// <param name="integer"></param>
        public IntFractionNumber(double fraction, int integer):base(fraction + integer)
        {
            this.Fraction = fraction;
            this.Int = integer;
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="val"></param>
        /// <param name="IsPositiveFraction"></param>
        public IntFractionNumber(double val = 0, bool IsPositiveFraction=true)
            : base(val)
        { 
            this.Int = (int)Value;
            if (IsPositiveFraction && Value < 0) { this.Int = this.Int - 1; } 
            this.Fraction = val - Int;
        }
        #region 属性 
        /// <summary>
        /// 整数部分
        /// </summary>
        public int Int { get; set; }

        /// <summary>
        /// 小数部分
        /// </summary>
        public double Fraction { get; set; }
        #endregion
    }
    
}
