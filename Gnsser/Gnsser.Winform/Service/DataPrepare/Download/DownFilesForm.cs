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

namespace Gnsser.Winform
{
    /// <summary>
    /// 下载地址生成算法针对陆态网命名方法。
    /// 可以直接粘贴地址到地址框中下载。
    /// </summary>
    public partial class DownFilesForm : Form
    {
        public DownFilesForm()
        {
            InitializeComponent();
        } 

        private void button_download_Click(object sender, EventArgs e)
        {
            saveDir = this.textBox2_localPath.Text;
            fileUrls = this.textBox_fileurls.Lines;
            this.progressBar1.Maximum = fileUrls.Length;
            this.progressBar1.Minimum = 1;
            this.progressBar1.Step = 1;
            this.progressBar1.Value = this.progressBar1.Minimum;

            this.button_download.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void ShowInfo(string url)
        {
            this.Invoke(new Action(delegate()
            {
                this.textBox_result.Text = DateTime.Now + ":" + url + "\r\n" + this.textBox_result.Text;
                this.textBox_result.Update();
            }));
        }

        private void button1_placeToSave_Click(object sender, EventArgs e) { if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox2_localPath.Text = this.folderBrowserDialog1.SelectedPath; }
    
        private void button_setFilePath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_filePath.Text = this.openFileDialog1.FileName;
                button_readFile_Click(null, e);
            }
        }

        private void button_readFile_Click(object sender, EventArgs e)
        {
            this.textBox_fileurls.Lines = File.ReadAllLines(this.textBox_filePath.Text);
        }
        string saveDir;
        string[] fileUrls;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

            List<string> failed = new List<string>();
            foreach (string url in fileUrls)
            {

                string info = "下载成功 ";

                if (!Geo.Utils.NetUtil.FtpDownload(url, Path.Combine(saveDir, Path.GetFileName(url))))
                {
                    failed.Add(url);
                    info = "下载失败！ ";
                }
                if (backgroundWorker1.CancellationPending) return;

                ShowInfo(info + url);

                this.Invoke(new Action(PerformStep));

            }
            String msg = "下载完成。共下载 " + (fileUrls.Length - failed.Count) + " 个文件。\r\n下载失败 " + failed.Count + " 个。\r\n";
            StringBuilder sb = new StringBuilder();
            foreach (string fail in failed)
            {
                sb.AppendLine(fail);
            }
            msg += sb.ToString();

            msg += "\r\n是否打开目录？";
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(saveDir, msg);
        }

        private void PerformStep()
        {
            progressBar1.PerformStep();
            this.progressBar1.Update();
            this.Update();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button_download.Enabled = true;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void DownFilesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy )
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("确认关闭，关闭后将取消下载文件。")
                == System.Windows.Forms.DialogResult.Yes)
            {
                backgroundWorker1.CancelAsync();
            }   else 
                e.Cancel = true;
        }

        private void DownFilesForm_Load(object sender, EventArgs e)
        {
            textBox2_localPath.Text = Setting.TempDirectory;
        } 
    }
}
