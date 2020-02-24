//2013.04, czs, create, 基于Bernese的分布式计算
//2015.11.02, czs, edit in K998  成都到西安南列车, 基于Gnsser操作任务的分布式计算   

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using Geo;
using Gnsser.Interoperation.Bernese;
using System.Net;//FtpWebRequest
using System.Windows.Forms;

namespace Gnsser.Winform
{
    /// <summary>
    /// 任务运行
    /// </summary>
    public class GnssTaskRunner
    {
        public event InfoProducedEventHandler InfoProduced;
        public event Action TaskFinished;

        Task task;
        
        public Interoperation.GPSTk.GpsTkBpe TkBpe { get; set; }

        /// <summary>
        /// 计算结果，Sinex文件。
        /// </summary>
        public string ResultRinexFtp { get; set; }
        string DataDir = @"C:\";//项目所在的上级目录

        string SaveCompaginDir = @"C:\Compagin\";//项目目录

        public GnssTaskRunner(string GpsDataDir = @"C:\")
        {
            this.DataDir = GpsDataDir;//
            
            this.TkBpe = new Interoperation.GPSTk.GpsTkBpe();
            this.TkBpe.WinCmd.ExitedOrDisposed += CmdHelper_ProcessExited;
        }

        void CmdHelper_ProcessExited(object sender, EventArgs e)
        {
            //获取结果
            string sinexPath = TkBpe.GetSinexPath(task.OperationName);
            string newName = task.Name + "_" + Path.GetFileName(sinexPath);
            this.ResultRinexFtp = Path.Combine(task.ResultFtp, newName);

            ShowInfo("正在上传结果文件" + sinexPath + " => " + this.ResultRinexFtp);
            new WebClient().UploadFile(this.ResultRinexFtp, sinexPath);
            ShowInfo("TaskRunner 执行完毕！");
            if (TaskFinished != null) TaskFinished();
        }
        /// <summary>
        /// 同步运行程序。
        /// </summary>
        /// <param name="task"></param>
        public void Run(Task task)
        {
            Init(task);    
            TkBpe.Run(task.OperationName);
        }
        /// <summary>
        /// 异步运行程序
        /// </summary>
        /// <param name="task"></param>
        public void RunAsyn(Task task)
        {
            Init(task);
            TkBpe.RunAsyn(task.OperationName);
        }

        private void Init(Task task)
        {
            this.task = task;
            this.SaveCompaginDir = Path.Combine(this.DataDir, task.Campaign); //本地工程保存路径
            PrepareData();
            this.ShowInfo("开始计算！");

          //  string teqcPath = "\"" + Application.StartupPath + "\\pPointPositioner.exe" + "\"";
            string startUpPath = Application.StartupPath;//可执行程序目录

            TkBpe.Init(task.Campaign, task.Time, true, startUpPath);
        }
        /// <summary>
        /// 数据准备（下载、解压）
        /// </summary>
        private void PrepareData()
        {
            string startUpPath = Application.StartupPath;//可执行程序目录
            //Go
            DataPrepare pre = new DataPrepare(
                task.Urls.ToArray(),
                Path.Combine(this.DataDir, task.Campaign, "Orx"), startUpPath, true, false, false, false);
            pre.InfoProduced += pre_InfoProduced;
            pre.strRun();

            this.ShowInfo("数据准备完毕！");
        }

        public void ShowInfo(string info) { if (InfoProduced != null)InfoProduced(info); }
        void pre_InfoProduced(string info) { ShowInfo(info); }

        public string GetBernRunningState()
        {
            return TkBpe.GetBernRunningState(task.OperationName);
        }
    }
}
