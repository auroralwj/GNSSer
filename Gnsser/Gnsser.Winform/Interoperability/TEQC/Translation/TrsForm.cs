//2014.12.27, lh, create in 郑州, TEQC 互操作

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Gnsser.Interoperation.Teqc;

namespace Gnsser.Winform
{
    public partial class TrsForm : Form
    {
        public TrsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox1.Lines = openFileDialog1.FileNames;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //check
                string[] files = this.textBox1.Lines;
                if (files.Length == 0) throw new ArgumentNullException("输入文件不可为空。");
                string[] ff = this.textBox2.Lines;
                if (ff.Length == 0) throw new ArgumentNullException("输入文件不可为空。");
                string fileName;

                TeqcFunctionCaller call = new TeqcFunctionCaller(TeqcSet.TeqcPath, TeqcFunction.Translation);
                switch (comboBox1.Text)
                {
                    //下面的代码有问题，因为原文件为.dat，而用到的n文件和o文件前面的年份暂无法确定
                    case "Trimble":
                        fileName = this.textBox2.Text + files[0].Substring(files[0].LastIndexOf("\\"));//获取新路径及其文件名
                        files[0] = "-tr do -week " + this.textBox4.Text + " +nav " + fileName.Substring(0, fileName.LastIndexOf(".") + 1) +
                                   this.textBox4.Text.Substring(2, 2) + "n " + this.textBox1.Text + " > " + fileName.Substring(0, fileName.LastIndexOf(".") + 1) +
                                   this.textBox4.Text.Substring(2, 2) + "o";
                        this.textBox3.Text = call.Run(files)[0];
                        break;
                    case "Leica":
                        //param = QualityChecking;
                        break;
                    case "Topcon":
                        fileName = this.textBox2.Text + files[0].Substring(files[0].LastIndexOf("\\"));
                        files[0] = "-top tps -week " + this.textBox4.Text + " +nav " + fileName.Substring(0, fileName.LastIndexOf(".") + 1) +
                                   this.textBox4.Text.Substring(2, 2) + "n " + this.textBox1.Text + " > " + fileName.Substring(0, fileName.LastIndexOf(".") + 1) +
                                   this.textBox4.Text.Substring(2, 2) + "o";
                        this.textBox3.Text = call.Run(files)[0];
                        break;
                    default:
                        //param = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox2.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
