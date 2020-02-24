//2017.11.22, czs, create in hongqing, 经纬仪度盘配置

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Gnsser.Winform.Suvey
{
    public partial class InitConfigAngleTheodoliteForm : Form
    {
        public InitConfigAngleTheodoliteForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            var isJ1 = this.radioButton_j1.Checked;
            var round = namedIntControl_round.Value;

            StringBuilder sb = new StringBuilder();
            if (isJ1)
            {
                for (int i = 1; i <= round; i++)
                {
                    var seconds = 180.0 * 3600.0 * (i - 1.0) / round + 4 * 60.0 * (i - 1) + 2.0 * 60.0 * ( i - 0.5 ) / round;
                    var val = new DMS(seconds, true);
                    sb.AppendLine( i + "\t" +  val.ToReadableDms());
                } 
            }else
            {
                for (int i = 1; i <= round; i++)
                {
                    var seconds = 180.0 * 3600.0 * (i - 1.0) / round + 10 * 60.0 * (i - 1) + 10.0 * 60.0 * (i - 0.5) / round;
                    var val = new DMS(seconds, true);
                    sb.AppendLine(i + "\t" + val.ToReadableDms());
                } 
            }

            this.richTextBoxControl1.Text = sb.ToString();
        }
    }
}
