//2017.08.12, czs, edit in hongqing, 提取参数，使得可以设置
//2017.10.24, czs, edit in hongqing, 修正时间赋值函数，避免重复更新,将电离层、对流层、系统时间偏差随机游走模型合并到一起，只是参数不同。


using System;
using Gnsser.Times;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times; 
using Gnsser.Data.Rinex;

namespace Gnsser.Models
{
    /// <summary>
    /// 通用测站随机游走模型。
    /// 通过时间和初始方差确定转移方差。
    /// </summary>
    public class RandomWalkModel : BaseStateTransferModel
    { 
        /// <summary>
        /// 通用随机游走模型
        /// </summary>
        /// <param name="stdDev"></param>
        public RandomWalkModel(double stdDev = 3e-2)
        {
            BaseVariance = stdDev * stdDev;
            PreviousTime =  Time.StartOfGpsT;
            CurrentTime = Time.MaxValue;
        }

        /// <summary>
        /// 单位间隔方差量，如 1 秒。
        /// </summary>
        public double BaseVariance { get; set; }
        

        /// <summary>
        /// 方差值。
        /// </summary>
        /// <returns></returns>
        public override double GetNoiceVariance()
        { 
            double variance = Math.Abs(CurrentTime - PreviousTime) * (BaseVariance);
            return variance;
        }

        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="epochSate"></param>
        public override void Init(EpochInformation epochSate)
        {
            Update(epochSate.ReceiverTime);
        }
        

        /// <summary>
        /// 时间更新。
        /// </summary>
        /// <param name="time"></param>
        public void Update(Time time)
        {
            //Upadat previous epoch
            if (!CurrentTime.Equals(time))
            {
                this.PreviousTime = CurrentTime;
                this.CurrentTime = time;
            }
        }
    }
}
