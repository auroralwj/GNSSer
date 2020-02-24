//2016.05.14, czs, create in hongqing, 重写 高次差探测周跳法
//2016.11.11, double, edit in zhengzhou, 修改了窗口数据的添加，并且采用拟合残差来进行比较
//2017.08.13, czs, edit in hongiqng, 面向对象重构，参数可配置处理

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
    /// 多项式拟合探测周跳,适合于单频数据。
    /// 周跳探测,只进行探测，而不标记， 
    /// </summary>
    public class LsPolyFitDetector : TimeValueCycleSlipDetector< TimeNumeralWindowData>, ICycleSlipDetector
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public LsPolyFitDetector(GnssProcessOption Option)
            : this(
                Option.ObsDataType,
                Option.MaxRmsTimesOfLsPolyCs,
                Option.PolyFitOrderOfBufferCs,
                 Option.DifferTimesOfBufferCs,
                 Option.MaxBreakingEpochCount,
                Option.IsUsingRecordedCycleSlipInfo
                )
        {
        }

        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        /// <param name="obsType"></param>
        /// <param name="maxMaxRmsTimesOfLsPolyCs"></param>
        /// <param name="order"></param>
        /// <param name="differTimes"></param>
        /// <param name="maxBreakingEpochCount"></param>
        /// <param name="isUsingRecoredCsInfo"></param>
        public LsPolyFitDetector(SatObsDataType obsType,
            double maxMaxRmsTimesOfLsPolyCs,
            int order,
            int differTimes, 
            double maxBreakingEpochCount, 
            bool isUsingRecoredCsInfo)
            : base(maxBreakingEpochCount, isUsingRecoredCsInfo)
        {
            Name = "多项式拟合周跳探测法";  
            this.SatObsDataType = obsType;
            this.MaxRmsTimes = maxMaxRmsTimesOfLsPolyCs; 
            this.WindowSize = 5;
            this.Order = order;
            this.DifferTimes = differTimes;
        }
        /// <summary>
        /// 差分次数
        /// </summary>
        public int DifferTimes { get; set; }
        /// <summary>
        /// 数据窗口大小
        /// </summary>
        public int WindowSize { get; set; } 

        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.多项式拟合法; } } 

        #region 变量  
        /// <summary>
        /// 最大的偏差值。
        /// </summary>
        public double MaxRmsTimes { get; set; }
        /// <summary>
        /// 拟合阶次
        /// </summary>
        public int Order { get; set; } 
        /// <summary>
        /// 需要探测的频率
        /// </summary>
        public SatObsDataType SatObsDataType { get; set; } 
        #endregion
        /// <summary>
        /// 新建一个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override TimeNumeralWindowData Create(SatelliteNumber key)
        {
            return new TimeNumeralWindowData(WindowSize,  this.MaxBreakingEpochCount * 30);
        } 

        /// <summary>
        /// 探测
        /// </summary> 
        /// <returns></returns>
        protected override bool Detect()
        {
            bool isBaseCS = base.Detect();

            bool isCS = Detect(EpochSat.Prn, EpochSat.ReceiverTime, EpochSat[SatObsDataType].Value);

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
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Detect(SatelliteNumber key, Time time, double val)
        {
            var windowData = GetOrCreate(key);
            bool isFull = windowData.IsFull;
            var isAddedOk = true;
            if (DifferTimes > 0)
            {
                windowData.DifferPloyfitCheckAddOrClear(time, val, Order, MaxRmsTimes, DifferTimes);
            }
            else
            {
                windowData.PolyfitCheckAddOrClear(time, val, Order, MaxRmsTimes);
            }

            bool result = !isFull || !isAddedOk; //如果未满员，或未通过检核，都认为是周跳。

            if (!isAddedOk)
            {
                int i = 0;
            }
            return  result;

        }
    }
}