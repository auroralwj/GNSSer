//2015.04.17, czs, create in namu,高精度轻量级时间表示法， 精度达1e-15秒，周-周秒表示法，或称GNSS/历元表示法。功能，精度 有待测试！！！！！

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Times;

namespace Geo.Times
{ 

    /// <summary>
    /// 高精度轻量级时间表示法，采用周，周秒和秒小数维持， 精度达1e-15秒，周-周秒表示法，或称GNSS/历元表示法。
    /// </summary>
    public struct WeekTime : IEquatable<WeekTime>, IComparable<WeekTime>, Algorithm.OneDimOperation<WeekTime>
    {
        #region 常量
        /// <summary>
        /// 一周秒数量
        /// </summary>
        public const int SECOND_PER_WEEK = 604800;
        /// <summary>
        /// 一天的秒数量
        /// </summary>
        public const int SECOND_PER_DAY = 86400;
        /// <summary>
        /// 一个小时的秒数量
        /// </summary>
        public const int SECOND_PER_HOUR = 3600;

        /// <summary>
        /// 精度范围，认为精度在 1e-13 为相同，可达 1e-5 米级别的精度。
        /// </summary>
        public const double TOLERANCE = 1e-13;

        #endregion

        #region 构造与检核
        /// <summary>
        /// 以秒赋值。 
        /// </summary>
        public WeekTime(double seconds)
        {
            this.Week = (int)seconds / SECOND_PER_WEEK;
            this.SecondOfWeek = (int)seconds - Week * SECOND_PER_WEEK;
            this.FractionOfSeconds = seconds % 1;

            Legitimize();
        }

        /// <summary>
        /// 以秒赋值。 整数秒赋值。
        /// </summary>
        public WeekTime(int second)
        {
            this.Week =  second / SECOND_PER_WEEK;
            this.SecondOfWeek = second - Week * SECOND_PER_WEEK;
            this.FractionOfSeconds = 0;// second % 1;

            Legitimize();
        }

        /// <summary>
        /// 构造函数。赋值后，做判断将其调整到合适位置。
        /// </summary>
        /// <param name="week">GNSS 周</param>
        /// <param name="secondOfWeek">周秒整数</param>
        /// <param name="fractionOfSeconds">秒小数</param>
        public WeekTime(int week, int secondOfWeek, double fractionOfSeconds)
        {
            //先赋值，再判断
            this.FractionOfSeconds = fractionOfSeconds;
            this.SecondOfWeek = secondOfWeek;
            this.Week = week;

            Legitimize();
        }

        /// <summary>
        /// 使合法。检查各个参数的范围，并设置在合理范围内。
        /// </summary>
        public void Legitimize()
        {
            if (this.FractionOfSeconds < 0)
            {
                int borrowSecond = (int)Math.Ceiling(0 - this.FractionOfSeconds);
                //向前借
                this.SecondOfWeek -= borrowSecond;
                this.FractionOfSeconds +=  borrowSecond; 
            }
            else if (FractionOfSeconds >= 1)
            {
                this.SecondOfWeek += (int)this.FractionOfSeconds;
                this.FractionOfSeconds %= 1; 
            }

            //一周一周的借啊！
            if (this.SecondOfWeek < 0)
            {
                int borrowWeek = (int)Math.Ceiling((0 - this.SecondOfWeek * 1.0) / SECOND_PER_WEEK);
                this.SecondOfWeek += borrowWeek * SECOND_PER_WEEK;

                this.Week -= borrowWeek;
            }
            else if (SecondOfWeek >= SECOND_PER_WEEK)
            {
                this.SecondOfWeek = this.SecondOfWeek % SECOND_PER_WEEK;
                this.Week += (int)(SecondOfWeek / SECOND_PER_WEEK);
            }
        }
        #endregion
        
        #region 核心变量
        /// <summary>
        /// 周，如 GPS 周.周的符号代表了时间的方向。
        /// </summary>
        public int Week;
        /// <summary>
        /// 周秒，其值为 0-10080
        /// </summary>
        public int SecondOfWeek;
        /// <summary>
        /// 秒的小数部分。此数值应该在 [0 1) 区间。
        /// </summary>
        public double FractionOfSeconds;
        #endregion

       
        /// <summary>
        /// 起始时间。时间 0 点。
        /// </summary>
        public static WeekTime Zero { get { return new WeekTime(); } }

        #region 操作数
        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static WeekTime operator -(WeekTime left, double seconds) { return left - (WeekTime.FromSecond(seconds)); }
        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static WeekTime operator +(WeekTime left, int seconds) { return left + (WeekTime.FromSecond(seconds)); }
        /// <summary>
        /// 减
        /// </summary>
        /// <param name="left"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static WeekTime operator -(WeekTime left, int seconds) { return left - (WeekTime.FromSecond(seconds)); }
        /// <summary>
        /// 加
        /// </summary>
        /// <param name="left"></param>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static WeekTime operator +(WeekTime left, double seconds) { return left + (WeekTime.FromSecond(seconds)); }
        /// <summary>
        /// 取负数
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        public static WeekTime operator -(WeekTime left) { return Zero.Minus(left); }
        /// <summary>
        /// 减去
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static WeekTime operator -(WeekTime left, WeekTime right) { return left.Minus(right); }
        /// <summary>
        /// 加上
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static WeekTime operator +(WeekTime left, WeekTime right) { return left.Plus(right); }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(WeekTime left, WeekTime right) { return left.CompareTo(right) >= 0; }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(WeekTime left, WeekTime right) { return left.CompareTo(right) <= 0; }
        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(WeekTime left, WeekTime right) { return left.CompareTo(right) > 0; }
        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(WeekTime left, WeekTime right) { return left.CompareTo(right) < 0; }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(WeekTime left, WeekTime right) { return left.Equals(right); }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(WeekTime left, WeekTime right) { return !left.Equals(right); }
        #endregion

        #region override 与比较
        /// <summary>
        /// 是否相等，重写后执行效率会提高。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) { return (obj is WeekTime) && this.Equals((WeekTime)obj); }

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() { return Week.GetHashCode() + SecondOfWeek.GetHashCode() + FractionOfSeconds.GetHashCode(); }
        /// <summary>
        /// 周 周秒.秒秒
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return Week + " " + SecondOfWeek + FractionOfSeconds.ToString(".000 000 000 000 000"); }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(WeekTime other)
        {
            return this.Week == other.Week && this.SecondOfWeek == other.SecondOfWeek
                && Math.Abs(this.FractionOfSeconds - other.FractionOfSeconds) < TOLERANCE;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(WeekTime other)
        {
            if (Week != other.Week) return Week - other.Week;

            if (SecondOfWeek != other.SecondOfWeek) return SecondOfWeek - other.SecondOfWeek;

            return this.FractionOfSeconds.CompareTo(other.FractionOfSeconds);
        }
        /// <summary>
        /// 加上
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public WeekTime Plus(WeekTime right)
        {
            return new WeekTime(this.Week + right.Week, this.SecondOfWeek + right.SecondOfWeek, this.FractionOfSeconds + right.FractionOfSeconds);
        }

        /// <summary>
        /// 减去
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public WeekTime Minus(WeekTime right)
        {
            return new WeekTime(this.Week - right.Week, this.SecondOfWeek - right.SecondOfWeek, this.FractionOfSeconds - right.FractionOfSeconds);
        }
        #endregion 

        #region 构造
        /// <summary>
        /// 从周解析，精度不高。除非为整数。
        /// </summary>
        /// <param name="Weeks"></param>
        /// <returns></returns>
        public static WeekTime FromWeek(double Weeks)
        {
            int week = (int)Weeks;
            double seondsOfWeek  = (Weeks - week) % 604800;
            int secondOfWeek = (int)(seondsOfWeek);
            double fractionOfSeconds = seondsOfWeek - secondOfWeek;
            return new WeekTime(week, secondOfWeek, fractionOfSeconds);
        }
        /// <summary>
        /// 从周解析.
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public static WeekTime FromWeek(int week) { return new WeekTime(week, 0, 0.0); }

        /// <summary>
        /// 从日解析
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static WeekTime FromDay(int day)
        {
            int week = day / 7;
            int second = (day % 7) * 86400;
            return new WeekTime(week, second, 0);
        }
        /// <summary>
        /// 从日解析
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static WeekTime FromDay(double days)
        {
            int week = ((int)days) / 7;
            double seconds = (days % 7) * 86400;
            return new WeekTime(week, (int)seconds, seconds % 1);
        }
        /// <summary>
        /// 从日解析
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static WeekTime FromHour(double hours)
        {
            double days = hours  / 24;
            int day = (int)days;
            var result = FromDay(day);

            var fractionOfSeconds = (hours - day * 24) * 3600;

            result.SecondOfWeek += (int)fractionOfSeconds;
            result.FractionOfSeconds += fractionOfSeconds % 1;

            return result;
        }
        /// <summary>
        /// 从分钟解析
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static WeekTime FromMinute(double minutes)
        {
            var result = new WeekTime(minutes * 60);
            //担心精度损失，秒重新考虑
            var fraction = minutes % 1;
            result.FractionOfSeconds = (fraction * 60) % 1;
            return result;
        }
        /// <summary>
        /// 从秒解析
        /// </summary>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static WeekTime FromSecond(double seconds) { return new WeekTime(seconds); }
        /// <summary>
        /// 从秒解析
        /// </summary>
        /// <param name="fraction"></param>
        /// <returns></returns>
        public static WeekTime FromSecond(int seconds) { return new WeekTime(seconds); }
        #endregion 

    }

}