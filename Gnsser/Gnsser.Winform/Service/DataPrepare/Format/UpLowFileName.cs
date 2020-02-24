using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Geo.Utils;

namespace Gnsser.Winform
{
    public partial class UpLowFileName : Form
    {
        public UpLowFileName()
        {
            InitializeComponent();
        }

        private void button_go_Click(object sender, EventArgs e)
        {
            string path = this.textBox_path.Text; 
            if (!Directory.Exists(path))
            {
                FormUtil.ShowWarningMessageBox("目录 " + path + " 不存在！");
                return;
            }
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (this.radioButton_up.Checked)
                    File.Move(file, file.ToUpper());
                if (radioButton_low.Checked)
                    File.Move(file, file.ToLower());
            }

            FormUtil.ShowIfOpenDirMessageBox(path); 
        }

        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_path.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
