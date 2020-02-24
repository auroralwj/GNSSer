//2016.10.17, czs, create in hongqing,  周跳性数据滤波器

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
    /// 周跳性数据滤波器管理器.周跳性数据滤波器，需要考虑周跳和粗差，此处采用一维常量进行滤波
    /// </summary>
    public class CyclicalNumerFilterManager : BaseDictionary<string, CyclicalNumerFilter>
    {
         /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MaxErrorTimes"></param>
        /// <param name="IsRelativeError"></param>
        public CyclicalNumerFilterManager(double MaxErrorTimes, bool IsRelativeError)
        {
            this.MaxErrorTimes = MaxErrorTimes;
            this.IsRelativeError = IsRelativeError; 
        }

        
        /// <summary>
        /// 最大误差倍数
        /// </summary>
        public double MaxErrorTimes { get; set; }
        /// <summary>
        /// 是否是相对误差
        /// </summary>
        public bool IsRelativeError { get; set; }
        /// <summary>
        /// 创建一个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override CyclicalNumerFilter Create(string key)
        {
            return new CyclicalNumerFilter(MaxErrorTimes, IsRelativeError);
        }
    }

    /// <summary>
    /// 周跳性数据滤波器，需要考虑周跳和粗差，此处采用一维常量进行滤波.
    /// 将小数和整数部分分别存储，发生周跳后，将对小数部分重新滤波，而只需更改整数部分。
    /// </summary>
    public class CyclicalNumerFilter
    {
        Log log = new Log(typeof(CyclicalNumerFilter));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MaxError"></param>
        /// <param name="IsRelativeError"></param>
        public CyclicalNumerFilter(double MaxError, bool IsRelativeError)
        {
            this.MaxError = MaxError; 
            AdjustFilter = new AdjustFilter();
            FactionCutter = new PeriodPipeFilter(1, 0.5);
            Index = -1;
            ErrorJudge = new ErrorJudge(MaxError, IsRelativeError);
        }
        #region  属性
        /// <summary>
        /// 误差判断器
        /// </summary>
        ErrorJudge ErrorJudge { get; set; } 
        /// <summary>
        /// 从外部指定指定是否需要重新对齐。如有周跳时。
        /// </summary>
        public bool IsAlignNeeded { get; set; }
        /// <summary>
        /// 当前编号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 最大误差倍数
        /// </summary>
        public double MaxError { get; set; } 
        /// <summary>
        /// 截断的整数部分，使得余下部分在同一数量级别。
        /// </summary>
        public int IntegerPart { get; set; }
        /// <summary>
        /// 窗口数据。用于判断是周跳，还是粗差。
        /// </summary>
        public NumeralWindowData Buffers { get; set; }
        /// <summary>
        /// 平差器
        /// </summary>
        AdjustFilter AdjustFilter { get; set; }
        /// <summary>
        /// 如果发生周跳，则保证小数部分对齐。
        /// </summary>
        public PeriodPipeFilter FactionCutter { get; set; }
        /// <summary>
        /// 是否需要缓存
        /// </summary>
        public bool IsBufferNeeded { get { return IsNeedInitValue() || IsGrossError(); } }
        /// <summary>
        /// 当前原始数据。
        /// </summary>
        public RmsedNumeral CurrentRawValue { get; set; }
        /// <summary>
        /// 当前滤波后的数据小数部分 。参考结果，一般为上一个结果，用于判断当前是否粗差。
        /// </summary>
        public RmsedNumeral CurrentFilteredFraction { get; set; }
        /// <summary>
        /// 当前滤波后的数据含小数和整数部分 。
        /// </summary>
        public RmsedNumeral CurrentFilteredValue { get; set; }
        /// <summary>
        /// 指示粗差设置后，是否计算过了。
        /// </summary>
        bool _isCurrentSolved = false;
        /// <summary>
        /// 设置当前值。
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public CyclicalNumerFilter SetRawValue(RmsedNumeral rawValue)
        {
             this._isCurrentSolved = false;
            this.CurrentRawValue = rawValue;
            return this;
        }
        /// <summary>
        /// 设置缓存。
        /// </summary>
        /// <param name="Buffers"></param>
        /// <returns></returns>
        public CyclicalNumerFilter SetBuffer(NumeralWindowData Buffers)
        {
            this.Buffers = Buffers;
            return this;
        }
        #endregion

        
        /// <summary>
        /// 返回滤波后的值.为小数部分的滤波值。
        /// </summary>
        /// <param name="rawValue">具有整周的原始值。</param>
        /// <returns></returns>
        public RmsedNumeral Filter(RmsedNumeral rawValue)
        {
            SetRawValue(rawValue).Calculate();
            return CurrentFilteredFraction;
        }
 
       
        /// <summary>
        /// 计算返回滤波后的数据，为小数部分的滤波值。
        /// </summary> 
        /// <returns></returns>
        public CyclicalNumerFilter Calculate()
        {
            if (_isCurrentSolved) { return this; }
            _isCurrentSolved = true;

            Index++;
            //决定截取整数部分
            if (IsNeedInitValue())
            {
                this.CurrentFilteredFraction = GetInitValue(CurrentRawValue);
            }
            else
            {
                var isError = IsGrossError();
                if (isError) //用于判断是周跳，还是粗差。
                {
                    var isGrossError = ErrorJudge.IsJumped(Buffers, CurrentRawValue.Value);//注意缓存为未对齐数据
                    if (isGrossError)//粗差,剔除，返回原来的。 //粗差不动，沿用当前
                    {
                        return this;
                    } 
                    else //发生周跳，重新初始化
                    {
                        var init = GetInitValue(CurrentRawValue);//
                        this.CurrentFilteredFraction = FilterAlignedValue(init);
                    }
                }
                else
                {
                    //滤波
                    this.CurrentFilteredFraction = FilterAlignedValue(new RmsedNumeral(CurrentRawValue.Value - IntegerPart, CurrentRawValue.Rms));
                }
            }
            this.CurrentFilteredValue = new RmsedNumeral(CurrentFilteredFraction.Value + IntegerPart, CurrentFilteredFraction.Rms);
            return this;
        }
        /// <summary>
        /// 当前值是否粗差。
        /// </summary>
        /// <returns></returns>
        private bool IsGrossError()
        {
            var alignedValue = CurrentRawValue.Value - IntegerPart;

            var isError = ErrorJudge.IsOverLimit(alignedValue, CurrentFilteredFraction.Value);
            return isError;
        }
        /// <summary>
        /// 是否需要初始化数据
        /// </summary>
        /// <returns></returns>
        private bool IsNeedInitValue()
        {
            return Index == 0 || IsAlignNeeded || CurrentFilteredFraction == null;
        }
 
        /// <summary>
        /// 对齐后的数据，进行滤波。
        /// </summary>
        /// <param name="aligned"></param>
        /// <returns></returns>
        private RmsedNumeral FilterAlignedValue(RmsedNumeral aligned)
        {
            var result = AdjustFilter.Filter(aligned);

            this.CurrentFilteredFraction = result;

            return result;
        }

        /// <summary>
        /// 第一次，初值,周跳发生后，也采用本法赋值。
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        private RmsedNumeral GetInitValue(RmsedNumeral rawValue)
        {
            //上一次小数作为参考，以使本次最为接近
            if (CurrentFilteredFraction != null) { FactionCutter.ReferenceValue = CurrentFilteredFraction.Value; }

            //计算整数部分或周跳的大小
            var bufferAve = Buffers.AverageValue;//采用平均数，避免跳跃
            var alignedFraction = FactionCutter.Filter(bufferAve); 

            this.IntegerPart = (int)(bufferAve - alignedFraction); 
            this.CurrentFilteredFraction = new RmsedNumeral(alignedFraction, rawValue.Rms);

            log.Debug("截取了整数部分 " + this.IntegerPart);
            return this.CurrentFilteredFraction;
        } 
    }
      
}