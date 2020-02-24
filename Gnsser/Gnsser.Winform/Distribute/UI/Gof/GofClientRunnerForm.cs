//2012,czs, create in zz, 分布式任务解析执行
//2015.11.02, czs edit in hongqing, Gnsser 分布式任务执行

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Geo.Common;
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
    public partial class GofClientRunnerForm : Form
    {
        Log log = new Log(typeof(GofClientRunnerForm));

        #region 构造函数
        /// <summary>
        /// 默认
        /// </summary>
        public GofClientRunnerForm()
        {
            InitializeComponent();
            InitProcessEngine();
        }
        /// <summary>
        /// 单个
        /// </summary>
        /// <param name="gofPath"></param>
        public GofClientRunnerForm(string gofPath)
        {
            InitializeComponent();
            InitProcessEngine();
            this.fileOpenControl1.FilePath = gofPath;
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="gofPathes"></param>
        public GofClientRunnerForm(string[] gofPathes)
        {
            InitializeComponent();
            InitProcessEngine();
            this.fileOpenControl1.FilePathes = gofPathes;
        }

        /// <summary>
        /// 初始化数据处理引擎
        /// </summary>
        private void InitProcessEngine()
        {
            try
            {
                ProcessEngine = new OperationProcessEngine(GnsserOperationManager.Default);
                ProcessEngine.GnsserConfig = Setting.GnsserConfig;
                ProcessEngine.BaseDirecory = Setting.GnsserConfig.CurrentProject.ProjectDirectory;

                ProcessEngine.OperationCompleted += ProcessEngine_OperationCompleted;
                ProcessEngine.OperationStatedMessageProduced += ProcessEngine_OperationStatedMessageProduced;
                ProcessEngine.OperationProcessing += ProcessEngine_OperationProcessing;

            }
            catch (Exception ex)
            {
                var msg = "出现错误： " + ex.Message;
                log.Error(msg);
                Geo.Utils.FormUtil.HandleException(ex, msg);
            }
        } 

        #endregion

        #region  属性
        /// <summary>
        /// 处理引擎
        /// </summary>
        OperationProcessEngine ProcessEngine;

        DateTime startTime;

        TcpListener TcpListener { get; set; }
        Thread Thread { get; set; }
        /// <summary>
        /// 控制端端口
        /// </summary>
        int MasterPort { get; set; }
        /// <summary>
        /// 控制端IP
        /// </summary>
        string MasterIp { get; set; } //,单机测试版

        static bool Stop = true;
        /// <summary>
        /// 命令集合
        /// </summary>
        List<TelCommand> list = new List<TelCommand>();
        /// <summary>
        /// 网络
        /// </summary>
        TcpClient TcpClient { get; set; }
        /// <summary>
        /// 当前任务
        /// </summary>
        GofTask CurrentTask { get; set; }
        #endregion

        #region 运行任务或停止
        private void button_runOrStop_Click(object sender, EventArgs e)
        {
            RunOrStopGpe();
        }

        private void RunOrStopGpe()
        {
            if (this.backgroundWorker1.IsBusy)
            {
                ProcessEngine.CancelProcessing = true;
                this.backgroundWorker1.CancelAsync();
                log.Info("已经请求停止执行！请稍后！");
            }
            else
            {
                RunGpeAsync();
            }
        }

        private void RunGpeAsync()
        {
            ProcessEngine.CancelProcessing = false;
            this.backgroundWorker1.RunWorkerAsync();
            this.Invoke(new Action(SetButtonStopText));
        }

        private void SetButtonStopText() { this.button_runOrStop.Text = "停止运行"; }
        private void SetButtonRunText() { this.button_runOrStop.Text = "运行"; }

        /// <summary>
        /// 添加任务到当前引擎，并显示在界面上。
        /// </summary>
        /// <param name="gofPathes"></param>
        private void AddTasksAndRun(string[] gofPathes)
        {
            //判断是否是相对路径，如果是，则转换为绝对路径，以当前工程脚本文件目录为基础
            gofPathes = Setting.GnsserConfig.CurrentProject.GetAbsScriptPath(gofPathes);
            ProcessEngine.OperationInfos.Clear();

            ProcessEngine.AddGofes(gofPathes.ToArray());

            if (!ProcessEngine.IsRunning)
            {
                ProcessEngine.Process();//在此处理！！
                this.progressBarComponent1.InitProcess(ProcessEngine.OperationInfos.Count);
            }
        }
        /// <summary>
        /// 把路径添加到界面文本框。
        /// </summary>
        /// <param name="gofPathes"></param>
        private void AppendPathToTextBox(string[] gofPathes)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(delegate()
                {
                    List<string> pathes = new List<string>(this.fileOpenControl1.FilePathes);
                    pathes.AddRange(gofPathes);

                    this.fileOpenControl1.FilePathes = pathes.ToArray();
                    this.progressBarComponent1.SetProgressCount(pathes.Count);
                }));
            }
        }

        void ProcessEngine_OperationProcessing(IOperation oper)
        {
            this.progressBarComponent1.ShowClassifyInfo(" 正在执行：" + oper.Name);
        }

        void ProcessEngine_OperationStatedMessageProduced(StatedMessage StatedMessage)
        {
            this.progressBarComponent1.ShowInfo(StatedMessage.Message);
            ShowInfo(StatedMessage.Message);
        }

        void ProcessEngine_OperationCompleted(IOperation oper)
        { 
            //通知结果
            var url = Path.Combine( Setting.GnsserConfig.FtpServerUrl, "Result",  NodeName + "_PppResult_GnssResults.xls");
            ReportResultToServer(url);

            this.progressBarComponent1.ShowClassifyInfo(oper.Name + " 执行完毕！");
            this.progressBarComponent1.PerformProcessStep();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var gpePathes = this.fileOpenControl1.FilePathes;
            startTime = DateTime.Now;

            AddTasksAndRun(gpePathes);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var span = DateTime.Now - startTime;
            log.Info("执行结束！耗时" + span);
            this.button_runOrStop.Text = "运行";
            //MessageBox.Show("执行结束！ 耗时" + span);
        }
        private void WorkflowRunnerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           // if (backgroundWorker1.IsBusy)
            {
                if (FormUtil.ShowYesNoMessageBox("确定退出客户端执行？") == DialogResult.No)
                { e.Cancel = true; }
                else
                {
                    //
                    CloseListener();

                    backgroundWorker1.CancelAsync();
                    LogWriter.MsgProduced -= LogWriter_MsgProduced;
                }
            } 
        }
        #endregion

        #region 监听网络并执行
        private void button_listenOrStop_Click(object sender, System.EventArgs e)
        {
            if (Stop)
            {
                StartListen();
            }
            else
            {
                Action action = CloseListener;
                action.BeginInvoke(ar => { if (ar.IsCompleted) ShowInfo("异步操作结束"); }, null);
            }
        }
        string NodeName = "Node";
        /// <summary>
        /// 开始监听
        /// </summary>
        private void StartListen()
        {
            NodeName = this.textBox_nodeName.Text;

            int listenPort = int.Parse(this.textBox_listenPort.Text);
            this.MasterPort = int.Parse(this.textBox_controlPort.Text);
            this.MasterIp = this.textBox_controlIp.Text;

            if (TcpListener == null)
            {
               TcpListener = new TcpListener(IPAddress.Any, listenPort);
            }

            Stop = false;
            this.label_state_info.Text = "正在运行....";
             
             
            TcpListener.Start(); 
           

            Thread = new Thread(ListenClient);
            Thread.Start();
            this.button_listenOrStop.Text = "停止监听";
        }

        public void ListenClient()
        {
            //try
            //{
            while (true)
            {
                if (Stop) break;
                try
                {
                    TcpClient = TcpListener.AcceptTcpClient();
                }
                catch (Exception ex)
                {
                    ShowInfo(ex.Message);
                    break;
                }
                TelCommand cmd = new TelCommand(TcpClient);
                list.Add(cmd);

                string info = "来自" + cmd.LocalEndPoint + ",命令：" + cmd.CmdStr + "\r\n";
                ShowInfo(info);
                ShowInfo("开始执行任务....");

                //解析并保存到当前目录
                CurrentTask = GofTask.ParseXml(TelMsg.ParseXml(cmd.CmdStr).XmlContent);
                var localGofPath = SaveGofToLocal(CurrentTask);

                //开始执行
                var pathes = new string[] { localGofPath };
                this.AppendPathToTextBox(pathes);
                this.AddTasksAndRun(pathes);
            }
            //}
            //catch (Exception ex)
            //{
            //    if (!this.IsDisposed)
            //        ShowInfo("运行错误：" + ex.Message);
            //}
        }

        #region 保存到本地
        /// <summary>
        /// 保存 GOF 及其参数文件到本地
        /// </summary>
        /// <param name="CurrentTask"></param>
        /// <returns></returns>
        private string SaveGofToLocal(GofTask CurrentTask)
        {
            //在此解析执行！
            //保存 Gof 文件，保存参数文件。执行！
            var name = CurrentTask.Name;
            if (!name.ToLower().Contains(".gof")) { name += ".gof"; }
            var localGofPath = Path.Combine(Setting.GnsserConfig.CurrentProject.ScriptDirectory, name);
            StringBuilder sb = new StringBuilder();
            foreach (var oper in CurrentTask.OperationInfos)
            {
                sb.AppendLine(oper.ToString());
            }
            File.WriteAllText(localGofPath, sb.ToString());

            //保存参数文件
            SaveParamToLocal(CurrentTask);
            return localGofPath;
        }
        /// <summary>
        /// 将参数文件保存到本地
        /// </summary>
        private void SaveParamToLocal(GofTask CurrentTask)
        {
            foreach (var kv in CurrentTask.Params)
            {
                var localParamPath = Path.Combine(Setting.GnsserConfig.CurrentProject.ParamDirectory, kv.Key);
                File.WriteAllText(localParamPath, kv.Value);
            }
        }
        #endregion

        /// <summary>
        /// 任务执行完后，报告控制端，让其下载结果。
        /// </summary>
        void ReportResultToServer(string ResultUrl)
        {
            try
            {
                //发送结果数据到服务器
                //    string ResultUrl = "";//runner.ResultRinexFtp;
                TcpClient newclient = new TcpClient(MasterIp, MasterPort);
                BinaryWriter newbr = new BinaryWriter(newclient.GetStream());

                TelMsg tMsg = new TelMsg()
                {
                    MsgType = Winform.MsgType.Path,
                    XmlContent = ResultUrl,
                    From = Geo.Utils.NetUtil.GetIp(),
                    To = MasterIp
                };

                newbr.Write(tMsg.ToXml());
                newbr.Flush();
            }
            catch (Exception ex)
            {
                log.Info("向服务器返回数据出错：" + ex.Message);
            }
        }
        /// <summary>
        ///关闭
        /// </summary>
        private void CloseListener()
        {
            log.Info("正在请求停止监听....");

            Stop = true;

            if (TcpListener != null)
            {
                TcpListener.Stop();
            }
            if (Thread != null && Thread.IsAlive)
            {
                Thread.Abort();
            }

            log.Info("已经成功停止监听。");

            this.Invoke(new Action(delegate()
            {
                this.button_listenOrStop.Text = "启动监听";
                this.label_state_info.Text = "没有运行";
            }));
        }
        #endregion

        #region 界面工具
        private void ShowInfo(string Message)
        {
            Geo.Utils.FormUtil.InsertLineWithTimeToTextBox(richTextBoxControl1, Message);
        }
        #endregion
        LogWriter LogWriter = LogWriter.Instance;
        private void GofClientRunnerForm_Load(object sender, EventArgs e)
        {
            this.textBox_controlIp.Text = Setting.GnsserConfig.ControlIp;
            this.textBox_controlPort.Text = Setting.GnsserConfig.ControlPort + "";
            textBox_controlIp.Text = Setting.GnsserConfig.ControlIp;

            LogWriter.MsgProduced+=LogWriter_MsgProduced; 
        }


        void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer)
        {
            var info = LogType.ToString() + "\t" + msg;// +"\t" + msgProducer.Name;
            ShowInfo(info);
        }

    }
}
