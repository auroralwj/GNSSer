//2014.09.10, czs, create, 周跳探测初步
//2017.08.13, czs, edit in hongqing, 面向对象重构

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo;
using Geo.Times;
using Geo.IO;

namespace Gnsser
{   
    
    /// <summary>
    /// 时间数值周跳探测器，标记第一次出现的数值，或者时间间隔超限的数据具有周跳。
    /// 本类可以作为通用探测器基类。
    /// </summary>
    public class TimeValueCycleSlipDetector : TimeValueCycleSlipDetector<Boolean>
    {        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public TimeValueCycleSlipDetector(GnssProcessOption Option)
            : base(Option)
        {
        }
        /// <summary>
        /// 构造函数。此方法可以反复调用，无时间积累效应。
        /// </summary>
        /// <param name="maxBreakingEpochCount">最大允许断裂历元数量</param>
        /// <param name="IsUsingRecordedCycleSlipInfo">是否采用已有的周跳信息</param>
        public TimeValueCycleSlipDetector(double maxBreakingEpochCount, bool IsUsingRecordedCycleSlipInfo)
            : base(maxBreakingEpochCount, IsUsingRecordedCycleSlipInfo) { }
    }

    /// <summary>
    /// 基础的指定时间的周跳探测器。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseValueCycleSlipDetector<T> : TimeValueCycleSlipDetector<T>
    {     
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Option"></param>
        public BaseValueCycleSlipDetector(GnssProcessOption Option)
            : base(Option){

                this.SatObsDataType = Option.ObsDataType;
        }
        /// <summary>
        /// 构造函数。此方法可以反复调用，无时间积累效应。
        /// </summary>
        /// <param name="maxBreakingEpochCount">最大允许断裂历元数量</param>
        /// <param name="IsUsingRecordedCycleSlipInfo">是否采用已有的周跳信息</param>
        public BaseValueCycleSlipDetector(SatObsDataType dataType, double maxBreakingEpochCount, bool IsUsingRecordedCycleSlipInfo)
            : base(maxBreakingEpochCount, IsUsingRecordedCycleSlipInfo)
        {
            this.SatObsDataType = dataType;
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        public SatObsDataType SatObsDataType { get; set; }

        protected override bool Detect()
        {
            bool isBase = base.Detect();
            return isBase;
            var win = this.SatValueManager.GetOrCreate(EpochSat.Prn);
            if (IsTimeBreak)
            {
                win.Clear();
            }
            //必须都要添加进来，才能判断
            win.Add(EpochSat.ReceiverTime, EpochSat[SatObsDataType].CorrectedValue);
            if (win.IsFull)
            {
                //win.IsFull
                var grossKeys = win.GetKeysOfGrossError(m => m.SecondsOfWeek, 3);
                foreach (var sat in CurentEpochSats)
                {
                    if (grossKeys.Contains(sat.ReceiverTime))
                    {
                        sat.IsUnstable = true;
                    }
                } 
                return false;
            }

            return false;//默认无，全靠过程中自己修改。     
        }
    }

    /// <summary>
    /// 时间数值周跳探测器，标记第一次出现的数值，或者时间间隔超限的数据具有周跳。
    /// 本类可以作为通用探测器基类。支持逆序探测。
    /// </summary>
    public class TimeValueCycleSlipDetector<T> : BaseCycleSlipDetector<T>
    {          
       /// <summary>
       /// 构造函数。此方法可以反复调用，无时间积累效应。
       /// </summary>
       /// <param name="maxBreakingEpochCount">最大允许断裂历元数量</param>
       /// <param name="IsUsingRecordedCycleSlipInfo">是否采用已有的周跳信息</param>
        public TimeValueCycleSlipDetector(double maxBreakingEpochCount, bool IsUsingRecordedCycleSlipInfo)
            : base(IsUsingRecordedCycleSlipInfo)
        { 
            LastTimeStorage = new BaseDictionary<SatelliteNumber, Time>();
            this.IntervalSecond = 30;
            this.MaxBreakingEpochCount = maxBreakingEpochCount;
            this.TableObjectManager = new ObjectTableManager() { Name = DetectorType.ToString() + "_周跳探测" }; 
            this.IsSaveResultToTable = true;
            SetMaxTimeSpan(IntervalSecond);

            log.Debug("采用了“" + DetectorType.ToString() + "”方法探测周跳");
        }

        /// <summary>
        /// 构造函数。此方法可以反复调用，无时间积累效应。
        /// </summary>
        /// <param name="Option">最大允许断裂历元数量, 是否采用已有的周跳信息</param> 
        public TimeValueCycleSlipDetector(GnssProcessOption Option)
            : base(Option)
        { 
            
            LastTimeStorage = new BaseDictionary<SatelliteNumber, Time>();
            this.IntervalSecond=30;
            this.MaxBreakingEpochCount = Option.MaxBreakingEpochCount;
            this.TableObjectManager = new ObjectTableManager() { Name = DetectorType.ToString() + "_周跳探测" }; 
            this.IsSaveResultToTable = true;
            SetMaxTimeSpan(IntervalSecond);

            log.Debug("采用了“" + DetectorType.ToString() + "”方法探测周跳");
        }
        #region  属性 
        /// <summary>
        /// 周跳探测类型。
        /// </summary>
        public override CycleSlipDetectorType DetectorType { get { return CycleSlipDetectorType.首次出现标记法; } }
        /// <summary>
        /// 采样间隔，秒
        /// </summary>
        public double IntervalSecond { get; set; }
        /// <summary>
        /// 允许的最大断裂历元
        /// </summary>
        public double MaxBreakingEpochCount { get; set; }
        /// <summary>
        /// 最大时间间隔,单位秒
        /// </summary>
        public Double MaxTimeSpan { get; set; }
        /// <summary>
        /// 时间是否断裂，是否在指定的时间间隔内。
        /// </summary>
        public bool IsTimeBreak { get; set; }
        /// <summary>
        /// 是否已经周跳了。
        /// </summary>
        public bool IsAlreadyCycleSliped  { get; set; }
        /// <summary>
        /// 字典
        /// </summary>
        public BaseDictionary<SatelliteNumber, Time> LastTimeStorage { get; set; }

        #endregion

        protected override void SetEpochSatellite(EpochSatellite epochSat)
        {
            base.SetEpochSatellite(epochSat);
            //设值 
            SetMaxTimeSpan(EpochSat.EpochInfo.ObsInfo.Interval); 
        }

        /// <summary>
        /// 根据历元断裂次数和采样间隔计算最大时间跨度，单位秒。
        /// 必须设定采用间隔。
        /// </summary>
        protected void SetMaxTimeSpan(double IntervalSecond)
        {
            if (IntervalSecond <= 0) { IntervalSecond = 30; }
            this.MaxTimeSpan = this.MaxBreakingEpochCount * IntervalSecond + 0.5;
        }
        /// <summary>
        /// 探测，有则返回 true。
        /// </summary>
        /// <param name="epochSat"></param>
        /// <returns></returns>
        public override bool Detect(EpochSatellite epochSat)
        {
            InitDetect(epochSat);

            bool hasCs = Detect();

            if (hasCs)
            {
                RegistCurrentCsed();
            }

            return hasCs;
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        private void InitDetect(EpochSatellite epochSat)
        {
            SetEpochSatellite(epochSat);
         
           // 时间判定
            SetIsTimeBreaked(epochSat);
            //时间检查与更新
            CheckOrUpdateTime(); 

            #region 已有数据判断
            this.IsAlreadyCycleSliped = (IsUsingRecordedCsInfo && (EpochSat.IsUnstable || EpochSat.EpochInfo.EpochState != EpochState.Ok));
            #endregion

            #region 缓存
            //缓存
            if (!EpochSats.Contains(epochSat.Prn)) { EpochSats.Add(epochSat.Prn, new WindowData<EpochSatellite>(10)); }
            this.CurentEpochSats = EpochSats[epochSat.Prn];
            if (IsTimeBreak)
            {
                CurentEpochSats.Clear();                    
            }

            CurentEpochSats.Add(epochSat);
            #endregion
        }
        /// <summary>
        /// 设置时间是否断裂
        /// </summary>
        /// <param name="epochSat"></param>
        private void SetIsTimeBreaked(EpochSatellite epochSat)
        {
            var key = epochSat.Prn;
            if (!LastTimeStorage.Contains(key)) { LastTimeStorage[key] = epochSat.ReceiverTime; }
            //比较 
            this.IsTimeBreak = Math.Abs(epochSat.ReceiverTime - LastTimeStorage[key]) > MaxTimeSpan; //支持逆序探测

        }
        /// <summary>
        /// 时间更新
        /// </summary>
        private void CheckOrUpdateTime()
        {
            //数值更新
            if (LastTimeStorage[EpochSat.Prn] != EpochSat.ReceiverTime) //时间不相等才更新，避免重复设置。
            {
                LastTimeStorage[EpochSat.Prn] = EpochSat.ReceiverTime;
            }
        }

        /// <summary>
        /// 探测
        /// </summary>
        /// <returns></returns>
        protected virtual bool Detect()
        {  
            bool haveCs = IsTimeBreak || IsAlreadyCycleSliped;

            //add to table
            if (IsSaveResultToTable && haveCs)
            {
                var table = GetOutTable();
                table.NewRow();
                table.AddItem("Epoch", EpochSat.ReceiverTime);
                if (IsTimeBreak) { table.AddItem("TimeBreak", true); }
                if (IsAlreadyCycleSliped) { table.AddItem("Already", true); }
            }
            this.CurrentResult = haveCs;
            return haveCs;
        }
         

    }
}
