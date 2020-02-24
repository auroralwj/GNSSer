//2015.04.24, czs, eidt in namu， 此类设置不合理，需要重新设计和区分，分为可忽略错误和不可忽略错误。
//2016.05.01, czs, edit in hongqing, 重命名为 ObsValueValidityChecker

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Domain;
using Geo.Times;


namespace Gnsser.Checkers
{
    /// <summary>
    /// 观测数据合法性检核。
    /// 包括频率数量，数值区间等。致命性错误，无频率不足，则直接返回false，其它的如观测量问题则标记。
    /// </summary>
    public class EpochObsValueValidityChecker : EpochInfoChecker, IChecker<MultiSiteEpochInfo>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GnssOption">设置</param>
        public EpochObsValueValidityChecker(GnssProcessOption GnssOption)
        {
            this.Option = GnssOption;
            checkPhase = GnssOption.IsPhaseValueRequired;
        }
        /// <summary>
        /// 卫星数量
        /// </summary>
        public bool checkPhase { get; set; }

        GnssProcessOption Option { get; set; }
         

        private string BuildCountNotEnoughMessage(string name, int required, int only)
        {
            return name + "数量不足，要求 " + required + ", 只有 " + only + ", 检核不通过！";
        }

        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="epochInfo"></param>
        public override bool Check(EpochInformation epochInfo)
        {
            List<SatelliteNumber> notPassedSats = new List<SatelliteNumber>(); 
            var infoHeader = epochInfo.Name + " " + epochInfo.ReceiverTime;

            if (epochInfo.Time.Equals(Time.MinValue) || epochInfo.Time.Equals(Time.Zero))
            {
                return false;
            }

            if (epochInfo.Count == 0)
            {
                log.Error(infoHeader + " 没有数据内容。");
                return false;
            }


            //频率数量通常代表了历元所有的频率
            var first = epochInfo.First;
            if (first.FrequencyCount < Option.MinFrequenceCount)
            {
                log.Error(infoHeader + BuildCountNotEnoughMessage(" 频率", Option.MinFrequenceCount, first.FrequencyCount));
                return false;
            }

            //逐渐检查所有卫星
            foreach (var sat in epochInfo)
            {
                var satInfoHeader = infoHeader + " " + sat.Prn;

                int freqIndex = 0;
                bool isSatPassed = true;
                foreach (var freq in sat)//检查观测值
                {
                    if (freqIndex >= Option.MinFrequenceCount)
                    {
                        continue;//指定频率数量外的不再检查
                    }

                    var freqHeader = satInfoHeader + " " + freq;
                    if (Option.IsRangeValueRequired)
                    {
                        var val = freq.PseudoRange.Value;
                        if (freq.PseudoRange.Value == 0)
                        {
                            isSatPassed = false;
                            var msg = freqHeader + " 伪距数值为 0，已标记停用";
                            log.Debug(msg);
                            freq.Enabled = false;
                            freq.Message += msg;
                        }else if( !Geo.Utils.DoubleUtil.IsBetween(freq.PseudoRange.Value, Option.MinAllowedRange, Option.MaxAllowedRange  )){

                            isSatPassed = false;
                            var msg = freqHeader + " 伪距数值为 "+val+"，不在允许范围 [" +Option.MinAllowedRange + " -> "+ Option.MaxAllowedRange  + "]";
                            log.Debug(msg);
                            freq.Enabled = false;
                            freq.Message += msg;
                        }
                    }

                    if (freq.PhaseRange.Value == 0 && Option.IsPhaseValueRequired)
                    {
                        isSatPassed = false; //lly 2017.09.04增加,载波=0的时候，也需要标记为FALSE，见stj20010.13O 00:10:00 G01卫星，缺失L2观测值
                        var msg = freqHeader + " 载波数值为 0，已标记停用";
                        log.Debug(msg);
                        freq.Enabled = false;
                        freq.Message += msg;
                    }

                    freqIndex++;
                } 
                if (!isSatPassed && !notPassedSats.Contains(sat.Prn)) { notPassedSats.Add(sat.Prn); }
            }

            if (notPassedSats.Count > 0)
            {
                if (Option.IsRemoveOrDisableNotPassedSat)
                {
                    epochInfo.Remove(notPassedSats, true, "没有通过检查，包括星历、数值等");
                }
                else
                {
                    epochInfo.Disable(notPassedSats);
                } 
            }

            if (epochInfo.EnabledSatCount < Option.MinSatCount)
            {
                log.Error(infoHeader + BuildCountNotEnoughMessage(" 有效卫星", Option.MinSatCount, epochInfo.EnabledSatCount));
                return false;
            }

            return true;

        }

        public bool Check(MultiSiteEpochInfo t)
        {
            //foreach (var key in t)
            //{
            //    if (!Check(key))
            //    { key.Clear(); t.Remove(key.Name); }// { return false; }
            //}
            
            for (int i = 0; i < t.Count; i++)
            {
                if (!Check(t.Values[i]))
                {
                    t.Remove(t.Values[i].Name);
                    i--;
                }// { return false; }

            }
            return true;
        }
    }
}
