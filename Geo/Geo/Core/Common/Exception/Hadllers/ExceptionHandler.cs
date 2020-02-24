//2014.05.20, czs, created, 错误处理基类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;


namespace Geo.Exceptions
{
    /// <summary>
    /// 默认错误处理基类。
    /// </summary>
    public  class ExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ExceptionHandler() { }        
       
        /// <summary>
        /// 日志记录
        /// </summary>
        protected ILog Log =  IO.Log.GetLog(typeof(ExceptionHandler));

        /// <summary>
        /// 递交错误处理。
        /// </summary>
        /// <param name="exception"></param>
        public virtual void Handle(Exception exception)
        {
            if (exception != null)
            { 
            //todo，子类实现
                Log.Error("接收到了错误", exception);
                throw exception;
            }
        }
    }
}
