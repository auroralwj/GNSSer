//2014.06.03, CuiYang, adding. 周跳探测, MW
//2014.09.11, czs, refactor
//2016.03.24, czs, refactor in hongqing, 重构
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
    /// 周跳探测,并进行标记，而不修复。 使用 MW observables探测周跳。 
    /// 只处理双频情况。
    /// </summary>
    public class MwCycleSlipDetector : TimeValueCycleSlipDetector<SmoothTimeValue>, ICycleSlipDetector
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public MwCycleSlipDetector(GnssProcessOption Option)
            : base(Option)
        {
            this.MaxDifferValue = Option.MaxDifferValueOfMwCs;
            log.Debug("采用了MW方法探测周跳。");
        }
        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        /// <param name="maxBreakingEpochCount"></param>
        /// <param name="maxDifferValue"></param>
        /// <param name="IsUsingRecordedCycleSlipInfo"></param>
        public MwCycleSlipDetector(double maxBreakingEpochCount = 4, double maxDifferValue = 8.6, bool IsUsingRecordedCycleSlipInfo = true)
            : base(maxBreakingEpochCount, IsUsingRecordedCycleSlipInfo)
        {  
            this.MaxDifferValue = maxDifferValue;  
            log.Debug("采用了MW方法探测周跳。"); 
        }
        SatObsDataType SatObsDataType = SatObsDataType.MwCombination;
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.MW组合; } } 
        #region 变量 
        /// <summary>
        /// 允许的最大数值差异
        /// </summary>
        public double MaxDifferValue { get; set; }
        #endregion
        /// <summary>
        /// 探测
        /// </summary> 
        /// <returns></returns>
        protected override bool Detect()
        {
            bool isBaseCS = base.Detect();

            bool isCS = GetDetection(EpochSat.RecevingTime, EpochSat.Prn, EpochSat[SatObsDataType].Value);

            if (IsSaveResultToTable && isCS)
            {
                var table = GetOutTable();
                    if (!isBaseCS) { table.NewRow(); table.AddItem("Epoch", EpochSat.ReceiverTime); }
                table.AddItem(DetectorType.ToString(), true);
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
        public bool GetDetection(Time epoch, SatelliteNumber prn, double mwValue)
        {
            bool isFirst = false;
            if (!this.Contains(prn)) { 
                this[prn] = new SmoothTimeValue( this.MaxTimeSpan, MaxDifferValue, true, false) { Name = prn.ToString() };
                isFirst = true;
                log.Debug(epoch + ", " + prn + " 卫星第一次出现，标记为有周跳。");
                return true;//第一次出现，默认有之。
            }

            SmoothTimeValue current = this[prn];
            var isExceed = !current.Regist(epoch, mwValue);
            var result =  isExceed || isFirst;
            return result;
        }
        public override SmoothTimeValue Create(SatelliteNumber key)
        {
            return new SmoothTimeValue(this.MaxTimeSpan, MaxDifferValue, true, false) { Name = key.ToString() };
        }
    }

}
