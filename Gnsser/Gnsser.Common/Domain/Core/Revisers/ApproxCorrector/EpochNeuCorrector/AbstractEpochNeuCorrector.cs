//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser.Domain; 
using Geo.Correction;

namespace Gnsser.Correction
{ 

    /// <summary>
    ///  历元测站坐标改正器。
    /// </summary>
    public abstract class AbstractEpochNeuReviser : AbstractCorrector<NEU, EpochInformation> 
    {

        public CorrectionType CorrectionType { get; protected set; }
         
        public override abstract void Correct(EpochInformation TInput);
    }
}