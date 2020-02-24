using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;


namespace Gnsser.Times
{
    /// <summary>
    /// 周秒表示法。 GPS 时间类表示方法。
    /// </summary>
    public class GnssTime : IComparable<GnssTime>, IEquatable<GnssTime>, IGnssTime
    {
        /// <summary>
        /// 日历时间转换为GNSS周周秒时间
        /// </summary>
        /// <param name="calendar">日历</param>
        /// <param name="gnssOriginMjdDay">GNSS时间起算点</param>
        /// <returns></returns>
        public  GnssTime(Calendar calendar, GnssSystem gnssSystem = null)
        { 
            if (gnssSystem == null) gnssSystem = GnssSystem.Gps;
            this.GnssSystem = gnssSystem;

            int Week;
            Decimal secondsOfWeek;
            TimeConvert.CalendarToGnssTime(calendar, gnssSystem.OriginMjdDay, out Week, out secondsOfWeek);

            this.GnssWeek = Week;
            this.SecondsOfWeek = new Second(secondsOfWeek);
        }

        /// <summary>
        /// 构造函数。周秒。
        /// </summary>
        /// <param name="week">周,可以为负数</param>
        /// <param name="secondsOfWeek">周内秒,自动判断大小，增减week,保证在 [0-604800) 区间</param>
        public GnssTime(int week, Decimal secondsOfWeek, GnssSystem gnssSystem = null)
        {  
            if (gnssSystem == null) gnssSystem = GnssSystem.Gps;
            this.GnssSystem = gnssSystem;

            //判断秒，是否在一周内，如果不在，则转换。
            if (secondsOfWeek >= TimeConsts.SECOND_PER_WEEK)
            {
                week += (int)(secondsOfWeek / TimeConsts.SECOND_PER_WEEK);
                secondsOfWeek = secondsOfWeek % TimeConsts.SECOND_PER_WEEK;
            }
            else if (secondsOfWeek < 0)
            {
                int wk = (int) Math.Ceiling( Math.Abs(secondsOfWeek / TimeConsts.SECOND_PER_WEEK));
                week -= wk;
                secondsOfWeek = secondsOfWeek + wk * TimeConsts.SECOND_PER_WEEK;
            }

            this.GnssWeek = week;
            this.SecondsOfWeek = new Second(secondsOfWeek);           
        }

        #region 核心属性
        /// <summary>
        /// GPS Week
        /// </summary>
        public int GnssWeek { get; protected set; }
        /// <summary>
        /// 周内秒。
        /// </summary>
        public ISecond SecondsOfWeek { get; protected set; }
        /// <summary>
        /// GNSS系统
        /// </summary>
        public GnssSystem GnssSystem { get; protected set; }        
        #endregion

        public double Tolerance = 1e-15;

        #region 扩展属性
        /// <summary>
        /// 返回年月日表示的时间。
        /// </summary>
        public Calendar Calendar
        {
            get
            {
                Decimal daysOfWeek = (this.SecondsOfWeek.DecimalValue * TimeConsts.SECOND_TO_DAY);
                Decimal mjd = this.GnssWeek * 7 + daysOfWeek;

                return TimeConvert.MjdToCalendar(mjd + this.GnssSystem.OriginMjdDay);
              //  return TimeConvert.GnssTimeToCalendar(this);
            }
        }
    
        #endregion

        #region 方法重写,接口实现
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            GnssTime o = obj as GnssTime;
            if (o == null) return false;

            return Equals(  o);
        }

        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return GnssWeek.GetHashCode() + SecondsOfWeek.GetHashCode() * 3;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GnssWeek + " " + SecondsOfWeek;
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="gnssTime"></param>
        /// <returns></returns>
        public int CompareTo(GnssTime gnssTime)
        {
            int week = this.GnssWeek - gnssTime.GnssWeek;
            if (week != 0) return week;
            return (int)(SecondsOfWeek.DecimalValue - gnssTime.SecondsOfWeek.DecimalValue);
        }
        /// <summary>
        ///  是否相等。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GnssTime other)
        {
            return GnssWeek == other.GnssWeek && SecondsOfWeek.Equals(other.SecondsOfWeek);
        }
        #endregion

        #region 操作数
        public static GnssTime operator +(GnssTime time, Second second)
        {
            return new GnssTime(time.GnssWeek, time.SecondsOfWeek.DecimalValue + second.DecimalValue, time.GnssSystem);
        }
        public static GnssTime operator -(GnssTime time, Second second)
        {
            return new GnssTime(time.GnssWeek, time.SecondsOfWeek.DecimalValue - second.DecimalValue, time.GnssSystem);
        }
        public static GnssTime operator -(GnssTime time, double second)
        {
            return new GnssTime(time.GnssWeek, time.SecondsOfWeek.DecimalValue - new Decimal(second));
        }
        public static GnssTime operator +(GnssTime time, double second)
        {
            return new GnssTime(time.GnssWeek, time.SecondsOfWeek.DecimalValue + new Decimal(second));
        }
        public static bool operator >(GnssTime t1, GnssTime t2)
        {
            return t1.Calendar.JulianDay > t2.Calendar.JulianDay;
        }
        public static bool operator <(GnssTime t1, GnssTime t2)
        {
            return t1.Calendar.JulianDay < t2.Calendar.JulianDay;
        }
        public static bool operator >=(GnssTime t1, GnssTime t2)
        {
            return t1.Calendar.JulianDay >= t2.Calendar.JulianDay;
        }
        public static bool operator <=(GnssTime t1, GnssTime t2)
        {
            return t1.Calendar.JulianDay <= t2.Calendar.JulianDay;
        }
        //public static bool operator ==(GnssTime t1, GnssTime t2)
        //{
        //    if (t1 == null || t2 == null) return false;

        //    return t1.Calendar.JulianDay == t2.Calendar.JulianDay;
        //}
        //public static bool operator !=(GnssTime t1, GnssTime t2)
        //{
        //    if (t1 == null || t2 == null) return false;

        //    return t1.Calendar.JulianDay != t2.Calendar.JulianDay;
        //}

        #endregion
    }
}
