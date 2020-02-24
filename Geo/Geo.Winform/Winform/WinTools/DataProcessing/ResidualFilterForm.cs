using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;
using Geo.Algorithm.Adjust;

namespace Geo.WinTools
{
    /// <summary>
    /// 残差分析。
    /// </summary>
    public class ResidualFilterForm : DataProcessingForm
    {
        private TextBox textBox_upperThreshold;
        private Label label1;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResidualFilterForm()
            : base()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 运行
        /// </summary>
        protected override void Run()
        {
            Double upperThreshold = Double.Parse(this.textBox_upperThreshold.Text);
            int count = this.InputLines.Length; 

            if (HasIndexColumn)
            {
                List<double[]> doubles = Utils.DoubleUtil.ParseTable(this.InputLines); 
                Dictionary<Double, Double> dic = DoubleUtil.ToDic(doubles);


                DicGrossErrorFilter filter = new DicGrossErrorFilter(dic);
                Dictionary<Double, Double>  results = filter.Filter(upperThreshold);

                this.OutputLines = DoubleUtil.ToStringLines(results).ToArray();
            }
            else
            {
                double[] doubles = Utils.DoubleUtil.ParseLines(this.InputLines);
                GrossErrorFilter filter = new GrossErrorFilter(doubles);
                List<Double> results = filter.Filter(upperThreshold); 

                this.OutputLines = DoubleUtil.ToStringLines(results).ToArray();
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_upperThreshold = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox_upperThreshold);
            this.panel1.Location = new System.Drawing.Point(2, 210);
            this.panel1.Size = new System.Drawing.Size(200, 142);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "阈值(RMS倍数)：";
            // 
            // textBox_upperThreshold
            // 
            this.textBox_upperThreshold.Location = new System.Drawing.Point(112, 12);
            this.textBox_upperThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_upperThreshold.Name = "textBox_upperThreshold";
            this.textBox_upperThreshold.Size = new System.Drawing.Size(56, 21);
            this.textBox_upperThreshold.TabIndex = 2;
            this.textBox_upperThreshold.Text = "3";
            // 
            // ResidualFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(506, 373);
            this.InputLines = new string[0];
            this.Name = "ResidualFilterForm";
            this.OutputLines = new string[0];
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
