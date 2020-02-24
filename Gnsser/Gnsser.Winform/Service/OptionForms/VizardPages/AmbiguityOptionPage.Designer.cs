namespace Gnsser.Winform
{
    partial class AmbiguityOptionPage
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_MaxAllowedRmsOfMw = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox_IsOutputPeriodData = new System.Windows.Forms.CheckBox();
            this.checkBox_IsEnableSiteSatPeriodDataService = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsFixParamByConditionOrHugeWeight = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat = new Geo.Winform.Controls.NamedFloatControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fileOpenControl_ambiguityFile = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed = new System.Windows.Forms.CheckBox();
            this.checkBox_ambiguityFile = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsUseFixedParamDirectly = new System.Windows.Forms.CheckBox();
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity = new Geo.Winform.Controls.NamedFloatControl();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_IsFixingAmbiguity = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_MinFixedAmbiRatio = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox_IsPhaseUnitMetterOrCycle = new System.Windows.Forms.CheckBox();
            this.namedFloatControl_MaxRatioOfLambda = new Geo.Winform.Controls.NamedFloatControl();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat = new Geo.Winform.Controls.NamedFloatControl();
            this.tabControl1.SuspendLayout();
            this.tabPage_receiver.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_receiver);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(566, 493);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_receiver
            // 
            this.tabPage_receiver.Controls.Add(this.groupBox6);
            this.tabPage_receiver.Controls.Add(this.groupBox5);
            this.tabPage_receiver.Controls.Add(this.groupBox4);
            this.tabPage_receiver.Controls.Add(this.groupBox3);
            this.tabPage_receiver.Controls.Add(this.groupBox2);
            this.tabPage_receiver.Controls.Add(this.checkBox_IsFixingAmbiguity);
            this.tabPage_receiver.Controls.Add(this.groupBox1);
            this.tabPage_receiver.Location = new System.Drawing.Point(4, 22);
            this.tabPage_receiver.Name = "tabPage_receiver";
            this.tabPage_receiver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_receiver.Size = new System.Drawing.Size(558, 467);
            this.tabPage_receiver.TabIndex = 7;
            this.tabPage_receiver.Text = "不变参数(模糊度)设置";
            this.tabPage_receiver.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat);
            this.groupBox6.Controls.Add(this.namedFloatControl_MaxAllowedRmsOfMw);
            this.groupBox6.Controls.Add(this.checkBox_IsOutputPeriodData);
            this.groupBox6.Controls.Add(this.checkBox_IsEnableSiteSatPeriodDataService);
            this.groupBox6.Location = new System.Drawing.Point(5, 391);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(533, 78);
            this.groupBox6.TabIndex = 55;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "无电离层模糊度固定选项";
            // 
            // namedFloatControl_MaxAllowedRmsOfMw
            // 
            this.namedFloatControl_MaxAllowedRmsOfMw.Location = new System.Drawing.Point(236, 23);
            this.namedFloatControl_MaxAllowedRmsOfMw.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_MaxAllowedRmsOfMw.Name = "namedFloatControl_MaxAllowedRmsOfMw";
            this.namedFloatControl_MaxAllowedRmsOfMw.Size = new System.Drawing.Size(171, 23);
            this.namedFloatControl_MaxAllowedRmsOfMw.TabIndex = 39;
            this.namedFloatControl_MaxAllowedRmsOfMw.Title = "MW最大允许RMS：";
            this.namedFloatControl_MaxAllowedRmsOfMw.Value = 0.1D;
            // 
            // checkBox_IsOutputPeriodData
            // 
            this.checkBox_IsOutputPeriodData.AutoSize = true;
            this.checkBox_IsOutputPeriodData.Location = new System.Drawing.Point(134, 23);
            this.checkBox_IsOutputPeriodData.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsOutputPeriodData.Name = "checkBox_IsOutputPeriodData";
            this.checkBox_IsOutputPeriodData.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsOutputPeriodData.TabIndex = 0;
            this.checkBox_IsOutputPeriodData.Text = "输出时段数据";
            this.checkBox_IsOutputPeriodData.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsEnableSiteSatPeriodDataService
            // 
            this.checkBox_IsEnableSiteSatPeriodDataService.AutoSize = true;
            this.checkBox_IsEnableSiteSatPeriodDataService.Location = new System.Drawing.Point(8, 23);
            this.checkBox_IsEnableSiteSatPeriodDataService.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsEnableSiteSatPeriodDataService.Name = "checkBox_IsEnableSiteSatPeriodDataService";
            this.checkBox_IsEnableSiteSatPeriodDataService.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsEnableSiteSatPeriodDataService.TabIndex = 0;
            this.checkBox_IsEnableSiteSatPeriodDataService.Text = "启用时段数据服务";
            this.checkBox_IsEnableSiteSatPeriodDataService.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.checkBox_IsFixParamByConditionOrHugeWeight);
            this.groupBox5.Location = new System.Drawing.Point(6, 262);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(533, 50);
            this.groupBox5.TabIndex = 55;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "模糊度算法";
            // 
            // checkBox_IsFixParamByConditionOrHugeWeight
            // 
            this.checkBox_IsFixParamByConditionOrHugeWeight.AutoSize = true;
            this.checkBox_IsFixParamByConditionOrHugeWeight.Location = new System.Drawing.Point(25, 22);
            this.checkBox_IsFixParamByConditionOrHugeWeight.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsFixParamByConditionOrHugeWeight.Name = "checkBox_IsFixParamByConditionOrHugeWeight";
            this.checkBox_IsFixParamByConditionOrHugeWeight.Size = new System.Drawing.Size(204, 16);
            this.checkBox_IsFixParamByConditionOrHugeWeight.TabIndex = 0;
            this.checkBox_IsFixParamByConditionOrHugeWeight.Text = "采用条件平差，否则加权参数解法";
            this.checkBox_IsFixParamByConditionOrHugeWeight.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.namedFloatControl1MaxAmbiDifferOfIntAndFloat);
            this.groupBox4.Location = new System.Drawing.Point(5, 217);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(538, 43);
            this.groupBox4.TabIndex = 54;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "模糊度/固定参数检核";
            // 
            // namedFloatControl1MaxAmbiDifferOfIntAndFloat
            // 
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.Location = new System.Drawing.Point(26, 19);
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.Name = "namedFloatControl1MaxAmbiDifferOfIntAndFloat";
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.Size = new System.Drawing.Size(247, 23);
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.TabIndex = 39;
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.Title = "与固定前最大偏差：";
            this.namedFloatControl1MaxAmbiDifferOfIntAndFloat.Value = 0.1D;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.fileOpenControl_ambiguityFile);
            this.groupBox3.Controls.Add(this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed);
            this.groupBox3.Controls.Add(this.checkBox_ambiguityFile);
            this.groupBox3.Location = new System.Drawing.Point(4, 316);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(539, 71);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "模糊度/固定参数文件（优先于实时固定）";
            // 
            // fileOpenControl_ambiguityFile
            // 
            this.fileOpenControl_ambiguityFile.AllowDrop = true;
            this.fileOpenControl_ambiguityFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_ambiguityFile.FilePath = "";
            this.fileOpenControl_ambiguityFile.FilePathes = new string[0];
            this.fileOpenControl_ambiguityFile.Filter = "GNSSer模糊度文件|*.amb.txt.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_ambiguityFile.FirstPath = "";
            this.fileOpenControl_ambiguityFile.IsMultiSelect = false;
            this.fileOpenControl_ambiguityFile.LabelName = "固定参数文件：";
            this.fileOpenControl_ambiguityFile.Location = new System.Drawing.Point(15, 21);
            this.fileOpenControl_ambiguityFile.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl_ambiguityFile.Name = "fileOpenControl_ambiguityFile";
            this.fileOpenControl_ambiguityFile.Size = new System.Drawing.Size(463, 22);
            this.fileOpenControl_ambiguityFile.TabIndex = 51;
            // 
            // checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed
            // 
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.AutoSize = true;
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.Location = new System.Drawing.Point(97, 50);
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.Name = "checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed";
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.Size = new System.Drawing.Size(216, 16);
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.TabIndex = 40;
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.Text = "文件固定失败后，是否进行实时固定";
            this.checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed.UseVisualStyleBackColor = true;
            // 
            // checkBox_ambiguityFile
            // 
            this.checkBox_ambiguityFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_ambiguityFile.AutoSize = true;
            this.checkBox_ambiguityFile.Location = new System.Drawing.Point(481, 26);
            this.checkBox_ambiguityFile.Name = "checkBox_ambiguityFile";
            this.checkBox_ambiguityFile.Size = new System.Drawing.Size(48, 16);
            this.checkBox_ambiguityFile.TabIndex = 40;
            this.checkBox_ambiguityFile.Text = "启用";
            this.checkBox_ambiguityFile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBox_IsUseFixedParamDirectly);
            this.groupBox2.Controls.Add(this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 34);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(539, 74);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模糊度/固定参数启用条件";
            // 
            // checkBox_IsUseFixedParamDirectly
            // 
            this.checkBox_IsUseFixedParamDirectly.AutoSize = true;
            this.checkBox_IsUseFixedParamDirectly.ForeColor = System.Drawing.Color.DarkOrange;
            this.checkBox_IsUseFixedParamDirectly.Location = new System.Drawing.Point(17, 20);
            this.checkBox_IsUseFixedParamDirectly.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_IsUseFixedParamDirectly.Name = "checkBox_IsUseFixedParamDirectly";
            this.checkBox_IsUseFixedParamDirectly.Size = new System.Drawing.Size(228, 16);
            this.checkBox_IsUseFixedParamDirectly.TabIndex = 51;
            this.checkBox_IsUseFixedParamDirectly.Text = "无条件直接使用(常用于外部文件固定)";
            this.checkBox_IsUseFixedParamDirectly.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_MaxFloatRmsNormToFixAmbiguity
            // 
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Location = new System.Drawing.Point(8, 45);
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Name = "namedFloatControl_MaxFloatRmsNormToFixAmbiguity";
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Size = new System.Drawing.Size(244, 23);
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.TabIndex = 39;
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Title = "浮点解RMS最大二范数：";
            this.namedFloatControl_MaxFloatRmsNormToFixAmbiguity.Value = 0.1D;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 50;
            this.label2.Text = "常用于实时固定";
            // 
            // checkBox_IsFixingAmbiguity
            // 
            this.checkBox_IsFixingAmbiguity.AutoSize = true;
            this.checkBox_IsFixingAmbiguity.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkBox_IsFixingAmbiguity.Location = new System.Drawing.Point(13, 10);
            this.checkBox_IsFixingAmbiguity.Name = "checkBox_IsFixingAmbiguity";
            this.checkBox_IsFixingAmbiguity.Size = new System.Drawing.Size(174, 16);
            this.checkBox_IsFixingAmbiguity.TabIndex = 40;
            this.checkBox_IsFixingAmbiguity.Text = "模糊度/不变参数使用总开关";
            this.checkBox_IsFixingAmbiguity.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.namedFloatControl_MinFixedAmbiRatio);
            this.groupBox1.Controls.Add(this.checkBox_IsPhaseUnitMetterOrCycle);
            this.groupBox1.Controls.Add(this.namedFloatControl_MaxRatioOfLambda);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 114);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(539, 98);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "实时模糊度固定选项";
            // 
            // namedFloatControl_MinFixedAmbiRatio
            // 
            this.namedFloatControl_MinFixedAmbiRatio.Location = new System.Drawing.Point(9, 44);
            this.namedFloatControl_MinFixedAmbiRatio.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_MinFixedAmbiRatio.Name = "namedFloatControl_MinFixedAmbiRatio";
            this.namedFloatControl_MinFixedAmbiRatio.Size = new System.Drawing.Size(247, 23);
            this.namedFloatControl_MinFixedAmbiRatio.TabIndex = 39;
            this.namedFloatControl_MinFixedAmbiRatio.Title = "模糊度参数固定成功的最小比例：";
            this.namedFloatControl_MinFixedAmbiRatio.Value = 0.1D;
            // 
            // checkBox_IsPhaseUnitMetterOrCycle
            // 
            this.checkBox_IsPhaseUnitMetterOrCycle.AutoSize = true;
            this.checkBox_IsPhaseUnitMetterOrCycle.Location = new System.Drawing.Point(25, 21);
            this.checkBox_IsPhaseUnitMetterOrCycle.Name = "checkBox_IsPhaseUnitMetterOrCycle";
            this.checkBox_IsPhaseUnitMetterOrCycle.Size = new System.Drawing.Size(168, 16);
            this.checkBox_IsPhaseUnitMetterOrCycle.TabIndex = 40;
            this.checkBox_IsPhaseUnitMetterOrCycle.Text = "载波相位单位：米，否则周";
            this.checkBox_IsPhaseUnitMetterOrCycle.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_MaxRatioOfLambda
            // 
            this.namedFloatControl_MaxRatioOfLambda.Location = new System.Drawing.Point(10, 71);
            this.namedFloatControl_MaxRatioOfLambda.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_MaxRatioOfLambda.Name = "namedFloatControl_MaxRatioOfLambda";
            this.namedFloatControl_MaxRatioOfLambda.Size = new System.Drawing.Size(247, 23);
            this.namedFloatControl_MaxRatioOfLambda.TabIndex = 39;
            this.namedFloatControl_MaxRatioOfLambda.Title = " Lambda算法的最大Ratio值：";
            this.namedFloatControl_MaxRatioOfLambda.Value = 0.1D;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 12);
            this.label1.TabIndex = 50;
            this.label1.Text = "若小于此比例，则不使用固定值";
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
            // namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat
            // 
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.Location = new System.Drawing.Point(219, 48);
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.Name = "namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat";
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.Size = new System.Drawing.Size(188, 23);
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.TabIndex = 39;
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.Title = "四舍五入最大偏差：";
            this.namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat.Value = 0.1D;
            // 
            // AmbiguityOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AmbiguityOptionPage";
            this.Size = new System.Drawing.Size(566, 493);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_receiver.ResumeLayout(false);
            this.tabPage_receiver.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_receiver;
        private System.Windows.Forms.CheckBox checkBox_IsFixingAmbiguity;
        private System.Windows.Forms.CheckBox checkBox_IsPhaseUnitMetterOrCycle;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_MinFixedAmbiRatio;
        private System.Windows.Forms.Label label1;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_MaxRatioOfLambda;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_MaxFloatRmsNormToFixAmbiguity;
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_ambiguityFile;
        private System.Windows.Forms.CheckBox checkBox_ambiguityFile;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1MaxAmbiDifferOfIntAndFloat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_IsUseFixedParamDirectly;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_IsFixParamByConditionOrHugeWeight;
        private System.Windows.Forms.CheckBox checkBox_IsRealTimeAmbiFixWhenOuterAmbiFileFailed;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox checkBox_IsEnableSiteSatPeriodDataService;
        private System.Windows.Forms.CheckBox checkBox_IsOutputPeriodData;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_MaxAllowedRmsOfMw;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_MaxRoundAmbiDifferOfIntAndFloat;
    }
}