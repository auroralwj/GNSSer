//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Correction;
using Geo.Correction;

namespace Gnsser.Domain
{

    /// <summary>
    /// 观测量，包含载波、伪距、多普勒，观测值得组合观测值等。
    /// 观测量由观测值和其改正组数成。
    /// </summary>
    public interface IObservation : ICorrectableNumeral
    {

    }
}
