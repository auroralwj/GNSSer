//2015.01.16, czs, create in namu, 提取 EXE文件执行器接口。

using System;
using Geo.Common;
using System.Collections.Generic;

namespace Gnsser.Interoperation
{
    /// <summary>
    /// EXE文件执行器接口
    /// </summary>
    public interface IExeRunner : IDisposable
    {
        /// <summary>
        /// 可执行程序的路径
        /// </summary>
        string ExePath { get; set; }
        /// <summary>
        /// 通过命令行调用程序
        /// </summary>
        /// <param name="options">调用选项</param>
        /// <returns>返回命令行结果</returns>
        List< string> Run(string options);

       /// <summary>
        /// 通过命令行调用程序
        /// </summary>
        /// <param name="options">调用选项</param>
        /// <returns></returns>
        List<string> Run(IExeOption options);
    } 
}
