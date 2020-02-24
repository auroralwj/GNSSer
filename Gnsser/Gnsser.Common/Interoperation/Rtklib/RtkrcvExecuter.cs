//2016.01.16, czs, create in namu, RTKLib 解算结果

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Geo.Common;

namespace Gnsser.Interoperation
{
   
    /// <summary>
    /// 实时计算调用类。
    /// </summary>
    public class RtkrcvExecuter : RtkExecuter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
         public  RtkrcvExecuter(){}

              /// <summary>
        /// RTKLib  程序执行者。
        /// </summary>
        /// <param name="exePath">路径</param>
        public RtkrcvExecuter(string exePath)
            : base(exePath)
        {
            this.IsRedirectPipe = false;
        }

        /// <summary>
        /// 是否采用管道命令。
        /// </summary>
        public bool IsRedirectPipe { get; set; }

        protected override ProcessRunner BuildProcessRunner()
        {
            //如果启用管道，则不用shell。
            bool IsUseShellExecute = !IsRedirectPipe; //有时候，重定向会出错误，则采用shell启动，2015.07版本 Rtkrcv.exe
            return new ProcessRunner(this.ExePath, IsUseShellExecute, IsRedirectPipe, IsSyncInput: false);
        }

        public List<string> Run(RtkrcvOption option)
        {
            if (IsRunning) { throw new Exception("程序正在运行，请停止后再调用。"); }

            List<string> results = null;

            if (IsRedirectPipe)//使用管道，参数随后发送
            {
                results = Run("");//启动内部界面
                foreach (var item in option.Commands)
                {
                    Thread.Sleep(200);
                    this.ProcessRunner.WriteCommand(item);
                }
            }
            else//不使用管道，参数与路径一起发送。
            {
              this.CheckOrCreateProcessRunner();
              this.IsRunning = true;
              results = this.ProcessRunner.Run(option.Arguments);
            }

            return results;
        }

        /// <summary>
        /// 停止运行。
        /// </summary>
        public override void Stop()
        {
            if (this.ProcessRunner != null && this.ProcessRunner.IsRedirectPipe)
            {
                this.ProcessRunner.WriteCommand("shutdown");
                Thread.Sleep(50);
                this.ProcessRunner.WriteCommand("y");
                Thread.Sleep(50);
                this.ProcessRunner.WriteExist();
                Thread.Sleep(50);
            }

            base.Stop();
        }
    }
}
