namespace Geo.WinTools.Net
{
    partial class ScanIpOpenPortsForm1
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
            this.button_Go = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_hostNameOrAddress = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_OutPut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_StartPort = new System.Windows.Forms.TextBox();
            this.textBox_EndPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Go);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_EndPort);
            this.groupBox1.Controls.Add(this.textBox_StartPort);
            this.groupBox1.Controls.Add(this.textBox_hostNameOrAddress);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // button_Go
            // 
            this.button_Go.Location = new System.Drawing.Point(329, 62);
            this.button_Go.Name = "button_Go";
            this.button_Go.Size = new System.Drawing.Size(75, 23);
            this.button_Go.TabIndex = 2;
            this.button_Go.Text = "扫描";
            this.button_Go.UseVisualStyleBackColor = true;
            this.button_Go.Click += new System.EventHandler(this.button_Go_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP地址：";
            // 
            // textBox_hostNameOrAddress
            // 
            this.textBox_hostNameOrAddress.Location = new System.Drawing.Point(70, 64);
            this.textBox_hostNameOrAddress.Name = "textBox_hostNameOrAddress";
            this.textBox_hostNameOrAddress.Size = new System.Drawing.Size(251, 21);
            this.textBox_hostNameOrAddress.TabIndex = 0;
            this.textBox_hostNameOrAddress.Text = "127.0.0.1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_OutPut);
            this.groupBox2.Location = new System.Drawing.Point(12, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 192);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "开放的端口";
            // 
            // textBox_OutPut
            // 
            this.textBox_OutPut.Location = new System.Drawing.Point(6, 19);
            this.textBox_OutPut.Multiline = true;
            this.textBox_OutPut.Name = "textBox_OutPut";
            this.textBox_OutPut.Size = new System.Drawing.Size(404, 167);
            this.textBox_OutPut.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "起始端口：";
            // 
            // textBox_StartPort
            // 
            this.textBox_StartPort.Location = new System.Drawing.Point(78, 23);
            this.textBox_StartPort.Name = "textBox_StartPort";
            this.textBox_StartPort.Size = new System.Drawing.Size(83, 21);
            this.textBox_StartPort.TabIndex = 0;
            this.textBox_StartPort.Text = "70";
            // 
            // textBox_EndPort
            // 
            this.textBox_EndPort.Location = new System.Drawing.Point(238, 20);
            this.textBox_EndPort.Name = "textBox_EndPort";
            this.textBox_EndPort.Size = new System.Drawing.Size(83, 21);
            this.textBox_EndPort.TabIndex = 0;
            this.textBox_EndPort.Text = "300";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(167, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "结束端口：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(18, 313);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(410, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // ScanIpOpenPortsForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 348);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanIpOpenPortsForm1";
            this.ShowIcon = false;
            this.Text = "扫描端口";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_Go;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_hostNameOrAddress;
        private System.Windows.Forms.TextBox textBox_OutPut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_EndPort;
        private System.Windows.Forms.TextBox textBox_StartPort;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}