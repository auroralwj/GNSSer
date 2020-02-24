//2015.01.16, czs, create in namu, Rtklib 的参数选项

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Interoperation
{
    /// <summary>
    /// Rtklib 的参数选项。
    /// 输入指定的功能，按照指定设计生成命令参数。
    /// </summary>
    public class RtkpostOption : IExeOption
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RtkpostOption( )
        {
            this.ObservationPathes = new List<string>();
        }

        //rtkpost bjf10250.12o bjf10250.12n  -o output.txt  -k rtkpost.conf
        /// <summary>
        /// 计算类型，包含实时处理和事后处理。
        /// </summary>
        public RtklibType RtklibType { get; set; }

        /// <summary>
        /// 观测文件列表
        /// </summary>
        public List<string> ObservationPathes { get; set; } 

        /// <summary>
        /// 导航文件路径。
        /// </summary>
        public string NavigationPath { get; set; }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigPath { get; set; }
        public string ClkPath { get; set; }
        public string Sp3Path { get; set; }
         
        /// <summary>
        /// 标准输出文件路径
        /// </summary>
        public string StandardOutputPath { get; set; }

        /// <summary>
        /// 添加流动站数据文件，流动站数据文件总是排在第一位。
        /// </summary>
        /// <param name="path">路径</param>
        public void AddRoverObsFilePath(string path)
        {
            ObservationPathes.Insert(0, path);
        }

        /// <summary>
        /// 添加参考站数据文件
        /// </summary>
        /// <param name="path">路径</param>
        public void AddRefObsFilePath(string path)
        {
            ObservationPathes.Add(path);
        }

        /// <summary>
        /// 是否满足可以计算的最低条件，是否可以计算。
        /// </summary>
        public bool CanSolve
        {
            get
            {
                return ObservationPathes.Count > 0 && NavigationPath != null;
            }
        }

        /// <summary>
        /// 字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //观测文件
            foreach (var obs in ObservationPathes)
            {
                AppendSpace(sb);
                sb.Append(obs);
            }
            if (NavigationPath != null) { sb.Append("  "); sb.Append(NavigationPath); }
            if (ClkPath != null) { sb.Append("  "); sb.Append(ClkPath); }
            if (Sp3Path != null) { sb.Append("  "); sb.Append(Sp3Path); } 

            if (ConfigPath != null) { sb.Append(" -k "); sb.Append(ConfigPath); }
            if (StandardOutputPath != null) { sb.Append(" -o "); sb.Append(StandardOutputPath); }

            var cmd = sb.ToString();
            return cmd;
        }

        private static void AppendSpace(StringBuilder sb)
        {
            sb.Append(" ");
        }
    }


    /// <summary>
    /// 计算类型，包含实时处理和事后处理。
    /// </summary>
    public enum RtklibType
    {
        /// <summary>
        /// 实时导航计算
        /// </summary>
        Navigation,
        /// <summary>
        /// 时候处理
        /// </summary>
        Post
    }
}
