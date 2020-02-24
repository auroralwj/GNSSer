using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

//using ICSharpCode.SharpZipLib.Zip.Compression;
//using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
//using ICSharpCode.SharpZipLib.GZip;

using System.IO.Compression;
using Geo.Utils;


namespace Geo.WinTools.Sys
{
    /// <summary>
    /// ZIP 压缩
    /// </summary>
    public partial class ZipForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ZipForm()
        {
            InitializeComponent();
        }

        private void button_mulit_decompress_Click(object sender, EventArgs e)
        {
            string sourseDir = this.textBox_source_dir.Text;
            string destDir = this.textBox_dest_dir.Text;
            string pass = GetPassword();
            string[] files = Directory.GetFiles(sourseDir);
            bool deleSourse = this.checkBox_deleSouse.Checked;

            progressBar1.Maximum = files.Length;
            progressBar1.Minimum = 1;
            progressBar1.Step = 1;
            progressBar1.Value = progressBar1.Minimum;

            foreach (var item in files)
            {
                try
                {
                    CompressUtil.Decompres(item, destDir, pass, deleSourse);
                }
                catch (Exception ex)
                {
                    if (!checkBox_ignoreError.Checked)
                        if (FormUtil.ShowYesNoMessageBox("出错了:" + ex.Message + "\r\n" + "是否继续?")
                            == System.Windows.Forms.DialogResult.No)
                            break;
                   
                }

                progressBar1.PerformStep();
                progressBar1.Refresh();
            }
            FormUtil.ShowIfOpenDirMessageBox(destDir); 
        }

        private void button_decompress_Click(object sender, EventArgs e)
        {
            string compressedFilePath = this.textBox_sourseZip.Text;        
            string destDir = this.textBox_destDir.Text;
            string pass = GetPassword();
            CompressUtil.Decompres(compressedFilePath, destDir, pass);

            FormUtil.ShowIfOpenDirMessageBox(destDir); 
        }
        
        private void button_comress_Click(object sender, EventArgs e)
        {
            string sourseDir = this.textBox_sourseDir.Text;
            string destZip = this.textBox_destZipDir.Text;
            string pass = GetPassword();
            
            //ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
            //if (pass != null) fastZip.Password = pass;
            //fastZip.CreateZip(destZip, sourseDir, true, "");
            //MessageBox.Show("操作完毕！" + destZip);
        }

        private string GetPassword() { string pass = null; if (checkBox_enablePass.Checked) if (this.textBox_pass.Text.Length > 0) pass = this.textBox_pass.Text; return pass; }
        private void button_setSourseDirPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_sourseDir.Text = this.folderBrowserDialog1.SelectedPath;
                this.textBox_source_dir.Text = this.folderBrowserDialog1.SelectedPath;

                if (textBox_dest_dir.Text == "")
                    textBox_dest_dir.Text = this.textBox_source_dir.Text;

                if (textBox_destZipDir.Text == "")
                {
                    textBox_destZipDir.Text = Path.GetDirectoryName(this.textBox_sourseDir.Text);
                }
            }
        }
        private void button_SetDestZip_Click(object sender, EventArgs e) { if (this.saveFileDialog1.ShowDialog() == DialogResult.OK) this.textBox_destZipDir.Text = this.saveFileDialog1.FileName; }
        private void button_setSourseZip_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_sourseZip.Text = this.openFileDialog1.FileName;
                if (textBox_destDir.Text == "")
                {
                    textBox_destDir.Text = Path.GetDirectoryName(this.textBox_sourseZip.Text);
              }
            }
        }
        private void button_setDestDir_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_destDir.Text = this.folderBrowserDialog1.SelectedPath;
                this.textBox_dest_dir.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }
        private void checkBox_enablePass_CheckedChanged(object sender, EventArgs e) { this.textBox_pass.Visible = checkBox_enablePass.Checked; }

    }
}