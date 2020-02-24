//2014.12.27, lh, create in 郑州, TEQC 互操作

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class BATScreeningForm : Form
    {
        public BATScreeningForm()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            try
            {
                BATScreeningParameter.Percent = Convert.ToDouble(this.textBox_percent.Text);
                BATScreeningParameter.Mp1 = Convert.ToDouble(this.textBox_mp1.Text);
                BATScreeningParameter.Mp2 = Convert.ToDouble(this.textBox_mp2.Text);
                BATScreeningParameter.O_slps = Convert.ToDouble(this.textBox_o_slps.Text);
                BATScreeningParameter.invert_selection = this.checkBox_invert.Checked;
                BATScreeningParameter.percent_selection = !this.checkBox_withoutpercent.Checked;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
