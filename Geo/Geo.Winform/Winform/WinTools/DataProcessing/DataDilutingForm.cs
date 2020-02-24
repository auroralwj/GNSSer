//2014.11.14, czs, create in namu, 数据按行提取/稀疏。

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
    /// 数据按行提取/稀疏。
    /// </summary>
    public class DataDilutingForm : DataProcessingForm
    {
        private TextBox textBox_interval;
        private CheckBox checkBox_ignoreEmptyLine;
        private Label label1;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataDilutingForm()
            : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 运行
        /// </summary>
        protected override void Run()
        {
            int interval = int.Parse(this.textBox_interval.Text);
            int count = this.InputLines.Length;
            bool ignoreEmptyLine = checkBox_ignoreEmptyLine.Checked;
            List<String> results = new List<string>();
            int i = 0;
            foreach (var item in this.InputLines)
            {
                if( item.Trim() =="" &&  ignoreEmptyLine ) {  i++; continue;}

                if (i % interval == 0)
                    results.Add(item);
                i++;
            }

            this.OutputLines = results.ToArray();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.checkBox_ignoreEmptyLine = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout(); 
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox_ignoreEmptyLine);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox_interval);
            this.panel1.Location = new System.Drawing.Point(2, 182);
            this.panel1.Size = new System.Drawing.Size(200, 170); 
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "间隔行数：";
            // 
            // textBox_interval
            // 
            this.textBox_interval.Location = new System.Drawing.Point(85, 14);
            this.textBox_interval.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(56, 21);
            this.textBox_interval.TabIndex = 2;
            this.textBox_interval.Text = "10";
            // 
            // checkBox_ignoreEmptyLine
            // 
            this.checkBox_ignoreEmptyLine.AutoSize = true;
            this.checkBox_ignoreEmptyLine.Location = new System.Drawing.Point(85, 48);
            this.checkBox_ignoreEmptyLine.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_ignoreEmptyLine.Name = "checkBox_ignoreEmptyLine";
            this.checkBox_ignoreEmptyLine.Size = new System.Drawing.Size(72, 16);
            this.checkBox_ignoreEmptyLine.TabIndex = 3;
            this.checkBox_ignoreEmptyLine.Text = "忽略空行";
            this.checkBox_ignoreEmptyLine.UseVisualStyleBackColor = true;
            // 
            // DataDilutingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(506, 373);
            this.InputLines = new string[0];
            this.Name = "DataDilutingForm";
            this.OutputLines = new string[0];
            this.Text = "数据按行提取/稀疏";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout(); 
            this.ResumeLayout(false);

        }
    }
}
