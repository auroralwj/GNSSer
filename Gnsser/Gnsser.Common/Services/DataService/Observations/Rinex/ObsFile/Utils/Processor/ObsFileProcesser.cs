//2015.02.09, czs, create in 麦克斯 双辽， 观测文件观测类型管理器

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Geo.Utils;
using System.Text;
using Geo.Coordinates; 
using Gnsser.Times;
using Geo;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 责任链
    /// </summary>
    public class ObsFileProcesserChain : ReviserManager<RinexObsFile>
    {
        public ObsFileProcesserChain() { }


    }


    /// <summary>
    /// 观测文件处理器。
    /// </summary>
    abstract public class ObsFileProcesser : Reviser<RinexObsFile>
    {
    }

    /// <summary>
    /// 观测文件处理器。
    /// </summary>
    abstract public class EpochObsProcesser : Reviser<RinexEpochObservation>
    {

    }

}