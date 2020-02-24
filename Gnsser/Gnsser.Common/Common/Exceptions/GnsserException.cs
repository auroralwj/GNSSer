//2014.05.22，czs，create

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;

namespace Gnsser.Excepts
{
    /// <summary>
    /// Gnsser 顶层异常。
    /// </summary>
    public class GnsserException: GeoException 
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public GnsserException() {  }
        /// <summary>
        /// 以信息初始化
        /// </summary>
        /// <param name="message"></param>
        public GnsserException(string message):base(message){}

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="exception">传入异常</param>
        public GnsserException(Exception exception) : base(exception) { }
    }
}
