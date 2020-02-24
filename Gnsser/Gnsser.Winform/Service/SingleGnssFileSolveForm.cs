//2016.04.27, czs, edit in hongqing, 修改表输出
//2016.09.05, czs, edit in hongqing, GNSS单文件计算器
//2017.07.23, czs, edit in hongqing, 增加界面更新函数，确保模型界面一致
//2017.10.26, czs, edit in hongqing, 封装业务

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
    public partial class SingleGnssFileSolveForm : ObsFileAdjustStreamForm
    {
        Log log = new Log(typeof(SingleGnssFileSolveForm));
        public SingleGnssFileSolveForm()
        {
            InitializeComponent();
            //this.SetExtendTabPageCount(2, 5);
            InitSolverType<SingleSiteGnssSolverType>();
            //enumRadioControl1_GnssSolverType.SetCurrent<SingleSiteGnssSolverType>(SingleSiteGnssSolverType.无电离层组合PPP);
        }

        #region 属性
        protected override IFileGnssSolver GnssSolver { get; set; }
        PointPositionBackGroundRunner Runner { get; set; }

        #endregion

        #region 方法

        protected override void Run(string[] inputPathes)
        {
            if (inputPathes.Length == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("巧妇难为无米之炊，请找点观测数据再来算 (*￣︶￣)"); return; }
            this.Coords.Clear();
            isFirstSolver = true;
            Option = CheckOrBuildGnssOption();
            Runner = null;

            Runner = new PointPositionBackGroundRunner(this.Option, inputPathes);
            Runner.ParallelConfig = ParallelConfig;
            Runner.ProgressViewer = this.ProgressBar;
            Runner.Processed += OneSolver_Processed;
            Runner.SolverCreated += Runner_SolverCreated;
            Runner.Init();

            Runner.Run();
        }
        bool isFirstSolver = true;
        private void Runner_SolverCreated(SingleSiteGnssSolveStreamer obj)
        {
            if (this.Option.TopSpeedModel) { return; }

            if (isFirstSolver)
            {
                isFirstSolver = false;
                var solver = obj;
                GnssSolver = obj;
                solver.ResultProduced += Solver_ResultProduced; ;//只保留第一个的结果在界面
                solver.IsClearTableWhenOutputted = false;//不清空表数据，用于图形显示
            }
        }

        private void Solver_ResultProduced(SimpleGnssResult entity, ObsFileProcessStreamer<EpochInformation, SimpleGnssResult> streamer)
        {
            if (entity == null) { return; }

            if (entity is SingleSiteGnssResult)
            {
                SingleSiteGnssResult result = entity as SingleSiteGnssResult;
                this.Coords.Add(new NamedRmsXyz(entity.Name, new RmsedXYZ(result.EstimatedXyz, result.EstimatedXyzRms)));
            }

            if (entity is IWithEstimatedBaseline)
            {
                var result = entity as IWithEstimatedBaseline;
                this.Coords.Add(new NamedRmsXyz(entity.Name, result.GetEstimatedBaseline().EstimatedRmsXyzOfRov));
            }
        }
                 
        /// <summary>
        /// 计算完毕一个测站
        /// </summary>
        /// <param name="Solver"></param>
        void OneSolver_Processed(SingleSiteGnssSolveStreamer Solver)
        {
            var entity = Solver.CurrentGnssResult;
            SingleSiteGnssResult result = entity as SingleSiteGnssResult;
            this.OutputFinalResult(result);
            this.AppendFinalResultOnUI(result);

            //只截获和显示这一个，文件输出将自动处理
            if (Solver == GnssSolver)
            {
                this.SetResultsToUITable(this.GnssSolver.TableTextManager);
            }
        } 

        #region 事件响应

        /// <summary>
        /// 是否停止计算
        /// </summary>
        /// <param name="runable"></param>
        protected override void SetRunable(bool runable) { base.SetRunable(runable); if (Runner != null) { Runner.IsCancel = !runable; } }

        #endregion

         
        #endregion

        private void button_drawDop_Click(object sender, EventArgs e)
        {
            if (this.GnssSolver.HasTableData)
            {
                var key = this.GnssSolver.TableTextManager.Keys.Find(m => m.ToLower().Contains("dop"));
                if (String.IsNullOrWhiteSpace(key)) { return; }

                this.paramVectorRenderControl1.SetTableTextStorage(this.GnssSolver.TableTextManager[key]);// (names);
                this.paramVectorRenderControl1.DrawParamLines();
            }

        }

        private void button_darwResiduals_Click(object sender, EventArgs e)
        {
            if (this.GnssSolver.HasTableData)
            {
                var key = this.GnssSolver.TableTextManager.Keys.Find(m => m.Contains(Setting.EpochResidualFileExtension));
                if (String.IsNullOrWhiteSpace(key)) { return; }
                var table = this.GnssSolver.TableTextManager[key];
                var titles = table.ParamNames.FindAll(m => m.EndsWith(ParamNames.PhaseL) || m.EndsWith(ParamNames.L1) || m.EndsWith(ParamNames.L2));
                var phaseObsTable = table.GetTable(table.Name + "_载波残差", titles);
                new Geo.Winform.CommonChartForm(phaseObsTable).Show();

                //this.paramVectorRenderControl1.SetTableTextStorage(table);// (names);
                //this.paramVectorRenderControl1.DrawParamLines();
            }
            else
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请在输出中选中残差，计算后再试");
            }
        }
    }
}