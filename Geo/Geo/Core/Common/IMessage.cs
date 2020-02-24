//2015.01.09, czs, create in namu, 文本信息接口

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo
{
    /// <summary>
    /// 文本信息接口
    /// </summary>
    public interface IMessage
    { 
        /// <summary>
        /// 对象信息
        /// </summary>
        string Message { get; set; }
    }

    /// <summary>
    /// 对象信息，如果对象停用了，一般给出其原因。这样便于调试。
    /// </summary>
    public interface IEnabledMessage : IMessage, IEnabled { }

    /// <summary>
    /// 对象信息，如果对象停用了，一般给出其原因。
    /// </summary>
    public class EnabledMessage : IEnabledMessage, IMessage, IEnabled
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EnabledMessage() { this.Enabled = true; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="msg"></param>
        public EnabledMessage(bool enabled, string msg) { this.Enabled = enabled; this.Message = msg; }

        /// <summary>
        /// 对象信息，如果对象停用了，一般给出其原因。
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 是否可用，是否启用。
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 消息为 OK，且 Enalbed = true.
        /// </summary>
        public static EnabledMessage Ok { get { return new EnabledMessage { Enabled = true, Message = "OK" }; } }
        public static EnabledMessage Bad(string msg) { return new EnabledMessage { Enabled = false, Message = msg }; } 

        /// <summary>
        /// 解析字符串。 如 1OK。首字符表示成功或失败。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static EnabledMessage Parse(string str)
        {
            bool enalbed = str[0] == 1;
            return new EnabledMessage{ Enabled = enalbed, Message = str.Substring(1)};
        }

        #region  override
        public override string ToString()
        {
            int bit = Enabled ? 0 : 1;
            return bit + Message;
        }

        public override bool Equals(object obj)
        {
            var o = obj as EnabledMessage;
            if (o == null) { return false; } 
            return o.Enabled == Enabled && o.Message == Message;
        }

        public override int GetHashCode()
        {
            return Enabled.GetHashCode() * 5 + Message.GetHashCode() * 13;
        }
        #endregion
    }
}
