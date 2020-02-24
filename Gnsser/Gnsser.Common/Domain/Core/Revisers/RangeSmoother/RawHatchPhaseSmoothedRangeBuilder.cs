//2016.05.08, czs, create in hongqing, 载波相位平滑伪距 PhaseSmoothedRangeBuilder
//2017.11.10, czs, create in hongqing, 载波相位平滑伪距，Hatch 递推滤波模型 HatchPhaseSmoothedRangeBuilder
//2018.05.20, czs, eidt in HMX， 采用缓存，改进载波平滑伪距算法,合并 PhaseSmoothedRangeBuilder 和 HatchPhaseSmoothedRangeBuilder
//2018.06.18, czs, edit in HMX, 重构，区分原始和改进平滑
//2018.06.21, czs, edit in HMX, 增加电离层延迟改正
//2018.06.25, czs, edit in HMX, 剥离，提取电离层改正到上层类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;
using Geo.Times;

namespace Gnsser
{
    /// <summary>
    /// 原始 Hatch 递推滤波模型，不考虑窗口大小，载波相位平滑伪距。
    /// </summary>
    public class RawHatchPhaseSmoothedRangeBuilder : AbstractSmoothRangeBuilder
    {
        Log log = new Log(typeof(RawHatchPhaseSmoothedRangeBuilder));

        /// <summary>
        /// Hatch 递推滤波模型，载波相位平滑伪距。
        /// </summary>
        /// <param name="maxEpochCount"></param>
        /// <param name="IsWeighted"></param>
        /// <param name="name"></param>
        /// <param name="IsDeltaIonoCorrect"></param>
        /// <param name="IonoFitOrder"></param>
        /// <param name="bufferSize"></param>
        public RawHatchPhaseSmoothedRangeBuilder(int maxEpochCount, bool IsWeighted, string name, IonoDifferCorrectionType IsDeltaIonoCorrect, int IonoFitOrder, int bufferSize, int IonoFitEpochCount)
            : base(IsWeighted, IsDeltaIonoCorrect, IonoFitOrder, bufferSize, IonoFitEpochCount)
        {
            this.MaxCount = maxEpochCount;//s
            this.IsWeighted = IsWeighted;
            this.Name = name;
            this.RawRangeStdDev = 2;
            this.CurrentRaw = new RangePhasePair(0, 0, 0);
            this.EpochCount = 1; 
        }

        #region  属性
        /// <summary>
        /// 指定的电离层变化率文件路径
        /// </summary>
        public string IndicatedIonoDeltaFilePath { get; set; }

        /// <summary>
        /// 当前历元数量
        /// </summary>
        public int EpochCount { get; set; }
        /// <summary>
        /// 平滑常数
        /// </summary>
        public double MaxCount { get; set; }

        #endregion

        #region  设值 



        /// <summary>
        /// 是否重置，如果发生周跳。
        /// </summary>
        /// <param name="IsReset"></param>
        /// <returns></returns>
        public override AbstractSmoothRangeBuilder SetReset(bool IsReset)
        {
            if (IsReset)
            {
                this.Reset(); 
            }
            return this;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public override RmsedNumeral Build()
        {

            double stdDev = RawRangeStdDev;
            if (EpochCount <= 1)
            {
                this.SmoothRange = CurrentRaw.PseudoRange;
            }
            else
            {
                double WeightOfRawRange = GetWeight();
                double deltaIono = 0;
                if (this.IonoDifferCorrectionType != IonoDifferCorrectionType.No)
                {
                    deltaIono = CurrentRaw.FittedIonoAndAmbiValue - PrevData.FittedIonoAndAmbiValue;
                }
                double phaseDiffer = CurrentRaw.PhaseRange - PrevData.PhaseRange;

                this.ExtraRange = PrevSmoothRange + phaseDiffer + 2 * deltaIono;//外推伪距
                if (IsWeighted)
                {
                    this.SmoothRange = WeightOfRawRange * CurrentRaw.PseudoRange + (1.0 - WeightOfRawRange) * ExtraRange;
                }
                else
                {
                    this.SmoothRange = ExtraRange;
                }
                stdDev = RawRangeStdDev * WeightOfRawRange;
            }
            EpochCount++;

            return new RmsedNumeral(this.SmoothRange, stdDev);
        }

        /// <summary>
        /// 获取原始伪距的权值，如果数据少于制定大小，则采用本身，否则采用指定的权值。
        /// </summary>
        /// <returns></returns>
        private double GetWeight()
        {
            double WeightOfRawRange = 1;
            if (EpochCount >= MaxCount)
            {
                WeightOfRawRange = 1.0 / MaxCount;
            }
            else
            {
                WeightOfRawRange = 1.0 / EpochCount;
            }

            return WeightOfRawRange;
        }


        /// <summary>
        /// 重设。
        /// </summary>
        public override void Reset()
        {
            base.Reset(); 
            EpochCount = 1;
        }
        #endregion
    }
}