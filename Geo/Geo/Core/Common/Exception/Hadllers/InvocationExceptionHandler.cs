//2014.05.20, czs, created, 错误处理基类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Exceptions
{

    /// <summary>
    /// 错误处理
    /// </summary>
    public class InvocationExceptionHandler : ExceptionHandler
    {

        /// <summary>
        /// 递交错误处理。
        /// </summary> 
        public override void Handle(Exception exception)
        {
            InvocationException ex = exception as InvocationException;
            if (ex == null) throw new ArgumentException("类型不对，请传入 InvocationException");

            string msg = "程序调用出错,路径为"+ ex.InvocationPath;
            Log.Error(msg, ex);
        }  
    }
}
