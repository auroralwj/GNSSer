using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Geo.WinTools.Sys
{
    /// <summary>
    /// 时间转换
    /// </summary>
    public partial class TimeConvertForm1 : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TimeConvertForm1()
        {
            InitializeComponent();

            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd hh:mm:ss";


            //long sn = 1282649504749L;//一个时间
            //DateTime time = new DateTime(sn, DateTimeKind.Utc);
            //DateTime now = DateTime.Now;
            //DateTime timeBegin = new DateTime(1970, 1, 1);
            //sn = (now.Ticks - timeBegin.Ticks) / 10000;

        }
        private void button1_utcToAll_Click(object sender, EventArgs e)
        {
            DateTime timeBegin = new DateTime(1970, 1, 1, 8, 0, 0);
            long sn = long.Parse(this.textBox_utcSeconds.Text.Trim());
            DateTime time = new DateTime(sn * 10000 + timeBegin.Ticks, DateTimeKind.Utc);
            this.dateTimePicker1.Value = time;
        }
        private void button_AllToUtc_Click(object sender, EventArgs e)
        {
            DateTime timeBegin = new DateTime(1970, 1, 1);
            long sn= (this.dateTimePicker1.Value.Ticks - timeBegin.Ticks) / 10000;
            this.textBox_utcSeconds.Text = sn.ToString();
            
        }


    }
}