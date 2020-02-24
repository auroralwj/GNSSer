//2016.05.08, czs, create in hongqing, 载波相位平滑伪距
//2016.05.10, czs, edit in hongqing, 采用窗口平滑伪距
//2018.05.20, czs, edit in HMX, 采用基于缓存的窗口算法，改进电离层延迟
//2018.06.06, czs, edit in HMX, 增加电离层改正，小小的突破！！
//2018.06.15, czs, edit in HMX, 重新编写了一次平滑伪距
//2018.06.21, czs, edit in HMX, 单独提出平滑伪距历元数据，增加电离层拟合存储

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;
using Geo.Times;
using Geo.Algorithm;

namespace Gnsser
{ 
    /// <summary>
    /// 平滑伪距历元数据
    /// </summary>
    public class RangePhasePair : NumerialPair
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="range">伪距</param>
        /// <param name="phaseRange">相位距离</param>
        /// <param name="ionoDiffer">电离层变化</param>
        public RangePhasePair(double range, double phaseRange, double ionoDiffer) :base(range, phaseRange)
        {
            this.InputtedIonoDiffer = ionoDiffer;
            //初始值
            this.FittedIonoAndAmbiValue = ionoDiffer;// GetRawIonoAndHalfAmbiValue();
        }
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 当前伪距
        /// </summary>
        public double PseudoRange { get =>this.First; set=>this.First = value; }

        /// <summary>
        /// 当前的相位的距离。
        /// </summary>
        public double PhaseRange { get => this.Second; set => this.Second = value; }
        /// <summary>
        /// 加上了2倍电离层和模糊度距离的
        /// </summary>
        public double IonoFittedPhaseRange { get { return PhaseRange + 2 * FittedIonoAndAmbiValue; } }
   
        /// <summary>
        /// 拟合的电离层和模糊度数值
        /// </summary>
        public double FittedIonoAndAmbiValue { get; set; }

        /// <summary>
        /// 伪距减去载波
        /// </summary>
        public double RangeMinusPhase { get { return PseudoRange - PhaseRange; } }
        /// <summary>
        /// 伪距减去带电离层和模糊度改正的的载波
        /// </summary>
        public double RangeMinusIonoFittedPhase { get { return PseudoRange - IonoFittedPhaseRange; } }
        /// <summary>
        /// 获取原始的电离层加一半模糊度数值。
        /// </summary>
        /// <returns></returns>
        public double GetRawIonoAndHalfAmbiValue()
        {
            return RangeMinusPhase / 2;
        }

        /// <summary>
        /// 电离层延迟，基于某一历元的变化
        /// </summary>
        public double InputtedIonoDiffer { get; set; }
        ///// <summary>
        ///// 改正数的相反数
        ///// </summary>
        //public double Bias { get => - this.Correction; set => this.Correction = -value; }
        /// <summary>
        /// 默认构造函数的初值通常为 0.
        /// </summary>
        public bool IsZero
        {
            get => First == 0 && Second == 0;
        }
    } 
}