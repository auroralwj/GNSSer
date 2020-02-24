namespace Geo.Winform.Tools
{
    partial class BarCodeGeneratorForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_notice = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label_barCodeInfo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_scope2End = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_scope2Step = new System.Windows.Forms.TextBox();
            this.textBox_scope1Step = new System.Windows.Forms.TextBox();
            this.textBox_scope1End = new System.Windows.Forms.TextBox();
            this.textBox_scope2Start = new System.Windows.Forms.TextBox();
            this.textBox_char2 = new System.Windows.Forms.TextBox();
            this.textBox_char3 = new System.Windows.Forms.TextBox();
            this.textBox_scope1Start = new System.Windows.Forms.TextBox();
            this.textBox_char1 = new System.Windows.Forms.TextBox();
            this.textBox_unitBarCode = new System.Windows.Forms.TextBox();
            this.button_gen = new System.Windows.Forms.Button();
            this.button_export = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_view = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_results = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label_notice);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label_barCodeInfo);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.textBox_scope2End);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.textBox_scope2Step);
            this.groupBox3.Controls.Add(this.textBox_scope1Step);
            this.groupBox3.Controls.Add(this.textBox_scope1End);
            this.groupBox3.Controls.Add(this.textBox_scope2Start);
            this.groupBox3.Controls.Add(this.textBox_char2);
            this.groupBox3.Controls.Add(this.textBox_char3);
            this.groupBox3.Controls.Add(this.textBox_scope1Start);
            this.groupBox3.Controls.Add(this.textBox_char1);
            this.groupBox3.Controls.Add(this.textBox_unitBarCode);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(507, 125);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "编号规则";
            // 
            // label_notice
            // 
            this.label_notice.AutoSize = true;
            this.label_notice.ForeColor = System.Drawing.Color.Red;
            this.label_notice.Location = new System.Drawing.Point(301, 110);
            this.label_notice.Name = "label_notice";
            this.label_notice.Size = new System.Drawing.Size(0, 12);
            this.label_notice.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(337, 75);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 3;
            this.label12.Text = "步长：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(155, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 3;
            this.label10.Text = "步长：";
            // 
            // label_barCodeInfo
            // 
            this.label_barCodeInfo.AutoSize = true;
            this.label_barCodeInfo.Location = new System.Drawing.Point(19, 105);
            this.label_barCodeInfo.Name = "label_barCodeInfo";
            this.label_barCodeInfo.Size = new System.Drawing.Size(41, 12);
            this.label_barCodeInfo.TabIndex = 3;
            this.label_barCodeInfo.Text = "示例：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(330, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "数字范围";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(357, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "到";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(155, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "数字范围";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "从";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(179, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "到";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(123, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "从";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(253, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "字母";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(437, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "字母/数字";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "字母/数字";
            // 
            // textBox_scope2End
            // 
            this.textBox_scope2End.Location = new System.Drawing.Point(383, 47);
            this.textBox_scope2End.Name = "textBox_scope2End";
            this.textBox_scope2End.Size = new System.Drawing.Size(52, 21);
            this.textBox_scope2End.TabIndex = 1;
            this.textBox_scope2End.Text = "00010";
            this.textBox_scope2End.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "单位简称";
            // 
            // textBox_scope2Step
            // 
            this.textBox_scope2Step.Location = new System.Drawing.Point(383, 71);
            this.textBox_scope2Step.Name = "textBox_scope2Step";
            this.textBox_scope2Step.Size = new System.Drawing.Size(33, 21);
            this.textBox_scope2Step.TabIndex = 1;
            this.textBox_scope2Step.Text = "1";
            // 
            // textBox_scope1Step
            // 
            this.textBox_scope1Step.Location = new System.Drawing.Point(201, 71);
            this.textBox_scope1Step.Name = "textBox_scope1Step";
            this.textBox_scope1Step.Size = new System.Drawing.Size(33, 21);
            this.textBox_scope1Step.TabIndex = 1;
            this.textBox_scope1Step.Text = "1";
            // 
            // textBox_scope1End
            // 
            this.textBox_scope1End.Location = new System.Drawing.Point(201, 47);
            this.textBox_scope1End.Name = "textBox_scope1End";
            this.textBox_scope1End.Size = new System.Drawing.Size(33, 21);
            this.textBox_scope1End.TabIndex = 1;
            this.textBox_scope1End.Text = "100";
            this.textBox_scope1End.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // textBox_scope2Start
            // 
            this.textBox_scope2Start.Location = new System.Drawing.Point(306, 47);
            this.textBox_scope2Start.Name = "textBox_scope2Start";
            this.textBox_scope2Start.Size = new System.Drawing.Size(45, 21);
            this.textBox_scope2Start.TabIndex = 1;
            this.textBox_scope2Start.Text = "00001";
            this.textBox_scope2Start.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // textBox_char2
            // 
            this.textBox_char2.Location = new System.Drawing.Point(250, 47);
            this.textBox_char2.Name = "textBox_char2";
            this.textBox_char2.Size = new System.Drawing.Size(28, 21);
            this.textBox_char2.TabIndex = 1;
            this.textBox_char2.Text = "A";
            this.textBox_char2.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // textBox_char3
            // 
            this.textBox_char3.Location = new System.Drawing.Point(448, 47);
            this.textBox_char3.Name = "textBox_char3";
            this.textBox_char3.Size = new System.Drawing.Size(28, 21);
            this.textBox_char3.TabIndex = 1;
            this.textBox_char3.Text = "1";
            this.textBox_char3.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // textBox_scope1Start
            // 
            this.textBox_scope1Start.Location = new System.Drawing.Point(143, 47);
            this.textBox_scope1Start.Name = "textBox_scope1Start";
            this.textBox_scope1Start.Size = new System.Drawing.Size(33, 21);
            this.textBox_scope1Start.TabIndex = 1;
            this.textBox_scope1Start.Text = "001";
            this.textBox_scope1Start.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // textBox_char1
            // 
            this.textBox_char1.Location = new System.Drawing.Point(80, 47);
            this.textBox_char1.Name = "textBox_char1";
            this.textBox_char1.Size = new System.Drawing.Size(28, 21);
            this.textBox_char1.TabIndex = 1;
            this.textBox_char1.Text = "A";
            this.textBox_char1.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // textBox_unitBarCode
            // 
            this.textBox_unitBarCode.Location = new System.Drawing.Point(17, 47);
            this.textBox_unitBarCode.Name = "textBox_unitBarCode";
            this.textBox_unitBarCode.Size = new System.Drawing.Size(39, 21);
            this.textBox_unitBarCode.TabIndex = 1;
            this.textBox_unitBarCode.Text = "ZCYC";
            this.textBox_unitBarCode.TextChanged += new System.EventHandler(this.textBox_unitBarCode_TextChanged);
            // 
            // button_gen
            // 
            this.button_gen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_gen.Location = new System.Drawing.Point(279, 148);
            this.button_gen.Name = "button_gen";
            this.button_gen.Size = new System.Drawing.Size(75, 23);
            this.button_gen.TabIndex = 18;
            this.button_gen.Text = "生成";
            this.button_gen.UseVisualStyleBackColor = true;
            this.button_gen.Click += new System.EventHandler(this.button_gen_Click);
            // 
            // button_export
            // 
            this.button_export.Location = new System.Drawing.Point(12, 148);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(75, 23);
            this.button_export.TabIndex = 19;
            this.button_export.Text = "文本导出";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(441, 148);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 20;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_view
            // 
            this.button_view.Location = new System.Drawing.Point(93, 148);
            this.button_view.Name = "button_view";
            this.button_view.Size = new System.Drawing.Size(75, 23);
            this.button_view.TabIndex = 21;
            this.button_view.Text = "展开 >>";
            this.button_view.UseVisualStyleBackColor = true;
            this.button_view.Click += new System.EventHandler(this.button_view_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(360, 148);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 22;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_results
            // 
            this.textBox_results.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_results.Location = new System.Drawing.Point(13, 198);
            this.textBox_results.Multiline = true;
            this.textBox_results.Name = "textBox_results";
            this.textBox_results.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_results.Size = new System.Drawing.Size(506, 6);
            this.textBox_results.TabIndex = 23;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "编号.txt";
            this.saveFileDialog1.Filter = "文本文件|*.txt";
            // 
            // BarCodeGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 191);
            this.Controls.Add(this.textBox_results);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_view);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_export);
            this.Controls.Add(this.button_gen);
            this.Controls.Add(this.groupBox3);
            this.Name = "BarCodeGeneratorForm";
            this.Text = "编码生成器";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_barCodeInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_scope1End;
        private System.Windows.Forms.TextBox textBox_char2;
        private System.Windows.Forms.TextBox textBox_scope1Start;
        private System.Windows.Forms.TextBox textBox_char1;
        private System.Windows.Forms.TextBox textBox_unitBarCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_scope2End;
        private System.Windows.Forms.TextBox textBox_scope2Start;
        private System.Windows.Forms.Button button_gen;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TextBox textBox_char3;
        private System.Windows.Forms.Button button_view;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_results;
        private System.Windows.Forms.Label label_notice;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_scope1Step;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_scope2Step;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}