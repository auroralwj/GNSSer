using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading; 
using Gnsser.Interoperation.Bernese;
using System.Net;//FtpWebRequest
using Geo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 任务运行
    /// </summary>
    public class TaskRunner
    {
        public event InfoProducedEventHandler InfoProduced;
        public event Action TaskFinished;

        Task task;
        public Interoperation.Bernese.BerBpe Bpe { get; set; }
        /// <summary>
        /// 计算结果，Sinex文件。
        /// </summary>
        public string ResultRinexFtp { get; set; }
        string BerDataDir = @"C:\GPSDATA\";

        public TaskRunner(string BerGpsDataDir = @"C:\GPSDATA\")
        {
            this.BerDataDir = BerGpsDataDir;
            this.Bpe = new BerBpe();
            this.Bpe.WinCmd.ExitedOrDisposed += CmdHelper_ProcessExited;
        }

        void CmdHelper_ProcessExited(object sender, EventArgs e)
        {
            //获取结果
            string sinexPath = Bpe.GetSinexPath(task.OperationName);
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
            Bpe.Run(task.OperationName);
        }
        /// <summary>
        /// 异步运行程序
        /// </summary>
        /// <param name="task"></param>
        public void RunAsyn(Task task)
        {
            Init(task);
            Bpe.RunAsyn(task.OperationName);
        }

        private void Init(Task task)
        {
            this.task = task;
            PrepareData();
            this.ShowInfo("开始计算！");
            Bpe.Init(task.Campaign, task.Time, true);
        }
        /// <summary>
        /// 数据准备（下载、解压）
        /// </summary>
        private void PrepareData()
        {
            //Go
            DataPrepare pre = new DataPrepare(
                task.Urls.ToArray(),
                Path.Combine(this.BerDataDir, task.Campaign, "ORX")  );
            pre.InfoProduced += pre_InfoProduced;
            pre.Run();

            this.ShowInfo("数据准备完毕！");
        }

        public void ShowInfo(string info) { if (InfoProduced != null)InfoProduced(info); }
        void pre_InfoProduced(string info) { ShowInfo(info); }

        public string GetBernRunningState()
        {
            return Bpe.GetRunningState(task.OperationName);
        }
    }


}
