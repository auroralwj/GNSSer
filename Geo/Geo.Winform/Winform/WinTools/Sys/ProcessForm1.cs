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
    /// ϵͳ���̲鿴
    /// </summary>
    public partial class ProcessForm1 : Form
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public ProcessForm1()
        {
            InitializeComponent();
        }

        private void ProcessForm1_Load(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcesses())
            {
                this.textBox_processes.Text +="�������ƣ�" + p.ProcessName + "�����ڴ��С��" +  p.VirtualMemorySize64 + "\r\n";
                this.textBox_processes.Text += "����̣߳�" + "\r\n";
                foreach (ProcessThread thread in p.Threads)
                {
                    this.textBox_processes.Text +=  "\t ��ţ� " + thread.Id+ "\t�߳��ڴ��ַ" + thread.StartAddress +"\r\n";
                }
            }





        }
    }
}