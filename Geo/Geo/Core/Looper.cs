//2017.11.07, czs, create in hongiqng, 遍历器

using System;
using System.Collections.Generic;
using System.Text; 
using Geo.Coordinates; 
using System.Linq; 
using Geo.Algorithm.Adjust;
using Geo;
using Geo.IO;
using Geo.Algorithm; 
using Geo.Data;
using Geo.Times;
using Geo.Utils;

namespace Geo
{
    /// <summary>
    /// 循环条件
    /// </summary>
    public class Looper : AbstractProcess
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Looper()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="Step"></param>
        /// <param name="isIncludedEnding"></param>
        public Looper(double From, double To, double Step, bool isIncludedEnding = true)
        {
            this.From = From;
            this.End = To;
            this.Step = Step;
            this.IsEndIncluded = isIncludedEnding;
        }
        /// <summary>
        /// 事件
        /// </summary>
        public event Action<double> Looping;
        /// <summary>
        /// 起始（含）
        /// </summary>
        public double From { get; set; }
        /// <summary>
        /// 结束
        /// </summary>
        public double End { get; set; }
        /// <summary>
        /// 步长
        /// </summary>
        public double Step { get; set; }
        /// <summary>
        /// 是否包含结束
        /// </summary>
        public bool IsEndIncluded { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int Count { get { return (int)Math.Round(Math.Abs((From - End) / Step)); } }
        /// <summary>
        /// 当前
        /// </summary>
        public double Current { get; set; }
        public override void Run()
        {
            if (Step == 0) { throw new ArgumentException("步长不可为 0 。"); }

            InitProcess(Count);
            //注意：此处循环条件可以提取为函数，但是顾及效率，此处采用枚举遍历的方式。待测试效率。2017.11.07， czs， hongqing
            if (Step > 0)
            {
                if (IsEndIncluded)
                    for (double i = From; i <= End; i += Step)
                    {
                        OnLooping(i);
                    }
                else for (double i = From; i < End; i += Step)
                    {
                        OnLooping(i);
                    }
            }
            else
            {
                if (IsEndIncluded)
                    for (double i = From; i >= End; i += Step)
                    {
                        OnLooping(i);
                    }
                else for (double i = From; i > End; i += Step)
                    {
                        OnLooping(i);
                    }
            }
        }

        protected void OnLooping(double val)
        {
            Current = val;
            if (Looping != null) { Looping(val); }
            this.PerformProcessStep();
        }

        public override string ToString()
        {
            return From + "->" + End + "," + Step + ", " + Current;
        }
    }

}
