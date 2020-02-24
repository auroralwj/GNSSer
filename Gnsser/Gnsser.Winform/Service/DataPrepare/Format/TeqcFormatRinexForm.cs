using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Geo.Utils;

namespace Gnsser.Winform
{
    public partial class TeqcFormatRinexForm : Form
    { 
        public TeqcFormatRinexForm()
        {
            InitializeComponent();
        }

        private void button_setInDirPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_inDirPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void button_setOutPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_savePath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }
        private void button_ok_Click(object sender, EventArgs e)
        {
            DateTime fromTime = DateTime.Now;
            ShowInfo("正在转换中，请稍后……");
            ShowResult("开始：" + fromTime, false);

            string inDirPath = this.textBox_inDirPath.Text;
            List<String> inFileList = new List<string>();
            List<string> searchPatterns = new List<string>() { "*.**n", "*.**o", "*.**m" };

            SearchOption opt = checkBox_subIncluded.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;


            foreach (string sp in searchPatterns)
            {
                string[] inFiles = Directory.GetFiles(inDirPath, sp, opt);
                if (inFiles.Length != 0)
                    inFileList.AddRange(inFiles);
            }

            if (inFileList.Count == 0)
            {
                FormUtil.ShowErrorMessageBox("该文件夹下没有文件。"); return;
            }

            Geo.Utils.FormUtil.InitProgressBar(this.progressBar1, inFileList.Count); 

            string outDirPath = this.textBox_savePath.Text.Trim();
            if (outDirPath == String.Empty)
            {
                FormUtil.ShowErrorMessageBox("请选择输出文件夹。"); return;
            } 
            int num = 0; 
            TeqcFormater formater = new TeqcFormater();
            foreach (string inPath in inFileList)
            {
                string outPath = inPath.Replace(inDirPath, outDirPath);

                string outDir = Path.GetDirectoryName(outPath);
                if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);
                 

                //notice 
                ShowInfo("正在转换 " + inPath + " " + (++num) + "/" + inFileList.Count);


                string info = formater.Formate(inPath, outDir, checkBox_delOrigin.Checked);
                ShowResult(info);

                // Shell.Run(exePath + param);
                progressBar1.PerformStep();
            }

            DateTime toTime = DateTime.Now;
            TimeSpan span = toTime - fromTime;

            string msg = "转换完毕。共转换" + inFileList.Count + "个文件。耗时:" + span.ToString();
            ShowInfo(msg);
            msg += "\r\n是否打开输出文件夹？";
            ShowResult(msg);

            FormUtil.ShowOkAndOpenDirectory(outDirPath, msg);
        }

     


        private void ShowResult(string info, bool append = true)
        {
            if (!checkBox_output.Checked) return;

            Geo.Utils.FormUtil.AppendLineToTextBox(textBox_result, info); 
        }

        private void ShowInfo(string msg)
        {
            label_info.Text = msg;
            this.label_info.Refresh();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_inDirPath_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox_savePath.Text.Trim() == String.Empty || this.textBox_savePath.Text.Contains(this.textBox_inDirPath.Text))
                this.textBox_savePath.Text = this.textBox_inDirPath.Text + "\\Formated";
        }


    }
}
