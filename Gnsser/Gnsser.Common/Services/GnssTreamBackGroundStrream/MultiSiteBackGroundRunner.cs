//2018.07.27, czs, create in hmx, 双差定位后台运行器
//2018.11.02, czs, edit in hmx, 新建为GNSS网解运行器

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
    /// GNSS网解后台运行器
    /// </summary>
    public class MultiSiteBackGroundRunner : AbstractGnssStreamBackGroundRunner<IntegralGnssFileSolver, MultiSiteEpochInfo, SimpleGnssResult>
    {
        Log log = new Geo.IO.Log(typeof(MultiSiteBackGroundRunner)); 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="oFilePathes"></param>
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public MultiSiteBackGroundRunner(GnssProcessOption Option, string[] oFilePathes, string nFilePath = null, string clkPath = null) : base(Option, oFilePathes, nFilePath, clkPath)
        {
        }
         
        /// <summary>
        /// 计算器总共的数量
        /// </summary>
        public override int TotalRunnerCount => 1;

        public override void Run()
        {
            this.RunningSolvers = new System.Collections.Generic.List<IntegralGnssFileSolver>();
            this.PostionReportBuilder = null;

            log.Info("开始初始化，加载资源等。。。");
            DateTime start = DateTime.Now;
            BuildSolverAndRun(OFilePathes);
            this.Complete();
            RunningSolvers.Clear();
            var span = DateTime.Now - start;
            log.Info("计算完毕，耗时：" + span.ToString() + ", " + span.TotalSeconds / OFilePathes.Length + " 秒/个");
        } 
        /// <summary>
        /// 计算，错误返回false。
        /// </summary>
        /// <param name="pathes"></param>
        /// <returns></returns>
        protected override bool BuildSolverAndRun(params string[] pathes)
        {
            IntegralGnssFileSolver Solver = null;
#if !DEBUG
            try
            {
#endif 
                Solver = BuildSolver(pathes);
            
            this.InitProcess(Solver.Context.ObservationDataSource.ObsInfo.Count);
            Solver.ResultProduced += Solver_ResultProduced;


            Solver.Completed += Solver_Completed;
                Run(Solver);

                log.Info(Solver.Name + " 计算完成。");
                RunningSolvers.Remove(Solver);
                if (Solver != FisrtSolver)
                {
                    Solver.Dispose();
                    Solver = null;
                }

                return true;
#if !DEBUG
            }
            catch (Exception ex)
            {
                var roverMsg = "";
                if (Solver != null) roverMsg = Solver.Current + "";
                var msg = Solver + ", " + roverMsg + " 计算发生致命错误：" + ex.Message + ",路径：\r\n" + path;
                log.Fatal(msg);
                if (Setting.GnsserConfig.IsDebug)
                {
                    ex.Data["GNSSerMsg"] = msg;
                    throw ex;
                }
            }
#endif
            return false;
        }




        /// <summary>
        /// 构建并初始化GNSS数据流计算器。
        /// </summary>
        /// <param name="pathes"></param>
        /// <returns></returns>
        protected override IntegralGnssFileSolver BuildSolver(params string [] pathes)
        {   
            var Solver = new IntegralGnssFileSolver(); 
            Solver.Option = Option;
            Solver.Init(pathes);
            Solver.Context.CheckOrUpdateEphAndClkService(EphFilePath, ClkFilePath);
            
            log.Info(Solver.Name + " 初始化完毕。");

            OnSolverCreated(Solver);

            return Solver;
        }
    }   
     
}
