using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Interoperation.Teqc
{
    /// <summary>
    /// TEQC 功能调用者，封装了 TEQC 功能的存在。
    /// 主要有以下使用方法，对应构造函数：
    /// 1.先指定功能，再指定执行文件；
    /// 2.先指定文件，然后运行不同的功能。
    /// 3.还可以只指定teqc程序路径，自己编写命令行参数。
    /// </summary>
    public class TeqcFunctionCaller
    {
        /// <summary>
        /// 只指定teqc程序路径
        /// </summary>
        /// <param name="teqcPath">teqc程序路径</param>
        public TeqcFunctionCaller(string teqcPath)
        {
            this.exe = new ExeRunner(teqcPath);
        }

        /// <summary>
        /// 先指定文件，然后运行不同的功能。
        /// </summary>
        /// <param name="teqcPath"></param>
        /// <param name="files"></param>
        public TeqcFunctionCaller(string teqcPath, params string[] files)
            :this(teqcPath)
        { 
            this.files = files;
        }

        /// <summary>
        /// 先指定功能，再指定执行文件
        /// </summary>
        /// <param name="teqcPath"></param>
        /// <param name="func"></param>
        public TeqcFunctionCaller(string teqcPath, TeqcFunction func)
            : this(teqcPath)
        { 
            this.func = func;
        }

        private string[] files = null;
        private ExeRunner exe;
        private TeqcFunction func = TeqcFunction.None;

        /// <summary>
        /// 运行功能。
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public List<string> Run(TeqcFunction func) { return Run(func, files); }
        /// <summary>
        /// 请先设置Function。
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<string> Run(params string[] files) { return Run(func, files); }
        /// <summary>
        /// 指定功能，文件，并运行teqc。
        /// </summary>
        /// <param name="func"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<string> Run(TeqcFunction func, params string[] files)
        {
            if (func == TeqcFunction.None) throw new ArgumentNullException(" 请先指定 Teqc 功能  TeqcFunction");
            if (files == null || files.Length == 0) throw new ArgumentNullException(" 没有可以处理的文件！");
            this.func = func;
            TeqcOption opt = new TeqcOption(func, files);

            return Run(opt);
        }

        /// <summary>
        /// 运行命令选项。
        /// 这是比较底层的调用，常常用于测试，推荐采用更高层次的方法.
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        public List<string> Run(TeqcOption opt)
        {
            return exe.Run(opt.ToString());
        }

    }
}
