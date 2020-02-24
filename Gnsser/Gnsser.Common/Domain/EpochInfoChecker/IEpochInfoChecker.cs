//2015.10.26, czs, edit in hongqing, 提取抽象接口，对某一对象进行检核

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Geo;

namespace Gnsser.Checkers
{ 

    /// <summary>
    /// 历元信息检核
    /// </summary>
    public interface IEpochInfoChecker : IChecker<EpochInformation>
    {


    }
}
