using System;
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
    public partial class GnssTimeForm : Form
    {
        public GnssTimeForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            Time time = new Time(this.dateTimePicker1.Value);

            String str = "GPS Week:\t" + time.GpsWeek + "\r\n";
            str += "DayOfWeek:\t" + (int)time.DayOfWeek + "\r\n";
            str += "SecondsOfWeek:\t" + (int)time.SecondsOfWeek + "\r\n";
            str += "SecondsOfDay:\t" + (int)time.SecondsOfDay + "\r\n";
            str += "DayOfYear:\t" + time.DayOfYear + "\r\n";
            str += "MJulianDays:\t" + time.MJulianDays + "\r\n";
            str += "JulianDays:\t" + time.JulianDays + "\r\n";
            
            
            
            this.textBox1.Text = str;
        }
    }
}
