using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Times; 
namespace Gnsser.Winform
{
    public partial class GpsTimeForm : Form
    {
        public GpsTimeForm()
        {
            InitializeComponent();
        }

        private void button_caculate_Click(object sender, EventArgs e)
        {
            DateTime dateTime = this.dateTimePicker1.Value;
            Time gpsTime = new Time(dateTime);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("时间：" + gpsTime.ToString());
            sb.AppendLine("年积日：" + gpsTime.DayOfYear);
            sb.AppendLine("GPS周：" + gpsTime.GpsWeek);
            sb.AppendLine("周天：" + (int)gpsTime.DayOfWeek);
            sb.AppendLine("儒略日：" + gpsTime.JulianDay);
            sb.AppendLine("平儒略日：" + gpsTime.MJulianDays);
            sb.AppendLine("秒：" + gpsTime.Seconds);

            this.textBox1.Text = sb.ToString();
        }

        private void button_cacuSpan_Click(object sender, EventArgs e)
        {
            DateTime dateTime = this.dateTimePicker_from.Value;
            DateTime to = this.dateTimePicker_to.Value;

            TimeSpan span = to - dateTime;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("日：" + span.Days);
            sb.AppendLine("累计日：" + span.TotalDays);
            sb.AppendLine("累计时：" + span.TotalHours);
            sb.AppendLine("累计秒：" + span.TotalSeconds);

            this.textBox1.Text = sb.ToString();
        }
    }
}
