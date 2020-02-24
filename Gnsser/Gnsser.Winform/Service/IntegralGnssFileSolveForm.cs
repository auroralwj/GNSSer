//2016.04.27, czs,  edit in hongqing, 修改表输出

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
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gnsser.Winform
{
    public partial class IntegralGnssFileSolveForm : ObsFileAdjustStreamForm
    { 
        Log log = new Log(typeof(IntegralGnssFileSolveForm));
        public IntegralGnssFileSolveForm()
        {
            InitializeComponent();
            //positonResultRenderControl11.IsHasIndexParamName = true;
            this.InitSolverType<GnssSolverType>();
            this.ProgressBar.IsUsePercetageStep = true;
            this.Solver = new IntegralGnssFileSolver();
            this.Solver.ResultProduced += Solver_ResultProduced;  
            this.Solver.InfoProduced += IntegralGnssFileSolver_InfoProduced;
            this.Solver.EpochEntityProduced += IntegralGnssFileSolver_MultiSiteEpochInfoProduced;
            this.Solver.Completed += Solver_Completed;
        }

        private void Solver_ResultProduced(SimpleGnssResult product, ObsFileProcessStreamer<MultiSiteEpochInfo, SimpleGnssResult> arg2)
        {
            if (product is SingleSiteGnssResult)
            {
               var entity =  product as SingleSiteGnssResult; 
               this.Coords.Add(new NamedRmsXyz(entity.Name, new RmsedXYZ(entity.EstimatedXyz, entity.EstimatedXyzRms)));            
            }
        }


        void Solver_Completed(object sender, EventArgs e)
        {
            //this.InsertToResultTextBox(this.Solver.CurrentGnssResult.Adjustment.Estimated.ToFormatedText());
            if (this.Option.IsOutputAdjust)
            {
                Solver.AioAdjustFileBuilder.WriteToFile();   
            }
            if (this.Option.IsOutputObsEquation)
            { 
                Solver.AdjustEquationFileBuilder.WriteToFile(); 
            }
            OutputFinalResult(Solver.CurrentGnssResult);
            AppendFinalResultOnUI(Solver.CurrentGnssResult);
            SetResultsToUITable(this.GnssSolver.TableTextManager);
            this.ProgressBar.Full();


            //RMS 输出
            var rmKey = this.GnssSolver.TableTextManager.Keys.Find(m => m.ToLower().Contains("rms"));
            if (rmKey != null)
            {
                var rmsTable = this.GnssSolver.TableTextManager[rmKey];
                var ave = rmsTable.GetAverages();
                var str = Geo.Utils.DictionaryUtil.ToString(ave);
                log.Fatal("Average of RMS: " + str);
            }
        } 
        void IntegralGnssFileSolver_MultiSiteEpochInfoProduced(MultiSiteEpochInfo MultiSiteEpochInfo)
        {
            Solver.IsCancel = this.IsCancel;//update
            this.ProgressBar.PerformProcessStep(); 
        }
        protected override void OnProcessCommandChanged(ProcessCommandType type)
        {
            base.OnProcessCommandChanged(type);
            Solver.IsCancel = type == ProcessCommandType.Cancel;
        }

        protected override IFileGnssSolver GnssSolver { get { return Solver; } }
        IntegralGnssFileSolver Solver { get; set; }

        void IntegralGnssFileSolver_InfoProduced(string info) { ShowNotice(info); }

        protected override void Run(string[] inputPathes)
        {
            this.Coords.Clear();
            this.Option = BuildGnssOption();
            Solver.Option = this.Option;
            Solver.OutputDirectory = this.OutputDirectory;
            Solver.IsCancel = false;
            Solver.Init(inputPathes);

            this.ProgressBar.InitProcess(Solver.DataSource.BaseDataSource.ObsInfo.Count);            

            Solver.Run();
        }

        protected virtual GnssProcessOption BuildGnssOption()
        {
            var type = this.GetSolverType<GnssSolverType>();
            return CheckOrBuildGnssOption(type); 
        }

        protected override void DetailSetting()
        {
            BuildGnssOption();
            base.DetailSetting();
        }
        private void IntegralGnssFileSolveForm_Load(object sender, EventArgs e)
        {

        }

        protected override void BackgroudWorkCompleted()
        {
            base.BackgroudWorkCompleted();
            if (this.checkBox_disposeWhenEnd.Checked && this.Solver != null)
            {
                Solver.Dispose();
            }
        }
    }
}