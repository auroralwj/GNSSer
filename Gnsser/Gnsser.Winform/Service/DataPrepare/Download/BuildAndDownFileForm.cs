//2017.07.13, czs, edit in hongqing, 生成并下载

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
    public partial class BuildAndDownFileForm : Form
    {
        public BuildAndDownFileForm()
        {
            InitializeComponent();
        }

        private void button_gen_urls_Click(object sender, EventArgs e)
        {
            string basicUrl = this.textBox1_uri.Text;
            int year = int.Parse(this.textBox_year.Text);
            int fromDay = int.Parse(this.textBox_fromDay.Text);
            int toDay = int.Parse(this.textBox_toDay.Text);
            string nameRule = textBox_fileNameRule.Text;//
            GnssUrl gUrl = new GnssUrl(year, basicUrl + nameRule);
            List<string> urls = gUrl.GetUrls(fromDay, toDay, this.textBox_sites.Lines);

            this.textBox_fileurls.Lines = urls.ToArray();
        } 

        private void button_download_Click(object sender, EventArgs e)
        {
            string saveDir = this.textBox2_localPath.Text;
            string[] fileUrls = this.textBox_fileurls.Lines;
            this.progressBar1.Maximum = fileUrls.Length;
            this.progressBar1.Minimum = 1;
            this.progressBar1.Step = 1;
            this.progressBar1.Value = this.progressBar1.Minimum;

            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
             
            List<string> failed = new List<string>();
            foreach (string url in fileUrls)
            {
                if (!Geo.Utils.NetUtil.FtpDownload(url, Path.Combine(saveDir, Path.GetFileName(url))))
                    failed.Add(url);

                this.progressBar1.PerformStep();
                this.progressBar1.Update();
                this.Update();
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

        private void button1_placeToSave_Click(object sender, EventArgs e) { if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox2_localPath.Text = this.folderBrowserDialog1.SelectedPath; }
        private void checkBox_login_CheckedChanged(object sender, EventArgs e) { this.panel1.Visible = this.checkBox_login.Checked; }

        private void button_extractSiteNames_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string dirPath = this.folderBrowserDialog1.SelectedPath;
                string[] files = Directory.GetFiles(dirPath, "*.z");
                List<String> fileList = new List<string>();
                string tobeRemove = files[0].Substring(files[0].Length - 10);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file).Replace(tobeRemove, "");
                    fileList.Add(fileName);
                }
                this.textBox_sites.Lines = fileList.ToArray();
                MessageBox.Show("共提取 " + fileList.Count + " 个文件名。");
            }
        }

         


    }
}
