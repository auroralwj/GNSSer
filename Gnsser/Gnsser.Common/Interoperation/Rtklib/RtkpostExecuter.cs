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
    /// RTKLib rtkpost 程序执行者。
    /// </summary>
    public class RtkpostExecuter : ExeRunner
    {
        /// <summary>
        /// RTKLib  程序执行者。
        /// </summary>
        /// <param name="exePath">路径</param>
        public RtkpostExecuter(string exePath)
            : base(exePath)
        {
        }

        protected override ProcessRunner BuildProcessRunner()
        {
            ProcessRunner ProcessRunner = new ProcessRunner(ExePath, false, true, true);
            return ProcessRunner;
        }
         
        /// <summary>
        /// 停止执行。
        /// </summary>
        public override void Stop()
        {
            this.ProcessRunner.WriteExist();

            Thread.Sleep(100);

            base.Stop();
        }

    }  
}
