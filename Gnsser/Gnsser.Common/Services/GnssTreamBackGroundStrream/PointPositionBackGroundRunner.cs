//2017.07.24, czs, create in hongqing, 后台定位器
//2017.10.26, czs, edit in hongqing, 修改用于通用单点定位后台运行器。
//2018.04.23, czs, edit in hmx, 合并 OptionRunner  运行器
//2018.04.27, czs, edit in hmx, 增加rnx观测文件的支持
//2018.05.03, czs, edit in hmx, 优化内存处理，保持在500MB左右
//2018.07.27, czs, edit in hmx, 重构为支持所有计算类型

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text; 
using System.ComponentModel;
using System.Data;   
using Geo.Coordinates;
using Geo;
using Geo.Algorithm;
using Geo.IO;
using System.Net; 
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser;
using Gnsser.Domain;

namespace Gnsser
{
    /// <summary>
    /// 定位后台运行器
    /// </summary>
    public class PointPositionBackGroundRunner : AbstractGnssStreamBackGroundRunner<SingleSiteGnssSolveStreamer, EpochInformation, SimpleGnssResult>
    {
        Log log = new Geo.IO.Log(typeof(PointPositionBackGroundRunner));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="optPath"></param> 
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public PointPositionBackGroundRunner(string optPath, string nFilePath = null, string clkPath = null) : base(optPath, nFilePath, clkPath)
        {
        }
        /// <summary>
        /// 构造函数，观测文件在配置对象中
        /// </summary>
        /// <param name="Option"></param> 
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public PointPositionBackGroundRunner(GnssProcessOption Option, string nFilePath = null, string clkPath = null) : base(Option, nFilePath, clkPath)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="oFilePathes"></param>
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public PointPositionBackGroundRunner(GnssProcessOption Option, string[] oFilePathes, string nFilePath = null, string clkPath = null) : base(Option, oFilePathes, nFilePath, clkPath)
        {
           
        }

        public override void Init()
        {
            base.Init();
        }
        /// <summary>
        /// 构建并初始化GNSS数据流计算器。
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <returns></returns>
        protected override SingleSiteGnssSolveStreamer BuildSolver(params string[] inputPathes)
        {
            var inputPath = inputPathes[0];
            var Solver = new SingleSiteGnssSolveStreamer();
            Solver.Path = inputPath;
            Solver.Option = Option;
            Solver.Init(inputPath);
            Solver.Context.CheckOrUpdateEphAndClkService(EphFilePath, ClkFilePath);

            log.Info(Solver.Name + " 初始化完毕。");

            OnSolverCreated(Solver);

            return Solver;
        }

        /// <summary>
        /// 处理了一个。
        /// </summary>
        /// <param name="solver"></param>
        protected override void OnProcessed(SingleSiteGnssSolveStreamer solver)
        {
            base.OnProcessed(solver);
        }

        public override void Complete()
        {
            base.Complete();
        }


    }

     
}
