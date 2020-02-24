//2016.05.08, czs, create in hongqing, 载波相位平滑伪距
//2016.05.10, czs, edit in hongqing, 采用窗口平滑伪距
//2018.05.20, czs, edit in HMX, 采用基于缓存的窗口算法，改进电离层延迟
//2018.06.06, czs, edit in HMX, 增加电离层改正，小小的突破！！
//2018.06.15, czs, edit in HMX, 重新编写了一次平滑伪距
//2018.06.21, czs, edit in HMX, 耗时数日，快速迭代算法及其验证基本完成！！！


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;
using Geo.Times;
using Geo.Algorithm;

namespace Gnsser
{ 
    /// <summary>
    /// 平滑伪距
    /// </summary>
    public class SmoothRangeWindowOld : WindowData<Time, RangePhasePair>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="windowSize"></param>
        /// <param name="IsWeighted"></param>
        /// <param name="IsDeltaIonoCorrect"></param>
        /// <param name="OrderOfDeltaIonoPolyFit"></param>
        /// <param name="SmoothRangeType"></param>
        public SmoothRangeWindowOld(int windowSize, bool IsWeighted = true, bool IsDeltaIonoCorrect = true, int OrderOfDeltaIonoPolyFit = 2, int bufferSize = 30, SmoothRangeSuperpositionType SmoothRangeType = SmoothRangeSuperpositionType.窗口AK迭代)
            : base(windowSize)
        {
            CurrentData = new RangePhasePair(0,0,0);
            this.IsWeighted = IsWeighted;
            this.IsDeltaIonoCorrect = IsDeltaIonoCorrect;
            this.OrderOfDeltaIonoPolyFit = OrderOfDeltaIonoPolyFit;
            this.MaxKeyGap = 151;
            this.RawRangeStdDev = 2;
            this.SmoothRangeType = SmoothRangeType;
            this.IonoWindow = new TimeNumeralWindowData(windowSize + bufferSize); 
            this.PrevData = new RangePhasePair(0, 0,0);
            this.IonoFitDataCount = 30;
             

        }
        #region  属性 
        #region 电离层改正  
        /// <summary>
        /// 电离层拟合窗口
        /// </summary>
        public TimeNumeralWindowData IonoWindow { get; set; } 
        /// <summary>
        /// 电离层变化拟合阶数。
        /// </summary>
        public int OrderOfDeltaIonoPolyFit { get; set; }
        /// <summary>
        /// 拟合数据量
        /// </summary>
        public int IonoFitDataCount { get; set; }
        /// <summary>
        /// 是否电离层改正
        /// </summary>
        public bool IsDeltaIonoCorrect { get; set; }

        /// <summary>
        /// 当前电离层和0.5倍模糊度的等效距离。 y = I0 + bt + λN /2
        /// </summary>
        public double CurrentIonoAndHalfLambdaLen { get; set; }
        #endregion

        /// <summary>
        /// 算法类型
        /// </summary>
        public SmoothRangeSuperpositionType SmoothRangeType { get; set; }
        /// <summary>
        /// 当前的原始数据
        /// </summary>
        public RangePhasePair CurrentData { get; set; }
        /// <summary>
        /// 上一个原始数据
        /// </summary>
        public RangePhasePair PrevData { get; set; }
        /// <summary>
        /// 上一窗口的第一个数据，即为刚刚被踢出的数据。
        /// </summary>
        public RangePhasePair LastRemovedPair { get { return LastRemovedItem.Value; } }
    
        /// <summary>
        /// 上一次的平滑伪距。
        /// </summary>
        public double LastSmoothedRange { get; set; }
        /// <summary>
        /// 上一次的载波距离
        /// </summary>
        public double LastPhaseRange { get; set; }

        /// <summary>
        /// 伪距观测噪声，默认为2.
        /// </summary>
        public double RawRangeStdDev { get; set; }
        /// <summary>
        /// 是否加权
        /// </summary>
        public bool IsWeighted { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 增加一个
        /// </summary>
        /// <param name="time"></param>
        /// <param name="rawRange"></param>
        /// <param name="rawPhaseRange"></param>
        public SmoothRangeWindowOld Add( Time time, double rawRange, double rawPhaseRange , double ionoDiffer)
        {  //通过是否相等，判断是否重复设值
            if (CurrentData.PseudoRange == rawRange && this.CurrentData.PhaseRange == rawPhaseRange)
            {
                return this;
            }
            this.CurrentData = new RangePhasePair(rawRange, rawPhaseRange, ionoDiffer) { Time = time} ;
            this.Add(time, CurrentData);
            
            //电离层改正数据
            if (IsDeltaIonoCorrect && SmoothRangeType == SmoothRangeSuperpositionType.快速更新算法  )
            {
                double y = CurrentData.GetRawIonoAndHalfAmbiValue();//.RangeMinusPhase / 2;
                IonoWindow.Add(time, y);

             //   BigIonoWindow.Add(time, y);
            }
            if (IsDeltaIonoCorrect)
            {
                var fit = IonoWindow.GetPolyFitValue(CurrentData.Time, OrderOfDeltaIonoPolyFit, IonoFitDataCount);
                if (fit != null)
                {
                    CurrentData.FittedIonoAndAmbiValue = fit.Value;
                }
                else
                {
                    CurrentData.FittedIonoAndAmbiValue = CurrentData.GetRawIonoAndHalfAmbiValue();
                }

                
            }

            return this;
        }
        /// <summary>
        /// 缓存提前增加
        /// </summary>
        /// <param name="time"></param>
        /// <param name="rawRange"></param>
        /// <param name="rawPhaseRange"></param>
        /// <returns></returns>
        public SmoothRangeWindowOld BufferAdd(Time time, double rawRange, double rawPhaseRange)
        {   
            //电离层改正数据
            if (IsDeltaIonoCorrect && SmoothRangeType == SmoothRangeSuperpositionType.快速更新算法)
            {
                var data = new RangePhasePair(rawRange, rawPhaseRange, 0) { Time = time };
                double y = data.GetRawIonoAndHalfAmbiValue(); 
                IonoWindow.Add(time, y); 
            }
            return this;
        }

        /// <summary>
        /// 计算平滑的伪距。
        /// </summary> 
        /// <returns></returns>
        public RmsedNumeral GetSmoothValue()
        {
            switch (SmoothRangeType)
            {
                case SmoothRangeSuperpositionType.快速更新算法:
                    return GetSmoothValueByFastUpdate();
                case SmoothRangeSuperpositionType.窗口AK迭代:
                    return GetSmoothValueByWholeWindowRecursionAk();
                case SmoothRangeSuperpositionType.窗口原始迭代:
                    return GetSmoothValueByWholeWindowRecursionRaw();
                default:
                    break;
            }
            return GetSmoothValueByWholeWindowRecursionRaw();
            
        }
        /// <summary>
        /// 计算平滑的伪距，最快的迭代，带电离层的递推平滑算法，可以大大加快速度。
        /// 算法已验证，和窗口单独迭代相同，需要注意的式如果正式平滑时，则不需要加权了。
        /// 2018.06.20, czs， 这是推导了好几天才出的结果，并得到了验证，具体公式请参看本人发表的文章：**平滑伪距***。
        /// </summary>
        /// <returns></returns>
        public RmsedNumeral GetSmoothValueByFastUpdate() 
        {
            int count = this.Count;
            RmsedNumeral result = null;
            double extraP = 0;//外推的结果
            double deltaTail = 0;
            if (this.Count <= 1)
            {
                result = GetFirstOrDefault();
                extraP = result.Value;

                CurrentData.FittedIonoAndAmbiValue = CurrentData.GetRawIonoAndHalfAmbiValue();
            }
            else //采用窗口内的数据进行平滑
            {
                double ionDiffer = 0;
                if (IsDeltaIonoCorrect)//电离层改正赋值
                {
                    //var fit = IonoWindow.GetPolyFitValue(CurrentData.Time,  OrderOfDeltaIonoPolyFit, IonoFitDataCount);
                    //if (fit != null)
                    //{
                    //    CurrentData.FittedIonoAndAmbiValue = fit.Value;
                    //}
                    //else
                    //{
                    //    CurrentData.FittedIonoAndAmbiValue = CurrentData.GetRawIonoAndHalfAmbiValue();
                    //}
                    ionDiffer = (CurrentData.FittedIonoAndAmbiValue - PrevData.FittedIonoAndAmbiValue);
                }

                //所谓的推估表达式，即不采用当前的伪距观测数据，    P0 + (L1 - L0) + 2DI
                extraP = LastSmoothedRange + (this.CurrentData.PhaseRange - PrevData.PhaseRange) + 2 * ionDiffer;

                double sP = extraP;
                if (IsWeighted)//加权，表示采用当前的伪距观测数据
                {
                    if (this.LastRemovedItem.Value != null)//当大于指定的窗口，将扣除窗口外的数据（包括电离层延迟）影响。
                    {
                        double ionoDiffer = 0;
                        if (IsDeltaIonoCorrect)
                        {
                            ionoDiffer = CurrentData.FittedIonoAndAmbiValue - this.LastRemovedPair.FittedIonoAndAmbiValue;

                            //var bigFit = BigIonoWindow.GetTimedLsPolyFit(1);
                            //ionoDiffer = bigFit.GetY( CurrentData.Time) - bigFit.GetY(LastRemovedPair.Time);
                        }
                        deltaTail = 1.0 / (this.Count) * (CurrentData.RangeMinusPhase - this.LastRemovedPair.RangeMinusPhase - 2.0 * ionoDiffer);
                        sP = extraP + deltaTail;
                    }
                    else //原始Hatch滤波
                    {
                        double weight = GetWeight(this.LastKey, count);
                        sP = weight * this.CurrentData.PseudoRange + (1 - weight) * extraP;
                    }
                }
        
                double stdDev = this.RawRangeStdDev * 0.1 / Math.Sqrt(count);
                result = new RmsedNumeral(sP, stdDev);

                //验证算法，2018.06.20, czs，
                //var result3  = GetSmoothValueByWholeWindowRecursionAk();      
                //double ionoDiff;
                //double Ak = GetAk(out ionoDiff);
                //double akDelta = Ak - PrevAk;
                //double differOfAkDelta = akDelta - deltaTail;
                ////var result2 = this.CurrentRaw.PhaseRange + Ak;
                ////double differ = result2 - result.Value;
                ////double differ2 = result2 - result3.Value;
                ////int oo = 0;
                //PrevAk = Ak;
            }


            this.PrevData = this.CurrentData;
            this.LastSmoothedRange = result.Value;
            return result;
        }


        /// <summary>
        /// 计算平滑的伪距，整个窗口迭代计算Ak
        /// </summary>
        /// <returns></returns>
        public RmsedNumeral GetSmoothValueByWholeWindowRecursionAk()
        {
            if (this.Count <= 1) { return GetFirstOrDefault(); }
            int count = this.Count;
            double deltaIono;
            double Ak =  GetAk(out deltaIono);

            double sP = Ak + this.CurrentData.PhaseRange + 2 * deltaIono;

            double stdDev = this.RawRangeStdDev * 0.1 / Math.Sqrt(count);
            //此处不用加权了
            //2018.06.20, czs， 这是推导了好几天才出的结果，并得到了验证，具体公式请参看本人发表的文章：**平滑伪距***。
            return new RmsedNumeral(sP, stdDev);
        }

        private double GetAk(out double deltaIono)
        { 
            deltaIono = 0;
            TimedLsPolyFit ionoFit = null;

             //var IonoWindow = new TimeNumeralWindowData(12);

            var keys = this.OrderedKeys;

            double firstIonoAndHalfLambdaLen = this[keys[0]].FittedIonoAndAmbiValue;
                //IonoWindow.GetPolyFitValue(keys[0], OrderOfDeltaIonoPolyFit, IonoFitDataCount).Value;
            //if (IsDeltaIonoCorrect)
            //{
            //    ionoFit = GetDeltaIonoPolyFit();
            //    if(ionoFit != null)
            //    { 
            //        firstIonoAndHalfLambdaLen = ionoFit.GetY(keys[0]); 
            //    }
            //}
            //采用窗口内的数据进行平滑  
            double sum = 0;
            foreach (var key in keys)
            {
                var PL = this[key];
                if (IsDeltaIonoCorrect && ionoFit != null)
                {
                    //IonoWindow.Add(key, PL.GetRawIonoAndHalfAmbiValue());

                    //var fiter = IonoWindow.GetTimedLsPolyFit(this.OrderOfDeltaIonoPolyFit);
                    //if (fiter != null)
                    //{
                    //    var ionoVal = fiter.GetY(key);
                    //    this.CurrentIonoAndHalfLambdaLen = ionoVal; //y = I0 + bt + λN /2
                    //}
                    //else
                    //{
                    //    this.CurrentIonoAndHalfLambdaLen = ionoFit.GetY(key); //y = I0 + bt + λN /2 
                    //}
                    CurrentIonoAndHalfLambdaLen = PL.FittedIonoAndAmbiValue;
                    deltaIono = (CurrentIonoAndHalfLambdaLen - firstIonoAndHalfLambdaLen);
                }

                sum += PL.RangeMinusPhase - 2 * deltaIono; 
            }
            double Ak = sum / this.Count;
            return Ak;
        }


        /// <summary>
        /// 计算平滑的伪距，整个窗口迭代,公式与上两个应该等价，用于验证，计算原始观测数据
        /// </summary>
        /// <returns></returns>
        public RmsedNumeral GetSmoothValueByWholeWindowRecursionRaw()
        {
            int count = this.Count;

            if (this.Count <= 1) { return GetFirstOrDefault(); }

            var ionoFit =  GetDeltaIonoPolyFit();
            var keys = this.OrderedKeys;
            var firstKey = keys[0];
            var prevKey = keys[0];
            double prevIonoNLen = 0;
            if (IsDeltaIonoCorrect && ionoFit != null)
            {
                prevIonoNLen = ionoFit.GetY(firstKey);
             }

            //采用窗口内的数据进行平滑
            double sP = 0;//最后的结果
            double prevL = 0;//上一个载波距离
            int i = 1;
            foreach (var key in keys)
            {
                var PL = this[key];
                var P = PL.PseudoRange;
                var L = PL.PhaseRange;
                double correction = 0;
                if(IsDeltaIonoCorrect && ionoFit != null)
                { 
                    this.CurrentIonoAndHalfLambdaLen = ionoFit.GetY(key); //y = I0 + bt + λN /2
                    correction =  2 * (CurrentIonoAndHalfLambdaLen - prevIonoNLen);

                
                    prevIonoNLen = CurrentIonoAndHalfLambdaLen;
                    prevKey = key;
                } 
                if (i == 1)
                {
                    sP = P;
                }
                else
                {
                    //2018.06.20, czs， 这是推导了好几天才出的结果，根本不需要所谓的加权，此结果与上面算法相同，并得到了验证，具体公式请参看本人发表的文章：**平滑伪距***。
                    double weight = 1.0 / i;//= GetWeight(key, i);
                    sP = weight * P + (1 - weight) * (sP + L - prevL + correction);
                }

                prevL = L;
                i++;
            }
            double stdDev = RawRangeStdDev / Math.Sqrt(count);
            return new RmsedNumeral(sP, stdDev);
        }
        /// <summary>
        /// 上一次计算的AK。
        /// </summary>
        protected double PrevAk { get; set; }

        /// <summary>
        /// 原始AK计算方法，用于迭代计算结果的验证。
        /// </summary>
        /// <returns></returns>
        protected double GetAk()
        {
            var keys = this.OrderedKeys;
            double sum = 0;
            foreach (var key in keys)
            {
                var PL = this[key]; 
                sum += PL.RangeMinusPhase; 
            }
            double Ak = sum / this.Count;
            return Ak;
        }

        private RmsedNumeral GetFirstOrDefault()
        {
            if (this.Count == 0)
            {
                return RmsedNumeral.Zero;
                throw new Exception("are you kidding? you must put one value first.");
            }

            //if (this.Count == 1)
            {
                this.LastSmoothedRange = First.PseudoRange;
                this.LastPhaseRange = First.PhaseRange;

                return new RmsedNumeral(LastSmoothedRange, RawRangeStdDev);
            }
        }

        private RmsedNumeral GetResult(double ExtraP)
        {
            int count = this.Count;
            //加权
            double weight = GetWeight(this.LastKey, count);
            double sP = weight * this.CurrentData.PseudoRange + (1 - weight) * ExtraP;

            double stdDev = this.RawRangeStdDev * 0.1 / Math.Sqrt(count);


            return new RmsedNumeral(sP, stdDev);
        }

        /// <summary>
        /// 获取权值. 指的是原始P码所占权比例，0 表示不占比例。
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual  double GetWeight(Time Time, int i)
        {
            if (IsWeighted)
            {
                return 1.0 / i;
            }

            return 0;
        }


        /// <summary>
        /// 电离层拟合器,第一个历元为0开始
        /// y = I0 + bt + λN /2。 Y 包含了当前历元的电离层和一半的模糊度距离
        /// </summary>
        /// <returns></returns>
        public TimedLsPolyFit GetDeltaIonoPolyFit()
        {
            if(OrderOfDeltaIonoPolyFit > this.Count  -1) { return null; }

            Dictionary<Time, double> dic = new Dictionary<Time, double>();
            var keys = this.OrderedKeys;
            var first = keys[0];
            
            foreach (var item in keys)
            {
                var data = this[item]; 
                double y = 0.5 * data.RangeMinusPhase;
                dic.Add(item, y);
            }

            TimedLsPolyFit fit = new TimedLsPolyFit(dic, OrderOfDeltaIonoPolyFit);

            fit.Init();
            //fit.InitAndFitParams<Time>(dic, m => m.SecondsOfWeek);
            return fit;
        }

        /// <summary>
        /// 电离层拟合器返回最后或第一相邻历元的电离层偏差
        /// </summary>
        /// <returns></returns>
        public double GetDeltaIonoPolyFitValue(bool isFirstOrLast, int fitDataCount = 10)
        {
            if(this.Count < 5) //数据太少，不如不算
            {
                return 0;
            }
            if (OrderOfDeltaIonoPolyFit > this.Count - 1) { return 0; }
            if(fitDataCount > this.Count ) { fitDataCount = Count; }
            int startCount = isFirstOrLast ? 0 : Count - fitDataCount;

            LsPolyFit fit = new LsPolyFit(OrderOfDeltaIonoPolyFit);

            Dictionary<double, double> dic = new Dictionary<double, double>();
            var keys = this.OrderedKeys.GetRange(startCount, fitDataCount) ;
            

            var first = keys[0];

            foreach (var item in keys)
            {
                var data = this[item];
                double x = (item - first);
                double y = 0.5 * (data.PseudoRange - data.PhaseRange);
                dic.Add(x, y);
            }

            fit.InitAndFitParams<double>(dic, m => m);
            //fit.InitAndFitParams<Time>(dic, m => m.SecondsOfWeek);

            double firstVal = fit.GetY(0);

            if (isFirstOrLast)
            {
                double secondKey = keys[1] - first;
                double secondVal = fit.GetY(secondKey);
                double firstDiffer = secondVal - firstVal;
                return firstDiffer;
            }
            else
            {
                double lastKeyDiffer = keys.Last() - first;
                double lastSeocndKeyDiffer = keys[keys.Count - 2] - first;
                double fitVal = fit.GetY(lastKeyDiffer);
                double fitSecondVal = fit.GetY(lastSeocndKeyDiffer);
                double lastDiffer = fitVal - fitSecondVal;
                return lastDiffer;
            } 
        }
        #endregion
    }
     
}