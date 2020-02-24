
//2014.12.30, czs, create, 相位距离观测量，Gnsser核心模型！
//2018.08.15, czs, edit in hmx, 可以加上相位值

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Correction;
using Gnsser.Correction;
using Geo;
using Gnsser.Data.Rinex;

namespace Gnsser.Domain
{

    /// <summary>
    /// 相位距离观测量。
    /// //czs, 2015.05.14, 本类是否该继承自 相位的 ObsValue ????有待考虑。
    /// </summary>
    public class PhaseRangeObservation : Observation, IValueClone<PhaseRangeObservation>
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="phaseValue">相位观测值</param>
        /// <param name="Frequence">频率</param> 
        public PhaseRangeObservation(RinexObsValue phaseValue, Frequence Frequence)
            : base(phaseValue.Value * Frequence.WaveLength, phaseValue.ObservationCode)
        {
            this.Frequence = Frequence;
            this.RawPhaseValue = phaseValue.Value;
            this.LossLockIndicator = phaseValue.LossLockIndicator;
            this.SignalStrength = phaseValue.SignalStrength;
        }
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="rawPhaseValue">相位观测值</param>
        /// <param name="Frequence">频率</param> 
        /// <param name="ObservationCode"></param> 
        /// <param name="LossLockIndicator"></param> 
        /// <param name="SignalStrength"></param> 
        public PhaseRangeObservation(double rawPhaseValue, ObservationCode ObservationCode, Frequence Frequence, int SignalStrength =50, LossLockIndicator LossLockIndicator = LossLockIndicator.OK)
            : base(rawPhaseValue * Frequence.WaveLength, ObservationCode)
        {
            this.Frequence = Frequence;
            this.RawPhaseValue = rawPhaseValue;
            this.LossLockIndicator = LossLockIndicator;
            this.SignalStrength = SignalStrength;
        }


        /// <summary>
        /// 信号失锁标记。此数据不应该赋值，只应该由观测数据决定
        /// </summary>
        public LossLockIndicator LossLockIndicator { get; private set; }
        /// <summary>
        /// 模糊度。接收时刻的相位等于发射时刻的相位（Remondi，1984；Leick，1995）。
        /// 那么中间就差个整周数。？？？
        /// 或者直接放在改正数里面？？？？
        /// </summary>
        public long OffsetCycle { get; set; }

        /// <summary>
        /// 等效的距离， 为 偏差距离改正数 + 改正后的距离。
        /// </summary>
        public double AlinedCorrectedRange { get { return (OffsetCycle) * this.Frequence.WaveLength + this.CorrectedValue; } }

        /// <summary>
        /// 频率
        /// </summary>
        public Frequence Frequence { get; set; }
        /// <summary>
        /// 信号强度
        /// </summary>
        public int SignalStrength { get; set; }

        /// <summary>
        /// 原始相位观测值,单位：周.
        /// </summary>
        public double RawPhaseValue { get; set; }
        /// <summary>
        /// 设置更新相位值
        /// </summary>
        /// <param name="rawVal"></param>
        public void SetRawValue(double rawVal)
        {
            this.RawPhaseValue = rawVal;
            this.Value = rawVal * Frequence.WaveLength;
        }
        /// <summary>
        ///加上一个相位距离，单位：m
        /// </summary>
        /// <param name="range"></param>
        public void AddPhaseRange(double range)
        {
            this.RawPhaseValue += range / Frequence.WaveLength; ;
            this.Value += range;
        }
        /// <summary>
        ///加上一个相位，单位：周
        /// </summary>
        /// <param name="cycle"></param>
        public void AddPhaseCycle(double cycle)
        {
            this.RawPhaseValue += cycle;
            this.Value += cycle * Frequence.WaveLength;
        }

        PhaseRangeObservation IValueClone<PhaseRangeObservation>.ValueClone()
        {
            return new PhaseRangeObservation(this.RawPhaseValue, this.ObservationCode, this.Frequence, this.SignalStrength, this.LossLockIndicator);
        }

        public override Observation ValueClone()
        {
            
            return new PhaseRangeObservation(this.RawPhaseValue, this.ObservationCode, this.Frequence, this.SignalStrength, this.LossLockIndicator);
        }

    }
}