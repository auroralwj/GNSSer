//2015.02.27, czs, create in namu,0多 EXE文件执行器。

using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Geo.Common;


namespace Gnsser.Interoperation
{
    /// <summary>
    /// 可以同时管理一个Exe执行文件，的多个线程。
    /// 命令行下的EXE文件执行，对CMD命令进行了封装,即：让cmd启动exe程序。
    /// 一次只能管理一个程序.
    /// 注意:
    /// 1.本类一个对象对应于1个运行的Command，对应于一个Process，当该线程释放或杀死后，则重新启用一个Conmand对象。
    /// 2.将可执行程序和参数分开设置，但是在运行时，一起写入命令行CMD。
    /// </summary>
    public class MultiExeRunner 
    {
        /// <summary>
        /// 文件执行器构造函数。采用参数进行初始化。
        /// </summary>
        /// <param name="ExePath">可执行程序的路径</param>
        public MultiExeRunner(string ExePath)
        {
            if (!File.Exists(ExePath)) throw new FileNotFoundException(ExePath);

            this.ExePath = ExePath;

            this.Commands = new List<ProcessRunner>();
        }
  
        /// <summary>
        /// 命令列表。
        /// </summary>
        private List<ProcessRunner> Commands { get; set; }
        /// <summary>
        /// CMD运行类。
        /// </summary>
        public  ProcessRunner CurrentCommand { get; set; }
        
        /// <summary>
        /// 可执行程序的路径
        /// </summary>
        public string ExePath { get; set; }
        
        
        #region 运行方法
        
        #endregion
      
    }
}