//2016.08.21, czs, create in fujian yongan, 文件选择器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using System.Threading.Tasks;

namespace Gnsser.Winform
{

    public partial class KeyNameFileSelectorForm : Form
    {

        public KeyNameFileSelectorForm()
        {
            InitializeComponent();
        }

        private void button_getObsPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog_obs.ShowDialog() == DialogResult.OK) textBox_obsPath.Lines = openFileDialog_obs.FileNames;
        }



        private void button_read_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            directory = this.directorySelectionControl1.Path;
        }
        string directory;
        private void ObsFileMetaViewerForm_Load(object sender, EventArgs e)
        {
            this.textBox_obsPath.Text = Setting.GnsserConfig.SampleOFile;
            this.directorySelectionControl1.Path = Setting.GnsserConfig.TempDirectory;
        }

        string[] keys;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime from = DateTime.Now;

                string[] filePathes = this.textBox_obsPath.Lines;
                if (this.textBox_obsPath.Lines.Length == 0) throw new ArgumentNullException("请输入文件！");
                var keystr = this.textBox_keywords.Text;
                keys = keystr.Split(new char[] { ' ', '\t', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                ParallelRunning(filePathes);
            }
            catch (Exception ex) { MessageBox.Show("出错了！ " + ex.Message); }
        }

        StringBuilder FailedPathes;
        /// <summary>
        /// 并行
        /// </summary>
        /// <param name="inputPathes"></param>
        protected void ParallelRunning(string[] inputPathes)
        {
            this.progressBarComponent1.InitProcess(inputPathes.Length);
            this.progressBarComponent1.ShowInfo("正在计算！");
            FailedPathes = new StringBuilder();
            Parallel.ForEach(inputPathes, (inputPath, state) =>
          {
              if (this.backgroundWorker1.CancellationPending) { state.Stop(); }

              if (!Run(inputPath))
              {
                  FailedPathes.AppendLine(inputPath);
              }
              progressBarComponent1.PerformProcessStep();
          });
        }

        bool Run(string path)
        {
            foreach (var key in keys)
            {
                if (path.ToLower().Contains(key.ToLower()))
                {
                    var dest = Path.Combine(directory, Path.GetFileName(path));
                    File.Copy(path, dest);
                    return true;
                }
            }
            return false;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.textBox_out.Text = "执行完毕！失败路径如下：\r\n" + FailedPathes.ToString();
        }
    }
}