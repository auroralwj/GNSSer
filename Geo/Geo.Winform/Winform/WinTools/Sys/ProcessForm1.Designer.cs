namespace Geo.WinTools.Sys
{
    partial class ProcessForm1
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
            this.textBox_processes = new System.Windows.Forms.TextBox();
            this.textBox_service = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_service);
            this.groupBox1.Controls.Add(this.textBox_processes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(864, 509);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "本机进程";
            // 
            // textBox_processes
            // 
            this.textBox_processes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_processes.Location = new System.Drawing.Point(3, 17);
            this.textBox_processes.Multiline = true;
            this.textBox_processes.Name = "textBox_processes";
            this.textBox_processes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_processes.Size = new System.Drawing.Size(428, 480);
            this.textBox_processes.TabIndex = 0;
            // 
            // textBox_service
            // 
            this.textBox_service.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_service.Location = new System.Drawing.Point(437, 17);
            this.textBox_service.Multiline = true;
            this.textBox_service.Name = "textBox_service";
            this.textBox_service.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_service.Size = new System.Drawing.Size(415, 480);
            this.textBox_service.TabIndex = 0;
            // 
            // ProcessForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 509);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProcessForm1";
            this.Text = "进程查看";
            this.Load += new System.EventHandler(this.ProcessForm1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_processes;
        private System.Windows.Forms.TextBox textBox_service;
    }
}