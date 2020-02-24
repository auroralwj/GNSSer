namespace Gnsser.Winform
{
    partial class ParamOptionPage
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
            this.tabPage_param = new System.Windows.Forms.TabPage();
            this.checkBox_IsSiteNameIncluded = new System.Windows.Forms.CheckBox();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage_param.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_param);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_param
            // 
            this.tabPage_param.Controls.Add(this.checkBox_IsSiteNameIncluded);
            this.tabPage_param.Location = new System.Drawing.Point(4, 22);
            this.tabPage_param.Name = "tabPage_param";
            this.tabPage_param.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_param.Size = new System.Drawing.Size(694, 500);
            this.tabPage_param.TabIndex = 12;
            this.tabPage_param.Text = "参数";
            this.tabPage_param.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSiteNameIncluded
            // 
            this.checkBox_IsSiteNameIncluded.AutoSize = true;
            this.checkBox_IsSiteNameIncluded.Location = new System.Drawing.Point(25, 20);
            this.checkBox_IsSiteNameIncluded.Name = "checkBox_IsSiteNameIncluded";
            this.checkBox_IsSiteNameIncluded.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsSiteNameIncluded.TabIndex = 70;
            this.checkBox_IsSiteNameIncluded.Text = "包含测站名称";
            this.checkBox_IsSiteNameIncluded.UseVisualStyleBackColor = true;
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
            // ParamOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ParamOptionPage";
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_param.ResumeLayout(false);
            this.tabPage_param.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_param;
        private System.Windows.Forms.CheckBox checkBox_IsSiteNameIncluded;
    }
}