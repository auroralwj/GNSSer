//2018.03.22, czs, create in hmx, OptionRunnerForm 运行器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.IO;
using Geo;
using Geo.Times;
using Gnsser.Api; 

namespace Gnsser.Winform
{
    public partial class OptionRunnerForm : Form
    {
        public OptionRunnerForm()
        {
            InitializeComponent();
            this.fileOpenControl1.FilePath = Setting.GnsserConfig.SampleOptFile;
        }

        public OptionRunnerForm(string gofPath)
        {
            InitializeComponent();
            this.fileOpenControl1.FilePath = gofPath;
        }

        private void button_runOrStop_Click(object sender, EventArgs e)
        {
            if ( this.backgroundWorker1.IsBusy)
            {
                if (Runner != null)
                {
                    Runner.IsCancel = true; 
                    this.backgroundWorker1.CancelAsync();
                    ShowInfo("已经请求停止执行！请稍后！");
                }
            }
            else
            {
                this.backgroundWorker1.RunWorkerAsync();
                this.button_runOrStop.Text = "停止";
            }
        }
       
        DateTime startTime;
        PointPositionBackGroundRunner Runner;


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            startTime = DateTime.Now;
               //Run();
               var optPath = this.fileOpenControl1.FilePath;
            if(String.IsNullOrWhiteSpace(optPath) || !System.IO.File.Exists(optPath))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件路径不存在 ！" + optPath);
                return;
            }
            Runner = new PointPositionBackGroundRunner(optPath);
            Runner.ProgressViewer = progressBarComponent1;
            Runner.Init();
            Runner.Run();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var span = DateTime.Now - startTime;
            ShowInfo("耗时" + span);
            this.button_runOrStop.Text = "运行";
            MessageBox.Show("执行结束！ 耗时" + span);
        }
        private void ShowInfo(string Message)
        {
            this.Invoke(new Action(delegate()
            {
                int maxCount = 2000;
                var msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + Message;
                if (this.richTextBoxControl1.Lines.Length > maxCount)
                {
                    var list = new List<string>(this.richTextBoxControl1.Lines);
                    list.GetRange(0, maxCount);
                    list.Insert(0, msg);
                    this.richTextBoxControl1.Lines = list.ToArray();
                }
                else
                {
                    this.richTextBoxControl1.Text = msg + "\r\n" + this.richTextBoxControl1.Text;
                    this.richTextBoxControl1.Update();

                }

            }));
        }

        private void OptionRunnerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
