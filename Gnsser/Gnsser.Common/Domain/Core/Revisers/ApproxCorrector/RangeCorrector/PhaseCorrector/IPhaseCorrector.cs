//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Geo.Correction;

namespace Gnsser.Correction
{
    /// <summary>
    /// 相位改正
    /// </summary>
    public interface IPhaseCorrector : IRangeCorrector<EpochSatellite>
    {
    }

    /// <summary>
    ///  卫星距离改正器基类。
    /// </summary>
    public abstract class AbstractPhaseCorrector : GnssCorrectorChain<double, EpochSatellite>, IRangeCorrector<EpochSatellite>, IPhaseCorrector
    {
        /// <summary>
        /// 改正数类型
        /// </summary>
        public CorrectionType CorrectionType { get; protected set; }
        /// <summary>
        /// 改正器类型
        /// </summary>
        public CorrectChianType CorrectChianType { get; set; }
        /// <summary>
        /// 改正
        /// </summary>
        /// <param name="input"></param>
        public override abstract void Correct(EpochSatellite input);
    }
}