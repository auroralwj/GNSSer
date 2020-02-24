//2015.10.26, czs, edit, 提取接口，新增抽象责任链

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Geo.IO;


namespace Gnsser.Checkers
{
    /// <summary>
    /// 历元信息检核
    /// </summary>
    public abstract class EpochInfoChecker : Checker<EpochInformation>, IEpochInfoChecker
    {
        public Log log = new Log(typeof(EpochInfoChecker));
        public EpochInfoChecker() { Name = "历元信息检核"; }
    }
   


}
