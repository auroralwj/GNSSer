using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Geo.WinTools.Sys
{
    /// <summary>
    /// 系统进程查看
    /// </summary>
    public partial class ProcessForm1 : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessForm1()
        {
            InitializeComponent();
        }

        private void ProcessForm1_Load(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcesses())
            {
                this.textBox_processes.Text +="进程名称：" + p.ProcessName + "虚拟内存大小：" +  p.VirtualMemorySize64 + "\r\n";
                this.textBox_processes.Text += "相关线程：" + "\r\n";
                foreach (ProcessThread thread in p.Threads)
                {
                    this.textBox_processes.Text +=  "\t 编号： " + thread.Id+ "\t线程内存地址" + thread.StartAddress +"\r\n";
                }
            }





        }
    }
}