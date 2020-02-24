//2014.12.25, czs, create in namu, 一段，有两个标量组成。
//2015.05.11, czs, create in namu, 具有缓冲的多个分段，断断续续

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{ 
    /// <summary>
    /// 断断续续的分段。
    /// </summary>
    /// <typeparam name="TSegment">区段</typeparam>
    /// <typeparam name="TValue">数值</typeparam>
    /// <typeparam name="TSpan">长度</typeparam>
    public abstract class BufferedSuccessiveSegment<TSegment, TValue, TSpan>
        : BufferedSegment<TValue, TSpan>, ISegment<TValue>, IEnumerable<TSegment>
        where TSegment : IBufferedSegment<TValue, TSpan>
        where TValue : IComparable<TValue>
    {
        /// <summary>
        /// 段的构造函数 
        /// </summary> 
        public BufferedSuccessiveSegment()
        {
            this.Segments = new List<TSegment>();
        }

        /// <summary>
        /// 独立时段数量。
        /// </summary>
        public int Count { get { return Segments.Count; } }

        /// <summary>
        /// 数据，核心存储
        /// </summary>
       protected  List<TSegment> Segments { get; set; }



       /// <summary>
       /// 具有缓冲的起始
       /// </summary>
       public override TValue BufferedStart
       {
           get
           {
               TValue val = default(TValue);
               int i = 0;
               foreach (var item in Segments)
               {
                   if (i == 0) val = item.BufferedStart;

                   if (item.BufferedStart.CompareTo(val) < 0) val = item.BufferedStart;
                   i++;
               }

               return val;
           }
       }
        /// <summary>
        /// 具有缓冲的结束
        /// </summary>
       public override TValue BufferedEnd
       {
           get
           {
               TValue val = default(TValue);
               int i = 0;
               foreach (var item in Segments)
               {
                   if (i == 0) val = item.BufferedEnd;

                   if (item.BufferedEnd.CompareTo(val) > 0) val = item.BufferedEnd;
                   i++;
               }

               return val;
           } 
       }


        /// <summary>
        /// 起始
        /// </summary>
        public override TValue Start
        {
            get
            {
                TValue val = default(TValue);
                int i = 0;
                foreach (var item in Segments)
                {
                    if (i == 0) val = item.Start;

                    if (item.Start.CompareTo(val) < 0) val = item.Start;
                    i++;
                }

                return val;
            }
        }
        /// <summary>
        /// 结束
        /// </summary>
        public override TValue End
        {
            get
            {
                TValue val = default(TValue);
                int i = 0;
                foreach (var item in Segments)
                {
                    if (i == 0) val = item.End;

                    if (item.End.CompareTo(val) > 0) val = item.End;
                    i++;
                }

                return val;
            } 
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="segment">分段</param>
        public virtual void Add(TSegment segment) { this.Segments.Add(segment); }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="segment">段落</param>
        public virtual void Remove(TSegment segment) { this.Segments.Remove(segment); }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override bool Contains(TValue val)
        {
            foreach (var item in Segments)
            {
                if (item.Contains(val)) return true;
            }
            return false;
        }

        /// <summary>
        /// 返回时段。若没有则返回默认值。
        /// </summary>
        /// <param name="time">指定时间</param>
        /// <returns>若没有则返回null</returns>
        public TSegment GetSegment(TValue time)
        {
            foreach (var period in this.Segments)
            {
                if (period.Contains(time)) return period;
            }
            return default(TSegment);
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string splitter = ",";
            int i = 0;
            foreach (var item in this.Segments)
            {
                if (i != 0) sb.Append(splitter);

                sb.Append("[" + item.ToString() + "]");
                i++;
            }

            return sb.ToString();
        }


       #region IEnumerator
        /// <summary>
        /// IEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TSegment> GetEnumerator()
        {
            return Segments.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Segments.GetEnumerator();
        }
        #endregion
    } 
}
