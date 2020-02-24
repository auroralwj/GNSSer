
// 2015.05.24, cy ,基于无电离层组合的双差的模糊度固定,LC-DD-AR
//2015.10.14, cy ,重命名，修复bug
//2015.10.18, cy, 该类仅是GPS卫星30秒采样率数据的无电离层模糊度固定
//2015.10.19, cy, 重新写类，不直接求单差，基于原始
//2016.11.25, cy, 参数优化

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Times;
using Gnsser.Times;
using Gnsser.Service;
using Gnsser.Domain;
using Gnsser.Checkers;
using Gnsser.Models;
using Gnsser.Data.Rinex;

namespace Gnsser.Service
{
    /// <summary>
    /// 模糊度固定，仅对GPS系统
    /// </summary>
    public class DoubleIonFreeAmbiguityReslution
    {
        #region 参数配置
        /// <summary>
        /// min arc gap(s)
        /// </summary>
        public double MinArcGap = 31.1; //min arc gap(s)
                                        //public double ConstraintToFixedAmbiguity = 0.001;  //constraint to fixed ambiguity
                                        //public double ThresasholdOfResiduals = 0.3;  //threashold of residuals test (m)
                                        ///// <summary>
                                        ///// Llog(pi)
                                        ///// </summary>
                                        //public double LogPi = 1.14472988584940017;  //log(pi)

        //以下参数需人工设置
        //measurements errors(1-sigma)
        double err1 = 0.003;    //err[1] errphase m
        double err2 = 0.003;   //err[2]  errphaseel m
        double err3 = 0;       //err[3] errphasebl m/10km
        double err4 = 10;      //err[4] errdoppler Hz

        double std0 = 30;     //std[0] stdbias m
        double std1 = 0.03;   //std[1] stdiono m
        double std2 = 0.3;    //std[2] stdtrpp m

        double eratio0 = 100.0; //eratio[0]   code_phase_error_ratio_L1
        double eratio1 = 100.0; //eratio[1]  code_phase_error_ratio_L2

        double thresar0 = 3;  //thresar[0] arthres  min_ration_to_fix_ambiguity
        //double min_ration_to_fix_ambiguity = 3;
        double thresar1 = 0.999;  //min_confidence_to_fix_amb
        double min_confidence_to_fix_amb = 0.9999;
        double thresar2 = 0.2;//max_FCB_to_fix_amb
        //double max_FCB_to_fix_amb = 0.2;

        #endregion

        /// <summary>
        /// 记忆中的基准星，即每次把基准星存起，备新历元计算时核查
        /// </summary>
        public SatelliteNumber LastBasePrn { get; set; }
        /// <summary>
        /// 基础参数的总数，即除了模糊度的剩余参数的个数
        /// 长基线时是5，即三个坐标参数+两个对流层参数
        /// 短基线时是3，即三个坐标参数
        /// </summary>
        public int BaseParamCount { get; set; }

        public SatelliteNumber CurrentBasePrn { get; set; }

        /// <summary>
        /// 存储之前（上一个历元）已经固定成功的模糊度
        /// </summary>
        public Vector FixIonFreeAmbiCyclesInfo { get; set; }
        /// <summary>
        /// 如果固定的模糊度子集维数小于4，则该历元所有模糊度采用实数解
        /// </summary>
        public int minFixedAmbiCount { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DoubleIonFreeAmbiguityReslution(int baseParamCount = 5)
        {

            //默认为5
            this.BaseParamCount = baseParamCount;
            this.LastBasePrn = SatelliteNumber.Default;
            //存储非差MW
            DoubleDifferWideLaneAmbiCyclesDic = new SortedDictionary<string, DDWideLineAmbiCycles>();

            //存储已经成功固定的模糊度
            FixIonFreeAmbiCyclesInfo = new Vector();
            //前后历元的时间间隔应等于采样率，否则有问题
            MinArcGap = 31.1;
            //如果固定的模糊度子集维数小于4，则该历元所有模糊度采用实数解
            minFixedAmbiCount = 3;
        }

        /// <summary>
        /// 逐历元进行双差模糊度固定和更新
        /// 目前只考虑算GPS的
        /// </summary>
        /// <param name="rovReceiverInfo"></param>
        /// <param name="refReceeiverInfo"></param>
        /// <param name="adjustment"></param>
        /// <param name="currentBasePrn"></param>
        /// <returns></returns>
        public WeightedVector Process(EpochInformation rovReceiverInfo, EpochInformation refReceeiverInfo, AdjustResultMatrix adjustment, SatelliteNumber currentBasePrn)
        {
            double Interval = rovReceiverInfo.ObsInfo.Interval;
            if (Interval == 0)
            {
                MinArcGap = 31.1;
            }
            else
            {
                MinArcGap = Interval + 1.1;
            }


            this.CurrentBasePrn = currentBasePrn;

            //如何模糊度子集维数小于4，则该历元所有模糊度采用实数解，参考论文
            //如果基准星发生变化，用于存储固定模糊度的字典应全部清空
            if (currentBasePrn != LastBasePrn || rovReceiverInfo[currentBasePrn].IsUnstable == true || refReceeiverInfo[currentBasePrn].IsUnstable == true)
            {
                DoubleDifferWideLaneAmbiCyclesDic = new SortedDictionary<string, DDWideLineAmbiCycles>();

                FixIonFreeAmbiCyclesInfo = new Vector();
                LastBasePrn = currentBasePrn;
            }
            //存储基准星
            LastBasePrn = currentBasePrn;

            WeightedVector fixIonFreeAmbiCycles = new WeightedVector();
            if (FixIonFreeAmbiCyclesInfo.Count > 0) //对曾经已经固定过的双差模糊度，如果当前双差卫星状态没有发生变化，则直接传递过来。
            {
                for (int i = 0; i < rovReceiverInfo.EnabledPrns.Count; i++)
                {
                    SatelliteNumber item = rovReceiverInfo.EnabledPrns[i];
                    if (item != currentBasePrn)
                    {
                        string paramName = item.ToString() + "-" + CurrentBasePrn + Gnsser.ParamNames.PhaseLengthSuffix;
                        if (FixIonFreeAmbiCyclesInfo.ParamNames.Contains(paramName))
                        {
                            if (rovReceiverInfo[item].IsUnstable == true || rovReceiverInfo[currentBasePrn].IsUnstable == true
                                || refReceeiverInfo[item].IsUnstable == true || refReceeiverInfo[currentBasePrn].IsUnstable == true)
                            {
                                continue;
                            }
                            else
                            {
                                int index = FixIonFreeAmbiCyclesInfo.ParamNames.IndexOf(paramName);
                                fixIonFreeAmbiCycles.Add(FixIonFreeAmbiCyclesInfo.Data[index], FixIonFreeAmbiCyclesInfo.ParamNames[index]);
                            }
                        }
                    }
                }
            }


            if (!rovReceiverInfo.SatelliteTypes.Contains(SatelliteType.G) || !refReceeiverInfo.SatelliteTypes.Contains(SatelliteType.G))
            {
                FixIonFreeAmbiCyclesInfo = fixIonFreeAmbiCycles;
                if (fixIonFreeAmbiCycles.Count > minFixedAmbiCount)
                {
                    return fixIonFreeAmbiCycles;
                }
                else
                { return null; }
            }

            Vector fixedIntWideLineAmbiCycles = null;

            if (!tryFixWideLaneAmbguity(rovReceiverInfo, refReceeiverInfo, out fixedIntWideLineAmbiCycles))
            {
                //宽巷固定失败
                FixIonFreeAmbiCyclesInfo = fixIonFreeAmbiCycles;
                if (fixIonFreeAmbiCycles.Count > minFixedAmbiCount)
                {
                    return fixIonFreeAmbiCycles;
                }
                else
                { return null; }
            }

            //如何模糊度子集维数小于4，则该历元所有模糊度采用实数解
            if (fixedIntWideLineAmbiCycles.Count < minFixedAmbiCount)//1
            {
                FixIonFreeAmbiCyclesInfo = fixIonFreeAmbiCycles;
                if (fixIonFreeAmbiCycles.Count > minFixedAmbiCount)
                {
                    return fixIonFreeAmbiCycles;
                }
                else
                { return null; }
            }

            //fix narrow-lane ambiguity

            //AR mode: PPP_AR
            // stat = fxi_amb_NL_ROUND(rovReceiverInfo, Adjustment, sat1, sat2, NW, m);

            //AR mode: PPP_AR ILS 
            if (!tryFixNarrowLaneAmbiguityByLAMBDA(rovReceiverInfo, refReceeiverInfo, adjustment, fixedIntWideLineAmbiCycles, ref fixIonFreeAmbiCycles))
            {
                //窄巷固定失败
                FixIonFreeAmbiCyclesInfo = fixIonFreeAmbiCycles;
                if (fixIonFreeAmbiCycles.Count > minFixedAmbiCount)
                {
                    return fixIonFreeAmbiCycles;
                }
                else
                { return null; }
            }
            //存储已经固定成功的模糊度
            if (fixIonFreeAmbiCycles.Count > FixIonFreeAmbiCyclesInfo.Count)
            {
                string ssss = "hahahahaha";
            }
            FixIonFreeAmbiCyclesInfo = fixIonFreeAmbiCycles;

            //for (int i = 0; i < fixIonFreeAmbiCycles.Count; i++)
            //{
            //    var newParam = fixIonFreeAmbiCycles.ParamNames[i];
            //    var newFixedValue = fixIonFreeAmbiCycles.Data[i];
            //    if (!FixIonFreeAmbiCyclesInfo.ParamNames.Contains(newParam))
            //    { FixIonFreeAmbiCyclesInfo.Add(newFixedValue, newParam); }
            //    else
            //    {
            //        int index = FixIonFreeAmbiCyclesInfo.ParamNames.IndexOf(newParam);
            //        double oldFixedValue = FixIonFreeAmbiCyclesInfo.Data[index];
            //        if (oldFixedValue != newFixedValue)
            //        {
            //            FixIonFreeAmbiCyclesInfo[index] = newFixedValue;
            //        }
            //    }
            //}

            if (fixIonFreeAmbiCycles.Count > minFixedAmbiCount)
            {
                return fixIonFreeAmbiCycles;
            }
            else
            { return null; }
        }


        /// <summary>
        /// 固定宽巷模糊度（fix wide-lane ambiguity）,以周为单位,波长是86cm
        /// 通过原始观测值构建宽巷观测值，取整固定
        /// </summary>
        /// <param name="rovReceiverInfo"></param>
        /// <param name="refReceiverInfo"></param>
        /// <param name="fixedIntWideLineAmbiCycles"></param>
        /// <returns></returns>
        private bool tryFixWideLaneAmbguity(EpochInformation rovReceiverInfo, EpochInformation refReceiverInfo, out Vector fixedIntWideLineAmbiCycles)
        {
            Vector Vector = new Vector();
            fixedIntWideLineAmbiCycles = new Vector();
            List<string> paraNames = new List<string>();

            foreach (var prn in rovReceiverInfo.EnabledPrns)
            {
                if (prn == CurrentBasePrn) continue;
                string paramName = prn.ToString() + "-" + CurrentBasePrn + Gnsser.ParamNames.PhaseLengthSuffix;
                if (!DoubleDifferWideLaneAmbiCyclesDic.ContainsKey(paramName)) { DoubleDifferWideLaneAmbiCyclesDic.Add(paramName, new DDWideLineAmbiCycles()); }

                EpochSatellite rovRoveSatInfo = rovReceiverInfo[prn];
                EpochSatellite rovBaseSatInfo = rovReceiverInfo[CurrentBasePrn];
                EpochSatellite refRoveSatInfo = refReceiverInfo[prn];
                EpochSatellite refBaseSatInfo = refReceiverInfo[CurrentBasePrn];

                //新弧段的判断，若有周跳、断裂等发生，重新计算新弧段MW平均值
                if (rovRoveSatInfo.IsUnstable == true || rovBaseSatInfo.IsUnstable == true
                    || refRoveSatInfo.IsUnstable == true || refBaseSatInfo.IsUnstable == true
                    || Math.Abs(DoubleDifferWideLaneAmbiCyclesDic[paramName].lastEpoch - rovRoveSatInfo.RecevingTime) > MinArcGap)
                {
                    DoubleDifferWideLaneAmbiCyclesDic[paramName] = new DDWideLineAmbiCycles();//是清零，而后再继续
                }

                if (rovRoveSatInfo.Polar.Elevation < 30 || rovBaseSatInfo.Polar.Elevation < 30
                    || refRoveSatInfo.Polar.Elevation < 30 || refBaseSatInfo.Polar.Elevation < 30
                    )//|| Math.Abs(DoubleDifferWideLaneAmbiCyclesDic[paramName].lastEpoch - rovRoveSatInfo.RecevingTime) > MinArcGap
                {
                    continue; //是继续，而不是清零！
                }

                //double SatCurrentMwValue = SatCurrentInfo.Combinations.MwPhaseCombinationValue; //以周为单位,波长是86cm
                //var freqA = SatCurrentInfo.FrequenceA.Frequence;
                //var freqB = SatCurrentInfo.FrequenceB.Frequence;

                //double f1 = SatCurrentInfo.FrequenceA.Frequence.Value;
                //double f2 = SatCurrentInfo.FrequenceB.Frequence.Value;
                //double L1 = SatCurrentInfo.FrequenceA.PhaseRange.Value;
                //double L2 = SatCurrentInfo.FrequenceB.PhaseRange.Value;
                //double P1 = SatCurrentInfo.FrequenceA.PseudoRange.Value;
                //double P2 = SatCurrentInfo.FrequenceB.PseudoRange.Value;

                //double L11 = SatCurrentInfo.FrequenceA.PhaseRange.CorrectedValue + SatCurrentInfo.ApproxPhaseRangeA.Correction;
                //double L22 = SatCurrentInfo.FrequenceB.PhaseRange.CorrectedValue + SatCurrentInfo.ApproxPhaseRangeB.Correction;
                //double P11 = SatCurrentInfo.FrequenceA.PseudoRange.CorrectedValue + SatCurrentInfo.ApproxPseudoRangeA.Correction;
                //double P22 = SatCurrentInfo.FrequenceB.PseudoRange.CorrectedValue + SatCurrentInfo.ApproxPseudoRangeB.Correction;

                ////double L111=SatCurrentInfo.FrequenceA

                //double e = f1 / (f1 - f2);
                //double f = f2 / (f1 - f2);
                //double c = f1 / (f1 + f2);
                //double d = f2 / (f1 + f2);

                //double SatCurrentMwValue2 = SatCurrentInfo.Combinations.MwRangeCombination.Value / SatCurrentInfo.Combinations.MwRangeCombination.Frequence.WaveLength; //以周为单位,波长是86cm
                //double value = e * L1 - f * L2 - c * P1 - d * P2; //米为单位啊

                //double value1 = e * L11 - f * L22 - c * P11 - d * P22;//米为单位啊

                //double SatCurrentMwValue222 = value / SatCurrentInfo.Combinations.MwRangeCombination.Frequence.WaveLength;
                double SatCurrentMwValue = (rovRoveSatInfo.MwCycle - refRoveSatInfo.MwCycle) - (rovBaseSatInfo.MwCycle - refBaseSatInfo.MwCycle);
                DoubleDifferWideLaneAmbiCyclesDic[paramName].MwDic.Add(rovRoveSatInfo.ReceiverTime, SatCurrentMwValue);
                DoubleDifferWideLaneAmbiCyclesDic[paramName].lastEpoch = rovRoveSatInfo.ReceiverTime;
                //
                //根据Ge的论文，为避免估计偏差，AR不采纳观测时间小于20分钟的,同时中误差大于0.2周的也不用
                if (DoubleDifferWideLaneAmbiCyclesDic[paramName].MwDic.Count <= 40)
                {
                    continue;
                }
                //根据Ge的论文，为避免估计偏差，AR不采纳观测时间小于20分钟的,同时中误差大于0.2周的也不用
                if (DoubleDifferWideLaneAmbiCyclesDic[paramName].MwDic.Count > 260)
                {
                    //
                }

                #region 剔除大于3倍中误差的观测数据
                DataInfo DDMwAverage = cleanMwData(DoubleDifferWideLaneAmbiCyclesDic[paramName]);
                #endregion

                double floatWL, vW;
                //variance of wide-lane ambiguity
                vW = DDMwAverage.obsDataAverageVariance;// /(lam_WL * lam_WL));

                double dW = DDMwAverage.obsDataVariance;// /(lam_WL * lam_WL));


                // MW值是取观测值均值，其方差即是观测值均值的方差，不是观测值的方差，所以下面要还原为观测值的方差，剔除超出观测值三倍方差以为的数据
                if (Math.Sqrt(DDMwAverage.obsDataAverageVariance) > 0.4 //|| Math.Sqrt(rovRoveMwAverage.obsDataVariance) > 0.1
                 || DDMwAverage.obsDataCount < 40)//|| Math.Sqrt(vW) > 0.2|| Math.Sqrt(dW) > 0.2)//)
                {
                    continue;
                }

                //wide-lane ambiguity(以周为单位）
                floatWL = (DDMwAverage.obsDataAverage);// / lam_WL;// +wlbias[sat1.PRN - 1] - wlbias[sat2.PRN - 1];

                //就近取整,最接近BW的整数
                int intWL = (int)Math.Floor(floatWL + 0.5);

                double intWL0 = Math.Round(floatWL);
                if (intWL0 != intWL)
                {
                    throw new Exception("宽巷模糊度固定是取最接近实数的整数！此时没有取成功！");
                }

                //validation of integer wide-lane ambiguity
                // if (Math.Abs(NW - BW) <= thresar2 && conffunc(NW, BW, Math.Sqrt(vW)) >= thresar1)
                if (conffunc(intWL, floatWL, Math.Sqrt(vW)) > thresar1 && Math.Abs(intWL - floatWL) < 0.25)  //实数宽巷模糊度与其最近整数之差小于0.25周，剔除明显粗差影响
                {
                    Vector.Add(intWL, paramName);
                    paraNames.Add(paramName);
                }
            }

            fixedIntWideLineAmbiCycles = Vector;
            fixedIntWideLineAmbiCycles.ParamNames = paraNames;
            if (fixedIntWideLineAmbiCycles.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 数据组的若干信息（均值、方差等）
        /// </summary>
        public class DataInfo
        {
            /// <summary>
            /// 数组的大小
            /// </summary>
            public int obsDataCount { get; set; }
            /// <summary>
            /// 数组的均值
            /// </summary>
            public double obsDataAverage { get; set; }
            /// <summary>
            /// 数组的均值的方差
            /// </summary>
            public double obsDataAverageVariance { get; set; }
            /// <summary>
            /// 数组的方差
            /// </summary>
            public double obsDataVariance { get; set; }
        }

        /// <summary>
        /// 清除粗差
        /// </summary>
        /// <param name="satMwAmbiguityBaseInfo"></param>
        /// <returns></returns>
        private DataInfo cleanMwData(DDWideLineAmbiCycles satMwAmbiguityBaseInfo)
        {
            int count = satMwAmbiguityBaseInfo.MwDic.Count;
            //MW值是取观测值均值，其方差即是观测值均值的方差，不是观测值的方差，所以下面要还原为观测值的方差，剔除超出观测值三倍中误差以为的数据
            double det0 = Math.Sqrt(satMwAmbiguityBaseInfo.GetDataVariance());
            double average = satMwAmbiguityBaseInfo.GetDataAverage();
            DataInfo cleanDataInfo = new DataInfo();
            List<double> newMwDataList = new List<double>(); // = new Dictionary<SatelliteNumber, List<double>>();    
            double total = 0.0;
            foreach (var item in satMwAmbiguityBaseInfo.MwDic)
            {
                if ((item.Value) <= (average + 3 * det0) && (item.Value) >= (average - 3 * det0))
                {
                    newMwDataList.Add(item.Value);
                    total += item.Value;
                }
                else
                {
                    continue;
                }
            }
            double newAverage = total / newMwDataList.Count;
            total = 0.0;
            int n = newMwDataList.Count;
            for (int i = 0; i < n; i++)
            {
                total += (newMwDataList[i] - newAverage) * (newMwDataList[i] - newAverage);
            }

            cleanDataInfo.obsDataCount = n;
            cleanDataInfo.obsDataAverage = newAverage;
            cleanDataInfo.obsDataAverageVariance = (total / n) / n;
            cleanDataInfo.obsDataVariance = total / (n);
            return cleanDataInfo;
        }

        /// <summary>
        ///  LAMBDA算法固定窄巷模糊度
        /// 当然，还有其他固定方法，比如取整固定
        /// </summary>
        /// <param name="rovReceiverInfo"></param>
        /// <param name="refReceeiverInfo"></param>
        /// <param name="adjustment"></param>
        /// <param name="fixedWindLaneAmbiCycles"></param>
        /// <param name="fixedIonFreeAmbiCycles"></param>
        /// <returns></returns>
        private bool tryFixNarrowLaneAmbiguityByLAMBDA(EpochInformation rovReceiverInfo, EpochInformation refReceeiverInfo, AdjustResultMatrix adjustment, Vector fixedWindLaneAmbiCycles, ref WeightedVector fixedIonFreeAmbiCycles)
        {
            List<string> ambiParamNames = fixedWindLaneAmbiCycles.ParamNames;
            WeightedVector estIonFreeAmbiVector = adjustment.Estimated.GetWeightedVector(ambiParamNames);

            double f1 = GnssConst.GPS_L1_FREQ;
            double f2 = GnssConst.GPS_L2_FREQ;

            IVector narrowLaneAmbiguity = (estIonFreeAmbiVector.Multiply((f1 + f2) / f1)).Minus(fixedWindLaneAmbiCycles.Multiply(f2 / (f1 - f2)));
            IMatrix narrowLaneAmbiguityCovariance = estIonFreeAmbiVector.InverseWeight.Multiply((f1 + f2) / f1).Multiply((f1 + f2) / f1);

            WeightedVector floatNLAmbiCycles = new WeightedVector(narrowLaneAmbiguity, narrowLaneAmbiguityCovariance);
            floatNLAmbiCycles.ParamNames = estIonFreeAmbiVector.ParamNames;

            //尝试固定模糊度
            //按照权逆阵排序，大的在后面，如果失败后，则删除之
            var orderedFloatNLAmbiCycles = floatNLAmbiCycles.GetCovaOrdered();
            //模糊度直接用lambda算法进行固定，部分模糊度固定策略
            double MaxRatioOfLambda = 3.0;
            var fixedIntNLAmbiCycles = Gnsser.LambdaAmbiguitySearcher.GetAmbiguity(orderedFloatNLAmbiCycles, MaxRatioOfLambda);

            if (fixedIntNLAmbiCycles.Count < 1)
            { return false; }

            // 关键问题：模糊度固定是否正确？？
            //允许整数与浮点数的最大偏差，如 浮点数为 0.1 而整数为 1，则认为失败。
            double maxDifferOfIntAndFloatAmbi = 0.35;
            List<string> failedParams = new List<string>();
            foreach (var name in fixedIntNLAmbiCycles.ParamNames)
            {
                if (Math.Abs(fixedIntNLAmbiCycles[name] - orderedFloatNLAmbiCycles[name]) > maxDifferOfIntAndFloatAmbi)
                {
                    double tmp = Math.Abs(fixedIntNLAmbiCycles[name] - orderedFloatNLAmbiCycles[name]);
                    failedParams.Add(name);
                }
            }
            fixedIntNLAmbiCycles.Remove(failedParams);

            if (fixedIntNLAmbiCycles.Count < 1)
            { return false; }

            //根据固定的宽巷和窄巷模糊度确定无电离层模糊度
            foreach (var name in fixedIntNLAmbiCycles.ParamNames)
            {
                double fixedIntIonFree = fixedIntNLAmbiCycles[name] * f1 / (f1 + f2) + fixedWindLaneAmbiCycles[name] * f1 * f2 / ((f1 - f2) * (f1 + f2));
                if (!fixedIonFreeAmbiCycles.ParamNames.Contains(name))
                {
                    fixedIonFreeAmbiCycles.Add(fixedIntIonFree, name);
                }
                else
                {
                    int index = fixedIonFreeAmbiCycles.ParamNames.IndexOf(name);
                    double oldFixedValue = fixedIonFreeAmbiCycles.Data[index];
                    if (oldFixedValue != fixedIntIonFree)
                    {
                        fixedIonFreeAmbiCycles[index] = fixedIntIonFree;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// confidence function of integer ambiguity
        /// </summary>
        /// <param name="N"></param>
        /// <param name="B"></param>
        /// <param name="sig"></param>
        /// <returns></returns>
        public double conffunc(int N, double B, double sig)
        {
            double x, p = 1.0;
            int i;
            x = Math.Abs(B - N);
            for (i = 1; i < 8; i++)
            {
                p -= f_erfc((i - x) / (Math.Sqrt(2) * sig)) - f_erfc((i + x) / (Math.Sqrt(2) * sig));
            }
            return p;
        }

        private double f_erfc(double x)
        {
            return x >= 0.0 ? q_gamma(0.5, x * x, Math.Log(Math.PI) / 2.0) : 1.0 + p_gamma(0.5, x * x, Math.Log(Math.PI) / 2.0);
        }

        //complementaty error function
        private double q_gamma(double a, double x, double log_gamma_a)
        {
            double y, w, la = 1.0, lb = x + 1.0 - a, lc;

            int i;

            if (x < a + 1.0) return 1.0 - p_gamma(a, x, log_gamma_a);
            w = Math.Exp(-x + a * Math.Log(x) - log_gamma_a);
            y = w / lb;
            for (i = 2; i < 100; i++)
            {
                lc = ((i - 1 - a) * (lb - la) + (i + x) * lb) / i;
                la = lb; lb = lc;
                w *= (i - 1 - a) / i;
                y += w / la / lb;
                if (Math.Abs(w / la / lb) < 1E-15) break;
            }

            return y;
        }
        private double p_gamma(double a, double x, double log_gamma_a)
        {
            double y, w;
            int i;
            if (x == 0) return 0.0;
            if (x >= a + 1.0) return 1.0 - q_gamma(a, x, log_gamma_a);
            y = w = Math.Exp(a * Math.Log(x) - x - log_gamma_a) / a;

            for (i = 1; i < 100; i++)
            {
                w *= x / (a + i);
                y += w;
                if (Math.Abs(w) < 1E-15) break;
            }
            return y;
        }

        /// <summary>
        /// 某个双差站星的连续宽巷模糊度序列 Winde Line Ambiguitys
        /// 存储一颗卫星在有效时段内的宽巷模糊度信息
        /// 有效时段：没有发生周跳，高度角满足要求，连续观测。
        /// </summary>
        public class DDWideLineAmbiCycles
        {
            /// <summary>
            /// 最后存储的观测时刻
            /// </summary>
            public Time lastEpoch = new Time(); //last epoch

            /// <summary>
            /// 存储该卫星在有效时段内所有历元的数据, double[0]表示卫星高度角，double[1]表示MW值
            /// </summary>
            public Dictionary<Time, double> MwDic = new Dictionary<Time, double>();

            /// <summary>
            /// 返回数组的均值
            /// </summary>
            /// <returns></returns>
            public double GetDataAverage()
            {
                double average = 0.0; int n = 0;
                foreach (var item in MwDic)
                {
                    n += 1;
                    average += (item.Value - average) / n;  //单差值的平均值
                }
                return average;

            }
            /// <summary>
            /// 返回数组均值的方差
            /// </summary>
            /// <returns></returns>
            public double GetDataVariance()
            {
                double average = GetDataAverage();
                double count_rov = 0;
                foreach (var item in MwDic)
                {
                    count_rov += (item.Value - average) * (item.Value - average);
                }
                return count_rov / (MwDic.Count); //流动站的方差
            }
            /// <summary>
            /// 返回数组均值的方差
            /// </summary>
            /// <returns></returns>
            public double GetDataAverageVariance()
            {
                double average = GetDataAverage();
                double count_rov = 0;
                foreach (var item in MwDic)
                {
                    count_rov += (item.Value - average) * (item.Value - average);
                }
                return count_rov / (MwDic.Count * MwDic.Count); //流动站的方差
            }
        }

        /// <summary>
        /// 所有双差卫星的MW模糊度信息
        /// </summary>
        public SortedDictionary<string, DDWideLineAmbiCycles> DoubleDifferWideLaneAmbiCyclesDic = new SortedDictionary<string, DDWideLineAmbiCycles>();


        public class fixLcAmbiguityInfo
        {
            public SatelliteNumber rovPrn;
            public SatelliteNumber basePrn;
            public Geo.Times.Time epoch;
            public double fixLcAmbiguityValue;
        }




    }



}
