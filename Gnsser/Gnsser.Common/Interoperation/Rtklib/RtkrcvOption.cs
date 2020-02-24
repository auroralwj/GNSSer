//2015.02.05, czs, create in pengzhou, Rtkrcv 的参数选项

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Interoperation
{
    /// <summary>
    /// Rtkrcv 的参数选项。
    /// 输入指定的功能，按照指定设计生成命令参数。
    /// </summary>
    public class RtkrcvOption : CommandLines, IStartArguments
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RtkrcvOption()
        {
        }
        /// <summary>
        /// 配置文件。
        /// </summary>
        public string ConfigFilePath { get; set; }

        /// <summary>
        /// 一个命令占用一行。逐行输入。
        /// </summary>
        public override List<string> Commands
        {
            get
            {
                List<string> optionLines = new List<string>();
                var fullPath = Path.GetFullPath(ConfigFilePath);
                optionLines.Add("load " + fullPath);
                optionLines.Add("start");
                optionLines.Add("y");
                return optionLines;
            }
            set { throw new NotSupportedException("不可设置！"); }
        }

        /// <summary>
        /// 退出的命令行
        /// </summary>
        public CommandLines ExitCommands
        {
            get
            {
                CommandLines lines = new CommandLines();

                List<string> optionLines = new List<string>(); 
                optionLines.Add("shutdown ");
                optionLines.Add("y");
                optionLines.Add("exit");

                lines.Commands = optionLines;
                return lines;
            } 
        }


        /// <summary>
        /// 字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Arguments;
        }

        /// <summary>
        /// 参数,与程序路径一起传入。
        /// </summary>
        public string Arguments
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(" -s -o ");

                var fullPath = Path.GetFullPath(ConfigFilePath);
                sb.Append(fullPath); 

                var cmd = sb.ToString();
                return cmd;
            }
        }
    }
}
