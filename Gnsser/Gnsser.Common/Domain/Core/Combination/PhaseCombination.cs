//2014.09.14, czs, create, 观测量，无电离层模糊度计算
//2014.04.24, czs, edit in hongqing, 增加测站信息

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using System.IO;
using Geo;

namespace Gnsser.Domain
{
    /// <summary>
    /// 无电离层模糊度计算，
    /// </summary>
    public class IonoFreeAmbiguityMgr :BaseDictionary<String, IonoFreeCombination>
    {
        /// <summary>
        /// 无电离层模糊度计算
        /// </summary>
        public IonoFreeAmbiguityMgr()
        {
         
        }

        /// <summary>
        /// 设置无电离层组合
        /// </summary>
        /// <param name="result"></param>
        public void SetIonoFreeCombination(PppResult result){
            EpochInformation epochInfo = result.MaterialObj;
            var nameBuilder = (GnssParamNameBuilder)result.NameBuilder;
            foreach (var sat in epochInfo.EnabledSats)
	        {
                var key = nameBuilder.GetParamName(sat.Prn);

                if (!this.Contains(key)) this[key] = new IonoFreeCombination(sat.FrequenceA.PhaseRange, sat.FrequenceB.PhaseRange);

                double ambiDistance = result.GetAmbiguityDistance(epochInfo.SiteName, sat.Prn);

                this[key].SetAmbiguity(ambiDistance);
                var amb = this[key];
                long ambiA = amb.NarrowPhase.Ambiguity;
                long ambiB = ambiA - amb.WidePhase.Ambiguity;

                //string msg = ambiA + "\t" + ambiB + "\r\n";
                //string path = "C:\\GnsserOutput\\模糊度\\" + result.EpochInfo.obsPath.MarkerName + "_" + result.EpochInfo.ReceiverTime.ToDateString() + "\\" + sat.Prn + "模糊度.txt";
                //if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));
                //System.IO.File.AppendAllText(path, msg); 
	        }
        }

        public void SetIonoFreeCombination(ClockEstimationResult result)
        {
            var epochInfos = result.MaterialObj;
            //var matrixBuilder = result.GnssSolver.MatrixBuilder;
            var nameBuilder = (ClockParamNameBuilder)result.NameBuilder;
                            
            foreach (var epochInfo in epochInfos)
            {
                foreach (var sat in epochInfo.EnabledSats)
                {
                    var key = nameBuilder.GetSiteSatAmbiguityParamName( sat);
                    if (!this.Contains(key)) this[key] = new IonoFreeCombination(sat.FrequenceA.PhaseRange, sat.FrequenceB.PhaseRange);

                    double ambiDistance = result.GetAmbiguityDistace(epochInfo.SiteName, sat.Prn);

                    this[key].SetAmbiguity(ambiDistance);
                    var amb = this[key];
                    long ambiA = amb.NarrowPhase.Ambiguity;
                    long ambiB = ambiA - amb.WidePhase.Ambiguity;

                    //string msg = ambiA + "\t" + ambiB + "\r\n";
                    //string path = "C:\\GnsserOutput\\模糊度\\" + result.EpochInfo.obsPath.MarkerName + "_" + result.EpochInfo.ReceiverTime.ToDateString() + "\\" + sat.Prn + "模糊度.txt";
                    //if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));
                    //System.IO.File.AppendAllText(path, msg); 
                }
            }

        }

        /// <summary>
        /// 消除电离层组合。
        /// </summary>
        //public Dictionary<SatelliteNumber, IonoFreeCombination> Data { get; set; }
    }


    //2015.01.02, czs, create in namu, 电离层无关频率组合

    /// <summary>
    /// 电离层无关频率组合.
    ///  采用宽项组合固定模糊度。
    /// </summary>
    public class IonoFreeCombination : PhaseCombination
    {
        public IonoFreeCombination(double val = 0, Frequence Frequence = null)
            : base(val, Frequence)
        { 
        }
        /// <summary>
        /// 构造函数。采用两个频率初始化。
        /// </summary>
        /// <param name="A">频率A</param>
        /// <param name="B">频率B</param>
        public IonoFreeCombination(PhaseRangeObservation A = null, PhaseRangeObservation B = null) 
        {
            PhaseCombination combination = PhaseCombinationBuilder.GetIonoFreeRangeCombination(A.Value, B.Value, A.Frequence, B.Frequence);
            this.Value = combination.Value;
            this.Frequence = combination.Frequence;

            double f1 = A.Frequence.Value;
            double f2 = B.Frequence.Value;

            this.WideFactor =(f1 * f2) / (f1 * f1 - f2 * f2);
            this.NarrowFactor =  f1 / (f1 + f2);

            Frequence wideFrequence = new Gnsser.Frequence("WidePhase", A.Frequence.Value - B.Frequence.Value);

            this.NarrowPhase = new PhaseValue(0, A.Frequence);
            this.WidePhase = new PhaseValue(0, wideFrequence);

            this.FrequenceA = A.Frequence;
            this.FrequenceB = B.Frequence;
        }

        Frequence FrequenceA { get; set; }
        Frequence FrequenceB { get; set; }


        #region 属性
        /// <summary>
        /// 宽巷.相位A - 相位B。
        /// </summary>
        public PhaseValue WidePhase { get; set; }
        /// <summary>
        /// 窄巷.相位A。
        /// </summary>
        public PhaseValue NarrowPhase { get; set; }

        /// <summary>
        /// 模糊度系数，相位A。
        /// </summary>
        public double WideFactor { get; set; }

        /// <summary>
        /// 模糊度系数，相位A - 相位B。
        /// </summary>
        public double NarrowFactor { get; set; } 
        #endregion

        /// <summary>
        /// 直接取整。设置属性。
        /// </summary>
        /// <param name="length">数值</param>
        public void SetAmbiguity(double length)
        {
            //宽巷模糊度
            long NaMinusMb =  (long)(length / WideFactor / WidePhase.Frequence.WaveLength);
            this.WidePhase.Ambiguity = NaMinusMb;
            //窄巷模糊度
            double remained = length - WideFactor * this.WidePhase.PhaseRange;
            long ambiA =  (long)(remained / NarrowFactor / NarrowPhase.Frequence.WaveLength);
            this.NarrowPhase.Ambiguity = ambiA;

            long ambiB = ambiA - NaMinusMb;

            //返回验证 
            var facs = PhaseCombinationBuilder.GetIonoFreePhaseCycleCombFactors(FrequenceA, FrequenceB);

            //double remain2 = facs[0] * this.WidePhase.Ambiguity * FrequenceA.WaveLength + facs[1] * this.WidePhase.Ambiguity * FrequenceB.WaveLength;
            //int ambi2 = (int)(remain2 / FrequenceB.WaveLength);

            //验证
            double distance = WideFactor * this.WidePhase.PhaseRange + NarrowFactor * this.NarrowPhase.PhaseRange;
         //   double differ2 = ambi2 - this.NarrowPhase.Ambiguity;
            double differ = length - distance;
        } 
    } 


    /// <summary>
    /// 频率相位
    /// </summary>
    public class PhaseValue {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="phase">相位</param>
        /// <param name="Frequence">频率</param>
         public  PhaseValue(double phase, Frequence Frequence){
             this.Phase = phase;
             this.Frequence = Frequence;
         }
        
        /// <summary>
        /// 模糊度。
        /// </summary>
        public long Ambiguity { get; set; }

        /// <summary>
        /// 相位值
        /// </summary>
        public double Phase { get; set; } 
        /// <summary>
        /// 组合成新的频率,但并不是真实的频率。
        /// </summary>
        public Frequence Frequence { get; set; } 

        /// <summary>
        /// 频率相位值代表的距离。
        /// </summary>
        public double PhaseRange { get { return Frequence.WaveLength * (Phase + Ambiguity); } }

    }


    /// <summary>
    /// 相位组合。
    /// 通常是一颗卫星的观测值，主要是载波相位的组合，因此输入卫星观测值。
    /// </summary>
    public class PhaseCombination : Combination, IObservation
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">改正值</param>
        public PhaseCombination(double val = 0, Frequence Frequence = null)
            :base(val)
        { 
            this.Frequence = Frequence;
        }

        /// <summary>
        /// 组合成新的频率,但并不是真实的频率。
        /// </summary>
        public Frequence Frequence { get; set; } 
    }


}