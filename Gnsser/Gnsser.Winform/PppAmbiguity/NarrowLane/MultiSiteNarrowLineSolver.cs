//2016.08.20, czs, create in 福建永安, 宽项计算器
//2016.08.29, czs, edit in 西安洪庆, 重构宽项计算器，精简代码
//2016.10.20, czs, edit in hongqing, 固定宽项计算窄巷。

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
    /// 窄项计算器
    /// </summary>
    public class MultiSiteNarrowLineSolver : BasedSatMultiSiteObsStreamer
    {
        ILog log = new Log(typeof(MultiSiteNarrowLineSolver));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <param name="BasePrn"></param>
        /// <param name="OutputDirectory"></param>
        public MultiSiteNarrowLineSolver(SatelliteNumber BasePrn, string OutputDirectory, string wideLanePath, int skipCount)
            :base(BasePrn, OutputDirectory)
        {
            this.RunnerFileExtension = "*.*o"; 
            this.PppResultBufferSize = 10;
            this.PppResultBuffer = new WindowData<Dictionary<string, PppResult>>(PppResultBufferSize);
            this.DifferFcbManager = DifferFcbManager.Read(wideLanePath);
            this.SkipCount = skipCount;
        } 
        
        #region 属性
        public int SkipCount { get; set; }
        /// <summary>
        /// 差分数据管理器，此处提供已有的宽项成果。
        /// </summary>
        public DifferFcbManager DifferFcbManager { get; set; }
        /// <summary>
        /// 缓存无处不在，PPP结果需要缓存，以获得更好的计算结果.
        /// </summary>
        public int PppResultBufferSize { get; set; }
        /// <summary>
        /// PPP 计算结果
        /// </summary>
        public IWindowData<Dictionary<string, PppResult>> PppResultBuffer { get; set; } 
        /// <summary>
        /// 多测站的PPP计算。
        /// </summary>
        public MultiSiteIonoFreePppManger IonoFreePppManager { get; set; }
        /// <summary>
        /// 多站模糊度值计算器。
        /// </summary>
        MultiSiteNarrowLaneSolver MultiSiteNarrowLaneSolver { get; set; }

        #endregion

        /// <summary>
        /// 算前初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            this.Option.IsBaseSatelliteRequried = false;
            this.IonoFreePppManager = new MultiSiteIonoFreePppManger(this.DataSource, this.Context, this.Option);
            this.MultiSiteNarrowLaneSolver = new MultiSiteNarrowLaneSolver(BasePrn, DifferFcbManager, OutputDirectory, SkipCount);
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
            //base.Process(mEpochInfo);
            var val = mEpochInfo; 

            //精密单点定位计算
            IonoFreePppManager.Solve(val);
            if (IonoFreePppManager.CurrentResults.Count == 0) { return; }

            //缓存与模糊度固定计算
            if (PppResultBuffer.IsFull)
            {
                MultiSiteNarrowLaneSolver.Solve(IonoFreePppManager.CurrentResults, PppResultBuffer, mEpochInfo, BufferedStream.MaterialBuffers);
            }

            PppResultBuffer.Add(IonoFreePppManager.CurrentResults);
        }

        /// <summary>
        /// 算后写结果
        /// </summary>
        public override void PostRun()
        {
            base.PostRun(); 
            MultiSiteNarrowLaneSolver.WriteAllToFiles();
            this.IonoFreePppManager.WriteRestults();
        }

        public override SimpleGnssResult Produce(MultiSiteEpochInfo material)
        {
            throw new NotImplementedException();
        }
    }
}