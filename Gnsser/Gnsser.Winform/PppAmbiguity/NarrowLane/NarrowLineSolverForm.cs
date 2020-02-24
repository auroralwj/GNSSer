//2016.08.15, czs, create in hongqing, 多测站多历元数据流


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Windows.Forms;
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
using Gnsser.Checkers;

namespace Gnsser.Winform
{
    public partial class NarrowLineSolverForm : MultiFileStreamerForm
    {
        public NarrowLineSolverForm()
        {
            InitializeComponent();
            SetExtendTabPageCount(0,0);
            SetEnableMultiSysSelection(false);
        }

        #region 属性
        MultiSiteNarrowLineSolver WideLaneSover{ get; set; }          

        DateTime startTime;
        SatelliteNumber BasePrn { get { return baseSatSelectingControl1.SelectedPrn; } }

        #endregion                  

        protected override void DetailSetting()
        {
            base.CheckOrBuildGnssOption(GnssSolverType.无电离层组合PPP);
            this.Option.SatelliteTypes = new List<SatelliteType>() { this.baseSatSelectingControl1.SatelliteType };
            var optionForm = new OptionVizardForm(Option);
            if (optionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Option = optionForm.Option;
                this.baseSatSelectingControl1.SetSatelliteType( this.Option.SatelliteTypes[0]);
                this.Option.SatelliteTypes = new List<SatelliteType>() { this.baseSatSelectingControl1.SatelliteType };
            }
        }

        /// <summary>
        /// 运行前执行。在主线程中。
        /// </summary>
        public override void PreRun()
        {
            startTime = DateTime.Now; 
        }

        protected override void Run(string[] inputPathes)
        {
            if (this.Option == null) { this.Option = GnssProcessOptionManager.Instance[GnssSolverType.无电离层组合PPP];}
            this.Option.SatelliteTypes = new List<SatelliteType>() { this.baseSatSelectingControl1.SatelliteType };
            var skipCount = int.Parse(this.textBox_skipCount.Text);
            WideLaneSover = new MultiSiteNarrowLineSolver(BasePrn, OutputDirectory, fileOpenControl_wideLane.FilePath, skipCount);
            WideLaneSover.Option = Option;
            this.Option.MinSatCount = 1;
            WideLaneSover.InfoProduced += WideLaneSover_InfoProduced;

            WideLaneSover.Init(inputPathes);
            WideLaneSover.Run();
        }

        protected override void SetRunable(bool runable)
        {
            base.SetRunable(runable);
            if (WideLaneSover != null && !runable) { WideLaneSover.IsCancel = !runable; }
        }

        protected override void OnProcessCommandChanged(ProcessCommandType type)
        {
            base.OnProcessCommandChanged(type);
            WideLaneSover.IsCancel = type == ProcessCommandType.Cancel;
        }
        void WideLaneSover_InfoProduced(string info)
        {
            if (IsShowProcessInfo)
            {
                ShowNotice(info);
            }        
        }

        private void WideLaneSolverForm_Load(object sender, EventArgs e)
        { 
            fileOpenControl_wideLane.FilePath = Path.Combine(Setting.DataDirectory, @"GNSS\DcbProducts.sdfcb.xls");
        }
    }
}
