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
using System.Net;//FtpWebRequest
using Geo.Times; 
namespace Gnsser.Winform
{
    public partial class TaskRunnerForm : Form
    {
        TaskRunner taskRunner;
        public TaskRunnerForm()
        {
            InitializeComponent();
            taskRunner = new TaskRunner();
            taskRunner.InfoProduced += taskRunner_InfoProduced;
            taskRunner.TaskFinished += taskRunner_TaskFinished;
            
            this.comboBox_solveType.DataSource = Enum.GetNames(typeof(Gnsser.Interoperation.Bernese.PcfName));          
        }

        void taskRunner_TaskFinished()
        {
            this.Invoke(new Action(delegate() { this.button_run.Enabled = true; }));
            //watch.Stop();
            ShowInfo("计算完毕！耗时：" + watch.Elapsed.ToString());
            string newName = Path.GetFileName(taskRunner.ResultRinexFtp);
            string localSinexPath = Path.Combine(resultLocalDir, newName);
            ShowInfo("正在下载结果文件到本地" + localSinexPath);
            new WebClient().DownloadFile(taskRunner.ResultRinexFtp, localSinexPath);

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(localSinexPath);

            ShowInfo("所有计算完毕！耗时：" + watch.Elapsed.ToString());
        }

        void taskRunner_InfoProduced(string info) { this.ShowInfo(info); }

        public void ShowInfo(string info)
        {
            this.Invoke(new Action(delegate() { this.textBox_info.Text += DateTime.Now.ToString() + ": "+ info + "\r\n"; }));
        }

        static string resultLocalDir;
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

        private void button_run_Click(object sender, EventArgs e)
        {
            ShowInfo("开始工作....");
            watch.Stop();
            watch.Start();
            if (this.checkBox_asyn.Checked)
                taskRunner.RunAsyn(task);
            else taskRunner.Run(task);

            this.button_run.Enabled = false;               
        }       

        private string GetPcfName()
        {
            return  this.comboBox_solveType.SelectedItem.ToString(); 
        }

        private void button_viewState_Click(object sender, EventArgs e)
        {
            if (taskRunner != null)
            {
                string state = taskRunner.GetBernRunningState();
                ShowInfo(state);
            }
        }
        Task task;
        private void button_genXml_Click(object sender, EventArgs e)
        { 
            //数据准备变量 
            resultLocalDir = this.textBox_resultDir.Text;
            if (!Directory.Exists(resultLocalDir)) Directory.CreateDirectory(resultLocalDir);           

            task = new Task();
            task.Campaign = this.textBox_campaign.Text;
            task.Time= new Time(this.dateTimePicker_date.Value);
            task.OperationName = GetPcfName();
            task.ResultFtp = textBox_ftpResult.Text;
            task.Sites = new List<Winform.Site>();

           var urls = new List<string>(File.ReadAllLines(this.textBox_gnssUrlFilePath.Text));
           foreach (var item in task.Sites)
           {
               task.Sites.Add(item);
           }

            this.textBox_taskXml.Text = this.task.ToXml();
        }

        private void button_parseXml_Click(object sender, EventArgs e)
        {
            Task t; 
            //try
            //{
               t = Task.ParseXml(this.textBox_taskXml.Text);
               this.textBox_info.Text = t.ToString();
            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }


        }        

    }

}
