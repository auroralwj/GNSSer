//2014.06.27,czs, edit, 进行了一些梳理，解决了高精度问题
//2015.04.18, czs, edit in namu, 采用 MinuteTime 作为核心变量，主要解决效率问题
//2015.04.19, cy, 修改了SecondsOfDay和SecondsOfWeek变量，目前可用
//2015.04.24, czs, edit in namu, 将 MinuteTime 替换为 SecondTime，修正了一些错误，如TotalDays
//2015.05.06，czs, add in namu, 增加了GPS周，周秒构造器。
//2015.11.06, czs & cy, edit in hongqing, 增加了LinePropertyString 的输入输出支持。

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Geo.Utils;
using Geo.Times;

namespace Geo.Times
{
    /// <summary>
    ///  时间类，是一个功能强大,集 Calendar、GnssTime、JulianDay 于一体的,以历元为基准的，便于使用的 GNSS 时间类。
    ///  时间核心为 SecondTime 。
    /// </summary>
    public struct Time : IComparable<Time>, IEquatable<Time>, IComparable
    {
        #region 构造函数
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="utcNow">UTC 系统时间</param>
        /// <param name="secondsOfWeekOrDay">周内秒或日内秒</param>
        public Time(DateTime utcNow, double secondsOfWeekOrDay) :
            this(utcNow.Date + TimeSpan.FromSeconds(secondsOfWeekOrDay % (3600 * 24))) { }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="gpsWeekNumber">GPS 周数</param>
        /// <param name="secondsOfWeek">周内秒</param>
        /// <param name="isGpstOrBdt">GPS时间或北斗时间</param>
        public Time(int gpsWeekNumber, double secondsOfWeek, bool isGpstOrBdt = true) :
            this(isGpstOrBdt ?
                Time.StartOfGpsT.DateTime + TimeSpan.FromDays(7 * gpsWeekNumber) + TimeSpan.FromSeconds(secondsOfWeek)
                : Time.StartOfBdT.DateTime + TimeSpan.FromDays(7 * gpsWeekNumber) + TimeSpan.FromSeconds(secondsOfWeek))
        { }

        /// <summary>
        /// 以年，年积日初始化
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="dayOfYear">年积日</param>
        public Time(int year, int dayOfYear) :
            this(new DateTime(year > 3000 ? 3000 : year, 1, 1) + TimeSpan.FromDays(dayOfYear > 366 ? 365 : dayOfYear - 1)) { }

        /// <summary>
        /// C# DateTime初始化。精度为100纳秒。
        /// </summary>
        /// <param name="time"></param>
        public Time(DateTime time) : this(new SecondTime(time)) { }

        /// <summary>
        /// 指定日历初始化
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param> 
        public Time(int year = 1980, int month = 1, int day = 6, int hour = 0, int minute = 0, double second = 0)
            : this(new SecondTime(year, month, day, hour, minute, second)) { }

        /// <summary>
        /// 以日历初始化
        /// </summary>
        /// <param name="time">日历</param>
        public Time(SecondTime time)
            : this()
        {
            this.TickTime = time;
            //  var SecondTime = new SecondTime(TickTime.MinuteTicks * 60, TickTime.FloatSeconds);
        }
        /// <summary>
        /// 以平儒略日初始化
        /// </summary>
        /// <param name="JulianDay">儒略日</param> 
        /// <param name="isMjd">是否平儒略日</param> 
        public Time(double JulianDay, bool isMjd = false) : this(new Calendar(Convert.ToDecimal(JulianDay), isMjd).DateTime)
        {

        }
        #endregion

        #region 核心存储
        /// <summary>
        /// 采用公元0年开始计时的，以整秒和秒小数标识的时间类。
        /// </summary>
        public SecondTime TickTime;//皇帝轮流坐，今年到我家 
        /// <summary>
        /// 方便存储数据、标识等。
        /// </summary>
        public Object Tag { get; set; }
        #endregion

        #region 对象属性
        #region Calendar 属性
        /// <summary>
        /// 日历型时间类。核心属性。
        /// </summary>
        public ICalendar Calendar { get { return new Calendar(this.TickTime.DateTime); } }
        /// <summary>
        /// 默认构造函数 1980-1-6 0:0:0
        /// </summary>
        public static Time Default = new Time(1980, 1, 6, 0, 0, 0);

        /// <summary>
        /// 四位数表示的年。
        /// 如果赋值为两位数，则自动设置为四位数
        /// ，规则：
        /// 80-99: 1980-1999，
        /// 00-79: 2000-2079
        /// </summary>
        public int Year { get { return TickTime.Date.Year; } }
        /// <summary>
        /// 两位数的年。
        /// </summary>
        public int SubYear { get { return int.Parse(Year.ToString("0000").Substring(Year.ToString().Length - 2)); } }
        /// <summary>
        /// 月：1-12
        /// </summary>
        public int Month { get { return TickTime.Date.Month; } }
        /// <summary>
        /// 1-31
        /// </summary>
        public int Day { get { return TickTime.Date.Day; } }
        /// <summary>
        /// 小时：0-23
        /// </summary>
        public int Hour { get { return TickTime.Hour; } }
        /// <summary>
        /// 0-59
        /// </summary>
        public int Minute { get { return TickTime.Minute; } }
        /// <summary>
        /// 以整数表示的秒 0-59
        /// </summary>
        public int Second { get { return TickTime.Second; } }
        /// <summary>
        /// 秒， 0-59， 双精度表示。
        /// </summary>
        public double Seconds { get { return TickTime.Seconds; } }



        #endregion
        /// <summary>
        /// 日期部分，年月日。
        /// </summary>
        public Time Date { get { return new Time(this.TickTime.Date); } }

        /// <summary>
        /// 返回 C# DateTime, 精确到0.1微秒（100纳秒）。
        /// </summary>
        public DateTime DateTime { get { return TickTime.DateTime; } }

        /// <summary>
        /// 日秒。
        /// </summary>
        public double SecondsOfDay { get { return TickTime.SecondsOfDay; } }
        /// <summary>
        /// 年积日
        /// </summary>
        public int DayOfYear { get { return TickTime.DayOfYear; } }
        /// <summary>
        /// 一周中的第几天，周日为 0，以此类推。
        /// </summary>
        public DayOfWeek DayOfWeek { get { return TickTime.DayOfWeek; } }

        /// <summary>
        /// 年内周
        /// </summary>
        public int WeekOfYear { get { return TickTime.WeekOfYear; } }
        /// <summary>
        /// GPS 周。
        /// </summary>
        public int GpsWeek { get { return TickTime.GpsWeek; } }// GnssTime.GnssWeek;
        /// <summary>
        /// BDS 周。
        /// </summary>
        public int BdsWeek { get { return TickTime.BdsWeek; } }
        /// <summary>
        /// 周秒。
        /// </summary>
        public double SecondsOfWeek { get {
                var ws = TickTime.SecondsOfWeek;
                return ws;

                //double minuteTics = TickTime.MinuteTicks;
                //double secondOfWeek = 7 * 24 * 60;

                //double factionOfWeeks = minuteTics / secondOfWeek - Math.Floor(minuteTics / secondOfWeek);

                //var ww = factionOfWeeks * secondOfWeek + TickTime.Seconds;
                //return ww;

            } }// GnssTime.SecondsOfWeek.DoubleValue;
        /// <summary>
        /// 儒略日（Julian day，JD）：约定从公元前4713年1月1日格林尼治平子午(世界时12h)开始起算到某天格林尼治平子午所经过的日数。
        /// </summary>
        public Decimal JulianDays { get { return Calendar.JulianDay; } }
        /// <summary>
        /// 简化的儒略日：由于儒略日数字位数太多，国际天文学联合会于1973年采用简化儒略日（MJD），其定义为 MJD = JD - 2400000.5。
        /// MJD相应的起点是1858年11月17日世界时0时。 例如1979年10月1日零时儒略日数为2,444,147.5。 
        /// </summary>
        public Decimal MJulianDays { get { return Calendar.MJulianDay; } }
        /// <summary>
        /// 双精度表示的 简化的儒略日
        /// </summary>
        public double MJulianDays_Double { get { return (double)Calendar.MJulianDay; } }

        // integer 'Julian day', = JD+0.5 (0 <= jday <= 3442448),下面三个参数即可确定一个时刻，也是GPSTk采用的方式
        public long JulianDay { get { return (long)(Calendar.JulianDay + (decimal)0.5); } }
        // integer milliseconds of the day (0 <= mSod <= 86400000)
        public long MillisecondOfDay { get { return (long)(TickTime.DateTime.TimeOfDay.TotalMilliseconds); } }
        /// <summary>
        /// double fractional milliseconds (mSec < 1.0)
        /// </summary>
        public double FractionalMilliseconds { get { return (TickTime.MilliSeconds) % 1; } }
        /// <summary>
        /// 选择最大的时间，靠后的。
        /// </summary>
        /// <param name="a">时间1</param>
        /// <param name="b">时间2</param>
        /// <returns></returns>
        public static Time Max(Time a, Time b) { if (a > b) return a; return b; }

        /// <summary>
        /// 选择最小的时间，靠前的。
        /// </summary>
        /// <param name="a">时间1</param>
        /// <param name="b">时间2</param>
        /// <returns></returns>
        public static Time Min(Time a, Time b) { if (a < b) return a; return b; }

        #endregion

        /// <summary>
        /// IGS 产品常用表达方式。周后面加一个周内日。
        /// </summary>
        /// <returns></returns>
        public int GetGpsWeekAndDay()
        {
            var day = this.GpsWeek * 10 + (int)this.DayOfWeek;
            return day;
        }
        /// <summary>
        /// IGS 小时产品常用表达方式。周后面加一个周内日和当前小时。
        /// </summary>
        /// <returns></returns>
        public int GetGpsWeekDayAndHour()
        {
            return GetGpsWeekAndDay() * 100 + Hour;
        }
        /// <summary>
        /// 一个副本
        /// </summary>
        /// <returns></returns>
        public Time Clone() { return new Time(this.TickTime); }
        #region 静态属性
        /// <summary>
        /// 系统当前时间转换为GpsTime
        /// </summary>
        public static Time Now { get { return new Time(DateTime.Now); } }
        /// <summary>
        /// 系统当前UTC时间转换为GpsTime
        /// </summary>
        public static Time UtcNow { get { return new Time(DateTime.UtcNow); } }
        /// <summary>
        /// 0，用于失败操作
        /// </summary>
        public static Time Zero = new Time(1, 1, 1);
        /// <summary>
        /// UTC 1980, 1, 6
        /// </summary>
        public static Time MinValue = new Time(1980, 1, 6);
        /// <summary>
        ///  UTC 1980, 1, 6
        /// </summary>
        public static Time StartOfGpsT = new Time(1980, 1, 6);
        /// <summary>
        /// 北斗起始时间，UTC 2006.01.01有待确定。
        /// </summary>
        public static Time StartOfBdT = new Time(2006, 1, 1);//??

        /// <summary>
        ///  1858, 11, 17。MJD相应的起点是1858年11月17日世界时0时,简化儒略日（MJD），其定义为 MJD = JD - 2400000.5。
        ///  这里采用历元描述，可能有问题。
        /// </summary>
        public static Time StartOfMjd = new Time(1858, 11, 17);

        /// <summary>
        /// 2079,12,31
        /// </summary>
        public static Time MaxValue = new Time(2079, 12, 31);
        #endregion

        #region override
        /// <summary>
        /// 判断数值是否相等
        /// </summary>
        /// <param name="other">Time</param>
        /// <returns></returns>
        public bool Equals(Time other)
        {
            return this.TickTime.Equals(other.TickTime);
        }
        /// <summary>
        /// 判断数值是否相等
        /// </summary>
        /// <param name="obj">待判断</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return ((obj is Time) && Equals((Time)obj));
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.TickTime.GetHashCode();
        }

        /// <summary>
        /// 指示此实例和 value 参数的相对值。值说明小于零此实例早于 value。零此实例与 value 相同。大于零此实例晚于 value。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public long CompareTo(Time other)
        {
            return this.TickTime.CompareTo(other.TickTime);
        }
        /// <summary>
        /// 以下划线分割的字符串
        /// </summary>
        /// <returns></returns>
        public string ToLinePropertyString()
        {
            string spliter = "_";
            return Year + spliter +
                  Month.ToString("00") + spliter +
                  Day.ToString("00") + spliter +
                  Hour.ToString("00") + spliter +
                  TickTime.Minute.ToString("00") + spliter +
                  TickTime.Seconds.ToString("00.000");
        }
        /// <summary>
        /// 向下取整到十分钟
        /// </summary>
        /// <returns></returns>
        public Time TrimToTenMinute()
        {
            var minuteFloat = Minute + Seconds / 60.0;
            var minute = (int)Math.Floor((minuteFloat / 10.0)) * 10;
            if (minute >= 60)
            {
                return new Time(Year, Month, Day, Hour + 1, minute - 60, 0);
            }
            return new Time(Year, Month, Day, Hour, minute, 0);
        }
        /// <summary>
        /// 四舍五入到十分钟
        /// </summary>
        /// <returns></returns>
        public Time RoundToTenMinute()
        {
            var minuteFloat = Minute + Seconds / 60.0;
            var minute = (int)Math.Round((minuteFloat / 10.0)) * 10;
            if (minute >= 60)
            {
                return new Time(Year, Month, Day, Hour + 1, minute - 60, 0);
            }
            return new Time(Year, Month, Day, Hour, minute, 0);
        }
        /// <summary>
        /// 利于作为文件名称输出的字符串。
        /// </summary>
        /// <returns></returns>
        public string ToPathString(bool isWithDate = true, bool isWithTime = true, bool isWithSecond = true, string secondFormat = "00.000")
        {
            var dateSpliter = "-";
            var str = "";
            if (isWithDate)
            {
                str += Year + dateSpliter +
                  Month.ToString("00") + dateSpliter +
                  Day.ToString("00") + "_";
            }
            var timeSpliter = ".";
            if (isWithTime)
            {
                str += Hour.ToString("00") + timeSpliter +
                      TickTime.Minute.ToString("00");
                if (isWithSecond)
                {
                    str += timeSpliter +  TickTime.Seconds.ToString(secondFormat);
                }
            }
            return str;
        }

        /// <summary>
        /// 解析形如 2015_11_06_09_53_11_01.025 字符串的时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Time ParseLinePropertyString(string str)
        {
            var spliter = '_';
            return Parse(str, spliter);
        }
        /// <summary>
        /// 尝试解析
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(Object obj, out Time result)
        {
            try
            {
                result = Parse(obj);
                return true;
            } catch (Exception ex)
            {
                result = Time.Default;
                return false;
            }

        }


        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Time Parse(Object obj)
        {
            if (obj is Time)
            {
                return (Time)obj;
            }
            if (obj is DateTime)
            {
                return new Time((DateTime)obj);
            }
            return Parse(obj.ToString(), new char[] { ' ', '-', '_' });
        }
        /// <summary>
        /// 解析指定分隔符的，形如 2015_11_06_09_53_11_01.025 字符串的时间，‘_’
        /// </summary>
        /// <param name="str"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static Time Parse(string str, char spliter)
        {
            var timeItems = str.Split(new char[] { spliter }, StringSplitOptions.RemoveEmptyEntries);

            return Parse(timeItems);
        }


        /// <summary>
        /// 解析指定分隔符的，形如 2015_11_06_09_53_11_01.025 字符串的时间，‘_’
        /// </summary>
        /// <param name="str"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static Time Parse(string str, char[] spliters)
        {
            var timeItems = str.Split(spliters, StringSplitOptions.RemoveEmptyEntries);

            return Parse(timeItems);
        }
        /// <summary>
        /// 解析字符串数组为时间。
        /// </summary>
        /// <param name="timeItems"></param>
        /// <returns></returns>
        public static Time Parse(string[] timeItems)
        {
            int i = 0;
            int year = int.Parse(timeItems[i++]);
            int month = int.Parse(timeItems[i++]);
            int day = int.Parse(timeItems[i++]);
            int hour = 0;
            int minute = 0;
            double second = 0;
            if (timeItems.Length > i)
            {
                hour = int.Parse(timeItems[i++]);
            }
            if (timeItems.Length > i)
            {
                minute = int.Parse(timeItems[i++]);
            }
            if (timeItems.Length > i)
            {
                second = Double.Parse(timeItems[i++]);
            }
            /// 80-99: 1980-1999，
            /// 00-79: 2000-2079
            if (year < 79 && year >= 0) year += 2000;
            if (year > 70 && year < 99) year += 1900;

            return new Time(year, month, day, hour, minute, second);
        }
        /// <summary>
        /// GPS周日
        /// </summary>
        /// <returns></returns>
        public string ToWeekDayString()
        {
            return GpsWeek + "" + DayOfWeek;
        }
        /// <summary>
        /// 年和年积日
        /// </summary>
        /// <returns></returns>
        public string ToYearDayString()
        {
            return Year + "" + DayOfYear.ToString("000");
        }
        /// <summary>
        /// 年和年积日
        /// </summary>
        /// <returns></returns>
        public string ToYearDayHourMinuteSecondString()
        {
            return Year + " " + DayOfYear.ToString("000") + " " + Hour + " " + Minute + " " + Second;
        }
        public static Time ParseYearDayString(string str)
        {
            int year = Int32.Parse(str.Substring(0, 4));
            int dayOfYear = Int32.Parse(str.Substring(4, 3));
            return new Time(year, dayOfYear);
        }

        /// <summary>
        /// 2002-05-23 12:00:00
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(10);
            sb.Append(Year)
                .Append("-")
                .Append(Month.ToString("00"))
                .Append("-")
                .Append(Day.ToString("00"))
                .Append(" ")
                .Append(Hour.ToString("00"))
                .Append(":")
                .Append(Minute.ToString("00"))
                .Append(":")
                .Append(Second.ToString("00"));
            return sb.ToString();

            var str = Year + "-" +
                Month.ToString("00") + "-" +
                Day.ToString("00") + " " +
                Hour.ToString("00") + ":" +
                TickTime.Minute.ToString("00") + ":" +
                TickTime.Second.ToString("00");

            return str;
        }

        /// <summary>
        /// 2002-05-23 12:00:00
        /// </summary>
        /// <returns></returns>
        public  string ToString(bool isWithDate , bool isWithTime, bool isWithSeconds =true, string secondFormat="00.00")
        {
            StringBuilder sb = new StringBuilder();
            if (isWithDate)
            {
                sb.Append(Year)
                    .Append("-")
                    .Append(Month.ToString("00"))
                    .Append("-")
                    .Append(Day.ToString("00"))
                    .Append(" ");
            }
            if (isWithTime)
            {
                sb.Append(Hour.ToString("00"))
                .Append(":")
                .Append(Minute.ToString("00"));
                if (isWithSeconds)
                {
                    sb.Append(":")
                      .Append(Second.ToString(secondFormat));
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 2002-05-23 12:00:00.000
        /// </summary>
        /// <returns></returns>
        public string ToDetailDateTimeString()
        {
            return Year + "-" +
                 Month.ToString("00") + "-" +
                 Day.ToString("00") + " " +
                 Hour.ToString("00") + ":" +
                 TickTime.Minute.ToString("00") + ":" +
                 TickTime.Seconds.ToString("00.000");
        }

        /// <summary>
        /// 12:20:30.000
        /// </summary>
        /// <returns></returns>
        public string ToTimeString()
        {
            return Hour.ToString("00") + ":" +
                Minute.ToString("00") + ":" +
                Seconds.ToString("00.000");
        }
        /// <summary>
        /// 短时间。 15:20:30
        /// </summary>
        /// <returns></returns>
        public string ToShortTimeString()
        {
            return Hour.ToString("00") + ":" +
                Minute.ToString("00") + ":" +
                Seconds.ToString("00");
        }

        public SecondTime From1970
        {
            get
            {
                var differ = (long)(this - Time1970);
                return new SecondTime(differ, this.TickTime.Fraction);
            }
        }
        /// <summary>
        /// 是否是1年1月1日
        /// </summary>
        public bool IsZero => this.TickTime.Fraction == 0 && this.TickTime.SecondTicks == 0;

        /// <summary>
        /// IGS 开始采用IGS08的时间, 2011-04-17 thru 2012-10-06 ,   GPS wks 1632 thru 1708 
        /// Gb08,2012-10-07 thru present,GPS wks 1709 thru present
        /// </summary>
        public static Time StartOfIgs08 = new Time(2011, 4, 17, 0, 0, 0);
        /// <summary>
        /// IGS 开始采用IGS14的时间
        /// </summary>
        public static Time StartOfIgs14 = new Time(2017, 1, 29, 0, 0, 0);

        public static Time Time1970 = new Time(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// 2002-05-23  
        /// </summary>
        /// <returns></returns>
        public string ToDateString()
        {
            StringBuilder sb = new StringBuilder(10);
            sb.Append(Year)
                .Append("-")
                .Append(Month.ToString("00"))
                .Append("-")
                .Append(Day.ToString("00"));
            return sb.ToString();
            //return Year + "-" +
            //    Month.ToString("00") + "-" +
            //    Day.ToString("00");
        }
        /// <summary>
        /// 2002-05-22_12
        /// </summary>
        /// <returns></returns>
        public string ToDateAndHourPathString()
        {
            StringBuilder sb = new StringBuilder(10);
            sb.Append(Year)
                .Append("-")
                .Append(Month.ToString("00"))
                .Append("-")
                .Append(Day.ToString("00"))
                .Append("_")
                .Append(Hour.ToString("00"));
            return sb.ToString();
            return ToDateString() + "_" + Hour.ToString("00");
        }
        public string ToDateAndHourMinitePathString()
        {
            return ToDateString() + "_" + Hour.ToString("00") + "_" + Minute.ToString("00");
        }
        int IComparable<Time>.CompareTo(Time other)
        {
            return (int)this.CompareTo(other);
        }

        #endregion

        #region 操作数
        /// <summary>
        /// 返回秒小数
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static double operator -(Time left, Time right) { return (left.TickTime - right.TickTime).TotalSeconds; }
        /// <summary>
        /// +
        /// </summary>
        /// <param name="left"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static Time operator +(Time left, double seconds) { return new Time(left.TickTime + seconds); }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="left"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static Time operator -(Time left, double seconds) { return new Time(left.TickTime - seconds); }
        /// <summary>
        /// +
        /// </summary>
        /// <param name="d"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Time operator +(Time d, TimeSpan t) { return new Time(d.DateTime + t); }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="d"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Time operator -(Time d, TimeSpan t) { return new Time(d.DateTime - t); }
        /// <summary>
        /// <
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <(Time t1, Time t2) { return t1.TickTime < t2.TickTime; }
        /// <summary>
        /// <=
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator <=(Time t1, Time t2) { return t1.TickTime <= t2.TickTime; }
        /// <summary>
        /// >
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >(Time t1, Time t2) { return t1.TickTime > t2.TickTime; }
        /// <summary>
        /// >=
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator >=(Time t1, Time t2) { return t1.TickTime >= t2.TickTime; }
        /// <summary>
        /// ==
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator ==(Time t1, Time t2) { return t1.TickTime == t2.TickTime; }
        /// <summary>
        /// !=
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool operator !=(Time t1, Time t2) { return t1.TickTime != t2.TickTime; }

        #endregion

        #region UTC 时间转换
        /// <summary>
        /// UTC Time  转为 gmst Time (Greenwich mean sidereal time)
        /// gmst 以弧度rad表示
        /// </summary>
        /// <param name="gpsTime">UTC Time</param>
        /// <param name="ut1_utc">UT1_UTC, s</param> 
        /// <returns></returns>
        public static double UtcToGmst(Time t, double ut1_utc)
        {
            Time gpstime0 = new Time(2000, 1, 1, 12);

            double ut, t1, t2, t3, gmst0, gmst;

            Time tut = t + ut1_utc;

            ut = tut.Hour * 3600.0 + tut.Minute * 60.0 + tut.Seconds;

            Time tut0 = tut.Date;

            t1 = (double)(tut0 - gpstime0) / 86400.0 / 36525.0;

            t2 = t1 * t1; t3 = t2 * t1;

            gmst0 = 24110.54841 + 8640184.812866 * t1 + 0.093104 * t2 - 6.2E-6 * t3;
            gmst = gmst0 + 1.002737909350795 * ut;

            return (gmst % 86400.0) * Math.PI * 2.0 / (2 * 43200.0);  // fmod(gmst,86400.0)*PI/43200.0; /* 0 <= gmst <= 2*PI */

        }
        /// <summary>
        /// UTC Time  转为 GPS Time
        /// 由于两者之间只是差了若干秒，还是采用GPS类表示UTC类
        /// </summary> 
        /// <returns></returns>
        public Time UtcToGpsT()
        {
            var result = this;
            for (int i = 0; i < leaps.Length; i++)
            {
                double[] leap = leaps[i];
                Time leapTime = new Time((int)leap[0], (int)leap[1], (int)leap[2], (int)leap[3], (int)leap[4], leap[5]);

                if ((result >= leapTime))
                {
                    Time UtcTime2gpsTime = result - leap[6];

                    result = UtcTime2gpsTime;
                    break;
                }

            }
            return result;
        }

        /// <summary>
        /// GPS Time 转为UTC TIME
        /// 由于两者之间只是差了若干秒，还是采用GPS类表示UTC类
        /// </summary>
        /// <param name="gpsTime"></param>
        /// <returns></returns>
        public Time GpstToUtc()
        {
            Time result = this;
            for (int i = 0; i < leaps.Length; i++)
            {
                double[] leap = leaps[i];
                Time leapTime = new Time((int)leap[0], (int)leap[1], (int)leap[2], (int)leap[3], (int)leap[4], leap[5]);

                Time gpsTime2UtcTime = result + leap[6];

                if ((gpsTime2UtcTime >= leapTime))
                {
                    result = gpsTime2UtcTime;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// UTC 跳秒 GPTTime
        /// leap fraction{y,m,d,h,m,s,utc-gpst}
        /// 2015年6月份闰秒，？？？//czs, 2015.05.11,这个应该采用一个数据文件记录。
        /// </summary>
        private static double[][] leaps = {
                                             new double[]{2017,1,1,0,0,0,-18},
                                             new double[]{2015,7,1,0,0,0,-17},
                                             new double[]{2012,7,1,0,0,0,-16},
                                             new double[]{2009,1,1,0,0,0,-15},
                                             new double[]{2006,1,1,0,0,0,-14},
                                             new double[]{1999,1,1,0,0,0,-13},
                                             new double[]{1997,7,1,0,0,0,-12},
                                             new double[]{1996,1,1,0,0,0,-11},
                                             new double[]{1994,7,1,0,0,0,-10},
                                             new double[]{1993,7,1,0,0,0,-9},
                                             new double[]{1992,7,1,0,0,0,-8},
                                             new double[]{1991,1,1,0,0,0,-7},
                                             new double[]{1990,1,1,0,0,0,-6},
                                             new double[]{1988,1,1,0,0,0,-5},
                                             new double[]{1985,7,1,0,0,0,-4},
                                             new double[]{1983,7,1,0,0,0,-3},
                                             new double[]{1982,7,1,0,0,0,-2},
                                             new double[]{1981,7,1,0,0,0,-1}
                                         };

        #endregion


        #region 静态工具算法
        /// <summary>
        /// 将UTC时间转换为弧度
        /// </summary>
        /// <param name="dateTimeUtc"></param>
        /// <returns></returns>
        public static double DateTimeToRad(DateTime dateTimeUtc)
        {
            return TimeSpanToRad(dateTimeUtc.TimeOfDay);
        }

        /// <summary>
        /// 返回1天内的弧度
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static double TimeSpanToRad(TimeSpan span)
        {
            return (span.TotalDays % 1) * 2.0 * Math.PI;
        }
        #region 计算时间偏差
        /// <summary>
        /// 是否在日边界
        /// </summary>
        /// <param name="thresholdSeconds"></param>
        /// <returns></returns>
        public bool IsNearToDayEdge(double thresholdSeconds = 1)
        {
            var differ = Math.Abs(GetMiddleDayDiffer(this).TotalSeconds);
            if (43200 - differ < thresholdSeconds)
                return true;
            return false;
        }
        /// <summary>
        /// 是否在周边界
        /// </summary>
        /// <param name="thresholdSeconds"></param>
        /// <returns></returns>
        public bool IsNearToWeekEdge(double thresholdSeconds = 1)
        {
            var differ = Math.Abs(GetMiddleWeekDiffer(this).TotalSeconds);
            if (302400 - differ < thresholdSeconds)
                return true;
            return false;
        }

        /// <summary>
        /// 距离最近的天
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetNearstWeekAndDay(Time time)
        {
            var differ = GetMiddleDayDiffer(time);
            if (differ.Ticks > 0)
            {
                return GetNextWeekAndDay(time.GetGpsWeekAndDay());
            }
            return GetPrevWeekAndDay(time.GetGpsWeekAndDay());
        }
        /// <summary>
        /// 与当天正午时间差。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeSpan GetMiddleDayDiffer(Time time)
        {
            var dateTime = time.DateTime;
            var middle = dateTime.Date + TimeSpan.FromHours(12);
            var differ = dateTime - middle;
            return differ;
        }
        /// <summary>
        /// 距离最近的天
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetNearstWeek(Time time)
        {
            var differ = GetMiddleWeekDiffer(time);
            if (differ.Ticks > 0)
            {
                return time.GpsWeek + 1;
            }
            return time.GpsWeek - 1;
        }
        /// <summary>
        /// 当前时刻与周中间时刻时间差。即与周三正午差。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeSpan GetMiddleWeekDiffer(Time time)
        {
            var dateTime = time.DateTime;
            var middle = dateTime.Date + TimeSpan.FromDays(3.5);
            var differ = dateTime - middle;
            return differ;
        }

        /// <summary>
        /// 下一天 周+日
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public static int GetPrevWeekAndDay(int weekAndDay)
        {
            int day = weekAndDay % 10;
            if (day > 0) { return weekAndDay - 1; }
            else // ==0
            {
                int week = weekAndDay / 10;
                return (week - 1) * 10 + 6;
            }
        }
        /// <summary>
        /// 上一天 周+日
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public static int GetNextWeekAndDay(int weekAndDay)
        {
            int day = weekAndDay % 10;
            if (day < 6) { return weekAndDay + 1; }
            else // ==6
            {
                int week = weekAndDay / 10;
                return (week + 1) * 10 + 0;
            }
        }
        #endregion

        #region IO
        /// <summary>
        /// YY:DDD:SSSSS,Bernese时间表示。
        /// </summary>
        /// <returns></returns>
        public string ToYdsString() { return TimeIoUtil.ToYdsString(this); }

        /// <summary>
        /// 保存字符串，附上标记。与 ParseWithTagString(string) 方法互为配对。
        /// </summary>
        /// <returns></returns>
        public string ToStringWithTag()
        {
            var str = ToString();
            if (Tag != null) { str += "," + Tag; }
            return str;
        }
        /// <summary>
        /// 保存字符串，附上标记。与 ParseWithTagString(string) 方法互为配对。
        /// </summary>
        /// <returns></returns>
        public string ToShortTimeOfDayStringWithTag()
        {
            var str = ToShortTimeString();
            if (Tag != null) { str += "," + Tag; }
            return str;
        }
        /// <summary>
        /// 解析字符串。同时解析附加的对象为字符串。 与 ToStringWithTag() 方法互为配对。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Time ParseWithTagString(string str)
        {
            if (str.Contains(","))
            {
                var strs = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var time = Parse(strs[0]);
                time.Tag = strs[1];
                return time;
            }
            return Parse(str);
        }
        public static Time ParseTimeOfDayWithTagString(string str)
        {
            if (str.Contains(","))
            {
                var strs = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var time = Time.Now.Date + TimeSpan.Parse(strs[0]);

                time.Tag = strs[1];
                return time;
            }
            return Parse(str);
        }
        /// <summary>
        /// 解析 YYYY DOY HOUR MINUTE SECOND
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Time ParseYearDayOfYear(string line)
        {
            var strs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            int year = int.Parse(strs[i++]);
            int dayOfYear = int.Parse(strs[i++]);
            int hour = 0;
            int minute = 0;
            int second = 0;
            if (strs.Length > i) {
                hour = int.Parse(strs[i++]);
            }

            if (strs.Length > i) {
                minute = int.Parse(strs[i++]);
            }

            if (strs.Length > i) {
                second = int.Parse(strs[i++]);
            }

            var time = new Time(year, dayOfYear) + new TimeSpan(hour, minute, second);
            return time;
        }

        /// <summary>
        ///  YY:DDD:SSSSS.
        /// </summary>
        /// <param name="degStr"></param>
        /// <returns></returns>
        public static Time ParseYds(string str) { return TimeIoUtil.ParseYds(str); }

        /// <summary>
        /// 解析以空格('-',':')分开的“年 月 日 时 分 秒”字符串，如 06 10 30  0  0  0.0000000 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Time Parse(string line) { return TimeIoUtil.Parse(line); }
        /// <summary>
        /// 以系统时间初始化。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static Time Parse(DateTime time) { return new Time(time); }


        /// <summary>
        /// 找到最接近的时间。如果失败，返回 Zero
        /// </summary>
        /// <param name="times">待查找的时间序列</param>
        /// <param name="time">时间</param>
        /// <param name="maxDelta">允许最大的偏差，如果超出，查找失败</param>
        /// <param name="minDelta">最小的偏差，如果找到的在此范围内，则直接返回避免重复查找</param>
        /// <returns></returns>

        public static Time GetNearst(IEnumerable<Time> times, Time time, double maxDelta, double minDelta = 0.0001)
        {
            double delta = double.MaxValue;
            Time current = Time.MinValue;
            foreach (var item in times)
            {
                var differ = Math.Abs(item - time);
                //找到，直接返回
                if (differ < minDelta) { return item; }
                //较小的，保存
                if (differ < delta)
                {
                    current = item;
                    delta = differ;
                }
            }

            if (delta < maxDelta) { return current; }
            else
            {
                return Time.Zero;
            }
        }

        #endregion
        /// <summary>
        /// 计算与参考历元之差，两个周内秒之差。考虑了周循环。
        /// </summary>
        /// <param name="secOfWeek">周内秒</param>
        /// <param name="referenceEpoch">参考历元（周内秒）</param>
        /// <returns></returns>
        public static double GetDifferSecondOfWeek(double secOfWeek, double referenceEpoch)
        {
            if ((secOfWeek - referenceEpoch) > 302400.0) secOfWeek = secOfWeek - 604800.0;            //*** handle week rollover
            if ((secOfWeek - referenceEpoch) < -302400.0) secOfWeek = secOfWeek + 604800.0;            //*** valid icd200
            return secOfWeek - referenceEpoch;
        }

        public int CompareTo(object obj)
        {
            if (obj is Time)
            {
                return DateTime.CompareTo(((Time)obj).DateTime);
            }
            return 0;
        }
        #endregion


    }
}
