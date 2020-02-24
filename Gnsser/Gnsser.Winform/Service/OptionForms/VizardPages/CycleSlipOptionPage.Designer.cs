namespace Gnsser.Winform
{
    partial class CycleSlipOptionPage
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsDetectClockJump = new System.Windows.Forms.CheckBox();
            this.checkBox_isClockJumpRepaired = new System.Windows.Forms.CheckBox();
            this.enumCheckBoxControl1 = new Geo.Winform.EnumCheckBoxControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl1MaxDifferValueOfMwCs = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1MaxDifferOfLsPolyCs = new Geo.Winform.Controls.NamedFloatControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl3MaxErrorTimesOfBufferCs = new Geo.Winform.Controls.NamedFloatControl();
            this.namedIntControl3DifferTimesOfBufferCs = new Geo.Winform.Controls.NamedIntControl();
            this.checkBox1IgnoreCsedOfBufferCs = new System.Windows.Forms.CheckBox();
            this.namedIntControl1PolyFitOrderOfBufferCs = new Geo.Winform.Controls.NamedIntControl();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsCycleSlipDetectionRequired = new System.Windows.Forms.CheckBox();
            this.namedFloatControlMaxBreakingEpochCount = new Geo.Winform.Controls.NamedIntControl();
            this.checkBox1IsUsingRecordedCycleSlipInfo = new System.Windows.Forms.CheckBox();
            this.checkBoxRealTimeCs = new System.Windows.Forms.CheckBox();
            this.checkBoxBufferCs = new System.Windows.Forms.CheckBox();
            this.checkBox_IsReverseCycleSlipeRevise = new System.Windows.Forms.CheckBox();
            this.namedIntControl2MinWindowSizeOfBufferCs = new Geo.Winform.Controls.NamedIntControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.fileOpenControl_clockJumpFilePath = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_clockJumpSwitsher = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage_stream.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.tabPage_stream.Controls.Add(this.groupBox3);
            this.tabPage_stream.Controls.Add(this.enumCheckBoxControl1);
            this.tabPage_stream.Controls.Add(this.groupBox2);
            this.tabPage_stream.Controls.Add(this.groupBox1);
            this.tabPage_stream.Controls.Add(this.groupBox17);
            this.tabPage_stream.Location = new System.Drawing.Point(4, 22);
            this.tabPage_stream.Name = "tabPage_stream";
            this.tabPage_stream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_stream.Size = new System.Drawing.Size(694, 500);
            this.tabPage_stream.TabIndex = 12;
            this.tabPage_stream.Text = "周跳和钟跳设置";
            this.tabPage_stream.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.fileOpenControl_clockJumpFilePath);
            this.groupBox3.Controls.Add(this.checkBox_clockJumpSwitsher);
            this.groupBox3.Controls.Add(this.checkBox_IsDetectClockJump);
            this.groupBox3.Controls.Add(this.checkBox_isClockJumpRepaired);
            this.groupBox3.Location = new System.Drawing.Point(7, 373);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(665, 128);
            this.groupBox3.TabIndex = 73;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "钟跳";
            // 
            // checkBox_IsDetectClockJump
            // 
            this.checkBox_IsDetectClockJump.AutoSize = true;
            this.checkBox_IsDetectClockJump.Location = new System.Drawing.Point(144, 18);
            this.checkBox_IsDetectClockJump.Name = "checkBox_IsDetectClockJump";
            this.checkBox_IsDetectClockJump.Size = new System.Drawing.Size(108, 16);
            this.checkBox_IsDetectClockJump.TabIndex = 0;
            this.checkBox_IsDetectClockJump.Text = "探测并标记钟跳";
            this.checkBox_IsDetectClockJump.UseVisualStyleBackColor = true;
            // 
            // checkBox_isClockJumpRepaired
            // 
            this.checkBox_isClockJumpRepaired.AutoSize = true;
            this.checkBox_isClockJumpRepaired.Location = new System.Drawing.Point(271, 18);
            this.checkBox_isClockJumpRepaired.Name = "checkBox_isClockJumpRepaired";
            this.checkBox_isClockJumpRepaired.Size = new System.Drawing.Size(120, 16);
            this.checkBox_isClockJumpRepaired.TabIndex = 0;
            this.checkBox_isClockJumpRepaired.Text = "修复钟跳(先探测)";
            this.checkBox_isClockJumpRepaired.UseVisualStyleBackColor = true;
            // 
            // enumCheckBoxControl1
            // 
            this.enumCheckBoxControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumCheckBoxControl1.Location = new System.Drawing.Point(250, 194);
            this.enumCheckBoxControl1.Name = "enumCheckBoxControl1";
            this.enumCheckBoxControl1.Size = new System.Drawing.Size(422, 173);
            this.enumCheckBoxControl1.TabIndex = 72;
            this.enumCheckBoxControl1.Title = "探测器";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.namedFloatControl1MaxDifferValueOfMwCs);
            this.groupBox2.Controls.Add(this.namedFloatControl1MaxValueDifferOfHeigherDifferCs);
            this.groupBox2.Controls.Add(this.namedFloatControl1MaxDifferOfLsPolyCs);
            this.groupBox2.Location = new System.Drawing.Point(3, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 129);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "其它探测";
            // 
            // namedFloatControl1MaxDifferValueOfMwCs
            // 
            this.namedFloatControl1MaxDifferValueOfMwCs.Location = new System.Drawing.Point(26, 20);
            this.namedFloatControl1MaxDifferValueOfMwCs.Name = "namedFloatControl1MaxDifferValueOfMwCs";
            this.namedFloatControl1MaxDifferValueOfMwCs.Size = new System.Drawing.Size(180, 23);
            this.namedFloatControl1MaxDifferValueOfMwCs.TabIndex = 1;
            this.namedFloatControl1MaxDifferValueOfMwCs.Title = "MW探测最大差值：";
            this.namedFloatControl1MaxDifferValueOfMwCs.Value = 0.1D;
            // 
            // namedFloatControl1MaxValueDifferOfHeigherDifferCs
            // 
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.Location = new System.Drawing.Point(3, 49);
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.Name = "namedFloatControl1MaxValueDifferOfHeigherDifferCs";
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.Size = new System.Drawing.Size(204, 23);
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.TabIndex = 1;
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.Title = "高次差探测最大差值：";
            this.namedFloatControl1MaxValueDifferOfHeigherDifferCs.Value = 0.1D;
            // 
            // namedFloatControl1MaxDifferOfLsPolyCs
            // 
            this.namedFloatControl1MaxDifferOfLsPolyCs.Location = new System.Drawing.Point(2, 78);
            this.namedFloatControl1MaxDifferOfLsPolyCs.Name = "namedFloatControl1MaxDifferOfLsPolyCs";
            this.namedFloatControl1MaxDifferOfLsPolyCs.Size = new System.Drawing.Size(204, 23);
            this.namedFloatControl1MaxDifferOfLsPolyCs.TabIndex = 1;
            this.namedFloatControl1MaxDifferOfLsPolyCs.Title = "多项式拟合探测最大RMS倍数：";
            this.namedFloatControl1MaxDifferOfLsPolyCs.Value = 0.1D;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.namedFloatControl3MaxErrorTimesOfBufferCs);
            this.groupBox1.Controls.Add(this.namedIntControl3DifferTimesOfBufferCs);
            this.groupBox1.Controls.Add(this.checkBox1IgnoreCsedOfBufferCs);
            this.groupBox1.Controls.Add(this.namedIntControl1PolyFitOrderOfBufferCs);
            this.groupBox1.Location = new System.Drawing.Point(241, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 158);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "缓存差分拟合探测";
            // 
            // namedFloatControl3MaxErrorTimesOfBufferCs
            // 
            this.namedFloatControl3MaxErrorTimesOfBufferCs.Location = new System.Drawing.Point(9, 26);
            this.namedFloatControl3MaxErrorTimesOfBufferCs.Name = "namedFloatControl3MaxErrorTimesOfBufferCs";
            this.namedFloatControl3MaxErrorTimesOfBufferCs.Size = new System.Drawing.Size(218, 27);
            this.namedFloatControl3MaxErrorTimesOfBufferCs.TabIndex = 1;
            this.namedFloatControl3MaxErrorTimesOfBufferCs.Title = "拟合探测最大RMS倍数：";
            this.namedFloatControl3MaxErrorTimesOfBufferCs.Value = 0.1D;
            // 
            // namedIntControl3DifferTimesOfBufferCs
            // 
            this.namedIntControl3DifferTimesOfBufferCs.Location = new System.Drawing.Point(74, 92);
            this.namedIntControl3DifferTimesOfBufferCs.Name = "namedIntControl3DifferTimesOfBufferCs";
            this.namedIntControl3DifferTimesOfBufferCs.Size = new System.Drawing.Size(119, 27);
            this.namedIntControl3DifferTimesOfBufferCs.TabIndex = 69;
            this.namedIntControl3DifferTimesOfBufferCs.Title = "差分次数：";
            this.namedIntControl3DifferTimesOfBufferCs.Value = 0;
            // 
            // checkBox1IgnoreCsedOfBufferCs
            // 
            this.checkBox1IgnoreCsedOfBufferCs.AutoSize = true;
            this.checkBox1IgnoreCsedOfBufferCs.Location = new System.Drawing.Point(29, 125);
            this.checkBox1IgnoreCsedOfBufferCs.Name = "checkBox1IgnoreCsedOfBufferCs";
            this.checkBox1IgnoreCsedOfBufferCs.Size = new System.Drawing.Size(228, 16);
            this.checkBox1IgnoreCsedOfBufferCs.TabIndex = 0;
            this.checkBox1IgnoreCsedOfBufferCs.Text = "忽略已标记周跳的历元卫星(节约资源)";
            this.checkBox1IgnoreCsedOfBufferCs.UseVisualStyleBackColor = true;
            // 
            // namedIntControl1PolyFitOrderOfBufferCs
            // 
            this.namedIntControl1PolyFitOrderOfBufferCs.Location = new System.Drawing.Point(74, 59);
            this.namedIntControl1PolyFitOrderOfBufferCs.Name = "namedIntControl1PolyFitOrderOfBufferCs";
            this.namedIntControl1PolyFitOrderOfBufferCs.Size = new System.Drawing.Size(119, 27);
            this.namedIntControl1PolyFitOrderOfBufferCs.TabIndex = 69;
            this.namedIntControl1PolyFitOrderOfBufferCs.Title = "拟合阶次：";
            this.namedIntControl1PolyFitOrderOfBufferCs.Value = 0;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.checkBox_IsCycleSlipDetectionRequired);
            this.groupBox17.Controls.Add(this.namedFloatControlMaxBreakingEpochCount);
            this.groupBox17.Controls.Add(this.checkBox1IsUsingRecordedCycleSlipInfo);
            this.groupBox17.Controls.Add(this.checkBoxRealTimeCs);
            this.groupBox17.Controls.Add(this.checkBoxBufferCs);
            this.groupBox17.Controls.Add(this.checkBox_IsReverseCycleSlipeRevise);
            this.groupBox17.Controls.Add(this.namedIntControl2MinWindowSizeOfBufferCs);
            this.groupBox17.Location = new System.Drawing.Point(6, 19);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(229, 213);
            this.groupBox17.TabIndex = 68;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "通用周跳探测设置";
            // 
            // checkBox_IsCycleSlipDetectionRequired
            // 
            this.checkBox_IsCycleSlipDetectionRequired.AutoSize = true;
            this.checkBox_IsCycleSlipDetectionRequired.ForeColor = System.Drawing.Color.Red;
            this.checkBox_IsCycleSlipDetectionRequired.Location = new System.Drawing.Point(24, 20);
            this.checkBox_IsCycleSlipDetectionRequired.Name = "checkBox_IsCycleSlipDetectionRequired";
            this.checkBox_IsCycleSlipDetectionRequired.Size = new System.Drawing.Size(108, 16);
            this.checkBox_IsCycleSlipDetectionRequired.TabIndex = 72;
            this.checkBox_IsCycleSlipDetectionRequired.Text = "周跳探测总开关";
            this.checkBox_IsCycleSlipDetectionRequired.UseVisualStyleBackColor = true;
            // 
            // namedFloatControlMaxBreakingEpochCount
            // 
            this.namedFloatControlMaxBreakingEpochCount.Location = new System.Drawing.Point(6, 142);
            this.namedFloatControlMaxBreakingEpochCount.Name = "namedFloatControlMaxBreakingEpochCount";
            this.namedFloatControlMaxBreakingEpochCount.Size = new System.Drawing.Size(184, 27);
            this.namedFloatControlMaxBreakingEpochCount.TabIndex = 69;
            this.namedFloatControlMaxBreakingEpochCount.Title = "时段内最大历元间隔：";
            this.namedFloatControlMaxBreakingEpochCount.Value = 0;
            // 
            // checkBox1IsUsingRecordedCycleSlipInfo
            // 
            this.checkBox1IsUsingRecordedCycleSlipInfo.AutoSize = true;
            this.checkBox1IsUsingRecordedCycleSlipInfo.Location = new System.Drawing.Point(23, 114);
            this.checkBox1IsUsingRecordedCycleSlipInfo.Name = "checkBox1IsUsingRecordedCycleSlipInfo";
            this.checkBox1IsUsingRecordedCycleSlipInfo.Size = new System.Drawing.Size(120, 16);
            this.checkBox1IsUsingRecordedCycleSlipInfo.TabIndex = 0;
            this.checkBox1IsUsingRecordedCycleSlipInfo.Text = "采用已有周跳信息";
            this.checkBox1IsUsingRecordedCycleSlipInfo.UseVisualStyleBackColor = true;
            // 
            // checkBoxRealTimeCs
            // 
            this.checkBoxRealTimeCs.AutoSize = true;
            this.checkBoxRealTimeCs.Location = new System.Drawing.Point(23, 67);
            this.checkBoxRealTimeCs.Name = "checkBoxRealTimeCs";
            this.checkBoxRealTimeCs.Size = new System.Drawing.Size(96, 16);
            this.checkBoxRealTimeCs.TabIndex = 0;
            this.checkBoxRealTimeCs.Text = "实时周跳标记";
            this.checkBoxRealTimeCs.UseVisualStyleBackColor = true;
            // 
            // checkBoxBufferCs
            // 
            this.checkBoxBufferCs.AutoSize = true;
            this.checkBoxBufferCs.Location = new System.Drawing.Point(23, 45);
            this.checkBoxBufferCs.Name = "checkBoxBufferCs";
            this.checkBoxBufferCs.Size = new System.Drawing.Size(120, 16);
            this.checkBoxBufferCs.TabIndex = 0;
            this.checkBoxBufferCs.Text = "启用缓存周跳标记";
            this.checkBoxBufferCs.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsReverseCycleSlipeRevise
            // 
            this.checkBox_IsReverseCycleSlipeRevise.AutoSize = true;
            this.checkBox_IsReverseCycleSlipeRevise.Location = new System.Drawing.Point(23, 93);
            this.checkBox_IsReverseCycleSlipeRevise.Name = "checkBox_IsReverseCycleSlipeRevise";
            this.checkBox_IsReverseCycleSlipeRevise.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsReverseCycleSlipeRevise.TabIndex = 0;
            this.checkBox_IsReverseCycleSlipeRevise.Text = "逆序周跳标记";
            this.checkBox_IsReverseCycleSlipeRevise.UseVisualStyleBackColor = true;
            // 
            // namedIntControl2MinWindowSizeOfBufferCs
            // 
            this.namedIntControl2MinWindowSizeOfBufferCs.Location = new System.Drawing.Point(24, 169);
            this.namedIntControl2MinWindowSizeOfBufferCs.Name = "namedIntControl2MinWindowSizeOfBufferCs";
            this.namedIntControl2MinWindowSizeOfBufferCs.Size = new System.Drawing.Size(166, 27);
            this.namedIntControl2MinWindowSizeOfBufferCs.TabIndex = 69;
            this.namedIntControl2MinWindowSizeOfBufferCs.Title = "最小时段历元数：";
            this.namedIntControl2MinWindowSizeOfBufferCs.Value = 0;
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
            // fileOpenControl_clockJumpFilePath
            // 
            this.fileOpenControl_clockJumpFilePath.AllowDrop = true;
            this.fileOpenControl_clockJumpFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_clockJumpFilePath.FilePath = "";
            this.fileOpenControl_clockJumpFilePath.FilePathes = new string[0];
            this.fileOpenControl_clockJumpFilePath.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_clockJumpFilePath.FirstPath = "";
            this.fileOpenControl_clockJumpFilePath.IsMultiSelect = false;
            this.fileOpenControl_clockJumpFilePath.LabelName = "外置钟跳文件：";
            this.fileOpenControl_clockJumpFilePath.Location = new System.Drawing.Point(6, 51);
            this.fileOpenControl_clockJumpFilePath.Name = "fileOpenControl_clockJumpFilePath";
            this.fileOpenControl_clockJumpFilePath.Size = new System.Drawing.Size(636, 22);
            this.fileOpenControl_clockJumpFilePath.TabIndex = 60;
            // 
            // checkBox_clockJumpSwitsher
            // 
            this.checkBox_clockJumpSwitsher.AutoSize = true;
            this.checkBox_clockJumpSwitsher.Location = new System.Drawing.Point(10, 18);
            this.checkBox_clockJumpSwitsher.Name = "checkBox_clockJumpSwitsher";
            this.checkBox_clockJumpSwitsher.Size = new System.Drawing.Size(108, 16);
            this.checkBox_clockJumpSwitsher.TabIndex = 0;
            this.checkBox_clockJumpSwitsher.Text = "钟跳操作总开关";
            this.checkBox_clockJumpSwitsher.UseVisualStyleBackColor = true;
            // 
            // CycleSlipOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CycleSlipOptionPage";
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_stream.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBox_IsReverseCycleSlipeRevise;
        private System.Windows.Forms.CheckBox checkBox1IsUsingRecordedCycleSlipInfo;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1MaxDifferValueOfMwCs;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1MaxValueDifferOfHeigherDifferCs;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1MaxDifferOfLsPolyCs;
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl3MaxErrorTimesOfBufferCs;
        private Geo.Winform.Controls.NamedIntControl namedIntControl3DifferTimesOfBufferCs;
        private Geo.Winform.Controls.NamedIntControl namedIntControl1PolyFitOrderOfBufferCs;
        private Geo.Winform.Controls.NamedIntControl namedIntControl2MinWindowSizeOfBufferCs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox1IgnoreCsedOfBufferCs;
        private System.Windows.Forms.CheckBox checkBoxRealTimeCs;
        private System.Windows.Forms.CheckBox checkBoxBufferCs;
        private System.Windows.Forms.CheckBox checkBox_IsCycleSlipDetectionRequired;
        private Geo.Winform.EnumCheckBoxControl enumCheckBoxControl1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_IsDetectClockJump;
        private System.Windows.Forms.CheckBox checkBox_isClockJumpRepaired;
        private Geo.Winform.Controls.NamedIntControl namedFloatControlMaxBreakingEpochCount;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_clockJumpFilePath;
        private System.Windows.Forms.CheckBox checkBox_clockJumpSwitsher;
    }
}