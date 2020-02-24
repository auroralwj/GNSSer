//2017.09.26, czs, create in hongqing, 基于缓存的周跳探测

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.IO;
using Geo.Algorithm;
using Geo; 
using Geo.Winform;


namespace Gnsser.Winform
{
    /// <summary>
    /// 基于缓存的周跳探测
    /// </summary>
    public partial class BufferedCycleSlipDetectForm : Form
    {
        ILog log = Log.GetLog(typeof(CycleSlipDetectForm));
        public BufferedCycleSlipDetectForm()
        {
            InitializeComponent();
            Option = new GnssProcessOption();
        }

        #region 属性

        GnssProcessOption Option { get; set; }
        public bool IsCancel { get; set; }
        BufferedStreamService<EpochInformation> DataSourse { get; set; }

        DateTime StartTime;

        InstantValueStorage InstantValueStorage;
        GnssSysRemover GnssSysRemover;
        ObjectTableManager ResultTables { get; set; }
        #endregion
        
        private void button_cancel_Click(object sender, EventArgs e)
        {
            SetRunable(false);
            this.backgroundWorker1.CancelAsync();
        }

        /// <summary>
        /// 设置是否可以运行
        /// </summary>
        /// <param name="runable"></param>
        private void SetRunable(bool runable)
        {
            this.IsCancel = !runable;
            this.button_process.Enabled = !runable;
            this.button_cancel.Enabled = runable;
        }
        private void button_process_Click(object sender, EventArgs e)
        {
            SetRunable(true);
            backgroundWorker1.RunWorkerAsync();
        }

        private void CycleSlipDetectForm_Load(object sender, EventArgs e)
        {
            fileOpenControl1.FilePath = Setting.GnsserConfig.SampleOFile;
            directorySelectionControl1.Path = Setting.TempDirectory;
            enumRadioControl1.Init<SatObsDataType>();
            enumRadioControl1.SetCurrent<SatObsDataType>(SatObsDataType.PhaseA);
            Option.ObsDataType = enumRadioControl1.GetCurrent<SatObsDataType>();

            gnssSystemSelectControl1.SetSatelliteType(SatelliteType.G);
        }
        /// <summary>
        /// 在输出框显示信息。
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowInfo(string msg)
        {
            FormUtil.InsertLineWithTimeToTextBox(this.richTextBoxControl1, msg);
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartTime = DateTime.Now;

            var path = fileOpenControl1.FilePath;
            if (!File.Exists(path)) { MessageBox.Show("文件不存在！"); return; }
            var fileName = Path.GetFileName(path);

            ResultTables = new ObjectTableManager(this.directorySelectionControl1.Path);
            DataSourse = new BufferedStreamService<EpochInformation>(new RinexFileObsDataSource(path), 500000);
            DataSourse.MaterialBuffersFullOrEnd += DataSourse_MaterialBuffersFullOrEnd;
            DataSourse.MaterialInputted += DataSourse_MaterialInputted;
            log.Info("载入数据源 " + DataSourse);
            GnssSysRemover = new Gnsser.GnssSysRemover(new List<SatelliteType>() { gnssSystemSelectControl1.SatelliteType });
            InstantValueStorage = new Geo.InstantValueStorage();
            int index = 0;
            var table = ResultTables.GetOrCreate(DataSourse.Name + "_周跳探测结果");
            foreach (var item in DataSourse)
            {
                if (IsCancel) { break; }

                index++;
            }
            var obsType = enumRadioControl1.GetCurrent<SatObsDataType>();
            ResultTables.WriteAllToFileAndClearBuffer();
        }

        void DataSourse_MaterialBuffersFullOrEnd(IWindowData<EpochInformation> obj)
        {
            BufferedCycleSlipDetector detector = new BufferedCycleSlipDetector(this.Option);
            detector.Detect(obj);

            InstantValueStorage = detector.InstantValueStorage;
            var path = Path.Combine(directorySelectionControl1.Path, "CycleSlip.clsp");
            InstantValueStorage.WriteToFile(path);

            ShowInfo(InstantValueStorage.ToString());
        }

        void DataSourse_MaterialInputted(EpochInformation material)
        {
            GnssSysRemover.Revise(ref material);
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var span = DateTime.Now - StartTime;
            ShowInfo("执行完毕！耗时：" + span.ToString());
            if (Geo.Utils.FormUtil.ShowYesNoMessageBox("后台执行完成！耗时：" + span.ToString() + "。\r\n是否打开输出目录？") == System.Windows.Forms.DialogResult.Yes)
            {
                Geo.Utils.FileUtil.OpenDirectory(this.directorySelectionControl1.Path);
            }
            SetRunable(false);
        }

        private void checkBox_debugModel_CheckedChanged(object sender, EventArgs e) { Setting.IsShowDebug = checkBox_debugModel.Checked; }

        private void button_draw_Click(object sender, EventArgs e)
        {
            if (InstantValueStorage == null)
            {
                MessageBox.Show("请先计算，再绘图！"); return;
            }
             //Draw(CycleSlipDetector.CycleSlipStorage);
            var form =  new CommonChartForm(InstantValueStorage);
            form.Text = "CycleSlip Of " + InstantValueStorage.Count + " Prns";
            form.Show();
        } 

        private void button_setting_Click(object sender, EventArgs e)
        {
            OptionVizardBuilder builder = new OptionVizardBuilder();
            builder.SetAll(false).SetIsShowObsAndApproxOptionPage(true).SetIsShowCycleSlipOptionPage(true).SetIsShowStreamOptionPage(true);

            Option.ObsDataType = enumRadioControl1.GetCurrent<SatObsDataType>();

            var form = new OptionVizardForm(this.Option);
            form.Init(builder);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Option = form.Option;
                enumRadioControl1.SetCurrent<SatObsDataType>(Option.ObsDataType);
            }
        }
    }
}