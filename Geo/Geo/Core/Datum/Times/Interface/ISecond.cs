using System;

namespace Geo.Times
{
    /// <summary>
    /// 高精度秒表示，整数部分和小数部分分开表示。
    /// 提供通用类型（整型和双精度）表示，方面与外部程序对接。 
    /// </summary>
    public interface ISecond
    {
        #region 核心属性
        /// <summary>
        /// 秒的整数部分。
        /// </summary>
        Int64 Int { get; }
        /// <summary>
        /// 秒的小数部分
        /// </summary>
        ISecondFraction SecondFraction { get; }

        #endregion
        

        #region 扩展属性
        /// <summary>
        /// 秒的小数部分。以Double表示。
        /// </summary>
        Double DoubleFraction { get; }
        /// <summary>
        /// 秒的小数部分。以Decimal表示。
        /// </summary>
        Decimal DecimalFraction { get; }
        /// <summary>
        /// 秒值，以Double表示。
        /// </summary>
        Double DoubleValue { get; }
        /// <summary>
        /// 秒值，以Decimal表示。
        /// </summary>
        Decimal DecimalValue { get; }

        #endregion
    }
}
