//2018.07.10, czs, create in hmx, 多项式拟合生成器
//2018.07.13, czs, edit in HMX, 改进中


using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using Geo.Algorithm;
using Geo.IO;

namespace Geo.Algorithm
{
    /// <summary>
    /// 拟合类型
    /// </summary>
    public enum PolyfitType
    {
        /// <summary>
        /// 所有数据一次性拟合
        /// </summary>
        WholeData,
        /// <summary>
        /// 逐历元滑动平均
        /// </summary>
        MovingWindow,
        /// <summary>
        /// 独立相连的窗口
        /// </summary>
        IndependentWindow,
        /// <summary>
        /// 重叠分段窗口
        /// </summary>
        OverlapedWindow,
    }


    /// <summary>
    /// 分段拟合历元数量计算器.
    /// </summary>
    public class MovingWindowCounter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="polyfitCount">用于拟合是数据量</param>
        /// <param name="polyfitType">是否采用重合区，否则 margin和Overlap为0</param>
        /// <param name="MarginCount">边界数量，如不指定，则采用默认算法</param>
        /// <param name="OverlapCount">重合数量，如不指定，则采用默认算法</param>
        public MovingWindowCounter(int polyfitCount, PolyfitType polyfitType, int MarginCount = 0, int OverlapCount = 0)
        {
            this.PolyfitCount = polyfitCount;
            this.PolyfitType = polyfitType;
            /*
             *规则：
             * 1.数据通过缓存，预先存入滑动窗口；
             * 2.遍历器开始拟合，从互动窗口读取数据。
             *
             *滑动窗口应该存储当前值，当前以后 (PolyfitCount - MarginCoun)，以及当前之前 MarginCount 的数据，即滑动窗口最小为 PolyfitCount。
             * 由于滑动窗口中在数据读取过程中，随着缓存的加入而不断释放先前的数据，
             * 因此缓存必须得到控制：一方面确保在前置重叠区时有必要的拟合数据 (PolyfitCount - MarginCoun)，
             * 其次，也要保留必要的历史数据 MarginCount，因此滑动缓存大小应该为：  (PolyfitCount - MarginCoun)。
             * 
             */
            switch (polyfitType)
            {
                case PolyfitType.WholeData:
                    this.OverlapCount = 0;
                    this.MarginCount = 0;
                    this.BufferSize = int.MaxValue;
                    this.WindowSize = int.MaxValue;
                    break;
                case PolyfitType.MovingWindow:
                    this.OverlapCount = 0;
                    this.MarginCount = 0;
                    this.BufferSize = this.PolyfitCount / 2;
                    this.WindowSize = this.PolyfitCount;
                    break;
                case PolyfitType.IndependentWindow:
                    this.OverlapCount = 0;
                    this.MarginCount = 0;
                    this.BufferSize = this.PolyfitCount;// - MarginCount;//缓存为后续数据，至少包含一半，若有边界，则至少再为拟合长度减去边界
                    this.WindowSize = this.BufferSize + this.MarginCount + 1;//窗口保存前后数据，至少包含拟合长度
                    break;
                case PolyfitType.OverlapedWindow:
                    if (MarginCount == 0 && OverlapCount == 0)
                    {
                        AutoSetMarginAndOverlapCount();
                    }
                    else
                    {
                        this.OverlapCount = OverlapCount;
                        this.MarginCount = MarginCount;
                    }
                    this.BufferSize = this.PolyfitCount;// - MarginCount;//缓存为后续数据，至少包含一半，若有边界，则至少再为拟合长度减去边界
                    this.WindowSize = this.BufferSize + this.MarginCount + 1;//窗口保存前后数据，至少包含拟合长度
                    break;
                default:
                    break;
            } 
        }

        private void AutoSetMarginAndOverlapCount()
        {
            if (PolyfitCount < 10)//太小了就直接用常规拟合
            {
                this.MarginCount = 0;
                this.OverlapCount = 0;
            }
            else if (PolyfitCount <= 20)
            {
                this.OverlapCount = PolyfitCount / 4;
                this.MarginCount = PolyfitCount / 4;
            }
            else if (PolyfitCount < 50)
            {
                this.OverlapCount = PolyfitCount / 3;
                this.MarginCount = PolyfitCount / 4;
            }
            else if (PolyfitCount < 100)
            {
                this.OverlapCount = PolyfitCount / 3;
                this.MarginCount = PolyfitCount / 4;
            }
            else if (PolyfitCount < 500)
            {
                this.OverlapCount = PolyfitCount /4;
                this.MarginCount = PolyfitCount / 4;
            }
            else
            {
                this.OverlapCount = PolyfitCount / 4;
                this.MarginCount = PolyfitCount / 5;
            }
        }
        /// <summary>
        /// 多项式拟合类型
        /// </summary>
        public PolyfitType PolyfitType { get; set; }
        /// <summary>
        /// 数据量缓存大小
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 数据窗口大小
        /// </summary>
        public int WindowSize { get; set; }
        /// <summary>
        /// 最大拟合窗口大小
        /// </summary>
        public int PolyfitCount { get; set; }
        /// <summary>
        /// 拟合边缘区
        /// </summary>
        public int MarginCount { get; set; }
        /// <summary>
        /// 重贴区
        /// </summary>
        public int OverlapCount { get; set; }
        /// <summary>
        /// 比较总数，并返回不大于总数的窗口。
        /// </summary>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public int GetWindowSize(int totalCount) {
              if(WindowSize > totalCount) { return totalCount; }
            return WindowSize;
        }
        /// <summary>
        /// 比较总数，并返回不大于总数的窗口。
        /// </summary>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public int GetBufferSize(int totalCount) {
              if(BufferSize > totalCount) { return totalCount; }
            return BufferSize;
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "PolyfitCount: " + PolyfitCount
                + ",MarginCount: " + MarginCount
                + ",OverlapCount: " + OverlapCount
                + ",BufferSize: " + BufferSize
                + ",WindowSize: " + WindowSize
                ;
        }
    }



    /// <summary>
    /// 在窗口的位置
    /// </summary>
    public enum PositionOfMovingWindow
    {
        /// <summary>
        /// 在前置外面
        /// </summary>
        FormerOutside = 0,
        /// <summary>
        /// 前面缓冲边界中
        /// </summary>
        FormerMargin = 1,
        /// <summary>
        /// 前面重叠
        /// </summary>
        FormerOverlap = 3,
        /// <summary>
        /// 在内部
        /// </summary>
        Inside = 4,
        /// <summary>
        /// 后面重叠
        /// </summary>
        LatterOverlap = 5,
        /// <summary>
        /// 后面缓冲边界中
        /// </summary>
        LatterMargin = 6,
        /// <summary>
        /// 在后置外面
        /// </summary>
        LatterOutside = 7,
    }
}