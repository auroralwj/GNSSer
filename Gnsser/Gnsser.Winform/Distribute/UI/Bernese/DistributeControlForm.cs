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

namespace Gnsser.Winform
{
    public delegate string RunNodeDelegate(BerComputeNode computeNode);

    public partial class DistributeControlForm : Form
    {
        List<BerComputeNode> ComputeNodes;

        public DistributeControlForm()
        {
            InitializeComponent();
        }

        private void SiteNodeMgrForm_Load(object sender, EventArgs e) { BindMain(); }

        private void 刷新计算节点ToolStripMenuItem_Click(object sender, EventArgs e) { BindMain(); }

        private void BindMain()
        {
            ComputeNodes = BerComputeNodeMgr.LoadComputeNodes(
                Setting.GnsserConfig.ComputeNodeFilePath,
                Setting.GnsserConfig.TaskFilePath,
                Setting.GnsserConfig.SiteFilePath);
            this.bindingSource1.DataSource = ComputeNodes;
        }

        public void ShowInfo(string info)
        {
            if (!this.IsDisposed)
            { this.Invoke(new Action(delegate() { this.textBox_info.Text += DateTime.Now.ToString() + ": " + info + "\r\n"; })); }
        }

        //
        static int _taskCount = 0;
        static string rawDataFtp;
        static string resultDir;
        static string resultFtp;
        static Dictionary<BerComputeNode, int> ComputeNodeTaskDic = new Dictionary<BerComputeNode, int>();

        static System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        private void button_run_Click(object sender, EventArgs e)
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
                BerComputeNode n = ComputeNodes[i];
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
        private void InvokeComputeNodeTask(BerComputeNode n)
        {
            RunNodeDelegate runNode = RunOne;
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
        private string RunOne(BerComputeNode node)
        {
            string str = "";
            try
            {
                //列表，记录任务
                int taskIndex = ComputeNodeTaskDic[node];

                if (node.TaskCount <= taskIndex) { return node + " 所有任务已经执行完！"; }
                
                Task task = node.Tasks[taskIndex];

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

        static int failedThreadCount = 0;
        static int completedThreadCount = 0;
        TcpListener lister;
        Thread t;
        static bool Stop = false;

        List<Geo.Common.TelCommand> list = new List<Geo.Common.TelCommand>();
        TcpClient client = null;

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
                    //int runCount = ComputeNodes.Count<ComputeNode>(m => m.Enabled);
                    if (completedThreadCount + failedThreadCount == _taskCount)
                    {
                        AfterCompleted();
                    }
                    else//继续执行。
                    {
                        BerComputeNode node = ComputeNodes.Find(m => tMsg.From.Contains(m.Ip));
                        if (node != null) InvokeComputeNodeTask(node);
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

    }
}