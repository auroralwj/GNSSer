using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Geo.Winform
{
    public partial class ChangeSplashForm : Form
    {
        public ChangeSplashForm()
        {
            InitializeComponent();
        }

        private void button_browsePath_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_path.Text = this.openFileDialog1.FileName;
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            string path = this.textBox_path.Text.Trim();
            if (!File.Exists(path))
            {
                MessageBox.Show("请输入图片路径！"); return;
            }
            try
            { 
                File.Delete(Geo.Winform.Setting.SplashPath);
                File.Copy(path, Geo.Winform.Setting.SplashPath, true);

                if (MessageBox.Show("更改成功，立刻重启查看效果？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Application.Restart();
                }
                MessageBox.Show("更改成功");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }catch(Exception ex){
                MessageBox.Show("更改失败了。\r\n" + ex.Message);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
