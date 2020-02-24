//2015.11.03, czs, edit in hongqing, 分布式界面
//2016.11.28, czs, edit in hongqing, 调试GOF分布式

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using Geo.IO;

namespace Gnsser.Winform
{

    public delegate string RunGofNodeDelegate(GofComputeNode computeNode);

    public partial class GofDistributeControlForm : Form
    {
        Log log = new Log(typeof(GofDistributeControlForm));

        public GofDistributeControlForm()
        {
            InitializeComponent();
        }

        private void SiteNodeMgrForm_Load(object sender, EventArgs e) {

            this.textBox_recevePort.Text = Setting.GnsserConfig.ControlPort + "";
            this.textBox_dataftp.Text = Setting.GnsserConfig.FtpServerUrl;
            this.textBox_ftpResult.Text = Setting.GnsserConfig.FtpServerUrl;

            BindMain(); 
        }

        private void 刷新计算节点ToolStripMenuItem_Click(object sender, EventArgs e) { BindMain(); }

        private void BindMain()
        {
            ComputeNodes = ComputeNodeMgr<GofTask>.LoadGofComputeNodes(
                Setting.GnsserConfig.ComputeNodeFilePath,
                Setting.GnsserConfig.GofTaskFilePath);
            this.bindingSource1.DataSource = ComputeNodes;
        }

        public void ShowInfo(string info)
        {
            if (!this.IsDisposed)
            {
                Geo.Utils.FormUtil.InsertLineToTextBox(textBox_info, DateTime.Now.ToString() + ": " + info + "\r\n");
            }
        }
         
        /// <summary>
        /// 计算节点
        /// </summary>
        List<GofComputeNode> ComputeNodes { get; set; }
        static int _taskCount = 0;
        static string rawDataFtp;
        static string resultDir { get; set; }
        static string resultFtp;
        static Dictionary<GofComputeNode, int> ComputeNodeTaskDic = new Dictionary<GofComputeNode, int>();
        static int failedThreadCount = 0;
        static int completedThreadCount = 0;
        TcpListener lister;
        Thread t;
        static bool Stop = false;

        List<Geo.Common.TelCommand> list = new List<Geo.Common.TelCommand>();
        TcpClient client = null;

        static System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        private void button_run_Click(object sender, EventArgs e)
        {
            try
            {
                this.button_run.Enabled = false;

                rawDataFtp = this.textBox_dataftp.Text;
                resultDir = this.textBox_resultDir.Text;
                if (!Directory.Exists(resultDir)) Directory.CreateDirectory(resultDir);
                resultFtp = textBox_ftpResult.Text;

                completedThreadCount = 0;
                failedThreadCount = 0;

                backgroundWorker1.RunWorkerAsync();

                //接收
                int port = int.Parse(this.textBox_recevePort.Text);
                lister = new TcpListener(IPAddress.Any, port);
                Stop = false;
                lister.Start();
                t = new Thread(ListenClient);
                t.Start();
            }
            catch (Exception ex)
            {
                log.Error("出现先错误，" + ex.Message);
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowInfo("开始工作....");
            watch.Restart();

            //统计有多少个任务
            _taskCount = 0;
            foreach (var item in ComputeNodes) if (item.Enabled) _taskCount += item.TaskCount;

            //清零工作记录列表   
            foreach (var node in ComputeNodes)
                if (node.Enabled)
                {
                    if (!ComputeNodeTaskDic.ContainsKey(node)) ComputeNodeTaskDic.Add(node, 0);
                    else ComputeNodeTaskDic[node] = 0;
                }


            //首次分配任务
            for (int i = 0; i < ComputeNodes.Count; i++)
            {
                GofComputeNode n = ComputeNodes[i];
                if (n.Enabled)
                {
                    string msg = n.ToString() + "开始执行......";
                    ShowInfo(msg);

                    InvokeComputeNodeTask(n);
                }
            }
        }

        /// <summary>
        /// 异步执行节点任务
        /// </summary>
        /// <param name="north"></param>
        private void InvokeComputeNodeTask(GofComputeNode n)
        {
            RunGofNodeDelegate runNode = RunOne;
            runNode.BeginInvoke(n, ar =>
            {// 380, 120
                string response = runNode.EndInvoke(ar);
                ShowInfo(n.ToString() + "返回信息：" + response);
            }, runNode);
        }

        /// <summary>
        /// 运行客户端，调用。
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string RunOne(GofComputeNode node)
        {
            string str = "";
            try
            {
                //列表，记录任务
                int taskIndex = ComputeNodeTaskDic[node];

                if (node.TaskCount <= taskIndex) { return node + " 所有任务已经执行完！"; }
                
                var task = node.Tasks[taskIndex];

                TelMsg tMsg = new TelMsg()
                {
                    MsgType = Winform.MsgType.Task,
                    XmlContent = task.ToXml(),
                    From = Geo.Utils.NetUtil.GetFirstIp(),
                    To = node.Ip
                };

                TcpClient client = new TcpClient(node.Ip, node.Port);
                NetworkStream stream = client.GetStream();
                BinaryReader br = new BinaryReader(stream);
                BinaryWriter bw = new BinaryWriter(stream);
                bw.Write(tMsg.ToXml());

                bw.Flush();
                //BinaryWriter.Close();
                str = br.ReadString();

                //工作编号增加1
                ComputeNodeTaskDic[node]++;

            }
            catch (Exception ex)
            {
                str = node + "出错， " + ex.Message;
                //失败的节点
                failedThreadCount++;
            }

            return str;
        }

        /// <summary>
        /// 监听客户端执行结果。并下载到本地。
        /// </summary>
        public void ListenClient()
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
                //有数据，则读取
                if (client.Available > 0)
                { 
                    BinaryReader br = new BinaryReader(client.GetStream());
                    string msg = br.ReadString();

                    TelMsg tMsg = TelMsg.ParseXml(msg);
                    ShowInfo("收到信息，来自 " + tMsg.From + "," + msg);
                    if (tMsg.MsgType == MsgType.Path)
                    {
                        string localPath = Path.Combine(resultDir, Path.GetFileName(tMsg.XmlContent));
                        Geo.Utils.NetUtil.FtpDownload(tMsg.XmlContent, localPath);
                        ShowInfo("已经下载到本地" + localPath + "耗时：" + Geo.Utils.DateTimeUtil.GetFloatString(watch.Elapsed));

                        completedThreadCount++;
                        //int runCount = ComputeNodes.Count<GofComputeNode>(m => m.Enabled);
                        if (completedThreadCount + failedThreadCount == _taskCount)
                        {
                            AfterCompleted();
                        }
                        else//继续执行。
                        {
                            GofComputeNode node = ComputeNodes.Find(m => tMsg.From.Contains(m.Ip));
                            if (node != null) InvokeComputeNodeTask(node);
                        }
                    }
                }
            }
        }

        private void AfterCompleted()
        {
            this.Invoke(new Action(delegate()
            {
                this.button_run.Enabled = true;
                watch.Stop();
                this.ShowInfo("计算完毕，总共耗时：" + Geo.Utils.DateTimeUtil.GetFloatString(watch.Elapsed));

                Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(resultDir);

            }));
            //没有手动结束，都不停止。
            //CloseListener();
        }

        private static string DownloadResult(string result)
        {
            string snxFileName = Path.GetFileName(result);
            string path = Path.Combine(resultDir, snxFileName);
            Geo.Utils.NetUtil.FtpDownload(result, path);
            return path;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            //在运行时检查客户机状态
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            CloseListener();
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
            }

            ShowInfo("已经成功停止监听。");

            this.Invoke(new Action(delegate()
            {
                this.button_run.Enabled = true;
            }));
        }

        private void DistributeControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseListener();
        }

        private void button_clean_Click(object sender, EventArgs e)
        {
            this.textBox_info.Text = "";
        }

        private void button_reload_Click(object sender, EventArgs e)
        {
            this.BindMain();
        }

        private void button_computeNode_Click(object sender, EventArgs e)
        {
            var form = new GofComputeNodeMgrForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.BindMain();
            }
        }

        private void button_taskMaitain_Click(object sender, EventArgs e)
        {
            var form = new GofTaskMgrForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.BindMain();
            } 
        } 
    }
}