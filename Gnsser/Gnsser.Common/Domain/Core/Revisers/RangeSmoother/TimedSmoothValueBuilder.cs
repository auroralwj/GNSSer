//2018.05.20, czs, create in HMX,  具有时间索引的数据多项式平滑器

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
    /// 数据多项式平滑管理器
    /// </summary>
    public class TimedSmoothValueBuilderManager : BaseDictionary<string, TimedSmoothValueBuilder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TimedSmoothValueBuilderManager(int windowSize)
        {
            this.WindowSize = windowSize;
        }
        /// <summary>
        /// 最大窗口大小，单位：历元 次
        /// </summary>
        public int WindowSize { get; set; }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override TimedSmoothValueBuilder Create(string key)
        {
            return new TimedSmoothValueBuilder(WindowSize, key );
        }
    }

    /// <summary>
    /// 数据多项式平滑器。
    /// </summary>
    public class TimedSmoothValueBuilder : AbstractBuilder<double>, Geo.Namable
    {
        Log log = new Log(typeof(TimedSmoothValueBuilder));

        /// <summary>
        ///  数据多项式平滑器
        /// </summary>
        /// <param name="maxEpochCount"></param>
        /// <param name="name"></param>
        public TimedSmoothValueBuilder(int maxEpochCount, string name)
        { 
            this.Name = name;
            NumeralWindowData = new TimeNumeralWindowData(maxEpochCount, 5 * 60);
            this.Order = 2;
        }

        #region  属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 阶次
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 多项式拟合伪距方法。
        /// </summary>
        private TimeNumeralWindowData NumeralWindowData { get; set; } 
         /// <summary>
         /// 当前时间
         /// </summary>
        public Time CurrentTime { get; set; }
        /// <summary>
        /// 当前原始数据
        /// </summary>
        public double CurrentRaw { get; set; }
        /// <summary>
        /// 平滑时刻，必须设置
        /// </summary>
        public Time SmoothTime { get; set; }
        #endregion

        #region  设值 

        /// <summary>
        /// 设置原始原始伪距,载波伪距
        /// </summary>
        /// <param name="time"></param> 
        /// <param name="rawRange"></param> 
        /// <returns></returns>
        public TimedSmoothValueBuilder SetRawValue(Time time, double rawRange)
        {
            //通过是否相等，判断是否重复设值
            if (CurrentTime  == time)
            {
                return this;
            }

            this.NumeralWindowData.Add(time, rawRange); 
            this.CurrentTime = time;
            this.CurrentRaw = rawRange;
            return this;
        }
        /// <summary>
        /// 是否重置，如果发生周跳。
        /// </summary>
        /// <param name="IsReset"></param>
        /// <returns></returns>
        public TimedSmoothValueBuilder SetReset(bool IsReset)
        {
            if (IsReset)
            { 
                this.NumeralWindowData.Clear();
            }
            return this;
        }
        /// <summary>
        /// 平滑时刻，必须设置。
        /// </summary>
        /// <param name="SmoothTime"></param>
        /// <returns></returns>
        public TimedSmoothValueBuilder SetSmoothTime(Time SmoothTime)
        {
            this.SmoothTime = SmoothTime;
            return this;
        }

        #endregion

        /// <summary>
        /// 采用窗口进行平滑
        /// </summary>
        /// <returns></returns>
        public double GetSmoothedRange()
        {
            if (NumeralWindowData.Count == 0)
            {
                throw new Exception("are you kidding? you must put one value first.");
            }

            if (NumeralWindowData.Count < this.Order + 1)
            {
                return this.CurrentRaw;
            }

            var rmsVal = NumeralWindowData.GetPolyFitValue(SmoothTime, Order);
            var differ = rmsVal.Value - this.CurrentRaw;

            if(rmsVal.Rms > 10)
            {
                return CurrentRaw;
            }
            return rmsVal.Value;
        }

        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public override double Build()
        { 
            return GetSmoothedRange(); 
        } 

        /// <summary>
        /// 重设。
        /// </summary>
        public void Reset()
        {
            NumeralWindowData.Clear();
        }
    }
}