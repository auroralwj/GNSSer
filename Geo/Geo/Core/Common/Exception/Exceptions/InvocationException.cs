//2014.05.22，czs，create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 程序调用错误。
    /// </summary>
    public class InvocationException : ApplicationException
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
         public InvocationException(){}

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public InvocationException(Exception exception)
        {
            this.exception = exception;
        }
        /// <summary>
        /// 父类异常。
        /// </summary>
        protected Exception exception;
       
        /// <summary>
        /// 程序调用路径。
        /// </summary>
        public string InvocationPath { get; set; }
    }
}
