namespace Geo.WinTools.Sys
{
    partial class PcInfoForm1
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_sysDir = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label2_sysTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_computerName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_osVersion = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label_cpu = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "系统当前时间：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_computerName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label_osVersion);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label_sysDir);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label2_sysTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(579, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "计算机系统信息";
            // 
            // label_sysDir
            // 
            this.label_sysDir.AutoSize = true;
            this.label_sysDir.Location = new System.Drawing.Point(89, 61);
            this.label_sysDir.Name = "label_sysDir";
            this.label_sysDir.Size = new System.Drawing.Size(41, 12);
            this.label_sysDir.TabIndex = 3;
            this.label_sysDir.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "系统目录：";
            // 
            // label2_sysTime
            // 
            this.label2_sysTime.AutoSize = true;
            this.label2_sysTime.Location = new System.Drawing.Point(113, 17);
            this.label2_sysTime.Name = "label2_sysTime";
            this.label2_sysTime.Size = new System.Drawing.Size(41, 12);
            this.label2_sysTime.TabIndex = 1;
            this.label2_sysTime.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "计算机名称：";
            // 
            // label_computerName
            // 
            this.label_computerName.Location = new System.Drawing.Point(89, 38);
            this.label_computerName.Name = "label_computerName";
            this.label_computerName.Size = new System.Drawing.Size(41, 12);
            this.label_computerName.TabIndex = 3;
            this.label_computerName.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(225, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "操作系统：";
            // 
            // label_osVersion
            // 
            this.label_osVersion.AutoSize = true;
            this.label_osVersion.Location = new System.Drawing.Point(297, 38);
            this.label_osVersion.Name = "label_osVersion";
            this.label_osVersion.Size = new System.Drawing.Size(41, 12);
            this.label_osVersion.TabIndex = 3;
            this.label_osVersion.Text = "label3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_cpu);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(31, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(431, 224);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "硬件信息";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "CPU编号：";
            // 
            // label_cpu
            // 
            this.label_cpu.AutoSize = true;
            this.label_cpu.Location = new System.Drawing.Point(76, 32);
            this.label_cpu.Name = "label_cpu";
            this.label_cpu.Size = new System.Drawing.Size(41, 12);
            this.label_cpu.TabIndex = 1;
            this.label_cpu.Text = "label6";
            // 
            // PcInfoForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 595);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PcInfoForm1";
            this.Text = "计算机信息";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_sysDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label2_sysTime;
        private System.Windows.Forms.Label label_computerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_osVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_cpu;
        private System.Windows.Forms.Label label5;
    }
}