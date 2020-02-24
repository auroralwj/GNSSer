//2014.10.02，czs，create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 程序调用错误。
    /// </summary>
    public class DimentionException: GeoException
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DimentionException() { this.ErrorMessage = "维数错误。"; }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public DimentionException(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// 以错误信息初始化
        /// </summary>
        /// <param name="Message">错误字符串</param>
        public DimentionException(string Message)
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



    }
}
