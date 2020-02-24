//2014.05.29, CuiYang, added. 周跳探测 LI
//2014.09.11, czs, refactor
//2016.03.24, czs, edit in hongqing, 简单重构
//2017.08.13, czs, edit in hongiqng, 面向对象重构，参数可配置处理

using Gnsser.Times;
using System;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data.Rinex;
using Geo.Times;
using Geo;
using Geo.Algorithm;

namespace Gnsser
{
    /// <summary>
    /// 通常探测器只能探测一个测站的周跳，如果有多个探测器，请使用多个对象周跳探测,并进行标记，而不修复。 
    /// GPSTk 第二种方法。
    /// 采用 LI 组合值、 LLI1 and LLI2指数，和 2 阶拟合曲线。
    /// 采用 5-12 个数据将LI组合值进行 2 阶拟合，并利用时变阈值进行比较。
    /// 阈值采用指数函数和饱和阈值以及时间常数确定。
    /// 默认最大间隔时间是 61 秒（针对采用率30秒的观测数据）
    /// 将采用一些列数据，配合 SatArcMarker 使用。
    /// </summary>
    public class LiCycleSlipDetector : TimeValueCycleSlipDetector<List<TimeValue>>, ICycleSlipDetector
    {
        #region 构造函数
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public LiCycleSlipDetector(GnssProcessOption Option)
            : base(Option)
        {
            Name = "LI周跳探测法";
            this.MaxTimeSpan = 121;
            this.SatThreshold = 0.08;
            this.TimeConst = 60.0;
            this.MaxBufferSize = 12;
            this.MinBufferSize = 5; 
        }
        /// <summary>
        ///周跳探测,并进行标记，而不修复。 Default constructore, setting default parameters.
        /// </summary>
        public LiCycleSlipDetector(double maxBreakingEpochCount, bool IsUsingRecordedCycleSlipInfo)
            : base(maxBreakingEpochCount, IsUsingRecordedCycleSlipInfo)
        {
            this.MaxTimeSpan = 121; 
            this.SatThreshold = 0.08;
            this.TimeConst = 60.0;
            this.MaxBufferSize = 12;
            this.MinBufferSize = 5;   
        }

        private void SetParam(double interval)
        { 
            TimeConst = interval * 2;
        }
        #endregion  
        SatObsDataType SatObsDataType = SatObsDataType.LiCombination;
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.LI组合; } }

      

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override List<TimeValue> Create(SatelliteNumber key)
        {
            return new List<TimeValue>();
        }
        /// <summary>
        /// 探测
        /// </summary> 
        /// <returns></returns>
        protected override bool Detect()
        {

            bool isBaseCS = base.Detect();

            this.SetParam(EpochSat.EpochInfo.ObsInfo.Interval);

            bool isCS = GetDetection(EpochSat.RecevingTime, EpochSat.Prn, EpochSat[SatObsDataType].Value, isBaseCS);

            if (IsSaveResultToTable && isCS)
            {
                var table = GetOutTable();
                if (!isBaseCS) { table.NewRow(); table.AddItem("Epoch", EpochSat.ReceiverTime); }
                table.AddItem(this.DetectorType, true);
            }

            if (!isBaseCS && isCS)
            {
                CycleSlipStorage.Regist(EpochSat.Prn.ToString(), EpochSat.Time.Value);
            }
            return isCS;
        }

        #region 参数设置 

        //saturation threshold to declare cycle slip, in meters.声明周跳的饱和阈值
        private double SatThreshold { get; set; }

        //threshold time constant, in fraction
        private double TimeConst { get; set; }

        //Maximu buffer aboutSize
        private int MaxBufferSize { get; set; }

        //Minimum aboutSize of buffer. it is always set to 5.
        private int MinBufferSize { get; set; }

        //dictionary holding the information regarding every satellite
        //private SortedDictionary<SatelliteNumber, List<DataItem>> satData { get; set; }

        #endregion

        /// <summary>
        /// 如果有周跳，则返回 1. 否则没有探测出。
        /// Method tat implements the LI cycle slip detection algorithm
        /// </summary>
        /// <param name="gpsTime">Time of observations</param>
        /// <param name="prn">SatId</param> 
        /// <param name="liValue">Current LI observation value</param>   
        /// <returns></returns>
        private bool GetDetection(Time gpsTime, SatelliteNumber prn, double liValue, bool alreadyHasSlip)
        {
            int isCS = 0; //没有周跳 

            var list = GetOrCreate(prn);
            int s = list.Count; 

            //get current buffer aboutSize
            // int bufferSize = satData[satelliteType].Count;
            //Get the difference between current epoch and last epoch, in fraction, but prevObj test if we have epoch satData inside LIData
            double deltaTime = 0.0;//difference between currrent and former epochs, in sec
            if (s > 0) deltaTime = Math.Abs(gpsTime - list.Last().Time);
            else deltaTime = Math.Abs(gpsTime - Time.StartOfMjd);


            //check if receiver already declared cycle slip or too much time has elapsed
            if (alreadyHasSlip || deltaTime > this.MaxTimeSpan)
            {
                list.Clear();
                s = list.Count;
                isCS = 1;
            }


                   //check if we have enough satData to start processing
            if (s < MinBufferSize)
            {
                isCS = 1;

                //Let's prepare for the next epoch//store current epoch at the end of deque            
                list.Add(new TimeValue(gpsTime, liValue));

                //默认没有周跳，误判比漏判更坏？？
                return true;
                //   return false;

            }//没有足够的数据也算。

            if (s >= MinBufferSize)
            {
                //declare a List for measurements
                Geo.Algorithm.ArrayMatrix y = new Geo.Algorithm.ArrayMatrix(s, 1, 0.0);

                //declare a Matrix for epoch information
                Geo.Algorithm.ArrayMatrix M = new Geo.Algorithm.ArrayMatrix(s, 3, 0.0);

                //we store here the oldest (or prevObj) epoch in buffer for future reference. this is important because adjustment will be made with respect to that prevObj epoch
                Time firstEpoch = list.First().Time;

                //feed 'y' with satData
                for (int i = 0; i < s; i++)
                {
                    //the newest goes prevObj in 'y' 
                    y[i, 0] = list[s - 1 - i].Value;
                }
                //feed 'M' with satData
                for (int i = 0; i < s; i++)
                {
                    //compute epoch difference with respect to prevObj epoch
                    double dT = list[s - 1 - i].Time - firstEpoch;
                    M[i, 0] = 1.0;
                    M[i, 1] = dT;
                    M[i, 2] = dT * dT;
                }

                //now, proceed to find a 2nd order fiting curve using a least mean squares(LMS) adjustment
                Geo.Algorithm.ArrayMatrix MT = M.Transpose();
                //Geo.Algorithm.Matrix covMatrix1 = MT * M;
                var covMatrix = Geo.Algorithm.ArrayMatrix.ATA(M);
                //let's try to invert MT*M matrix
                try
                {
                    covMatrix = covMatrix.GetInverse();//.Inverse;
                }
                catch
                {
                    //if covMatrix can't be inverted we have a serious problem with satData, so reset buffer and declare cycle slip
                    list.Clear();
                    //return true; 
                    isCS = 1;

                    //Let's prepare for the next epoch//store current epoch at the end of deque            
                    list.Add(new TimeValue(gpsTime, liValue));
                    return true;

                }

                //Now, compute the Vector holding the results of adjustment to second order curve
                IMatrix a = covMatrix.Multiply(MT).Multiply(y);// *MT * y;

                //the nest step is to compute the maximum deviation from adjustment, in order to assess if our adjustment is too noisy
                double maxDeltaLI = 0.0;

                for (int i = 0; i < s; i++)
                {
                    int index = s - 1 - i;

                    // if (satData[satelliteType].Count <= index + 1) continue;  //出错

                    //compute epoch difference with respect to prevObj epoch
                    double dT = list[index].Time - firstEpoch;

                    //compute adjusted LI value，二次多项式拟合值
                    double LIa = a[0, 0] + a[1, 0] * dT + a[2, 0] * dT * dT;

                    //find maximum deviation in current satData buffer
                    double deltaLI = Math.Abs(LIa - list[index].Value);

                    if (deltaLI > maxDeltaLI)
                    {
                        maxDeltaLI = deltaLI;
                    }
                }

                //compute epoch difference with respect to prevObj epoch
                double deltaT = gpsTime - firstEpoch;
                //compute current adjusted LI value
                double currentLIa = a[0, 0] + a[1, 0] * deltaT + a[2, 0] * deltaT * deltaT;

                //difference between current and adjusted LI value
                double currentBias = Math.Abs(currentLIa - liValue);


                //we will continue processsing only if we trust our current adjustment, time.e: it is not too noisy.
                if ((2.0 * maxDeltaLI) < currentBias)
                {
                    //compute limit to declare cycle slip
                  //  double deltaTime = this.MaxBreakingEpochCount * IntervalSecond;
                    double deltaLimit = SatThreshold / (1.0 + (1.0 / Math.Exp(deltaTime / TimeConst)));

                    //check if current LI deviation is above deltaLimit threshold
                    if (currentBias > deltaLimit)
                    {
                        //reset buffer and declare cycle slip
                        list.Clear();
                        // return true;
                        isCS = 1;
                    }
                }
            }

            //Let's prepare for the next epoch//store current epoch at the end of deque            
            list.Add(new TimeValue(gpsTime, liValue));


            //check if we have exceeded maximum window aboutSize
            if (list.Count > MaxBufferSize) list.RemoveAt(0);

            // return false;
            return (isCS == 1);
        }
         
    }
    /// <summary>
    ///  a class used to store filter satData for a SV.
    /// </summary>
    public class TimeValue
    {
        public TimeValue(Time gpsTime, double li)
        {
            this.Time = gpsTime;
            this.Value = li;
        }

        public Time Time { get; set; }

        public double Value { get; set; }
    }
}
