//2014.12.06, czs, create in jinxingliaomao shaungliao, 异常信息

using System;
using System.Collections.Generic;
using System.Text;
using Geo;

namespace Geo.Utils
{
    /// <summary>
    /// 用于异常检查，直接抛出异常。
    /// </summary>
    public static class ExceptionUtil
    {
        /// <summary>
        /// 尝试执行方法。
        /// </summary>
        /// <param name="action">待执行方法</param>
        /// <param name="exceptionInfo">异常信息</param>
        public static void TryInvoke(Action action, ExceptionInfo exceptionInfo = null)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (exceptionInfo != null)
                {
                    string msg =  ex.Message;
                    exceptionInfo.Message += msg;
                    if (!exceptionInfo.IgnoreException) throw ex;
                }
            }
        }
    }

    /// <summary>
    /// 异常信息
    /// </summary>
    public class ExceptionInfo
    {
        public ExceptionInfo(bool IgnoreException)
        {
            this.IgnoreException = IgnoreException;
        }
        /// <summary>
        /// 是否忽略异常
        /// </summary>
        public bool IgnoreException;

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Message = "";

        public override string ToString()
        {
            return Message;
        }
    }
}
