using System;

namespace Geo.Times
{
    /// <summary>
    /// 表示秒以下部分， 最小单元采用双精度维持的高精度时间接口。
    /// 以通用类型（整型和双精度）表示，方面与外部程序对接。 
    /// </summary>
    public interface ISecondFraction
    {
        /// <summary>
        /// 秒的小数部分。以Decimal表示。
        /// </summary>
        Decimal DecimalValue { get; }
        /// <summary>
        /// 秒的小数部分。以Double表示。
        /// </summary>
        Double DoubleValue { get; }
        /// <summary>
        /// 微秒
        /// </summary>
        int MilliSecond { get; }
        /// <summary>
        /// 毫秒
        /// </summary>
        int MicroSecond { get; }
        /// <summary>
        /// 纳秒
        /// </summary>
        Double NanoSeconds { get; }
    }
}
