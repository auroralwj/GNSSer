//2015.04.23, czs , create in namu, 秒为核心级数的高精度轻量级时间系统

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Times;

namespace Geo.Times
{
    /// <summary>
    /// 高精度轻量级时间表示法，采用秒维持， 精度达1e-15秒。
    /// 此类并没有定义起始时间，只是一个一维时间表示方法。
    /// 采用公元0年开始计时的，以整秒和秒小数标识的时间类。
    /// </summary>
    public struct SecondTime : IEquatable<SecondTime>, IComparable<SecondTime>, ITime, Algorithm.OneDimOperation<SecondTime>
    {
        #region 常量
        /// <summary>
        /// 604800, 一周秒数量
        /// </summary>
        public const int SECOND_PER_WEEK = 604800;
        /// <summary>
        /// 10080, 一周的分钟数量
        /// </summary>
        public const int MINUTE_PER_WEEK = 10080;
        /// <summary>
        /// 86400, 一天的秒数量
        /// </summary>
        public const int SECOND_PER_DAY = 86400;
        /// <summary>
        /// 3600 一个小时的秒数量
        /// </summary>
        public const int SECOND_PER_HOUR = 3600;
        /// <summary>
        /// 60, 一小时的分钟数量
        /// </summary>
        public const int MINUTE_PER_HOUR = 60;
        /// <summary>
        /// 1440, 一天的分钟数量
        /// </summary>
        public const int MINUTE_PER_DAY = 1440;
        /// <summary>
        /// 60,  一个小时的秒数量
        /// </summary>
        public const int SECOND_PER_MINUTE = 60;

        /// <summary>
        /// 24, 一天的小时数量
        /// </summary>
        public const int HOUR_PER_DAY = 24;

        /// <summary>
        /// 精度范围，认为精度在 1e-12 为相同，可达 1e-15 米级别的精度。
        /// </summary>
        public const double TOLERANCE = 1e-11;

        #endregion

        #region 构造与检核
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="dateTime">系统时间</param>
        public SecondTime(DateTime dateTime)
            : this((long)(TimeSpan.FromTicks(dateTime.Ticks).TotalSeconds), dateTime.Ticks % 1e7 / 1e7) { }

        /// <summary>
        /// 以日历初始化
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="seconds">秒钟</param>
        public SecondTime(int year, int month, int day, int hour = 0, int minute = 0, double seconds = 0)
        {
            var date = new DateTime(year, month, day, hour, minute, (int)seconds);
            this.SecondTicks = (long)(TimeSpan.FromTicks(date.Ticks).TotalSeconds);
            this.Fraction = seconds % 1;

            Legitimize();
        }
        /// <summary>
        /// 以秒赋值。 
        /// </summary>
        public SecondTime(double seconds)
        {
            this.SecondTicks = (long)(seconds);

            this.Fraction = seconds % 1;

            Legitimize();
        }


        /// <summary>
        /// 构造函数。赋值后，做判断将其调整到合适位置。
        /// </summary>
        /// <param name="second">分钟</param> 
        /// <param name="fraction">秒</param>
        public SecondTime(long second, double fraction)
        {
            this.SecondTicks = second;
            this.Fraction = fraction;

            //先赋值，再判断
            Legitimize();
        }
        /// <summary>
        /// 构造函数。赋值后，做判断将其调整到合适位置。
        /// </summary>
        /// <param name="hour">小时</param> 
        /// <param name="minute">分钟</param> 
        /// <param name="seconds">秒</param>
        public SecondTime(int hour, int minute, double seconds)
        {
            this.SecondTicks =  hour * SECOND_PER_HOUR + minute * SECOND_PER_MINUTE + (int)seconds;
            this.Fraction = seconds % 1;

            //先赋值，再判断
            Legitimize();
        }

        /// <summary>
        /// 使合法。检查各个参数的范围，并设置在合理范围内。
        /// </summary>
        public void Legitimize()
        {
            //解题思路：使小数部分在 (-1, 1) 区间，多余部分叠加到整数部分。
            //二者正负号符号相同。
            int extra = 0;

            if (Fraction >= 1)
            {
                extra = (int)Fraction; 
            }

            if (Fraction < 0)
            {
                extra = (int)Math.Floor(Fraction); 
            }

            this.SecondTicks += extra;
            this.Fraction -= extra;
        }
        #endregion

        #region 核心变量
        /// <summary>
        /// 秒计数器，整数秒。
        /// </summary>
        public long SecondTicks;
        /// <summary>
        /// 秒的小数部分。此数值应该在[0-1)。
        /// </summary>
        public double Fraction;
        #endregion

        #region 方便属性

        /// <summary>
        /// 毫秒部分 1e-3 秒
        /// </summary>
        public double MilliSeconds { get { return (Fraction * 1000) % 1000; } }
        /// <summary>
        /// 日秒。
        /// </summary>
        public double SecondsOfDay
        {
            get
            {
                return ((SecondTicks % SECOND_PER_DAY) + Fraction);
            }
        }

        /// <summary>
        /// 年秒。 
        /// </summary>
        public double SecondsOfYear
        {
            get
            {
                return (DayOfYear -1) * SECOND_PER_HOUR + SecondOfDay;
            }
        }
        /// <summary>
        /// 日内秒
        /// </summary>
        public double SecondOfDay{get{return DateTime.TimeOfDay.TotalSeconds;}}

        /// <summary>
        /// 年积日.该年中的第几天，表示为 1 和 366 之间的一个值。
        /// </summary>
        public int DayOfYear { get { return DateTime.DayOfYear; } }



        /// <summary>
        /// 周整数。
        /// </summary>
        public int Week { get { return (int)TotalWeeks; } }

        /// <summary>
        /// 总周
        /// </summary>
        public double TotalWeeks { get { return 1.0 * SecondTicks / SECOND_PER_WEEK + Fraction / SECOND_PER_WEEK; } }
        /// <summary>
        /// 总天数
        /// </summary>
        public double TotalDays { get { return 1.0 * SecondTicks / SECOND_PER_DAY + Fraction / SECOND_PER_DAY; } }
        /// <summary>
        /// 总小时
        /// </summary>
        public double TotalHours { get { return 1.0 * SecondTicks / SECOND_PER_HOUR + Fraction / SECOND_PER_HOUR; } }
        /// <summary>
        /// 总分钟
        /// </summary>
        public double TotalMinutes { get { return 1.0 * SecondTicks / SECOND_PER_MINUTE + Fraction / SECOND_PER_MINUTE; } }
        /// <summary>
        /// 总秒钟
        /// </summary>
        public double TotalSeconds { get { return  SecondTicks  + Fraction; } }

        /// <summary> 
        /// 本结构分钟可表示的最小日期
        /// </summary>
        public static SecondTime MinValue { get { return new SecondTime(Int64.MinValue, 0.0); } }
        /// <summary>
        /// 本结构分钟可表示的最大日期
        /// </summary>
        public static SecondTime MaxValue { get { return new SecondTime(Int64.MaxValue, 0.0); } }

        /// <summary>
        /// 起始时间。时间 0 点。实际为 1 年 1 月 1 日 0 时 0 分 0 秒
        /// 00:00.000 000 000 000
        /// </summary>
        public static SecondTime Zero { get { return new SecondTime(); } }

        /// <summary>
        /// 计算刻度起始点。起始/参考历元，设为GPS起始时间： 1980-1-6 00:00:00。
        /// 若从0年开始计算，则计算闰年将花费较长时间，从而影响效率。
        /// </summary>
        public static SecondTime StartOfGpsT =  new SecondTime(62451561600, 0.0); // 1980-1-6 Ticks	62451561600秒 000毫 000微 0++纳秒


        /// <summary>
        /// 计算刻度起始点。起始/参考历元，设为GPS起始时间： 2006-1-1 00:00:00。
        /// 若从0年开始计算，则计算闰年将花费较长时间，从而影响效率。
        /// </summary>
        public static SecondTime StartOfBdT = new SecondTime(new DateTime(2006, 1, 1).Ticks / 10000000, 0.0); // 1980-1-6 Ticks	62451561600秒 000毫 000微 0++纳秒

        /// <summary>
        /// GPS周
        /// </summary>
        /// <returns></returns>
        public int GpsWeek { get { return (this - StartOfGpsT).Week; } }
        /// <summary>
        /// GPS周
        /// </summary>
        /// <returns></returns>
        public int BdsWeek { get { return (this - StartOfBdT).Week; } }
        /// <summary>
        /// 周秒
        /// </summary>
        public double SecondsOfWeek
        {
            get
            {
                return (int)DayOfWeek * SecondTime.SECOND_PER_DAY + this.SecondsOfDay;
            }
        }

        /// <summary>
        /// 年内周,定义：元旦大于等于周四，则为第0周，否则为第一周。
        /// </summary>
        public int WeekOfYear
        {
            get
            {
                var newYearDay = (int)NewYearDate.DayOfWeek;
                if (newYearDay > 3) { newYearDay =  newYearDay-6; }//则为第0周
                var dayofYear = DayOfYear + newYearDay;//否则为第一周。
                return (dayofYear / 7) + 1;
            }
        }

        /// <summary>
        /// 本年元旦日期
        /// </summary>
        public DateTime NewYearDate
        {
            get
            {
                var current = DateTime;
                return new DateTime(current.Year, 1, 1); 
            }
        }

        /// <summary>
        /// 返回年月日
        /// </summary>
        public DateTime Date
        {
            get
            {
                return (DateTime.MinValue + TimeSpan.FromHours(this.TotalHours)).Date;
            }
        }

        /// <summary>
        /// 周几啊
        /// </summary>
        public DayOfWeek DayOfWeek { get { return DateTime.DayOfWeek; } }// (DayOfWeek)(((this - StartOfGpsT).SecondTicks / MINUTE_PER_WEEK) % 7); } }

        /// <summary>
        /// 小时 [0-24)
        /// </summary>
        public int Hour { get { return (int)(TotalHours % HOUR_PER_DAY); } }
        /// <summary>
        /// 分钟 [0-60)
        /// </summary>
        public int Minute { get { return (int)(TotalMinutes % 60); } }
        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get { return (int)(SecondTicks % 60); } }
        /// <summary>
        /// 秒，[0, 60)连同所有
        /// </summary>
        public double Seconds { get { return Second + Fraction; } }

        /// <summary>
        /// 返回系统兼容时间格式
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                //用小时避免数据超限
                return DateTime.MinValue + TimeSpan.FromHours(TotalHours);
            }
        }

        #endregion

        #region 操作数
        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static SecondTime operator -(SecondTime left, double seconds) { return left - SecondTime.FromSecond(seconds); }
        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static SecondTime operator +(SecondTime left, double seconds) { return left + SecondTime.FromSecond(seconds); }
        /// <summary>
        /// 取负数
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        public static SecondTime operator -(SecondTime left) { return Zero.Minus(left); }
        /// <summary>
        /// 减去
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static SecondTime operator -(SecondTime left, SecondTime right) { return left.Minus(right); }
        /// <summary>
        /// 加上
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static SecondTime operator +(SecondTime left, SecondTime right) { return left.Plus(right); }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(SecondTime left, SecondTime right) { return left.CompareTo(right) >= 0; }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(SecondTime left, SecondTime right) { return left.CompareTo(right) <= 0; }
        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(SecondTime left, SecondTime right) { return left.CompareTo(right) > 0; }
        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(SecondTime left, SecondTime right) { return left.CompareTo(right) < 0; }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(SecondTime left, SecondTime right) { return left.Equals(right); }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SecondTime left, SecondTime right) { return !left.Equals(right); }
        #endregion

        #region override 与比较
        /// <summary>
        /// 是否相等，重写后执行效率会提高。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) { return (obj is SecondTime) && this.Equals((SecondTime)obj); }

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() { return SecondTicks.GetHashCode() + Fraction.GetHashCode(); }
        /// <summary>
        /// 周 周秒.秒秒
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return SecondTicks.ToString("### ### ### ### ###") + Fraction.ToString(".000 000 000 000 000"); }

        /// <summary>
        /// 从日开始。 60 12:00:00.012 345 678 901
        /// </summary>
        /// <returns></returns>
        public string ToTimeString()
        {
            int day = (int)TotalDays;

            return day.ToString("###############00") + " "
                + Hour.ToString("00") + ":"
                + Minute.ToString("00") + ":"
                + Second.ToString("00")
                + Fraction.ToString(".000 000 000");
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SecondTime other)
        {
            return this.SecondTicks == other.SecondTicks && Math.Abs(this.Fraction - other.Fraction) < TOLERANCE;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public long CompareTo(SecondTime other)
        {
            if (SecondTicks != other.SecondTicks) return SecondTicks .CompareTo( other.SecondTicks);

            return this.Fraction.CompareTo(other.Fraction);
        }

        int IComparable<SecondTime>.CompareTo(SecondTime other) { return (int)this.CompareTo(other); }
        /// <summary>
        /// 加上
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns></returns>
        public SecondTime Plus(SecondTime right)
        {
            return new SecondTime(this.SecondTicks + right.SecondTicks, this.Fraction + right.Fraction);
        }

        /// <summary>
        /// 减去
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns></returns>
        public SecondTime Minus(SecondTime right)
        {
            return new SecondTime(this.SecondTicks - right.SecondTicks, this.Fraction - right.Fraction);
        }
        #endregion

        #region 构造
        /// <summary>
        /// 从周解析，精度不高。
        /// </summary>
        /// <param name="weeks">日</param>
        /// <returns></returns>
        public static SecondTime FromWeek(double weeks) { return FromVal(weeks, SECOND_PER_WEEK); } 
        /// <summary>
        /// 从日解析，精度不高。
        /// </summary>
        /// <param name="day">日</param>
        /// <returns></returns>
        public static SecondTime FromDay(int day) { return new SecondTime(day * SECOND_PER_DAY); }
        /// <summary>
        /// 从日解析
        /// </summary>
        /// <param name="days">日</param>
        /// <returns></returns>
        public static SecondTime FromDay(double days) { return FromVal(days, SECOND_PER_DAY); }  
        /// <summary>
        /// 从小时解析
        /// </summary>
        /// <param name="hours">小时</param>
        /// <returns></returns>
        public static SecondTime FromHour(double hours){ return FromVal(hours, SECOND_PER_HOUR); } 

        /// <summary>
        /// 从分钟解析
        /// </summary>
        /// <param name="minutes">分钟</param>
        /// <returns></returns>
        public static SecondTime FromMinute(double minutes) { return FromVal(minutes, SECOND_PER_MINUTE); }
        /// <summary>
        //
        /// 转换，
        /// </summary>
        /// <param name="val">数值</param>
        /// <param name="secondPerVal">与秒转换因子</param>
        /// <returns></returns>
        public static SecondTime FromVal( double val, int secondPerVal)
        {
            long intPart = (long)val * secondPerVal;
            double fraction = val % 1;
            double seconds = fraction * secondPerVal;

            return new SecondTime(intPart + (long)seconds, seconds % 1);
        }
        /// <summary>
        /// 从秒解析
        /// </summary>
        /// <param name="fraction">秒</param>
        /// <returns></returns>
        public static SecondTime FromSecond(double seconds) { return new SecondTime(seconds); }
        #endregion


    }
}