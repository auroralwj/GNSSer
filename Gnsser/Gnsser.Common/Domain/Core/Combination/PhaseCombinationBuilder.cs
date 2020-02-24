//2014.09.14, czs, create, 观测量，Gnsser核心模型！
//2015.01.02, czs, eidt in namu，添加了大量其它组合
//2015.01.09, 崔阳, 增加, 以周为单位的无电离层组合的系数，只有以周为单位才能计算波长！！！！
//2016.04.06, czs, edit in hongqing, 进行了整理
//2017.10.23, czs, edit in hongqing, 无电离层组合，若有一个值为0，则不再组合，直接采用另一个值。
//2018.09.26, czs, edit in hmx, 重构频率


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.IO;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Times;

namespace Gnsser.Domain
{
    /// <summary>
    /// 载波相位组合构造器,构造常见的在载波或伪距组合值。
    /// </summary>
    public class PhaseCombinationBuilder
    {
        Log log = new Log(typeof(PhaseCombinationBuilder));
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="epochSatellite">站星线段</param>
        public PhaseCombinationBuilder(EpochSatellite epochSatellite)
        {
            this.EpochSat = epochSatellite;
        }

        #region 输入属性
        /// <summary>
        /// 站星线段。一颗卫星的观测值。
        /// </summary>
        public EpochSatellite EpochSat { get; protected set; }
        #endregion

        #region 双频

        #region  无电离层组合
        /// <summary>
        /// 无电离层双频伪距组合。第一和第二频率。
        /// 消电离层组合，如果只有一个频率有值，则返回一个频率的伪距。
        /// 如果都为0，则返回 0 值，请调用时判断！！。
        /// </summary>
        public PhaseCombination IonoFreeRange   {   get {   return GetIonoFreeRangeOf(FrequenceType.A, FrequenceType.B, true);    } }

        /// <summary>
        /// 无电离层双频载波相位组合,单位：米。第一和第二频率。原始数据，未对齐。
        /// 若有一个值为0，则不再组合，直接返回 0！！！！！
        /// </summary>
        public PhaseCombination IonoFreePhaseRange { get { return GetIonoFreeRangeOf(FrequenceType.A, FrequenceType.B, false); } }
       
        /// <summary>
        /// 无电离层距离距值
        /// </summary>
        /// <param name="FreqCombinationType"></param>
        /// <param name="isPsuedoOrPhaseRange">是否伪距或相位</param>
        /// <returns></returns>
        public PhaseCombination GetIonoFreeRange(FreqCombinationType FreqCombinationType,bool isPsuedoOrPhaseRange)
        {
            PhaseCombination ifvalue = null;
            switch (FreqCombinationType)
            {
                case FreqCombinationType.AB:
                    ifvalue = GetIonoFreeRangeOf(FrequenceType.A, FrequenceType.B, isPsuedoOrPhaseRange);
                    break;
                case FreqCombinationType.BC:
                    ifvalue = GetIonoFreeRangeOf(FrequenceType.B, FrequenceType.C, isPsuedoOrPhaseRange);
                    break;
                case FreqCombinationType.AC:
                    ifvalue = GetIonoFreeRangeOf(FrequenceType.A, FrequenceType.C, isPsuedoOrPhaseRange);
                    break;
                default:
                    break;
            } //伪距也用组合
            return ifvalue;
        }        
       
        #endregion

        #region LI MW 常见双频组合

        /// <summary>
        /// LI双频载波相位组合
        /// </summary>
        public PhaseCombination LiPhaseComb
        {
            get
            {
                return GetDoublePhaseRangeCombination(EpochSat, 1, -1, "Li_" + EpochSat.Prn.SatelliteType);
            }
        }
        /// <summary>
        ///宽项组合，B频段的电离层效应下降 20%
        /// </summary>
        public PhaseCombination XBandPhaseComb
        {
            get
            {
                return GetDoublePhaseRangeCombination(EpochSat, 2, -1, "XBand_" + EpochSat.Prn.SatelliteType);
            }
        }
        /// <summary>
        ///宽项组合，B频段的电离层效应下降 40%
        /// </summary>
        public PhaseCombination WideBandPhaseCycleComb
        {
            get
            {
                return GetDoublePhaseRangeCombination(EpochSat, 1, -1, "WideBand_" + EpochSat.Prn.SatelliteType);
            }
        }
        /// <summary>
        ///窄项组合
        /// </summary>
        public PhaseCombination NarrowBandPhaseCycleComb
        {
            get
            {
                return GetDoublePhaseRangeCombination(EpochSat, 1, 1, "NarrowBand_" + EpochSat.Prn.SatelliteType);
            }
        }
        /// <summary>
        /// MW组合值，单位：周。原始组合，改正了P1C1（若有）
        /// 通常作为具有卫星和接收机硬件延迟的宽巷模糊度。
        /// </summary>
        public double MwPhaseCombinationValue
        {
            get
            {
                return MwRangeCombination.Value / MwRangeCombination.Frequence.WaveLength;
            }
        }

        /// <summary>
        /// MW双频载波相位组合，单位：米(宽项)，改正了P1C1（若有）,MW 组合观测量定义为宽巷载波与窄巷伪距之差,组合后，只剩下模糊度参数。
        /// 主要用于探测周跳和求宽项模糊度值。
        /// </summary>
        public PhaseCombination MwRangeCombination
        {
            get
            {
                EpochSatellite epochSat = EpochSat;
                var freqA = epochSat.FrequenceA.Frequence;
                var freqB = epochSat.FrequenceB.Frequence;

                double f1 = epochSat.FrequenceA.Frequence.Value;
                double f2 = epochSat.FrequenceB.Frequence.Value;
                double L1 = epochSat.FrequenceA.PhaseRange.Value;
                double L2 = epochSat.FrequenceB.PhaseRange.Value;
                double P1 = epochSat.FrequenceA.PseudoRange.CorrectedValue;//改正P1C1，需要外部改正，此处只是调用而已。
                double P2 = epochSat.FrequenceB.PseudoRange.CorrectedValue;//May be P2C2

                double value = GetMwValue(f1, f2, L1, L2, P1, P2);

                double freqVal = f1 - f2; //此处采用宽项的频率，以周为单位推导波长和频率

                Frequence freqence = new Frequence("MW_" + epochSat.Prn.SatelliteType, freqVal);
                return new PhaseCombination(value, freqence);
            }
        }

        /// <summary>
        /// MW 值。
        /// </summary>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static double GetMwValue(double L1, double L2, double P1, double P2, SatelliteType type = SatelliteType.G)
        {
            double f1 = 0;
            double f2 = 0;
            switch (type)
            {
                case SatelliteType.G:
                    f1 = Frequence.GpsL1.Value;
                    f2 = Frequence.GpsL2.Value;
                    break;
                default:
                    throw new NotImplementedException("请实现 " + type);
            }
            return GetMwValue(f1, f2, L1, L2, P1, P2);
        }
        
        /// <summary>
        /// MW组合值
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public static double GetMwValue(double f1, double f2, double L1, double L2, double P1, double P2)
        {
            double e = f1 / (f1 - f2);
            double f = f2 / (f1 - f2);
            double c = f1 / (f1 + f2);
            double d = f2 / (f1 + f2);

            double value =
                e * L1
              - f * L2

              - c * P1
              - d * P2;
            return value;
        }
        

        /// <summary>
        /// MW3频码相无电离层无几何组合，单位：周(宽项)。
        /// </summary>
        public PhaseCombination MwTriFreqCombination
        {
            get
            {
                EpochSatellite epochSat = EpochSat;

                double f1 = epochSat.FrequenceA.Frequence.Value;
                double f2 = epochSat.FrequenceB.Frequence.Value;
                double L1 = epochSat.FrequenceA.PhaseRange.Value;
                double L2 = epochSat.FrequenceB.PhaseRange.Value;
                double P1 = epochSat.FrequenceA.PseudoRange.Value;
                double P2 = epochSat.FrequenceB.PseudoRange.Value;
                double f3 = epochSat.FrequenceC.Frequence.Value;
                double L3 = epochSat.FrequenceC.PhaseRange.Value;
                double P3 = epochSat.FrequenceC.PseudoRange.Value;

                int i = 0, j = 0, k = 0;
                double l = 0, m = 0, n = 0;//伪距的三个系数
                if (EpochSat.Prn.SatelliteType == SatelliteType.G)
                {
                    l = 0.012109; m = 0.444991; n = 0.542900;
                    i = 0; j = 1; k = -1;
                }
                else if (EpochSat.Prn.SatelliteType == SatelliteType.C)
                {
                    l = 0.019945; m = 0.552577; n = 0.427478;
                    i = 0; j = -1; k = 1;
                }
                else { return null; }
                double freqVal = i * f1 + j * f2 + k * f3;

                double value = i * L1 + j * L2 + k * L3 - (l * P1 + m * P2 + n * P3) * freqVal / GnssConst.LIGHT_SPEED;


                Frequence freqence = new Frequence("MwTriFreq_" + epochSat.Prn.SatelliteType, freqVal);
                return new PhaseCombination(value, freqence);
            }
        }
        /// <summary>
        /// 三频，单位：m。相位无几何[1,-1,0]
        /// </summary>
        public double TriFreqBasedOnGF1Combination
        {
            get
            {
                EpochSatellite epochSat = EpochSat;

                double f1 = epochSat.FrequenceA.Frequence.Value;
                double f2 = epochSat.FrequenceB.Frequence.Value;
                double L1 = epochSat.FrequenceA.PhaseRange.Value;
                double L2 = epochSat.FrequenceB.PhaseRange.Value;
                double f3 = epochSat.FrequenceC.Frequence.Value;
                double L3 = epochSat.FrequenceC.PhaseRange.Value;

                int i = 1, j = -1, k = 0;//相位的三个系数                

                double value = (i * L1 + j * L2 + k * L3 );
                
                 return value;
            }
        }
        /// <summary>
        /// 三频，单位：m。相位无几何[1,0,-1]
        /// </summary>
        public double TriFreqBasedOnGF2Combination
        {
            get
            {
                EpochSatellite epochSat = EpochSat;

                double f1 = epochSat.FrequenceA.Frequence.Value;
                double f2 = epochSat.FrequenceB.Frequence.Value;
                double L1 = epochSat.FrequenceA.PhaseRange.Value;
                double L2 = epochSat.FrequenceB.PhaseRange.Value;
                double f3 = epochSat.FrequenceC.Frequence.Value;
                double L3 = epochSat.FrequenceC.PhaseRange.Value;

                int i = 1, j = 0, k = -1;//相位的三个系数                

                double value = (i * L1  + j * L2 + k * L3 ) ;

                return value;
            }
        }  
        #endregion
        #endregion
         

        #region 3频
        /// <summary>
        /// 无电离层3频伪距组合
        /// </summary>
        public PhaseCombination IonoFreeRangeThreeFrequency
        {
            get
            {
                EpochSatellite epochSat = EpochSat;
                Frequence freqence = GetIonoFreeCompositBandThreeFrequency(epochSat);
                double value = GetIonoFreeComValue(
                    epochSat.FrequenceA.PseudoRange.CorrectedValue,
                    epochSat.FrequenceB.PseudoRange.CorrectedValue,
                    epochSat.FrequenceC.PseudoRange.CorrectedValue,
                    epochSat);
                return new PhaseCombination(value, freqence);
            }
        }
         
        /// <summary>
        /// 无电离层3频载波相位组合
        /// </summary>
        public PhaseCombination IonoFreePhaseRangeThreeFrequency
        {
            get
            {
                EpochSatellite epochSat = EpochSat;
                Frequence freqence = GetIonoFreeCompositBandThreeFrequency(epochSat);
                double value = GetIonoFreeComValue(
                    epochSat.FrequenceA.PhaseRange.CorrectedValue,
                    epochSat.FrequenceB.PhaseRange.CorrectedValue,
                    epochSat.FrequenceC.PhaseRange.CorrectedValue,
                    epochSat);
                return new PhaseCombination(value, freqence);
            }
        } 
        #endregion 

        #region 计算工具,主要是双频无电离层组合的计算工具

        #region 双频

     
        /// <summary>
        /// 无电离层距离组合
        /// </summary>
        /// <param name="range1">距离1</param>
        /// <param name="range2">距离2</param>
        /// <param name="A">频率1</param>
        /// <param name="B">频率2</param>
        /// <returns></returns>
        public static PhaseCombination GetIonoFreeRangeCombination(double range1, double range2, Frequence A, Frequence B)
        {
            double[] facs = GetIonoFreeRangeCombFactors(A, B);
            double fac1 = facs[0];
            double fac2 = facs[1]; 
            double range = GetCombinationValue(fac1, range1, fac2, range2); 
            var freq = GetIonoFreeComPhaseFrequence(A, B);

            return new PhaseCombination(range, freq);
        }
         /// <summary>
         /// 无电离层相位组合，单位:周。
         /// </summary>
         /// <param name="phaseA"></param>
         /// <param name="phaseB"></param>
         /// <param name="A"></param>
         /// <param name="B"></param>
         /// <returns></returns>
        public static PhaseCombination GetIonoFreePhaseCycleCombination(double phaseA, double phaseB, Frequence A, Frequence B)
        {
            double[] facs = GetIonoFreePhaseCycleCombFactors(A, B);
            double fac1 = facs[0];
            double fac2 = facs[1]; 
            double val = GetCombinationValue(fac1, phaseA, fac2, phaseB); 
            var freq = GetIonoFreeComPhaseFrequence(A, B);

            return new PhaseCombination(val, freq);
        }

        /// <summary>
        /// 获取无电离层组合量,双频无电离层距离组合,返回已改正后的值。s
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <param name="isPsuedoOrPhaseRange">是否伪距或相位距离</param>
        /// <returns></returns>
        public PhaseCombination GetIonoFreeRangeOf(FrequenceType type1, FrequenceType type2, bool isPsuedoOrPhaseRange)
        {
            var freA = EpochSat[type1];
            var freB = EpochSat[type2];
            Frequence A = freA.Frequence;
            Frequence B = freB.Frequence;
            Frequence freqence = GetIonoFreeComPhaseFrequence(A, B);

            double rangeA = 0;
            double rangeB = 0; 
            if (isPsuedoOrPhaseRange)
            {
                rangeA =  freA.PseudoRange.CorrectedValue;
                rangeB =  freB.PseudoRange.CorrectedValue;
            }
            else
            {
                rangeA = freA.PhaseRange.CorrectedValue;
                rangeB = freB.PhaseRange.CorrectedValue;
            }
            if (rangeA == 0)
            {
                if (rangeB == 0)
                {
                    log.Debug( this.EpochSat.Prn + ", " +this.EpochSat.ReceiverTime + ", 载波 “"+type1+ "”和载波 “" + type2 + "”的"+(isPsuedoOrPhaseRange?"伪距":"载波") +"都为 0 。");
                    return new PhaseCombination(0, freqence);
                    throw new ArgumentNullException("载波 1 和载波 2 的伪距都为 0 。");
                }
                else  {  return new PhaseCombination(rangeB, freqence);  }
            }
            else if (rangeB == 0)    { return new PhaseCombination(rangeA, freqence);   }

            double[] facs = GetIonoFreeRangeCombFactors(A, B);
            double fac1 = facs[0];
            double fac2 = facs[1];
            double range = GetCombinationValue(fac1, rangeA, fac2, rangeB);        
            //伪距也用组合
            var val = new PhaseCombination(range, freqence); 
            return val;
        }

        #region 计算频率
          
        /// <summary>
        /// 无电离层的等效组合频率（非真的频率）。
        /// 单位为周的频率。
        /// </summary>
        /// <param name="bandA">频率1</param>
        /// <param name="bandB">频率2</param>
        /// <returns></returns>
        public static Frequence GetIonoFreeComPhaseFrequence(Frequence bandA, Frequence bandB)
        {
            return GetIonoFreeFrequence(bandA, bandB);
            //频率采用相位组合
            var factors = GetIonoFreeRangeCombFactors(bandA, bandB);

            string name = bandA.Name + bandB.Name + "_IonoFree";

            return GetCompositFreqence(factors[0], bandA, factors[1], bandB, name);
        }

        public static Frequence KycGetIonoFreeComPhaseFrequence(Frequence bandA, Frequence bandB)
        {
            //频率采用相位组合
            var factors = KycGetIonoFreeRangeCombFactors(bandA, bandB);

            string name = bandA.Name + bandB.Name + "_IonoFree";

            return GetCompositFreqence(factors[0], bandA, factors[1], bandB, name);
        }
        #endregion
         
         
        #region 计算无电离层系数
         

        /// <summary>
        /// 两个频率的无电离层组合参数。适用于距离组合计算。
        /// 含伪距距离，也包含载波对应的距离。
        /// 若是GPS L1 和 L2 ，数值为 2.54 和 -1.54
        /// </summary>
        /// <param name="A">频率A</param>
        /// <param name="B">频率B</param>
        /// <returns></returns>
        public static double[] GetIonoFreeRangeCombFactors(Frequence A, Frequence B)
        {
            double f1 = A.Value;
            double f2 = B.Value;

            double f1_2 = f1 * f1;
            double f2_2 = f2 * f2;
            double down = (f1_2 - f2_2);

            double factor1 = f1_2 / down;
            double factor2 = 1 - factor1; //已标准化，相加为 1.

            return new double[] { factor1, factor2 };
        }
        /// <summary>
        /// 无电离层组合频率，参加许老师《GPS理论算法与应用》P85
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Frequence GetIonoFreeFrequence(Frequence A, Frequence B)
        {
            return A;
            double f1 = A.Value * 1e6;
            double f2 = B.Value * 1e6;

            double f1_2 = f1 * f1;
            double f2_2 = f2 * f2;
            double newFreqValue = GnssConst.LIGHT_SPEED / (f1_2 - f2_2) / 1e6;

            var freq = new Frequence("IonoFreeOf" + A + "_" + B, newFreqValue);
            return freq;
        }
        /// <summary>
        ///  kyc:双频载波相位平滑伪距中两个频率的无电离层组合参数。适用于距离组合计算。
        ///  含伪距距离，也包含载波对应的距离。
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double[] KycGetIonoFreeRangeCombFactors(Frequence A, Frequence B)
        {
            double f1 = A.Value;
            double f2 = B.Value;

            double f1_2 = f1 * f1;
            double f2_2 = f2 * f2;
            double down = (f1_2 - f2_2);



            //kyc :双频载波相位平滑伪距
            double factor1 = 1 + 2 * f2_2 / down;
            double factor2 = -2 * f2_2 / down;//P1

            //double factor1 = 2 + 2 * f2_2 / down;
            //double factor2 = -1-2 * f2_2 / down;//P2

            return new double[] { factor1, factor2 };
        }
        /// <summary>
        /// 2015.1.9 ，崔阳， 增加 以周为单位的无电离层组合的系数，只有以周为单位才能计算波长！！！！
        /// 载波线性组合法的两个参数。 m和n。 P = m * p1 + n * p2. 
        /// 系统前两个频率的无电离层组合系数。对于北斗是 L1=E2，L2=E6的组合。
        /// 相位组合（以周为单位）.
        /// 备注：这是以轴为单位的组合。
        /// 整周未知数的系数,组合值为加号。
        /// 分解为：N1 和 delta N = N1 - N2。
        /// 见李征航 GPS测量数据处理 P101。
        /// 若是GPS L1 和 L2，数值为 2.54 和 -1.98
        /// </summary>
        /// <param name="bandA">频率1</param>
        /// <param name="bandB">频率2</param>
        /// <returns></returns>
        public static double[] GetIonoFreePhaseCycleCombFactors(Frequence bandA, Frequence bandB)
        {
            double f1 = bandA.Value;
            double f2 = bandB.Value;
            double f1_2 = f1 * f1;
            double f2_2 = f2 * f2;
            double down = (f1_2 - f2_2);

            double factor1 = f1_2 / down;
            double factor2 = -f1 * f2 / down;//以周为单位的载波系数，只有以周为单位的观测值才存在波长。
            //与距离组合存在着 f1/f2 的关系
               // double factor2 = 1 - factor1; //已标准化，相加为 1.
            return new double[] { factor1, factor2 };
        }

        #endregion

        #endregion

        #region 3 频
         
        /// <summary>
        /// 3频无电离层组合,返回已改正后的值。
        /// </summary>
        /// <param name="range1">载波或伪距1</param>
        /// <param name="range2">载波或伪距2</param>
        /// <param name="range3">载波或伪距3</param>
        /// <returns></returns>
        public static double GetIonoFreeComValue(double range1, double range2, double range3, EpochSatellite sat)
        {
            double range;

            double[] facs = GetIonoFreeCombFactorsThreeFrequency(sat);
            double fac1 = facs[0];
            double fac2 = facs[1];
            double fac3 = facs[2];

            range = fac1 * range1 + fac2 * range2 + fac3 * range3;

            return range;
        }
        /// <summary>
        /// 指定系统的无电离层组合
        /// </summary>
        /// <param name="type">系统类型</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Frequence GetIonoFreeCompositBandThreeFrequency(EpochSatellite sat, string name = null)
        {
            double[] facs = GetIonoFreeCombFactorsThreeFrequency(sat);

            Frequence bandA = sat.FrequenceA.Frequence;// Frequence.GetFrequenceA(sat.Prn, sat.ReceiverTime);
            Frequence bandB = sat.FrequenceA.Frequence;// Frequence.GetFrequenceB(sat.Prn, sat.ReceiverTime);
            Frequence bandC = sat.FrequenceA.Frequence;// Frequence.GetFrequenceC(sat.Prn, sat.ReceiverTime);
            name = name ?? sat + "_" + bandA.Name + bandB.Name + bandC.Name + "_IonoFree";

            return GetCompositFreqenceBandThreeFrequency(facs[0], bandA, facs[1], bandB, facs[2], bandC, name);
        }

        /// <summary>
        /// 3个频率组成新的频率。
        /// </summary>
        /// <param name="factorA"></param>
        /// <param name="bandA"></param>
        /// <param name="factorB"></param>
        /// <param name="bandB"></param>
        /// <param name="factorC"></param>
        /// <param name="bandC"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Frequence GetCompositFreqenceBandThreeFrequency(double factorA, Frequence bandA, double factorB, Frequence bandB, double factorC, Frequence bandC, string name = null)
        {
            double frequence = (factorA * bandA.Value + factorB * bandB.Value + factorC * bandC.Value);// / (factorA + factorB);
            //   double frequence = bandA.Freqence + bandB.Freqence;

            name = name ?? bandA.Name + "(" + factorA.ToString("0.00") + ")" + "_" + bandB.Name + "(" + factorB.ToString("0.00") + ")" + "_" + bandC.Name + "(" + factorC.ToString("0.00") + ")";

            Frequence band = new Frequence(name, frequence);
            return band;
        }
        /// <summary>
        /// 三频无电离层组合
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public static double[] GetIonoFreeCombFactorsThreeFrequency2(EpochSatellite sat)
        {
            double f1 = sat.FrequenceA.Frequence.Value;// Frequence.GetFrequence(sat, 1).Value;
            double f2 = sat.FrequenceB.Frequence.Value;// Frequence.GetFrequence(sat, 2).Value;
            double f3 = sat.FrequenceC.Frequence.Value;// Frequence.GetFrequence(sat, 3).Value;

            double f1_2 = f1 * f1;
            double f2_2 = f2 * f2;
            double f3_2 = f3 * f3;

            double f1_3 = f1 * f1 * f1;
            double f2_3 = f2 * f2 * f2;
            double f3_3 = f3 * f3 * f3;

            double factorUp1 = (f1_3 * f1) * (f3 - f2) / (f3 - f1) / (f2 - f1);
            double factorUp2 = -(f2_3 * f1) / (f2 - f1);
            double factorUp3 = (f3_3 * f1) / (f3 - f1);

            double down = factorUp1 + factorUp2 + factorUp3;

            double factor1 = factorUp1 / down;
            double factor2 = factorUp2 / down;
            double factor3 = factorUp3 / down;
            //double frequence = factor1 * FreqenceBand.GpsL1.Freqence + factor2 * FreqenceBand.GpsL2.Freqence;
            //double waveLen = GnssConst.LIGHT_SPEED / frequence / 1e6;

            return new double[] { factor1, factor2, factor3 };
        }
        /// <summary>
        /// 三频无电离层组合系数
        /// </summary>
        /// <param name="sat"></param>
        /// <returns></returns>
        public static double[] GetIonoFreeCombFactorsThreeFrequency(EpochSatellite  sat)
        {
            double f1 = sat.FrequenceA.Frequence.Value;// Frequence.GetFrequence(gnssType, 1).Value;
            double f2 = sat.FrequenceB.Frequence.Value;// Frequence.GetFrequence(gnssType, 2).Value;
            double f3 = sat.FrequenceC.Frequence.Value;// Frequence.GetFrequence(gnssType, 3).Value;


            double f1_2 = f1 * f1;
            double f2_2 = f2 * f2;
            double f3_2 = f3 * f3;

            double f1_3 = f1 * f1 * f1;
            double f2_3 = f2 * f2 * f2;
            double f3_3 = f3 * f3 * f3;

            double factor4 = f1_3 * (f3 - f2) - f2_3 * (f3 - f1) + f3_3 * (f2 - f1);

            double factor1 = f1_3 * (f3 - f2) / factor4;
            double factor2 = -f2_3 * (f3 - f1) / factor4;
            double factor3 = f3_3 * (f2 - f1) / factor4;

            //波长 = c / factor4;
            //N = C4 * ( C1 * N1 + C2 * N2 + C3 * N3 );

            return new double[] { factor1, factor2, factor3 };
        }

        #endregion

        #endregion

        #region 通用工具方法
        /// <summary>
        /// 双频载波距离组合生成,返回相位组合，单位为米，如LI组合等。
        /// </summary>
        /// <param name="epochSat">卫星</param>
        /// <param name="factor1">系数</param>
        /// <param name="factor2">系数</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static PhaseCombination GetDoublePhaseRangeCombination(EpochSatellite epochSat, double factor1, double factor2, string name = "")
        {
            if(epochSat.FrequencyCount < 2)
            {
                throw new Exception("频率数量必须大于等于2！" + epochSat.SiteInfo.SiteName + ", " + epochSat);
            }
            if (name == "") { name = epochSat.Prn.SatelliteType + "_" + factor1 + "_" + factor2; }
            //距离系数和载波相位系数能不能一样？？？？？？？？？？是一个问题。2018.09.01，czs, hmx,
            Frequence freqence = GetDoubleCompositFreqence(epochSat, factor1, factor2, name);

            double valA = epochSat.FrequenceA.PhaseRange.Value;
            double valB = epochSat.FrequenceB.PhaseRange.Value;


            double value = GetCombinationValue(factor1, valA, factor2, valB);
            return new PhaseCombination(value, freqence);
        }
        /// <summary>
        /// 提供便捷的AB频率无电离层组合。
        /// </summary>
        /// <param name="rangeA">伪距或载波距离</param>
        /// <param name="rangeB">伪距或载波距离</param>
        /// <param name="prn"></param>
        /// <param name="receiverTime"></param>
        /// <returns></returns>
        public static double GetIonoFreeRangeValue(double rangeA, double rangeB, SatelliteNumber prn, Time receiverTime)
        {
            var A = Frequence.GetFrequenceA(prn, receiverTime);
            var B = Frequence.GetFrequenceB(prn, receiverTime); 
            double[] facs = GetIonoFreeRangeCombFactors(A, B);
            double fac1 = facs[0];
            double fac2 = facs[1];
            double range = GetCombinationValue(fac1, rangeA, fac2, rangeB); 
            return range;
        }

        /// <summary>
        /// 双频组合后的频率
        /// </summary>
        /// <param name="epochSat"></param>
        /// <param name="factorA">载波相位系数</param>
        /// <param name="factorB">载波相位系数</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Frequence GetDoubleCompositFreqence(EpochSatellite epochSat, double factorA, double factorB, string name = null)
        {
            return GetCompositFreqence(factorA, epochSat.FrequenceA.Frequence, factorB, epochSat.FrequenceB.Frequence, name);
        }
        /// <summary>
        /// 两个频率组成新的频率。简单的线性组合
        /// 频率和载波相位都可以直接相加，而距离需要转换。
        /// </summary>
        /// <param name="factorA">系数A</param>
        /// <param name="bandA">频率A</param>
        /// <param name="factorB">系数B</param>
        /// <param name="bandB">频率B</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static Frequence GetCompositFreqence(double factorA, Frequence bandA, double factorB, Frequence bandB, string name = null)
        {
            return Frequence.GetCompositFreqence(factorA, bandA, factorB, bandB, name); 
        }

        /// <summary>
        /// 两个数组成新的数。简单的线性组合
        /// </summary>
        /// <param name="factorA">系数A</param>
        /// <param name="valA">数值A</param>
        /// <param name="factorB">系数B</param>
        /// <param name="valB">数值B</param>
        /// <returns></returns>
        public static double GetCombinationValue(double factorA, double valA, double factorB, double valB)
        {
            double combined = (factorA * valA + factorB * valB);
            return combined;
        }

        #endregion
    }
}