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
    /// RTKLib  程序执行者。
    /// </summary>
    public class RtkExecuter : ExeRunner
    { 
        /// <summary>
        /// 构造
        /// </summary>
        public  RtkExecuter(){}
        /// <summary>
        /// RTKLib  程序执行者。
        /// </summary>
        /// <param name="exePath">路径</param>
        public RtkExecuter(string exePath)
            : base(exePath)
        {
        }
    }  
}
