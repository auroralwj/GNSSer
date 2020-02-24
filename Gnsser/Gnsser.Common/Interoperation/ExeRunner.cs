//2015.01.16, czs, create in namu, EXE文件执行器。

using System;
using System.Threading;
using System.IO;
using Geo.Common;
using System.Collections.Generic;
using System.Diagnostics;//加入，使用进程类，创建独立进程
using Microsoft.CSharp;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Gnsser.Interoperation
{
    /// <summary>
    /// 命令行下的EXE文件执行器，对CMD命令进行了封装,即：让cmd启动一个exe程序。
    /// 一次只能管理一个程序.
    /// 注意:
    /// 1.本类一个对象对应于1个运行的Command，对应于一个Process，当该线程释放或杀死后，则重新启用一个Conmand对象。
    /// 故Command对象退出或释放后，本对象只是停止Stoped。Start时，则重新构建一个对象。
    /// 2.将可执行程序和参数分开设置，但是在运行时，一起写入命令行CMD。
    /// </summary>
    public class ExeRunner : IExeRunner
    {
        #region 事件，此处再定义一次是因为，本类与 ProcessRunner 为1对多的关系
        /// <summary>
        /// 停止事件
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
        #endregion

        public ExeRunner() { }
        /// <summary>
        /// 文件执行器构造函数。采用参数进行初始化。
        /// </summary>
        /// <param name="ExePath">可执行程序的路径</param>
        public ExeRunner(string ExePath)
        {
            if (!File.Exists(ExePath)) throw new FileNotFoundException(ExePath);

            this.ExePath = ExePath;
        }


        /// <summary>
        /// 建立一个 ProcessRunner 对象。在每一次任务运行前必须有一个。
        /// </summary>
        protected virtual ProcessRunner BuildProcessRunner()
        {
            ProcessRunner ProcessRunner = new ProcessRunner(ExePath);
            return ProcessRunner;
        }

        /// <summary>
        /// 程序执行完毕。无论是同步或是异步。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Command_ExitedOrDisposed(object sender, EventArgs e)
        {
            this.IsRunning = false;
            if (ExitedOrDisposed != null) { ExitedOrDisposed(sender, e); }
        }

        /// <summary>
        /// CMD运行类。
        /// </summary>
        public ProcessRunner ProcessRunner { get; set; }

        /// <summary>
        /// 可执行程序的路径
        /// </summary>
        public string ExePath { get; set; }
        /// <summary>
        /// 是否正在运行。
        /// 在异步执行时，虽然调用主线程已经退出，但是异步线程仍然在执行，因此认为是在执行。
        /// </summary>
        public bool IsRunning { get { return isRunning; }
            protected set { isRunning = value;
                if (!isRunning) 
                { }
            }
        }
        bool isRunning = false;//用于调试
        #region 运行方法
        /// <summary>
        /// 通过命令行调用程序
        /// </summary>
        /// <param name="options">调用选项</param>
        /// <returns>返回命令行结果</returns>
        public virtual List<string> Run(string options)
        {
            if (IsRunning) { throw new Exception("程序正在运行，请停止后再调用。"); }

            this.CheckOrCreateProcessRunner();

            this.IsRunning = true;//放在此处，防止已启动就停止
            var result = this.ProcessRunner.Run(options);

            return result;
        }
        /// <summary>
        /// 检查，如果线程退出了则重新开启一个。
        /// </summary>
        protected void CheckOrCreateProcessRunner()
        {
            if (this.ProcessRunner == null //如果没有，则需要建立一个
                || (this.ProcessRunner != null && this.ProcessRunner.HasExited)) //如果已经退出了则需要建立一个
            {
                this.IsRunning = false;
                this.IsDisposed = true;

                this.ProcessRunner = BuildProcessRunner();
                //绑定事件
                this.ProcessRunner.ExitedOrDisposed += Command_ExitedOrDisposed;
                this.ProcessRunner.ErrorDataReceived += Process_ErrorDataReceived;
                this.ProcessRunner.OutputDataReceived += Process_OutputDataReceived;
            }
        }

        void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (OutputDataReceived != null) OutputDataReceived(sender, e);
        }

        void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (ErrorDataReceived != null) ErrorDataReceived(sender, e);
        }

        /// <summary>
        /// 通过命令行调用程序
        /// </summary>
        /// <param name="options">调用选项</param>
        /// <returns></returns>
        public List<string> Run(IExeOption options)
        {
            return Run(options.ToString());
        }
        /// <summary>
        /// 这个没有什么用处，只是用来测试的。
        /// </summary>
        public IAsyncResult AsyncResult { get; set; }
        /// <summary>
        /// 异步运行
        /// </summary>
        /// <param name="options">参数选项</param>
        public IAsyncResult RunAsyn(IExeOption options)
        {
            return RunAsyn(options.ToString());
        }
        /// <summary>
        /// 异步运行
        /// </summary>
        /// <param name="options">参数选项</param>
        public IAsyncResult RunAsyn(string options)
        {
            if (!System.IO.File.Exists(ExePath)) throw new FileNotFoundException(ExePath);

            this.CheckOrCreateProcessRunner();
            var result = this.ProcessRunner.RunAsyn(options);
            this.IsRunning = true;
            this.AsyncResult = result;
            return result;
        }
        #endregion

        /// <summary>
        /// 写Cmd退出命令，如果程序还在运行，则通过杀死进程的方式停止运行。
        /// 释放资源。
        /// </summary>
        public virtual void Stop()
        {
            if (IsRunning && this.ProcessRunner.IsRedirectPipe)
            {
                this.ProcessRunner.WriteExist();
                Thread.Sleep(100);
                this.ProcessRunner.WriteExist();
            }

            System.Threading.Thread.Sleep(10);
            if ( AsyncResult != null &&  !AsyncResult.IsCompleted)
            {

            }

            //简单粗暴
            this.ProcessRunner.KillProcess();
            this.IsRunning = false;
        }

        /// <summary>
        /// 是否抛弃了资源。
        /// </summary>
        public bool IsDisposed { get; set; }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Stop();
            }
            this.IsDisposed = true;
        }
    }
}