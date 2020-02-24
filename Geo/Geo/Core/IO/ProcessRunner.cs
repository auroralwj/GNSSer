//2014.05.21,czs, add logs
//2014.11.24, lv, add StandardError, by cy
//2015.07.12, czs, 修改，不再基于命令行程序 , 可以基于任何可执行程序。


using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Diagnostics;//加入，使用进程类，创建独立进程
using Microsoft.CSharp;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Geo.Common
{
    /// <summary>
    /// 处理操作
    /// </summary>
    /// <param name="cmd"></param> 
    /// <returns></returns>
    delegate void CmdHandler(string cmd);

    /// <summary>
    ///  封装了一个Process类。对指定程序提供便利的操作。
    ///  通常一个线程结束了，就重新再建立一个。
    /// </summary>
    public class ProcessRunner : IDisposable
    {
        /// <summary>
        /// 是否启用调试模式，即是否显示黑色命令框。
        /// </summary>
        public bool IsDebug = false;
        /// <summary>
        /// 程序退出事件
        /// </summary>
        public event EventHandler ExitedOrDisposed;
        /// <summary>
        /// 被调用程序输出数据事件
        /// </summary>          
        public event DataReceivedEventHandler ErrorDataReceived; 
        /// <summary>
        /// 输出数据
        /// </summary>
        public event DataReceivedEventHandler OutputDataReceived;

        /// <summary>
        /// 异步执行中程序执行完毕时激活事件。
        /// </summary>
        public event AsyncCallback AsyncProcessExited;

        /// <summary>
        /// 构造函数。如果启用管道（IsRedirectPipe），则不用shell启动进程（IsUseShellExecute）。
        /// 如果采用异步（！IsSyncInput）输入，则需要重定向（IsRedirectPipe）。
        /// </summary>
        /// <param name="exePath">默认为cmd.exe</param>
        /// <param name="IsUseShellExecute">是否采用shell启动进程</param>
        /// <param name="IsRedirectPipe">是否启用管道重定向输入输出，包括输入/输出和错误的重定向</param>
        /// <param name="IsSyncInput">是否启用异步输入</param>
        public ProcessRunner(string exePath = "cmd.exe", bool IsUseShellExecute = false, bool IsRedirectPipe = true, bool IsSyncInput = true)
        {
            //初始化进程
            this.Process = new Process();              //实例化独立进程
            this.Process.EnableRaisingEvents = true;   //事件监听退出 
            this.StartInfo =  this.Process.StartInfo;        //---绑定初始化信息        

            //事件
            this.Process.Exited += process_Exited;
            this.Process.Disposed += process_Exited;
            this.Process.OutputDataReceived += process_OutputDataReceived;
            this.Process.ErrorDataReceived += process_ErrorDataReceived;

            SetExePath(exePath);
            SetIsUseShellExecute(IsUseShellExecute);
            SetIsRedirectPipe(IsRedirectPipe);
            SetIsSyncInput(IsSyncInput);

            Init();
        }

        /// <summary>
        ///  初始化。调用了设置后必须设置。
        /// </summary>
        public ProcessRunner Init()
        { 
            //初始化进程启动的信息
            StartInfo.FileName  = ExePath;
            this.StartInfo.UseShellExecute = IsUseShellExecute;     //是否关闭Shell的使用

            //决定采用CMD输入输出或是采用程序输入输出
            this.StartInfo.RedirectStandardInput = IsRedirectPipe;   //重定向标准输入
            this.StartInfo.RedirectStandardOutput = IsRedirectPipe;  //重定向标准输出
            this.StartInfo.RedirectStandardError = IsRedirectPipe;   // 把外部程序错误输出写到StandardError流中 LV ADDS

            //显示窗口以方便调试
            this.StartInfo.CreateNoWindow = !IsDebug;          //设置显示窗口   
    
            return this;
        }

        #region 核心属性参数和Smart方法

        public string ExePath ;
        public bool IsUseShellExecute = false;
        public bool IsRedirectPipe = true;
        public bool IsSyncInput = true;

        public ProcessRunner SetExePath(string exePath = "cmd.exe") { this.ExePath = exePath; return this; }
        public ProcessRunner SetIsUseShellExecute(bool IsUseShellExecute = false) { this.IsUseShellExecute = IsUseShellExecute; return this; }
        public ProcessRunner SetIsRedirectPipe(bool IsRedirectPipe = false) { this.IsRedirectPipe = IsRedirectPipe; return this; }
        public ProcessRunner SetIsSyncInput(bool IsSyncInput = false) { this.IsSyncInput = IsSyncInput; return this; }       

        #endregion


        #region 核心进程
        public ProcessStartInfo StartInfo { get; set; }
        /// <summary>
        /// 当前进程
        /// </summary>
        public Process Process { get; set; }
        /// <summary>
        /// 指示进程是否已经启动。仅此而已。
        /// </summary>
        public bool Started { get; protected set; }
        /// <summary>
        /// 是否已经退出
        /// </summary>
        public bool HasExited { get { return this.Started && this.Process.HasExited; } }
        #endregion

        #region 数据流、输入输出
        /// <summary>
        /// 输入流写入器。相当于写到控制台。
        /// </summary>
        public StreamWriter StreamWriter { get { return Process.StandardInput; } }
        /// <summary>
        /// 数据流读取器，从程序的控制台读取数据。
        /// </summary>
        public StreamReader StreamReader { get { return Process.StandardOutput; } }
        /// <summary>
        /// 异常输出流。
        /// </summary>
        public StreamReader StreamErrorReader { get { return Process.StandardError; } }
        #endregion

        #region 事件方法响应函数
        /// <summary>
        /// 程序已退出。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void process_Exited(object sender, EventArgs e) { if (ExitedOrDisposed != null) ExitedOrDisposed(sender, e); }
        /// <summary>
        /// 接收内部程序数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void process_OutputDataReceived(object sender, DataReceivedEventArgs e) { if (OutputDataReceived != null) OutputDataReceived(sender, e); }
        protected void process_ErrorDataReceived(object sender, DataReceivedEventArgs e) { if (ErrorDataReceived != null) ErrorDataReceived(sender, e); }

        #endregion

        /// <summary>
        /// 启动程序,并开始异步读取输出
        /// </summary>
        /// <param name="Arguments">启动程序时同一行输入的参数</param>
        public void Start(string Arguments = "")
        {
            this.StartInfo.Arguments = Arguments;    //启动程序时同一行输入的参数

            bool stated = this.Process.Start();     //启动 
            this.Started = true;

            //开始异步读取输出
            if (!this.IsSyncInput && this.StartInfo.RedirectStandardInput) { this.Process.BeginOutputReadLine(); }
            if (!this.IsSyncInput && this.StartInfo.RedirectStandardError) { this.Process.BeginErrorReadLine(); }//如果重定向后不读取，则可能发生阻塞            
        }

        /// <summary>
        /// 同步运行。运行后就退出。
        /// </summary>
        /// <param name="cmd">待执行命令</param>
        /// <returns></returns>
        public List<string> Run(string cmd)
        {
           List<string> results = new List<string>();
            string result = "异步无返回";
            this.Start(cmd);//启动线程 

            if (this.IsSyncInput)
            {
                this.WriteExist();
                //平行才并行
                Task<string> taskresult = Task<string>.Factory.StartNew(GetOutputText);
                Task<string> taskGetError = Task<string>.Factory.StartNew(GetErrorsText);

                results.Add(taskresult.Result);
                results.Add(taskGetError.Result);

                //等待线程退出
                this.Process.WaitForExit();
            }
            else
            {
                results.Add(result);
            }

            return results;
        }
        
        public void RunNoReturn(string cmd)  {  Run(cmd);    }

        /// <summary>
        /// 获取标准输出的文本。需要同步且重定向才能读取。
        /// </summary>
        /// <returns></returns>
        public string GetOutputText()   {    return  StreamReader.ReadToEnd(); }
        /// <summary>
        /// 获取出错文本.需要同步且重定向才能读取。
        /// </summary>
        /// <returns></returns>
        public string GetErrorsText()   { return StreamErrorReader.ReadToEnd();  }

        #region 写入命令
        /// <summary>
        /// 写入一条命令。
        /// </summary>
        /// <param name="cmd"></param>
        public void WriteCommand(string cmd)
        {
            if (this.StartInfo.RedirectStandardInput)  { this.StreamWriter.WriteLine(cmd); this.StreamWriter.Flush(); }
            else { throw new Exception("请确保重定向 RedirectStandardInput 启用再试，否则只能通过Shell界面输入。"); }
        }
        
        #region 常用命令
        /// <summary>
        /// 向管道写入 exit 命令。
        /// </summary>
        public void WriteExist() { WriteCommand("exit"); }
        /// <summary>
        /// 向管道写入 y 命令。
        /// </summary>
        public void WriteY() { WriteCommand("y"); }
        /// <summary>
        /// 向管道写入 n 命令。
        /// </summary>
        public void WriteN() { WriteCommand("n"); }
        /// <summary>
        /// 向管道写入 shutdown 命令。
        /// </summary>
        public void WriteShutdown() { WriteCommand("shutdown"); }
        /// <summary>
        /// 向管道写入 start 命令。
        /// </summary>
        public void WriteStart() { WriteCommand("start"); }
        #endregion
        #endregion

        #region 异步运行

        /// <summary>
        /// 异步运行，不用等待。需要考虑：如何清理进程。
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public IAsyncResult RunAsyn(string cmd)
        {
            CmdHandler handler = RunNoReturn;
            IAsyncResult result = handler.BeginInvoke(cmd, AsyncProcessExited, null );
            return result;
        }

        #endregion

        /// <summary>
        /// 直接杀死进程。
        /// </summary>
        public void KillProcess() { 
            if (!this.Process.HasExited) 
            { this.Process.Kill(); }  
        }

        /// <summary>
        /// 直接释放进程资源
        /// </summary>
        public void Dispose() { if (Process != null) { KillProcess(); Process.Dispose(); } }

        #region 工具类
        /// <summary>
        /// 同步运行命令行。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string RunCmd(string cmd)
        {
            using (ProcessRunner c = new ProcessRunner())
            {
                return c.Run(cmd)[0];
            }
        }
        /// <summary>
        /// 异步运行命令行。
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCmdAsyn(string cmd)
        {
            new ProcessRunner().RunAsyn(cmd);
        }
        #endregion
    }
}
