//2015.04.18, czs, create in namu, 高精度轻量级时间表示法， 精度达1e-13秒，分秒表示法。
//2014.04.19, cy, 将MinuteTicks类型由int改为long（时间有超限的地方，大于5000年了），Legitimize()对齐函数有错误，目前的方法考虑是否全面有待继续验证。改正了SecondsOfDay和SecondsOfWeek的赋值。
//2015.04.24， czs , edit in namu, 修正了一些错误，如 TotalDays, 修改了 Legitimize()，该类与SecondTime 测试完全等价。

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Times;

namespace Geo.Times
{
    /// <summary>
    /// 高精度轻量级时间表示法，采用分钟和秒维持， 精度达1e-13秒。
    /// 此类并没有定义起始时间，只是一个一维时间表示方法。
    /// </summary>
    public struct MinuteTime : IEquatable<MinuteTime>, IComparable<MinuteTime>, Algorithm.OneDimOperation<MinuteTime>, Geo.Times.ITime
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
        /// 一个小时的秒数量
        /// </summary>
        public const int SECOND_PER_HOUR = 3600;
        /// <summary>
        /// 60, 一小时的分钟数量
        /// </summary>
        public const int MINUTE_PER_HOUR= 60;
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
        /// 精度范围，认为精度在 1e-12 为相同，可达 1e-5 米级别的精度。
        /// </summary>
        public const double TOLERANCE = 1e-10;

        #endregion

        #region 构造与检核
         /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="dateTime">系统时间</param>
        public MinuteTime(DateTime dateTime)
            : this((int)(TimeSpan.FromTicks(dateTime.Ticks).TotalMinutes), dateTime.Second + dateTime.Millisecond / 1000.0) { }

        /// <summary>
        /// 以日历初始化
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="seconds">秒钟</param>
        public MinuteTime(int year, int month, int day, int hour = 0, int minute = 0, double seconds = 0)
        {
            var date = new DateTime(year, month, day, hour, minute, 0);
            this.MinuteTicks = (long)(TimeSpan.FromTicks(date.Ticks).TotalMinutes);
            this.FloatSeconds = seconds;

            Legitimize();
        }
        /// <summary>
        /// 以秒赋值。 
        /// </summary>
        public MinuteTime(double seconds)
        {
            this.MinuteTicks = (long)seconds / SECOND_PER_MINUTE;

            this.FloatSeconds = seconds % SECOND_PER_MINUTE;

            Legitimize();             
        }

        /// <summary>
        /// 以秒赋值。 整数秒赋值。
        /// </summary>
        public MinuteTime(int second)
        {
            this.MinuteTicks = second / SECOND_PER_MINUTE;
            this.FloatSeconds = second % SECOND_PER_MINUTE;

            Legitimize();
        }

        /// <summary>
        /// 构造函数。赋值后，做判断将其调整到合适位置。
        /// </summary>
        /// <param name="second">分钟</param> 
        /// <param name="fraction">秒</param>
        public MinuteTime(long minute, double seconds)
        {
            this.MinuteTicks = minute;
            this.FloatSeconds = seconds;

            //先赋值，再判断
            Legitimize();
        }
        /// <summary>
        /// 构造函数。赋值后，做判断将其调整到合适位置。
        /// </summary>
        /// <param name="hour">小时</param> 
        /// <param name="second">分钟</param> 
        /// <param name="fraction">秒</param>
        public MinuteTime(int hour, int minute, double seconds)
        {
            this.MinuteTicks = minute + hour * MINUTE_PER_HOUR;
            this.FloatSeconds = seconds;

            //先赋值，再判断
            Legitimize();
        }

        /// <summary>
        /// 使合法。检查各个参数的范围，并设置在合理范围内。
        /// </summary>
        public void Legitimize()
        { 
            //解题思路：使秒部分在 [0, 60) 区间，多余部分叠加到整数部分。
            int extra = 0;

            if (FloatSeconds >= 60)
            {
                extra = (int)(FloatSeconds / 60 );
            }

            if (FloatSeconds < 0)
            {
                extra = (int)Math.Floor(FloatSeconds / 60.0) ;
            }

            this.MinuteTicks += extra;
            this.FloatSeconds -= extra * 60;  

        }
        #endregion
        
        #region 核心变量
        /// <summary>
        /// 分，一分钟等于60秒。
        /// </summary>
        public long MinuteTicks; 
        /// <summary>
        /// 秒的小数部分。此数值应该在 [0 60) 区间。
        /// </summary>
        public double FloatSeconds;
        #endregion
        #region 方便属性

        /// <summary>
        /// 毫秒部分 1e-3 秒
        /// </summary>
        public double MilliSeconds { get { return  (FloatSeconds % 1 * 1000) % 1000; } }
        /// <summary>
        /// 日秒。
        /// </summary>
        public double SecondsOfDay
        { 
            get
            { 
                return ((MinuteTicks % MINUTE_PER_DAY) * SECOND_PER_MINUTE + FloatSeconds);
            }
        }
        /// <summary>
        /// 秒 [0, 60)
        /// </summary>
        public double Seconds { get { return FloatSeconds;} }

        /// <summary>
        /// 年积日
        /// </summary>
        public int DayOfYear { get { return DateTime.DayOfYear;} }

        /// <summary>
        /// 周整数。
        /// </summary>
        public int Week { get { return (int)TotalWeeks; } }

        /// <summary>
        /// 总周
        /// </summary>
        public double TotalWeeks { get { return 1.0 * MinuteTicks / MINUTE_PER_WEEK + FloatSeconds / SECOND_PER_WEEK; } }
        /// <summary>
        /// 总天数
        /// </summary>
        public double TotalDays { get { return 1.0 * MinuteTicks / MINUTE_PER_DAY + FloatSeconds / SECOND_PER_DAY; } }
        /// <summary>
        /// 总小时
        /// </summary>
        public double TotalHours { get { return 1.0 * MinuteTicks / MINUTE_PER_HOUR + FloatSeconds / SECOND_PER_HOUR; } }
        /// <summary>
        /// 总分钟
        /// </summary>
        public double TotalMinutes { get { return MinuteTicks + FloatSeconds / SECOND_PER_MINUTE; } }  

        /// <summary> 
        /// 本结构分钟可表示的最小日期
        /// </summary>
        public static  MinuteTime MinValue { get { return new MinuteTime(Int32.MinValue, 0.0); } }
        /// <summary>
        /// 本结构分钟可表示的最大日期
        /// </summary>
        public static  MinuteTime MaxValue { get { return new MinuteTime(Int32.MaxValue, 0.0); } }
        
        /// <summary>
        /// 起始时间。时间 0 点。
        /// 00:00.000 000 000 000
        /// </summary>
        public static MinuteTime Zero { get { return new MinuteTime(); } }

        /// <summary>
        /// 计算刻度起始点。起始/参考历元，设为GPS起始时间： 1980-1-6 00:00:00。
        /// 若从0年开始计算，则计算闰年将花费较长时间，从而影响效率。
        /// </summary>
        public static MinuteTime StartOfGpsT { get { return new MinuteTime(1040859360, 0.0); } }// 1980-1-6 Ticks	62451561600秒 000毫 000微 0++纳秒

        /// <summary>
        /// GPS周
        /// </summary>
        /// <returns></returns>
        public int GpsWeek { get { return (this - StartOfGpsT).Week; } }
        /// <summary>
        /// 周秒
        /// </summary>
        public double SecondsOfWeek
        {
            get
            {
                return (int)DayOfWeek * MinuteTime.SECOND_PER_DAY + this.SecondsOfDay;// this.MinuteTicks + this.FloatSeconds;
               
            }
        }


        /// <summary>
        /// 返回年月日
        /// </summary>
        public DateTime Date
        {
            get
            {
                return (DateTime.MinValue + TimeSpan.FromMinutes(this.MinuteTicks)).Date;

                //var day = (int)(TotalDays);
                //return TimeConvert.GetDate(day);
            }
        }

        /// <summary>
        /// 周几啊
        /// </summary>
        public DayOfWeek DayOfWeek { get { return DateTime.DayOfWeek;}}// (DayOfWeek)(((this - StartOfGpsT).SecondTicks / MINUTE_PER_WEEK) % 7); } }

        /// <summary>
        /// 小时 [0-24)
        /// </summary>
        public int Hour { get { return (int)(MinuteTicks / MINUTE_PER_HOUR) % HOUR_PER_DAY; } }
        /// <summary>
        /// 分钟 [0-60)
        /// </summary>
        public int Minute { get { return (int)(MinuteTicks % 60); } }
        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get { return (int)(FloatSeconds); } }

        /// <summary>
        /// 返回系统兼容时间格式
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return DateTime.MinValue + TimeSpan.FromMinutes(this.MinuteTicks) + TimeSpan.FromSeconds(FloatSeconds); 
            }
        }

        #endregion

        #region 操作数
        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static MinuteTime operator -(MinuteTime left, double seconds) { return left - MinuteTime.FromSecond(seconds); }
        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static MinuteTime operator +(MinuteTime left, double seconds) { return left + MinuteTime.FromSecond(seconds); }
        /// <summary>
        /// 取负数
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        public static MinuteTime operator -(MinuteTime left) { return Zero.Minus(left); }
        /// <summary>
        /// 减去
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static MinuteTime operator -(MinuteTime left, MinuteTime right) { return left.Minus(right); }
        /// <summary>
        /// 加上
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static MinuteTime operator +(MinuteTime left, MinuteTime right) { return left.Plus(right); }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(MinuteTime left, MinuteTime right) { return left.CompareTo(right) >= 0; }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(MinuteTime left, MinuteTime right) { return left.CompareTo(right) <= 0; }
        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(MinuteTime left, MinuteTime right) { return left.CompareTo(right) > 0; }
        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(MinuteTime left, MinuteTime right) { return left.CompareTo(right) < 0; }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(MinuteTime left, MinuteTime right) { return left.Equals(right); }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(MinuteTime left, MinuteTime right) { return !left.Equals(right); }
        #endregion

        #region override 与比较
        /// <summary>
        /// 是否相等，重写后执行效率会提高。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) { return (obj is MinuteTime) && this.Equals((MinuteTime)obj); }

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() { return MinuteTicks.GetHashCode()  + FloatSeconds.GetHashCode(); }
        /// <summary>
        /// 周 周秒.秒秒
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return MinuteTicks + " " + FloatSeconds.ToString("00.000 000 000 000 000"); }

        /// <summary>
        /// 从日开始。 60 12:00:00.012 345 678 901
        /// </summary>
        /// <returns></returns>
        public string ToTimeString()
        {
            int day = (int)MinuteTicks / MINUTE_PER_DAY;
            int hour = (int)(MinuteTicks - day * MINUTE_PER_DAY) / MINUTE_PER_HOUR;
            int minute = (int)MinuteTicks % MINUTE_PER_HOUR;

            return day.ToString("00") + " "
                + hour.ToString("00") + ":"
                + minute.ToString("00") + ":"
                + FloatSeconds.ToString("00.000 000 000");
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MinuteTime other)
        {
            return this.MinuteTicks == other.MinuteTicks  && Math.Abs(this.FloatSeconds - other.FloatSeconds) < TOLERANCE;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public long CompareTo(MinuteTime other)
        {
            if (MinuteTicks != other.MinuteTicks) return MinuteTicks - other.MinuteTicks; 

            return this.FloatSeconds.CompareTo(other.FloatSeconds);
        }
        /// <summary>
        /// 加上
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns></returns>
        public MinuteTime Plus(MinuteTime right)
        {
            return new MinuteTime(this.MinuteTicks + right.MinuteTicks,  this.FloatSeconds + right.FloatSeconds);
        }

        /// <summary>
        /// 减去
        /// </summary>
        /// <param name="right">右边</param>
        /// <returns></returns>
        public MinuteTime Minus(MinuteTime right)
        {
            return new MinuteTime(this.MinuteTicks - right.MinuteTicks, this.FloatSeconds - right.FloatSeconds);
        }
        #endregion 

        #region 构造
        /// <summary>
        /// 从周解析，精度不高。
        /// </summary>
        /// <param name="weeks">日</param>
        /// <returns></returns>
        public static MinuteTime FromWeek(double weeks)
        { 
            int minute = (int)(weeks * MINUTE_PER_WEEK);
            double seconds = (weeks - 1.0 * minute / MINUTE_PER_WEEK) * SECOND_PER_WEEK;

            return new MinuteTime(minute, seconds);
        }
        /// <summary>
        /// 从日解析，精度不高。
        /// </summary>
        /// <param name="day">日</param>
        /// <returns></returns>
        public static MinuteTime FromDay(int day)
        {
            int minute = (int)(day * MINUTE_PER_DAY);
            double seconds = (day - 1.0 * minute / MINUTE_PER_DAY) * SECOND_PER_DAY;

            return new MinuteTime(minute, seconds); 
        }
        /// <summary>
        /// 从日解析
        /// </summary>
        /// <param name="days">日</param>
        /// <returns></returns>
        public static MinuteTime FromDay(double days)
        {
            int minute = (int)(days * MINUTE_PER_DAY);
            double seconds = (days - 1.0 * minute / MINUTE_PER_DAY) * SECOND_PER_DAY;

            return new MinuteTime(minute, seconds); 
        }
        /// <summary>
        /// 从小时解析
        /// </summary>
        /// <param name="hours">小时</param>
        /// <returns></returns>
        public static MinuteTime FromHour(double hours)
        {
            int minute = (int)(hours * MINUTE_PER_HOUR);
            double seconds = (hours - 1.0 * minute / MINUTE_PER_HOUR) * SECOND_PER_HOUR;

            return new MinuteTime(minute, seconds);  
        }
        /// <summary>
        /// 从分钟解析
        /// </summary>
        /// <param name="minutes">分钟</param>
        /// <returns></returns>
        public static MinuteTime FromMinute(double minutes)
        {
            int minute = (int)(minutes);
            double seconds = (minutes - minute) * SECOND_PER_MINUTE;

            return new MinuteTime(minute, seconds); 
        }
        /// <summary>
        /// 从秒解析
        /// </summary>
        /// <param name="fraction">秒</param>
        /// <returns></returns>
        public static MinuteTime FromSecond(double seconds)  {  return new MinuteTime(seconds);   }
        #endregion 

    
        int IComparable<MinuteTime>.CompareTo(MinuteTime other)
        {
            return (int)this.CompareTo(other);
        }
    }
}