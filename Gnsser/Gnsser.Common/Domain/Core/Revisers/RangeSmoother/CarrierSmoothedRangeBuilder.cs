//2016.05.08, czs, create in hongqing, 载波相位平滑伪距 PhaseSmoothedRangeBuilder
//2017.11.10, czs, create in hongqing, 载波相位平滑伪距，Hatch 递推滤波模型 HatchPhaseSmoothedRangeBuilder
//2018.05.20, czs, eidt in HMX， 采用缓存，改进载波平滑伪距算法,合并 PhaseSmoothedRangeBuilder 和 HatchPhaseSmoothedRangeBuilder
//2018.06.18, czs, edit in HMX, 重构，区分原始和改进平滑

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
    /// 电离层延迟变化的改正类型
    /// </summary>
    public enum IonoDifferCorrectionType
    {
        /// <summary>
        /// 不改正
        /// </summary>
        No,
        /// <summary>
        /// 双频载波电离层改正，可以精确求出
        /// </summary>
        DualFreqCarrier,
        /// <summary>
        /// 滑动开窗多项式拟合， 在历元当中多项式拟合，需设置阶次和历元数量，以及对应的缓存。
        /// </summary>
        WindowPolyfit,
        /// <summary>
        /// 滑动开窗加权平均
        /// </summary>
        WindowWeightedAverage,
        /// <summary>
        /// 电离层变化率文件 IonoDeltaFilePath
        /// </summary>
        IndicatedFile,
    }

    /// <summary>
    /// 管理器
    /// </summary>
    public class CarrierSmoothedRangeBuilderManager : NamedCarrierSmoothedRangeBuilderManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isApprovedSmooth">是否采用GNSSer改进平滑算法</param>
        /// <param name="windowSize">窗口大小</param>
        /// <param name="IsWeighted"></param>
        /// <param name="SmoothRangeType"></param>
        /// <param name="IsDeltaIonoCorrect">仅改进算法有效</param>
        /// <param name="OrderOfDeltaIonoPolyFit">仅改进算法有效</param>
        /// <param name="BufferSize">缓存大小</param>
        /// <param name="ionoFitEpochCount">电离层拟合历元数量，如果非滑动窗口，则与指定窗口相同</param>
        public CarrierSmoothedRangeBuilderManager(
            bool isApprovedSmooth,
            int windowSize,
            bool IsWeighted,
            IonoDifferCorrectionType IsDeltaIonoCorrect,
            int OrderOfDeltaIonoPolyFit = 2,
             int BufferSize = 60,
             int ionoFitEpochCount = 20,
            SmoothRangeSuperpositionType SmoothRangeType = SmoothRangeSuperpositionType.快速更新算法)
           : base(isApprovedSmooth,
                 windowSize,
            IsWeighted,
            IsDeltaIonoCorrect,
            OrderOfDeltaIonoPolyFit, 
            BufferSize,
            ionoFitEpochCount,
            SmoothRangeType
            )
        {
            this.WindowSize = windowSize;
        }
        /// <summary>
        /// 获取或创造
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AbstractSmoothRangeBuilder GetOrCreate(SatelliteNumber key)
        {
            return GetOrCreate(key + "");
        }
    }
    /// <summary>
    /// 管理器
    /// </summary>
    public class NamedCarrierSmoothedRangeBuilderManager : BaseDictionary<string, AbstractSmoothRangeBuilder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isApprovedSmooth">是否采用GNSSer改进平滑算法</param>
        /// <param name="windowSize">窗口大小</param>
        /// <param name="SmoothRangeType"></param>
        /// <param name="IsWeighted"></param>
        /// <param name="IonoDifferCorrectionType">仅改进算法有效</param>
        /// <param name="IonoFitEpochCount"> </param>
        /// <param name="OrderOfDeltaIonoPolyFit">仅改进算法有效</param>
        /// <param name="BufferSize">仅改进算法有效</param>
        public NamedCarrierSmoothedRangeBuilderManager(
            bool isApprovedSmooth,
            int windowSize,
            bool IsWeighted,
            IonoDifferCorrectionType IonoDifferCorrectionType,
            int OrderOfDeltaIonoPolyFit = 1,
             int BufferSize = 60,
             int IonoFitEpochCount= 30,
            SmoothRangeSuperpositionType SmoothRangeType = SmoothRangeSuperpositionType.快速更新算法)
        {
            this.WindowSize = windowSize;
            this.IsApprovedSmooth = isApprovedSmooth;
            this.IsWeighted = IsWeighted;
            this.IonoDifferCorrectionType = IonoDifferCorrectionType;
            this.OrderOfDeltaIonoPolyFit = OrderOfDeltaIonoPolyFit;
            this.SmoothRangeType = SmoothRangeType;
            this.BufferSize = BufferSize;
            this.IonoFitEpochCount = IonoFitEpochCount;
        }

        SmoothRangeSuperpositionType SmoothRangeType;

        /// <summary>
        /// 是否采用窗口。
        /// </summary>
        public bool IsApprovedSmooth { get; set; }

        /// <summary>
        /// 最大窗口大小，单位：历元 次
        /// </summary>
        public int WindowSize { get; set; }
        /// <summary>
        /// 缓存大小，用于拟合电离层变化，使其在数据中部
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 电离层拟合历元数量
        /// </summary>
        public int IonoFitEpochCount { get; set; }

        /// <summary>
        /// 电离层变化拟合阶数。
        /// </summary>
        public int OrderOfDeltaIonoPolyFit { get; set; }

        /// <summary>
        /// 是否加权
        /// </summary>
        public bool IsWeighted { get; set; }
        /// <summary>
        /// 是否电离层改正
        /// </summary>
        public IonoDifferCorrectionType IonoDifferCorrectionType { get; set; }

        /// <summary>
        /// 指定的电离层变化率文件路径
        /// </summary>
        public string IndicatedIonoDeltaFilePath { get; set; }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override AbstractSmoothRangeBuilder Create(string key)
        {
            if (IsApprovedSmooth)
            {
                return new GnsserWindowedPhaseSmoothedRangeBuilder(
                    WindowSize,
                    key,
                    IsWeighted,
                    IonoDifferCorrectionType,
                    OrderOfDeltaIonoPolyFit, BufferSize, this.IonoFitEpochCount, SmoothRangeType);
            }
            else
            {
                return new RawHatchPhaseSmoothedRangeBuilder(WindowSize, IsWeighted, key, IonoDifferCorrectionType, OrderOfDeltaIonoPolyFit, BufferSize, IonoFitEpochCount)
                {
                    IndicatedIonoDeltaFilePath = IndicatedIonoDeltaFilePath
                };
            }
        }
    }

    /// <summary>
    /// 递推滤波模型，载波相位平滑伪距。
    /// </summary>
    public abstract class AbstractSmoothRangeBuilder : AbstractBuilder<RmsedNumeral>, Geo.Namable
    {
        Log log = new Log(typeof(AbstractSmoothRangeBuilder));

        /// <summary>
        /// 载波相位平滑伪距。
        /// </summary> 
        public AbstractSmoothRangeBuilder()
        {
            this.Name = "伪距平滑";
        }
        /// <summary>
        /// Hatch 递推滤波模型，载波相位平滑伪距。
        /// </summary> 
        /// <param name="IsWeighted"></param> 
        /// <param name="IonoDifferCorrectionType"></param>
        /// <param name="IonoFitOrder"></param>
        /// <param name="BufferSize"></param>
        /// <param name="IonoFitEpochCount">电离层拟合历元数量</param>
        public AbstractSmoothRangeBuilder(bool IsWeighted, IonoDifferCorrectionType IonoDifferCorrectionType, int IonoFitOrder, int BufferSize, int IonoFitEpochCount)
        { 
            this.IsWeighted = IsWeighted; 
            this.RawRangeStdDev = 2;
            this.CurrentRaw = new RangePhasePair(0, 0, 0); 

            this.IonoWindow = new TimeNumeralWindowData(BufferSize * 2);

            this.IonoDifferCorrectionType = IonoDifferCorrectionType; 
            this.IonoFitOrder = IonoFitOrder;
            this.IonoFitDataCount = IonoFitEpochCount;
            this.FirstInputtedIonoDiffer = 0;
        }
        #region  属性

        /// <summary>
        /// 伪距观测噪声，默认为2m.
        /// </summary>
        public double RawRangeStdDev { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        #region 电离层改正
        /// <summary>
        /// 电离层拟合类型
        /// </summary>
        public IonoDifferCorrectionType IonoDifferCorrectionType { get; set; }

        /// <summary>
        /// 第一次输入的电离层改正
        /// </summary>
        public double FirstInputtedIonoDiffer { get; set; }

        /// <summary>
        /// 是否拟合电离层
        /// </summary>
        public bool IsFitDeltaIono
        {
            get =>
                    IonoDifferCorrectionType != IonoDifferCorrectionType.No
                    && IonoDifferCorrectionType != IonoDifferCorrectionType.DualFreqCarrier
                    && IonoDifferCorrectionType != IonoDifferCorrectionType.IndicatedFile
                    ;
        } 
        /// <summary>
        /// 电离层偏差拟合阶次
        /// </summary>
        public int IonoFitOrder { get; set; }
        /// <summary>
        /// 拟合数据量
        /// </summary>
        public int IonoFitDataCount { get; set; }
        /// <summary>
        /// 电离层拟合窗口
        /// </summary>
        public TimeNumeralWindowData IonoWindow { get; set; }
        #endregion

        #region 计算结果
        /// <summary>
        /// 外推伪距
        /// </summary>
        public double ExtraRange { get; set; }
        /// <summary>
        /// 平滑伪距
        /// </summary>
        public double SmoothRange { get; set; }

        /// <summary>
        /// 是否加权
        /// </summary>
        public bool IsWeighted { get; set; }
        #endregion
        /// <summary>
        /// 上一个平滑伪距
        /// </summary>
        public double PrevSmoothRange { get; set; }
        /// <summary>
        /// 上一个载波等效距离
        /// </summary>
        public RangePhasePair PrevData { get; set; }

        /// <summary>
        ///当前原始值，第一个为伪距，第二个为载波
        /// </summary>
        public virtual RangePhasePair CurrentRaw { get; set; }

        #endregion

        #region  设值 


        /// <summary>
        /// 是否重置，如果发生周跳。
        /// </summary>
        /// <param name="IsReset"></param>
        /// <returns></returns>
        public abstract AbstractSmoothRangeBuilder SetReset(bool IsReset);


        /// <summary>
        /// 设置原始缓存，用于计算电离层，原始伪距,载波伪距
        /// </summary>
        /// <param name="time"></param>
        /// <param name="rawRange"></param>
        /// <param name="rawPhaseRange"></param>
        /// <returns></returns>
        public virtual AbstractSmoothRangeBuilder SetBufferValue(Time time, double rawRange, double rawPhaseRange)
        {
            if (IonoDifferCorrectionType == IonoDifferCorrectionType.WindowPolyfit)//需要缓存
            { 
                var currentRaw = new RangePhasePair(rawRange, rawPhaseRange, 0) { Time = time };
                double y = currentRaw.GetRawIonoAndHalfAmbiValue();
                IonoWindow.Add(time, y);
            }
            return this;
        }
        /// <summary>
        /// 设置原始原始伪距,载波伪距
        /// </summary>
        /// <param name="time"></param>
        /// <param name="rawRange"></param>
        /// <param name="rawPhaseRange"></param>
        /// <param name="ionoDiffer">历元间电离层变化</param>
        /// <returns></returns>
        public virtual AbstractSmoothRangeBuilder SetRawValue(Time time, double rawRange, double rawPhaseRange, double ionoDiffer )
        {
            //通过是否相等，判断是否重复设值
            if (CurrentRaw !=null &&  ( CurrentRaw.First == rawRange && this.CurrentRaw.Second == rawPhaseRange))
            {
                return this;
            };

            this.PrevSmoothRange = SmoothRange;
            this.PrevData = CurrentRaw;

            if(FirstInputtedIonoDiffer == 0)
            {
                FirstInputtedIonoDiffer = CurrentRaw.GetRawIonoAndHalfAmbiValue();// ionoDiffer;
                if(FirstInputtedIonoDiffer == 0)
                {
                    FirstInputtedIonoDiffer = ionoDiffer;
                }
            }
            double differIono = 0;
            if (IonoDifferCorrectionType == IonoDifferCorrectionType.DualFreqCarrier ||IonoDifferCorrectionType ==  IonoDifferCorrectionType.IndicatedFile)
            {
                differIono = ionoDiffer - FirstInputtedIonoDiffer;
            }

            this.CurrentRaw = new RangePhasePair(rawRange, rawPhaseRange, differIono) { Time = time };


            if (IsFitDeltaIono)
            {
                FitIonoAndAmbiValue(CurrentRaw);
            }
            return this;
        }

        /// <summary>
        /// 拟合电离层和模糊度值
        /// </summary> 
        public void FitIonoAndAmbiValue(RangePhasePair CurrentRaw)
        {
            double y = CurrentRaw.GetRawIonoAndHalfAmbiValue();
            IonoWindow.Add(CurrentRaw.Time, y);//应该在缓存时已经添加了，此处以防万一没有添加，程序将自动判断，不会重复添加

            if (IonoDifferCorrectionType == IonoDifferCorrectionType.WindowPolyfit)
            {
                var fit = IonoWindow.GetPolyFit(CurrentRaw.Time, IonoFitOrder, IonoFitDataCount);//  .GetTimedLsPolyFit(IonoFitOrder);
                if (fit != null)
                {
                    CurrentRaw.FittedIonoAndAmbiValue = fit.GetY(CurrentRaw.Time);
                }
            }else if(IonoDifferCorrectionType == IonoDifferCorrectionType.WindowWeightedAverage)
            {
                CurrentRaw.FittedIonoAndAmbiValue =IonoWindow.GetAdaptiveLinearFitValue(CurrentRaw.Time, IonoFitDataCount).Value;
            }
        }

        #endregion
        /// <summary>
        /// 重设。
        /// </summary>
        public virtual void Reset()
        {
            FirstInputtedIonoDiffer = 0;
        }
    }

    /// <summary>
    /// GNSSer 改进伪距平滑 递推滤波模型，载波相位平滑伪距。
    /// </summary>
    public class GnsserWindowedPhaseSmoothedRangeBuilder : AbstractSmoothRangeBuilder
    {
        Log log = new Log(typeof(GnsserWindowedPhaseSmoothedRangeBuilder));


        /// <summary>
        ///  Hatch 递推滤波模型，载波相位平滑伪距。
        /// </summary>
        /// <param name="maxEpochCount"></param>
        /// <param name="name"></param>
        /// <param name="IsWeighted"></param>
        /// <param name="IsDeltaIonoCorrect"></param>
        /// <param name="OrderOfDeltaIonoPolyFit"></param>
        /// <param name="bufferSize"></param>
        /// <param name="IonoFitEpochCount"></param>
        /// <param name="SmoothRangeType"></param>
        public GnsserWindowedPhaseSmoothedRangeBuilder(
            int maxEpochCount,
            string name,
            bool IsWeighted,
            IonoDifferCorrectionType IsDeltaIonoCorrect,
            int OrderOfDeltaIonoPolyFit = 2, int bufferSize = 30, int IonoFitEpochCount = 30,
            SmoothRangeSuperpositionType SmoothRangeType = SmoothRangeSuperpositionType.快速更新算法)
            :base(  IsWeighted,   IsDeltaIonoCorrect, OrderOfDeltaIonoPolyFit, bufferSize, IonoFitEpochCount)
        {
            this.Name = name;
            
            SmoothRangeWindow = new SmoothRangeWindow(
                maxEpochCount,
                IsWeighted,
                IsDeltaIonoCorrect == IonoDifferCorrectionType.WindowPolyfit,
                OrderOfDeltaIonoPolyFit, bufferSize, SmoothRangeType
                );
        }
        #region  属性

        /// <summary>
        /// 真正的平滑器。
        /// </summary>
        public SmoothRangeWindow SmoothRangeWindow { get; set; }
        #endregion

        #region  设值  
        /// <summary>
        /// 设置原始原始伪距,载波伪距
        /// </summary>
        /// <param name="time"></param>
        /// <param name="rawRange"></param>
        /// <param name="rawPhaseRange"></param>
        /// <param name="inputeddIonoDiffer"></param>
        /// <returns></returns>
        public override AbstractSmoothRangeBuilder SetRawValue(Time time, double rawRange, double rawPhaseRange, double inputeddIonoDiffer)
        {
            base.SetRawValue(time, rawRange, rawPhaseRange, inputeddIonoDiffer);
            // this.SmoothRangeWindow.Add(time, rawRange, rawPhaseRange);
            this.SmoothRangeWindow.Add(this.CurrentRaw);
            return this;
        }
        /// <summary>
        /// 是否重置，如果发生周跳。
        /// </summary>
        /// <param name="IsReset"></param>
        /// <returns></returns>
        public override AbstractSmoothRangeBuilder SetReset(bool IsReset)
        {
            if (IsReset)
            {
                this.SmoothRangeWindow.Clear();
            };
            return this;
        }

        #endregion

        /// <summary>
        /// 采用窗口进行平滑
        /// </summary>
        /// <returns></returns>
        public RmsedNumeral GetSmoothedRange()
        {
            if (SmoothRangeWindow.Count == 0)//有可能，刚刚被清空了
            {
                if(CurrentRaw.PseudoRange == 0)
                {
                    throw new Exception("are you kidding? you must put one value first.");
                }

                return new RmsedNumeral(CurrentRaw.PseudoRange, RawRangeStdDev);

            }

            return SmoothRangeWindow.GetSmoothValue();
        }

        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public override RmsedNumeral Build()
        {
            return GetSmoothedRange();
        }

        /// <summary>
        /// 重设。
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            SmoothRangeWindow.Clear();
        }
    }
}