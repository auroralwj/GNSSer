//2016.05.10, czs, edit， 滑动窗口数据
//2016.08.04, czs, edit in fujian yongan, 修正

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Times;
using Geo.IO;
using Geo;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Common;

namespace Geo
{
    

    /// <summary>
    /// 误差判断器,误差大法官.
    /// 通过给定的初值，判断下一个值是否为误差，并根据缓存数据判断是否为跳变。
    /// </summary>
    public class ErrorJudge
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="MaxError"></param>
        /// <param name="IsRelativeError"></param>
        public ErrorJudge(double MaxError, bool IsRelativeError)
        {
            this.MaxError = MaxError;
            this.IsRelativeError = IsRelativeError;
        }

        #region 属性
        /// <summary>
        /// 当前值
        /// </summary>
        public double ReferenceValue { get; private set; }
        /// <summary>
        /// 允许的最大误差（含）
        /// </summary>
        public double MaxError { get; set; }
        /// <summary>
        /// 是否是相对误差
        /// </summary>
        public bool IsRelativeError { get; set; }
        /// <summary>
        /// 是否第一次计算，用于判断赋予初值。
        /// </summary>
        public bool IsFirst { get { return this.Index == 0; } }
        /// <summary>
        /// 计算次数
        /// </summary>
        public int Index { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 当前值。
        /// </summary>
        /// <param name="val"></param>
        public void SetReferenceValue(double val)
        {
            this.ReferenceValue = val;
            Index++; 
        }

        /// <summary>
        /// 判断是否是跳变。通过后续缓存窗口数据判断。
        /// 请先调用 IsOverTolerance 确定超限后，再调用我。
        /// 这里假定粗差只出现少数几次或1次。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool IsJumped(IEnumerable<double> buffer)
        {
            //算法：如果缓存中下面几个全超限，则认为不是粗差，是跳变。           
            var nextFive = new NumeralWindowData(buffer);
            return IsOverLimit(nextFive.AverageValue);
        }
        /// <summary>
        /// 判断是否是跳变。通过后续缓存窗口数据判断。
        /// 请先调用 IsOverTolerance 确定超限后，再调用我。
        /// 这里假定粗差只出现少数几次或1次。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="referValue"></param>
        /// <returns></returns>
        public bool IsJumped(IEnumerable<double> buffer, double referValue)
        {
            if(buffer == null) { return false; }

            //算法：如果缓存中下面几个全超限，则认为不是粗差，是跳变。           
            var nextFive = new NumeralWindowData(buffer);
            return IsOverLimit(nextFive.AverageValue, referValue);
        }

        /// <summary>
        /// 检核是否超限
        /// </summary>
        /// <param name="newVal"></param>
        /// <returns></returns>
        public virtual bool IsOverLimit(double newVal)
        {
            double delta = Math.Abs(ReferenceValue - newVal);
            if (IsRelativeError) { delta /= Math.Abs(ReferenceValue); }
            if (delta > MaxError) { return true; }//超限啦！
            return false;
        }
        /// <summary>
        /// 检核是否超限.无记忆效应，可以多次重复使用
        /// </summary>
        /// <param name="newVal"></param>
        /// <param name="referValue"></param>
        /// <returns></returns>
        public virtual bool IsOverLimit(double newVal, double referValue)
        {
            double delta = Math.Abs(referValue - newVal);
            if (IsRelativeError) { delta /= Math.Abs(referValue); }
            if (delta > MaxError) { return true; }//超限啦！
            return false;
        }
        #endregion
    }


}