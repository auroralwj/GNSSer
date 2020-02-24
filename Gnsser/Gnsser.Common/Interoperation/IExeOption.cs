//2015.01.16, czs, create in namu, 可执行程序的配置选项

using System;
using System.Collections.Generic;

namespace Gnsser.Interoperation
{
    /// <summary>
    /// 可执行程序的配置选项
    /// </summary>
    public interface IExeOption
    {  
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        string ToString();
    }

    /// <summary>
    /// 在程序内部界面输入选项，可执行程序的配置选项
    /// </summary>
    public interface ICommandLines
    {
        /// <summary>
        /// 每一行的命令，按照顺序执行。
        /// </summary>
        List<String> Commands { get; }
    }

    /// <summary>
    /// 启动命令，在路径之后
    /// </summary>
    public interface IStartArguments
    {
        string Arguments { get; }
    }



    /// <summary>
    /// 在程序内部界面输入选项，可执行程序的配置选项
    /// </summary>
    public class CommandLines : ICommandLines
    {
        /// <summary>
        /// 选项
        /// </summary>
        public virtual List<string> Commands { get; set; }
    }

    /// <summary>
    /// 在一行中录入可执行程序的配置选项
    /// </summary>
    public interface IOuterExeOption : IExeOption
    {  
    }
}
