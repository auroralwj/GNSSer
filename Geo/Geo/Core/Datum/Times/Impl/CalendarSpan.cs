using System;
using System.Collections.Generic;

using System.Text;


namespace Geo.Times
{
    /// <summary>
    /// 一个时间段。采用 Decimal TotalDays 维持。
    /// 最大可表示
    /// </summary>
    public class CalendarSpan : IComparable<CalendarSpan>, IEquatable<CalendarSpan>, IComparable//, IFormattable
    {
        /// <summary>
        /// 以天初始化
        /// </summary>
        /// <param name="days"></param>
        public CalendarSpan(Decimal days =0)
        {
            this.TotalDays = days;//核心数据
        }

        /// <summary>
        /// 初始化一个时间段。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public CalendarSpan(Calendar start, Calendar end)
            : this(end.JulianDay - start.JulianDay)
        { 
            
        }
         
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return  TotalDays + "";
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            CalendarSpan o = obj as CalendarSpan;
            if (o == null) return false;

            return Equals(this, o);
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.TotalDays.GetHashCode() * 13;
        }
 
        /// <summary>
        /// 返回其值为指定实例的相反值的 Gnsser.CalendarSpan。
        /// </summary>
        /// <param name="t"> 要求反的时间间隔。</param>
        /// <returns>与此实例的数值相同，但符号相反的对象。</returns>
        public static CalendarSpan operator -(CalendarSpan t)
        {
            return new CalendarSpan(-t.TotalDays);
        }
        /// <summary>
        /// 正号。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static CalendarSpan operator +(CalendarSpan t)
        {
            return t;
        }
        /// <summary>
        ///  从另一个指定的 Gnsser.CalendarSpan 中减去指定的 Gnsser.CalendarSpan。
        /// </summary>
        /// <param name="t1"> 被减数。</param>
        /// <param name="t2">减数。</param>
        /// <returns></returns>
        public static CalendarSpan operator -(CalendarSpan t1, CalendarSpan t2)
        {
            return new CalendarSpan(t1.TotalDays - t2.TotalDays);
        }

        /// <summary>
        /// 指示两个 Gnsser.CalendarSpan 实例是否不相等。
        /// </summary>
        /// <param name="t1">要比较的第一个时间间隔。</param>
        /// <param name="t2">  要比较的第二个时间间隔。</param>
        /// <returns></returns>
        public static bool operator !=(CalendarSpan t1, CalendarSpan t2)
        {
            return t1.TotalDays != t2.TotalDays;
        }

        /// <summary>
        /// 添加两个指定的 Gnsser.CalendarSpan 实例。
        /// </summary>
        /// <param name="t1">要加上的第一个时间间隔。</param>
        /// <param name="t2"> 要加上的第二个时间间隔。</param>
        /// <returns></returns>
        public static CalendarSpan operator +(CalendarSpan t1, CalendarSpan t2)
        {
            return new CalendarSpan(t1.TotalDays + t2.TotalDays);
        }
        /// <summary>
        /// 指示指定的 Gnsser.CalendarSpan 是否小于另一个指定的 Gnsser.CalendarSpan。
        /// </summary>
        /// <param name="t1">要比较的第一个时间间隔。</param>
        /// <param name="t2">  要比较的第二个时间间隔。</param>
        /// <returns></returns>
        public static bool operator <(CalendarSpan t1, CalendarSpan t2)
        {
            return t1.TotalDays < t2.TotalDays;
        }
        /// <summary>
        /// 指示指定的 Gnsser.CalendarSpan 是否小于或等于另一个指定的 Gnsser.CalendarSpan。
        /// </summary>
        /// <param name="t1">要比较的第一个时间间隔。</param>
        /// <param name="t2">  要比较的第二个时间间隔。</param>
        /// <returns></returns>
        public static bool operator <=(CalendarSpan t1, CalendarSpan t2)
        {
            return t1.TotalDays <= t2.TotalDays;
        }

        /// <summary>
        /// 指示两个 Gnsser.CalendarSpan 实例是否相等。
        /// </summary>
        /// <param name="t1">要比较的第一个时间间隔。</param>
        /// <param name="t2">  要比较的第二个时间间隔。</param>
        /// <returns></returns>
        public static bool operator ==(CalendarSpan t1, CalendarSpan t2)
        {
            return t1.TotalDays == t2.TotalDays;
        }
        /// <summary>
        /// 指示指定的 Gnsser.CalendarSpan 是否大于另一个指定的 Gnsser.CalendarSpan。
        /// </summary>
        /// <param name="t1"> 要比较的第一个时间间隔。</param>
        /// <param name="t2"> 要比较的第二个时间间隔。</param>
        /// <returns>如果 t1 的值大于 t2 的值，则为 true；否则为 false。</returns>
        public static bool operator >(CalendarSpan t1, CalendarSpan t2)
        {
            return t1.TotalDays > t2.TotalDays;
        }

        /// <summary>
        /// 指示指定的 Gnsser.CalendarSpan 是否大于或等于另一个指定的 Gnsser.CalendarSpan。
        /// </summary>
        /// <param name="t1"> 要比较的第一个时间间隔。</param>
        /// <param name="t2">要比较的第二个时间间隔。</param>
        /// <returns> 如果 t1 的值大于或等于 t2 的值，则为 true；否则为 false。</returns>
        public static bool operator >=(CalendarSpan t1, CalendarSpan t2)
        {
            return t1.TotalDays >= t2.TotalDays;
        }
        /// <summary>
        ///   获取当前 Gnsser.CalendarSpan 结构所表示的时间间隔的天数部分。此实例的天数部分。返回值可以是正数也可以是负数。
        /// </summary>
        public int Days { get { return (int)TotalDays; } }
        /// <summary>
        ///  获取当前 Gnsser.CalendarSpan 结构所表示的时间间隔的小时数部分。 
        /// 当前 Gnsser.CalendarSpan 结构的小时分量。返回值的范围为 -23 到 23。
        /// </summary>
        public int Hours
        {
            get
            {
                return (int)(TotalHours % 24);
            }
        }
        /// <summary>
        ///  获取当前 Gnsser.CalendarSpan 结构所表示的时间间隔的毫秒数部分。   当前 Gnsser.CalendarSpan 结构的毫秒分量。返回值的范围为 -999 到 999。
        /// </summary>
        public int Milliseconds { get { return ((int)TotalMilliseconds % 1000); } }

        /// <summary>
        ///  获取当前 Gnsser.CalendarSpan 结构所表示的时间间隔的分钟数部分。当前 Gnsser.CalendarSpan 结构的分钟分量。返回值的范围为 -59 到 59。
        /// </summary>
        public int Minutes { get { return (int)(TotalMinutes % 60); } }
        /// <summary>
        /// 获取当前 Gnsser.CalendarSpan 结构所表示的时间间隔的秒数部分。 当前 Gnsser.CalendarSpan 结构的秒分量。返回值的范围为 -59 到 59。
        /// </summary>
        public int Seconds { get { return (int)(TotalSeconds % 60); } }
        /// <summary>
        ///   获取以整天数和天的小数部分表示的当前 Gnsser.CalendarSpan 结构的值。
        /// </summary>
        public Decimal TotalDays { get; protected set; }
        /// <summary>
        /// 获取以整小时数和小时的小数部分表示的当前 Gnsser.CalendarSpan 结构的值。
        /// </summary>
        public Decimal TotalHours { get { return TotalDays * 24; } }
        /// <summary>
        ///  获取以整毫秒数和毫秒的小数部分表示的当前 Gnsser.CalendarSpan 结构的值。
        /// </summary>
        public Decimal TotalMilliseconds { get { return TotalDays * TimeConsts.DAY_TO_MILLISECOND; } }
        /// <summary>
        ///  获取以整分钟数和分钟的小数部分表示的当前 Gnsser.CalendarSpan 结构的值。 
        /// </summary>
        public Decimal TotalMinutes { get { return TotalDays * TimeConsts.DAY_TO_MINUTE; } }
        /// <summary>
        /// 获取以整秒数和秒的小数部分表示的当前 Gnsser.CalendarSpan 结构的值。
        /// </summary>
        public Decimal TotalSeconds { get { return TotalDays * TimeConsts.DAY_TO_SECOND; } }

        /// <summary>
        /// 将指定的 Gnsser.CalendarSpan 添加到此实例中。
        /// </summary>
        /// <param name="ts">要加上的时间间隔。</param>
        /// <returns>一个对象，表示此实例的值加 ts 的值。</returns>
        public CalendarSpan Add(CalendarSpan ts)
        {
            return FromDays(TotalDays + ts.TotalDays);
        }
        /// <summary>
        /// 比较两个 Gnsser.CalendarSpan 值，并返回一个整数，该整数指示第一个值是短于、等于还是长于第二个值。
        /// </summary>
        /// <param name="t1"> 要比较的第一个时间间隔。</param>
        /// <param name="t2"> 要比较的第二个时间间隔。</param>
        /// <returns>以下值之一。值说明-1t1 短于 t2。0t1 等于 t2。1t1 长于 t2。</returns>
        public static int Compare(CalendarSpan t1, CalendarSpan t2)
        {
            return (int)(t1.TotalDays - t2.TotalDays);
        }
        /// <summary>
        ///  将此实例与指定对象进行比较，并返回一个整数，该整数指示此实例是短于、等于还是长于指定对象。
        /// </summary>
        /// <param name="value">要比较的对象，或为 null。</param>
        /// <returns>以下值之一。值说明-1此实例短于 value。0此实例等于 value。1此实例长于 value。- 或 -value 为 null。</returns>
        public int CompareTo(object value)
        {
            if (!(value is CalendarSpan)) throw new ArgumentException("类型不对");
            return CompareTo(value as Calendar);
        }
        /// <summary>
        /// 将此实例与指定的 Gnsser.CalendarSpan 对象进行比较，并返回一个整数，该整数指示此实例是短于、等于还是长于 Gnsser.CalendarSpan对象。
        /// </summary>
        /// <param name="value"> 要与此实例进行比较的对象。</param>
        /// <returns>一个有符号数字，该数字指示此实例与 value 的相对值。值说明负整数此实例短于 value。零此实例等于 value。正整数此实例长于 value。</returns>
        public int CompareTo(CalendarSpan value)
        {
            return (int)(TotalDays - value.TotalDays);
        }
        /// <summary>
        /// 返回新的 Gnsser.CalendarSpan 对象，其值是当前 Gnsser.CalendarSpan 对象的绝对值。
        /// </summary>
        /// <returns></returns>
        public CalendarSpan Duration()
        {
            return new CalendarSpan(Math.Abs(TotalDays));
        }

        /// <summary>
        /// 返回一个值，该值指示此实例是否与指定的 Gnsser.CalendarSpan 对象相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(CalendarSpan obj)
        {
            return Equals(this, obj);
        }

        /// <summary>
        /// 返回一个值，该值指示 Gnsser.CalendarSpan 的两个指定实例是否相等。
        /// </summary>
        /// <param name="t1">要比较的第一个时间间隔。</param>
        /// <param name="t2">要比较的第二个时间间隔。</param>
        /// <returns>如果 t1 和 t2 的值相等，则为 true；否则为 false。</returns>
        public static bool Equals(CalendarSpan t1, CalendarSpan t2)
        {
            return t1.TotalDays == t2.TotalDays;
        }
        /// <summary>
        ///  返回表示指定天数的 Gnsser.CalendarSpan
        /// </summary>
        /// <param name="value">天数</param>
        /// <returns></returns>
        public static CalendarSpan FromDays(Decimal value)
        {
            return new CalendarSpan(value);
        }
        /// <summary>
        /// 返回表示指定小时数的 Gnsser.CalendarSpan
        /// </summary>
        /// <param name="value">小时数</param>
        /// <returns></returns>
        public static CalendarSpan FromHours(Decimal value)
        {
            return FromDays(value * 24);
        }
        /// <summary>
        ///  返回表示指定毫秒数的 Gnsser.CalendarSpa
        /// </summary>
        /// <param name="value">毫秒数</param>
        /// <returns></returns>
        public static CalendarSpan FromMilliseconds(Decimal value)
        {
            return new CalendarSpan(value * TimeConsts.MILLISECOND_TO_DAY);
        }
        /// <summary>
        /// 返回表示指定分钟数的 Gnsser.CalendarSpan
        /// </summary>
        /// <param name="value">分钟数</param>
        /// <returns></returns>
        public static CalendarSpan FromMinutes(Decimal value)
        {
            return new CalendarSpan(value * TimeConsts.MINUTE_TO_DAY);
        }

        /// <summary>
        /// 返回表示指定秒数的 Gnsser.CalendarSpan
        /// </summary>
        /// <param name="value">秒数</param>
        /// <returns></returns>
        public static CalendarSpan FromSeconds(Decimal value)
        {
            return new CalendarSpan(value * TimeConsts.SECOND_TO_DAY);
        }
    }
}
