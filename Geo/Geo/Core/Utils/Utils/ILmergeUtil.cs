using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Geo.Utils
{
    //public class ILmergeUtil
    //{
    //    public static string RunMergeExeCmd(string DllDirectory, string SourceExeFile, string TargetExeFile)
    //    {
    //        Geo.Common.Command 
    //    }

    //    public static string RunMergeDllCmd(string DllDirectory, string TargetDllFile)
    //    { 
        
    //    }
    //}
        /// <summary>  
        /// 使用ILmerge合并Exe、Dll文件的帮助类  
        /// </summary>  
        public class ILmerge
        {
            /// <summary>  
            /// 得到合并Exe、Dll文件的ILmerge语句  
            /// </summary>  
            /// <param name="DllDirectory">Dll文件目录</param>  
            /// <param name="SourceExeFile">原exe文件全路径</param>  
            /// <param name="TargetExeFile">要生成的exe文件全路径</param>  
            /// <returns></returns>  
            public static string GetMergeExeCmd(string DllDirectory, string SourceExeFile, string TargetExeFile)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ILmerge /ndebug /target:winexe /targetplatform:v4  /out:");
                sb.Append(TargetExeFile);
                sb.Append(" /log ");
                sb.Append(SourceExeFile);
                string[] dllFiles = Directory.GetFiles(DllDirectory, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    sb.Append(" " + dllFile);
                }
                return sb.ToString();
            }
            /// <summary>  
            /// 得到合并Dll文件的ILmerge语句  
            /// </summary>  
            /// <param name="DllDirectory">Dll文件目录</param>  
            /// <param name="TargetDllFile">要生成的Dll文件全路径</param>  
            /// <returns></returns>  
            public static string GetMergeDllCmd(string DllDirectory, string TargetDllFile)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ILmerge /ndebug /targetplatform:v4  /target:dll /out:");
                sb.Append(TargetDllFile);
                sb.Append(" /log ");
                string[] dllFiles = Directory.GetFiles(DllDirectory, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    sb.Append(" " + dllFile);
                }
                return sb.ToString();
            }


        }

}
