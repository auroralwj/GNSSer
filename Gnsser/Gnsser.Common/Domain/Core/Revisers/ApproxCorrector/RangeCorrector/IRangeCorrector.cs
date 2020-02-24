//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Correction;

namespace Gnsser.Correction
{


    /// <summary>
    /// 数值改正器顶层接口
    /// </summary>
    /// <typeparam name="TInputObject">改正时，需要传入的对象类型</typeparam>
    public interface IRangeCorrector<TInputObject> : ICorrector<Double, TInputObject>, ICorrector
    {
    }
     
}
