//2015.02, czs, edit ,change to C#
//2015.11.05, czs & double, edit, 增加原始数据输出
//2016.10.16, czs, create in hongqing, 新设计，多站数据接收器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;
using Geo.Winform;
using Geo.Draw;
using System.Windows.Forms;
using Geo.Coordinates;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Service;
using Geo.Times; 


namespace Gnsser.Ntrip.WinForms
{
    /// <summary>
    /// 多站数据接收器
    /// </summary>
    public partial class DataReceiverForm : Form
    {
        Log log = new Log(typeof(DataReceiverForm));
        public DataReceiverForm()
        {
            InitializeComponent();
        }

        LogWriter LogWriter = LogWriter.Instance;
        MultiSiteNtripRunner NtripRunner { get; set; }

        private void button_start_Click(object sender, EventArgs e)
        {
            var selectedSites = Geo.Utils.DataGridViewUtil.GetObjects<NtripStream>(dataGridView_site);
            if (selectedSites.Count == 0)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请选择测站后再试！"); return;
            }
            button_start.Enabled = false;

            var tables = new BaseDictionary<string, SourceTable>(Setting.NtripSourceManager.SourceTables);
            var table = tables.First;
            var sites = Setting.NtripSourceManager.CurrentCasterSites[selectedSites[0].CasterName];//tables.FirstKey];

            IEphemerisService indicatedEph = null;
            if (System.IO.File.Exists(fileOpenControl_nav.FilePath))
            {
                indicatedEph = EphemerisDataSourceFactory.Create(fileOpenControl_nav.FilePath);
            }

            var type = GnssSolverTypeHelper.GetGnssSolverType(this.singleSiteGnssSolverTypeSelectionControl1.CurrentdType);


            NtripRunner = new MultiSiteNtripRunner();
            foreach (var item in selectedSites)
            {
                if (NtripRunner.Contains(item.Mountpoint)) { continue; }

                var caster = Setting.NtripSourceManager.NtripCasters[item.CasterName];

                var t = tables.Get(item.CasterName);
                NtripOption option = new NtripOption();
                option.IsWriteToLocal = this.checkBox_saveRawData.Checked;
                option.LocalDirectory = this.directorySelectionControl1.Path;
                option.CasterIp = caster.Host;
                option.CasterName = item.CasterName;
                option.Port = caster.Port;
                option.Username = caster.UserName;
                option.Password = caster.Password;
                option.PreferredMountPoint = item.Mountpoint;
                option.IsRequiresGGA = t.Get(item.Mountpoint).Num11 == 1;
                var runner = new NtripRunner(option);
                runner.GnssSolverType = type;

                runner.InfoProduced += NtripRunner_InfoProduced;
                runner.InstantInfoProduced += NtripRunner_InstantInfoProduced;
                NtripRunner.Add(item.Mountpoint, runner);
            }

            NtripRunner.Init();
            foreach (var runner in NtripRunner)
            {
                //runner.RealTimeGnssPositioner.EphemerisService = indicatedEph;
            }

            NtripRunner.Start();
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (NtripRunner != null)
            {
                NtripRunner.Stop();
            }

            button_start.Enabled = true;

            string filePath = this.directorySelectionControl1.Path;
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(filePath);
        }


        void NtripRunner_InfoProduced(string info) { this.ShowInfo(info); }

        void NtripRunner_InstantInfoProduced(string info) { this.ShowInstantInfo(info); }

        protected void ShowInfo(string msg) { Geo.Utils.FormUtil.InsertLineWithTimeToTextBox(this.richTextBoxControl1, msg); }
        /// <summary>
        /// 显示瞬时信息，显示完毕则丢失。
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowInstantInfo(string msg)
        {
            Geo.Utils.FormUtil.SetText(this.label_info, msg);
        }

        private void DataReceiverForm_Load(object sender, EventArgs e)
        {
            LogWriter.MsgProduced += LogWriter_MsgProduced;
          //  paramVectorRenderControl1.IndicatedYSpan

            bindingSource_site.DataSource = Setting.NtripSourceManager.GetCurretnNtripStreams();
        }

        void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer) { ShowInfo(msg); }

        private void 选择测站SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SourceTableForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<NtripStream> olds = bindingSource_site.DataSource as List<NtripStream>;
                if (olds == null) { olds = new List<NtripStream>(); }
                olds.AddRange(form.SelectedNtripStreams);
                bindingSource_site.DataSource = null;
                bindingSource_site.DataSource = olds;

                SaveSiteToLocalFile();
            }
        }

        private void 移除所选DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Geo.Utils.DataGridViewUtil.RemoveSelectedRows(dataGridView_site))
            {
                SaveSiteToLocalFile();
            }
        }


        private void SaveSiteToLocalFile()
        {
            var objs = Geo.Utils.DataGridViewUtil.GetObjects<NtripStream>(dataGridView_site);
            var newCasters = new Dictionary<string, List<string>>();
            foreach (var item in objs)
            {
                if (!newCasters.ContainsKey(item.CasterName))
                {
                    newCasters.Add(item.CasterName, new List<string>());
                }
                newCasters[item.CasterName].Add(item.Mountpoint);
            }

            Setting.NtripSourceManager.CurrentCasterSites.Clear();
            Setting.NtripSourceManager.CurrentCasterSites = newCasters;
            Setting.NtripSourceManager.SaveCurrentCasterSites();
        }

        private void DataReceiverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( NtripRunner!=null && NtripRunner.IsRunning)
            {
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    //解除绑定
                    LogWriter.MsgProduced -= LogWriter_MsgProduced;

                    if (NtripRunner != null)
                    {
                        NtripRunner.Dispose();
                    }
                }
            }
        }

        private void button_drawDxyz_Click(object sender, EventArgs e)
        {
           
            foreach (var item in NtripRunner.Values)
            {
                var runner = item;
                if (runner.RealTimeGnssPositioner == null || runner.RealTimeGnssPositioner.Solver == null)
                { continue; }

                var table = runner.RealTimeGnssPositioner.Solver.TableTextManager.First;
                this.paramVectorRenderControl1.SetTableTextStorage(table);// (names);
                this.paramVectorRenderControl1.DrawParamLines();

            }
            //   paramVectorRenderControl1.DrawTable(table);
            //ChartForm form = new ChartForm(table);
            //form.Show();
        }

        #region 显示和日志控制 

        private void checkBox_showData_CheckedChanged(object sender, EventArgs e)
        {

        }
           
        private void checkBox_debugModel_CheckedChanged(object sender, EventArgs e) { Setting.IsShowDebug = checkBox_debugModel.Checked; }
        private void checkBox_showWarn_CheckedChanged(object sender, EventArgs e) { Setting.IsShowWarning = checkBox_showWarn.Checked; }
        private void checkBox1_enableShowInfo_CheckedChanged(object sender, EventArgs e) { Setting.IsShowInfo = checkBox1_enableShowInfo.Checked; }

        private void checkBox_showError_CheckedChanged(object sender, EventArgs e) { Setting.IsShowError = this.checkBox_showError.Checked; }


        #endregion

        private void paramVectorRenderControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
