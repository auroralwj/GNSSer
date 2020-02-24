using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Geo.Common;

namespace Geo.WinTools
{
    /// <summary>
    /// 任务监听
    /// </summary>
    public partial class TaskListenerForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskListenerForm()
        {
            InitializeComponent();
        }

        TcpListener lister;  
        Thread t;
        private void button_run_Click(object sender, EventArgs e)
        {
            int port = int.Parse( this.textBox_port.Text);

            lister = new TcpListener(IPAddress.Any, port);  
            Stop = false;
            this.button_run.Enabled = false;
            this.label_state_info.Text = "正在运行....";

            lister.Start();

            t = new Thread(ListenClient);
            t.Start();
        }

        static bool Stop = false;

        List<TelCommand> list = new List<TelCommand>();
        /// <summary>
        /// 监听
        /// </summary>
        public void ListenClient()
        {
            TcpClient client = null;
            while (true)
            {
               if (Stop) break;
                try
                {
                    client = lister.AcceptTcpClient();
                }
                catch
                {
                    break;
                }
                TelCommand cmd = new TelCommand(client);
                list.Add(cmd);

                string info = "来自" + client.Client.RemoteEndPoint + ",信息：" + cmd.CmdStr + "\r\n";

                //Geo.Utils.Command comand = new Utils.Command();
                //comand.RunAsyn(cmd.CmdStr);

                ShowInfo(info);
            }
        }

        private void ShowInfo(string info)
        {
            this.Invoke(new Action(delegate() { this.textBox_info.Text += DateTime.Now + ":" +  info + "\r\n"; }));
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            Action action = CloseListener;
            action.BeginInvoke(ar => { if (ar.IsCompleted) ShowInfo("异步操作结束"); }, null);

            ShowInfo("正在请求停止监听....");
        }

        private void CloseListener()
        {

            Stop = true;           

            if (lister != null)
            {
                lister.Stop();               
            }
            if (t != null && t.IsAlive)
            {               
                t.Abort();
                t.Join();
            }

            ShowInfo("已经成功停止监听。");

            this.Invoke(new Action(delegate()
            {
                this.button_run.Enabled = true;
                this.label_state_info.Text = "没有运行";
            }));
        }

        private void TaskListenerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseListener();
        }
    }
}
