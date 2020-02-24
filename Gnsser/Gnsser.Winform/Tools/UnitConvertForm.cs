//2018.08.15, czs, create in hmx, 单位换算

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Gnsser;

namespace Gnsser.Winform
{
    public partial class UnitConvertForm : Form
    {
        public UnitConvertForm()
        {
            InitializeComponent();
        }

        private void button_exchange_Click(object sender, EventArgs e)
        {
            var temp = this.richTextBoxControl_left.Text;
            this.richTextBoxControl_left.Text = this.richTextBoxControl_right.Text;
            this.richTextBoxControl_right.Text = temp;
        }


        /// <summary>
        /// 是否反向计算
        /// </summary>
        bool IsReverse { get => this.checkBox_inverse.Checked; }

        private void button_nsTom_Click(object sender, EventArgs e)
        {
            Convert(new Func<double, double>(input=>
            input * GnssConst.MeterPerNano
            ));
        }
        private void button_mToNs_Click(object sender, EventArgs e)
        {
            Convert(new Func<double, double>(input =>
            input / GnssConst.MeterPerNano
            ));
        }

        private void Convert(Func<double, double> ConvertFunc)
        {
            var valLeft = GetDoubleInput();

            List<double> reslut = new List<double>();
            foreach (var input in valLeft)
            {
                double val = ConvertFunc(input);

                reslut.Add(val);
            } 
            SetValue(reslut);
        }
         


        #region 工具
        private void SetValue(List<double> vals)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in vals)
            {
                sb.AppendLine(item.ToString());
            }
            if (IsReverse)
            {
                this.richTextBoxControl_left.Text = sb.ToString();
            }
            else
            {
                this.richTextBoxControl_right.Text = sb.ToString();
            }
        }

        private List<double> GetDoubleInput()
        {
            List<double> list = new List<double>();
            try
            {
                var lines = this.richTextBoxControl_left.Lines;
                if (IsReverse)
                {                    
                    lines = this.richTextBoxControl_right.Lines;
                }

                foreach (var item in lines)
                {
                    if (String.IsNullOrWhiteSpace(item)) { continue; }

                    var valLeft = Double.Parse(item);
                    list.Add(valLeft);
                }
                 
                return list;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return list;
            }
        }
        #endregion

    }
}
