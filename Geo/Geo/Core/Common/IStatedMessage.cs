//2015.09.29, czs, create in xi'an hongqing, 具有状态的信息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 数据处理状态
    /// </summary>
    public enum ProcessState { 
        /// <summary>
        /// 数据处理失败
        /// </summary>
        Failed = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Sucessed = 1, 
        /// <summary>
        /// 处理通过，但是带有警告
        /// </summary>
        Warning = 2,
        /// <summary>
        /// 正在处理
        /// </summary>
        Processing = 3,
        Processed = 4
    }

    /// <summary>
    /// 处理过程中的信息
    /// </summary>
    /// <param name="StatedMessage"></param>
    public delegate void StatedMessageProducedEventHandler(StatedMessage StatedMessage);

    /// <summary>
    /// 对象信息，如果对象停用了，一般给出其原因。这样便于调试。
    /// </summary>
    public interface IStatedMessage : IMessage {
        /// <summary>
        /// 处理状态
        /// </summary>
        ProcessState ProcessState { get; set; }
    }

    /// <summary>
    /// 对象信息，如果对象停用了，一般给出其原因。
    /// </summary>
    public class StatedMessage : IStatedMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatedMessage() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ProcessState"></param>
        /// <param name="msg"></param>
        public StatedMessage(ProcessState ProcessState, string msg) { this.ProcessState = ProcessState; this.Message = msg; }

        /// <summary>
        /// 对象信息，如果对象停用了，一般给出其原因。
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 是否可用，是否启用。
        /// </summary>
        public ProcessState ProcessState { get; set; }

        /// <summary>
        /// 解析字符串。 如 1OK。首字符表示成功或失败。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static StatedMessage Parse(string str)
        {
            ProcessState enalbed = (ProcessState) str[0];
            return new StatedMessage{ ProcessState = enalbed, Message = str.Substring(1)};
        }

        #region  override
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int bit = (int)ProcessState;
            return bit + Message;
        }
        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var o = obj as StatedMessage;
            if (o == null) { return false; } 
            return o.ProcessState == ProcessState && o.Message == Message;
        }
        /// <summary>
        /// 哈希数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ProcessState.GetHashCode() * 5 + Message.GetHashCode() * 13;
        }
        #endregion
        /// <summary>
        /// 失败实例
        /// </summary>
        public static StatedMessage Faild { get { return new StatedMessage { ProcessState = ProcessState.Failed, Message = "Failed" }; } }
        /// <summary>
        /// 消息为 OK，且 Enalbed = true.
        /// </summary>
        public static StatedMessage Ok { get { return new StatedMessage { ProcessState = ProcessState.Sucessed, Message = "OK" }; } }
        public static StatedMessage Warning { get { return new StatedMessage { ProcessState = ProcessState.Warning, Message = "Warning" }; } }
        public static StatedMessage Processing { get { return new StatedMessage { ProcessState = ProcessState.Processing, Message = "Processing" }; } }

        public static StatedMessage GetProcessing(string msg) { return new StatedMessage { ProcessState = ProcessState.Processing, Message = msg }; }


        public static StatedMessage GetProcessed(string msg) { return new StatedMessage { ProcessState = ProcessState.Processed, Message = msg }; }
        public static StatedMessage GetFailed(string msg) { return new StatedMessage { ProcessState = ProcessState.Failed, Message = msg }; }
 
    }
}
