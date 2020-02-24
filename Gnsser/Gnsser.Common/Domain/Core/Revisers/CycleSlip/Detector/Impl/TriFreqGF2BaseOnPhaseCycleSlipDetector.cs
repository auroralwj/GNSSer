//2016.11.18,double, create in hongqing, 采用了GF方法[1,0,-1]三频探测周跳
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
    public class TriFreqGF2BaseOnPhaseCycleSlipDetector : TimeValueCycleSlipDetector<SmoothTimeValue>, ICycleSlipDetector
    { 
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public TriFreqGF2BaseOnPhaseCycleSlipDetector(GnssProcessOption Option)
            : base(Option){ 
        }
        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        public TriFreqGF2BaseOnPhaseCycleSlipDetector(double maxBreakingEpochCount = 4, bool isUsingRecoredCsInfo=true)
            : base(maxBreakingEpochCount, isUsingRecoredCsInfo)
        {  
            MaxNumLambdas = 10.0;
            log.Info("采用了GF方法[1,0,-1]三频探测周跳。");
        } 

        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.三频GF1组合; } }
 
        #region 变量
        

        /// <summary>
        /// maximum deviation allowed before declaring cycle slip, in number of Melbourne-Wubbena wavelenghts.
        /// </summary>
        public double MaxNumLambdas { get; set; }


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

            double lam = 0.01;
            double lambda1 = EpochSat.FrequenceA.Frequence.WaveLength;
            double lambda2 = EpochSat.FrequenceB.Frequence.WaveLength; 
            double lambdaLimit = Math.Sqrt(2 * (lambda1 * lambda1 + lambda2 * lambda2)) * lam;
            //if (epochSat.Prn.PRN == 29)
            //{ int ee = 0; }
            bool isCS = GetDetection(EpochSat.RecevingTime, EpochSat.Prn,
                EpochSat.Combinations.TriFreqBasedOnGF2Combination, lambdaLimit);

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
        /// <param name="GFValue">Current mw observation value</param>
        /// <returns></returns>
        public bool GetDetection(Time gpsTime, SatelliteNumber prn, double GFValue, double limit)
        {
            bool isFirst = false;
            if (!data1.ContainsKey(prn))
            {
                isFirst = true;
                log.Debug(gpsTime + ", " + prn + " 卫星第一次出现，标记为有周跳。");
            }
            int isCS = 0;
            //difference between currrent and former epochs, in sec
            double deltaTime = 0.0;
            //difference between current and former MW values
            double currentBias = 0.0;

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

            currentBias = GFValue - data1[prn].lastValue;
            if (prn.PRN == 6)
            { int ee = 0; }
            if (Math.Abs(currentBias) > 10 * limit)
                isCS = 1;
            data1[prn].lastValue = GFValue;

            //store current gpsTime as former gpsTime
            data1[prn].FormerEpoch = gpsTime;


            if (isCS == 1 || isFirst)
            {
                return true;
            }
            else
            {
                return false;
            }
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

            public double lastValue = 0;
        }



        public EpochSatellite EpochSat { get; set; }

        public bool IsSaveResultToTable { get; set; }
        public bool IsUsingRecordedCsInfo { get; set; }
        public ObjectTableManager TableObjectManager { get; set; }
    }
}

