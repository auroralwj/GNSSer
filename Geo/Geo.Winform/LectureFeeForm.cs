//2019.04.22, czs, Lecture Fee, 讲课费

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo
{
    public partial class LectureFeeForm : Form
    {
        public LectureFeeForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            var stardard = this.namedIntControl_stardard.Value;
            var times = this.namedIntControl_times.Value;
            //所得
            double fee = stardard * times;

            double step1Fee = namedFloatControl_step1Fee.Value;
            double step2Fee = namedFloatControl_step2Fee.Value;
            double step1TaxRatio = this.namedFloatControl1_step1Tax.Value;
            double step2TaxRatio = namedFloatControl1_step2Tax.Value;

            double tax = 0;
            double taxRatio = 0;

            if(fee > step1Fee && fee <= step2Fee)
            {
                taxRatio = step1TaxRatio;
                tax = (fee - step1Fee) * taxRatio / 100.0;
            }
            if(fee > step2Fee)
            { 
                taxRatio = step2TaxRatio;
                tax = fee * taxRatio / 100.0;
            }
             

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("税后：  " + Round(fee).ToString("0.00"));
            sb.AppendLine("税额：  " + Round(tax).ToString("0.00"));
            var total = (tax + fee);
            sb.AppendLine("总金额：  " + Round(total).ToString("0.00"));


            richTextBoxControl_result.Text = sb.ToString();
        }

        public static double Round(double val)
        {
            return Math.Round(val, 2);
        }
    }
}
