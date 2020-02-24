//2015.01.08, czs, create in namu shangliao, 断断续续的时段
//2015.05.11, czs, create in namu, 具有时段缓冲的多个组成的时段
//2015.10.18, czs, edit in pengzhou railway station, 相反的时段，即有时段的为空，没有时段的为时段。

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geo;

namespace Geo.Times
{ 
    /// <summary>
    /// 断断续续的时段。时段约定：各个时段不相接或者相交，否则认为是一个时段。
    /// </summary>
    public class BufferedSuccessiveTimePeriod : SuccessiveSegment<BufferedTimePeriod, Time, Double>, IEnumerable<BufferedTimePeriod>
    {
        /// <summary>
        /// 断断续续的时段,构造函数。
        /// </summary>
        public BufferedSuccessiveTimePeriod()
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
        /// 添加到序列中，自动拼接。
        /// </summary>
        /// <param name="timePeriod">时段</param>
        public override void Add(BufferedTimePeriod timePeriod)
        {
            //尝试提取包含首位的两段
            BufferedTimePeriod start = GetSegment(timePeriod.Start);
            BufferedTimePeriod end = GetSegment(timePeriod.End);

            //几种情况
            //1.不包含
            if (start == null && end == null)
            {
                this.Segments.Add(timePeriod);
            }
            //2.包含头部
            else if (start != null && end == null)
            {
                start.End = timePeriod.End;
            }
            //3.包含尾部
            else if (start == null && end != null)
            {
                end.Start = timePeriod.Start;
            }
            //4.包含
            else if (start != null && end != null)
            {
                //4.1.同一个,则不管
                //4.2.非同一个，说明此时段为填空，合并前后两个为 1 个，消去多余的一个。
                if (!start.Equals(end))
                {
                    start.End = end.End;
                    this.Segments.Remove(end);
                }
            }
        }

        /// <summary>
        /// 移除一个时段。
        /// </summary>
        /// <param name="timePeriod">时段</param>
        public override void Remove(BufferedTimePeriod timePeriod)
        {
            List<BufferedTimePeriod> tobeRemove = new List<BufferedTimePeriod>();
            List<BufferedTimePeriod> tobeAdd = new List<BufferedTimePeriod>();
            //时段流水作业
            foreach (var item in this.Segments)
            {
                //1.早于被移除时段，则袖手旁观
                if (item.End < timePeriod.Start)
                {
                    continue;
                }
                //2.与移除时段前部相交，则忍痛截断相交部分
                else if (item.End < timePeriod.Start)
                {
                    item.End = timePeriod.Start;
                }
                //3.全包含这个区段，则抠出这个区段
                else if (item.Contains(timePeriod))
                {
                    BufferedTimePeriod contaiedPeriod = GetSegment(timePeriod.End);
                    BufferedTimePeriod laterPeriod = new BufferedTimePeriod(timePeriod.End, contaiedPeriod.End, timePeriod.EndBuffer, contaiedPeriod.EndBuffer);
                    contaiedPeriod.End = timePeriod.Start;

                    tobeAdd.Add(laterPeriod);
                }
                //4.被这个时段包含，则抛弃之
                else if (timePeriod.Contains(item))
                {
                    tobeRemove.Add(item);
                }
                //5.与这个时段后半部分相交，则忍痛截断相交部分
                else if (item.Start < timePeriod.End)
                {
                    item.Start = timePeriod.End;
                }
            }

            foreach (var item in tobeAdd)
            {
                this.Add(item);
            }
            foreach (var item in tobeRemove)
            {
                this.Remove(item);
            }

        }

        /// <summary>
        /// 相反的时段，即有时段的为空，没有时段的为时段。
        /// </summary>
        public BufferedSuccessiveTimePeriod Oppersite
        {
            get
            {
                BufferedSuccessiveTimePeriod newObj = new BufferedSuccessiveTimePeriod();
                
                newObj.Add(new BufferedTimePeriod(Start, End));
                foreach (var item in this)
                {
                    newObj.Remove(item);
                }
                return newObj;
            }
        }

      
        /// <summary>
        /// 提取长度小于或大于指定参数的时段
        /// </summary>
        /// <param name="span">时段长度（含）</param>
        /// <param name="smallerOrLarger">小于或大于，true为小于（默认）</param>
        /// <returns></returns>
        public BufferedSuccessiveTimePeriod GetFilteredPeriods(double span = 40 * 30, bool smallerOrLarger = true)
        { 
            BufferedSuccessiveTimePeriod period = new BufferedSuccessiveTimePeriod();

            foreach (var item in this)
            {
                if (smallerOrLarger )
                {
                    if (item.Span <= span)
                    { period.Add(item); }
                }
                else
                {
                    if (item.Span >= span)
                    { period.Add(item); }
                }
            } 
            return period;
        }
    }
}