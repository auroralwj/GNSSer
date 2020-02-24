//2018.12.31, czs, create in hmx, 单系统的无电离层模糊度计算器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Times;
using Geo.IO;

namespace Gnsser.Service
{
  
    
    /// <summary>
    /// 多系统无电离层模糊度计算器
    /// </summary>
    public class IonoFreeAmbiguitySolverManager:BaseDictionary<SatelliteType, IonoFreeAmbiguitySolver>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IonoFreeAmbiguitySolverManager()
        {

        }
        public override IonoFreeAmbiguitySolver Create(SatelliteType key)
        {
            return new IonoFreeAmbiguitySolver();
        }
    }

    /// <summary>
    /// 单系统的无电离层模糊度计算器
    /// </summary>
    public class IonoFreeAmbiguitySolver
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IonoFreeAmbiguitySolver()
        {

        }
        /// <summary>
        /// 是否初始化，减少计算量
        /// </summary>
        public bool IsInited { get; set; }
        /// <summary>
        /// 用于提取卫星频率的时间（GLO需要），一般不用改变
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 第一频率，单位 10^6
        /// </summary>
        public double FreqA { get; set; }
        /// <summary>
        /// 第二频率，单位 10^6
        /// </summary>
        public double FreqB { get; set; }
        /// <summary>
        /// 第一频率波长
        /// </summary>
        public double WaveLengthOfL1 { get; set; }
        /// <summary>
        /// 宽巷波长
        /// </summary>
        public double WaveLenOfWideLane { get; set; }
        /// <summary>
        /// 窄巷波长
        /// </summary>
        public double WaveLenOfNarrowLane { get; set; }
        /// <summary>
        /// 模糊度是否以距离为单位
        /// </summary>
        public  bool IsCycleOrMeterOfAmbiUnit{ get; set; }
        private double tempCoeef { get; set; }
        private double tempCoeef2 { get; set; }
        private double tempCoeef3 { get; set; }

        /// <summary>
        /// 检查并初始化
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="time">用于计算频率，非GLO可以忽略</param>
        /// <param name="isCycleAmbi">模糊度单位是否为周</param>
        /// <param name="forceToInit">强制初始化，GLO系统适用,也可以手动指定</param>
        public void CheckOrInit(SatelliteNumber prn, Time time, bool isCycleAmbi, bool forceToInit=false)
        {
            if(prn.SatelliteType == SatelliteType.R) { forceToInit = true;  }

            if (!IsInited || forceToInit)
            {
                this.Time = time;
                this.IsCycleOrMeterOfAmbiUnit = isCycleAmbi;
                var freq1 = Frequence.GetFrequenceA(prn, time);
                FreqA = freq1.Value;
                WaveLengthOfL1 = freq1.WaveLength;
                FreqB = Frequence.GetFrequenceB(prn, time).Value;
                WaveLenOfWideLane = Frequence.GetMwFrequence(prn, time).WaveLength;
                WaveLenOfNarrowLane = Frequence.GetNarrowLaneFrequence(prn, time).WaveLength;

                tempCoeef = FreqB / (FreqA + FreqB);                          //算法1
                tempCoeef2 = (FreqA + FreqB) * 1e6 / GnssConst.LIGHT_SPEED;//算法2：单位转换,窄巷波长的倒数
                tempCoeef3 = FreqB / (FreqA - FreqB);                         //算法2
                IsInited = true;
            }
        }

        /// <summary>
        /// 计算窄巷模糊度，单位周
        /// </summary>
        /// <param name="wideIntCyle">以周为单位的宽巷整数</param>
        /// <param name="floatIfAmbiValue">以距离为单位模糊度浮点解</param>
        /// <returns></returns>
        public NameRmsedNumeralVector GetNarrowFloatValue(NameRmsedNumeralVector wideIntCyle, NameRmsedNumeralVector floatIfAmbiValue)
        {
            NameRmsedNumeralVector floatIfAmbLen = floatIfAmbiValue;
            if (IsCycleOrMeterOfAmbiUnit)
            {
                floatIfAmbLen = floatIfAmbiValue *  WaveLengthOfL1; //转换为距离，以L1为参考
            }

            var narrowFloat = (floatIfAmbLen - tempCoeef * WaveLenOfWideLane * wideIntCyle) / WaveLenOfNarrowLane;//算法1
    
            return narrowFloat;
        }

         
        /// <summary>
        /// 计算无电离层组合模糊度,返回单位按照 指定的相位单位
        /// </summary>
        /// <param name="wideIntCyle">以周为单位的宽巷整数</param>
        /// <param name="narrowIntCyle">以周为单位的窄巷整数</param> 
        /// <returns>返回以距离或周为单位的模糊度（波长默认L1）</returns>
        public NameRmsedNumeralVector GetIonoFreeAmbiValue(NameRmsedNumeralVector wideIntCyle, NameRmsedNumeralVector narrowIntCyle)
        {
            var IonoFreeAmbiValueLen = (tempCoeef * WaveLenOfWideLane * wideIntCyle) + (narrowIntCyle * WaveLenOfNarrowLane);//算法1
            if (IsCycleOrMeterOfAmbiUnit)
            {
                return IonoFreeAmbiValueLen / WaveLengthOfL1;
            }
            return IonoFreeAmbiValueLen;
        }
    }
} 