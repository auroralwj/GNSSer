// 2015.01.07, czs, edit in namu, 修正当年闰年2月后少加一天的错误

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Geo.Times
{

    /// <summary>
    /// 时间转换算法。
    /// </summary>
    public class TimeConvert
    {
        #region 常用常数
        /// <summary>
        ///平年每月包含的天数。下标对应月，下标 0 只是占位。
        ///如 1 月对应 31
        /// </summary>
        private static int[] DaysInMonthOfCommonYear = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        #endregion

        #region 儒略日与日历的计算
        /// <summary>
        /// 由日历计算儒略日。参照GpsTk。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns></returns>
        public static long CalendarToJulianDay(int year, int month, int day)
        {
            if (year == 0)
            { --year; } //there is no year 0
            if (year < 0)
            { ++year; }

            long jd;
            double y = year, m = month, d = day;

            // In the conversion from the Julian Calendar to the Gregorian
            // Calendar the day after October 4, 1582 was October 15, 1582.
            //
            // if the date is before October 15, 1582
            if (year < 1582 || (year == 1582 && (month < 10 || (month == 10 && day < 15))))
            {
                jd = 1729777 + day + 367 * year
                   - Convert.ToInt64(Math.Floor(7 * (y + 5001 + Math.Floor((m - 9) / 7)) / 4))
                   + Convert.ToInt64(Math.Floor(275 * m / 9));
            }
            else   // after Oct 4, 1582
            {
                jd = 1721029 + day + 367 * year
                   - Convert.ToInt64(Math.Floor((7 * (y + Math.Floor((m + 9) / 12)) / 4)))
                   - Convert.ToInt64(Math.Floor((3 * (1 + Math.Floor((y + (m - 9) / 7) / 100)) / 4)))
                   + Convert.ToInt64(Math.Floor((275 * m / 9)));

                // catch century/non-400 non-leap years
                if (((year % 100) != 0 &&
                     (year % 400) == 0 &&
                     month > 2 &&
                     month < 9) ||
                    (((year - 1) % 100) != 0 &&
                     ((year - 1) % 400) == 0 &&
                     month == 1))
                {
                    --jd;
                }
            }
            return jd;
        }

        /// <summary>
        /// 儒略日转为日历。只有整数部分。
        /// </summary>
        /// <param name="jd"></param>
        /// <param name="iyear"></param>
        /// <param name="imonth"></param>
        /// <param name="iday"></param>
        public static void JulianDayToCalendar(long jd, out int iyear, out int imonth, out int iday)
        {
            long L, M, N, P, Q;
            if (jd > 2299160)    // after Oct 4, 1582
            {
                L = jd + 68569;
                M = (4 * L) / 146097;
                L = L - ((146097 * M + 3) / 4);
                N = (4000 * (L + 1)) / 1461001;
                L = L - ((1461 * N) / 4) + 31;
                P = (80 * L) / 2447;
                iday = Convert.ToInt32(L - (2447 * P) / 80);
                L = P / 11;
                imonth = Convert.ToInt32(P + 2 - 12 * L);
                iyear = Convert.ToInt32(100 * (M - 49) + N + L);
            }
            else
            {
                P = jd + 1402;
                Q = (P - 1) / 1461;
                L = P - 1461 * Q;
                M = (L - 1) / 365 - L / 1461;
                N = L - 365 * M + 30;
                P = (80 * N) / 2447;
                iday = Convert.ToInt32(N - (2447 * P) / 80);
                N = P / 11;
                imonth = Convert.ToInt32(P + 2 - 12 * N);
                iyear = Convert.ToInt32(4 * Q + M + N - 4716);
                if (iyear <= 0)
                {
                    --iyear;
                }
            }
            // catch century/non-400 non-leap years
            if (iyear > 1599 &&
               (iyear % 100) != 0 &&
               (iyear % 400) == 0 &&
               imonth == 2 &&
               iday == 29)
            {
                imonth = 3;
                iday = 1;
            }
        }

        /// <summary>
        ///计算日历差
        /// </summary>
        /// <param name="calenda"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static decimal GetPassedDays(Calendar calenda, Calendar origin = null)
        {
            if (origin == null) origin = new Calendar();

            decimal passedA = GetPassedDaysFromYearOne(calenda);
            decimal oriday = GetPassedDaysFromYearOne(origin);
            return passedA - oriday;
        }

        /// <summary>
        /// 以 1-1-1 0:0:0 为基准, 连续的格力历，没有跳变。
        /// </summary>
        /// <param name="calenda"></param>
        /// <returns></returns>
        public static decimal GetPassedDaysFromYearOne(Calendar calenda)
        {
            int calendaYear = calenda.Year;
            int calendaMonth = calenda.Month;
            int calendarDay = calenda.Day;

            //以 1-1-1 0:0:0 为基准
            Decimal passedDay = GetDayCountBetweenYear(calendaYear);

            int passedDays = GetDayOfYear(calendaYear, calendaMonth);

            passedDay += passedDays;
            passedDay += calendarDay - 1;

            passedDay += calenda.Hour / 24M;
            passedDay += calenda.Minute / 24M / 60M;
            passedDay += calenda.Seconds * TimeConsts.SECOND_TO_DAY;

            return passedDay;
        }



        /// <summary>
        /// 日历到平儒略日。
        /// </summary>
        /// <param name="calendar">日历</param>
        /// <returns></returns>
        public static decimal CalendarToMjd(Calendar calendar)
        {
            return JdToMjd(CalendarToJulianDay(calendar));
        }
        /// <summary>
        /// 平儒略日转换到日历。
        /// </summary>
        /// <param name="modifiedJulianDay">平儒略日</param>
        /// <returns></returns>
        public static Calendar MjdToCalendar(Decimal modifiedJulianDay)
        {
            Decimal JulianDay = modifiedJulianDay + TimeConsts.MJD_TO_JD;
            return JdToCalendar(JulianDay);
        }
        /// <summary>
        /// 儒略日转换到日历。
        /// </summary>
        /// <param name="JulianDay">儒略日</param>
        /// <returns></returns>
        public static Calendar JdToCalendar(Decimal JulianDay)
        {
            Int32 Day;
            Int32 Month;
            Int32 Year;
            Int32 Hour;
            Int32 Minute;
            Int32 Second;
            Decimal milliSecond;
            JdToCalendar(JulianDay, out Day, out Month, out Year, out Hour, out Minute, out Second, out milliSecond);
            return new Calendar(Year, Month, Day, Hour, Minute, Second, milliSecond);
        }
        /// <summary>
        /// 儒略日转换到日历。
        /// </summary>
        /// <param name="JulianDay">儒略日</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <param name="Day">日</param>
        /// <param name="Hour">时</param>
        /// <param name="Minute">分</param>
        /// <param name="Second">秒</param>
        /// <param name="milliSeconds">毫秒（秒的小数部分）</param>
        public static void JdToCalendar(Decimal JulianDay, out Int32 Day, out Int32 Month, out Int32 Year, out Int32 Hour, out Int32 Minute, out Int32 Second, out Decimal milliSeconds)
        {
            int a = Decimal.ToInt32(JulianDay + 0.5m);
            int b = a + 1537;
            int c = (int)((b - 122.1) / 365.25);
            int d = (int)(365.25 * c);
            int e = (int)((b - d) / 30.6001);

            Decimal fracDay = (JulianDay + 0.5m) % 1m;

            Day = (Int32)(b - d - (int)(30.6001 * e) + fracDay);
            Month = (Int32)(e - 1 - 12 * (int)(e / 14));
            Year = (Int32)(c - 4715 - (int)((7 + Month) / 10));

            Decimal hours = fracDay * 24.0m;
            Hour = (Int32)(hours);
            Decimal Minutes = (fracDay * 24.0m - Hour) * 60m;
            Minute = (Int32)(Minutes);
            Decimal Seconds = ((Minutes - Minute) * 60.0m);
            Second = (Int32)(Seconds);
            milliSeconds = (Seconds - Second) * 1000m;
        }
        /// <summary>
        /// 日历转换到儒略日。
        /// 儒略日从公元前4713年1月1日GMT正午0时开始。
        /// MJD 从1858年11月17日GMT0时开始。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="second">分</param>
        /// <param name="second">秒</param>
        /// <param name="milliSeconds">毫秒</param>
        /// <returns>儒略日</returns>
        public static Decimal CalendarToMjd(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, Decimal milliSeconds)
        {
            return TimeConsts.JD_TO_MJD + CalendarToJd(year, month, day, hour, minute, second, milliSeconds);
        }
        /// <summary>
        /// 日历转换到儒略日。
        /// </summary>
        /// <param name="calendar">日历</param>
        /// <returns></returns>
        public static Decimal CalendarToJulianDay(Calendar calendar)
        {
            return CalendarToJd(calendar.Year, calendar.Month, calendar.Day, calendar.Hour, calendar.Minute, calendar.Second, calendar.MilliSeconds);
        }
        /// <summary>
        /// 日历转换到儒略日。采用算法2 ，与 1 等价。
        /// 
        /// 从公元 1582 年 10月4日以前，都按照儒略历计算。
        /// </summary>
        /// <param name="calendar">日历</param>
        /// <returns></returns>
        public static Decimal CalendarToJulianDay_2(Calendar calendar)
        {
            return CalendarToJd_2(calendar.Year, calendar.Month, calendar.Day, calendar.Hour, calendar.Minute, calendar.Second, calendar.MilliSeconds);
        }
        /// <summary>
        /// 平儒略日到儒略日
        /// </summary>
        /// <param name="modifiedJulianDay">平儒略日</param>
        /// <returns>儒略日</returns>
        public static Decimal MjdToJd(Decimal modifiedJulianDay)
        {
            return modifiedJulianDay + 2400000.5M;
        }
        /// <summary>
        /// 儒略日到平儒略日
        /// </summary>
        /// <param name="julianDay">儒略日</param>
        /// <returns>平儒略日</returns>
        public static Decimal JdToMjd(Decimal julianDay)
        {
            return julianDay - 2400000.5M;
        }
        /// <summary>
        /// 日历转换到儒略日。
        /// 儒略日从公元前4713年1月1日GMT正午0时开始。
        /// MJD 从1858年11月17日GMT0时开始。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="second">分</param>
        /// <param name="second">秒</param>
        /// <param name="milliSeconds">毫秒</param>
        /// <returns>儒略日</returns>
        public static Decimal CalendarToJd(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, Decimal milliSeconds)
        {
            // if ((year <= 1582)) throw new ArgumentException("此公式只能转换1582年10月15日后（格里历）！之前是儒略历。您可以更新该算法。", "year");


            //下面算法参考自：李征航，黄劲松.GPS测量与数据处理(第二版)[M].武汉大学出版社,2011.P27.
            //求 儒略日
            //公式1 % Julianisches Datum, Modifiziertes Julianisches Datum
            //2015.2.6 经测算证明，李的方法都是错误的，补充在后面的方法才是正确的，验算MJD相应的起点1858年11月17日世界时0时。1979年10月1日零时儒略日数为2,444,147.5。

            Decimal hourFrac = hour + minute / 60.0m + second / 3600.0m + milliSeconds / 3600000.0m; //小时


            Decimal JulianDay = 1721013.5m + 367 * year - (int)((year + (int)((month + 9) / 12 + 0.5)) * 7 / 4 + 0.5)
                  + (int)(275 * month / 9 + 0.5)
                  + day
                  + hourFrac / 24.0m;



            Decimal yearDay = 0; Decimal monthDay = 0; Decimal A = 0;
            if (month <= 2)
            {
                yearDay = (int)(365.25 * (year - 1) + 0.5);
                monthDay = (int)(30.6001 * ((month + 12) + 1) + 0.5);

                yearDay = (Decimal)Math.Floor(365.25 * (year - 1));
                monthDay = (Decimal)Math.Floor(30.6001 * ((month + 12) + 1));

                A = 2 - ((Decimal)Math.Floor((year - 1) / 100.0m)) + (Decimal)Math.Floor((year - 1) / 400.0m);  //特殊的

            }
            else
            {
                yearDay = (int)(365.25 * year + 0.5);
                monthDay = (int)(30.6001 * (month + 1) + 0.5);

                yearDay = (Decimal)Math.Floor(365.25 * year);
                monthDay = (Decimal)Math.Floor(30.6001 * (month + 1));

                A = 2 - (Decimal)(Math.Floor((year) / 100.0m)) + (Decimal)Math.Floor((year) / 400.0m);

            }


            Decimal jd1 = yearDay + monthDay + day + hourFrac / 24.0m + A + 1720994.5m; //不同的

            JulianDay = jd1;

            return JulianDay;
        }
        /// <summary>
        /// 日历转换到儒略日。第二个公式，结果验证与前一个公式相同。2014.06.25,计算精度在亚纳秒以下级别。可满足要求。
        /// 儒略日从公元前4713年1月1日GMT正午0时开始。
        /// MJD 从1858年11月17日GMT0时开始。
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="second"></param>
        /// <param name="second"></param>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static Decimal CalendarToJd_2(int year, int month, int day, int hour, int minute, int second, Decimal milliSeconds)
        {
            if ((year <= 1582)) throw new ArgumentException("此公式只能转换1582年10月15日后（格里历）！之前是儒略历。您可以更新该算法。", "year");
            //公式2
            //下面算法参考自：李征航，黄劲松.GPS测量与数据处理(第二版)[M].武汉大学出版社,2011.P27.
            Decimal hourFrac = hour + minute / 60.0m + second / 3600.0m + milliSeconds / 3600000.0m; //小时
            int yearDay = 0; int monthDay = 0;
            if (month <= 2)
            {
                yearDay = (int)(365.25 * (year - 1));
                monthDay = (int)(30.6001 * ((month + 12) + 1));
            }
            else
            {
                yearDay = (int)(365.25 * year);
                monthDay = (int)(30.6001 * (month + 1));
            }
            //double jd = yearDay + monthDay + Day + 1720981.5;
            Decimal jd = yearDay + monthDay + day + hourFrac / 24.0m + 1720981.5m;
            return jd;
        }

        /// <summary>
        /// Jean Meeus 所介绍的方法的实际代码
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static Decimal CalendarToJulianDay_3(Calendar date)
        {
            int y, m, B;
            y = date.Year;
            m = date.Month;
            if (date.Month <= 2)//如果日期在1月或2月，则被看作是在前一年的13月或14月
            {
                y = date.Year - 1;
                m = date.Month + 12;
            }
            // Correct for the lost days in Oct 1582 when the Gregorian calendar 
            // replaced the Julian calendar. 
            B = -2;
            if (date.Year > 1582 || (date.Year == 1582 && (date.Month > 10
                || (date.Month == 10 && date.Day >= 15))))
            {
                B = y / 400 - y / 100;
            }
            Decimal julianDay = ((int)Math.Floor(365.25 * y) + (int)Math.Floor(30.6001 * (m + 1)) + B + 1720996.5M + date.Day);
            Decimal fracHour = date.Hour / 24.0M + date.Minute / 1440.0M + date.Seconds / 86400.0M;
            return julianDay + fracHour;
        }

        #endregion

        #region GNSS 时间转换 周周秒
        /// <summary>
        /// 日历时间转换为GNSS周周秒时间
        /// </summary>
        /// <param name="calendar">日历</param>
        /// <param name="gnssOriginMjdDay">GNSS时间起算点</param>
        /// <param name="Week">返回周</param>
        /// <param name="secondsOfWeek">返回秒</param>
        public static void CalendarToGnssTime(ICalendar calendar, Decimal gnssOriginMjdDay, out int Week, out Decimal secondsOfWeek)
        {
            Decimal passedMJd = calendar.MJulianDay - gnssOriginMjdDay;
            Week = (int)Math.Floor(passedMJd / 7);
            if (passedMJd < 0) Week = (int)Math.Ceiling(passedMJd / 7);

            Int32 DayOfWeek = (Int32)(passedMJd % 7);

            secondsOfWeek = DayOfWeek * 86400 + calendar.Hour * 3600 + calendar.Minute * 60 + calendar.Second + calendar.MilliSeconds * 0.001M;
        }

        #endregion

        #region 工具算法
        
        #region 借助系统实现
        /// <summary>
        /// 从 1 年 1 月 1 日开始到前一天所经历的天数。以 1-1-1 0:0:0 为基准
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dayInclude"></param>
        /// <returns></returns>
        public static int GetPassedDaysFromYearOne(int year, int month, int dayInclude)        {              return (int)TimeSpan.FromTicks(new DateTime(year, month, dayInclude).Ticks).TotalDays;         }
        
        /// <summary>
        /// 计算年 ，从 1 年 1 月 1 日开始。
        /// </summary>
        /// <param name="totalDay">从 1 年 1 月 1 日开始</param>
        /// <returns></returns>
        public static int GetYear(int totalDay) { return GetDate(totalDay).Year; }

        /// <summary>
        /// 计算年 ，从 1 年 1 月 1 日开始。
        /// </summary>
        /// <param name="totalDay"></param>
        /// <returns></returns>
        public static DateTime GetDate(int totalDay)  {   return new DateTime(TimeSpan.FromDays(totalDay).Ticks);    }

        /// <summary>
        /// 计算指定起始年（含）到截止年（不含）两年之间经历的天数，默认起始年为 1 年。
        /// 如截止年输入 1 则为 0 日，2 则为 365 日。是存储的天数统计。
        /// </summary>
        /// <param name="toYearExclude">不含（该年的前一年）</param>
        /// <param name="fromYearInclude">含</param>
        /// <returns></returns>
        public static int GetDayCountBetweenYear(int toYearExclude, int fromYearInclude = 1) { return TimeSpan.FromTicks(new DateTime(toYearExclude, 1, 1).Ticks).Days; }

        /// <summary>
        /// 由年积日获取当天所在的月份，从 1 开始，到 12 结束。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="dayOfYear">年积日，从0开始</param>
        /// <returns>月 [1,12] </returns>
        public static int GetMonth(int year, int dayOfYear) { return GetDate(year, dayOfYear).Month; }

        /// <summary>
        /// 获取月日。注意：月日的取值范围是 [1-28,29,30,31],不会出现 0 的情况。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="dayOfYear">年积日</param>
        /// <returns></returns>
        public static int GetDayOfMonth(int year, int dayOfYear) {   return GetDate(year, dayOfYear).Day;  }
        /// <summary>
        /// 根据年与年积日，返回日期。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="dayOfYear">年积日, 从 1 开始</param>
        /// <returns></returns>
        public static DateTime GetDate(int year, int dayOfYear)  {  return (new DateTime(year, 1, 1) + TimeSpan.FromDays(dayOfYear-1));   } //减去 1 啊！
        
        #endregion

        #region 完全自我实现
        /// <summary>
        /// get the GPS Day form the Year 1980? or so.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetGpsDay(int year, int month, int day)
        {
            CheckMonthDay(month, day);

            int dayOfYear = GetDayOfYear(year, month, day);
            var passed = GetDayCountBetweenYear(1981, year) + 360;//1980年还剩360天。 
            return passed + passed;
        }
        /// <summary>
        /// 计算年 ，从 1 年 1 月 1 日开始。
        /// </summary>
        /// <param name="totalDay">从 1 年 1 月 1 日开始</param>
        /// <returns></returns>
        public static int GetYearMySelf(int totalDay)
        {
            var temp = totalDay;

            int year = 1;
            while (totalDay >= 365)
            {
                totalDay -= GetDayCountOfYear(year); ;

                if (totalDay >= 0) //若小于0，则可能是闰年， 是否考虑等于 0 ？
                    year++;
            } 

            return year;
        }
        static int GetMonthMySelf(int year, int dayOfYear)
        {
            CheckYearRange(year);
            CheckDayOfYearRange(dayOfYear);
            //估算起始月，提升查找效率
            int startMonth = (dayOfYear / 31);
            if (startMonth < 1) startMonth = 1;

            int month = startMonth;
            for (int m = startMonth; m <= 12; m++)
            {
                if (GetDayOfYear(year, m) >= dayOfYear) { break; } //若刚好等于，则正是上月底，也属于上一月

                month = m;//update
            }
            return month;
        }

        /// <summary>
        /// 所经历的年积日。从当年第一月到前一天所经过的天数，不含当天。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月份</param>
        /// <param name="dayExclude">日（不含）</param>
        /// <returns></returns>
        public static int GetPassedDayOfYear(int year, int month, int dayExclude) { return GetDayOfYear(year, month, dayExclude) - 1; }

        /// <summary>
        /// 计算年积日，从当年 1 月 1 日 到 当天（含）。
        /// 年积日从 1 到 365 或 366.
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="dayInclude">日，若为 0 ，则表示上下月交接 0 点时分</param>
        /// <returns></returns>
        public static int GetDayOfYear(int year, int month, int dayInclude) { CheckDayRange(dayInclude); return GetDayOfYear(year, month) + dayInclude; }

        /// <summary>
        /// 所经历的年积日。从当年第一月到前一月底所经过的天数。等同于下月初的 GetPassedDayOfYear 
        /// </summary>
        /// <param name="year">年，输入年判断是否闰年</param>
        /// <param name="monthExclude">当前月（不含）</param>
        /// <returns>0或月日累计数量</returns>
        public static int GetDayOfYear(int year, int monthExclude)
        {
            CheckYearRange(year);
            CheckMonthRange(monthExclude);

            int days = 0;
            for (int m = 1; m < monthExclude; m++)
            {
                days += GetDayCountOfMonth(year, m);
            }
            return days;
        }

        /// <summary>
        /// 获取一个月总共的天数。
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月(1-12)</param>
        /// <returns></returns>
        public static int GetDayCountOfMonth(int year, int month)
        {
            CheckYearRange(year);
            CheckMonthRange(month);

            int day = DaysInMonthOfCommonYear[month];
            if (month == 2 && IsGregorianLeapYear(year)) { day++; } // 2015.01.07, czs, edit in namu, 修正当年闰年2月后少加一天的错误

            return day;
        }

        /// <summary>
        /// 一年的总天数，366 or 365
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetDayCountOfYear(int year) { int day = 365; if (IsGregorianLeapYear(year)) { day++; } return day; }

        /// <summary>
        /// 格里历，公历 common，阳历， 新历 闰年，年长度365.2425日。 计算是否闰年。4年一闰，百年不闰，4百年再闰。
        /// </summary>
        /// <param name="year">年，4位数（含）以上</param>
        /// <returns></returns>
        public static bool IsGregorianLeapYear(int year) { return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0); }

        /// <summary>
        /// 儒略历  闰年，年长度365.25日。 计算是否闰年。4年一闰。
        /// </summary>
        /// <param name="year">年，4位数（含）以上</param>
        /// <returns></returns>
        public static bool IsJulianLeapYear(int year) { return (year % 4 == 0); }

        #region 检查变量范围

        private static void CheckYearMonthDayRange(int year, int month, int day)
        {
            CheckYearRange(year);
            CheckMonthDay(month, day);
        }

        private static void CheckMonthDay(int month, int day)
        {
            CheckDayRange(day);
            CheckMonthRange(month);
        }

        static private void CheckMonthRange(int month) { CheckRange("month", month, 1, 12); }
        static private void CheckDayOfYearRange(int day) { CheckRange("dayOfYear", day, 0, 366); }
        static private void CheckDayRange(int day) { CheckRange("dayOfMonth", day, 1, 31); }
        static private void CheckMinuteRange(int minute) { CheckRange("second", minute, 1, 59); }
        static private void CheckHourRange(int hour) { CheckRange("hour", hour, 0, 23); }
        static private void CheckYearRange(int year) { CheckRange("year", year, 1, 9999); }

        static private void CheckRange(string name, double val, double minInclude, double maxInclude)
        {
            if (val < minInclude || val > maxInclude)
            {
                var msg = name + " 取值范围是 从 " + minInclude + "(含) 到 " + maxInclude + "(含)";
                Geo.IO.Log.GetLog(typeof(TimeConvert)).Error(msg);
                throw new ArgumentException(msg, name);
            }
        }

        #endregion
        #endregion
        #endregion
    }
}
