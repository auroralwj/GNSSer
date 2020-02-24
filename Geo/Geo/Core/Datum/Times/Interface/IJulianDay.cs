using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Geo.Times
{
    /// <summary>
    /// 以日为基本单位表示时间，如 儒略日，新儒略日。
    /// 提供整形和双精度访问接口，精度表示到纳秒以下。
    /// 内部采用 Decimal 维持，以保证精度。
    /// 如果都采用Decimal计算，则不需要采用此类。
    /// </summary>
    public interface IJulianDay
    {

        #region 核心属性
        /// <summary>
        /// Decimal 表示，具有高精度。
        /// </summary>
        Decimal DecimalDays { get; }
        /// <summary>
        /// 整数日.
        /// </summary>
        int Day { get; }
        /// <summary>
        /// 日内秒。
        /// </summary>
        ISecond SecondOfDay { get; }
        #endregion

        #region 便利属性

        /// <summary>
        /// 最直接的表示，但是精度差很多。
        /// </summary>
        double DoubleDays { get; }


        #endregion
    }
}
