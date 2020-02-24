//2015.03.23, czs, edit in namu, 修改执行方式

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Geo.IO;
using System.Windows.Forms;
using Gnsser.Interoperation;
using Geo.Common;
using System.Threading;
using System.Diagnostics;

namespace Gnsser.Winform
{
    public partial class RtkrcvCallerForm : Form
    {
        public RtkrcvCallerForm()
        {
            InitializeComponent();

            var fullPath = Path.GetFullPath(Setting.GnsserConfig.RtkrcvExe);

            this.RtkExecuter = new RtkrcvExecuter(fullPath);
            this.RtkExecuter.ErrorDataReceived += RtkExecuter_ErrorDataReceived;
            this.RtkExecuter.OutputDataReceived += RtkExecuter_OutputDataReceived;
            this.RtkExecuter.ExitedOrDisposed += RtkExecuter_Stoped;
        }

        void RtkExecuter_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke(new Action(()=> this.richTextBoxControl_info.Text += "\r\n" + e.Data));
        }

        void RtkExecuter_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke(new Action(() => this.richTextBoxControl_info.Text += "\r\n" + e.Data)); 
        }

        void RtkExecuter_Stoped(object sender, EventArgs e)
        {
            this.timer1.Stop();

           //update UI
            this.Invoke(new Action(() => { ShowRunnerState(); this.StateInfo = "系统已停止";}));
        }

        /// <summary>
        /// RTK 执行器
        /// </summary>
        public RtkrcvExecuter RtkExecuter { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        private string StateInfo { get { return textBox_stateInfo.Text; } set { textBox_stateInfo.Text = value; } }

        private void button_stop_Click(object sender, EventArgs e)
        {
            this.RtkExecuter.Stop();
            this.timer1.Stop();

            System.Threading.Thread.Sleep(100);

            ShowResult();

            MoveOutputTextToHistory();

            System.Threading.Thread.Sleep(100);
            ShowRunnerState();

            this.button_start.Enabled = true;
        }

        private void button_readConfig_Click(object sender, EventArgs e) { new Geo.WinTools.ConfigFileEditForm(Setting.GnsserConfig.RtkrcvConfig).ShowDialog(); }

        private void button_openHisDir_Click(object sender, EventArgs e) { Geo.Utils.FileUtil.OpenDirectory(OutputHistoryDirectory); }

        private void ShowRunnerState()
        {
            if (this.RtkExecuter != null)
            {
                this.StateInfo = RtkExecuter.IsRunning ? "正在运行" : "停止运行";
                this.StateInfo += "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                this.button_start.Enabled = !RtkExecuter.IsRunning;
                this.button_startFromPipe.Enabled = !RtkExecuter.IsRunning;
            }
            else
            {
                this.StateInfo = "未开始执行";
            }
        }

        private void button_startFromPipe_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
            this.RtkExecuter.IsRedirectPipe = true;
            this.Start();
        }
        private void button_start_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
            this.RtkExecuter.IsRedirectPipe = false;
            this.Start();
        }

        private void Start()
        {

            if (!this.RtkExecuter.IsRunning)
            {
                RtkrcvOption option = new RtkrcvOption();
                option.ConfigFilePath = Path.GetFullPath(Setting.GnsserConfig.RtkrcvConfig);
                RtkExecuter.Run(option);

                this.richTextBoxControl_result.Text = "已经运行啦，运行结果稍后呈现。。。";
                this.button_start.Enabled = !RtkExecuter.IsRunning;
                this.button_startFromPipe.Enabled = !RtkExecuter.IsRunning;
            }
            else
            {
                this.richTextBoxControl_result.Text = "已经运行,不用再运行了。";
            }
        }


        private   void StartService()
        {
            //System.Diagnostics.Process.Start（“程序的路径”，“参数1参数2”）; />第一参数aaa.exe路径，第二个参数是两个参数中的一个空格组成的字符串分开。 
            Process p = new Process();
            p.StartInfo.FileName =Path.GetFullPath( Setting.GnsserConfig.RtkrcvExe);
            p.StartInfo.Arguments = " -s -o  " + Path.GetFullPath(Setting.GnsserConfig.RtkrcvConfig);
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardError = false;
            // info.StartInfo.CreateNoWindow = true;//true表示不显示黑框，false表示显示dos界面 
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.UseShellExecute = false;


            p.EnableRaisingEvents = true;

            p.Exited += process_Exited;
            p.OutputDataReceived += DataReceived;
            p.ErrorDataReceived += DataReceived;

            p.Start();
            //info.StandardInput.WriteLine(" load " +   Setting.GnsserConfig.RtkrcvConfig);
            //System.Threading.Thread.Sleep(100);
            //info.StandardInput.WriteLine("start");

            //开始异步读取输出
           // info.BeginOutputReadLine();
            //info.BeginErrorReadLine();

            if ( p.StartInfo.RedirectStandardInput &&  !p.HasExited )
            {
                System.Threading.Thread.Sleep(100);
                p.StandardInput.WriteLine(" load " + Path.GetFullPath(Setting.GnsserConfig.RtkrcvConfig));
                System.Threading.Thread.Sleep(100);
                p.StandardInput.WriteLine("start");
                System.Threading.Thread.Sleep(100);
                p.StandardInput.WriteLine("y");

            } 

            //调用WaitForExit会等待Exited事件完成后再继续往下执行。
          //  info.WaitForExit();
            //  info.Close();

            //   Console.WriteLine("exit");
        }

        protected void process_Exited(object sender, EventArgs e)
        {

            MessageBox.Show("程序退出了！");
      
        }
        public void DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke(new Action(() => { this.richTextBoxControl_info.Text += e.Data + "\r\n"; this.richTextBoxControl_info.Update(); }));
           // MessageBox.Show(e.Data);
        }
        #region 处理细节
        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowStateAndResult();
        }

        private void ShowStateAndResult()
        {
            ShowRunnerState();
            ShowResult();
        }

        #region 路径

        static public string RtklibDir { get { return Path.GetDirectoryName(Setting.GnsserConfig.RtklibPostExe); } }
        static public string OutputPath { get { return System.IO.Path.Combine(RtklibDir, "rtcrcv_output.txt"); } }
        static public string OutputPathCopy { get { return System.IO.Path.Combine(RtklibDir, "rtcrcv_output_copy.txt"); } }
        static public string OutputHistoryPath { get { return System.IO.Path.Combine(OutputHistoryDirectory, "rtcrcv_output_" + Geo.Utils.DateTimeUtil.GetDateTimePathStringNow() + ".txt"); } }
        static public string OutputHistoryDirectory
        {
            get
            {
                string historyDirectory = System.IO.Path.Combine(RtklibDir, "History");
                if (!Directory.Exists(historyDirectory)) Directory.CreateDirectory(historyDirectory);
                return historyDirectory;
            }
        }
        #endregion

        /// <summary>
        /// 将输入文件转移到历史文件夹
        /// </summary>
        private static void MoveOutputTextToHistory()
        {
            if (System.IO.File.Exists(OutputPath))
            {
                try
                {
                    //采用复制而不采用移动，可以优先满足复制成功
                    File.Copy(OutputPath, OutputHistoryPath);

                    System.Threading.Thread.Sleep(100);

                    File.Delete(OutputPath);
                }
                catch (Exception ex) { MessageBox.Show("移动输出文件失败：" + ex.Message); }
            }
        }

        private void ShowResult()
        {
            if (!checkBox_showResult.Checked) return;

            if (System.IO.File.Exists(OutputPath))
            {
                try
                {
                    //首先复制一个拷贝，然后再读取拷贝文件 
                    File.Copy(OutputPath, OutputPathCopy, true);

                    System.Threading.Thread.Sleep(100);
                    var result = File.ReadAllText(OutputPathCopy);
                    if (result.Length > 10000) result.Substring(result.Length - 10000);

                    this.richTextBoxControl_result.Text = result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.richTextBoxControl_result.Text = ex.Message;
                }
            }
            else
            {
                this.richTextBoxControl_result.Text = "尚未产生输出 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + OutputPath;
            }
        }

        #endregion


    }
}