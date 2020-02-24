//2014.05.22，czs，create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Excepts
{
    /// <summary>
    /// Gnsser 顶层异常。
    /// </summary>
    public class SynchronizeException: GnsserException 
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
         public SynchronizeException(){}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
         public SynchronizeException(string error) : base(error) { }


        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
         public SynchronizeException(Exception exception):base(exception)
        { 
        } 

         


    }
}
