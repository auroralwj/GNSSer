//2017.09.12, czs, create in hongiqng, 平均数探测法

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
    ///平均数探测法
    /// </summary>
    public class AverageDetector : TimeValueCycleSlipDetector< NumeralWindowData>, ICycleSlipDetector
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public AverageDetector(GnssProcessOption Option)
            : base(Option){
                Name = "平均数探测法";
                this.SatObsDataType = Option.ObsDataType;
                MaxRmsTimes = Option.MaxRmsTimesOfLsPolyCs;
                this.WindowSize = 5; 
        }
        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        public AverageDetector(SatObsDataType obsType, double maxMaxRmsTimesOfLsPolyCs, double maxBreakingEpochCount, bool isUsingRecoredCsInfo)
            : base(maxBreakingEpochCount, isUsingRecoredCsInfo)
        {
            Name = "平均数探测法";  
            this.SatObsDataType = obsType;  
            MaxRmsTimes = maxMaxRmsTimesOfLsPolyCs; 
            this.WindowSize = 5; 
        }
        /// <summary>
        /// 数据窗口大小
        /// </summary>
        public int WindowSize { get; set; } 

        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.数值平均法; } } 

        #region 变量  
        /// <summary>
        /// 最大的偏差值。
        /// </summary>
        public double MaxRmsTimes { get; set; } 
        /// <summary>
        /// 需要探测的频率
        /// </summary>
        public SatObsDataType SatObsDataType { get; set; } 
        #endregion

        public override NumeralWindowData Create(SatelliteNumber key)
        {
            return new NumeralWindowData(WindowSize);
        } 

        /// <summary>
        /// 探测
        /// </summary> 
        /// <returns></returns>
        protected override bool Detect()
        {
            bool isBaseCS = base.Detect();

            bool isCS = Detect(EpochSat.Prn, EpochSat[SatObsDataType].Value);

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
        /// 返回差分数据。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool Detect(SatelliteNumber key, double val)
        {
            var windowData = GetOrCreate(key);
            bool isFull = windowData.IsFull;
            var isAddedOk = windowData.AverageCheckAddOrClear(val, MaxRmsTimes);

            bool result = !isFull || !isAddedOk; //如果未满员，或未通过检核，都认为是周跳。

            if (!result)
            {
                int i = 0;
            }
            return result;

        }
    }
}