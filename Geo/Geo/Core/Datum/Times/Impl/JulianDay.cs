using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Geo.Times
{
    /// <summary>
    /// 以日为基本单位表示时间，如 儒略日，新儒略日。天文历法。
    /// 儒略日比回归年长0.0078日。 400年多3.12日。闰月不同，一年12月，大小月交替，4年一闰。
    /// 儒略日以 “儒略历”公元前4713年1月1日GMT正午为第0日的开始。
    /// 而简化儒略日以 公历 1858年11月17日 GMT 0时开始。
    /// 提供整形和双精度访问接口，精度表示到纳秒以下。
    /// 内部采用 Decimal 维持，以保证精度。
    /// 如果都采用Decimal计算，则不需要采用此类。
    /// </summary>
    public class JulianDay : IJulianDay
    {
        #region 构造函数

        /// <summary>
        /// 通过decimal实例化。。
        /// </summary>
        /// <param name="days">decimal表示的日</param>
        public  JulianDay(decimal days){
            this.DecimalDays = days;
            this.Day = Decimal.ToInt32(DecimalDays);
            this.SecondOfDay = new Second(((DecimalDays - this.Day) * TimeConsts.DAY_TO_SECOND));
        }
        /// <summary>
        /// 通过双精度日实例化。精度可能有损失。
        /// </summary>
        /// <param name="days">双精度表示的日</param>
        public JulianDay(double days) 
            : this(new Decimal(days)) { }
        /// <summary>
        /// 实例化高精度对象。
        /// </summary>
        /// <param name="Day">日</param>
        /// <param name="SecondOfDay">日内秒</param>
        /// <param name="Milliseconds">毫秒</param>
        public JulianDay(Int32 Day, int SecondOfDay, double Milliseconds)
            : this(Day + SecondOfDay * TimeConsts.SECOND_TO_DAY + new Decimal(Milliseconds) * TimeConsts.MILLISECOND_TO_DAY) { }
        #endregion

        #region 属性
        #region  基本属性
        /// <summary>
        /// Decimal 表示，具有高精度。
        /// </summary>
        public Decimal DecimalDays { get; set; } 

        /// <summary>
        /// 最直接的表示，但是精度差很多。
        /// </summary>
        public double DoubleDays { get { return Decimal.ToDouble(DecimalDays); } }

        /// <summary>
        /// 日
        /// </summary>
        public Int32 Day { get; protected set; } 
        /// <summary>
        /// 秒
        /// </summary>
        public ISecond SecondOfDay { get; protected set; }
        #endregion
        
        #endregion

        #region 重写通用方法
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            JulianDay o = obj as JulianDay;
            if (o == null) return false;

            return DecimalDays == o.DecimalDays;
        }

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return DecimalDays.GetHashCode();
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DecimalDays+"";
        }
        #endregion
    }
}
