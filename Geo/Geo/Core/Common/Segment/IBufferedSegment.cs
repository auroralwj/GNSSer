//2015.05.11, czs, create in namu,提取缓冲的一维段

using System;

namespace Geo
{
    /// <summary>
    /// 具有缓冲的一维段接口
    /// </summary>
    /// <typeparam name="TValue">数值</typeparam>
    /// <typeparam name="TSpan">范围</typeparam>
   public  interface IBufferedSegment<TValue, TSpan> :ISegment<TValue>
     where TValue : IComparable<TValue>
    {
       /// <summary>
       /// 含缓冲后起始
       /// </summary>
        TValue BufferedStart { get; }
       /// <summary>
       /// 含缓冲后的终止
       /// </summary>
        TValue BufferedEnd { get; }
       /// <summary>
       /// 包含起始缓冲后的范围
       /// </summary>
        TSpan BufferedSpan { get; }
       /// <summary>
       /// 起始段的缓冲大小
       /// </summary>
        TSpan StartBuffer { get; set; }
       /// <summary>
       /// 结束端的缓冲大小
       /// </summary>
        TSpan EndBuffer { get; set; }

        /// <summary>
        /// 完全包含或相等才返回true，包含缓冲部分。
        /// </summary>
        /// <param name="val">待判断时段</param>
        /// <returns></returns>
        bool BufferedContains(TValue val);
        /// <summary>
        /// 是否包含这段
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        bool BufferedContains(ISegment<TValue, TSpan> segment);
        /// <summary>
        /// 是否包含这段
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        bool BufferedContains(IBufferedSegment<TValue, TSpan> segment); 
       /// <summary>
       /// 是否相交
       /// </summary>
       /// <param name="segment"></param>
       /// <returns></returns>
        bool BufferedIntersect(ISegment<TValue, TSpan> segment);
       
        /// <summary> 
        /// 是否相交，包含缓冲部分。
        /// </summary>
        /// <param name="segment">另一个段</param> 
        /// <returns></returns>
        bool BufferedIntersect(IBufferedSegment<TValue, TSpan> segment);
    }
}
