

using System;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using System.Collections.Generic;
using System.Collections;
using Geo.Correction;

using Gnsser.Domain;
namespace Gnsser.Correction
{

    /// <summary>
    /// 相位距离改正。
    /// 卫星改正的责任链,是一组改正对象的组合。一般采用此类将各种改正进行组合。
    /// </summary>
    public class PhaseCorrectionReviser : GnssCorrectorChain<double, EpochSatellite>, IRangeCorrector<EpochSatellite>, IPhaseCorrector, IEpochInfoReviser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PhaseCorrectionReviser()
        { 
            this.Name = "相位距离改正集合";
            this.CorrectionType = Gnsser.Correction.CorrectionType.PhaseChain;
        }
        /// <summary>
        /// 改正器类型
        /// </summary>
        public CorrectChianType CorrectChianType { get; set; }
        public CorrectionType CorrectionType { get; protected set; }
        /// <summary>
        /// 缓存
        /// </summary>
        public Geo.IWindowData<EpochInformation> Buffers { get; set; }

        /// <summary>
        /// 执行信息
        /// </summary>
        public string Message { get; set; }

        public override void Correct(EpochSatellite TInput)
        {
            this.Corrections = new Dictionary<string, double>();
            double val = 0;
            foreach (var item in Correctors)
            {
                item.Correct(TInput);
                val += item.Correction;

                this.Corrections.Add(item.Name, item.Correction);
            }
            //可在此设断点查看各个改正情况。
           this.Correction = (val);
        }

        public bool Revise(ref EpochInformation info)
        { 
            foreach (var sat in info)//分别对指定卫星指定频率进行改正
            {

                if (!sat.Enabled) continue;

                this.Correct(sat);
                double val = this.Correction;

                sat.AddCommonCyleCorrection(this.Name, val);  //这只有天线相位缠绕改正，同时加到了伪距和相位上。
              
            //    sat.AddPhaseCyleCorrection(this.Name, val);
            }
            return true;
        }
    } 
}
