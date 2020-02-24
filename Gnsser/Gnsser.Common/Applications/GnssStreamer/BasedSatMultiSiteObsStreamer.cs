//2016.08.20, czs, create in 福建永安, 宽项计算器，多站观测数据遍历器
//2016.08.29, czs, edit in 西安洪庆, 重构多站观测数据遍历器

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Gnsser
{
    /// <summary>
    /// 具有基准卫星的多站数据流
    /// </summary>
    public abstract class BasedSatMultiSiteObsStreamer : MultiSiteObsStreamer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BasePrn"></param>
        /// <param name="OutputDirectory"></param>
        public BasedSatMultiSiteObsStreamer(SatelliteNumber BasePrn, string OutputDirectory)
            : base(OutputDirectory)
        {
            this.BasePrn = BasePrn;
        }


        /// <summary>
        /// 基础卫星编号
        /// </summary>
        public SatelliteNumber BasePrn { get; set; }
        /// <summary>
        /// 预处理
        /// </summary>
        /// <param name="mEpochInfo"></param>
        /// <returns></returns>
        public override LoopControlType PreProcess(MultiSiteEpochInfo mEpochInfo)
        {
            var loop = CheckLoop(mEpochInfo);
            if (loop != LoopControlType.GoOn) { return loop; }

            //避免无谓工作
            if (!mEpochInfo.BaseEpochInfo.Contains(BasePrn.SatelliteType)) { log.Error("糟糕！是不是设置错了？基准数据 " + mEpochInfo.BaseEpochInfo.Name + " 不包含系统： " + BasePrn.SatelliteType); return LoopControlType.Continue; }
            if (!mEpochInfo.TotalPrns.Contains(BasePrn)) { return LoopControlType.Continue; }

            return ProducingRevise(mEpochInfo);
        }
        /// <summary>
        /// 构建选项
        /// </summary>
        /// <returns></returns>
        protected override GnssProcessOption BuildGnssOption()
        {
            if (Option == null) { Option = new GnssProcessOption(); }
            Option.OutputDirectory = this.OutputDirectory;
            return Option;
        }
    }
}