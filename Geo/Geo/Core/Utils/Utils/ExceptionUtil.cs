//2014.12.06, czs, create in jinxingliaomao shaungliao, �쳣��Ϣ

using System;
using System.Collections.Generic;
using System.Text;
using Geo;

namespace Geo.Utils
{
    /// <summary>
    /// �����쳣��飬ֱ���׳��쳣��
    /// </summary>
    public static class ExceptionUtil
    {
        /// <summary>
        /// ����ִ�з�����
        /// </summary>
        /// <param name="action">��ִ�з���</param>
        /// <param name="exceptionInfo">�쳣��Ϣ</param>
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
    /// �쳣��Ϣ
    /// </summary>
    public class ExceptionInfo
    {
        public ExceptionInfo(bool IgnoreException)
        {
            this.IgnoreException = IgnoreException;
        }
        /// <summary>
        /// �Ƿ�����쳣
        /// </summary>
        public bool IgnoreException;

        /// <summary>
        /// �쳣��Ϣ
        /// </summary>
        public string Message = "";

        public override string ToString()
        {
            return Message;
        }
    }
}
