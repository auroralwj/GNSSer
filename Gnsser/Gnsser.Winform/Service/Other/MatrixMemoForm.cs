using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class MatrixMemoForm : Form
    {
        public MatrixMemoForm()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            int row = int.Parse(this.textBox_row.Text);
            int col = int.Parse(this.textBox_col.Text);
            bool isFolat = checkBox_float.Checked;
            int ItemSize = isFolat ? sizeof(float) : sizeof(double);
            double memo = 1.0 * row * col * ItemSize;

             string msg = String.Format("Double[{0},{1}] , 计算内存占用 {2}", row, col, Geo.Utils.ByteUtil.GetReadableFileSize(memo));
            ShowInfo(msg);

            GC.Collect();
            DateTime start = DateTime.Now;

            Thread.Sleep(500);


            //double[,] array = new double[row, col];
            if (isFolat)
            {
                Geo.Utils.MatrixUtil.CreateFloat(row, col, 1);
            }
            else
            {
                var array = Geo.Utils.MatrixUtil.Create(row, col, 1);
            }

            var span = DateTime.Now - start;
            ShowInfo("内存分配成功！当前程序占用：" + Geo.Utils.ProcessUtil.GetProcessUsedMemoryString() + ", " + span.TotalMilliseconds + " ms");


            Thread.Sleep(500);
            GC.Collect();

           // MessageBox.Show("内存分配成功！" + msg );

        }




        public void ShowInfo(string msg)
        {
            this.textBox_info.Text = DateTime.Now + "：" + msg  + "\r\n"+ this.textBox_info.Text;
            this.textBox_info.Update();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
