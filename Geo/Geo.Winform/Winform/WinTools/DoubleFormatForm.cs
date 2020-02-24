using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.WinTools
{
    /// <summary>
    /// 双精度格式化
    /// </summary>
    public partial class DoubleFormatForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DoubleFormatForm()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            double val = double.Parse(this.textBox_number.Text);
            string format = this.textBox_format.Text.Trim();
            if (format == "")
                this.textBox_out.Text = val.ToString();
            else
                this.textBox_out.Text = val.ToString(format);
        }
    }
}
