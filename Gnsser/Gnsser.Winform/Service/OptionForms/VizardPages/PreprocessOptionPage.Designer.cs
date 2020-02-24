namespace Gnsser.Winform
{
    partial class PreprocessOptionPage
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_breakEndsMinute = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox_IsBreakOffBothEnds = new System.Windows.Forms.CheckBox();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsAllowMissingEpochSite = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_epochMaxAllowedGap = new System.Windows.Forms.TextBox();
            this.textBox_minSatCount = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_MinFrequenceCount = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_maxAllowedRange = new System.Windows.Forms.TextBox();
            this.textBox_minAllowedRange = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsRemoveIonoFreeUnavaliable = new System.Windows.Forms.CheckBox();
            this.checkBox_IsAliningPhaseWithRange = new System.Windows.Forms.CheckBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsRemoveSmallPartSat = new System.Windows.Forms.CheckBox();
            this.namedFloatControlMaxBreakingEpochCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_MinContinuouObsCount = new Geo.Winform.Controls.NamedIntControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.checkBox_IsEnableSatAppearenceService = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage_stream.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_stream);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_stream
            // 
            this.tabPage_stream.Controls.Add(this.groupBox1);
            this.tabPage_stream.Controls.Add(this.groupBox15);
            this.tabPage_stream.Controls.Add(this.groupBox9);
            this.tabPage_stream.Controls.Add(this.groupBox8);
            this.tabPage_stream.Controls.Add(this.groupBox17);
            this.tabPage_stream.Location = new System.Drawing.Point(4, 22);
            this.tabPage_stream.Name = "tabPage_stream";
            this.tabPage_stream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_stream.Size = new System.Drawing.Size(694, 500);
            this.tabPage_stream.TabIndex = 12;
            this.tabPage_stream.Text = "预处理";
            this.tabPage_stream.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_IsEnableSatAppearenceService);
            this.groupBox1.Controls.Add(this.namedFloatControl_breakEndsMinute);
            this.groupBox1.Controls.Add(this.checkBox_IsBreakOffBothEnds);
            this.groupBox1.Location = new System.Drawing.Point(308, 209);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 113);
            this.groupBox1.TabIndex = 77;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "掐头去尾";
            // 
            // namedFloatControl_breakEndsMinute
            // 
            this.namedFloatControl_breakEndsMinute.Location = new System.Drawing.Point(15, 55);
            this.namedFloatControl_breakEndsMinute.Name = "namedFloatControl_breakEndsMinute";
            this.namedFloatControl_breakEndsMinute.Size = new System.Drawing.Size(159, 23);
            this.namedFloatControl_breakEndsMinute.TabIndex = 78;
            this.namedFloatControl_breakEndsMinute.Title = "掐头去尾分钟数：";
            this.namedFloatControl_breakEndsMinute.Value = 0.1D;
            // 
            // checkBox_IsBreakOffBothEnds
            // 
            this.checkBox_IsBreakOffBothEnds.AutoSize = true;
            this.checkBox_IsBreakOffBothEnds.Location = new System.Drawing.Point(15, 20);
            this.checkBox_IsBreakOffBothEnds.Name = "checkBox_IsBreakOffBothEnds";
            this.checkBox_IsBreakOffBothEnds.Size = new System.Drawing.Size(144, 16);
            this.checkBox_IsBreakOffBothEnds.TabIndex = 32;
            this.checkBox_IsBreakOffBothEnds.Text = "启用观测数据掐头去尾";
            this.checkBox_IsBreakOffBothEnds.UseVisualStyleBackColor = true;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.checkBox_IsAllowMissingEpochSite);
            this.groupBox15.Location = new System.Drawing.Point(294, 138);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(289, 54);
            this.groupBox15.TabIndex = 76;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "多测站设置";
            // 
            // checkBox_IsAllowMissingEpochSite
            // 
            this.checkBox_IsAllowMissingEpochSite.AutoSize = true;
            this.checkBox_IsAllowMissingEpochSite.Location = new System.Drawing.Point(29, 20);
            this.checkBox_IsAllowMissingEpochSite.Name = "checkBox_IsAllowMissingEpochSite";
            this.checkBox_IsAllowMissingEpochSite.Size = new System.Drawing.Size(132, 16);
            this.checkBox_IsAllowMissingEpochSite.TabIndex = 47;
            this.checkBox_IsAllowMissingEpochSite.Text = "允许某历元丢失测站";
            this.checkBox_IsAllowMissingEpochSite.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label9);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.textBox_epochMaxAllowedGap);
            this.groupBox9.Controls.Add(this.textBox_minSatCount);
            this.groupBox9.Controls.Add(this.label12);
            this.groupBox9.Controls.Add(this.textBox_MinFrequenceCount);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Controls.Add(this.textBox_maxAllowedRange);
            this.groupBox9.Controls.Add(this.textBox_minAllowedRange);
            this.groupBox9.Controls.Add(this.label14);
            this.groupBox9.Location = new System.Drawing.Point(6, 6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(282, 186);
            this.groupBox9.TabIndex = 75;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "源数据检核";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "历元最大间隙(秒)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "最少卫星数量：";
            // 
            // textBox_epochMaxAllowedGap
            // 
            this.textBox_epochMaxAllowedGap.Location = new System.Drawing.Point(128, 16);
            this.textBox_epochMaxAllowedGap.Name = "textBox_epochMaxAllowedGap";
            this.textBox_epochMaxAllowedGap.Size = new System.Drawing.Size(90, 21);
            this.textBox_epochMaxAllowedGap.TabIndex = 2;
            this.textBox_epochMaxAllowedGap.Text = "121.0";
            // 
            // textBox_minSatCount
            // 
            this.textBox_minSatCount.Location = new System.Drawing.Point(129, 136);
            this.textBox_minSatCount.Name = "textBox_minSatCount";
            this.textBox_minSatCount.Size = new System.Drawing.Size(89, 21);
            this.textBox_minSatCount.TabIndex = 2;
            this.textBox_minSatCount.Text = "4";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 47);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "最小观测频率数：";
            // 
            // textBox_MinFrequenceCount
            // 
            this.textBox_MinFrequenceCount.Location = new System.Drawing.Point(128, 44);
            this.textBox_MinFrequenceCount.Name = "textBox_MinFrequenceCount";
            this.textBox_MinFrequenceCount.Size = new System.Drawing.Size(89, 21);
            this.textBox_MinFrequenceCount.TabIndex = 2;
            this.textBox_MinFrequenceCount.Text = "2";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 12);
            this.label13.TabIndex = 1;
            this.label13.Text = "允许最小的伪距：";
            // 
            // textBox_maxAllowedRange
            // 
            this.textBox_maxAllowedRange.Location = new System.Drawing.Point(129, 100);
            this.textBox_maxAllowedRange.Name = "textBox_maxAllowedRange";
            this.textBox_maxAllowedRange.Size = new System.Drawing.Size(89, 21);
            this.textBox_maxAllowedRange.TabIndex = 2;
            this.textBox_maxAllowedRange.Text = "2";
            // 
            // textBox_minAllowedRange
            // 
            this.textBox_minAllowedRange.Location = new System.Drawing.Point(128, 72);
            this.textBox_minAllowedRange.Name = "textBox_minAllowedRange";
            this.textBox_minAllowedRange.Size = new System.Drawing.Size(89, 21);
            this.textBox_minAllowedRange.TabIndex = 2;
            this.textBox_minAllowedRange.Text = "2";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(22, 103);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(101, 12);
            this.label14.TabIndex = 1;
            this.label14.Text = "允许最大的伪距：";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.checkBox_IsRemoveIonoFreeUnavaliable);
            this.groupBox8.Controls.Add(this.checkBox_IsAliningPhaseWithRange);
            this.groupBox8.Location = new System.Drawing.Point(294, 6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(289, 115);
            this.groupBox8.TabIndex = 74;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "数据预处理";
            // 
            // checkBox_IsRemoveIonoFreeUnavaliable
            // 
            this.checkBox_IsRemoveIonoFreeUnavaliable.AutoSize = true;
            this.checkBox_IsRemoveIonoFreeUnavaliable.Location = new System.Drawing.Point(35, 42);
            this.checkBox_IsRemoveIonoFreeUnavaliable.Name = "checkBox_IsRemoveIonoFreeUnavaliable";
            this.checkBox_IsRemoveIonoFreeUnavaliable.Size = new System.Drawing.Size(228, 16);
            this.checkBox_IsRemoveIonoFreeUnavaliable.TabIndex = 32;
            this.checkBox_IsRemoveIonoFreeUnavaliable.Text = "移除无法组成无电离层组合的观测数据";
            this.checkBox_IsRemoveIonoFreeUnavaliable.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsAliningPhaseWithRange
            // 
            this.checkBox_IsAliningPhaseWithRange.AutoSize = true;
            this.checkBox_IsAliningPhaseWithRange.Location = new System.Drawing.Point(35, 20);
            this.checkBox_IsAliningPhaseWithRange.Name = "checkBox_IsAliningPhaseWithRange";
            this.checkBox_IsAliningPhaseWithRange.Size = new System.Drawing.Size(144, 16);
            this.checkBox_IsAliningPhaseWithRange.TabIndex = 32;
            this.checkBox_IsAliningPhaseWithRange.Text = "将初始相位用伪距对齐";
            this.checkBox_IsAliningPhaseWithRange.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.checkBox_IsRemoveSmallPartSat);
            this.groupBox17.Controls.Add(this.namedFloatControlMaxBreakingEpochCount);
            this.groupBox17.Controls.Add(this.namedIntControl_MinContinuouObsCount);
            this.groupBox17.Location = new System.Drawing.Point(6, 198);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(296, 111);
            this.groupBox17.TabIndex = 68;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "数据流";
            // 
            // checkBox_IsRemoveSmallPartSat
            // 
            this.checkBox_IsRemoveSmallPartSat.AutoSize = true;
            this.checkBox_IsRemoveSmallPartSat.Location = new System.Drawing.Point(54, 20);
            this.checkBox_IsRemoveSmallPartSat.Name = "checkBox_IsRemoveSmallPartSat";
            this.checkBox_IsRemoveSmallPartSat.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsRemoveSmallPartSat.TabIndex = 70;
            this.checkBox_IsRemoveSmallPartSat.Text = "移除小历元段卫星";
            this.checkBox_IsRemoveSmallPartSat.UseVisualStyleBackColor = true;
            // 
            // namedFloatControlMaxBreakingEpochCount
            // 
            this.namedFloatControlMaxBreakingEpochCount.Location = new System.Drawing.Point(6, 50);
            this.namedFloatControlMaxBreakingEpochCount.Name = "namedFloatControlMaxBreakingEpochCount";
            this.namedFloatControlMaxBreakingEpochCount.Size = new System.Drawing.Size(188, 27);
            this.namedFloatControlMaxBreakingEpochCount.TabIndex = 69;
            this.namedFloatControlMaxBreakingEpochCount.Title = "时段内最大历元间隔：";
            this.namedFloatControlMaxBreakingEpochCount.Value = 0;
            // 
            // namedIntControl_MinContinuouObsCount
            // 
            this.namedIntControl_MinContinuouObsCount.Location = new System.Drawing.Point(29, 79);
            this.namedIntControl_MinContinuouObsCount.Name = "namedIntControl_MinContinuouObsCount";
            this.namedIntControl_MinContinuouObsCount.Size = new System.Drawing.Size(166, 27);
            this.namedIntControl_MinContinuouObsCount.TabIndex = 69;
            this.namedIntControl_MinContinuouObsCount.Title = "最小时段历元数：";
            this.namedIntControl_MinContinuouObsCount.Value = 0;
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
            // checkBox_IsEnableSatAppearenceService
            // 
            this.checkBox_IsEnableSatAppearenceService.AutoSize = true;
            this.checkBox_IsEnableSatAppearenceService.Location = new System.Drawing.Point(15, 84);
            this.checkBox_IsEnableSatAppearenceService.Name = "checkBox_IsEnableSatAppearenceService";
            this.checkBox_IsEnableSatAppearenceService.Size = new System.Drawing.Size(180, 16);
            this.checkBox_IsEnableSatAppearenceService.TabIndex = 32;
            this.checkBox_IsEnableSatAppearenceService.Text = "启用时段分析(掐头去尾需要)";
            this.checkBox_IsEnableSatAppearenceService.UseVisualStyleBackColor = true;
            // 
            // PreprocessOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "PreprocessOptionPage";
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_stream.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_stream;
        private System.Windows.Forms.GroupBox groupBox17;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_MinContinuouObsCount;
        private Geo.Winform.Controls.NamedIntControl namedFloatControlMaxBreakingEpochCount;
        private System.Windows.Forms.CheckBox checkBox_IsRemoveSmallPartSat;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox checkBox_IsAliningPhaseWithRange;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_epochMaxAllowedGap;
        private System.Windows.Forms.TextBox textBox_minSatCount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_MinFrequenceCount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_maxAllowedRange;
        private System.Windows.Forms.TextBox textBox_minAllowedRange;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.CheckBox checkBox_IsAllowMissingEpochSite;
        private System.Windows.Forms.CheckBox checkBox_IsRemoveIonoFreeUnavaliable;
        private System.Windows.Forms.CheckBox checkBox_IsBreakOffBothEnds;
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_breakEndsMinute;
        private System.Windows.Forms.CheckBox checkBox_IsEnableSatAppearenceService;
    }
}