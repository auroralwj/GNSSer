using System;
using Geo.Utils;

namespace Geo.Times
{
    /// <summary>
    /// 表示秒以下部分， 最小单元采用双精度维持的高精度时间接口。
    /// 以通用类型（整型和双精度）表示，方面与外部程序对接。 
    /// </summary>
    public class SecondFraction : ISecondFraction
    {
        /// <summary>
        /// 以 Double 类型表示的秒小数初始化。
        /// </summary>
        /// <param name="fractionOfSecond">秒小数</param>
        public SecondFraction(Double fractionOfSecond)
        {
            if (fractionOfSecond >= 1) throw new ArgumentException("小数部分应该小于1","fractionOfSecond");
            this.DecimalValue =(Decimal) fractionOfSecond;
        }
        /// <summary>
        /// 以 Decimal 类型表示的秒小数初始化。
        /// </summary>
        /// <param name="fractionOfSecond">秒小数</param>
        public SecondFraction(decimal fractionOfSecond)
        {
            if (fractionOfSecond >= 1) throw new ArgumentException("小数部分应该小于1", "fractionOfSecond");
            this.DecimalValue = fractionOfSecond;
        }
        /// <summary>
        /// 初始化一个实例。
        /// </summary>
        /// <param name="MilliSeconds">毫秒</param>
        /// <param name="MicroSeconds">微秒</param>
        /// <param name="NanoSeconds">纳秒</param>
        public SecondFraction(int MilliSeconds, int MicroSeconds, Double NanoSeconds)
        {
            ExceptionCheckUtil.Scope(MilliSeconds, 0, 1000, "MilliSeconds");
            ExceptionCheckUtil.Scope(MicroSeconds, 0, 1000, "MicroSeconds");
            ExceptionCheckUtil.Scope(NanoSeconds, 1000, Double.MaxValue, "NanoSeconds"); 

            this.DecimalValue = MilliSeconds * 1e-3M + MicroSeconds *1e-6M + new Decimal(NanoSeconds) *1e-9M;
        }
  

        #region 属性
        /// <summary>
        /// 秒的小数部分。
        /// </summary>
        public Decimal DecimalValue { get; set; }
        /// <summary>
        /// 秒的小数部分。精度可达小数位后15位，飞母托 femto 量级。
        /// </summary>
        public Double DoubleValue { get { return Decimal.ToDouble(DecimalValue); } }
        /// <summary>
        /// 毫秒
        /// </summary>
        public int MilliSecond { get { return Decimal.ToInt32((DecimalValue* 1000)%1000); } }
        /// <summary>
        /// 微秒
        /// </summary>
        public int MicroSecond { get { return Decimal.ToInt32((DecimalValue * 1e6M) % 1000); } }
        /// <summary>
        /// 纳秒
        /// </summary>
        public Double NanoSeconds { get { return Decimal.ToDouble((DecimalValue * 1e9M) % 1000); } }


          #endregion

        #region 重写通用方法
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SecondFraction o = obj as SecondFraction;
            if (o == null) return false;

            return DecimalValue == o.DecimalValue;
        }

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return DecimalValue.GetHashCode();
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DecimalValue + "";
        }
        #endregion
    }
}
