using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;

namespace Geo.Winform.Sys
{
    /// <summary>
    /// CMD运行
    /// </summary>
    public partial class CmdForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CmdForm()
        {
            InitializeComponent();
        }
        Geo.Common.ProcessRunner helper = new Geo.Common.ProcessRunner();

        private void button_run_Click(object sender, EventArgs e)
        {
            string cmdStr = this.textBox_cmd.Text;

           
           helper.RunAsyn(cmdStr);
          //  this.textBox_result.Text += result + "\r\n";
        }

        private void button_runAsyn_Click(object sender, EventArgs e)
        {
            string cmdStr = this.textBox_cmd.Text;
            string result =  helper.Run(cmdStr)[0];
            this.textBox_result.Text += result + "\r\n";

        }
    }
}
