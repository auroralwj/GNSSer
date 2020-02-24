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
    public abstract class AbstractGnssStreamBackGroundRunner<TStreamer, TMaterial, TResult> : AbstractProcess, ICancelAbale
        where TStreamer : ObsFileProcessStreamer<TMaterial, TResult>
        where TMaterial : ISiteSatObsInfo
        where TResult : SimpleGnssResult
    {
        Log log = new Geo.IO.Log(typeof(AbstractGnssStreamBackGroundRunner<TStreamer, TMaterial, TResult>));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="optPath"></param> 
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public AbstractGnssStreamBackGroundRunner(string optPath, string nFilePath = null, string clkPath = null)
            :this(new OptionManager().Read(optPath), nFilePath, clkPath)
        {  
        }

        /// <summary>
        /// 构造函数，观测文件在配置对象中
        /// </summary>
        /// <param name="Option"></param> 
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public AbstractGnssStreamBackGroundRunner(GnssProcessOption Option, string nFilePath = null, string clkPath = null)
        {
            this.ClkFilePath = clkPath;
            InputFileManager inputFileManager = new InputFileManager();
            List<string> obsFiles = inputFileManager.GetLocalFilePathes(Option.ObsFiles.ToArray(), "*.*o;*.rnx", "*.*");

            this.ClkFilePath = clkPath;
            this.OFilePathes = obsFiles.ToArray();
            this.EphFilePath = nFilePath;
            this.Option = Option;

            this.IsOutputResult = this.Option.IsOutputResult;
            this.ParallelConfig = new ParallelConfig();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="oFilePathes"></param>
        /// <param name="nFilePath">星历路径，如果不设置，则自动匹配</param>
        /// <param name="clkPath">路径，如果不设置，则自动匹配</param>
        public AbstractGnssStreamBackGroundRunner(GnssProcessOption Option, string[] oFilePathes, string nFilePath = null, string clkPath = null)
        {
            this.ClkFilePath = clkPath;
            this.OFilePathes = oFilePathes;
            this.EphFilePath = nFilePath;
            this.Option = Option;
            this.ParallelConfig = new ParallelConfig();

            this.IsOutputResult = this.Option.IsOutputResult;
        }
        
        #region 属性
        /// <summary>
        /// 是否输出结果
        /// </summary>
        public bool IsOutputResult { get; set; }

        /// <summary>
        /// 历元参数分析器
        /// </summary>
        public EpochParamAnalyzer EpochParamAnalyzer { get; set; }
        /// <summary>
        /// 数据处理了一个，在Solver的PostRun之后，Complete事件之前发生。
        /// </summary>
        public event Action<TStreamer> Processed;

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool isCancel = false;
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel
        {
            get { return isCancel; }
            set
            {
                if(RunningSolvers != null)
                foreach (var item in RunningSolvers)
                {
                   if(item != null)
                    item.IsCancel = value;
                }
                isCancel = value;
            }
        }
        /// <summary>
        /// 报表生成器
        /// </summary>
        public HtmlReportBuilder PostionReportBuilder { get; set; }
        /// <summary>
        /// 导航文件路径
        /// </summary>
        public string EphFilePath { get; set; }
        /// <summary>
        /// 钟差文件路径
        /// </summary>
        public string ClkFilePath { get; set; }
        /// <summary>
        /// 观测文件路径
        /// </summary>
        public string[] OFilePathes { get; set; }
        /// <summary>
        /// 定位选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 正在运行的计算器,除了第一个外，计算完毕一个，应该移除一个，以解除内存消耗。 
        /// </summary>
        public List<TStreamer> RunningSolvers { get; protected set; } 
        /// <summary>
        /// 保留第一个，用于在线查看分析
        /// </summary>
        public TStreamer FisrtSolver { get; private set; }
        /// <summary>
        /// 并行计算选项
        /// </summary>
        public IParallelConfig ParallelConfig { get; set; }
        /// <summary>
        /// 所有运行器数量，单站定位为文件数，双差应该减1
        /// </summary>
        public virtual int TotalRunnerCount { get { return OFilePathes.Length;   } }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化，需要手动调用。
        /// </summary>
        public override void Init()
        {
            IsCancel = false;

            if (Option.IsEnableEpochParamAnalysis && Option.PositionType != PositionType.动态定位)
            {
                this.EpochParamAnalyzer = new EpochParamAnalyzer(new List<string>(this.Option.AnalysisParamNames),
                   Option.SequentialEpochCountOfAccuEval,
                   Option.MaxDifferOfAccuEval, Option.MaxAllowedConvergenceTime,
                   Option.KeyLabelCharCount, Option.MaxAllowedDifferAfterConvergence, Option.MaxAllowedRmsOfAccuEval);
            }
        }
        /// <summary>
        /// 计算器创建事件
        /// </summary>
        public event Action<TStreamer> SolverCreated; 
         
        static object locker = new object();
        /// <summary>
        /// 异步运行
        /// </summary>
        public override void Run()
        {
            RunSingleSite();
        }

        private void RunSingleSite()
        {
            List<string> failedPathes = new List<string>();
            this.RunningSolvers = new System.Collections.Generic.List<TStreamer>();
            this.PostionReportBuilder = null;// new PostionReportBuilder(this.Option, RunningSolvers[0].Context);

            log.Info("开始初始化，加载资源等。。。");
            DateTime start = DateTime.Now;

            if (TotalRunnerCount > 1)
            {
                this.InitProcess(TotalRunnerCount);


                if (ParallelConfig.EnableParallel)
                {
                    log.Info("开始并行计算，并行度：" + ParallelConfig.ParallelOptions.MaxDegreeOfParallelism);
                    Parallel.ForEach(OFilePathes, ParallelConfig.ParallelOptions, (path, state) =>
                    {
                        if (!BuildSolverAndRun(path))
                        {
                            failedPathes.Add(path);
                        }

                        if (IsCancel) { state.Break(); }
                    });
                }
                else
                {
                    log.Info("开始串行计算。");
                    foreach (var path in OFilePathes)
                    {
                        BuildSolverAndRun(path);

                        if (IsCancel) { break; }
                    }

                }
            }
            else
            {
                TStreamer Solver = null;
                string singlePath = OFilePathes[0];
//#if !DEBUG
                try
                {
//#endif
                    Solver = BuildSolver(singlePath);
                    this.InitProcess(Solver.Context.ObservationDataSource.ObsInfo.Count);
                    Solver.ResultProduced += Solver_ResultProduced;
                    Solver.Completed += Solver_Completed;
                    Run(Solver);
//#if !DEBUG
                }
                catch (Exception ex)
                {
                    if (BuildSolverAndRun(singlePath))
                    {
                        failedPathes.Add(singlePath);
                    }

                    var roverMsg = "";
                    if (Solver != null) roverMsg = Solver.Current + "";
                    var msg = Solver + singlePath + ", " + roverMsg + ", 计算发生致命错误：" + ex.Message;
                    log.Fatal(msg);
                    if (Setting.GnsserConfig.IsDebug)
                    {
                        ex.Data["GNSSerMsg"] = msg;
                        throw ex;
                    }
                }
//#endif
            }
            this.Complete();
            RunningSolvers.Clear();
            var span = DateTime.Now - start;
            log.Info("计算完毕，耗时：" + span.ToString() + ", " + span.TotalSeconds / TotalRunnerCount + " 秒/个");


            if (failedPathes.Count > 0)
            {
                StringBuilder ssb = new StringBuilder();
                ssb.AppendLine("失败 " + failedPathes.Count + " 个文件（请在日志[Fatal]中查找原因）：");
                foreach (var item in failedPathes)
                {
                    ssb.AppendLine(item);
                }
                log.Fatal(ssb.ToString());
                log.Error(ssb.ToString());
            }
        }
        /// <summary>
        /// 产生了一个结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="streamer"></param>
        protected void Solver_ResultProduced(TResult result, ObsFileProcessStreamer<TMaterial, TResult> streamer)
        {
            if (this.IsCancel)
            {
                streamer.IsCancel = this.IsCancel;
            }

            if (TotalRunnerCount == 1)
            {
                if (streamer.Previous != null && this.ProgressViewer != null)
                {
                    this.ProgressViewer.IsBackwardProcess = (result.ReceiverTime < streamer.Previous.ReceiverTime);
                }
                this.PerformProcessStep();
            }
        }

        /// <summary>
        /// 计算，错误返回false。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual bool BuildSolverAndRun(params string[] path)
        {
            TStreamer Solver = null;
#if !DEBUG
            try
            {
#endif
            Solver = BuildSolver(path);
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
            return false;
           }
#endif
        }

        /// <summary>
        /// 异步运行指定文件
        /// </summary>
        /// <param name="Solver"></param>
        public void Run(TStreamer Solver)
        {
            RunningSolvers.Add(Solver);
            log.Info(Solver.Name + " 开始计算。");
           
            Solver.Run();
          
        }
        /// <summary>
        /// 构建并初始化GNSS数据流计算器。
        /// </summary>
        /// <param name="inputPathes"></param>
        /// <returns></returns>
        protected abstract TStreamer BuildSolver(params string[] inputPathes);

        /// <summary>
        /// 创建后
        /// </summary>
        /// <param name="Solver"></param>
        protected void OnSolverCreated(TStreamer Solver)
        {
            SolverCreated?.Invoke(Solver); 

            if(this.FisrtSolver == null)
            {
                this.FisrtSolver = Solver;
                FisrtSolver.IsClearTableWhenOutputted = false;
            }

            if (this.PostionReportBuilder == null)
            {
                lock (locker)
                {
                    if (this.PostionReportBuilder == null)
                    {
                        FisrtSolver = Solver;
                        this.PostionReportBuilder = new HtmlReportBuilder(this.Option, Solver.Context);

                    }
                }
            }
        }

        /// <summary>
        /// 计算完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Solver_Completed(object sender, EventArgs e)
        {
            var solver = sender as TStreamer;

            //历元结果分析,第一时间进行
            if (Option.IsEnableEpochParamAnalysis && Option.PositionType != PositionType.动态定位)
            {
                var key = solver.TableTextManager.Keys.Find(m => m.Contains(Setting.EpochParamFileExtension));
                if (key != null)
                {
                    var table = solver.TableTextManager[key];

                    var info = EpochParamAnalyzer.GetParamAccuracyInfos(table);
                    if (info != null)
                    {
                        solver.CurrentGnssResult.ParamAccuracyInfos = info;
                        log.Info(info.ToReadableText());
                    }
                }
            }

            // 产生了一个结果。
            OnProcessed(solver);
            
            
            PostionReportBuilder.Add(solver.CurrentGnssResult);
            PostionReportBuilder.Context = solver.Context;

            if (TotalRunnerCount > 1)
            {
                PerformProcessStep();
            }
            //结果输出
            solver.WriteResultsToFileAndClearBuffer();


        }

        /// <summary>
        /// 处理了一个。
        /// </summary>
        /// <param name="solver"></param>
        protected virtual void OnProcessed(TStreamer solver) { if (Processed != null) { Processed(solver); }    }

        /// <summary>
        /// 结束
        /// </summary>
        public override void Complete()
        {
            if (this.IsOutputResult)
            {
                PostionReportBuilder.BuildWriteAndOpen(this.Option.IsOpenReportWhenCompleted);
            }
          

            log.Info("本批次计算任务完成！");
          //  Geo.Utils.FormUtil.ShowOkAndOpenFile(Option.OutputDirectory);
            base.Complete();
        }


#endregion
    }
}
