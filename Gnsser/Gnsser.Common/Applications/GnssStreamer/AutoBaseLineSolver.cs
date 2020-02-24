//2019.01.13, czs, create in hmx, 多时段基线网解算综合窗口


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq; 
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo; 
using System.Threading; 

namespace Gnsser
{
    /// <summary>
    /// 自动基线解算类型
    /// </summary>
    public enum AutoBaseLinSolveType
    {
        单一算法,
        按基线长度采用不同算法
    }


    /// <summary>
    /// 自动基线解算
    /// </summary>
    public class AutoBaseLineSolver
    {
        Log log = new Log(typeof(AutoBaseLineSolver));
        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoBaseLineSolver()
        {
            this.ParallelConfig = new ParallelConfig();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="AutoBaseLinSolveType"></param>
        /// <param name="gnssSolverType"></param>
        /// <param name="Options"></param>
        /// <param name="maxShortLineLength"></param>
        public AutoBaseLineSolver(
            AutoBaseLinSolveType AutoBaseLinSolveType,
            GnssSolverType gnssSolverType,
            Dictionary<GnssSolverType, GnssProcessOption> Options, 
            double maxShortLineLength)
        {
            Init(AutoBaseLinSolveType, gnssSolverType, Options, maxShortLineLength);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="AutoBaseLinSolveType"></param>
        /// <param name="gnssSolverType"></param>
        /// <param name="Options"></param>
        /// <param name="maxShortLineLength"></param>
        public void Init(AutoBaseLinSolveType AutoBaseLinSolveType, GnssSolverType gnssSolverType, Dictionary<GnssSolverType, GnssProcessOption> Options, double maxShortLineLength)
        {
            this.AutoBaseLinSolveType = AutoBaseLinSolveType;
            this.GnssSolverType = gnssSolverType;
            this.MaxShortLineLength = maxShortLineLength;
            this.Options = Options;
        }
        /// <summary>
        /// 结束激发
        /// </summary>
        public event Action Completed;

        #region  属性
        /// <summary>
        /// 最长短基线
        /// </summary>
        public double MaxShortLineLength { get; set; }
        /// <summary>
        /// 自动计算类型
        /// </summary>
        public AutoBaseLinSolveType AutoBaseLinSolveType { get; set; }
        /// <summary>
        /// GNSS计算类型
        /// </summary>
        public GnssSolverType GnssSolverType { get; set; }
        /// <summary>
        /// 单一计算或短基线选项
        /// </summary>
        public GnssProcessOption Option => Options[GnssSolverType];
        /// <summary>
        /// 当前选项
        /// </summary>
        public GnssProcessOption CurrentOption { get; set; }
        /// <summary>
        /// 所有计算类型集合
        /// </summary>
        public Dictionary<GnssSolverType, GnssProcessOption> Options { get; set; }
        /// <summary>
        /// 并行计算设置
        /// </summary>
        public IParallelConfig ParallelConfig { get; set; }
        /// <summary>
        /// 当前计算器数量
        /// </summary>
        int CurrentSolverCount { get; set; }
        /// <summary>
        /// 已完成的计算器数量
        /// </summary>
        int CompletedSolverCount { get; set; }
        List<SiteObsBaseline> SitebaseLines { get; set; }
        /// <summary>
        /// 进度条
        /// </summary>
        public  IProgressViewer ProgressViewer { get; set; }
        #endregion

        /// <summary>
        /// 解算
        /// </summary> 
        /// <param name="LineManager"></param>
        public void Solve(MutliPeriodBaseLineManager LineManager)
        {
            SitebaseLines = LineManager.AllLineObjs;
            if (ProgressViewer != null) ProgressViewer.InitProcess(LineManager.Count);

            InitSolverCount(LineManager);

            //分时段计算 ,时段之间采用串行算法
            foreach (var item in LineManager.KeyValues)
            {
                var sitebaseLines = item.Value.Values;
                var netTimePeriod = item.Key;

                switch (AutoBaseLinSolveType)
                {
                    case AutoBaseLinSolveType.单一算法:
                        {
                            SolveBaseLine(sitebaseLines, Option, netTimePeriod);
                        }
                        break;
                    case AutoBaseLinSolveType.按基线长度采用不同算法:
                        {
                            //长基线解算
                            var longOption = Options[GnssSolverType.无电离层双差];
                            //长基线 
                            List<SiteObsBaseline> shorts = item.Value.GetShortLines(MaxShortLineLength);
                            List<SiteObsBaseline> longLines = item.Value.GetLongLines(MaxShortLineLength);

                            if (longLines.Count > 0)
                            {
                                SolveBaseLine(longLines, longOption, netTimePeriod);
                            }
                            //短基线
                            if (shorts.Count > 0)
                            {
                                SolveBaseLine(shorts, Option, netTimePeriod);
                            }
                        }
                        break;
                    default:
                        {
                            SolveBaseLine(sitebaseLines, Option, netTimePeriod);
                        }
                        break;
                }

                ProgressViewer.PerformProcessStep();
            }
            if (ProgressViewer != null) ProgressViewer.Full();
        }

        private void InitSolverCount(MutliPeriodBaseLineManager LineManager)
        {
            CompletedSolverCount = 0;
            CurrentSolverCount = 0;

            switch (AutoBaseLinSolveType)
            {
                case AutoBaseLinSolveType.单一算法:
                    CurrentSolverCount = LineManager.Count;
                    break;
                case AutoBaseLinSolveType.按基线长度采用不同算法:
                    CurrentSolverCount = 0;
                    foreach (var item in LineManager.KeyValues)
                    {
                        var sitebaseLines = item.Value.Values;
                        var netTimePeriod = item.Key;

                        List<SiteObsBaseline> shorts = item.Value.GetShortLines(MaxShortLineLength);
                        //短基线
                        if (shorts.Count > 0)
                        {
                            CurrentSolverCount++;
                        }
                        //长基线 
                        if (shorts.Count < item.Value.Count)
                        {
                            CurrentSolverCount++;
                        }
                    }
                    break;
                default: break;
            }
        }
        /// <summary>
        /// 初始目录
        /// </summary>
        string OriginalDirectory { get; set; }

        /// <summary>
        /// 解算基线
        /// </summary>
        /// <param name="sitebaseLines"></param>
        /// <param name="option"></param>
        /// <param name="netPeriod"></param>
        public void SolveBaseLine(List<SiteObsBaseline> sitebaseLines, GnssProcessOption option, TimePeriod netPeriod)
        {
            if (sitebaseLines == null || sitebaseLines.Count == 0) { log.Warn("没有计算数据！"); return; }
            this.CurrentOption = option;

            log.Info("即将解算指定的 " + sitebaseLines.Count + " 条基线。");
            List<GnssBaseLineName> baseLines = new List<GnssBaseLineName>() { };
            List<string> pathes = new List<string>();
            foreach (var item in sitebaseLines)
            {
                pathes.Add(item.Start.FilePath);
                pathes.Add(item.End.FilePath);

                //更新最新路径，当修改后存储在Temp目录中的需要更新地址
                item.LineName.RefFilePath = item.Start.FilePath;
                item.LineName.RovFilePath = item.End.FilePath;

                baseLines.Add(item.LineName);
            }
            //设置独立的输出目录
            this.OriginalDirectory = option.OutputDirectory;
            option.OutputDirectory = option.GetSolverDirectory(netPeriod); 
            Geo.Utils.FileUtil.CheckOrCreateDirectory(option.OutputDirectory);

            TwoSiteBackGroundRunner runner = new TwoSiteBackGroundRunner(option, pathes.Distinct().ToArray(), baseLines);
            runner.ParallelConfig = ParallelConfig;
            runner.Completed += Runner_Completed;
            runner.Processed += OneSolver_Processed;
            //runner.ProgressViewer = progressBarComponent1;
            runner.Init();
            runner.Run(); 
        }

        private void Runner_Completed(object sender, EventArgs e)
        {
            CompletedSolverCount++;
            if (CompletedSolverCount >= CurrentSolverCount)
            {
                Completed?.Invoke();
            }
            //恢复目录
            this.CurrentOption.OutputDirectory = OriginalDirectory;
        }
        /// <summary>
        /// 计算完毕一个测站
        /// </summary>
        /// <param name="Solver"></param>
        void OneSolver_Processed(IntegralGnssFileSolver Solver)
        {
            var result = Solver.CurrentGnssResult;
            var entity = result as IWithEstimatedBaseline; if (entity == null) { return; }
            var line = entity.GetEstimatedBaseline();

            var lineName = line.BaseLineName;
            var time = result.ReceiverTime;
            SiteObsBaseline lineObj = GetLineObj(line, time);
            if(lineObj == null)
            {
                return;
            }
            lineObj.EstimatedResult = line;


            // lineObj.EstimatedResult = line;
            log.Info(result.Name + ", " + result.ReceiverTime + "， 即将输出结果文件...");
            var writer = new GnssResultWriter(Solver.Option, Solver.Option.IsOutputEpochResult,
                Solver.Option.IsOutputEpochSatInfo);
            writer.WriteFinal((BaseGnssResult)result);
        }

        private SiteObsBaseline GetLineObj(IEstimatedBaseline line, Time time)
        {
            SiteObsBaseline lineObj = null;// BaselineManager.Get(line.BaseLineName);
            foreach (var lineO in SitebaseLines)
            {
                if (lineO.LineName == line.BaseLineName && lineO.TimePeriod.Contains(time))
                {
                    lineObj = lineO;
                    break;
                }
            }

            return lineObj;
        }
    }






}