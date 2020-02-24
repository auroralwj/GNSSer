using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Geo.WinTools
{
    /// <summary>
    /// 文件选择
    /// </summary>
    public partial class FileSelectorForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileSelectorForm()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            string[] lines = this.textBox_incKeys.Lines;
            string indir = this.textBox_inDir.Text;
            string saveDir = this.textBox_saveDir.Text;
            string patern = this.textBox_filePatern.Text;

            string[] paths = Directory.GetFiles(indir, patern);
            foreach (var item in paths)
            {
                string fileName = Path.GetFileNameWithoutExtension(item);

                foreach (var name in lines)
                {
                    if(name.Trim() != "")
                    if (fileName.ToUpper().Contains(name.Trim().ToUpper()))
                        File.Copy(item, Path.Combine(saveDir, fileName));

                } 
            }

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(saveDir);
        }

        private void button_setInDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_inDir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_setSaveDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_saveDir.Text = folderBrowserDialog1.SelectedPath;
   
        }
    }
}
