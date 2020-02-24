//2018.08.15，czs, create in hmx, 钟跳探测器

using System;
using System.Collections.Generic;
using System.Text;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Geo;
using Geo.Utils;
using Geo.Common;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.IO;

namespace Gnsser
{

    /// <summary>
    /// 钟跳探测器
    /// </summary>
    public class ClockJumpDetector : EpochInfoReviser
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ClockJumpDetector(GnssProcessOption Option)
        {
            log = Log.GetLog(typeof(ClockJumpDetector));
            this.IsRepaire = Option.IsClockJumpReparationRequired;

            MaxThresholdSeconds = 1E-3 - 10000 / GnssConst.LIGHT_SPEED; //误差范围
            Ratio = 0.5;
            int prevBufferSize = 5;
            PrevBuffers = new WindowData<EpochInformation>(prevBufferSize);
            TotalClockJumpState = ClockJumpState.Ok;


        }
        #region  属性
        /// <summary>
        /// 是否修复
        /// </summary>
        public bool IsRepaire { get; set; }

        /// <summary>
        /// 同时出现钟跳的比列，大于此则认为钟跳。
        /// </summary>
        public double Ratio { get; set; }
        /// <summary>
        /// 最大阈值,单位：秒
        /// </summary>
        public double MaxThresholdSeconds { get; set; }

        /// <summary>
        /// 总共跳了多少
        /// </summary>
        public double CorrectionOfTotalJumpedSeconds { get; set; }
        /// <summary>
        /// 当前跳跃
        /// </summary>
        public double CorrectionOfCurrrentJumpedSeconds { get; set; }
       

         

        WindowData<EpochInformation> PrevBuffers { get; set; }
        /// <summary>
        /// 存储上一个
        /// </summary>
        public EpochInformation Prev { get; set; }
        /// <summary>
        /// 上二个
        /// </summary>
        EpochInformation Prev2 { get; set; }
        /// <summary>
        /// 全局周跳状态
        /// </summary>
        public ClockJumpState TotalClockJumpState { get; set; }
        /// <summary>
        /// 添加前一个
        /// </summary>
        /// <param name="obj"></param>
        public ClockJumpDetector SetPrev(EpochInformation obj)
        {
            this.Prev2 = Prev;
            this.Prev = obj;
            PrevBuffers.Add(Prev);
            return this;
        }
        #endregion

        /// <summary>
        /// 探测
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation current)
        {
            if (Prev == null)
            {
                SetPrev(current);
                return true;
            }
            if (Prev.ReceiverTime == current.ReceiverTime) { return true; }//避免重复调用


            //如果选择了修复，需要将进来的修理成以前的一个尺度，再进行判断
            if (IsRepaire && CorrectionOfTotalJumpedSeconds != 0)
            {
                Repair(ref current, CorrectionOfTotalJumpedSeconds, TotalClockJumpState);
            }

            //探测，只需要上一个和当前比较
            var ClockJumpState = Detect(this.Prev, current, MaxThresholdSeconds, Ratio);

            if (ClockJumpState != ClockJumpState.Ok)
            {
                TotalClockJumpState = ClockJumpState;
                log.Warn(current.Name + " " + current.ReceiverTime + " 发生钟跳！" + ClockJumpState);

                if (Buffers == null || Buffers.Count == 0) { return true; }
                //计算跳秒改正数
                double jumped = GetJumpSecondCorrection(current);

                CorrectionOfCurrrentJumpedSeconds = jumped;

                CorrectionOfTotalJumpedSeconds += CorrectionOfCurrrentJumpedSeconds;
                log.Warn(current.Name + " " + current.ReceiverTime + " 钟跳大小：" + CorrectionOfCurrrentJumpedSeconds + ", 总跳： " + CorrectionOfTotalJumpedSeconds);

                if (IsRepaire)
                {
                    Repair(ref current, CorrectionOfCurrrentJumpedSeconds, ClockJumpState); //前面已经修复过一次了，这里补充修复
                    log.Info(current.Name + " " + current.ReceiverTime + " 尝试修复了周跳！" + current.EpochState);

                    //探测检核一下
                    //var checkState = Detect(this.Prev, current, MaxThresholdSeconds, Ratio);
                    //if (checkState != ClockJumpState.Ok)
                    //{
                    //    int i = 0;
                    //}

                    //var check = GetClockJumpSeconds(this.Prev2, this.Prev, current, next, FrequenceType.A);
                    //if (Math.Abs(check) > 0.00001)
                    //{
                    //    int a = 0;
                    //}

                }
            }

            SetPrev(current);

            return true;
        }

        /// <summary>
        /// 计算跳秒改正数。初步修复。
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private double GetJumpSecondCorrection(EpochInformation current)
        {
            var firstBuffer = Buffers[0];
            var next = firstBuffer;
            if (CorrectionOfTotalJumpedSeconds != 0)
            {
                next = next.ValueClone();
                //注意：前面的已经修复过了，后面的没有修复，因此当前判断next需要用修复后的数据,统一到一个标准
                Repair(ref next, CorrectionOfTotalJumpedSeconds, TotalClockJumpState);
            }
            var jumpedA = GetClockJumpSeconds(this.Prev2, this.Prev, current, next, FrequenceType.A);
            var jumpedB = GetClockJumpSeconds(this.Prev2, this.Prev, current, next, FrequenceType.B);
            var jumped = (jumpedA + jumpedB) / 2.0;

            return jumped;
        }

        /// <summary>
        /// 探测是否钟跳，返回结果
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="current"></param>
        /// <param name="MaxThresholdSeconds"></param>
        /// <param name="Ratio"></param>
        /// <returns></returns>
        private static ClockJumpState Detect(EpochInformation prev, EpochInformation current,double MaxThresholdSeconds, double Ratio)
        {
            ClockJumpState clockJumpState = ClockJumpState.Ok;
            int jumpCountOfL = 0;
            int jumpCountOfP = 0;

            foreach (var sat in current)
            {
                if (prev.Contains(sat.Prn))
                {
                    var prevSat = prev[sat.Prn];
                    //这里只比较载波A即可
                    var differLSecond = (sat.FrequenceA.PhaseRange.Value - prevSat.FrequenceA.PhaseRange.Value) / GnssConst.LIGHT_SPEED;


                    if (Math.Abs(differLSecond) > MaxThresholdSeconds)
                    {
                        jumpCountOfL++;
                    }
                    var differPSecond = (sat.FrequenceA.PseudoRange.Value - prevSat.FrequenceA.PseudoRange.Value) / GnssConst.LIGHT_SPEED;
                    if (Math.Abs(differPSecond) > MaxThresholdSeconds)
                    {
                        jumpCountOfP++;
                    }
                }
            }
            var minCount = Ratio * current.Count;
            if (jumpCountOfL > minCount)
            { 
                if (jumpCountOfP > minCount)
                {
                    clockJumpState = ClockJumpState.ClockJumped;
                    current.EpochState = EpochState.ClockJumped;
                }
                else
                {
                    clockJumpState = ClockJumpState.ClockJumpedPhaseOnly;
                    current.EpochState = EpochState.ClockJumpedPhaseOnly;
                }
            }
            else if (jumpCountOfP > minCount)
            {
                clockJumpState = ClockJumpState.ClockJumpedRangeOnly;
                current.EpochState = EpochState.ClockJumpedRangeOnly;
            }
            
            return clockJumpState;
        }

         
        /// <summary>
        /// 最简单的4历元钟跳修复法，返回改正数。
        /// </summary>
        /// <param name="prev2"></param>
        /// <param name="prev1"></param>
        /// <param name="current"></param>
        /// <param name="next"></param>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public double GetClockJumpSeconds(EpochInformation prev2, EpochInformation prev1, EpochInformation current, EpochInformation next, FrequenceType frequenceType)
        {
            List<SatelliteNumber> commonPrns = Geo.Utils.ListUtil.GetCommons<SatelliteNumber>(current.Keys, prev1.Keys, prev2.Keys, next.Keys);
              
            //比较二者预报之差求后平均
            var currentPreditceDiffer = GetPredictedDiffer(prev2, prev1, current, commonPrns, frequenceType);
            //var aveVal0 = Geo.Utils.DoubleUtil.Average(currentPreditceDiffer.Values);

            //var seconds0 = 1.0 * aveVal0 / GnssConst.LIGHT_SPEED;
            //return seconds0;

            var prevPreditceDiffer = GetPredictedDiffer(next, current, prev1, commonPrns, frequenceType);//这个符号是反的 
            //求预报平均值
            var aves = new BaseDictionary<SatelliteNumber, double>();
            foreach (var item in commonPrns)
            {
                aves[item] = (currentPreditceDiffer[item] - prevPreditceDiffer[item]) / 2.0;
            }
            var aveVal = Geo.Utils.DoubleUtil.Average(aves);

            var seconds = aveVal / GnssConst.LIGHT_SPEED;
            return seconds;
        }

        /// <summary>
        /// 通过前两个历元，预报当前历元，并与当前历元实际数值作差。返回预报差值。
        /// </summary>
        /// <param name="prev2"></param>
        /// <param name="prev1"></param>
        /// <param name="current"></param>
        /// <param name="commonPrns"></param>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public BaseDictionary<SatelliteNumber, double> GetPredictedDiffer(
            EpochInformation prev2,
            EpochInformation prev1,
            EpochInformation current,
            List<SatelliteNumber> commonPrns,
            FrequenceType frequenceType)
        {
            var predictedCurrent = GetPredicted(prev2, prev1, commonPrns, frequenceType);
            //最后比较预报与实际二者之差
            var currentPreditceDiffer = new BaseDictionary<SatelliteNumber, double>();
            foreach (var item in commonPrns)
            {
                var predictValue =   predictedCurrent[item];                 
                var trueVal = current[item][frequenceType].PhaseRange.Value;

                var differ = predictValue - trueVal;

                currentPreditceDiffer[item] = differ;
            }
            return currentPreditceDiffer;
        }

        /// <summary>
        /// 通过前两个预报当前值。预报值采用上一段差值。
        /// </summary>
        /// <param name="prev1"></param>
        /// <param name="prev2"></param>
        /// <param name="commonPrns"></param>
        /// <param name="frequenceType"></param>
        /// <returns></returns>
        public BaseDictionary<SatelliteNumber, double> GetPredicted(
            EpochInformation prev2,
            EpochInformation prev1, 
            List<SatelliteNumber> commonPrns,
            FrequenceType frequenceType)
        {
            //从前和后，两个方向预报当前星和上一个的大小
            var predictedCurrent = new BaseDictionary<SatelliteNumber, double>();
            foreach (var item in commonPrns)
            {
                var prev2Val = prev2[item][frequenceType].PhaseRange.Value;
                var prev1Val = prev1[item][frequenceType].PhaseRange.Value;
                predictedCurrent[item] = prev1Val + (prev1Val - prev2Val);//上一减去上二，得钟跳前差,以此差预报下一个 
            } 

            return predictedCurrent;
        }

        /// <summary>
        /// 直接修复,按照指定的修复。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="seonds"></param>
        /// <param name="clockJumpState"></param>
        public void Repair(ref EpochInformation obj, double seonds, ClockJumpState clockJumpState = ClockJumpState.ClockJumped)
        {
            obj.CorrectClockJump(seonds, clockJumpState); 
        }
    }
}
