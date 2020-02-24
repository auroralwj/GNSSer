//2015.01.09，czs，create in namu, 不应该出现的错误

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Common;

namespace Geo
{
    /// <summary>
    /// 程序调用错误。
    /// </summary>
    public class ShouldNotHappenException: GeoException
    {

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public ShouldNotHappenException(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// 以错误信息初始化
        /// </summary>
        /// <param name="Message">错误字符串</param>
        public ShouldNotHappenException(string Message)
            : base(Message)
        {
            this.ErrorMessage = Message;
        } 

        /// <summary>
        /// 错误信息描述。
        /// </summary>
        public override string Message { get { return ErrorMessage; } }
         

    }
}
