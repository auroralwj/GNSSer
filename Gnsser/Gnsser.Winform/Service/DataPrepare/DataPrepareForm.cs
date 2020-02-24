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
using Geo;

namespace Gnsser.Winform
{
    public partial class DataPrepareForm : Form
    {
        public DataPrepareForm()
        {
            InitializeComponent();
        }
        private void button_setFilePath_Click(object sender, EventArgs e)
        {
            if (radioButton_local.Checked)
            {
                if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.textBox_urlfilePath.Text = this.folderBrowserDialog1.SelectedPath;
            }
            else
                if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.textBox_urlfilePath.Text = this.openFileDialog1.FileName;
                }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.button_OK.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        DataPrepare prepare;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            watch.Stop();
            watch.Start();

            string[] inputs = this.textBox_urlfilePath.Lines;
            string[] outputs = this.directorySelectionControl1.Pathes;
            if (inputs.Length != outputs.Length)
            {
                MessageBox.Show("输入输出路径数量必须一致！");
                return;
            }
            for (int i = 0; i < inputs.Length; i++)
            {
                var path = inputs[i];
                var outpath = outputs[i];

                string[] files;
                if (radioButton_local.Checked)
                {
                    string pattern = "*.*";
                    if (this.checkBox1.Checked) pattern = "*.*Z";
                    files = Directory.GetFiles(path, pattern);
                }
                else files = File.ReadAllLines(path);
                prepare = new DataPrepare(
                    files,
                    outpath,
                    Application.StartupPath,
                    this.radioButton_remote.Checked,
                    this.checkBox1.Checked,
                    this.checkBox_teqcFormat.Checked,
                    this.checkBox_formatOfile.Checked,
                    this.radioButton_up.Checked || this.radioButton_low.Checked,
                    this.radioButton_up.Checked,
                    this.checkBox_delOriFile.Checked,
                    this.checkBox_delMidFile.Checked,
                    this.checkBox_ignoreError.Checked,
                    this.progressBarComponent1);
                prepare.InfoProduced += new InfoProducedEventHandler(prepare_InfoProduced);
                prepare.Run();

            }
            watch.Stop();
            string msg = "\r\n是否打开目录？";
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(this.directorySelectionControl1.Path, msg);
        }

        void prepare_InfoProduced(string info)
        {
            this.Invoke(new Action(delegate() { this.textBox_result.Text = DateTime.Now + ": " + info + "\r\n" + this.textBox_result.Text; }));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button_OK.Enabled = true;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (prepare != null) prepare.IsCancelProcessing = true;

            backgroundWorker1.CancelAsync();
            this.button_OK.Enabled = true;
        }

        private void DownFilesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                if (Geo.Utils.FormUtil.ShowYesNoMessageBox("确认关闭，关闭后将取消下载文件。")
                == System.Windows.Forms.DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
                }
                else
                    e.Cancel = true;
        }

        private void DataPrepareForm_Load(object sender, EventArgs e)
        {
            directorySelectionControl1.Path = Setting.TempDirectory;
        }

    }
}