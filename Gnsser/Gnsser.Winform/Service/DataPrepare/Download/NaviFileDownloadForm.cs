//2017.10.09, czs, create in hongqing, 导航文件生成并下载
//2017.10.17, czs, edit in hongqing,增加 cddis 选项，数据更全

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Geo;
using Geo.IO;

namespace Gnsser.Winform
{
    /// <summary>
    /// 导航文件生成并下载
    /// </summary>
    public partial class NaviFileDownloadForm : Form
    {
        Log log = new Log(typeof(NaviFileDownloadForm));
        public NaviFileDownloadForm()
        {
            InitializeComponent();
        }

        private void button_gen_urls_Click(object sender, EventArgs e)
        {
            string urlModel = this.textBox1_uriModel.Text;
            DateTime timeFrom = timePeriodControl1.TimePeriod.StartDateTime;//.TimeFrom;
            DateTime timeTo = this.timePeriodControl1.TimePeriod.EndDateTime;//.TimeTo;

            var daySpan = TimeSpan.FromDays(1);
            List<string> urls = ELMarkerReplaceService.BuildWithTime(urlModel, timeFrom, timeTo, TimeSpan.FromDays(1));

            this.richTextBoxControl1.Lines = urls.ToArray();

            log.Info("生成 " + urls.Count + " 条地址。");
        }

        private void button_download_Click(object sender, EventArgs e)
        {
            this.button_download.Enabled = false;
            fileUrls = this.richTextBoxControl1.Lines;
            saveDir = this.directorySelectionControl1.Path;
            backgroundWorker1.RunWorkerAsync();

        }

        string saveDir = null;
        string[] fileUrls = null;
        private void checkBox_login_CheckedChanged(object sender, EventArgs e) { this.panel1.Visible = this.checkBox_login.Checked; }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            InitProgressBar(fileUrls);

            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

            List<string> failed = new List<string>();
            foreach (string url in fileUrls)
            {
                if (!Geo.Utils.NetUtil.FtpDownload(url, Path.Combine(saveDir, Path.GetFileName(url))))
                    failed.Add(url);

                PerformStep();
            }
            String msg = "下载完成。共下载 " + (fileUrls.Length - failed.Count) + " 个文件。\r\n下载失败 " + failed.Count + " 个。\r\n";
            StringBuilder sb = new StringBuilder();
            foreach (string fail in failed)
            {
                sb.AppendLine(fail);
            }
            log.Info(sb.ToString());
            this.Invoke(new Action(
                delegate ()
                {
                    richTextBoxControl_failedUrls.Lines = failed.ToArray();
                }));
            msg += sb.ToString();

            msg += "\r\n是否打开目录？";
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(saveDir, msg);

        }

        private void InitProgressBar(string[] fileUrls)
        {
            Invoke(new Action(delegate()
            {
                this.progressBar1.Maximum = fileUrls.Length;
                this.progressBar1.Minimum = 1;
                this.progressBar1.Step = 1;
                this.progressBar1.Value = this.progressBar1.Minimum;
            }));
        }

        private void PerformStep()
        {
            Invoke(new Action(delegate()
            {
                this.progressBar1.PerformStep();
                this.progressBar1.Update();
                this.Update();
            }));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button_download.Enabled = true;
        }

        string wudaPFile = "ftp://igs.gnsswhu.cn/pub/gnss/BRDC_v3/{Year}/{DayOfYear}/brdm{DayOfYear}0.{SubYear}p.Z";
        string cddisPFile = "ftp://cddis.gsfc.nasa.gov/pub/gps/data/campaign/mgex/daily/rinex3/{Year}/brdm/brdm{DayOfYear}0.{SubYear}p.Z";
        string cddisNfile = "ftp://cddis.gsfc.nasa.gov/pub/gps/data/daily/{Year}/{DayOfYear}/{SubYear}n/brdc{DayOfYear}0.{SubYear}n.Z";

        private void NaviFileDownloadForm_Load(object sender, EventArgs e)
        {
            textBox1_uriModel.Text = cddisPFile;
            directorySelectionControl1.Path = Setting.TempDirectory;
        }

        private void radioButton1cddis_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_uriModel.Text = cddisPFile;
        }

        private void radioButton2gnsswhu_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_uriModel.Text = wudaPFile;
        }

        private void radioButton_cddisNFile_CheckedChanged(object sender, EventArgs e)
        {
           textBox1_uriModel.Text = cddisNfile;
        }
    }
}
