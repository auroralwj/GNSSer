//2018.07.27, czs, create in hmx, 双差定位后台运行器

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
    /// 双差定位后台运行器
    /// </summary>
    public class TwoSiteBackGroundRunner : AbstractGnssStreamBackGroundRunner<IntegralGnssFileSolver, MultiSiteEpochInfo, SimpleGnssResult>
    {
        Log log = new Geo.IO.Log(typeof(PointPositionBackGroundRunner));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="oFilePathes"></param>
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public TwoSiteBackGroundRunner(GnssProcessOption Option, string[] oFilePathes, string nFilePath = null, string clkPath = null) : base(Option, oFilePathes, nFilePath, clkPath)
        {
            BaseLineSelector = new BaseLineSelector(Option.BaseLineSelectionType, Option.IndicatedBaseSiteName, Option.BaseLineFilePath);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="baseLines">已经选择好的基线</param>
        /// <param name="oFilePathes">已经选择好的基线</param>
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public TwoSiteBackGroundRunner(GnssProcessOption Option, string[] oFilePathes, List<GnssBaseLineName> baseLines, string nFilePath = null, string clkPath = null) : base(Option, oFilePathes, nFilePath, clkPath)
        {
            BaseLineNames = baseLines;
        }

        BaseLineSelector BaseLineSelector { get; set; }
        /// <summary>
        /// 待计算的基线
        /// </summary>
        public List<GnssBaseLineName> BaseLineNames { get; set; }
        /// <summary>
        /// 计算器总共的数量
        /// </summary>
        public override int TotalRunnerCount => base.TotalRunnerCount - 1;

        public override void Run()
        {
            List<string> failedPathes = new List<string>();
            this.RunningSolvers = new System.Collections.Generic.List<IntegralGnssFileSolver>();
            this.PostionReportBuilder = null;

            log.Info("开始初始化，加载资源等。。。");
            DateTime start = DateTime.Now;

            //多时段探测，待做


            if (BaseLineNames == null)
            {
                log.Info("外部没有输入基线，即将选择基线...");
                this.BaseLineNames = BaseLineSelector.GetBaselines(OFilePathes);
            }

            // public List<BaseLineName> GetBaselineNames(string[] obsFilePaths)
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("所选基线：");
            foreach (var item in BaseLineNames)
            {
                sb.AppendLine(item.ToString());
            }
            log.Info(sb.ToString());


            if (BaseLineNames.Count > 1)
            {
                MultiRun(failedPathes, BaseLineNames);
            }
            else
            {
                SingleRun(failedPathes, BaseLineNames[0]);
            }
            this.Complete();
            RunningSolvers.Clear();
            var span = DateTime.Now - start;
            log.Info("计算完毕，耗时：" + span.ToString() + ", " + span.TotalSeconds / OFilePathes.Length + " 秒/个");


            if (failedPathes.Count > 0)
            {
                StringBuilder ssb = new StringBuilder();
                ssb.AppendLine("失败 " + failedPathes.Count + " 个文件（请在日志[Fatal]中查找原因）：");
                foreach (var item in failedPathes)
                {
                    ssb.AppendLine(item);
                }
                log.Fatal(ssb.ToString());
            }
        }
        
        private void SingleRun(List<string> failedPathes, GnssBaseLineName baselines)
        {
            IntegralGnssFileSolver Solver = null;
            string baseline = baselines.ToString();
#if !DEBUG
            try
            {

#endif
            Solver = BuildSolver(baselines.RefFilePath, baselines.RovFilePath);
                this.InitProcess(Solver.Context.ObservationDataSource.ObsInfo.Count);
                Solver.ResultProduced += Solver_ResultProduced;
                Solver.Completed += Solver_Completed;
                Run(Solver);
#if !DEBUG
            }
            catch (Exception ex)
            {
                if (BuildSolverAndRun(baseline))
                {
                    failedPathes.Add(baseline);
                }

                var roverMsg = "";
                if (Solver != null) roverMsg = Solver.Current + "";
                var msg = Solver + baseline + ", " + roverMsg + ", 计算发生致命错误：" + ex.Message;
                log.Fatal(msg);
                if (Setting.GnsserConfig.IsDebug)
                {
                    ex.Data["GNSSerMsg"] = msg;
                    throw ex;
                }
            }
#endif
            }

        private void MultiRun(List<string> failedPathes, List<GnssBaseLineName> baselines)
        {
            this.InitProcess(baselines.Count);
            
            if (ParallelConfig.EnableParallel)
            {
                log.Info("开始并行计算，并行度：" + ParallelConfig.ParallelOptions.MaxDegreeOfParallelism);
                Parallel.ForEach(baselines, ParallelConfig.ParallelOptions, (baseline, state) =>
                {
                    var lineName = baseline.ToString();
                    if (!BuildSolverAndRun(baseline.RefFilePath, baseline.RovFilePath))
                    {
                        failedPathes.Add(lineName);
                    }

                    if (IsCancel) { state.Break(); }
                });
            }
            else
            {
                log.Info("开始串行计算。");
                foreach (var baseline in baselines)
                {
                    BuildSolverAndRun(baseline.RefFilePath, baseline.RovFilePath);

                    if (IsCancel) { break; }
                }

            }
        }


        /// <summary>
        /// 计算，错误返回false。
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        protected override bool BuildSolverAndRun(params string[] fileNames)
        {
            IntegralGnssFileSolver Solver = null;
#if !DEBUG
            try
            {
#endif 
                Solver = BuildSolver(fileNames);
               if(Solver == null)
                {
                    log.Error("GNSS 计算器创建失败！" + Geo.Utils.StringUtil.ToString(fileNames ));
                    return false;
                }
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
        /// <param name="baselines"></param>
        /// <returns></returns>
        protected override IntegralGnssFileSolver BuildSolver(params string [] baselines)
        {
            var refPath = baselines[0];
            var rovPath = baselines[1];

            var Solver = new IntegralGnssFileSolver(); 
            Solver.Option = Option; 
            Solver.OutputDirectory = Option.OutputDirectory;
            Solver.Init(new string[] { refPath, rovPath });
            Solver.Context.CheckOrUpdateEphAndClkService(EphFilePath, ClkFilePath);
            

            log.Info(Solver.Name + " 初始化完毕。");

            OnSolverCreated(Solver);

            return Solver;
        }
    }   
     
}
