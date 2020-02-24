//2016.03.04, cy added, 双频双差周跳探测算法 含修正
//这是个事后处理算法，需要完善

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Domain;
using System.Text;
using Gnsser.Data.Rinex;
using Geo.Times; 

namespace Gnsser
{
    /// <summary>
    /// 周跳探测,并进行标记，而不修复。 使用 MW observables探测周跳。
    /// 需要采用LI组合值、LLI1、LLI2观测量，如果返回 1 表示有周跳。
    /// </summary>
    public class DualBandCycleSlipDetector //: ICycleSlipRemoveor
    {

        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        public DualBandCycleSlipDetector()
        {
            MaxDeltaTime = 61.0;
           

            sigm1 = 0.005;

            sigm2 = 0.005;

            factor = Math.Sqrt(8);


        }

        public bool Dector(ref EpochInformation rovInfo,ref EpochInformation refInfo, SatelliteNumber basePrn)
        {

            double length = (refInfo.SiteInfo.ApproxXyz - rovInfo.SiteInfo.ApproxXyz).Length / 1000.0;

            if (length < 20) //20km为短基线
            {
                Mion = MinMion;

            }
            else if (length >= 2000)
            {
                Mion = MaxMion;
            }
            else
            {
                Mion = ((400 - 30) * length + 30 * 2000 - 400 * 20) / (2000.0 - 20.0);

            }


            int n = refInfo.EnabledSatCount; //.Count;

            int k = refInfo.EnabledPrns.IndexOf(basePrn);
       
            double f1_f2 = refInfo[k].FrequenceA.PhaseRange.Frequence.Value * refInfo[k].FrequenceA.PhaseRange.Frequence.Value - refInfo[k].FrequenceB.PhaseRange.Frequence.Value * refInfo[k].FrequenceB.PhaseRange.Frequence.Value;

            double b1 = refInfo[k].FrequenceA.PhaseRange.Frequence.Value * refInfo[k].FrequenceA.PhaseRange.Frequence.Value / f1_f2;
            double b2 = -refInfo[k].FrequenceB.PhaseRange.Frequence.Value * refInfo[k].FrequenceB.PhaseRange.Frequence.Value / f1_f2;

            double f2f1 = refInfo[k].FrequenceB.PhaseRange.Frequence.Value * refInfo[k].FrequenceB.PhaseRange.Frequence.Value / (refInfo[k].FrequenceA.PhaseRange.Frequence.Value * refInfo[k].FrequenceA.PhaseRange.Frequence.Value);

            Time recTime = refInfo.ReceiverTime;

            //
            if (basePrn != lastBasePrn)
            {
                data.Clear();
                lastBasePrn = basePrn;

                for (int i = 0; i < n; i++)
                {
                    if (i != k)
                    {
                        SatelliteNumber prn = refInfo.EnabledPrns[i]; //.Prn;
                        
                        data.Add(prn, new FilterData());

                        data[prn].refBasePrn = basePrn;
                        data[prn].FormerEpoch = recTime;

                        
                        //组建双差观测值
                        double phaseA_Rd = rovInfo[prn].ApproxVector.Length - refInfo[prn].ApproxVector.Length -
                                  (rovInfo[k].ApproxVector.Length - refInfo[k].ApproxVector.Length);

                        double phaseA_DD = (rovInfo[prn].FrequenceA.PhaseRange.CorrectedValue - refInfo[prn].FrequenceA.PhaseRange.CorrectedValue) -
                            (rovInfo[k].FrequenceA.PhaseRange.CorrectedValue - refInfo[k].FrequenceA.PhaseRange.CorrectedValue);

                        double phaseB_DD = (rovInfo[prn].FrequenceB.PhaseRange.CorrectedValue - refInfo[prn].FrequenceB.PhaseRange.CorrectedValue) -
                          (rovInfo[k].FrequenceB.PhaseRange.CorrectedValue - refInfo[k].FrequenceB.PhaseRange.CorrectedValue);

                        double residuum_PhaseA_DD = phaseA_DD - phaseA_Rd;
                        double residuum_PhaseB_DD = phaseB_DD - phaseA_Rd;


                        data[prn].doubleDifferencePhaseAData.Add(recTime, residuum_PhaseA_DD);

                        data[prn].doubleDifferencePhaseBData.Add(recTime, residuum_PhaseB_DD);
                      
                        rovInfo[prn].IsUnstable = true;
                        refInfo[prn].IsUnstable = true;
                    }

                   
                }


            }
            else
            {
                //
                lastBasePrn = basePrn;



                for (int i = 0; i < n; i++)
                {
                    if (i != k)
                    {
                        SatelliteNumber prn = refInfo.EnabledPrns[i];


                        if (data.ContainsKey(prn))
                        {
                            //
                            if (Math.Abs(data[prn].FormerEpoch - recTime) > MaxDeltaTime)
                            {
                                data.Remove(prn);

                                //

                                data.Add(prn, new FilterData());

                                data[prn].refBasePrn = basePrn;
                                data[prn].FormerEpoch = recTime;


                                //组建双差观测值
                                double phaseA_Rd = rovInfo[prn].ApproxVector.Length - refInfo[prn].ApproxVector.Length -
                                 (rovInfo[k].ApproxVector.Length - refInfo[k].ApproxVector.Length);

                                double phaseA_DD = (rovInfo[prn].FrequenceA.PhaseRange.CorrectedValue - refInfo[prn].FrequenceA.PhaseRange.CorrectedValue) -
                                    (rovInfo[k].FrequenceA.PhaseRange.CorrectedValue - refInfo[k].FrequenceA.PhaseRange.CorrectedValue);

                                double phaseB_DD = (rovInfo[prn].FrequenceB.PhaseRange.CorrectedValue - refInfo[prn].FrequenceB.PhaseRange.CorrectedValue) -
                                  (rovInfo[k].FrequenceB.PhaseRange.CorrectedValue - refInfo[k].FrequenceB.PhaseRange.CorrectedValue);

                                double residuum_PhaseA_DD = phaseA_DD - phaseA_Rd;
                                double residuum_PhaseB_DD = phaseB_DD - phaseA_Rd;



                                data[prn].doubleDifferencePhaseAData.Add(recTime, residuum_PhaseA_DD);

                                data[prn].doubleDifferencePhaseBData.Add(recTime, residuum_PhaseB_DD);

                                rovInfo[prn].IsUnstable = true;
                                refInfo[prn].IsUnstable = true;

                            }
                            else
                            {
                                data[prn].refBasePrn = basePrn;
                                data[prn].FormerEpoch = recTime;

                                //组建双差观测值

                                double phaseA_Rd = rovInfo[prn].ApproxVector.Length - refInfo[prn].ApproxVector.Length -
                                   (rovInfo[k].ApproxVector.Length - refInfo[k].ApproxVector.Length);

                                double phaseA_DD = (rovInfo[prn].FrequenceA.PhaseRange.CorrectedValue - refInfo[prn].FrequenceA.PhaseRange.CorrectedValue) -
                                    (rovInfo[k].FrequenceA.PhaseRange.CorrectedValue - refInfo[k].FrequenceA.PhaseRange.CorrectedValue);

                                double phaseB_DD = (rovInfo[prn].FrequenceB.PhaseRange.CorrectedValue - refInfo[prn].FrequenceB.PhaseRange.CorrectedValue) -
                                  (rovInfo[k].FrequenceB.PhaseRange.CorrectedValue - refInfo[k].FrequenceB.PhaseRange.CorrectedValue);

                                double residuum_PhaseA_DD = phaseA_DD - phaseA_Rd;
                                double residuum_PhaseB_DD = phaseB_DD - phaseA_Rd;



                                //判断
                                //历元差
                                double r1 = residuum_PhaseA_DD - data[prn].doubleDifferencePhaseAData.Last().Value;
                                double r2 = residuum_PhaseB_DD - data[prn].doubleDifferencePhaseBData.Last().Value;

                                double r3 = b1 * r1 + b2 * r2;

                                bool isCS =true ;
                                double m = (r1 + f2f1 * r2) / 2;

                                double threshold = 3 * factor * Math.Sqrt((b1 * sigm1) * (b1 * sigm1) + (b2 * sigm2) * (b2 * sigm2));

                                //同时满足，则认为没有周跳
                                if ((Math.Abs(r3) <= threshold) && m <= Mion)
                                {
                                    isCS = false;
                                    data[prn].doubleDifferencePhaseAData.Add(recTime, residuum_PhaseA_DD);

                                    data[prn].doubleDifferencePhaseBData.Add(recTime, residuum_PhaseB_DD);

                                   
                                   //rovInfo[i].HasCycleSlip = false;
                                    //refInfo[i].HasCycleSlip = false;
                                    
                                    //rovInfo[k].HasCycleSlip = false;
                                    //refInfo[k].HasCycleSlip = false
                                }
                                else
                                {
                                    isCS = true;                                 
                                    //发生周跳
                                    data.Remove(prn);

                                    data.Add(prn, new FilterData());

                                    data[prn].refBasePrn = basePrn;
                                    data[prn].FormerEpoch = recTime;

                                    data[prn].doubleDifferencePhaseAData.Add(recTime, residuum_PhaseA_DD);

                                    data[prn].doubleDifferencePhaseBData.Add(recTime, residuum_PhaseB_DD);

                                    rovInfo[prn].IsUnstable = true;
                                    refInfo[prn].IsUnstable = true;
                                }
                            }                       
                        }
                        else
                        {
                            data.Add(prn, new FilterData());

                            data[prn].refBasePrn = basePrn;
                            data[prn].FormerEpoch = recTime;

                            //组建双差观测值
                            double phaseA_Rd = rovInfo[prn].ApproxVector.Length - refInfo[prn].ApproxVector.Length -
                                   (rovInfo[k].ApproxVector.Length - refInfo[k].ApproxVector.Length);

                            double phaseA_DD = (rovInfo[prn].FrequenceA.PhaseRange.CorrectedValue - refInfo[prn].FrequenceA.PhaseRange.CorrectedValue) -
                                (rovInfo[k].FrequenceA.PhaseRange.CorrectedValue - refInfo[k].FrequenceA.PhaseRange.CorrectedValue);

                            double phaseB_DD = (rovInfo[prn].FrequenceB.PhaseRange.CorrectedValue - refInfo[prn].FrequenceB.PhaseRange.CorrectedValue) -
                              (rovInfo[k].FrequenceB.PhaseRange.CorrectedValue - refInfo[k].FrequenceB.PhaseRange.CorrectedValue);

                            double residuum_PhaseA_DD = phaseA_DD - phaseA_Rd;
                            double residuum_PhaseB_DD = phaseB_DD - phaseA_Rd;

                            data[prn].doubleDifferencePhaseAData.Add(recTime, residuum_PhaseA_DD);

                            data[prn].doubleDifferencePhaseBData.Add(recTime, residuum_PhaseB_DD);

                            rovInfo[prn].IsUnstable = true;
                            refInfo[prn].IsUnstable = true;
                        }
                    }
                }

            }
             




            return true;
        }





        #region 变量

        


        /// <summary>
        /// maximum interval of time allowed between two successive epochs in fraction
        /// </summary>
        private double MaxDeltaTime = 61.0;

        

        //dictionary holding the information regarding every satellite
        private SortedDictionary<SatelliteNumber, FilterData> data = new SortedDictionary<SatelliteNumber, FilterData>();


        private SatelliteNumber lastBasePrn = SatelliteNumber.Default;


        private double sigm1 = 0.002;

        private double sigm2 = 0.002;

        private double factor = Math.Sqrt(8);

        private double MaxMion = 400;
        private double MinMion = 30;

        //线性内插计算
        private double Mion = 400;


        #endregion

  
        /// <summary>
        ///  a class used to store filter satData for a SV.
        /// </summary>
        private class FilterData
        {
            /// <summary>
            /// the previous epoch time stamp
            /// </summary>
            public Time FormerEpoch = Time.Default;
          



            public Dictionary<Time, double> doubleDifferencePhaseAData = new Dictionary<Time, double>();

            public Dictionary<Time, double> doubleDifferencePhaseBData = new Dictionary<Time, double>();

            public SatelliteNumber refBasePrn = new SatelliteNumber();

           
        }

    }
}
