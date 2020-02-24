namespace Gnsser.Winform
{
    partial class ResultCheckOptionPage
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
            this.tabPage_stream = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.namedFloatControlExemptedStdDev = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1MaxErrorTimesOfPostResdual = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox1IsResidualCheckEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsResultCheckEnabled = new System.Windows.Forms.CheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_maxStdDev = new System.Windows.Forms.TextBox();
            this.textBoxMaxMeanStdTimes = new System.Windows.Forms.TextBox();
            this.textBox_maxLoopCount = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage_stream.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_stream);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(671, 473);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_stream
            // 
            this.tabPage_stream.Controls.Add(this.groupBox5);
            this.tabPage_stream.Controls.Add(this.groupBox3);
            this.tabPage_stream.Location = new System.Drawing.Point(4, 22);
            this.tabPage_stream.Name = "tabPage_stream";
            this.tabPage_stream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_stream.Size = new System.Drawing.Size(663, 447);
            this.tabPage_stream.TabIndex = 12;
            this.tabPage_stream.Text = "结果检核";
            this.tabPage_stream.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.namedFloatControl1MaxErrorTimesOfPostResdual);
            this.groupBox5.Controls.Add(this.checkBox1IsResidualCheckEnabled);
            this.groupBox5.Location = new System.Drawing.Point(251, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(389, 110);
            this.groupBox5.TabIndex = 69;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "残差检核";
            // 
            // namedFloatControlExemptedStdDev
            // 
            this.namedFloatControlExemptedStdDev.Location = new System.Drawing.Point(26, 110);
            this.namedFloatControlExemptedStdDev.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControlExemptedStdDev.Name = "namedFloatControlExemptedStdDev";
            this.namedFloatControlExemptedStdDev.Size = new System.Drawing.Size(184, 23);
            this.namedFloatControlExemptedStdDev.TabIndex = 48;
            this.namedFloatControlExemptedStdDev.Title = "中误差免检大小：";
            this.namedFloatControlExemptedStdDev.Value = 0.1D;
            // 
            // namedFloatControl1MaxErrorTimesOfPostResdual
            // 
            this.namedFloatControl1MaxErrorTimesOfPostResdual.Location = new System.Drawing.Point(14, 57);
            this.namedFloatControl1MaxErrorTimesOfPostResdual.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl1MaxErrorTimesOfPostResdual.Name = "namedFloatControl1MaxErrorTimesOfPostResdual";
            this.namedFloatControl1MaxErrorTimesOfPostResdual.Size = new System.Drawing.Size(216, 23);
            this.namedFloatControl1MaxErrorTimesOfPostResdual.TabIndex = 48;
            this.namedFloatControl1MaxErrorTimesOfPostResdual.Title = "验收残差最允许粗倍数：";
            this.namedFloatControl1MaxErrorTimesOfPostResdual.Value = 0.1D;
            // 
            // checkBox1IsResidualCheckEnabled
            // 
            this.checkBox1IsResidualCheckEnabled.AutoSize = true;
            this.checkBox1IsResidualCheckEnabled.Location = new System.Drawing.Point(14, 21);
            this.checkBox1IsResidualCheckEnabled.Name = "checkBox1IsResidualCheckEnabled";
            this.checkBox1IsResidualCheckEnabled.Size = new System.Drawing.Size(96, 16);
            this.checkBox1IsResidualCheckEnabled.TabIndex = 47;
            this.checkBox1IsResidualCheckEnabled.Text = "验后残差检核";
            this.checkBox1IsResidualCheckEnabled.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.namedFloatControlExemptedStdDev);
            this.groupBox3.Controls.Add(this.checkBox_IsResultCheckEnabled);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox_maxStdDev);
            this.groupBox3.Controls.Add(this.textBoxMaxMeanStdTimes);
            this.groupBox3.Controls.Add(this.textBox_maxLoopCount);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(230, 155);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "计算结果质量控制";
            // 
            // checkBox_IsResultCheckEnabled
            // 
            this.checkBox_IsResultCheckEnabled.AutoSize = true;
            this.checkBox_IsResultCheckEnabled.ForeColor = System.Drawing.Color.Red;
            this.checkBox_IsResultCheckEnabled.Location = new System.Drawing.Point(58, 21);
            this.checkBox_IsResultCheckEnabled.Name = "checkBox_IsResultCheckEnabled";
            this.checkBox_IsResultCheckEnabled.Size = new System.Drawing.Size(108, 16);
            this.checkBox_IsResultCheckEnabled.TabIndex = 68;
            this.checkBox_IsResultCheckEnabled.Text = "结果检核总开关";
            this.checkBox_IsResultCheckEnabled.UseVisualStyleBackColor = true;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(12, 84);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(101, 12);
            this.label23.TabIndex = 1;
            this.label23.Text = "最大均方根倍数：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "最大循环次数：";
            // 
            // textBox_maxStdDev
            // 
            this.textBox_maxStdDev.Location = new System.Drawing.Point(129, 37);
            this.textBox_maxStdDev.Name = "textBox_maxStdDev";
            this.textBox_maxStdDev.Size = new System.Drawing.Size(42, 21);
            this.textBox_maxStdDev.TabIndex = 2;
            this.textBox_maxStdDev.Text = "2";
            // 
            // textBoxMaxMeanStdTimes
            // 
            this.textBoxMaxMeanStdTimes.Location = new System.Drawing.Point(130, 82);
            this.textBoxMaxMeanStdTimes.Name = "textBoxMaxMeanStdTimes";
            this.textBoxMaxMeanStdTimes.Size = new System.Drawing.Size(42, 21);
            this.textBoxMaxMeanStdTimes.TabIndex = 2;
            this.textBoxMaxMeanStdTimes.Text = "4";
            // 
            // textBox_maxLoopCount
            // 
            this.textBox_maxLoopCount.Location = new System.Drawing.Point(129, 59);
            this.textBox_maxLoopCount.Name = "textBox_maxLoopCount";
            this.textBox_maxLoopCount.Size = new System.Drawing.Size(42, 21);
            this.textBox_maxLoopCount.TabIndex = 2;
            this.textBox_maxLoopCount.Text = "4";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(24, 40);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 12);
            this.label15.TabIndex = 1;
            this.label15.Text = "最大的均方差：";
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
            // ResultCheckOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ResultCheckOptionPage";
            this.Size = new System.Drawing.Size(671, 473);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_stream.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox textBox_maxLoopCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_maxStdDev;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tabPage_stream;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox textBoxMaxMeanStdTimes;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox1IsResidualCheckEnabled;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1MaxErrorTimesOfPostResdual;
        private System.Windows.Forms.CheckBox checkBox_IsResultCheckEnabled;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControlExemptedStdDev;
    }
}