using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class SiteNameMgrForm : Form
    {
        public SiteNameMgrForm()
        {
            InitializeComponent();
        }

        private void button_setPath_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Multiselect = false;
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_path.Text = this.openFileDialog1.FileName;
                button_read_Click(sender, e);
            }

        }

        private void button_read_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.textBox_path.Text))
            {
                MessageBox.Show("文件不存在！"); return;
            }

            this.textBox_sites.Lines = File.ReadAllLines(this.textBox_path.Text);
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            //if (this.saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllLines(this.textBox_path.Text, this.textBox_sites.Lines);
                MessageBox.Show("OK！");
            }

        }

        private void button_fromDir_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Multiselect = true;
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                string[] files = this.openFileDialog1.FileNames;
                List<String> fileList = new List<string>();
                //string tobeRemove = files[0].Substring(files[0].Length - 10);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file).Substring(0, 4);//.Replace(tobeRemove, "");
                    if (!fileList.Contains(fileName)) fileList.Add(fileName);
                }
                this.textBox_sites.Lines = fileList.ToArray();
                MessageBox.Show("共提取 " + fileList.Count + " 个文件名。");
            }
        }
        bool isLow = true;
        private void button_trigerUpLow_Click(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            for (int i = 0; i < this.textBox_sites.Lines.Length; i++)
            {
               string line = isLow ?
                    this.textBox_sites.Lines[i].ToLower() :
                    this.textBox_sites.Lines[i].ToUpper();
               ls.Add(line);
            }
            this.textBox_sites.Lines = ls.ToArray();

            if (isLow) this.button_trigerUpLow.Text = "转换为大写";
            else this.button_trigerUpLow.Text = "转换为小写";

            isLow = !isLow;

        }
    }
}
