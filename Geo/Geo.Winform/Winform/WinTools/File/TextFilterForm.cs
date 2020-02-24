using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.WinTools
{
    /// <summary>
    /// 文本过滤器
    /// </summary>
    public class TextFilterForm : FileExecutingForm 
    {
        private GroupBox groupBox2;
        private Label label1;
        private TextBox textBox_interval;
    
        /// <summary>
        /// 构造函数
        /// </summary>
        public TextFilterForm():base()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public override void Run(string input, string output)
        {
            int interval = int.Parse(this.textBox_interval.Text);

            var inputLines = this.GetInputLines();
            int startRowIndex = 0;
            if (this.IsIgnoreFirstRow)
            {
                this.AppendToOutFile(inputLines[0] + "\r\n");
                startRowIndex = 1;
            }
            int len = inputLines.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = startRowIndex; i < len; i++)
            {
                if((i-startRowIndex) % interval == 0)
                {
                    sb.AppendLine(inputLines[i]);
                }
            }
            this.AppendToOutFile(sb.ToString());

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(System.IO.Path.GetDirectoryName(OutputPath));

           Utils.FormUtil.ShowOkAndOpenFile(this.OutputPath);

   //         List<double[]> doubles = GetInputDoubleRows();

   //         string [] strings = GetInputLines();
   //         int len = doubles.Count; 

   //         for (int i = 0; i < len; i++)
			//{ 
   //             double first = (doubles[i][0]); 

   //             if (first % interval == 0)
   //             { 
   //                 this.AppendToOutFile( DoubleUtil.ToTabString(doubles[i], 2) + "\r\n");
   //             }
			//} 
          
        }

        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox_interval);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(19, 198);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(618, 57);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置";
            // 
            // textBox_interval
            // 
            this.textBox_interval.Location = new System.Drawing.Point(124, 25);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(100, 25);
            this.textBox_interval.TabIndex = 1;
            this.textBox_interval.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "首列整除数：";
            // 
            // TextFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.ClientSize = new System.Drawing.Size(650, 280);
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TextFilterForm";
            this.Text = "过滤";
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    } 






}
