using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;//加入，使用进程类，创建独立进程


namespace Geo.Utils
{
    /// <summary>
    /// 进程工具
    /// </summary>
   public  class ProcessUtil
    {
        /// <summary>
        /// 获得为该进程(程序)分配的内存.
        /// 做一个计时器, 就可以时时查看程序占用系统资源  
        /// </summary>
        /// <returns></returns> 
        public static double GetProcessUsedMemoryMB()
        {
            return Process.GetCurrentProcess().WorkingSet64 / (1024.0 * 1024.0);
        }
       /// <summary>
        /// 获得为该进程(程序)分配的内存.返回人们能读懂的文件大小的字符串。如 “5MB”
       /// </summary>
       /// <returns></returns>
        public static string GetProcessUsedMemoryString()
        {
           long l =  Process.GetCurrentProcess().WorkingSet64;
           return ByteUtil.GetReadableFileSize(l);
        } 

    }
}
