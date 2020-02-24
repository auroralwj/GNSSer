//2015.01.08, czs, create in namu shangliao, 断断续续的时段
//2016.08.03, czs, create in fujian yongan, 多个时段的拼接
//2018.12.27, czs, edit in ryd, 自动增加

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geo;
using Geo.Times;

namespace Geo.Times
{ 
    /// <summary>
    /// 多个时段的拼接。按照时间先后排列，如果获取，则返回匹配的第一个时段。
    /// </summary>
    public class SuccessiveTimePeriod : SuccessiveSegment<TimePeriod, Time, Double>, IEnumerable<TimePeriod>
    {
        /// <summary>
        ///  多个时段的拼接。按照时间先后排列，如果获取，则返回匹配的第一个时段。
        /// </summary>
        public SuccessiveTimePeriod()
        { 
        }

        /// <summary>
        /// 单位秒
        /// </summary>
        public override double Span  {   get { return End - Start; }   }
        /// <summary>
        /// 中间数
        /// </summary>
        public override Time Median
        {
            get { return Start + Span / 2.0; }
        }

        /// <summary>
        /// 获取时段
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TimePeriod Get(Time time)
        {
            var find = this.GetSegment(time);
            if(find == null)
            {
                return find;
            }
            return find;

        }

        /// <summary>
        /// 获取或创建
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TimePeriod GetOrCreate(Time time)
        {
           var find = this.GetSegment(time);
            if(find == null)
            {
                find = new TimePeriod(time, time);
                this.Add(find);
            }
            return find;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static SuccessiveTimePeriod Parse(string content)
        {
            SuccessiveTimePeriod periods = new SuccessiveTimePeriod();
            var segments = content.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in segments)
            {
                 var tp = TimePeriod.Parse(item);
                 periods.Add(tp);
            }

            return periods;
        }

    }
}