namespace Geo.WinTools.Sys
{
    partial class TimeConvertForm1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1_utcToAll = new System.Windows.Forms.Button();
            this.button_AllToUtc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_utcSeconds = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_utcSeconds);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "UTC19700101";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Location = new System.Drawing.Point(318, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "时间";
            // 
            // button1_utcToAll
            // 
            this.button1_utcToAll.Location = new System.Drawing.Point(90, 162);
            this.button1_utcToAll.Name = "button1_utcToAll";
            this.button1_utcToAll.Size = new System.Drawing.Size(75, 23);
            this.button1_utcToAll.TabIndex = 1;
            this.button1_utcToAll.Text = "=>";
            this.button1_utcToAll.UseVisualStyleBackColor = true;
            this.button1_utcToAll.Click += new System.EventHandler(this.button1_utcToAll_Click);
            // 
            // button_AllToUtc
            // 
            this.button_AllToUtc.Location = new System.Drawing.Point(417, 165);
            this.button_AllToUtc.Name = "button_AllToUtc";
            this.button_AllToUtc.Size = new System.Drawing.Size(75, 23);
            this.button_AllToUtc.TabIndex = 2;
            this.button_AllToUtc.Text = "<=";
            this.button_AllToUtc.UseVisualStyleBackColor = true;
            this.button_AllToUtc.Click += new System.EventHandler(this.button_AllToUtc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "UTC:";
            // 
            // textBox_utcSeconds
            // 
            this.textBox_utcSeconds.Location = new System.Drawing.Point(67, 24);
            this.textBox_utcSeconds.Name = "textBox_utcSeconds";
            this.textBox_utcSeconds.Size = new System.Drawing.Size(135, 21);
            this.textBox_utcSeconds.TabIndex = 1;
            this.textBox_utcSeconds.Text = "1282649504749";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "秒";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(35, 27);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(217, 21);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // TimeConvertForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 321);
            this.Controls.Add(this.button_AllToUtc);
            this.Controls.Add(this.button1_utcToAll);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TimeConvertForm1";
            this.Text = "时间转换器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_utcSeconds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1_utcToAll;
        private System.Windows.Forms.Button button_AllToUtc;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}