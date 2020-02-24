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
    public partial class SystemSelectionForm : Form
    {
        public SystemSelectionForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox1.Lines = openFileDialog1.FileNames;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox3.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //check
                string[] files = this.textBox1.Lines;
                if (files.Length == 0) throw new ArgumentNullException("输入文件不可为空。");
                string[] ff = this.textBox3.Lines;
                if (ff.Length == 0)
                    ff[0] = files[0].Substring(0, files[0].LastIndexOf("\\"));
                string[] fs = this.textBox2.Lines;
                string sp;
                if (fs.Length == 0)
                    sp = "";
                else
                    sp = fs[0];
                string newrinex_path = "0";//此处若不赋值，则下文读写文件时会提示使用为赋值的路径？


                TeqcFunctionCaller call = new TeqcFunctionCaller(TeqcSet.TeqcPath, TeqcFunction.Translation);
                switch (comboBox1.Text)
                {
                    case "GPS":
                        string fileName = this.textBox3.Text + files[0].Substring(files[0].LastIndexOf("\\"));//获取新路径及其文件名
                        files[0] = "-G " + sp + " " + files[0] + " > " + fileName;
                        this.textBox4.Text = call.Run(files)[0];
                        newrinex_path = fileName;
                        break;
                    case "GLONASS":
                        string fileName2 = this.textBox3.Text + files[0].Substring(files[0].LastIndexOf("\\"));//获取新路径及其文件名
                        files[0] = "-R " + sp + " " + files[0] + " > " + fileName2;
                        this.textBox4.Text = call.Run(files)[0];
                        newrinex_path = fileName2;
                        break;
                    default:
                        //param = "";
                        break;
                }

                //下面部分对卫星系统选择后的o文件的文件头信息 进行修改，使其在后续质量检核中生成的结果文件可以被绘图文件顺利读取
                string[] readText = File.ReadAllLines(newrinex_path);
                File.WriteAllText(newrinex_path, "");//创建一个空文件，覆盖原来的newrinex_path
                StreamWriter sr = new StreamWriter(newrinex_path);
                for (int i = 0; i < readText.Length; i++)
                {
                    if (readText[i].Contains("M (MIXED)"))
                        readText[i] = readText[i].Replace("M (MIXED)", "G (GPS)  ");
                    if (readText[i].Contains("Mixed(MIXED)"))
                        readText[i] = readText[i].Replace("Mixed(MIXED)", "G (GPS)     ");
                    sr.WriteLine(readText[i]);
                    //if (readText[time].Contains("END OF HEADER"))
                    //{
                    //    for (int j = time+1; j < readText.Length; j++)
                    //    {
                    //        readText[j] = readText[j].Replace("G", " ");
                    //        sr.WriteLine(readText[j]);
                    //    }
                    //    break;
                    //}
                }
                sr.Flush();
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
