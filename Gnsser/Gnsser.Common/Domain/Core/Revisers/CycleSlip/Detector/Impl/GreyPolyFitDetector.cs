//2016.11.11, double, create in zhengzhou, 灰色模型拟合探测周跳
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
    /// 灰色模型拟合探测周跳,适合于单频数据。
    /// </summary>
    public class GreyPolyFitDetector : BaseValueCycleSlipDetector<NumeralWindowData>, ICycleSlipDetector
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public GreyPolyFitDetector(GnssProcessOption Option)
            : base(Option)
        {
            Name = "灰色模型拟合周跳探测法"; 
                this.WindowSize = 9; 
        }

        /// <summary>
        /// 数据窗口大小
        /// </summary>
        public int WindowSize { get; set; } 

        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.灰色模型法; } }
  
        #region 变量 
        /// <summary>
        /// 数值阈值
        /// </summary>
        public double MaxValueDiffer { get; set; } 


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
            
            if (windowData.IsFull)
            {
                GreyModel GreyModel = new GreyModel();
                //计算拟合和差值                
                GreyModel.Calculate(windowData.Data, IntervalSecond);
                double error = GreyModel.nextFitValue - (val - windowData.Data[WindowSize-1]);
                if (Math.Abs(error) > 10 * GreyModel.PolyRms)
                {
                    windowData.Clear();
                    //double val1 = GreyModel.nextFitValue+windowData.Data[WindowSize-1];
                    //val1 = val1 + Math.Truncate(nextFit.Value);
                    windowData.Add(val);
                    //出现周跳啦！！并可以采用拟合值修复，
                    return true;
                }
                #region 多项式拟合
                //var nextFit = windowData.GetNextLsPolyFitValue(0, 3, IntervalSecond);
                //double errorPercent = (nextFit.Value - val) ;
                //if (Math.Abs(errorPercent) > 10 * nextFit.Rms)
                //{
                //    //double val1 = val%1;
                //    //val1 = val1 + Math.Truncate(nextFit.Value);
                //    windowData.Add(nextFit.Value);
                //    //出现周跳啦！！并可以采用拟合值修复，
                //    return true;
                //}
                #endregion
                windowData.Add(val);
                return false ;
            }
            windowData.Add(val);
            return true ;
        }

    }
}