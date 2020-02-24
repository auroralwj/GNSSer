using System;

namespace Geo.Times
{
    /// <summary>
    /// 高精度秒表示，整数部分和小数部分分开表示。
    /// 提供通用类型（整型和双精度）表示，方面与外部程序对接。 
    /// </summary>
    public class Second: ISecond, IEquatable<Second> , IComparable<Second>
    {
        #region 构造函数
        /// <summary>
        /// 以秒初始化
        /// </summary>
        /// <param name="second">秒</param>
        public Second(Double second)
            : this((int)second, new SecondFraction(second % 1.0D)) { }
        /// <summary>
        /// 以秒初始化
        /// </summary>
        /// <param name="second">秒</param>
        public Second(Decimal second)
            : this((int)second, new SecondFraction(second % 1.0M)) { }

        /// <summary>
        /// 以整数和小数初始化。
        /// </summary>
        /// <param name="Int">整数部分</param>
        /// <param name="fraction">小数部分</param>
        public Second(Int64 Int, Double fraction)
            : this(Int, new SecondFraction(fraction)) { }

        /// <summary>
        /// 以整数和小数初始化。
        /// </summary>
        /// <param name="Int">整数部分</param>
        /// <param name="fraction">小数部分</param>
        public Second(Int64 Int, Decimal fraction)
            : this(Int, new SecondFraction(fraction)) { }
        /// <summary>
        /// 以整数和小数初始化。
        /// </summary>
        /// <param name="Int">整数部分</param>
        /// <param name="fraction">小数部分</param>
        public Second(Int64 Int, ISecondFraction fraction)
        {
            this.Int = Int;
            this.SecondFraction = fraction;
        }

        #endregion

        #region 核心属性
        /// <summary>
        /// 秒的精度
        /// </summary>
        public decimal Tolerance = (1e-15M);

        /// <summary>
        /// 秒的整数部分。
        /// </summary>
        public Int64 Int { get; protected set; }
        /// <summary>
        /// 秒的小数部分
        /// </summary>
        public ISecondFraction SecondFraction { get; protected set; }

        #endregion

        #region 便利属性
        /// <summary>
        /// 秒的小数部分。以Double表示。
        /// </summary>
        public Double DoubleFraction { get { return SecondFraction.DoubleValue; } }
        /// <summary>
        /// 秒的小数部分。以Decimal表示。
        /// </summary>
        public Decimal DecimalFraction { get { return SecondFraction.DecimalValue; } }
        /// <summary>
        /// 秒值，以Double表示。
        /// </summary>
        public Double DoubleValue { get { return Int + DoubleFraction; } }
        /// <summary>
        /// 秒值，以Decimal表示。
        /// </summary>
        public Decimal DecimalValue { get { return Int + DecimalFraction; } }

        #endregion

        #region 方法重写,接口实现
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Second o = obj as Second;
            if (o == null) return false;

            return Equals(o);
        }

        /// <summary>
        ///  是否相等。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Second other)
        {
            return Math.Abs(this.DecimalValue - other.DecimalValue) < Tolerance;
          //  return this.DecimalValue == other.DecimalValue;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.DecimalValue.GetHashCode();
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DecimalValue + "";
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Second other)
        {
            return (int)(this.DecimalValue - other.DecimalValue);
        }
        #endregion
    }
}
