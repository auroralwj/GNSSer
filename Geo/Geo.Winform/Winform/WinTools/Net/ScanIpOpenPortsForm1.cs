using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets ;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Geo.WinTools.Net
{
    /// <summary>
    /// 扫描打开的端口
    /// </summary>
    public partial class ScanIpOpenPortsForm1 : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScanIpOpenPortsForm1()
        {
            InitializeComponent();
        }

        private void button_Go_Click(object sender, EventArgs e)
        {
            TcpClient client = null;
            int start = int.Parse(this.textBox_StartPort.Text);
            int end = int.Parse(this.textBox_EndPort.Text) +1;
            string port = "";
            this.progressBar1.Step = 1;
            this.progressBar1.Minimum = start;
            this.progressBar1.Maximum = end;
            this.progressBar1.Value = this.progressBar1.Minimum;

            for (int i = start; i < end; i++)
            {
                try
                {
                    string hostNmeOrAddress = this.textBox_hostNameOrAddress.Text;
                    client = new TcpClient(hostNmeOrAddress,i );

                    port += i + "\r\n";
                    this.textBox_OutPut.Text = port;
                    this.Refresh();

                }
                catch //(Exception ex)
                {
                    // MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //progressBar1.Value += 1;
                progressBar1.PerformStep();
            }
            this.textBox_OutPut.Text = port;
        }
    }
}