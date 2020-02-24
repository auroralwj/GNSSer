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
    public partial class FileUrlGenForm : Form
    {
        public FileUrlGenForm()
        {
            InitializeComponent();
        }

        private void button_gen_urls_Click(object sender, EventArgs e)
        { 
            int year = int.Parse(this.textBox_year.Text);
            int fromDay = int.Parse(this.textBox_fromDay.Text);
            int toDay = int.Parse(this.textBox_toDay.Text);
            string nameRule = textBox_fileNameRule.Text;//
            GnssUrl gUrl = new GnssUrl(year,   nameRule);
            List<string> urls = gUrl.GetUrls(fromDay, toDay, this.textBox_sites.Lines);

            this.textBox_fileurls.Lines = urls.ToArray();
        } 
       
        private void button_extractSiteNames_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string dirPath = this.folderBrowserDialog1.SelectedPath;
                string[] files = Directory.GetFiles(dirPath, "*.*");
                List<String> fileList = new List<string>();
                //string tobeRemove = files[0].Substring(files[0].Length - 10);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file).Substring(0,4);
                    fileList.Add(fileName);
                }
                this.textBox_sites.Lines = fileList.ToArray();
                MessageBox.Show("共提取 " + fileList.Count + " 个文件名。");
            }
        }

        private void button_savePath_Click(object sender, EventArgs e)
        {
            File.WriteAllLines(textBox_urlPath.Text, this.textBox_fileurls.Lines);
            //SaveFileDialog dlg = new SaveFileDialog();
            //dlg.IsSatisfied = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            //dlg.FileName = "文件地址";
            //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    using (StreamWriter sw = new StreamWriter(dlg.FileName))
            //    {
            //        sw.Write(this.textBox_fileurls.Text);
            //    }


            string msg = "\r\n是否打开？";
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(textBox_urlPath.Text, msg);
            //}
        }
        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_path.Text = this.openFileDialog1.FileName;
                button_read_Click(sender, e);
            }

        }

        private void button_read_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.textBox_path.Text))
            {
                MessageBox.Show("文件不存在！"); return;
            }

            this.textBox_sites.Lines = File.ReadAllLines(this.textBox_path.Text);
        }

    }
}
