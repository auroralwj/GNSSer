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
    public class SatCountException : GnsserException
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SatCountException()
        {
            this.ErrorMessage = "卫星数量错误。";
        }
        /// <summary>
        /// 自定义字符串数量。
        /// </summary>
        /// <param name="error"></param>
        public SatCountException(string error) : base(error) { }


        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public SatCountException(Exception exception)
            : base(exception)
        {
        }
    }
}
