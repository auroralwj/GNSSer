//2016.08.20, czs, create in 福建永安大湖, 序列化数据，链表结构

using System;

namespace Geo
{ 
    /// <summary>
    /// 具有整数偏差的序列值，只保存当前历元和上一历元信息。实际为一个链条结构。
    /// </summary>
    public abstract class IntBiasedSequenceValue<T> : SequenceValue<T>, IIntBiasedSequenceValue<T> where T : IntBiasedSequenceValue<T>
    { 

        #region 基本属性 
        /// <summary>
        /// 整数，用于数值归算到小数。
        /// </summary>
        public int IntBias { get; set; }
        #endregion

        #region 辅助属性
        /// <summary>
        /// 判断整数偏差值是否改变了。
        /// </summary>
        public bool IsIntBiasChanged { get { if (HasPrevValue) { return IntBias == PrevValue.IntBias; } return true; } }

        /// <summary>
        /// 原始数据减去整数偏差。
        /// </summary>
        public double FractionOfRawValue { get { return RawValue - IntBias; } }
        /// <summary>
        /// 平滑数据减去整数偏差。
        /// </summary>
        public double FractionOfSmoothValue { get { return SmoothValue - IntBias; } }  

        #endregion

        #region 方法 

        /// <summary>
        /// 设置偏差整数值，取自平滑值的取整。
        /// </summary>
        public void SetIntBias()
        {
            this.IntBias = new IntFractionNumber(SmoothValue).Int;
        }
        #endregion
    }


    /// <summary>
    /// MW 值，只保存当前历元和上一历元信息。实际为一个链条结构。
    /// </summary>
    public abstract class SequenceValue<T> : ISequenceValue<T> where T : SequenceValue<T>
    { 

        #region 基本属性 
        /// <summary>
        /// 上一个值。
        /// </summary>
        public T PrevValue { get; set; }

        /// <summary>
        /// 平滑后的MW值，通常为滤波值，需要外部计算并赋值。
        /// </summary>
        public virtual double SmoothValue { get; set; }  
        /// <summary>
        /// 历元编号
        /// </summary>
        public abstract  int Index { get; }
        /// <summary>
        /// 原始MW值
        /// </summary>
        public abstract double RawValue { get; }
        /// <summary>
        /// 是否具有先前信息。
        /// </summary>
        public bool HasPrevValue { get { return PrevValue != null; } }
        /// <summary>
        /// 编号间隙，可以探知是否发生了跳变。
        /// </summary>
        public int IndexGap { get { if (HasPrevValue) return this.Index - PrevValue.Index; return 0; } }
        /// <summary>
        /// 与上一历元差，平滑数据。
        /// </summary>
        public double SmoothDifferValue { get { if (HasPrevValue) return this.SmoothValue - PrevValue.SmoothValue; return 0; } }
        /// <summary>
        /// 实际数据与上一历元平滑数据差。
        /// </summary>
        public double DifferValue { get { if (HasPrevValue) return this.RawValue - PrevValue.SmoothValue; return 0; } }
        /// <summary>
        /// 与上一历元差，原始数据。
        /// </summary>
        public double RawDifferValue { get { if (HasPrevValue) return this.RawValue - PrevValue.RawValue; return 0; } }

        #endregion

        #region 方法 

        /// <summary>
        /// 通过比较上一个数据的编号间隔和数据差分值
        /// </summary>
        /// <param name="maxIndexGap"></param>
        /// <param name="maxDiffer"></param>
        /// <returns></returns>
        public bool IsBreaked(int maxIndexGap = 10, double maxDiffer = 0.2)
        {
            if (HasPrevValue)
            {
                if (IndexGap >= maxIndexGap
                    || Math.Abs(DifferValue) >= maxDiffer)
                { return true; }
                else { return false; }
            }
            return true;//默认为真
        } 
        #endregion
    }

    /// <summary>
    /// 具有整数偏差的序列数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIntBiasedSequenceValue<T> : ISequenceValue<T> where T : IIntBiasedSequenceValue<T>
    {

        #region 整数偏差相关
        /// <summary>
        /// 与整数差异
        /// </summary>
        int IntBias { get; set; }
        /// <summary>
        /// 对齐整数是否改变
        /// </summary>
        bool IsIntBiasChanged { get; }
        /// <summary>
        /// 设置偏差整数
        /// </summary>
        void SetIntBias();
        #endregion
    }

    /// <summary>
    /// 序列化数据，链表结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISequenceValue<T> where T : ISequenceValue<T>
    {
        /// <summary>
        /// 编号
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 是否具有上一数据
        /// </summary>
        bool HasPrevValue { get; }
        /// <summary>
        /// 与上一数据的编号差异
        /// </summary>
        int IndexGap { get; }
        /// <summary>
        /// 上一个数据
        /// </summary>
        T PrevValue { get; set; }
           /// <summary>
        /// 原始数据
        /// </summary>
        double RawValue { get; }
        /// <summary>
        /// 平滑数据
        /// </summary>
        double SmoothValue { get; set; }

        /// <summary>
        /// 是否断裂
        /// </summary>
        /// <param name="maxIndexGap"></param>
        /// <param name="maxDiffer"></param>
        /// <returns></returns>
        bool IsBreaked(int maxIndexGap = 10, double maxDiffer = 0.2);
    }


}
