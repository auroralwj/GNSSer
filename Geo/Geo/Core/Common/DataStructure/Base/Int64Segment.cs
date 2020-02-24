//2018.11.23, czs, create in ryd, 64整型的数据范围

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{

    /// <summary>
    /// 32位整型的数据范围。
    /// </summary>
    public class Int64Segment : Segment<Int64, Int64>
    {
        /// <summary>
        /// 32位整型的数据范围
        /// </summary>
        /// <param name="start"></param>
        public Int64Segment(Int64 start)
            : base(start, start)
        {

        }
        /// <summary>
        /// 32位整型的数据范围,构造函数。
        /// </summary>
        /// <param name="start">起始数值</param>
        /// <param name="end">结束值</param>
        public Int64Segment(Int64 start, Int64 end): base(start, end)
        { 
        }
        /// <summary>
        /// 检查并扩展，扩展则返回true
        /// </summary>
        /// <param name="val"></param>
        public bool CheckOrExpand(Int64 val)
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
        public override bool Contains(Int64 val)
        {
            return val <= End && val >= Start;
        }
        /// <summary>
        /// 跨度
        /// </summary>
        public override Int64 Span
        {
            get { return End - Start; }
        }
        /// <summary>
        /// 中间数
        /// </summary>
        public override Int64 Median
        {
            get { return Start + End / 2; }
        }
        /// <summary>
        /// 一个副本
        /// </summary>
        /// <returns></returns>
        public Int64Segment Clone() { return new Int64Segment(this.Start, this.End); }
    }
}
