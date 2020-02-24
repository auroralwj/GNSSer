//2014.05.22，czs，create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Excepts
{
    /// <summary>
    /// 卫星数量不足。
    /// </summary>
    public class TimeException : GnsserException
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TimeException()
        {
            this.ErrorMessage = "时间错误。"; 
        }
        /// <summary>
        /// 自定义字符串数量。
        /// </summary>
        /// <param name="error"></param>
        public TimeException(string error) : base(error) { }


        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public TimeException(Exception exception)
            : base(exception)
        {
        }
    }
}
