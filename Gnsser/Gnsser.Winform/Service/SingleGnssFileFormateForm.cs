//2016.09.24, czs,  create in hongqing, GNSS单文件格式化

using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;
using Geo.Utils;
using Gnsser;
using Gnsser.Checkers;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using  System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gnsser.Winform
{
    /// <summary>
    /// GNSS单文件计算器。
    /// </summary>
    public partial class SingleGnssFileFormateForm : MultiFileStreamerForm
    { 
        Log log = new Log(typeof(IntegralGnssFileSolveForm));
        public SingleGnssFileFormateForm()
        {
            InitializeComponent();
            //this.SetIsMultiObsFile(false);
            //positonResultRenderControl11.IsHasIndexParamName = true;
            this.SetExtendTabPageCount(0, 0);
            CurrentRunningSolvers = new BaseList<ObsObjFormatStreamer>(); 
        }

        #region 属性
         
     
        BaseList<ObsObjFormatStreamer> CurrentRunningSolvers { get; set; }
        static int _resultCount = 0;
        DateTime startTime;

        #endregion

        #region 方法
        protected override GnssProcessOption CheckOrBuildGnssOption()
        {
            var Option = CheckOrBuildGnssOption(this.rinexObsFileFormatTypeControl1.CurrentdType);
            //增加一些默认选项
            //Option.IsIndicatingCoordFile = true;
            //Option.IsSiteCoordServiceRequired = true;
            //Option.IsSetApproxXyzWithCoordService = true;
            //Option.IsApproxXyzRequired = true;

            return Option;
        }
        protected override void OnOptionChanged(GnssProcessOption Option)
        {
            base.OnOptionChanged(Option);
            this.rinexObsFileFormatTypeControl1.CurrentdType = Option.RinexObsFileFormatType;
        }

        bool IsUseRangeCorrections { get => checkBox_IsUseRangeCorrections.Checked; }

        protected override void Run(string[] inputPathes)
        {
            startTime = DateTime.Now;
            //this.GnssResults.Clear();
            CurrentRunningSolvers.Clear();
            #region 一些设置
            _resultCount = 0;

            #endregion
            //计算
            if (ParallelConfig.EnableParallel)//并行计算
            {
                ShowInfo("开始并行计算");
                this.ProgressBar.IsUsePercetageStep = false;
                ParallelRunning(inputPathes);
            }
            else//串行计算
            {
                ShowInfo("开始串行计算");
                this.ProgressBar.IsUsePercetageStep = true;
                this.ProgressBar.Init(new List<string>(inputPathes));
                foreach (var item in inputPathes)
                {
                    if (IsCancel || this.backgroundWorker1.CancellationPending) { break; }
                    Run(item);
                }
            }
        }

        protected override void Run(string inputPathes)
        {
            if (this.IsCancel) { return; }

            var Solver = new ObsObjFormatStreamer(IsUseRangeCorrections);
            CurrentRunningSolvers.Add(Solver);
            Solver.Option = CheckOrBuildGnssOption();
            
            Solver.Init(inputPathes);
            Solver.InfoProduced += IntegralGnssFileSolver_InfoProduced;
            Solver.EpochEntityProduced += IntegralGnssFileSolver_MultiSiteEpochInfoProduced;
            Solver.Completed += OneSolver_Completed;

            if (_resultCount == 0)
            {
                this.ProgressBar.InitFirstProcessCount(Solver.DataSource.ObsInfo.Count);
            }

            Solver.Run();

            this.ProgressBar.PerformClassifyStep(Solver.DataSource.ObsInfo.Count);
        }

        protected void ParallelRunning(string[] inputPathes)
        {
            this.ProgressBar.InitProcess(inputPathes.Length);
            this.ProgressBar.ShowInfo("正在计算！");
            //  this.Invoke(new Action(delegate() { this.ProgressBar.Update();  }));

            this.Option = CheckOrBuildGnssOption();
            Parallel.ForEach(inputPathes, this.ParallelConfig.ParallelOptions, (obsData, state) =>
            {
                if (IsCancel || this.backgroundWorker1.CancellationPending) { state.Stop(); }

                var Solver = new ObsObjFormatStreamer(IsUseRangeCorrections);
                Solver.InfoProduced += IntegralGnssFileSolver_InfoProduced;
                Solver.IsCancel = this.IsCancel;
                CurrentRunningSolvers.Add(Solver);
                Solver.Option = this.Option;
                Solver.Init(obsData);
                Solver.Completed += OneSolver_Completed;
                Solver.Run();
            });
        }

        void OneSolver_Completed(object sender, EventArgs e)
        {
            var Solver = sender as ObsObjFormatStreamer;
            CurrentRunningSolvers.Remove(Solver);
            Solver.Dispose();

            if (ParallelConfig.EnableParallel)
            {
                this.ProgressBar.PerformProcessStep();
            }

            //显示和输出结果
            //ShowAndWriteResults(GnssResults);
        }

  
        #region 事件响应
        void IntegralGnssFileSolver_MultiSiteEpochInfoProduced(EpochInformation MultiSiteEpochInfo) { this.ProgressBar.PerformProcessStep(); }

        //protected override void OnOptionChanged(GnssProcessOption Option) { rinexObsFileFormatTypeControl1.CurrentdType = GnssSolverTypeHelper.GetGnssSolverType(Option.GnssSolverType); foreach (var key in CurrentRunningSolvers) { key.Option = this.Option; } }

        void IntegralGnssFileSolver_InfoProduced(string info) { ShowNotice(info); }
        /// <summary>
        /// 是否停止计算
        /// </summary>
        /// <param name="runable"></param>
        protected override void SetRunable(bool runable) { base.SetRunable(runable); foreach (var item in CurrentRunningSolvers)  { item.IsCancel = this.IsCancel; } }
        #endregion

        #region 显示和输出
        protected override void UiToOption()
        {
            base.UiToOption();
            var type = (this.rinexObsFileFormatTypeControl1.CurrentdType);
            this.Option.RinexObsFileFormatType= type;
        }
        protected override void OptionToUi(GnssProcessOption Option)
        {
            base.OptionToUi(Option);
            this.rinexObsFileFormatTypeControl1.CurrentdType = this.Option.RinexObsFileFormatType;
        }
        #endregion


        private void IntegralGnssFileSolveForm_Load(object sender, EventArgs e)
        {
            if (Option != null)
            {
                
            }
        }
        #endregion
    }
}