//2016.05.14, czs, create in hongqing, 重写 高次差探测周跳法
//2016.08.04, czs, edit in fujian yongan, 修正
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
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 高次差探测周跳法,适合于单频数据。支持逆序探测。
    /// 周跳探测,只进行探测，而不标记和修复。
    /// </summary>
    public class HighDifferSlipDetector : TimeValueCycleSlipDetector<NumeralWindowData>, ICycleSlipDetector
    {
        /// <summary>
        /// 周跳探测,并进行标记，而不修复。 默认构造函数。
        /// </summary>
        public HighDifferSlipDetector(SatObsDataType obsType, double MaxValueDifferOfHeigherDifferCs, double maxBreakingEpochCount, bool isUsingRecordedCsInfo)
            : base(maxBreakingEpochCount, isUsingRecordedCsInfo)
        {
            MaxValueDiffer = MaxValueDifferOfHeigherDifferCs; //14
            //if (obsType.ToString().Contains("Range"))
            //{
            //    MaxValueDiffer *= 0.2;//如果是距离，则阈值相应的乘以一个波长
            //}
            this.SatObsDataType = obsType;
            WindowSize = 4;
            log.Info("采用了高次差方法探测周跳。");
        }
        public HighDifferSlipDetector(GnssProcessOption Option)
            : base(Option)
        {
            MaxValueDiffer = Option.MaxValueDifferOfHigherDifferCs; //14
            //if (obsType.ToString().Contains("Range"))
            //{
            //    MaxValueDiffer *= 0.2;//如果是距离，则阈值相应的乘以一个波长
            //}
            this.SatObsDataType = Option.ObsDataType;
            WindowSize = 4;
            log.Info("采用了高次差方法探测周跳。");
        }


        #region 变量
        /// <summary>
        /// 数据窗口大小
        /// </summary>
        public int WindowSize { get; set; } 
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.高次差法; } }
         
        /// <summary>
        /// 数值阈值
        /// </summary>
        public double MaxValueDiffer { get; set; }
        /// <summary>
        /// 需要探测的观测变量
        /// </summary>
        public SatObsDataType SatObsDataType { get; set; } 
        #endregion
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

            bool isCS = GetDetection(EpochSat.Prn, EpochSat[SatObsDataType].Value);
           
            if ( IsSaveResultToTable && isCS)
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
        /// 探测。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">待探测的数据</param> 
        /// <returns></returns>
        public bool GetDetection(SatelliteNumber key, double val)
        {
            var windowData = GetOrCreate(key);
            windowData.Add(val);//先加进来再探测。
            if (windowData.IsFull)
            {
                var neatData = windowData.GetNeatlyWindowData(3);
                var differ = neatData.LoopDifferValue;
                var isExceed = (Math.Abs(differ) > MaxValueDiffer);
                if (isExceed)
                {
                    //发现周跳，把之前的数据清除,后继在此基础上重新探测
                    windowData.Clear();
                    windowData.Add(val);

                    return true;
                }
                return false;
            }
            return true;
        }
    }
}