//2014.06.03, CuiYang, adding. 周跳探测, MW
//2014.09.11, czs, refactor
//2016.03.24, czs, refactor in hongqing, 重构

using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Domain;
using System.Text;
using Gnsser.Data.Rinex;
using Geo;
using Geo.Times;

namespace Gnsser
{
    /// <summary>
    /// 周跳探测,并进行标记，而不修复。 使用 MW observables探测周跳。 
    /// 只处理双频情况。
    /// </summary>
    public class MwCycleSlipDetectorOld : BaseDictionary<SatelliteNumber, SmoothTimeValue>, ICycleSlipDetector
    {
        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        public MwCycleSlipDetectorOld()
        {
            MaxDeltaTime = 121;
            MaxNumLambdas = 10.0; 
            lambdaLimit = MaxNumLambdas * 0.862;   //limit to declare cycle slip based on lambdas( LambdaLW= 0.862 m) ，对于北斗，应该考虑  
            this.IsUsingRecordedCsInfo = true;
            CycleSlipStorage = new InstantValueStorage();
            log.Debug("采用了MW方法探测周跳。");
            TableObjectManager = new ObjectTableManager();
            //  satData = new SortedDictionary<SatelliteNumber, List<DataItem>>();
        }  


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
        #endregion
        /// <summary>
        /// 探测
        /// </summary>
        /// <param name="epochSat"></param>
        /// <returns></returns>
        public bool Detect(EpochSatellite epochSat)
        {
            bool isRecordedCycleSlipe = epochSat.EpochInfo.EpochState == EpochState.CycleSlip || epochSat.FrequenceA.IsPhaseLossedLock || epochSat.FrequenceB.IsPhaseLossedLock;
            var alreadyHasSlip = IsUsingRecordedCsInfo && isRecordedCycleSlipe;

            //double f1 = epochSat.FrequenceA.Frequence.Value * 1E6;
            //double f2 = epochSat.FrequenceB.Frequence.Value * 1E6;
            //double lam2 = GnssConst.LIGHT_SPEED / (f1 - f2);
            double lam = epochSat.Combinations.MwRangeCombination.Frequence.WaveLength;
            MaxNumLambdas = 1; //去掉之，会造成误探
            lambdaLimit = MaxNumLambdas * lam;


            bool isCS = GetDetection(epochSat.RecevingTime, epochSat.Prn,
                epochSat.Combinations.MwRangeCombination.Value, alreadyHasSlip);
            if (isCS)
            {
                CycleSlipStorage.Regist(epochSat.Prn.ToString(), epochSat.Time.Value);
            }
            if (epochSat.Time.Value.Hour == 2 && epochSat.Time.Value.Minute == 27)
            { 
                int a = 0;
            }
            return isCS;
        }

        /// <summary>
        /// Method tat implements the mw cycle slip detection algorithm
        /// </summary>
        /// <param name="epoch">Time of observations</param>
        /// <param name="prn">SatId</param> 
        /// <param name="mwValue">Current mw observation value</param>
        /// <param name="alreadyHasSlip">数据是否已经记录周跳</param> 
        /// <returns></returns>
        public bool GetDetection(Time epoch, SatelliteNumber prn, double mwValue, bool alreadyHasSlip)
        {
            bool isFirst = false;
            if (!this.Contains(prn)) { 
                this[prn] = new SmoothTimeValue(MaxDeltaTime, lambdaLimit, true, false) { Name = prn.ToString() };
                isFirst = true;
                log.Debug(epoch + ", " + prn + " 卫星第一次出现，标记为有周跳。");
                return true;//第一次出现，默认有之。
            }

            SmoothTimeValue current = this[prn];
            var isExceed = !current.Regist(epoch, mwValue);
            var result = (alreadyHasSlip || isExceed || isFirst);
            if (result)
            {
                int i = 0;
            }
            return result;
        }


        #region override interface
        /// <summary>
        /// 当前观测的卫星信息
        /// </summary>
        public EpochSatellite EpochSat { get; set; }
        /// <summary>
        /// 是否保存到表
        /// </summary>
        public bool IsSaveResultToTable { get; set; }
        /// <summary>
        /// 是否使用已有周跳信息
        /// </summary>
        public bool IsUsingRecordedCsInfo { get; set; }
        /// <summary>
        /// 表存储器
        /// </summary>
        public ObjectTableManager TableObjectManager { get; set; }
        /// <summary>
        /// 当前探测结果
        /// </summary>
        public bool CurrentResult { get; set; }
        #endregion

    }

}
