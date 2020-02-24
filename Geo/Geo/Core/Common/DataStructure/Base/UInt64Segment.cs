//2018.11.23, czs, create in ryd,UInt64整型的数据范围

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{

    /// <summary>
    /// 64无符号位整型的数据范围。
    /// </summary>
    public class UInt64Segment : Segment<UInt64, UInt64>
    {
        /// <summary>
        /// 64无符号整型的数据范围
        /// </summary>
        /// <param name="start"></param>
        public UInt64Segment(UInt64 start)
            : base(start, start)
        {

        }
        /// <summary>
        /// 64无符号整型的数据范围,构造函数。
        /// </summary>
        /// <param name="start">起始数值</param>
        /// <param name="end">结束值</param>
        public UInt64Segment(UInt64 start, UInt64 end): base(start, end)
        { 
        }
        /// <summary>
        /// 检查并扩展，扩展则返回true
        /// </summary>
        /// <param name="val"></param>
        public bool CheckOrExpand(UInt64 val)
        {
            bool isAdded = false;
            if (Start > val) { Start = val; isAdded = true; }
            if (End < val) { End = val; isAdded = true; }
            return isAdded;
        }


        /// <summary>
        /// 是否在范围内，含边界。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override bool Contains(UInt64 val)
        {
            return val <= End && val >= Start;
        }
        /// <summary>
        /// 跨度
        /// </summary>
        public override UInt64 Span
        {
            get { return End - Start; }
        }
        /// <summary>
        /// 中间数
        /// </summary>
        public override UInt64 Median
        {
            get { return Start + End / 2; }
        }
        /// <summary>
        /// 一个副本
        /// </summary>
        /// <returns></returns>
        public UInt64Segment Clone() { return new UInt64Segment(this.Start, this.End); }
    }
}
