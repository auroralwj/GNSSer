namespace Gnsser.Winform
{
    partial class TaskClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_clean = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_gpsdata = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_viewState = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_controlPort = new System.Windows.Forms.TextBox();
            this.textBox_controlIp = new System.Windows.Forms.TextBox();
            this.textBox_listenPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_state_info = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_run = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_clean
            // 
            this.button_clean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clean.Location = new System.Drawing.Point(543, 152);
            this.button_clean.Margin = new System.Windows.Forms.Padding(4);
            this.button_clean.Name = "button_clean";
            this.button_clean.Size = new System.Drawing.Size(100, 34);
            this.button_clean.TabIndex = 15;
            this.button_clean.Text = "清除显示";
            this.button_clean.UseVisualStyleBackColor = true;
            this.button_clean.Click += new System.EventHandler(this.button_clean_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox_gpsdata);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(13, 81);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(768, 59);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "本地设置";
            // 
            // textBox_gpsdata
            // 
            this.textBox_gpsdata.Location = new System.Drawing.Point(131, 21);
            this.textBox_gpsdata.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_gpsdata.Name = "textBox_gpsdata";
            this.textBox_gpsdata.Size = new System.Drawing.Size(132, 25);
            this.textBox_gpsdata.TabIndex = 1;
            this.textBox_gpsdata.Text = "E:\\";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "工程目录：";
            // 
            // button_viewState
            // 
            this.button_viewState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_viewState.Location = new System.Drawing.Point(660, 150);
            this.button_viewState.Margin = new System.Windows.Forms.Padding(4);
            this.button_viewState.Name = "button_viewState";
            this.button_viewState.Size = new System.Drawing.Size(121, 38);
            this.button_viewState.TabIndex = 13;
            this.button_viewState.Text = "查看运行状态";
            this.button_viewState.UseVisualStyleBackColor = true;
            this.button_viewState.Click += new System.EventHandler(this.button_viewState_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox_controlPort);
            this.groupBox2.Controls.Add(this.textBox_controlIp);
            this.groupBox2.Controls.Add(this.textBox_listenPort);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(768, 59);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "网络设置";
            // 
            // textBox_controlPort
            // 
            this.textBox_controlPort.Location = new System.Drawing.Point(651, 22);
            this.textBox_controlPort.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_controlPort.Name = "textBox_controlPort";
            this.textBox_controlPort.Size = new System.Drawing.Size(79, 25);
            this.textBox_controlPort.TabIndex = 1;
            this.textBox_controlPort.Text = "10001";
            // 
            // textBox_controlIp
            // 
            this.textBox_controlIp.Location = new System.Drawing.Point(376, 22);
            this.textBox_controlIp.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_controlIp.Name = "textBox_controlIp";
            this.textBox_controlIp.Size = new System.Drawing.Size(189, 25);
            this.textBox_controlIp.TabIndex = 1;
            this.textBox_controlIp.Text = "127.0.0.1";
            this.textBox_controlIp.TextChanged += new System.EventHandler(this.textBox_controlIp_TextChanged);
            // 
            // textBox_listenPort
            // 
            this.textBox_listenPort.Location = new System.Drawing.Point(129, 22);
            this.textBox_listenPort.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_listenPort.Name = "textBox_listenPort";
            this.textBox_listenPort.Size = new System.Drawing.Size(132, 25);
            this.textBox_listenPort.TabIndex = 1;
            this.textBox_listenPort.Text = "10000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(579, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "端口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "控制端IP：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "接收端口：";
            // 
            // label_state_info
            // 
            this.label_state_info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_state_info.AutoSize = true;
            this.label_state_info.Location = new System.Drawing.Point(13, 499);
            this.label_state_info.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_state_info.Name = "label_state_info";
            this.label_state_info.Size = new System.Drawing.Size(67, 15);
            this.label_state_info.TabIndex = 11;
            this.label_state_info.Text = "状态信息";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox_info);
            this.groupBox1.Location = new System.Drawing.Point(9, 196);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(760, 254);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // textBox_info
            // 
            this.textBox_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_info.Location = new System.Drawing.Point(4, 22);
            this.textBox_info.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_info.Size = new System.Drawing.Size(752, 228);
            this.textBox_info.TabIndex = 0;
            // 
            // button_stop
            // 
            this.button_stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_stop.Location = new System.Drawing.Point(681, 475);
            this.button_stop.Margin = new System.Windows.Forms.Padding(4);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(100, 35);
            this.button_stop.TabIndex = 9;
            this.button_stop.Text = "停止";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(573, 475);
            this.button_run.Margin = new System.Windows.Forms.Padding(4);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(100, 35);
            this.button_run.TabIndex = 8;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // TaskClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 523);
            this.Controls.Add(this.button_clean);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button_viewState);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label_state_info);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.button_run);
            this.Name = "TaskClientForm";
            this.Text = "任务监听执行器";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_clean;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_gpsdata;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_viewState;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_controlPort;
        private System.Windows.Forms.TextBox textBox_controlIp;
        private System.Windows.Forms.TextBox textBox_listenPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_state_info;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Button button_run;
    }
}