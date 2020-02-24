namespace Gnsser.Winform
{
    partial class MutiGnssOptionPage
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_receiver = new System.Windows.Forms.TabPage();
            this.namedStringControl_satStdDev = new Geo.Winform.Controls.NamedStringControl();
            this.namedStringControl_sysStdDev = new Geo.Winform.Controls.NamedStringControl();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.checkBox_isSameTimeSystem = new System.Windows.Forms.CheckBox();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage_receiver.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_receiver);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_receiver
            // 
            this.tabPage_receiver.Controls.Add(this.namedStringControl_satStdDev);
            this.tabPage_receiver.Controls.Add(this.namedStringControl_sysStdDev);
            this.tabPage_receiver.Controls.Add(this.multiGnssSystemSelectControl1);
            this.tabPage_receiver.Controls.Add(this.checkBox_isSameTimeSystem);
            this.tabPage_receiver.Location = new System.Drawing.Point(4, 22);
            this.tabPage_receiver.Name = "tabPage_receiver";
            this.tabPage_receiver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_receiver.Size = new System.Drawing.Size(694, 500);
            this.tabPage_receiver.TabIndex = 7;
            this.tabPage_receiver.Text = "多频多系统";
            this.tabPage_receiver.UseVisualStyleBackColor = true;
            // 
            // namedStringControl_satStdDev
            // 
            this.namedStringControl_satStdDev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_satStdDev.Location = new System.Drawing.Point(6, 137);
            this.namedStringControl_satStdDev.Name = "namedStringControl_satStdDev";
            this.namedStringControl_satStdDev.Size = new System.Drawing.Size(666, 23);
            this.namedStringControl_satStdDev.TabIndex = 32;
            this.namedStringControl_satStdDev.Title = "指定卫星标准差（默认为1）：";
            // 
            // namedStringControl_sysStdDev
            // 
            this.namedStringControl_sysStdDev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_sysStdDev.Location = new System.Drawing.Point(6, 108);
            this.namedStringControl_sysStdDev.Name = "namedStringControl_sysStdDev";
            this.namedStringControl_sysStdDev.Size = new System.Drawing.Size(666, 23);
            this.namedStringControl_sysStdDev.TabIndex = 32;
            this.namedStringControl_sysStdDev.Title = "系统误差系数(将与卫星标准差相乘)：";
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(3, 3);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(688, 48);
            this.multiGnssSystemSelectControl1.TabIndex = 31;
            // 
            // checkBox_isSameTimeSystem
            // 
            this.checkBox_isSameTimeSystem.AutoSize = true;
            this.checkBox_isSameTimeSystem.Location = new System.Drawing.Point(5, 72);
            this.checkBox_isSameTimeSystem.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_isSameTimeSystem.Name = "checkBox_isSameTimeSystem";
            this.checkBox_isSameTimeSystem.Size = new System.Drawing.Size(120, 16);
            this.checkBox_isSameTimeSystem.TabIndex = 30;
            this.checkBox_isSameTimeSystem.Text = "采用同一时间系统";
            this.checkBox_isSameTimeSystem.UseVisualStyleBackColor = true;
            // 
            // openFileDialog_nav
            // 
            this.openFileDialog_nav.FileName = "星历文件";
            this.openFileDialog_nav.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件,压缩文件（*.*O;*.*D;*.*D.Z;|*.*o;*.*D.Z;*.*D|所有文件|*.*";
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "钟差文件";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk;*.clk_30s|所有文件|*.*";
            // 
            // MutiGnssOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "MutiGnssOptionPage";
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_receiver.ResumeLayout(false);
            this.tabPage_receiver.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_receiver;
        private System.Windows.Forms.CheckBox checkBox_isSameTimeSystem;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_satStdDev;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_sysStdDev;
    }
}