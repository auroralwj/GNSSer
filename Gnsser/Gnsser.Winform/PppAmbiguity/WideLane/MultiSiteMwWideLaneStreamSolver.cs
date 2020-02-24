//2016.08.20, czs, create in 福建永安, 宽项计算器
//2016.08.29, czs, edit in 西安洪庆, 重构宽项计算器，精简代码
//2016.10.17, czs, edit in hongqing, 只输出宽项结果
//2016.10.19, czs, edit in hongqing, 只关注宽项，去除其它

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
    /// 宽项计算器。
    /// 读入多站数据流，解算宽项结果。
    /// </summary>
    public class MultiSiteMwWideLaneStreamSolver : BasedSatMultiSiteObsStreamer
    {
        ILog log = new Log(typeof(MultiSiteMwWideLaneStreamSolver));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="BasePrn"></param>
        /// <param name="OutputDirectory"></param>
        public MultiSiteMwWideLaneStreamSolver(SatelliteNumber BasePrn, string OutputDirectory, bool setWeightWithSat, double MaxError, bool isOutputDetail)
            :base(BasePrn, OutputDirectory)
        {
            this.RunnerFileExtension = "*.*o";
            this.MsiteMwWideLaneProcesser = new MultiSiteMwWideLaneProcesser(BasePrn, OutputDirectory, setWeightWithSat, MaxError, isOutputDetail);
         } 
        
        #region 属性 
        /// <summary>
        /// 宽项处理器。
        /// </summary>
        public MultiSiteMwWideLaneProcesser MsiteMwWideLaneProcesser { get; set; } 
        #endregion

        /// <summary>
        /// 算前初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            this.Option.IsBaseSatelliteRequried = false; //此处直接从外部指定，避免无谓的搜索
        }

        protected override IChecker<MultiSiteEpochInfo> BuildChecker()
        {
            var checker = base.BuildChecker() as MultiSiteEpochCheckingManager;
            var add = new MultiSiteIndicatedSatContainedChecker(BasePrn);
            checker.Add(add);
            return checker;
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="mEpochInfo"></param>
        public override void Process(MultiSiteEpochInfo mEpochInfo)
        { 
            var val = mEpochInfo; 

            //计算宽项模糊度
            MsiteMwWideLaneProcesser.Buffers = BufferedStream.MaterialBuffers;
            MsiteMwWideLaneProcesser.Revise(ref val); 
        }

        /// <summary>
        /// 算后写结果
        /// </summary>
        public override void PostRun()
        {
            base.PostRun();
            MsiteMwWideLaneProcesser.WriteToFiles();
            MsiteMwWideLaneProcesser.DifferFcbManager.WriteToFile(Path.Combine(OutputDirectory, "DcbProducts.sdfcb.xls"));
        }

        public override SimpleGnssResult Produce(MultiSiteEpochInfo material)
        {
            throw new NotImplementedException();
        }
    }
}