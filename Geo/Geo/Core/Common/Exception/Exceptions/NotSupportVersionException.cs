//2016.01.20，czs double wyp，create in hongqing 火大 招待所, 版本不支持错误

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 版本不支持错误。
    /// </summary>
    public class NotSupportVersionException: GeoException
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public NotSupportVersionException() { this.ErrorMessage = "对不起，不支持当前版本，请于开发人员联系，gnsser@163.com"; }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public NotSupportVersionException(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// 以错误信息初始化
        /// </summary>
        /// <param name="Message">错误字符串</param>
        public NotSupportVersionException(string Message)
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
