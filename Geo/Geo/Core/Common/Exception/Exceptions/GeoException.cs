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
    public class GeoException : ApplicationException
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GeoException() : base("Geo通用错误类型。") { this.ErrorMessage = "Geo通用错误类型。"; }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public GeoException(Exception exception)
            : base(exception.Message) 
        {
            this.Exception = exception;
        }

        /// <summary>
        /// 以错误信息初始化
        /// </summary>
        /// <param name="Message">错误字符串</param>
        public GeoException(string Message)
            : base(Message) 
        {
            this.ErrorMessage = Message;
        }

        /// <summary>
        /// 父类异常。
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 错误信息描述。
        /// </summary>
        public override string Message { get { return ErrorMessage; } }

        /// <summary>
        /// 错误信息描述。
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
