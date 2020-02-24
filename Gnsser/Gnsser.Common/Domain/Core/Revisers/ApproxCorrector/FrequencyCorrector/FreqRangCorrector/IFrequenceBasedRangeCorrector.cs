//2014.09.20, czs, create, 依据不同的频率进行距离改正

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Geo.Correction;
using Gnsser.Domain;

namespace Gnsser.Correction
{ 
    /// <summary>
    /// 依据不同的频率进行距离改正
    /// </summary>
    /// <typeparam name="TInputObject">改正时，需要传入的对象类型</typeparam>
    public interface IFrequenceBasedRangeCorrector<TInputObject> : ICorrector<Dictionary<RinexSatFrequency, double>, TInputObject>, ICorrector
    {

    }

    /// <summary>
    ///  依据频率进行卫星距离的改正器基类。
    /// </summary>
    public abstract class AbstractFreqBasedRangeCorrector : AbstractCorrector<Dictionary<RinexSatFrequency, double>, EpochSatellite>, IFrequenceBasedRangeCorrector<EpochSatellite>
    {
        /// <summary>
        /// 改正类型
        /// </summary>
        public CorrectionType CorrectionType { get; protected set; }
        /// <summary>
        /// 执行改正
        /// </summary>
        /// <param name="input"></param>
        public override abstract void Correct(EpochSatellite input);
    }
}
