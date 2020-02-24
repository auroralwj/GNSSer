//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;

namespace Gnsser.Correction
{

    public interface IRangeCorrector : IRangeCorrector<EpochSatellite>
    {
    }
}