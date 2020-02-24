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
    public partial class CaculateAntenaHeightForm : Form
    {
        public CaculateAntenaHeightForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            double slantHeight = double.Parse(this.textBox_slantheight.Text);//斜高
            double antDiameter = double.Parse(this.textBox_antdiameter.Text);//直径
            var phaseHeight = namedFloatControl_phaseHeight.GetValue();
            int fractionCount = namedIntControl_fractionCount.GetValue();

            //勾股定理
            var radius = antDiameter / 2.0; //半径
            var qrt = slantHeight * slantHeight - radius * radius;
            //  var qrt = slantHeight * slantHeight + antDiameter * antDiameter;
            double heigth = Math.Sqrt(qrt); //正高
            double result = Math.Round(heigth + phaseHeight, fractionCount);
            //反算

            //var h = Math.Cos(Math.Asin(radius / hegith)) * hegith;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("天线相位中心高：");
            sb.AppendLine(result.ToString());

            //求斜高与相位同高的高度
            double sameHeight = (phaseHeight * phaseHeight + radius * radius) / (2 * phaseHeight);

            sb.AppendLine("等斜高：");
            sb.AppendLine(sameHeight.ToString());


            this.textBox_out.Text = sb.ToString();
        }
    }
}
