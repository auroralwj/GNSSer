//2016.03.27, czs, edit & create in hongiqng, 平滑数据
//2016.05.10, czs, edit， 增加滑动窗口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo.IO;
using Geo.Common;

namespace Geo
{
    /// <summary>
    /// 平滑数据发生了中断。
    /// </summary>
    public delegate void SmoothValueEventHandler(SmoothValue SmoothValue);

    /// <summary>
    /// 平滑数据.一定数量的平均值。
    /// 规则：
    /// 如果第二个数据超限，则采用第二个数据。
    /// </summary>
    public class SmoothValue : NumeralValue, Namable
    {
        protected Log log = new Log(typeof(SmoothValue));
        /// <summary>
        /// 平滑数据超限制，重新计数
        /// </summary>
        public event SmoothValueEventHandler ValueReseted;
        /// <summary>
        /// 数据超出了。但不一定中断。
        /// </summary>
        public event SmoothValueEventHandler ValueExceeded;
         /// <summary>
        /// 默认构造函数，具有超限判断重置能力
         /// </summary>
         /// <param name="maxValueDiffer"></param>
         /// <param name="isResetWhenExceeded"></param>
         /// <param name="isPercentMaxDifferValue"></param>
         /// <param name="IsWindowSizeFixed"></param>
        /// <param name="IndicatedWindowSize"></param>
        public SmoothValue(double maxValueDiffer, bool isResetWhenExceeded, bool isPercentMaxDifferValue = false, bool IsWindowSizeFixed = false, int IndicatedWindowSize = 10)
        {
            this.MaxAllowedValueDiffer = maxValueDiffer;
            this.IsResetWhenExceeded = isResetWhenExceeded;
            this.IsPercentMaxDifferValue = isPercentMaxDifferValue;

            this.IsWindowSizeFixed = IsWindowSizeFixed;
            this.IndicatedWindowSize = IndicatedWindowSize;
            Values = new List<double>();
        }
        /// <summary>
        /// 专门针对平滑数值，认为数据已经经过判断，比较良好。
        /// </summary>
        /// <param name="IndicatedWindowSize"></param>
        public SmoothValue(int IndicatedWindowSize = 10)
        {
            this.MaxAllowedValueDiffer = Int16.MaxValue ;
            this.IsResetWhenExceeded = false;
            this.IsPercentMaxDifferValue = false;

            this.IsWindowSizeFixed = true;
            this.IndicatedWindowSize = IndicatedWindowSize;
            Values = new List<double>();
        }



        #region 属性
        #region 滑动窗口属性
        /// <summary>
        /// 是否固定窗口大小。如滑动窗口。
        /// </summary>
        public bool IsWindowSizeFixed { get; set; }
        /// <summary>
        /// 数值，在滑动窗口时使用。
        /// </summary>
        public List<double> Values { get; set; }
        /// <summary>
        /// 指定的窗口大小。
        /// </summary>
        public int IndicatedWindowSize { get; set; }
        #endregion

        /// <summary>
        /// 名称，如卫星编号。
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 最大偏差值，是否实时百分比值（相对误差），如0.5表示50%，对于相位，以周为单位，适用。
        /// </summary>
        public bool IsPercentMaxDifferValue { get; set; }
        /// <summary>
        /// 允许最大的数值差
        /// </summary>
        public double MaxAllowedValueDiffer { get; set; }
        /// <summary>
        /// 最后一个原始值。
        /// </summary>
        public double LastValue { get; protected set; }

        /// <summary>
        /// 当前超限时，是否继续统计ture，或者重新设置统计窗口false.
        /// </summary>
        public bool IsResetWhenExceeded { get; set; }

        /// <summary>
        /// 最后两历元数值差绝对值，如果指定采用百分比形式，则返回分数数值。
        /// </summary>
        public double LastValueDiffer { get { return GetValueDiffer(LastValue); } }
        /// <summary>
        /// 获取一个数值与当前平滑值的偏差，用以判断是否超限。
        /// 若是第一个值，则返回本身。
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private double GetValueDiffer(double newValue)
        {
            if (this.CurrentWindowSize == 0) return Math.Abs(newValue);

            var lastValueDiffer = Math.Abs(newValue - this.Value);
            //相对值
            if (IsPercentMaxDifferValue) { lastValueDiffer = Math.Abs(lastValueDiffer / this.Value); }
            return lastValueDiffer;
        }
        /// <summary>
        /// 平滑值。
        /// </summary>
        public override double Value { get { return base.Value; } set { base.Value = value; } }

        int _currentWindowSize = 0;
        /// <summary>
        /// 当前窗口大小,有一个数据是一个，如果没有重置，窗口逐渐递增。
        /// aboutSize of current window, in samples.
        /// </summary>
        public int CurrentWindowSize
        {
            get
            {
                if (IsWindowSizeFixed) { return this.Values.Count; }
                else return _currentWindowSize;
            }
            private set { _currentWindowSize = value; }
        }
        /// <summary>
        /// 数值是否复合限差，每次获取时都计算一次。
        /// </summary>
        public bool IsValueAdaptable(double val) { return (GetValueDiffer(val) <= MaxAllowedValueDiffer); } 
        /// <summary>
        /// 平滑值的中误差,为正数。如果没有数据，则返回 0。
        /// </summary>
        public double StdOrRms { get { if (this.CurrentWindowSize == 0) return 0; return Math.Sqrt(Math.Pow((LastValue - Value), 2) / this.CurrentWindowSize); } }

        #endregion

        /// <summary>
        /// 新增一个数据，重新计算平滑值。如果超限，则返回false，如果正常则返回true
        /// </summary>
        /// <param name="newVal"></param>
        public bool Regist(double newVal)
        {
            //首先判断新数值是否超限
            var isOk = IsValueAdaptable(newVal);

            if (!isOk)
            {
                OnValueExceeded(this); 
                
                if (IsResetWhenExceeded)
                {
                    this.ResetWindowSize();//重置窗口，并代入新值，重新计算 
                }
                else
                {
                    return isOk; //忽略本次结果
                } 
            }

            //认为取得的值是可用值
            AddAndCaculateSmoothValue(newVal);

            return isOk;
        }

        #region 处理细节

        /// <summary>
        /// 添加一个新值，并计算平滑值。通常在质量检核之后调用。
        /// </summary>
        /// <param name="newVal"></param>
        protected void AddAndCaculateSmoothValue(double newVal)
        {
            //1、设置新数值,并计算新数值
            SetNewValue(newVal);

            UpdateSmoothValue();
        } 

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="newVal"></param>
        private void SetNewValue(double newVal)
        {
            this.LastValue = newVal; 
            //如果窗口固定
            if (IsWindowSizeFixed)
            {
                //不管那么多，添加再说
                Values.Add(newVal);
                //判断数量,多则删除
                while (this.CurrentWindowSize > IndicatedWindowSize) { Values.RemoveAt(0); }
            }
            else
            {
                CurrentWindowSize++; //窗口增加一个。
            }
        }

        /// <summary>
        /// 更新并平滑数据，增加窗口。
        /// </summary>
        public virtual void UpdateSmoothValue()
        {
            //如果窗口固定
            if (IsWindowSizeFixed) { this.Value =  CaculateSmoothValue(Values); }
            else { this.Value = CaculateSmoothValue(LastValue, Value, CurrentWindowSize); }
        }

        public virtual double CaculateSmoothValue(double newVal, double oldAverageValue, int currentValCount)
        {
            return Geo.Utils.DoubleUtil.GetAverageValue(newVal, oldAverageValue, currentValCount);
        }

        public virtual double CaculateSmoothValue(List<double> values)
        {
            return  Geo.Utils.DoubleUtil.Average(values);
        }
        
        #endregion

        /// <summary>
        /// 重置，窗口重置为0.重新开始统计和计算。
        /// </summary>
        public virtual void ResetWindowSize()
        {
            this.CurrentWindowSize = 0;
            this.Values.Clear();

            this.OnValueReseted(this);
        }

        /// <summary>
        /// 平滑数据终端，重新统计。
        /// </summary>
        /// <param name="val"></param>
        protected void OnValueReseted(SmoothValue val)
        {
            //log.Debug("Smooth Value Reseted:" + this.ToString());
            if (ValueReseted != null) ValueReseted(val);
        }
        /// <summary>
        /// 数据超出限制了。
        /// </summary>
        /// <param name="val"></param>
        protected void OnValueExceeded(SmoothValue val)
        {
            //log.Debug("Smooth Value Exceeded:" + this.ToString());
            if (ValueExceeded != null) ValueExceeded(val);
        }

        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ", Value: " + this.Value.ToString("0.000")
                + ", WindowSize: " + CurrentWindowSize
                + ", LastValue: " + LastValue.ToString("0.000")
                + ", LastValueDiffer: " + LastValueDiffer.ToString("0.000")
                //  + ", MaxAllowedValueDiffer: " + MaxAllowedValueDiffer.ToString("0.000")
                ;
        }
    }
}