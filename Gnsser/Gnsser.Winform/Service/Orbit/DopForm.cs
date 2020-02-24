//2017.07.21, czs, create in hongiqng, DOP计算
//2017.10.09, czs, edit in hongqing, 改进技术，添加方法
//2017.10.17, czs, edit in hongqing, 增加DOP范围控制

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo;
using Geo.Service;
using Geo.Data;
using Geo.IO;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Service;
using Gnsser.Data;
using Geo.Times;
using Geo.Algorithm;

namespace Gnsser.Winform
{
    /// <summary>
    /// DOP 查看与绘图
    /// </summary>
    public partial class DopForm : Form
    {
        Log log = new Log(typeof(DopForm));

        public DopForm()
        {
            InitializeComponent();
        } 
        
        DopSolver DopSolver { get; set; }

        private void button_run_Click(object sender, EventArgs e)
        { 
            if (!backgroundWorker1.IsBusy)
            {
                this.button_run.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }           
        }

        private void DopForm_Load(object sender, EventArgs e)
        { 
            directorySelectionControl1.Path = Setting.GnsserConfig.TempDirectory;
            this.fileOpenControl_ephe.FilePath = Setting.GnsserConfig.SampleNFileV3;
            this.enabledFloatControl1maxDop.SetValue(20);
            geoGridLoopControl1.SetStep(30, 30);
        }

        /// <summary>
        /// 在输出框显示信息。
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowInfo(string msg) { Geo.Utils.FormUtil.InsertLineWithTimeToTextBox(this.richTextBoxControl_result, msg); }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            string navPath = this.fileOpenControl_ephe.FilePath;
            if (!System.IO.File.Exists(navPath)) { return; }
            try
            {
                var phe = EphemerisDataSourceFactory.Create(navPath);
                this.timeLoopControl1.TimePeriodControl.SetTimePerid(phe.TimePeriod);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DopForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy && (Geo.Utils.FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No))
            { 
               e.Cancel = true; 
            } else   {   } 
        }
       
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        { 
            string navPath = this.fileOpenControl_ephe.FilePath;
            string directory = this.directorySelectionControl1.Path; 
            var cutOffAngle = namedFloatControl_cutOffAnlgle.Value;
            var gridLooper = geoGridLoopControl1.GetGridLooper();
            var timeLooper = timeLoopControl1.GetTimeLooper();
            string satWeightPath = fileOpenControl_prnWeight.FilePath;

            var SatWeightData = new TwoKeyDictionary<Time, SatelliteNumber, double>();
            SatWeightTable SatWeights = null;
            
            if (System.IO.File.Exists(satWeightPath))
            { 
                var reader = new ObjectTableReader(satWeightPath);
                var data = reader.Read();

                SatWeights = new SatWeightTable(data, 730 * 24 * 3600, "Epoch", "PRN", "Weight");
                SatWeights.Init();

                log.Info("载入卫星权值文件 : "  + satWeightPath);
            }
            else
            {
                log.Info("没有卫星权值文件");
            }
            IEphemerisService EphemerisService  =  EphemerisDataSourceFactory.Create(navPath);
            var satTypes = multiGnssSystemSelectControl1.SatelliteTypes;
            List<SatelliteNumber> EnabledPrns = EphemerisService.Prns.FindAll(m => satTypes.Contains(m.SatelliteType));
            if (EnabledPrns.Count == 0)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("所选系统星历卫星数量为 0 ."); return;
            }
            EnabledPrns.Sort();
            log.Info("当前系统：" + Geo.Utils.EnumerableUtil.ToString(satTypes) + "，卫星数量：" + EnabledPrns.Count + ", " + Geo.Utils.EnumerableUtil.ToString(EnabledPrns));
             
            DopSolver = new DopSolver(EphemerisService, cutOffAngle, directory,EnabledPrns,SatWeights, timeLooper, gridLooper);
            DopSolver.IsSimpleModel = checkBox1IsSimpleModel.Checked;
            if (enabledFloatControl1maxDop.Enabled)
            {
                DopSolver.MaxDopThreshold = enabledFloatControl1maxDop.Value;
            }
            
            DopSolver.TimeLooper.ProgressViewer = progressBarComponent1;    
            DopSolver.SolveAsync();

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(directory);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button_run.Enabled = true;
        } 
        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (DopSolver != null)
            {
                DopSolver.GeoGridLooper.IsCancel = true;
                DopSolver.TimeLooper.IsCancel = true;
            } 
        } 
    }
}
