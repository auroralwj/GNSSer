//2018.07.06,, czs, create in HMX, 具有时间的变化率求解



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Times;

namespace Geo
{

    /// <summary>
    /// 具有时间的变化率求解
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class TimedEpochChangeRateSolverManager<TKey> : BaseDictionary<TKey, TimedEpochChangeRateSolver>
    {
        /// <summary>
        /// 具有时间的变化率求解
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="maxGapCount"></param>
        public TimedEpochChangeRateSolverManager(double interval, int maxGapCount = 5)
        {
            this.Interval = interval;
            this.maxGapCount = maxGapCount; 

        }
        /// <summary>
        /// 采样间隔。
        /// </summary>
        public double Interval { get; set; }
        /// <summary>
        /// 最大允许断裂的时间段，秒
        /// </summary>
        public int maxGapCount { get; set; }

        public override TimedEpochChangeRateSolver Create(TKey key)
        {
            return new TimedEpochChangeRateSolver(Interval, maxGapCount);
        }
    }




    /// <summary>
    /// 具有时间的变化率求解，如,站星电离层历元间变化求解
    /// </summary>
    public class TimedEpochChangeRateSolver
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="maxGapCount"></param>
        public TimedEpochChangeRateSolver(double interval, int maxGapCount = 5)
        {
            this.Interval = interval;
            this.MaxTimeSpan = maxGapCount * interval;
            this.PrevTime = Time.MinValue;
        }
        #region 属性
        /// <summary>
        /// 采样间隔。
        /// </summary>
        public double Interval { get; set; }
        /// <summary>
        /// 最大允许断裂的时间段，秒
        /// </summary>
        public double MaxTimeSpan { get; set; }
        /// <summary>
        /// 上一时间
        /// </summary>
        public Time PrevTime { get; set; }
        /// <summary>
        /// 上一实测数据
        /// </summary>
        public double PrevValue { get; set; }
        /// <summary>
        /// 是否重置
        /// </summary>
        /// <param name="trueOrFalse"></param>
        /// <returns></returns>
        public TimedEpochChangeRateSolver SetIsReset(bool trueOrFalse)
        {
            if (trueOrFalse)
            {
                this.PrevTime = Time.MinValue;
            }
            return this;
        }

        #endregion

        /// <summary>
        /// 计算变化率
        /// </summary>
        /// <param name="epoch"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public double GetChangeRate(Time epoch, double val)
        {
            double timeDiffer = epoch - PrevTime;
            if (Math.Abs(timeDiffer) > MaxTimeSpan)
            {
                this.PrevTime = epoch;
                this.PrevValue = val;
                return 0;
            }

            double valDiffer = val - PrevValue;
            double rate = valDiffer / timeDiffer;


            this.PrevTime = epoch;
            this.PrevValue = val;

            return rate;
        }

    }

}
