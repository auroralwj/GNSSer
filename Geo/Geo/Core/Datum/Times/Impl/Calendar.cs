using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Geo.Times
{
    /// <summary>
    /// 高精度， 日历型时间类。
    /// </summary>
    public class Calendar :  ICalendar, IEquatable<Calendar>
    {
        #region 构造函数
        public Calendar(int year, int dayOfYear, int hour, int minute, Decimal seconds)
            :this(
            year,
            TimeConvert.GetMonth(year, dayOfYear),
            TimeConvert.GetDayOfMonth(year, dayOfYear),
            hour,
            minute,
            seconds  )
        {

        }



        /// <summary>
        /// 以儒略日初始化
        /// </summary>
        /// <param name="JulianDay">儒略日</param>
        /// <param name="isMjd">是否是平儒略日</param>
        public Calendar(decimal JulianDay, bool isMjd = false)
        {
            if (isMjd)
            {
                JulianDay += 2400000.5m;
            }

            Int32 Day;
            Int32 Month;
            Int32 Year;
            Int32 Hour;
            Int32 Minute;
            Int32 Second;
            Decimal milliSeconds;
            TimeConvert.JdToCalendar(JulianDay, out Day, out Month, out Year, out Hour, out Minute, out Second, out milliSeconds);
           //  this(Year, Month, Day, Hour, Minute, Second, milliSeconds);

             this.Year = Year;
             this.Month = Month;
             this.Day = Day;
             this.Hour = Hour;
             this.Minute = Minute;
             this.Second = Second;
             this.Seconds = Second + (milliSeconds / 1000.0M);
             this.MilliSeconds = milliSeconds;
             this.MilliSecond = (int)milliSeconds;
             this.SecondFraction = new SecondFraction(this.MilliSeconds / 1000M);
        }
        /// <summary>
        /// 以系时间类初始化。
        /// </summary>
        /// <param name="time">系统时间，可以精确表示到100毫微秒（10 ^-7秒）即0.1毫秒</param>
        /// <param name="milliseconds">如果指定，则采用本毫秒(1000毫秒 = 1秒 )</param>
        public Calendar(DateTime time, Decimal milliseconds = 0M)
            : this((Int32)time.Year, (Int32)time.Month, (Int32)time.Day, (Int32)time.Hour, (Int32)time.Minute, (Int32)time.Second,
            (milliseconds == 0M ? (time.Ticks % (1E7M)) / 10000M : milliseconds))
        {
        }
        /// <summary>
        /// 以日历数字进行初始化
        /// </summary>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <param name="Day">日</param>
        /// <param name="Hour">时</param>
        /// <param name="Minute">分</param>
        /// <param name="Second">秒</param>
        /// <param name="milliSeconds">毫秒（秒的小数部分）</param>
        public Calendar(
        Int32 Year,
        Int32 Month,
        Int32 Day,
        Int32 Hour,
        Int32 Minute,
        Int32 Second,
        Decimal milliSeconds)
        {

            if (Month < 1 || Month > 12
                || Hour < 0 || Hour > 23
                || Minute < 0 || Minute > 59
                || Second < 0 || Second > 59
                || milliSeconds < 0 || milliSeconds > 1000
                )
                throw new ArgumentException("时间范围不对！");

            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
            this.Seconds = Second + (milliSeconds / 1000.0M);
            this.MilliSeconds = milliSeconds;
            this.MilliSecond = (int)milliSeconds;
            this.SecondFraction = new SecondFraction(this.MilliSeconds / 1000M);
        }
        public Calendar(
            Int32 Year = 1,
            Int32 Month = 1,
            Int32 Day = 1,
            Int32 Hour = 0,
            Int32 Minute = 0,
            Decimal Seconds = 0)
        {

            if (Month < 1 || Month > 12
                || Hour < 0 || Hour > 23
                || Minute < 0 || Minute > 59
                )
                throw new ArgumentException("时间范围不对！");

            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = (int)Seconds;
            this.Seconds = Seconds;
            this.MilliSeconds = Seconds%1M * 1000M;
            this.MilliSecond = (int)MilliSeconds;
            this.SecondFraction = new SecondFraction(this.MilliSeconds / 1000M);
        }
        #endregion

        #region 属性
        #region 基本属性
        /// <summary>
        /// 年
        /// </summary>
        public Int32 Year { get; protected set; }
        /// <summary>
        /// 月
        /// </summary>
        public Int32 Month { get; protected set; }
        /// <summary>
        /// 天
        /// </summary>
        public Int32 Day { get; protected set; }
        /// <summary>
        /// 小时
        /// </summary>
        public Int32 Hour { get; protected set; }
        /// <summary>
        /// 分钟
        /// </summary>
        public Int32 Minute { get; protected set; }
        /// <summary>
        /// 秒
        /// </summary>
        public Int32 Second { get; protected set; }
        /// <summary>
        /// 秒
        /// </summary>
        public decimal Seconds { get; protected set; }
        /// <summary>
        /// 微秒
        /// </summary>
        public Int32 MilliSecond { get; protected set; }
        /// <summary>
        /// 毫秒（秒的小数部分）
        /// </summary>
        public Decimal MilliSeconds { get; protected set; }
        /// <summary>
        /// 小数部分的高精度表示。
        /// </summary>
        public ISecondFraction SecondFraction { get; protected set; }
        /// <summary>
        /// 周几
        /// </summary>
        public DayOfWeek DayOfWeek { get { 
            int day = (int)((MJulianDay + 3) % 7);
            return (DayOfWeek)(day);
        }
        }

        #endregion

        #region 扩展属性
        /// <summary>
        /// 年纪日
        /// </summary>
        public Int32 DayOfYear { get {return TimeConvert.GetDayOfYear(Year, Month, Day); } }
        /// <summary>
        /// 是否闰年
        /// </summary>
        public bool IsLeapYear { get { return TimeConvert.IsGregorianLeapYear(Year); } }
        /// <summary>
        /// 儒略日
        /// </summary>
        public Decimal JulianDay
        {
            get
            {
                return TimeConvert.CalendarToJulianDay(this);
            }
        }
        /// <summary>
        /// 平儒略日
        /// </summary>
        public Decimal MJulianDay
        {
            get
            {
                return TimeConvert.CalendarToMjd(this);
            }
        }


        /// <summary>
        /// 系统时间核心计数。精度是 0.1 微秒， 即 100 纳秒
        /// </summary>
        public long DateTimeTicks
        {
            get
            {
                decimal passedDays = TimeConvert.GetPassedDaysFromYearOne(this);

              //  return DateTime.Ticks;
               // long passedDays = (long)(JulianDay - Zero.JulianDay);//与儒略历有15天偏差。
                long ticks = Decimal.ToInt64(passedDays * TimeConsts.DAY_TO_MICROSECOND * 10);
                return ticks;
            }
        }

        /// <summary>
        /// 返回系统时间，精度到微秒。
        /// </summary>
        //public DateTime DateTime { get { return new DateTime(Year, Month, Day, Hour, Minute, Second, MilliSecond); } }
        public DateTime DateTime { get { return new DateTime(DateTimeTicks); } }
        /// <summary>
        /// 年月日部分。
        /// </summary>
        public Calendar Date { get { return new Calendar(Year, Month, Day); } }
        /// <summary>
        /// 从 0 时开始的时间段。
        /// </summary>
        public CalendarSpan TimeOfDay
        {
            get
            {
                return new CalendarSpan(Date, this);
            }
        }
        #endregion
        #endregion

        #region 常用静态

        /// <summary>
        /// 公元元年 0001年 01 月 01 日。没有 10 天的跳变。是理想的格里历。
        /// </summary>
        public static Calendar Zero { get { return new Calendar(); } }
        /// <summary>
        /// 当前系统时间。
        /// </summary>
        public static Calendar Now { get { return new Calendar(DateTime.Now); } }

        #endregion

        #region 重写方法

        /// <summary>
        /// 是否相等. 比较到皮秒(10^-12)级别。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Calendar g = obj as Calendar;
            if (g == null) return false;

            return Equals(g);
        }

        public bool Equals(Calendar other)
        {
            return
             Year == other.Year
          && Month == other.Month
          && Day == other.Day
          && Hour == other.Hour
          && Minute == other.Minute
          && Second == other.Second
          && Math.Abs(MilliSeconds - other.MilliSeconds) < 1e-12M;
        }

        public override int GetHashCode()
        {
            return Year.GetHashCode() * 13
                + Month.GetHashCode() * 3
                + Day.GetHashCode() * 13
                + Hour.GetHashCode() * 3
                + Minute.GetHashCode() * 3
                + Second.GetHashCode() * 7
                + MilliSeconds.GetHashCode() * 13;
        }

        /// <summary>
        /// 2002-05-23 12:00:00.000
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Year + "-" +
                Month.ToString("00") + "-" +
                Day.ToString("00") + " " +
                Hour.ToString("00") + ":" +
                Minute.ToString("00") + ":" +
                Second.ToString("00") +
                (MilliSeconds * 0.001m).ToString(".000 000 000 000");
        }
        #endregion

        #region 操作数
        /// <summary>
        /// 将指定的日期和时间与另一个指定的日期和时间相减，返回一个时间间隔。
        /// </summary>
        /// <param name="d1">System.Calendar（被减数）。</param>
        /// <param name="d2"> System.Calendar（减数）。</param>
        /// <returns>CalendarSpan，它是 d1 和 d2 之间的时间间隔，即 d1 减去 d2。</returns>
        public static CalendarSpan operator -(Calendar d1, Calendar d2) {
            return CalendarSpan.FromDays(TimeConvert.GetPassedDays(d2, d1));
        }
     
        /// <summary>
        /// 从指定的日期和时间减去指定的时间间隔，返回新的日期和时间。
        /// </summary>
        /// <param name="d">Calendar</param>
        /// <param name="t">TimeSpan</param>
        /// <returns>Calendar，它的值为 d 的值减去 t 的值。</returns>
        public static Calendar operator -(Calendar d, TimeSpan t) { return new Calendar(d.JulianDay - (Decimal)t.Ticks / TimeConsts.TicksPerDay); }
        public static Calendar operator -(Calendar d, CalendarSpan t) { return new Calendar(d.JulianDay - t.TotalDays); }
       
        /// <summary>
        ///   确定 Calendar 的两个指定实例是否不等。
        /// </summary>
        /// <param name="d1">Calendar</param>
        /// <param name="d2">Calendar</param>
        /// <returns></returns>
      //  public static bool operator !=(Calendar d1, Calendar d2) { return d1.JulianDay != d2.JulianDay; }
  
        /// <summary>
        ///  将指定的时间间隔加到指定的日期和时间以生成新的日期和时间。
        /// </summary>
        /// <param name="d">Calendar</param>
        /// <param name="t">TimeSpan</param>
        /// <returns>它是 d 和 t 值的和。</returns>
        public static Calendar operator +(Calendar d, TimeSpan t) { return new Calendar(d.JulianDay + (Decimal)t.Ticks / 1E-7M / TimeConsts.SECOND_PER_DAY); }
        /// <summary>
        ///  将指定的时间间隔加到指定的日期和时间以生成新的日期和时间。
        /// </summary>
        /// <param name="d">Calendar</param>
        /// <param name="t">CalendarSpan</param>
        /// <returns>它是 d 和 t 值的和。</returns>
        public static Calendar operator +(Calendar d, CalendarSpan t) { return new Calendar(d.JulianDay + t.TotalDays ); }
       
        /// <summary>
        /// 确定指定的 System.Calendar 是否小于另一个指定的 System.Calendar。
        /// </summary>
        /// <param name="t1">Calendar</param>
        /// <param name="t2">Calendar</param>
        /// <returns>如果 t1 小于 t2，则为 true；否则为 false。</returns>
        public static bool operator <(Calendar t1, Calendar t2) { return t1.JulianDay < t2.JulianDay; }
       
        /// <summary>
        /// 确定指定的 Calendar 是否小于或等于另一个指定的 Calendar。
        /// </summary>
        /// <param name="t1">Calendar</param>
        /// <param name="t2">Calendar</param>
        /// <returns>如果 t1 小于或等于 t2，则为 true；否则为 false。</returns>
        public static bool operator <=(Calendar t1, Calendar t2) { return t1.JulianDay <= t2.JulianDay; }
 
        /// <summary>
        ///  确定 Calendar 的两个指定的实例是否相等。
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns>  如果 d1 和 d2 表示同一日期和时间，则为 true；否则为 false。</returns>
       // public static bool operator ==(Calendar d1, Calendar d2) { return d1.Equals(d2);; }

        /// <summary>
        /// 确定指定的 System.Calendar 是否大于另一个指定的 Calendar。
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns>如果 t1 大于 t2，则为 true；否则为 false。</returns>
        public static bool operator >(Calendar t1, Calendar t2) { return t1.JulianDay > t2.JulianDay; }

        /// <summary>
        /// 确定指定的 Calendar 是否大于等于另一个指定的 Calendar。
        /// </summary>
        /// <param name="t1">Calendar</param>
        /// <param name="t2">Calendar</param>
        /// <returns>如果 t1 大于等于 t2，则为 true；否则为 false。</returns>
        public static bool operator >=(Calendar t1, Calendar t2) { return t1.JulianDay >= t2.JulianDay; }

        #endregion
    }
}
