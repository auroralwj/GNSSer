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
    /// 时间计算
    /// </summary>
    public partial class TimeCaculatorForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TimeCaculatorForm()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            string timeStr = this.textBox_code.Text;
            string[] strs = timeStr.Split(new char[] { ' ', '-', ':' }, StringSplitOptions.RemoveEmptyEntries);
            //int i = 0;

            //if (strs.Length == 3)//时 分 秒
            //{
                //int hour = int.Parse(strs[i++]);
                //int min = int.Parse(strs[i++]);
                //double sec = double.Parse(strs[i++]);

                TimeSpan span = TimeSpan.Parse(timeStr);// new TimeSpan(hour, min, sec,);
                string result = "总共： " + span.TotalMinutes + " 分" + "\r\n";
                result += "即： " + span.TotalSeconds + " 秒" + "\r\n";
                this.textBox_result.Text = result;
            //} 
            //if (strs.Length == 2)//时 分 秒
            //{
            //    int min = int.Parse(strs[i++]);
            //    int sec = int.Parse(strs[i++]);

            //    TimeSpan span = new TimeSpan(0, min, sec);
            //    string result = "总共： " + span.TotalMinutes + " 分" + "\r\n";
            //    result += "即： " + span.TotalSeconds + " 秒" + "\r\n";
            //    this.textBox_result.Text = result;
            //}
        }

        private void button_cancel_Click(object sender, EventArgs e) { this.Close(); }
    }
}
