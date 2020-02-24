//2017.09.24, czs, create in hongqing, 多项式拟合
  
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text; 
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting; 

namespace Geo.WinTools
{
    /// <summary>
    /// 多项式拟合
    /// </summary>
    public partial class PolyfitForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PolyfitForm()
        {
            InitializeComponent();
        }
        Geo.Algorithm.LsPolyFit fit;
        double[] ys;
        double[] xs;
        double nextY = 0, nextFitY = 0;
        double errorTimes = 3;
        private void button_ok_Click(object sender, EventArgs e)
        {
            ys = Geo.Utils.DoubleUtil.ParseLines(this.textBox_number.Lines);
            if (ys.Length < 2)
            {
                MessageBox.Show("数据太少了，无法差分！"); return;
            }
            nextY = Double.Parse(textBox_nextY.Text);
            errorTimes = Double.Parse(this.textBox_errorTimes.Text);
            var length = ys.Length;
            xs = new double[ys.Length];
            for (int i = 0; i < ys.Length; i++)
            {
                xs[i] = i;
            }
            var order = int.Parse(this.textBox_order.Text);

            fit = new Geo.Algorithm.LsPolyFit(xs, ys, order); 
            double[] paralist = fit.FitParameters();

            // return new RmsedNumeral(fit.GetY(len + nextIndex), fit.Rms);
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Parameters:");
            //foreach (var item in paralist)
            //{
            //    sb.Append("\t"+item);
            //}
            sb.AppendLine("拟合器信息：");
            sb.AppendLine(fit.ToString());
            sb.AppendLine();

            sb.AppendLine("下一拟合信息：");
            nextFitY = fit.GetY(xs.Length);
            var differ = (nextY - nextFitY);
            sb.AppendLine("NextInputY:\t" + nextY);
            sb.AppendLine("NextFitY:\t" + nextFitY);
            sb.AppendLine("DifferOfY:\t" + differ);
            sb.AppendLine("Times Rms:\t" + differ / fit.StdDev);

            sb.AppendLine();
            //sb.AppendLine("Count:" + length);
            //sb.AppendLine("RMS:" + fit.StdDev);
            //sb.AppendLine("Dy:");
            sb.AppendLine("RmsTimes,\tyi\t-\tfitYi\t=\tdy");
            int count = 0;
            for (int i = 0; i < length; i++)
            {
                var xi = xs[i];
                var yi = ys[i];
                var fitYi = fit.GetY(xi);
                var dy = yi - fitYi;
                var RmsTimes = Math.Abs(dy) / fit.StdDev;
                if (RmsTimes > errorTimes)
                {
                    count++;
                }
                sb.AppendLine(RmsTimes.ToString("0.000") + ",\t" + yi.ToString("0.0000") + "\t-\t" + fitYi.ToString("0.0000") + "\t=\t" + dy.ToString("0.0000"));
            } 
            
            sb.AppendLine();
            sb.AppendLine("Exceed Count:\t" + count);

            this.textBox_out.Text = sb.ToString();
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            if (xs == null) { return; }
            ObjectTableStorage table = new ObjectTableStorage();
            var length = xs.Length;
            for (int i = 0; i < length; i++)
            {
                table.NewRow();

                table.AddItem("x", xs[i]);
                table.AddItem("y", ys[i]);
                table.AddItem("FitY", fit.GetY(xs[i]));
            }

            table.NewRow();
            table.AddItem("x", xs.Length);
            table.AddItem("y", nextY);
            table.AddItem("FitY", nextFitY);

            new Geo.Winform.CommonChartForm(table).Show();
        }

        private void button_differOnce_Click(object sender, EventArgs e)
        {
            ys = Geo.Utils.DoubleUtil.ParseLines(this.textBox_number.Lines);
            if (ys.Length < 2)
            {
                MessageBox.Show("数据太少了，无法差分！"); return;
            }
            ys = Geo.Utils.DoubleUtil.GetDiffer(ys, 1);
            this.textBox_number.Lines = Geo.Utils.DoubleUtil.ToStringLines(ys).ToArray();
        }

        private void button_extractLasttoNext_Click(object sender, EventArgs e)
        {
            ys = Geo.Utils.DoubleUtil.ParseLines(this.textBox_number.Lines);
            if (ys.Length < 2)
            {
                MessageBox.Show("数据太少了，无法提取！"); return;
            }
            var list = new List<double>(ys); 
            this.textBox_nextY.Text =  list.Last().ToString();
            list.RemoveAt(list.Count -1);

            ys = list.ToArray();
            this.textBox_number.Lines = Geo.Utils.DoubleUtil.ToStringLines(ys).ToArray();
        }
    }
}
