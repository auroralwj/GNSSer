using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gnsser.Winform.Other
{
    public partial class FileSize : Form
    {
        public FileSize()
        {
            InitializeComponent();
        }
        List<string> files = new List<string>();
        List<double> sizes = new List<double>();
        private void button_select_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                this.textBox_dir.Text = this.folderBrowserDialog1.SelectedPath;
            string sourseDir1 = this.textBox_dir.Text;
            string[] tmpfiles = Directory.GetFiles(sourseDir1, "*");
            for(int i =0 ; i< tmpfiles.Length;i++)
            {
                files.Add(tmpfiles[i]);
            }
        }

        private void button_calsize_Click(object sender, EventArgs e)
        {
            foreach(var item in files)
            {
                FileInfo fi = new FileInfo(item);
                sizes.Add(fi.Length / 1024);
            }
            double[] a = new double[8];

            double[] b = new double[8];
            foreach(var num in sizes)
            {
                if(num <= 10)
                {
                    a[0]++;
                    b[0] += num;
                }  
                else if(num > 10 && num <= 50)
                {
                    a[1]++;
                    b[1] += num;
                }
                else if(num > 50 && num <= 100)
                {
                    a[2]++;
                    b[2] += num;
                }
                else if (num > 100 && num <= 500)
                {
                    a[3]++;
                    b[3] += num;
                }
                else if (num > 500 && num <= 1000)
                {
                    a[4]++;
                    b[4] += num;
                }
                else if (num > 1000 && num <= 4000)
                {
                    a[5]++;
                    b[5] += num;
                }
                else if (num > 4000 && num <= 10000)
                {
                    a[6]++;
                    b[6] += num;
                }
                else if (num > 10000)
                {
                    a[7]++;
                    b[7] += num;
                }
            }
            double sum = b[0] + b[1] + b[2] + b[3] + b[4] + b[5] + b[6] + b[7];
        }
    }
}
