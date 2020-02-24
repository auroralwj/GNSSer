//2014.12.25, czs, create in namu, 一段，有两个标量组成。

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
    public abstract class SuccessiveSegment<TSegment, TValue, TSpan>
        : Segment<TValue, TSpan>, ISegment<TValue>, IEnumerable<TSegment>
        where TSegment : ISegment<TValue, TSpan>
        where TValue : IComparable<TValue>
    {
        /// <summary>
        /// 段的构造函数 
        /// </summary> 
        public SuccessiveSegment()
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
        /// 最后一个时段
        /// </summary>
        public TSegment Last => Segments.LastOrDefault();

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
        /// 返回第一个匹配的时段。若没有则返回默认值。
        /// </summary>
        /// <param name="time">指定时间</param>
        /// <returns>若没有则返回null</returns>
        public TSegment GetSegment(TValue time)
        {
            return this.Segments.FirstOrDefault(m => m.Contains(time));
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string splitter = ";";
            int i = 0;
            foreach (var item in this.Segments)
            {
                if (i != 0) sb.Append(splitter);

                sb.Append(item.ToString()); 
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
