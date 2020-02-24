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
    public class ArgumentExceptionHandler : ExceptionHandler
    {

        /// <summary>
        /// 递交错误处理。
        /// </summary>
        /// <param name="exception"></param>
        public override void Handle(Exception exception)
        {
           
          //  ArgumentNullException
        }  
    }
}
