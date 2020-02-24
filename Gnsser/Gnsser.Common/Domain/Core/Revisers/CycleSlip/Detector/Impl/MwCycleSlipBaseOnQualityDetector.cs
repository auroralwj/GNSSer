//2016.10.20, double, create in hongqing, 基于数据质量的MW周跳探测。
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
    public class MwCycleSlipBaseOnQualityDetector : BaseDictionary<SatelliteNumber, SmoothTimeValue>, ICycleSlipDetector
    {

        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        public MwCycleSlipBaseOnQualityDetector()
        {
            MaxDeltaTime = 121;
            MaxNumLambdas = 10.0;
            MaxBufferSize = 15;
            Multiple = 3;
            lambdaLimit = MaxNumLambdas * 0.862;   //limit to declare cycle slip based on lambdas( LambdaLW= 0.862 m) ，对于北斗，应该考虑  
            this.UseRecordedSlipInfo = true;
            CycleSlipStorage = new InstantValueStorage();
            log.Debug("采用了基于数据质量的MW方法探测周跳。");
        }
        /// <summary>
        /// 是否使用已经记录的周跳信息，否则只从数据本身探测。
        /// </summary>
        public bool UseRecordedSlipInfo { get; set; }

        //Maximu buffer aboutSize
        private int MaxBufferSize { get; set; }
        private int Multiple { get; set; }
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.MW组合; } }
        /// <summary>
        /// 周跳探测结果存储器
        /// </summary>
        public InstantValueStorage CycleSlipStorage { get; set; }
        #region 变量
        /// <summary>
        /// 允许最大时间差
        /// </summary>
        public double MaxDeltaTime { get; set; }

        /// <summary>
        /// maximum deviation allowed before declaring cycle slip, in number of Melbourne-Wubbena wavelenghts.
        /// </summary>
        public double MaxNumLambdas { get; set; }

        double lambdaLimit;      
       

        //dictionary holding the information regarding every satellite
        private SortedDictionary<SatelliteNumber, FilterData> data1 = new SortedDictionary<SatelliteNumber, FilterData>();
        
        #endregion

        public bool Detect(EpochSatellite epochSat)
        {
            bool isRecordedCycleSlipe = epochSat.EpochInfo.EpochState == EpochState.CycleSlip || epochSat.FrequenceA.IsPhaseLossedLock || epochSat.FrequenceB.IsPhaseLossedLock;
            var alreadyHasSlip = UseRecordedSlipInfo && isRecordedCycleSlipe;
                       
            double lam = epochSat.Combinations.MwRangeCombination.Frequence.WaveLength;
          
            lambdaLimit = MaxNumLambdas * lam;
            if (epochSat.Prn.PRN == 29)
            { int ee = 0; }
            bool isCS = GetDetection(epochSat.RecevingTime, epochSat.Prn,
                epochSat.Combinations.MwPhaseCombinationValue, alreadyHasSlip);
            if (isCS)
            {
                CycleSlipStorage.Regist(epochSat.Prn.ToString(), epochSat.Time.Value);
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
        public bool GetDetection(Time gpsTime, SatelliteNumber prn, double mwValue, bool alreadyHasSlip)
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
                    Multiple = 3;
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
            if (deltaTime > MaxDeltaTime)
            {
                isCS = 1;
            }
            if (data1[prn].Mw.Count == MaxBufferSize)
            {
                data1[prn].Mw.RemoveAt(0); data1[prn].Mw.Add(mwValue);
            }
            else data1[prn].Mw.Add(mwValue);
            //store current gpsTime as former gpsTime
            data1[prn].FormerEpoch = gpsTime;


            if (isCS == 1 || alreadyHasSlip || isFirst)
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
            if (rms >= 0.3)
            {
                MaxBufferSize = 15; Multiple = 3;
            }
            else if (rms >= 0.2 && rms < 0.3)
            {
                MaxBufferSize = 20; Multiple = 3;
            }
            else if (rms < 0.2)
            {
                MaxBufferSize = 25; Multiple = 4;
            }
            

        }


        public EpochSatellite EpochSat { get; set; }

        public bool IsSaveResultToTable { get; set; }
        public bool IsUsingRecordedCsInfo { get; set; }
        public ObjectTableManager TableObjectManager { get; set; }
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



        public bool CurrentResult
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

