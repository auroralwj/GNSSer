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
    /// 平滑算法叠加类型
    /// </summary>
    public enum SmoothRangeSuperpositionType
    {
        /// <summary>
        /// 快速更新计算，但是电离层精度不一样
        /// </summary>
        快速更新算法,
        /// <summary>
        /// 等价于窗口原始迭代
        /// </summary>
        窗口AK迭代,
        /// <summary>
        /// 等价于窗口原始迭代
        /// </summary>
        窗口原始迭代

    }  
    /// <summary>
    /// 平滑伪距
    /// </summary>
    public class SmoothRangeWindow : WindowData<Time, RangePhasePair>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="windowSize"></param>
        /// <param name="IsWeighted"></param>
        /// <param name="IsDeltaIonoCorrect"></param>
        /// <param name="OrderOfDeltaIonoPolyFit"></param>
        /// <param name="SmoothRangeType"></param>
        public SmoothRangeWindow(int windowSize, bool IsWeighted = true, bool IsDeltaIonoCorrect = true, int OrderOfDeltaIonoPolyFit = 2, int bufferSize = 30, SmoothRangeSuperpositionType SmoothRangeType = SmoothRangeSuperpositionType.窗口AK迭代)
            : base(windowSize)
        {
            CurrentData = new RangePhasePair(0,0, 0);
            this.IsWeighted = IsWeighted; 
            this.MaxKeyGap = 151;
            this.RawRangeStdDev = 2;
            this.SmoothRangeType = SmoothRangeType; 
            this.PrevData = new RangePhasePair(0, 0, 0); 
             

        }
        #region  属性 
        #region 电离层改正  

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
        /// 增加数据
        /// </summary>
        /// <param name="rangePhasePair"></param>
        /// <returns></returns>
        public SmoothRangeWindow Add(RangePhasePair rangePhasePair)
        { //通过是否相等，判断是否重复设值
            if (CurrentData.PseudoRange == rangePhasePair.PseudoRange && this.CurrentData.PhaseRange == rangePhasePair.PhaseRange)
            {
                return this;
            }
            this.CurrentData = rangePhasePair;
            this.Add(rangePhasePair.Time, CurrentData);
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
            }
            else //采用窗口内的数据进行平滑
            { 
                //所谓的推估表达式，即不采用当前的伪距观测数据，    P0 + (L1 - L0) + 2DI
                extraP = LastSmoothedRange + (this.CurrentData.IonoFittedPhaseRange - PrevData.IonoFittedPhaseRange) ;

                double sP = extraP;
                if (IsWeighted)//加权，表示采用当前的伪距观测数据
                {
                    if (this.LastRemovedItem.Value != null)//当大于指定的窗口，将扣除窗口外的数据（包括电离层延迟）影响。
                    {
                        double ionoDiffer =  CurrentData.FittedIonoAndAmbiValue - this.LastRemovedPair.FittedIonoAndAmbiValue;
                         
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
            double Ak =  GetAk( );

            double sP = Ak + this.CurrentData.IonoFittedPhaseRange;

            double stdDev = this.RawRangeStdDev * 0.1 / Math.Sqrt(count);
            //此处不用加权了
            //2018.06.20, czs， 这是推导了好几天才出的结果，并得到了验证，具体公式请参看本人发表的文章：**平滑伪距***。
            return new RmsedNumeral(sP, stdDev);
        }

        private double GetAk( )
        { 
            var keys = this.OrderedKeys;
            var firstKey = keys[0];

            double firstIonoAndHalfLambdaLen = this[firstKey].FittedIonoAndAmbiValue;
            //采用窗口内的数据进行平滑  
            double sum = 0;
            foreach (var key in keys)
            {
                var PL = this[key];
                //问：一定要从初始窗口开始吗？从第一历元开始应该也是可以的，或者让 firstIonoAndHalfLambdaLen = 0。？？？？？
                sum += PL.RangeMinusIonoFittedPhase - 2 * firstIonoAndHalfLambdaLen;
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
             
            var keys = this.OrderedKeys;
            var firstKey = keys[0];

            double firstIonoAndHalfLambdaLen = this[firstKey].FittedIonoAndAmbiValue;
            double prevIonoNLen = firstIonoAndHalfLambdaLen;
            RangePhasePair prev = null;
            RangePhasePair current = null;

            //采用窗口内的数据进行平滑
            double sP = 0;//最后的结果 
            int i = 1;
            foreach (var key in keys)
            {
                current = this[key];  

                if (i == 1)
                {
                    sP = current.PseudoRange;
                }
                else
                {
                    //2018.06.20, czs， 这是推导了好几天才出的结果，根本不需要所谓的加权，此结果与上面算法相同，并得到了验证，具体公式请参看本人发表的文章：**平滑伪距***。
                    double weight = 1.0 / i;//= GetWeight(key, i);
                    sP = weight * current.PseudoRange + (1 - weight) * (sP + current.IonoFittedPhaseRange - prev.IonoFittedPhaseRange);
                }
                 
                prev = current;
                i++;
            }
            double stdDev = RawRangeStdDev / Math.Sqrt(count);
            return new RmsedNumeral(sP, stdDev);
        }
        /// <summary>
        /// 上一次计算的AK。
        /// </summary>
        protected double PrevAk { get; set; }
         

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

         
 
        #endregion
    }
     
}