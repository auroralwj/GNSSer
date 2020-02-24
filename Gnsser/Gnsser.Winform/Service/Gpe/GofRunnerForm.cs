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
    public partial class OperflowRunnerForm : Form
    {
        public OperflowRunnerForm()
        {
            InitializeComponent();
            this.fileOpenControl1.FilePath = Setting.GnsserConfig.SampleGofFile;
        }

        public OperflowRunnerForm(string gofPath)
        {
            InitializeComponent();
            this.fileOpenControl1.FilePath = gofPath;
        }

        private void button_runOrStop_Click(object sender, EventArgs e)
        {
            if ( this.backgroundWorker1.IsBusy)
            {
                if (ProcessEngine != null)
                {
                    ProcessEngine.CancelProcessing = true;
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
        OperationProcessEngine ProcessEngine;
        DateTime startTime;
        private void Run()
        {
            startTime = DateTime.Now;

            var gpePath = this.fileOpenControl1.FilePath;

            OperationManager OperationManager = GnsserOperationManager.Default;
             
            ProcessEngine = new OperationProcessEngine(OperationManager, gpePath); 
            ProcessEngine.OperationCompleted += ProcessEngine_OperationProceed;
            ProcessEngine.OperationStatedMessageProduced += ProcessEngine_OperationStatedMessageProduced;
            ProcessEngine.OperationProcessing += ProcessEngine_OperationProcessing;
            this.progressBarComponent1.InitProcess(ProcessEngine.OperationInfos.Count);

            ProcessEngine.Process();

        }

        void ProcessEngine_OperationProcessing(IOperation oper)
        {
            this.progressBarComponent1.ShowClassifyInfo(" 正在执行：" +　oper.Name); 
        }

        void ProcessEngine_OperationStatedMessageProduced(StatedMessage StatedMessage)
        {
                this.progressBarComponent1.ShowInfo(StatedMessage.Message);
                ShowInfo(StatedMessage.Message);
        }

        void ProcessEngine_OperationProceed(IOperation oper)
        {
            this.progressBarComponent1.ShowClassifyInfo(oper.Name + " 执行完毕！");
            this.progressBarComponent1.PerformProcessStep(); 
        }
         
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Run();
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

    }
}
