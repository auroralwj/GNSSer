using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Geo.WinTools
{
    /// <summary>
    /// 处理窗口
    /// </summary>
    public partial class ProcessInfoForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessInfoForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            int pid = int.Parse(this.textBox_pid.Text);
            Process p = Process.GetProcessById(pid);
            string info = "程序名称：" + p.ProcessName;

            this.textBox_result.Text = info;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
