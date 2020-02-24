using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;

namespace Geo.WinTools
{
    /// <summary>
    /// 合并dll
    /// </summary>
    public partial class ILmergeForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ILmergeForm()
        {
            InitializeComponent();
        }

        private void button_setDllDir_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBox_dllDir.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void button_setExePath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_exePath.Text = this.openFileDialog1.FileName;
        }

        private void button_setoutputpath_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox1_outputpath.Text = this.saveFileDialog1.FileName;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            string dllDir = this.textBox_dllDir.Text;
            string outpath = this.textBox1_outputpath.Text;
            string exePath = this.textBox_exePath.Text;

            Geo.Common.ProcessRunner cmd = new Common.ProcessRunner();
            if (this.textBox_exePath.Enabled)
            {
                var list = cmd.Run(ILmerge.GetMergeExeCmd(dllDir, exePath, outpath));
                this.textBox_results.Text = list[0];
                this.textBox1.Text = list[list.Count - 1];
            }
            else
            { 
                var list = cmd.Run(ILmerge.GetMergeDllCmd(dllDir, outpath));
                this.textBox_results.Text = list[0];
                this.textBox1.Text = list[list.Count - 1];
            }       
        }

        private void textBox1_outputpath_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_enableExe_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox_exePath.Enabled = checkBox_enableExe.Checked;
            this.button_setExePath.Enabled = checkBox_enableExe.Checked;
        }
    }
}
