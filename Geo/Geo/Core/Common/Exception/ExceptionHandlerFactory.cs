//2014.05.22, czs, created, 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Exceptions
{
    /// <summary>
    /// 异常处理工程。采用简单工厂方法模式创建。
    /// </summary>
    public static class ExceptionHandlerFactory
    {
        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <param name="excetpion">异常实例</param>
        public static IExceptionHandler Create(Exception excetpion)
        {
            if (excetpion is InvocationException)
                return new InvocationExceptionHandler();

            return new ExceptionHandler();
        }
        /// <summary>
        /// 工工厂方法
        /// </summary>
        /// <param name="excetpionType">异常类型</param>
        /// <returns></returns>
        public static IExceptionHandler Create(Type excetpionType)
        {
            if (excetpionType.Equals( typeof( InvocationException)))
                return new InvocationExceptionHandler();

            return new ExceptionHandler();
        }
    }
}
