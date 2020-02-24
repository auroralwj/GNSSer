using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Exceptions
{
    /// <summary>
    /// 异常处理接口
    /// </summary>
    public interface IExceptionHandler
    {       
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="exception"></param>
        void Handle(Exception exception);
    }
}
