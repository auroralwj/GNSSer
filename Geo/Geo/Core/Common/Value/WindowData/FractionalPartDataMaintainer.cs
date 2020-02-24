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
    /// 设计：
    ///1.数据进入后，要判断是否超限，
    ///如果是，则判断是跳跃还是粗差。
    ///如果是跳跃，则多读取几个数据重新计算其偏差；
    ///如果是粗差，则简单的剔除之。
    /// </summary>
    public class FractionalDataMaintainerManager : BaseDictionary<string, FractionalPartDataMaintainer>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MaxError"></param>
        /// <param name="IsRelativeError"></param>
        public FractionalDataMaintainerManager(double MaxError, bool IsRelativeError= false)
        {
            this.MaxError = MaxError;
            this.IsRelativeError = IsRelativeError;
        }
        #region 属性 
        /// <summary>
        /// 允许的最大误差（含）
        /// </summary>
        public double MaxError { get; set; }
        /// <summary>
        /// 是否是相对误差
        /// </summary>
        public bool IsRelativeError { get; set; }
        #endregion

        public override FractionalPartDataMaintainer Create(string key)
        {
            return new FractionalPartDataMaintainer(MaxError, IsRelativeError);
        }
    }

    /// <summary>
    ///小数部分数据维护器，如用于MW数值的维护。与父类不同，此处CurrentValue为小数值。
    ///只有小数部分才有意义，但同时需要考虑数据的偏移量，判断粗差和跳变更新等。
    /// </summary>
    public class FractionalPartDataMaintainer : FloatNumberMaintainer
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="jugedWindowSize">窗口大小</param>
        public FractionalPartDataMaintainer(double MaxError, bool IsRelativeError = false):base(MaxError, IsRelativeError )
        { 
        }

        #region 属性 
        /// <summary>
        /// 当前的整数变差
        /// </summary>
        public int IntBias { get; set; }
        /// <summary>
        /// 滤波后的整数和小数。
        /// </summary>
        public IntFractionNumber IntFraction { get { return new IntFractionNumber(CurrentValue, IntBias); } }
        #endregion 
        /// <summary>
        /// 整数偏差发生了改变。
        /// </summary>
        public event IntEventHandler IntBiasChanged;

        /// <summary>
        /// 新增加一个数据判断通过后才入库。
        /// 添加成功后返回true。
        /// </summary>
        /// <param name="key"></param>
        public override Boolean CheckAndAdd(double newVal, NumeralWindowData buffer)
        {
            //这里同父类一样，采用原始数据判断，以减少计算量。                 
            if (ErrorJudge.IsFirst) { // 第一次取其小数
                IntBias = CaculateIntBias(buffer.GetNeatlyWindowData()); 
                ErrorJudge.SetReferenceValue(newVal);
            } 
            else { ErrorJudge.SetReferenceValue(this.IntFraction.Value); }

            var fraction = newVal - IntBias;

            if (ErrorJudge.IsOverLimit(newVal))//判断是否超限
            {
                var list = buffer.InsertAndReturnNew(0, newVal); 

                if (ErrorJudge.IsJumped(list))//跳跃，则重新计算整数值，否则认为是粗差，直接忽略。
                {
                    var newBuffer2 = new NumeralWindowData(list);
                    var biasBuffer = newBuffer2.GetNeatlyWindowData();
                    IntBias = CaculateIntBias(biasBuffer);

                    fraction = newVal - IntBias;
                    Add(fraction);
                    return true;
                }
                return false;
            }
            else
            {
                Add(fraction);
                return true;
            }
        }
         

        /// <summary>
        /// 计算整数偏差。新偏差与原数据应该控制在 0.5 以内。
        /// </summary> 
        /// <param name="buffer"></param>
        /// <returns></returns>
        public int CaculateIntBias(NumeralWindowData buffer)
        {
            var average = buffer.AverageValue;
            var newIntBias = Geo.Utils.DoubleUtil.GetNearstIntBias(average, this.CurrentValue);
            if (IntBias != newIntBias) { OnIntBiasChanged(newIntBias); }
            return newIntBias;
        } 
        /// <summary>
        /// 事件函数
        /// </summary>
        /// <param name="val"></param>
        protected void OnIntBiasChanged(int val) { if (IntBiasChanged != null) IntBiasChanged(val); } 
    }

    /// <summary>
    /// 数值序列维护期。数值进行滤波，如果发现粗差则忽略，如果发现跳变，则重新滤波。
    /// </summary>
    public class FloatNumberMaintainer
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="jugedWindowSize">窗口大小</param>
        public FloatNumberMaintainer(double MaxError, bool IsRelativeError = false)
        {
            ErrorJudge = new ErrorJudge(MaxError, IsRelativeError);
        }

        #region 属性
        /// <summary>
        /// 当前平差对象
        /// </summary>
        public AdjustResultMatrix CurrentAdjustment { get; set; }
        /// <summary>
        /// 当前值，通常是滤波值或者平均值。
        /// </summary>
        public double CurrentValue { get; set; }
        /// <summary>
        /// 误差判断
        /// </summary>
        public ErrorJudge ErrorJudge { get; set; }
        #endregion

        /// <summary>
        /// 输入采纳后激发。
        /// </summary>
        public event NumberEventHandler ValueUpdated;

        /// <summary>
        /// 新增加一个数据判断通过后才入库。
        /// 添加成功后返回true。
        /// </summary>
        /// <param name="key"></param>
        public virtual Boolean CheckAndAdd(double newVal, NumeralWindowData buffer)
        {
            if (ErrorJudge.IsFirst) { ErrorJudge.SetReferenceValue(newVal); }
            else { ErrorJudge.SetReferenceValue(this.CurrentValue); }

            if (ErrorJudge.IsOverLimit(newVal))//判断是否超限
            {
                var list = buffer.ToList();
                list.Insert(0, newVal);

                NumeralWindowData newBuffer = new NumeralWindowData(list);

                if (ErrorJudge.IsJumped(buffer))//跳跃，则重新计算整数值，否则认为是粗差，直接忽略。
                {
                    CurrentAdjustment = null; //重新计算滤波值，

                    Add(newVal);
                    return true;
                }
                return false;
            }
            else
            {
                Add(newVal);
                return true;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newVal"></param>
        protected void Add(double newVal)
        {
            CurrentValue = Filter(newVal);
            OnValueUpdated(CurrentValue);
        }

        /// <summary>
        /// 事件函数
        /// </summary>
        /// <param name="val"></param>
        protected void OnValueUpdated(double val) { if (ValueUpdated != null) ValueUpdated(val); } 

        /// <summary>
        /// 滤波。默认期望为常数。
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        protected double Filter(double rawValue)
        {
            AdjustResultMatrix prev = CurrentAdjustment;

            var builder = new OneDimAdjustMatrixBuilder(rawValue, prev);
            var sp = new SimpleKalmanFilter();
            var ad = sp.Run(builder);
            CurrentAdjustment = ad;
            var smoothData = ad.Estimated[0];
            return smoothData;
        }

    }
     

}