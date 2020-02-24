//2016.03.27, czs, edit & create in hongiqng, 平滑数据
//2016.04.10, czs, edit in hongqing, 重构

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;

namespace Geo
{
    /// <summary>
    /// 时间超过了最大允许的间隙
    /// </summary>
    public delegate void SmoothTimeExceededEventHandler(SmoothTimeValue SmoothValue);
    
    /// <summary>
    /// 平滑数据
    ///  a class used to store filter data for a SV.
    /// </summary>
    public class SmoothTimeValue : SmoothValue
    {
        
        /// <summary>
        /// 平滑数据改变
        /// </summary>
        public event SmoothTimeExceededEventHandler TimeExceeded;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxTimeDiffer"></param>
        /// <param name="maxValueDiffer"></param>
        /// <param name="isResetWhenExceeded"></param>
        /// <param name="isPercentMaxDifferValue"></param>
        public SmoothTimeValue(double maxTimeDiffer = 121, double maxValueDiffer = 90, bool isResetWhenExceeded = true, bool isPercentMaxDifferValue = false)
            : base(maxValueDiffer, isResetWhenExceeded, isPercentMaxDifferValue)
        {
            this.MaxAllowedTimeDiffer = maxTimeDiffer;
            this.LastTime = Time.Default;
        }

        #region 属性
        #region 核心属性
        /// <summary>
        /// 最后一个时间。
        /// </summary>
        public Time LastTime { get; set; }
        /// <summary>
        /// 最后两历元时间差，秒,返回为绝对值
        /// </summary>
        public double LastTimeDiffer { get; set; }
        /// <summary>
        /// 允许最大的时间差，时间秒.
        /// 需要构造时赋值，或在Add前赋值。
        /// </summary>
        public double MaxAllowedTimeDiffer { get; set; }
        #endregion

        /// <summary>
        /// 当前时间是否超出了最大允许的时间差。每次获取时计算。
        /// </summary>
        public bool IsTimeExceeded { get { return LastTimeDiffer > MaxAllowedTimeDiffer; } }
        #endregion

        /// <summary>
        /// 由于时间超限，平滑数据中断。
        /// </summary>
        /// <param name="val"></param>
        protected void OnTimeExceeded(SmoothTimeValue val)
        {
            //log.Debug("Smooth Time Exceeded:" + this.ToString()); 
            if (TimeExceeded != null) { TimeExceeded(val); }
        }

        /// <summary>
        /// 更新时间和数值,数值许可，则返回true，数值超限则重置并返回false。
        /// </summary>
        /// <param name="time"></param>
        /// <param name="newVal"></param>
        public bool Regist(Time time, double newVal)
        {
            var isTimeFitable = Regist(time);
            var isValueFitable = Regist(newVal);
            return isTimeFitable && isValueFitable;
        }

        /// <summary>
        /// 更新时间,如果超限，则返回false，如果正常则返回true。
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Regist(Time time)
        {
            //1、设置时间
            Set(time);

            //2、判断和激发事件
            var isTimeExceeded = IsTimeExceeded;

            if (this.CurrentWindowSize != 0 && isTimeExceeded)
            {
                ResetWindowSize();
                OnTimeExceeded(this);
            }

            return !isTimeExceeded;
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="time"></param>
        private void Set(Time time)
        {
            this.LastTimeDiffer = Math.Abs(time - LastTime);
            this.LastTime = time; 
        }


        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ", Time: " + LastTime.ToShortTimeString()
                + ", Value: " + Value.ToString("0.000") 
                + ", WindowSize: " + CurrentWindowSize
                + ", LastTimeDiffer : " + LastTimeDiffer.ToString("0")
                + ", LastValueDiffer: " + LastValueDiffer.ToString("0.000");
        }
    }
}
