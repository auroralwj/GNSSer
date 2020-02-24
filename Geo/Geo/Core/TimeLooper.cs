//2017.07.21, czs, create in hongiqng, DOP计算
//2017.10.09, czs, edit in hongqing, 改进，采用实际数据计算


using System;
using System.Collections.Generic;
using System.Text; 
using Geo.Coordinates; 
using System.Linq; 
using  Geo.Algorithm.Adjust;
using System.Threading;
using System.Threading.Tasks;
using Geo;
using Geo.IO;
using Geo.Algorithm; 
using Geo.Data;
using Geo.Times;
using Geo.Utils;

namespace Geo
{
    /// <summary>
    /// 时间遍历器
    /// </summary>
    public class TimeLooper : AbstractProcess, ICancelAbale, IProgressNotifier
    {
        /// <summary>
        /// 遍历事件
        /// </summary>
        public event Action<Time> Looping;
        /// <summary>
        /// 构造has
        /// </summary>
        /// <param name="TimePeriod"></param>
        /// <param name="StepInSeconds"></param>
        public TimeLooper(TimePeriod TimePeriod, double StepInSeconds)
        {
            this.TimePeriod = TimePeriod;
            this.StepInSeconds = StepInSeconds;
        }
      

        /// <summary>
        /// 时段
        /// </summary>
        public TimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 步长，秒
        /// </summary>
        public double StepInSeconds { get; set; }
        /// <summary>
        /// 是否取消计算
        /// </summary>
        public bool IsCancel { get; set; }
        /// <summary>
        /// 循环
        /// </summary>
        public override void Run()
        {
            int steps = (int)Math.Round(TimePeriod.TimeSpan.TotalSeconds / StepInSeconds);
            InitProcess(steps);

            for (Time time = TimePeriod.Start; time <= TimePeriod.End; time = time + StepInSeconds)
            {
                if (IsCancel) { break; }
                OnLooping(time);
            }

            FullProcess();
        }
        /// <summary>
        /// 并行循环
        /// </summary>
        public void LoopAsync(int MaxDegreeOfParallelism = 4)
        {
            int steps = (int)Math.Round(TimePeriod.TimeSpan.TotalSeconds / StepInSeconds);
            InitProcess(steps);

            List<Time> times = new List<Time>();

            for (Time time = TimePeriod.Start; time <= TimePeriod.End; time = time + StepInSeconds)
            {
                times.Add(time);
            }
            Parallel.ForEach(times, new ParallelOptions()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism
            },
            new Action<Time, ParallelLoopState>(delegate(Time time, ParallelLoopState state)
            {
                if (IsCancel) { state.Break(); }
                OnLooping(time);
            }));

            FullProcess();
        }
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="Time"></param>
        protected void OnLooping(Time Time)
        {
            if (Looping != null) { Looping(Time); }
            PerformProcessStep();
        }
    }
}
