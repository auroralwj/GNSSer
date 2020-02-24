//2016.01.24, cy, 验后残差分析，剔除未发现的周跳和粗差
//2017.08.17 cy, 非差模型的残差分析
//2018.09.28 cy, 修改bug

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Domain;
using System.Text;
using Gnsser.Data.Rinex;
using Geo.Times;
using Geo.Algorithm.Adjust;

namespace Gnsser.Checkers
{
    /// <summary>
    /// 验后残差分析
    /// 如果没有通过检验，一般是未发现的周跳或粗差，这里统一当粗差处理，剔除。
    /// </summary>
    public class ResidualsAnalysis
    {

        /// <summary>
        /// 
        /// </summary>
        public ResidualsAnalysis()
        {
            lastObsTime = Time.Default;
            lastBasePrn = SatelliteNumber.Default;
            this.thresholdOfVarice = 1.5;//均方根误差倍数，无单位
            this.thresholdOfResidual = 0.05;//残差阈值，单位取米
            this.MaxDeltaTime = 31.0;
        }

        #region 变量
        /// <summary>
        /// maximum interval of time allowed between two successive epochs in fraction
        /// </summary>
        private double MaxDeltaTime = 31.0;


        private double thresholdOfVarice = 1.5; //均方根误差倍数，无单位
        private double thresholdOfResidual = 0.05;//残差阈值，单位取米

        //dictionary holding the information regarding every satellite
        private SortedDictionary<SatelliteNumber, FilterData> data = new SortedDictionary<SatelliteNumber, FilterData>();

        /// <summary>
        /// 基准星
        /// </summary>
        private SatelliteNumber lastBasePrn { get; set; }

        /// <summary>
        /// 历元观测时间
        /// </summary>
        private Time lastObsTime { get; set; }

        ///// <summary>
        ///// 待移除的卫星队列
        ///// </summary>
        //private List<SatelliteNumber> ListPrnRemove = new List<SatelliteNumber>();

        /// <summary>
        /// 待移除的卫星，一次只删除一个
        /// </summary>
        private SatelliteNumber ListPrnRemove = new SatelliteNumber();


        private double maxResiduals = 0.0;
        private double maxResidualsVariance = 0.0;

        #endregion

        public bool Detect(ref EpochInformation rovEpochInfo, ref EpochInformation refEpochInfo, AdjustResultMatrix Adjustment, SatelliteNumber currentBasePrn)
        {
            bool isCS = false;

            if (currentBasePrn != lastBasePrn || Math.Abs(refEpochInfo.ReceiverTime - lastObsTime) > MaxDeltaTime)
            {
                data = new SortedDictionary<SatelliteNumber, FilterData>();

            }

            double[] v = Adjustment.PostfitResidual.OneDimArray;

            int k = rovEpochInfo.EnabledPrns.IndexOf(currentBasePrn);

            for (int i = 0; i < rovEpochInfo.EnabledSatCount; i++)
            {
                if (i == k) continue;

                double vi = 0;
                if (i < k)
                {
                    vi = (v[i + rovEpochInfo.EnabledSatCount - 1]);
                }
                else
                { vi = (v[i - 1 + rovEpochInfo.EnabledSatCount - 1]); }


                SatelliteNumber prn = rovEpochInfo.EnabledPrns[i];

                if (prn != currentBasePrn)
                {
                    if (!data.ContainsKey(prn))
                    {
                        data.Add(prn, new FilterData());

                        data[prn].obsEpoch = refEpochInfo.ReceiverTime;
                        data[prn].residualSquare += vi * vi;
                        data[prn].count += 1;
                        break;

                    }
                    else if (data.ContainsKey(prn))
                    {
                        if (Math.Abs(data[prn].obsEpoch - refEpochInfo.ReceiverTime) >= MaxDeltaTime ||
                            refEpochInfo[i].IsUnstable == true || rovEpochInfo[i].IsUnstable == true ||
                            rovEpochInfo[k].IsUnstable == true || refEpochInfo[k].IsUnstable == true)  //有周跳发生或者发生断裂，则重新统计
                        {
                            data[prn] = new FilterData();
                            data[prn].obsEpoch = refEpochInfo.ReceiverTime;
                            data[prn].residualSquare += vi * vi;
                            data[prn].count += 1;
                            break;
                        }
                        else
                        {
                            //根据历史数据，判断当前残差有无异常发生
                            if (data[prn].count < 11)
                            {
                                data[prn].obsEpoch = refEpochInfo.ReceiverTime;
                                data[prn].residualSquare += vi * vi;
                                data[prn].count += 1;
                                break;
                            }

                            double m0 = Math.Sqrt(data[prn].residualSquare / data[prn].count);
                            double m1 = Math.Sqrt((data[prn].residualSquare + vi * vi) / (data[prn].count + 1));

                            double ratio = m1 / m0;

                            if (Math.Abs(vi) > thresholdOfResidual || ratio > thresholdOfVarice) //满足任何一个条件，则认为该历元发生了周跳或粗差，删除再进行解算
                            //if (ratio > thresholdOfVarice) //满足任何一个条件，则认为该历元发生了周跳或粗差，删除再进行解算
                            {
                                //
                                if (Math.Abs(vi) > maxResiduals || ratio > maxResidualsVariance)
                                {
                                    maxResiduals = Math.Abs(vi);
                                    maxResidualsVariance = ratio;

                                    ListPrnRemove = prn;

                                    isCS = true;
                                }
                            }
                            else
                            {
                                data[prn].obsEpoch = refEpochInfo.ReceiverTime;
                                data[prn].residualSquare += vi * vi;
                                data[prn].count += 1;
                            }
                        }
                    }
                }
            }

            lastBasePrn = currentBasePrn;
            lastObsTime = refEpochInfo.ReceiverTime;

            if (isCS == true) //标记 还是 剔除？
            {
                data.Remove(ListPrnRemove);
                //refEpochInfo[ListPrnRemove].IsUnstable = true;
                // rovEpochInfo[ListPrnRemove].IsUnstable = true;
                refEpochInfo.Remove(ListPrnRemove);
                rovEpochInfo.Remove(ListPrnRemove);

            }

            return isCS;
        }


        /// <summary>
        /// PPP的残差分析
        /// </summary>
        /// <param name="rovEpochInfo"></param>
        /// <param name="refEpochInfo"></param>
        /// <param name="Adjustment"></param>
        /// <param name="currentBasePrn"></param>
        /// <returns></returns>
        public bool Detect(ref EpochInformation rovEpochInfo, AdjustResultMatrix Adjustment)
        {
            bool isCS = false;
            //当前历元残差（载波+伪距）
            if (Math.Abs(rovEpochInfo.ReceiverTime - lastObsTime) > MaxDeltaTime)
            {
                data = new SortedDictionary<SatelliteNumber, FilterData>();

            }
            //判断是否还有伪距观测值，残差只分析载波（周跳和粗差）。不完善，目前只是考虑单频。
            double[] v = Adjustment.PostfitResidual.OneDimArray;

            //判断是否还有伪距观测值，残差只分析载波（周跳和粗差）。不完善，目前只是考虑单频。
            int PhaseObsCount = 0;
            //if (!isOnlyPhase) PhaseObsCount = CurrentMaterial.EnabledSatCount;

            //遍历更新卫星残差和
            for (int i = 0; i < rovEpochInfo.EnabledSatCount; i++)
            {

                double vi = 0;
                //有问题！！！！！！
                vi = (v[i + rovEpochInfo.EnabledSatCount - 1]);


                SatelliteNumber prn = rovEpochInfo[i].Prn;


                if (!data.ContainsKey(prn))
                {
                    data.Add(prn, new FilterData());

                    data[prn].obsEpoch = rovEpochInfo.ReceiverTime;
                    data[prn].residualSquare += vi * vi;
                    data[prn].count += 1;
                    break;

                }
                else if (data.ContainsKey(prn))
                {
                    if (Math.Abs(data[prn].obsEpoch - rovEpochInfo.ReceiverTime) >= MaxDeltaTime ||
                        rovEpochInfo[i].IsUnstable == true)
                    {
                        data[prn] = new FilterData();
                        data[prn].obsEpoch = rovEpochInfo.ReceiverTime;
                        data[prn].residualSquare += vi * vi;
                        data[prn].count += 1;
                        break;
                    }
                    else
                    {
                        //
                        double m0 = Math.Sqrt(data[prn].residualSquare / data[prn].count);
                        double m1 = Math.Sqrt((data[prn].residualSquare + vi * vi) / (data[prn].count + 1));

                        double residualsVarice = Math.Abs(m1 - thresholdOfVarice * m0);
                        double residuals = Math.Abs(vi - thresholdOfResidual);
                        if (residualsVarice > 0 || residuals > 0)
                        {
                            //satData[prn] = new FilterData();

                            if (residuals > maxResiduals || residualsVarice > maxResidualsVariance)
                            {
                                maxResiduals = residuals;
                                maxResidualsVariance = residualsVarice;

                                ListPrnRemove = prn;

                                isCS = true;
                            }

                        }
                        else
                        {
                            data[prn].obsEpoch = rovEpochInfo.ReceiverTime;
                            data[prn].residualSquare += vi * vi;
                            data[prn].count += 1;
                        }

                    }
                }
            }


            lastObsTime = rovEpochInfo.ReceiverTime;

            if (isCS == true)
            {
                data.Remove(ListPrnRemove);

                rovEpochInfo.Remove(ListPrnRemove);

            }

            return isCS;
        }


        /// <summary>
        ///  a class used to store filter satData
        /// </summary>
        private class FilterData
        {
            /// <summary>
            /// the current epoch time 
            /// </summary>
            public Time obsEpoch = Time.Default;
            /// <summary>
            /// residual at current time, v=ax-l, det2= v * v 
            /// </summary>
            public double residualSquare = 0;

            /// <summary>
            /// 有效的个数
            /// </summary>
            public int count = 0;

        }

    }
}
