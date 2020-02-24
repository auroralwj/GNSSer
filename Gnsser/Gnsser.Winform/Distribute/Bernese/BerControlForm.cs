using System;
using Gnsser.Times;
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
using Gnsser.Interoperation.Bernese;
using System.Net;//FtpWebRequestusing Geo.Times; 
using Geo.Times; 

namespace Gnsser.Winform
{
    public partial class BerControlForm : Form
    {
        public BerControlForm()
        {
            InitializeComponent();

            this.comboBox_solveType.DataSource = Enum.GetNames(typeof(Gnsser.Interoperation.Bernese.PcfName));

            bpe = new BerBpe();
            bpe.WinCmd.ExitedOrDisposed += CmdHelper_ProcessExited;
        }
        void CmdHelper_ProcessExited(object sender, EventArgs e)
        {
            this.Invoke(new Action(delegate() { this.button_run.Enabled = true; }));

            //watch.Stop();
            ShowInfo("计算完毕！耗时：" + watch.Elapsed.ToString());

            //获取结果
            string sinexPath = bpe.GetSinexPath(pcfName);
            string newName =  Path.GetFileName(sinexPath);
            string ftpPath =  Path.Combine( resultFtp, newName);

            ShowInfo("正在上传结果文件" + sinexPath + " => " + ftpPath);
            new WebClient().UploadFile(ftpPath, sinexPath);

            string localSinexPath = Path.Combine(resultLocalDir, newName);
            ShowInfo("正在下载结果文件到本地" + localSinexPath);
            new WebClient().DownloadFile(ftpPath, localSinexPath);

            ShowInfo("所有计算完毕！耗时：" + watch.Elapsed.ToString());
            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(localSinexPath);
        }

        static Interoperation.Bernese.BerBpe bpe = null;
        string BerDataDir = @"C:\GPSDATA\";
        string pcfName;

        public void ShowInfo(string info)
        {
            this.Invoke(new Action(delegate() { this.textBox_info.Text += DateTime.Now.ToString() + ": "+ info + "\r\n"; }));
        }

        static string gnssUrlFilePath;
        static string resultLocalDir;
        static string resultFtp;
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        private void button_run_Click(object sender, EventArgs e)
        {
            ShowInfo("开始工作....");
            watch.Stop();
            watch.Start();   

            this.button_run.Enabled = false;
            //数据准备变量
            gnssUrlFilePath = this.textBox_gnssUrlFilePath.Text; 
            resultLocalDir = this.textBox_resultDir.Text;
            if (!Directory.Exists(resultLocalDir)) Directory.CreateDirectory(resultLocalDir);
            resultFtp = textBox_ftpResult.Text;
            //计算变量
            string campaign = this.textBox_campaign.Text;
            Time gpsTime = new Time(this.dateTimePicker_date.Value);
            this.pcfName = GetPcfName();

            //Go
            DataPrepare pre = new DataPrepare(
                File.ReadAllLines(gnssUrlFilePath),
                Path.Combine(this.BerDataDir, campaign,"ORX"));
            pre.InfoProduced += pre_InfoProduced;
            pre.Run();

            this.ShowInfo("数据准备完毕！");

            this.ShowInfo("开始计算！");
            bpe.Init(campaign, gpsTime, checkBox_skip.Checked);

            if (this.checkBox_asyn.Checked)
                bpe.RunAsyn(pcfName);
            else bpe.Run(pcfName);          
        }

        void pre_InfoProduced(string info) { this.ShowInfo(info); }

        private string  GetPcfName()
        {
            return  this.comboBox_solveType.SelectedItem.ToString(); 
        }

        private void button_viewState_Click(object sender, EventArgs e)
        {
            if (bpe != null)
            {
                string state = bpe.GetRunningState(pcfName);
                ShowInfo(state);
            }
        }        

    }

}
