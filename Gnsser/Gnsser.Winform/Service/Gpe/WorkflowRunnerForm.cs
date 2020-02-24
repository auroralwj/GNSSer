//2015.10.21, czs, create in hongqing, 工作流运行器，即多个 gof 文件运行

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
    /// <summary>
    /// 工作流运行器，即多个 gof 文件运行
    /// </summary>
    public partial class WorkflowRunnerForm : Form
    {
        Log log = new Log(typeof(WorkflowRunnerForm));
        #region 构造函数
        /// <summary>
        /// 默认
        /// </summary>
        public WorkflowRunnerForm()
        {
            InitializeComponent();
            this.fileOpenControl1.FilePath = Setting.GnsserConfig.SampleGofFile;
        }
        /// <summary>
        /// 单个
        /// </summary>
        /// <param name="gofPath"></param>
        public WorkflowRunnerForm(string gofPath)
        {
            InitializeComponent();
            this.fileOpenControl1.FilePath = gofPath;
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="gofPathes"></param>
        public WorkflowRunnerForm(string [] gofPathes)
        {
            InitializeComponent();
            
            this.fileOpenControl1.FilePathes = gofPathes;
        }
        #endregion

        OperationProcessEngine ProcessEngine { get; set; }
        DateTime startTime;

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
                if (ProcessEngine != null)
                {
                    ProcessEngine.CancelProcessing = false;
                }
                this.backgroundWorker1.RunWorkerAsync();
                this.button_runOrStop.Text = "停止";
            }
        }

        /// <summary>
        /// 核心运行程序
        /// </summary>
        private void Run()
        {
            startTime = DateTime.Now;

            var gpePathes = this.fileOpenControl1.FilePathes;
            //判断是否是相对路径，如果是，则转换为绝对路径，以当前工程脚本文件目录为基础
            gpePathes = Setting.GnsserConfig.CurrentProject.GetAbsScriptPath(gpePathes);          

            ProcessEngine.SetGofes(gpePathes);
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
          

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e){   Run(); }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var span = DateTime.Now - startTime;
            log.Fatal("本次工作流执行完毕！耗时 " + span);
            ShowInfo("耗时" + span);
            this.button_runOrStop.Text = "运行";
            var msg = "执行结束！ 耗时" + span + ", " + "是否打开工程输出目录？" + Setting.GnsserConfig.CurrentProject.OutputDirectory;

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(Setting.GnsserConfig.CurrentProject.OutputDirectory, msg);                 
        }

        private void ShowInfo(string Message)
        {
            if (this.Disposing || this.IsDisposed) return;

            var info = DateTimeUtil.GetFormatedTimeNow(true) + ":\t" + Message;
            FormUtil.InsertLineToTextBox(this.richTextBoxControl1, info);  
        }

        private void WorkflowRunnerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                if (FormUtil.ShowYesNoMessageBox("请留步，后台没有运行完毕。是否一定要退出？") == DialogResult.No)
                { e.Cancel = true; }
                else {
                    logWriter.MsgProduced -= LogWriter_MsgProduced;
                }
            else
            {
                backgroundWorker1.CancelAsync();
                logWriter.MsgProduced -= LogWriter_MsgProduced;
            
            }
        }
        LogWriter logWriter = LogWriter.Instance;
        private void WorkflowRunnerForm_Load(object sender, EventArgs e)
        {

            OperationManager OperationManager = GnsserOperationManager.Default;

            ProcessEngine = new OperationProcessEngine(OperationManager);
            ProcessEngine.IsParallel = false;
            ProcessEngine.ProcessCount = 1;
            ProcessEngine.GnsserConfig = Setting.GnsserConfig;
            ProcessEngine.BaseDirecory = Setting.GnsserConfig.CurrentProject.ProjectDirectory;

            ProcessEngine.OperationCompleted += ProcessEngine_OperationProceed;
            ProcessEngine.OperationStatedMessageProduced += ProcessEngine_OperationStatedMessageProduced;
            ProcessEngine.OperationProcessing += ProcessEngine_OperationProcessing;



            
            logWriter.MsgProduced += LogWriter_MsgProduced;
            this.checkBox_debugModel.Checked =    Setting.IsShowDebug;
            this.checkBox_showError.Checked = Setting.IsShowError;
            this.checkBox_showWarn.Checked = Setting.IsShowWarning;
            this.checkBox1IsShowProcessInfo.Checked = Setting.IsShowInfo;
        }
        bool IsShowProcessInfo { get { return checkBox1IsShowProcessInfo.Checked; } }
        void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer)
        {
            if (IsShowProcessInfo ||  LogType != Geo.IO.LogType.Fatal)
            {
                var info = LogType.ToString() + "\t" + msg;// +"\t" + msgProducer.Name;
                ShowInfo(info);
            }
        }

        private void checkBox1IsShowProcessInfo_CheckedChanged(object sender, EventArgs e)
        {
            Setting.IsShowInfo = checkBox1IsShowProcessInfo.Checked;
        }

        private void checkBox_showWarn_CheckedChanged(object sender, EventArgs e)
        {
            Setting.IsShowWarning = checkBox_showWarn.Checked;

        }

        private void checkBox_debugModel_CheckedChanged(object sender, EventArgs e)
        {
            Setting.IsShowDebug = checkBox_debugModel.Checked;
        }

        private void checkBox_showError_CheckedChanged(object sender, EventArgs e)
        {
            Setting.IsShowError = checkBox_showError.Checked; ;

        }
    }
}
