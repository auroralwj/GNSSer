//2014.09.20, czs, create, 依据不同的频率进行距离改正

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Correction;
using Geo.Coordinates;
using Gnsser.Domain;

namespace Gnsser.Correction
{ 
    /// <summary>
    /// 依据不同的频率进行距离改正
    /// </summary>
    /// <typeparam name="TInputObject">改正时，需要传入的对象类型</typeparam>
    public interface IFrequenceBasedNeuCorrector<TInputObject> : ICorrector<Dictionary<RinexSatFrequency, NEU>, TInputObject>, ICorrector
    {

    }

    /// <summary>
    ///  依据频率进行卫星距离的改正器基类。
    /// </summary>
    public abstract class AbstractFreqBasedNeuCorrector : AbstractCorrector<Dictionary<RinexSatFrequency, NEU>, EpochInformation>, IFrequenceBasedNeuCorrector<EpochInformation>
    {
        public CorrectionType CorrectionType { get; protected set; }
        public override abstract void Correct(EpochInformation input);
    }
}
