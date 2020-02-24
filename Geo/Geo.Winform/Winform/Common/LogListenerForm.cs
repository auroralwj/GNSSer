//2016.11.30, czs, create in hongqing, 具有日志监听功能的窗体

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.IO;
using Geo.Utils;

namespace Geo.Winform
{
    /// <summary>
    /// 具有日志监听功能的窗体
    /// </summary>
    public partial class LogListenerForm : Form
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog log = Log.GetLog(typeof(LogListenerForm));
        /// <summary>
        /// 具有日志监听功能的窗体
        /// </summary>
        public LogListenerForm()
        {
            InitializeComponent();
        }
        LogWriter LogWriter = LogWriter.Instance;
        /// <summary>
        /// 是否启用日志显示
        /// </summary>
        public bool IsEnableLogShowing { get; set; }
        private void LogListenerForm_Load(object sender, EventArgs e)
        {
            LogWriter.MsgProduced += LogWriter_MsgProduced;
        }

        /// <summary>
        /// 实现日志读取
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="LogType"></param>
        /// <param name="msgProducer"></param>
        protected virtual void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer)
        {
            if (!IsEnableLogShowing) { return; }

            var info = LogType.ToString() + "\t" + msg;// +"\t" + msgProducer.Name;
            ShowInfo(info);
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="info"></param>
        protected virtual void ShowInfo(string info)
        {
            //throw new NotImplementedException();
        }          

        private void LogListenerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogWriter.MsgProduced -= LogWriter_MsgProduced;
        }
    }
}
