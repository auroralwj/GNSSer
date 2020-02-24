//2018.05.03, czs, create in hmx, 观测文件生成并下载

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

namespace Gnsser.Winform
{
    /// <summary>
    /// 观测文件生成并下载
    /// </summary>
    public partial class ObsFileDownloadForm : Form
    {
        public ObsFileDownloadForm()
        {
            InitializeComponent();
        }

        private void button_gen_urls_Click(object sender, EventArgs e)
        {
            string urlModel = this.textBox1_uriModel.Text;
            DateTime timeFrom = timePeriodControl1.TimePeriod.StartDateTime;//.TimeFrom;
            DateTime timeTo = this.timePeriodControl1.TimePeriod.EndDateTime;//.TimeTo;

            string[] siteNames = namedStringControl_siteNames.GetValue().Split(new char[] { ',', '，',' ' }, StringSplitOptions.RemoveEmptyEntries);

            var daySpan = TimeSpan.FromDays(1);
            List<string> urls = ELMarkerReplaceService.BuildWithTime(urlModel, timeFrom, timeTo, TimeSpan.FromDays(1));
            urls = ELMarkerReplaceService.BuildWithKeys(urls, ELMarker.SiteName, siteNames);

            richTextBoxControl_allUrls.Lines = urls.ToArray();

            var info = "生成 " + urls.Count + " 条数据。";
            ShowInfo(info);
            Geo.Utils.FormUtil.ShowOkMessageBox(info);
        }

        private void button_download_Click(object sender, EventArgs e)
        {
            this.button_download.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void checkBox_login_CheckedChanged(object sender, EventArgs e) { this.panel1.Visible = this.checkBox_login.Checked; }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string saveDir = this.directorySelectionControl1.Path;


            string[] fileUrls = null;
            this.Invoke(new Action(() => { fileUrls = this.richTextBoxControl_allUrls.Lines; }));


            InitProgressBar(fileUrls);

            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

            List<string> failed = new List<string>();
            foreach (string url in fileUrls)
            {
                if (!Geo.Utils.NetUtil.FtpDownload(url, Path.Combine(saveDir, Path.GetFileName(url))))
                {
                    failed.Add(url);
                    AppendToFailedTextBox(url);
                }
                else
                {
                    ShowInfo( "成功下载了 " + url );
                }

                PerformStep();
            } 

            //richTextBoxControl_failedUrls.Lines = failed.ToArray();

            String msg = "下载完成。共下载 " + (fileUrls.Length - failed.Count) + " 个文件。\r\n下载失败 " + failed.Count + " 个。\r\n";       

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
//      ftp://igs.gnsswhu.cn/pub/gps/data/daily/2017/016/17o/antc0160.17o.Z
        string wuda = "ftp://igs.gnsswhu.cn/pub/gps/data/daily/{Year}/{DayOfYear}/{SubYear}o/{SiteName}{DayOfYear}0.{SubYear}o.Z";
        string cddis = "ftp://cddis.gsfc.nasa.gov/pub/gps/data/daily/{Year}/{DayOfYear}/{SubYear}o/{SiteName}{DayOfYear}0.{SubYear}o.Z";
  
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="info"></param>
        private void ShowInfo(string info)
        {
            Geo.Utils.FormUtil.InsertLineToTextBox(textBox_result, info, 5000);
        }
        private void AppendToFailedTextBox(string url)
        {
            Geo.Utils.FormUtil.InsertLineToTextBox(richTextBoxControl_failedUrls, url, 50000);
           // Geo.Utils.FormUtil.InvokeTextBoxSetText(richTextBoxControl_failedUrls, url + "\r\n", true);
        }

        private void NaviFileDownloadForm_Load(object sender, EventArgs e)
        {
            namedStringControl_siteNames.SetValue("ajac,algo");
            textBox1_uriModel.Text = cddis;
            directorySelectionControl1.Path = Setting.TempDirectory;
        }

        private void radioButton1cddis_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_uriModel.Text = cddis;
        }

        private void radioButton2gnsswhu_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_uriModel.Text = wuda;
        }
    }
}
