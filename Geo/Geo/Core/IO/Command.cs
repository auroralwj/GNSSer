 //2014.05.21,czs, add logs
//2016.11.23, czs & cuiyang,  发现只有这个才支持 TEQC ，恢复之

using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Diagnostics;//加入，使用进程类，创建独立进程

namespace Geo.Common
{
    /// <summary>
    /// CMD运行类。
    /// </summary>
    public class Command : IDisposable
    {
        delegate string CmdHandler(string cmd);
        /// <summary>
        /// 程序退出事件
        /// </summary>
        public event EventHandler ProcessExited;
        /// <summary>
        /// 进程
        /// </summary>
        protected Process Process { get { return process; } }

        private  Process process;
       
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Command()
        {
            process = new Process();//启动独立进程
            process.StartInfo.FileName = "cmd.exe";           //设定程序名

            process.StartInfo.UseShellExecute = false;        //关闭Shell的使用
            process.StartInfo.RedirectStandardInput = true;   //重定向标准输入
            process.StartInfo.RedirectStandardOutput = true;  //重定向标准输出
            process.StartInfo.RedirectStandardError = true;   //重定向错误输出
            process.StartInfo.CreateNoWindow = true;          //设置显示窗口

            process.Exited += process_Exited;
        } 

        /// <summary>
        /// 程序已退出。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void process_Exited(object sender, EventArgs e)
        {
            if (ProcessExited != null) ProcessExited(sender, e);
        }

        /// <summary>
        /// 同步运行
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string Run(string cmd)
        {
            string result;
            try
            {
                //p.StartInfo.Arguments = "/c " + cmd;    //設定程式執行參數
                process.Start();   //啟動  sw.Close(); 
                StreamWriter sw = process.StandardInput;
                sw.WriteLine(cmd);       //也可以用這種方式輸入要執行的命令
                sw.WriteLine("exit");        //不過要記得加上Exit要不然下一行程式執行的時候會當機
                sw.Close();

                result = process.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果

                process.WaitForExit();

                //throw new InvocationException() ;
            }
            catch (Exception ex)
            {
                result = "ERROR:" + ex.Message;

               // ExceptionHandlerFactory.Create(typeof(InvocationException))
                //    .Handle(new InvocationException(ex) { InvocationPath = cmd });
                //
                //重新抛出
                throw;
            }
            finally
            {
                if (process.HasExited)
                    process.Close();
            }
            return result;
        }


        /// <summary>
        /// 异步运行，不用等待。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public void RunAsyn(string cmd)
        {
            CmdHandler handler = Run;
            handler.BeginInvoke(cmd,
                null,
                null);
        }
        /// <summary>
        /// 同步运行命令行。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string RunCmd(string cmd)
        {
            using (Command c = new Command())
            {
                return c.Run(cmd);
            }
        }
        /// <summary>
        /// 异步运行命令行。
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCmdAsyn(string cmd)
        {
            new Command().RunAsyn(cmd);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
           // Dispose(true);

            if (process != null)
                process.Dispose();
        }
    }
}
