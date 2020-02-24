//2012,czs, create in zz, 分布式任务解析执行
//2015.11.02, czs edit in hongqing, Gnsser 分布式任务执行

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

namespace Gnsser.Winform 
{
    public partial class TaskClientForm : Form
    {
        public TaskClientForm()
        {
            InitializeComponent();
        }

        TcpListener lister;
        Thread t;

        int controlPort = 10001;
        string controlIp = "107.0.0.1";//控制端IP,单机测试版

        private void button_run_Click(object sender, EventArgs e)
        {

            Gnsser.Interoperation.GPSTk.GpsTkBpe.campaignDir = this.textBox_gpsdata.Text;//工程所在目录

            int listenPort = int.Parse(this.textBox_listenPort.Text);
            controlPort = int.Parse(this.textBox_controlPort.Text);
            controlIp = this.textBox_controlIp.Text;

            lister = new TcpListener(IPAddress.Any, listenPort);
            Stop = false;
            this.button_run.Enabled = false;
            this.label_state_info.Text = "正在运行....";

            lister.Start();

            t = new Thread(ListenClient);
            t.Start();
        }

        static bool Stop = false;

        List<TelCommand> list = new List<TelCommand>();
        TcpClient client = null;
        public void ListenClient()
        {
            try
            {
                while (true)
                {
                    if (Stop) break;
                    try
                    {
                        client = lister.AcceptTcpClient();
                    }
                    catch (Exception ex)
                    {
                        ShowInfo(ex.Message);
                        break;
                    }
                    TelCommand cmd = new TelCommand(client);
                    list.Add(cmd);

                    string info = "来自" + cmd.LocalEndPoint + ",命令：" + cmd.CmdStr + "\r\n";
                    ShowInfo(info);
                    ShowInfo("开始执行任务....");

                    currentTask = Task.ParseXml(TelMsg.ParseXml(cmd.CmdStr).XmlContent);

                    runner = new GnssTaskRunner(Gnsser.Interoperation.GPSTk.GpsTkBpe.campaignDir);
                    runner.InfoProduced += runner_InfoProduced;
                    runner.TaskFinished += runner_TaskFinished;
                    runner.RunAsyn(currentTask);
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                    ShowInfo("运行错误：" + ex.Message);
            }
        }


        static GnssTaskRunner runner;
        Task currentTask;
        /// <summary>
        /// 任务执行完后
        /// </summary>
        void runner_TaskFinished()
        {
            try
            {
                //发送结果数据到服务器
                string path = runner.ResultRinexFtp;
                TcpClient newclient = new TcpClient(controlIp, controlPort);
                BinaryWriter newbr = new BinaryWriter(newclient.GetStream());

                TelMsg tMsg = new TelMsg()
                {
                    MsgType = Winform.MsgType.Path,
                    XmlContent = path,
                    From = Geo.Utils.NetUtil.GetIp(),
                    To = controlIp
                };

                newbr.Write(tMsg.ToXml());
                newbr.Flush();
            }
            catch (Exception ex)
            {
                ShowInfo("向服务器返回数据出错：" + ex.Message);
            }
        }

        void runner_InfoProduced(string info) { ShowInfo(info); }
        private void ShowInfo(string info)
        {
            if (this != null)
                this.Invoke(new Action(delegate()
                {
                    if (!this.IsDisposed && !this.textBox_info.IsDisposed)
                        this.textBox_info.Text += DateTime.Now + ":" + info + "\r\n";
                }));
        }

         
        private void CloseListener()
        {
            ShowInfo("正在请求停止监听....");

            Stop = true;

            if (lister != null)
            {
                lister.Stop();
            }
            if (t != null && t.IsAlive)
            {
                t.Abort();
                //t.Join();
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

         

        private void button_clean_Click(object sender, EventArgs e)
        {
            textBox_info.Text = "";
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            Action action = CloseListener;
            action.BeginInvoke(ar => { if (ar.IsCompleted) ShowInfo("异步操作结束"); }, null);
        }

        private void button_viewState_Click(object sender, EventArgs e)
        {
            ShowInfo(runner.GetBernRunningState());
        }

        private void textBox_controlIp_TextChanged(object sender, EventArgs e)
        {

        }

         

    }
}
