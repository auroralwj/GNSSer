//2016.11.18,double, create in hongqing, 采用了MW方法三频探测周跳
//2017.08.13, czs, edit in hongiqng, 面向对象重构，参数可配置处理

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Domain;
using System.Text;
using Gnsser.Data.Rinex;
using Geo.Times;
using Geo;
namespace Gnsser
{
    /// <summary>
    /// 周跳探测,并进行标记，而不修复。 使用 MW observables探测周跳。
    /// 如果返回 1 表示有周跳。
    /// </summary>
    public class TriFreqMwCycleSlipDetector : TimeValueCycleSlipDetector<SmoothTimeValue>, ICycleSlipDetector
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public TriFreqMwCycleSlipDetector(GnssProcessOption Option)
            : base(Option){ 
        }
 /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        public TriFreqMwCycleSlipDetector(double maxBreakingEpochCount = 4, bool isUsingRecoredCsInfo=true)
            : base(maxBreakingEpochCount, isUsingRecoredCsInfo) 
        {  
            MaxNumLambdas = 10.0;
            MaxBufferSize = 15;
            Multiple = 4;
            lambdaLimit = MaxNumLambdas * 0.862;   //limit to declare cycle slip based on lambdas( LambdaLW= 0.862 m) ，对于北斗，应该考虑  
         
            log.Debug("采用了MW方法三频探测周跳。");
        } 

        //Maximu buffer aboutSize
        private int MaxBufferSize { get; set; }
        private int Multiple { get; set; }
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.三频MW组合; } }
  
        #region 变量 
      //  SatObsDataType SatObsDataType = SatObsDataType.m
        /// <summary>
        /// maximum deviation allowed before declaring cycle slip, in number of Melbourne-Wubbena wavelenghts.
        /// </summary>
        public double MaxNumLambdas { get; set; }

        double lambdaLimit;      
       

        //dictionary holding the information regarding every satellite
        private SortedDictionary<SatelliteNumber, FilterData> data1 = new SortedDictionary<SatelliteNumber, FilterData>();
        
        #endregion

        /// <summary>
        /// 探测
        /// </summary> 
        /// <returns></returns>
        protected override bool Detect()
        {
            bool isBaseCS = base.Detect();

            bool isCS = GetDetection(EpochSat.RecevingTime, EpochSat.Prn, EpochSat.Combinations.MwTriFreqCombination.Value);

            if (IsSaveResultToTable && isCS)
            {
                var table = GetOutTable();
                    if (!isBaseCS) { table.NewRow(); table.AddItem("Epoch", EpochSat.ReceiverTime); }
                table.AddItem(DetectorType, true);
            }
            if (!isBaseCS && isCS)
            {
                CycleSlipStorage.Regist(EpochSat.Prn.ToString(), EpochSat.Time.Value);
            }
            return isCS;
        }
         

        /// <summary>
        /// Method tat implements the mw cycle slip detection algorithm
        /// </summary>
        /// <param name="epoch">Time of observations</param>
        /// <param name="prn">SatId</param> 
        /// <param name="mwValue">Current mw observation value</param>
        /// <returns></returns>
        public bool GetDetection(Time gpsTime, SatelliteNumber prn, double mwValue)
        {
            bool isFirst = false;
            if (!data1.ContainsKey(prn))
            {
                isFirst = true;
                log.Debug(gpsTime + ", " + prn + " 卫星第一次出现，标记为有周跳。");
            }
            if (prn.PRN == 29)
            { int ee = 0; }
            int isCS = 0;
            //difference between currrent and former epochs, in sec
            double deltaTime = 0.0;
            //difference between current and former MW values
            double currentBias = 0.0;
            double sigma = 0.25;
            //Get the difference between current epoch and last epoch, in fraction, but prevObj test if we have epoch satData inside LIData
            if (data1.ContainsKey(prn) && !data1[prn].FormerEpoch.Equals(Time.Default))
            {
                deltaTime = (double)(gpsTime - data1[prn].FormerEpoch);
            }
            else
            {
                data1.Add(prn, new FilterData());
                deltaTime = (double)(gpsTime - Time.StartOfMjd);
            }
            if (data1[prn].Mw.Count > 0)
            {
                GetRmsAndWindowSize(prn);
                bool revise = data1[prn].Mw.Count > MaxBufferSize;
                while (data1[prn].Mw.Count > MaxBufferSize)
                {
                    data1[prn].Mw.RemoveAt(0);
                }
                if (revise)
                    GetRmsAndWindowSize(prn);

                double averageMw = data1[prn].Mw[0];
                //double sigma = 0.25;
                for (int i = 1; i < data1[prn].Mw.Count; i++)
                {
                    sigma = sigma * i / (i + 1) + (data1[prn].Mw[i] - averageMw) * (data1[prn].Mw[i] - averageMw) / (i + 1);
                    averageMw = averageMw + (data1[prn].Mw[i] - averageMw) / (i + 1);
                }
                if (data1[prn].LastEpochIsUnable)
                {
                    sigma = data1[prn].LastSigma;
                    data1[prn].LastEpochIsUnable = false;
                }
                lambdaLimit = Multiple * Math.Sqrt(sigma);

                currentBias = Math.Abs(mwValue - averageMw);

                if (currentBias > lambdaLimit)
                {
                    //we reset the filter with this
                    
                    isCS = 1;
                    data1[prn].LastEpochIsUnable = true; data1[prn].LastSigma = sigma;
                    data1[prn].Mw.Clear();
                }

                //check if receiver already declared cycle slip or too much time has elapsed            
                
            } 
            if (data1[prn].Mw.Count == MaxBufferSize)
            {
                data1[prn].Mw.RemoveAt(0); data1[prn].Mw.Add(mwValue);
            }
            else data1[prn].Mw.Add(mwValue);
            //store current gpsTime as former gpsTime
            data1[prn].FormerEpoch = gpsTime;


            if (isCS == 1 ||  isFirst)
            {
               data1[prn].LastEpochIsUnable = true; data1[prn].LastSigma = sigma; return true; 
            }
            else
            {
                return false;
            }
        }

        private void  GetRmsAndWindowSize(SatelliteNumber prn)
        {
            double allValue = 0;
            for (int i = 0; i < data1[prn].Mw.Count; i++)
                allValue += data1[prn].Mw[i];
            data1[prn].MeanMW = allValue / data1[prn].Mw.Count;
            double rms = 0;
            for (int i = 0; i < data1[prn].Mw.Count; i++)
                rms += (data1[prn].Mw[i] - data1[prn].MeanMW) * (data1[prn].Mw[i] - data1[prn].MeanMW);
            rms = Math.Sqrt(rms / (data1[prn].Mw.Count - 1));
        }

        /// <summary>
        ///  a class used to store filter satData for a SV.
        /// </summary>
        private class FilterData
        {
            /// <summary>
            /// the previous epoch time stamp
            /// </summary>
            public Time FormerEpoch = Time.Default;
            
            /// <summary>
            /// accumulated mean value of combination.
            /// </summary>
            public double MeanMW = 0.0;
            public List< double> Mw = new List<double>();
            public bool LastEpochIsUnable = false;
            public double LastSigma = 0.25;
        } 
    }
}

