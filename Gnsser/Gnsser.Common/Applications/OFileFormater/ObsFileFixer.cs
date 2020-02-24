//2017.10.22, czs, create  in hongqing, 观测文件探测与修复器

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Geo.Common;
using Geo.Coordinates;
using Geo;
using Geo.IO;
using Geo.Times;
using Gnsser.Data.Rinex;
using Gnsser.Models;
using Gnsser.Domain;

namespace Gnsser
{

    /// <summary>
    /// 钟跳修复器
    /// </summary>
    public class ClockJumpReviser : EpochInfoReviser
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ClockJumpReviser(Double interval)
        {
            interval = interval == 0 ? 15 : interval;
            var maxBreakingSecond = interval * 10;
            LastJumpTime = Time.MinValue;
            ClockJumpCorrectManager = new BaseDictionary<SatelliteNumber, ClockJumpCorrector>("钟跳修复器", m => new ClockJumpCorrector(maxBreakingSecond));
            TotalCount = 0;
            this.MinDetectingRatio = 0.4;
            this.PhaseAlignerManager = new NumericalAlignerManager<SatelliteNumber, Time>(maxBreakingSecond, m => m.SecondsOfWeek);
        }

        #region  属性
        /// <summary>
        /// 探测器
        /// </summary>
        public BaseDictionary<SatelliteNumber, ClockJumpCorrector> ClockJumpCorrectManager { get; set; }
        /// <summary>
        /// 上一个跳跃时间
        /// </summary>
        public Time LastJumpTime { get; set; }
        /// <summary>
        /// 总共发生钟跳数量
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 最小探测率（探测到的各卫星钟跳发生率），如果大于此，则认为该历元发生了钟跳。
        /// </summary>
        public double MinDetectingRatio { get; set; }
        /// <summary>
        /// 上一个历元信息 
        /// </summary>
        public EpochInformation PrevEpochInfo { get; set; }

        NumericalAlignerManager<SatelliteNumber, Time> PhaseAlignerManager { get; set; }

        #endregion
        /// <summary>
        /// 探测
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation obj)
        {
            Detect(obj);
           
            //-----------------------数据对齐-----------
            //修复
            double interval = obj.ObsInfo.Interval;
            if (PrevEpochInfo != null) { interval = Math.Round(obj.ReceiverTime - PrevEpochInfo.ReceiverTime); }//真间隔
            
            //首先优先采用多普勒频移
            if (obj.First.First.DopplerShift != null) { 
          
                foreach (var sat in obj)
                {
                    foreach (var fre in sat)
                    {

                        var increasedCycles = interval * fre.DopplerShift.Value;
                        var aliger = PhaseAlignerManager.GetOrCreate(sat.Prn);

                        if (obj.EpochState == EpochState.ClockJumped)
                        {
                            aliger.IsReset = true;
                        }

                        double referVal = 0;
                        if (aliger.LastAlignedValue == 0 || PrevEpochInfo == null)//第一次
                        {
                            referVal = fre.PseudoRange.Value / fre.Frequence.WaveLength;
                        }
                        else
                        {
                            referVal = aliger.LastAlignedValue + increasedCycles;
                        }

                        var val = aliger.GetAlignedValue(sat.ReceiverTime, fre.PhaseRange.RawPhaseValue, referVal);
                        fre.PhaseRange.SetRawValue(val);
                    }
                }
            }
            else//采用伪距对齐
            { 
                foreach (var sat in obj)
                {
                    foreach (var fre in sat)
                    {                         
                        var aliger = PhaseAlignerManager.GetOrCreate(sat.Prn);

                        if(obj.EpochState == EpochState.ClockJumped){
                            aliger.IsReset = true;
                        }

                        double referVal = fre.PseudoRange.Value / fre.Frequence.WaveLength; 

                        var val = aliger.GetAlignedValue(sat.ReceiverTime, fre.PhaseRange.RawPhaseValue, referVal);
                        fre.PhaseRange.SetRawValue(val);
                    }
                }
            }

            PrevEpochInfo = obj;

            return true;
        }

        /// <summary>
        /// 探测历元钟跳
        /// </summary>
        /// <param name="obj"></param>
        private void Detect(EpochInformation obj)
        {
            List<SatelliteNumber> isJumpedes = new List<SatelliteNumber>();
            foreach (var sat in obj)
            {
                var reffer = sat;
                if (!ClockJumpCorrectManager.GetOrCreate(sat.Prn).Revise(ref reffer))
                {
                    isJumpedes.Add(sat.Prn);
                }
            }

            int jumped = isJumpedes.Count;
            double jumpedRatio = 1.0 * jumped / obj.Count;
            if (jumpedRatio > MinDetectingRatio)
            {
                obj.EpochState = EpochState.ClockJumped;

                var differSeconds = obj.ReceiverTime - LastJumpTime;

                var interval = TimeSpan.FromSeconds(differSeconds).ToString();
                if (LastJumpTime == Time.MinValue)
                {
                    interval = "第一次";
                }

                log.Info(obj.Name + "，" + obj.ReceiverTime + "，钟跳，间隔 " + interval + "，" + jumped.ToString("##") + "/" + obj.Count.ToString("##") + "=" + jumpedRatio.ToString("0.00"));
                //是否修复

                TotalCount++;
                LastJumpTime = obj.ReceiverTime;
            }
        }

        //public void 

        /// <summary>
        /// 完成
        /// </summary>
        public override void Complete()
        {
            base.Complete();
            log.Info("共发生了 " + TotalCount + " 次 周跳");
        }
    }

    /// <summary>
    /// 钟差探测与修复器
    /// </summary>
    public class ClockJumpCorrector : EpochSatReviser  {
       
        /// <summary>
        /// 钟差探测与修复器
        /// </summary>
        /// <param name="maxBreakingSecond"></param>
        /// <param name="jumpSpan">钟跳范围</param>
        public ClockJumpCorrector(double maxBreakingSecond, double jumpSpan= 0.001)
        { 
            MaxDiffer = jumpSpan * GnssConst.LIGHT_SPEED / 2;
        }

        /// <summary>
        /// 上一个。
        /// </summary>
        public EpochSatellite Previous { get; set; }
        /// <summary>
        /// 最大允许误差阈值
        /// </summary>
        public double MaxDiffer { get; set; }
        /// <summary>
        /// 上一个差分值，即平均径向速度
        /// </summary>
        public double LastDifferVaue{ get; set; } 

        /// <summary>
        /// 探测，如果有，返回false。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochSatellite obj)
        {
            var rangeA = obj.FrequenceA.PseudoRange.Value;

            bool isNoJump = true;
            double speedAbs = 0;
            if (Previous != null)
            {
                speedAbs = Math.Abs(rangeA - Previous.FrequenceA.PseudoRange.Value);
                if (speedAbs > MaxDiffer)
                {
                    isNoJump = false;
                }
            }

            this.LastDifferVaue = speedAbs;
            Previous = obj;
            return isNoJump;
        }
    }

    /// <summary>
    /// 钟差探测与修复器
    /// </summary>
    public class ClockJumpCorrector2 : EpochSatReviser  {
        /// <summary>
        /// 钟差探测与修复器
        /// </summary>
        /// <param name="maxBreakingSecond"></param>
        public ClockJumpCorrector2(double maxBreakingSecond)
        {
            int windowSize = 5;
            WindowA = new TimeNumeralWindowData(windowSize, maxBreakingSecond);         
            MaxRmsTimes = 8;
        }

        /// <summary>
        /// 上一个。
        /// </summary>
        public EpochSatellite Previous { get; set; }
        /// <summary>
        /// 最大允许误差阈值
        /// </summary>
        public double MaxRmsTimes { get; set; }
        public TimeNumeralWindowData WindowA { get; set; } 

        public override bool Revise(ref EpochSatellite obj)
        {
            var rangeA = obj.FrequenceA.PseudoRange.Value;
            if (!WindowA.DifferCheckAddOrClear(obj.ReceiverTime, rangeA, MaxRmsTimes,1))
            {
                //改正              
                return false;
            } 
            Previous = obj;
            return true;
        }
    }


    /// <summary>
    /// 观测文件探测与修复器
    /// </summary>
    public class ObsFileFixer : ObsFileEpochRunner<EpochInformation>
    {
        Log log = new Log(typeof(ObsFileFixer));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="FilePath"></param>
        public ObsFileFixer(ObsFileFixOption Option, string FilePath)
        {
            this.FilePath = FilePath;
            this.OutputDirectory = Option.OutputDirectory;
            this.Option = Option;
        }
        #region 属性
        /// <summary>
        /// 待分析的文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 选项
        /// </summary>
        public ObsFileFixOption Option { get; set; }
        /// <summary>
        /// 历元信息转换为RINEX
        /// </summary>
        public EpochInfoToRinex EpochInfoToRinex { get; set; }
        /// <summary>
        /// 数据第一次加载（到缓存）时执行。
        /// </summary>
        public EpochInfoReviseManager RawReviser { get; set; }
        /// <summary>
        /// 矫正赋值器,在计算前一刻执行。
        /// </summary>
        public EpochInfoReviseManager ProducingReviser { get; set; }
        /// <summary>
        /// 写RINEX
        /// </summary>
        public RinexObsFileWriter RinexObsFileWriter { get; set; }

        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();

            var outPath = Path.Combine(OutputDirectory, Path.GetFileName(FilePath));
            RinexObsFileWriter = new Data.Rinex.RinexObsFileWriter(outPath, Option.Version);

            this.EpochInfoToRinex = new Domain.EpochInfoToRinex(Option.Version,false);

            this.RawReviser = new EpochInfoReviseManager();
            var dataSource =this.BufferedStream.DataSource as RinexFileObsDataSource;
            RawReviser.AddProcessor(new ClockJumpReviser(dataSource.ObsInfo.Interval));
            RawReviser.Init();
              
        }

        public override void Process(EpochInformation epochInfo)
        {
            RawReviser.Revise(ref epochInfo);

            if (Previous != null)
            {


            }
            if (this.CurrentIndex == 0)
            {
                var dataSource = this.BufferedStream.DataSource as RinexFileObsDataSource;
                var header = dataSource.Header;
                RinexObsFileWriter.WriteHeader(header);
            }

            var epochObs = this.EpochInfoToRinex.Build(epochInfo);
            RinexObsFileWriter.WriteEpochObservation(epochObs);            
        }

        public override void PostRun()
        {
            base.PostRun();
            RawReviser.Complete();
            RinexObsFileWriter.Dispose();
            log.Info("处理完毕！");
        }

        /// <summary>
        /// 缓存数据流
        /// </summary>
        /// <returns></returns>
        protected override BufferedStreamService<EpochInformation> BuildBufferedStream()
        {
            var DataSource = new RinexFileObsDataSource(FilePath);
            var bufferStream = new BufferedStreamService<EpochInformation>(DataSource, Option.BufferSize);
            return bufferStream;
        }
  
    }

 
}
