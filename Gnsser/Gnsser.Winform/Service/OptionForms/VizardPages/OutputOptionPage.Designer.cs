namespace Gnsser.Winform
{
    partial class OutputOptionPage
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
            this.tabPage_output = new System.Windows.Forms.TabPage();
            this.checkBox_IsOpenReportWhenCompleted = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_ = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_outEpochInterval = new Geo.Winform.Controls.NamedFloatControl();
            this.checkBox_epochDops = new System.Windows.Forms.CheckBox();
            this.checkBox_EpochCoord = new System.Windows.Forms.CheckBox();
            this.checkBox_outputEpochInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOutputAdjustMatrix = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOutputObsEquation = new System.Windows.Forms.CheckBox();
            this.checkBox_outputAdjust = new System.Windows.Forms.CheckBox();
            this.checkBox_residual = new System.Windows.Forms.CheckBox();
            this.checkBox_observation = new System.Windows.Forms.CheckBox();
            this.checkBox_outputSiteSat = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOutputJumpClockFile = new System.Windows.Forms.CheckBox();
            this.checkBoxIonoOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxIsOutputWetTrop = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsOutputInGnsserFormat = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsOutputSinex = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOutputSummery = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOutputCycleSlipFile = new System.Windows.Forms.CheckBox();
            this.checkBox_outputResult = new System.Windows.Forms.CheckBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox_OutputBufferCount = new System.Windows.Forms.TextBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.checkBox_IsOutputEpochParam = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOutputEpochParamRms = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage_output.SuspendLayout();
            this.groupBox_.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_output);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(669, 526);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_output
            // 
            this.tabPage_output.Controls.Add(this.checkBox_IsOpenReportWhenCompleted);
            this.tabPage_output.Controls.Add(this.label1);
            this.tabPage_output.Controls.Add(this.groupBox_);
            this.tabPage_output.Controls.Add(this.groupBox2);
            this.tabPage_output.Controls.Add(this.groupBox1);
            this.tabPage_output.Controls.Add(this.checkBox_outputResult);
            this.tabPage_output.Controls.Add(this.groupBox18);
            this.tabPage_output.Controls.Add(this.directorySelectionControl1);
            this.tabPage_output.Location = new System.Drawing.Point(4, 22);
            this.tabPage_output.Name = "tabPage_output";
            this.tabPage_output.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_output.Size = new System.Drawing.Size(661, 500);
            this.tabPage_output.TabIndex = 5;
            this.tabPage_output.Text = "输出";
            this.tabPage_output.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOpenReportWhenCompleted
            // 
            this.checkBox_IsOpenReportWhenCompleted.AutoSize = true;
            this.checkBox_IsOpenReportWhenCompleted.Location = new System.Drawing.Point(301, 261);
            this.checkBox_IsOpenReportWhenCompleted.Name = "checkBox_IsOpenReportWhenCompleted";
            this.checkBox_IsOpenReportWhenCompleted.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsOpenReportWhenCompleted.TabIndex = 76;
            this.checkBox_IsOpenReportWhenCompleted.Text = "计算完后打开报表";
            this.checkBox_IsOpenReportWhenCompleted.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 12);
            this.label1.TabIndex = 75;
            this.label1.Text = "如果不输出，可以节约内存和时间";
            // 
            // groupBox_
            // 
            this.groupBox_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_.Controls.Add(this.namedFloatControl_outEpochInterval);
            this.groupBox_.Controls.Add(this.checkBox_epochDops);
            this.groupBox_.Controls.Add(this.checkBox_IsOutputEpochParamRms);
            this.groupBox_.Controls.Add(this.checkBox_IsOutputEpochParam);
            this.groupBox_.Controls.Add(this.checkBox_EpochCoord);
            this.groupBox_.Controls.Add(this.checkBox_outputEpochInfo);
            this.groupBox_.Controls.Add(this.checkBox_IsOutputAdjustMatrix);
            this.groupBox_.Controls.Add(this.checkBox_IsOutputObsEquation);
            this.groupBox_.Controls.Add(this.checkBox_outputAdjust);
            this.groupBox_.Controls.Add(this.checkBox_residual);
            this.groupBox_.Controls.Add(this.checkBox_observation);
            this.groupBox_.Controls.Add(this.checkBox_outputSiteSat);
            this.groupBox_.Controls.Add(this.checkBox_IsOutputJumpClockFile);
            this.groupBox_.Controls.Add(this.checkBoxIonoOutput);
            this.groupBox_.Controls.Add(this.checkBoxIsOutputWetTrop);
            this.groupBox_.Location = new System.Drawing.Point(6, 48);
            this.groupBox_.Name = "groupBox_";
            this.groupBox_.Size = new System.Drawing.Size(649, 131);
            this.groupBox_.TabIndex = 74;
            this.groupBox_.TabStop = false;
            this.groupBox_.Text = "历元输出";
            // 
            // namedFloatControl_outEpochInterval
            // 
            this.namedFloatControl_outEpochInterval.Location = new System.Drawing.Point(359, 20);
            this.namedFloatControl_outEpochInterval.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_outEpochInterval.Name = "namedFloatControl_outEpochInterval";
            this.namedFloatControl_outEpochInterval.Size = new System.Drawing.Size(185, 23);
            this.namedFloatControl_outEpochInterval.TabIndex = 73;
            this.namedFloatControl_outEpochInterval.Title = "历元输出最小间隔(s)：";
            this.namedFloatControl_outEpochInterval.Value = 0.1D;
            // 
            // checkBox_epochDops
            // 
            this.checkBox_epochDops.AutoSize = true;
            this.checkBox_epochDops.Location = new System.Drawing.Point(15, 98);
            this.checkBox_epochDops.Name = "checkBox_epochDops";
            this.checkBox_epochDops.Size = new System.Drawing.Size(66, 16);
            this.checkBox_epochDops.TabIndex = 70;
            this.checkBox_epochDops.Text = "历元DOP";
            this.checkBox_epochDops.UseVisualStyleBackColor = true;
            // 
            // checkBox_EpochCoord
            // 
            this.checkBox_EpochCoord.AutoSize = true;
            this.checkBox_EpochCoord.Location = new System.Drawing.Point(15, 59);
            this.checkBox_EpochCoord.Name = "checkBox_EpochCoord";
            this.checkBox_EpochCoord.Size = new System.Drawing.Size(72, 16);
            this.checkBox_EpochCoord.TabIndex = 70;
            this.checkBox_EpochCoord.Text = "历元坐标";
            this.checkBox_EpochCoord.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputEpochInfo
            // 
            this.checkBox_outputEpochInfo.AutoSize = true;
            this.checkBox_outputEpochInfo.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkBox_outputEpochInfo.Location = new System.Drawing.Point(15, 20);
            this.checkBox_outputEpochInfo.Name = "checkBox_outputEpochInfo";
            this.checkBox_outputEpochInfo.Size = new System.Drawing.Size(108, 16);
            this.checkBox_outputEpochInfo.TabIndex = 70;
            this.checkBox_outputEpochInfo.Text = "历元输出总开关";
            this.checkBox_outputEpochInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOutputAdjustMatrix
            // 
            this.checkBox_IsOutputAdjustMatrix.AutoSize = true;
            this.checkBox_IsOutputAdjustMatrix.Location = new System.Drawing.Point(520, 59);
            this.checkBox_IsOutputAdjustMatrix.Name = "checkBox_IsOutputAdjustMatrix";
            this.checkBox_IsOutputAdjustMatrix.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsOutputAdjustMatrix.TabIndex = 72;
            this.checkBox_IsOutputAdjustMatrix.Text = "历元平差文本文件";
            this.checkBox_IsOutputAdjustMatrix.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOutputObsEquation
            // 
            this.checkBox_IsOutputObsEquation.AutoSize = true;
            this.checkBox_IsOutputObsEquation.Location = new System.Drawing.Point(442, 98);
            this.checkBox_IsOutputObsEquation.Name = "checkBox_IsOutputObsEquation";
            this.checkBox_IsOutputObsEquation.Size = new System.Drawing.Size(114, 16);
            this.checkBox_IsOutputObsEquation.TabIndex = 72;
            this.checkBox_IsOutputObsEquation.Text = "历元观测AIO文件";
            this.checkBox_IsOutputObsEquation.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputAdjust
            // 
            this.checkBox_outputAdjust.AutoSize = true;
            this.checkBox_outputAdjust.Location = new System.Drawing.Point(322, 98);
            this.checkBox_outputAdjust.Name = "checkBox_outputAdjust";
            this.checkBox_outputAdjust.Size = new System.Drawing.Size(114, 16);
            this.checkBox_outputAdjust.TabIndex = 72;
            this.checkBox_outputAdjust.Text = "历元平差AIO文件";
            this.checkBox_outputAdjust.UseVisualStyleBackColor = true;
            // 
            // checkBox_residual
            // 
            this.checkBox_residual.AutoSize = true;
            this.checkBox_residual.Location = new System.Drawing.Point(89, 59);
            this.checkBox_residual.Name = "checkBox_residual";
            this.checkBox_residual.Size = new System.Drawing.Size(96, 16);
            this.checkBox_residual.TabIndex = 71;
            this.checkBox_residual.Text = "历元算后残差";
            this.checkBox_residual.UseVisualStyleBackColor = true;
            // 
            // checkBox_observation
            // 
            this.checkBox_observation.AutoSize = true;
            this.checkBox_observation.Location = new System.Drawing.Point(208, 59);
            this.checkBox_observation.Name = "checkBox_observation";
            this.checkBox_observation.Size = new System.Drawing.Size(96, 16);
            this.checkBox_observation.TabIndex = 71;
            this.checkBox_observation.Text = "历元观测残差";
            this.checkBox_observation.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputSiteSat
            // 
            this.checkBox_outputSiteSat.AutoSize = true;
            this.checkBox_outputSiteSat.Location = new System.Drawing.Point(322, 59);
            this.checkBox_outputSiteSat.Name = "checkBox_outputSiteSat";
            this.checkBox_outputSiteSat.Size = new System.Drawing.Size(96, 16);
            this.checkBox_outputSiteSat.TabIndex = 71;
            this.checkBox_outputSiteSat.Text = "历元站星信息";
            this.checkBox_outputSiteSat.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOutputJumpClockFile
            // 
            this.checkBox_IsOutputJumpClockFile.AutoSize = true;
            this.checkBox_IsOutputJumpClockFile.Location = new System.Drawing.Point(442, 59);
            this.checkBox_IsOutputJumpClockFile.Name = "checkBox_IsOutputJumpClockFile";
            this.checkBox_IsOutputJumpClockFile.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsOutputJumpClockFile.TabIndex = 0;
            this.checkBox_IsOutputJumpClockFile.Text = "输出钟跳";
            this.checkBox_IsOutputJumpClockFile.UseVisualStyleBackColor = true;
            // 
            // checkBoxIonoOutput
            // 
            this.checkBoxIonoOutput.AutoSize = true;
            this.checkBoxIonoOutput.Location = new System.Drawing.Point(89, 98);
            this.checkBoxIonoOutput.Name = "checkBoxIonoOutput";
            this.checkBoxIonoOutput.Size = new System.Drawing.Size(108, 16);
            this.checkBoxIonoOutput.TabIndex = 0;
            this.checkBoxIonoOutput.Text = "历元电离层结果";
            this.checkBoxIonoOutput.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsOutputWetTrop
            // 
            this.checkBoxIsOutputWetTrop.AutoSize = true;
            this.checkBoxIsOutputWetTrop.Location = new System.Drawing.Point(208, 98);
            this.checkBoxIsOutputWetTrop.Name = "checkBoxIsOutputWetTrop";
            this.checkBoxIsOutputWetTrop.Size = new System.Drawing.Size(108, 16);
            this.checkBoxIsOutputWetTrop.TabIndex = 0;
            this.checkBoxIsOutputWetTrop.Text = "历元对流层结果";
            this.checkBoxIsOutputWetTrop.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_IsOutputInGnsserFormat);
            this.groupBox2.Location = new System.Drawing.Point(7, 299);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(457, 54);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "预处理输出";
            // 
            // checkBox_IsOutputInGnsserFormat
            // 
            this.checkBox_IsOutputInGnsserFormat.AutoSize = true;
            this.checkBox_IsOutputInGnsserFormat.Location = new System.Drawing.Point(19, 20);
            this.checkBox_IsOutputInGnsserFormat.Name = "checkBox_IsOutputInGnsserFormat";
            this.checkBox_IsOutputInGnsserFormat.Size = new System.Drawing.Size(156, 16);
            this.checkBox_IsOutputInGnsserFormat.TabIndex = 0;
            this.checkBox_IsOutputInGnsserFormat.Text = "输出为 GNSSer 格式文件";
            this.checkBox_IsOutputInGnsserFormat.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_IsOutputSinex);
            this.groupBox1.Controls.Add(this.checkBox_IsOutputSummery);
            this.groupBox1.Controls.Add(this.checkBox_IsOutputCycleSlipFile);
            this.groupBox1.Location = new System.Drawing.Point(6, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 54);
            this.groupBox1.TabIndex = 73;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "最终结果";
            // 
            // checkBox_IsOutputSinex
            // 
            this.checkBox_IsOutputSinex.AutoSize = true;
            this.checkBox_IsOutputSinex.Location = new System.Drawing.Point(19, 20);
            this.checkBox_IsOutputSinex.Name = "checkBox_IsOutputSinex";
            this.checkBox_IsOutputSinex.Size = new System.Drawing.Size(114, 16);
            this.checkBox_IsOutputSinex.TabIndex = 0;
            this.checkBox_IsOutputSinex.Text = "输出 SINEX 文件";
            this.checkBox_IsOutputSinex.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOutputSummery
            // 
            this.checkBox_IsOutputSummery.AutoSize = true;
            this.checkBox_IsOutputSummery.Location = new System.Drawing.Point(145, 20);
            this.checkBox_IsOutputSummery.Name = "checkBox_IsOutputSummery";
            this.checkBox_IsOutputSummery.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsOutputSummery.TabIndex = 0;
            this.checkBox_IsOutputSummery.Text = "输出结果汇总文件";
            this.checkBox_IsOutputSummery.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOutputCycleSlipFile
            // 
            this.checkBox_IsOutputCycleSlipFile.AutoSize = true;
            this.checkBox_IsOutputCycleSlipFile.Location = new System.Drawing.Point(271, 20);
            this.checkBox_IsOutputCycleSlipFile.Name = "checkBox_IsOutputCycleSlipFile";
            this.checkBox_IsOutputCycleSlipFile.Size = new System.Drawing.Size(144, 16);
            this.checkBox_IsOutputCycleSlipFile.TabIndex = 0;
            this.checkBox_IsOutputCycleSlipFile.Text = "是否输出周跳结果文件";
            this.checkBox_IsOutputCycleSlipFile.UseVisualStyleBackColor = true;
            // 
            // checkBox_outputResult
            // 
            this.checkBox_outputResult.AutoSize = true;
            this.checkBox_outputResult.ForeColor = System.Drawing.Color.Red;
            this.checkBox_outputResult.Location = new System.Drawing.Point(24, 16);
            this.checkBox_outputResult.Name = "checkBox_outputResult";
            this.checkBox_outputResult.Size = new System.Drawing.Size(132, 16);
            this.checkBox_outputResult.TabIndex = 69;
            this.checkBox_outputResult.Text = "输出结果文件总开关";
            this.checkBox_outputResult.UseVisualStyleBackColor = true;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.label21);
            this.groupBox18.Controls.Add(this.textBox_OutputBufferCount);
            this.groupBox18.Location = new System.Drawing.Point(7, 245);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(262, 48);
            this.groupBox18.TabIndex = 68;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "缓存设置";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(20, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(125, 12);
            this.label21.TabIndex = 1;
            this.label21.Text = "输出结果缓存历元数：";
            // 
            // textBox_OutputBufferCount
            // 
            this.textBox_OutputBufferCount.Location = new System.Drawing.Point(163, 17);
            this.textBox_OutputBufferCount.Name = "textBox_OutputBufferCount";
            this.textBox_OutputBufferCount.Size = new System.Drawing.Size(89, 21);
            this.textBox_OutputBufferCount.TabIndex = 2;
            this.textBox_OutputBufferCount.Text = "100000";
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(7, 380);
            this.directorySelectionControl1.Margin = new System.Windows.Forms.Padding(4);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(569, 22);
            this.directorySelectionControl1.TabIndex = 2;
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
            // checkBox_IsOutputEpochParam
            // 
            this.checkBox_IsOutputEpochParam.AutoSize = true;
            this.checkBox_IsOutputEpochParam.Location = new System.Drawing.Point(129, 20);
            this.checkBox_IsOutputEpochParam.Name = "checkBox_IsOutputEpochParam";
            this.checkBox_IsOutputEpochParam.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsOutputEpochParam.TabIndex = 70;
            this.checkBox_IsOutputEpochParam.Text = "历元参数";
            this.checkBox_IsOutputEpochParam.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOutputEpochParamRms
            // 
            this.checkBox_IsOutputEpochParamRms.AutoSize = true;
            this.checkBox_IsOutputEpochParamRms.Location = new System.Drawing.Point(232, 20);
            this.checkBox_IsOutputEpochParamRms.Name = "checkBox_IsOutputEpochParamRms";
            this.checkBox_IsOutputEpochParamRms.Size = new System.Drawing.Size(90, 16);
            this.checkBox_IsOutputEpochParamRms.TabIndex = 70;
            this.checkBox_IsOutputEpochParamRms.Text = "历元参数RMS";
            this.checkBox_IsOutputEpochParamRms.UseVisualStyleBackColor = true;
            // 
            // OutputOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "OutputOptionPage";
            this.Size = new System.Drawing.Size(669, 526);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_output.ResumeLayout(false);
            this.tabPage_output.PerformLayout();
            this.groupBox_.ResumeLayout(false);
            this.groupBox_.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.TabPage tabPage_output;
        private System.Windows.Forms.CheckBox checkBox_IsOutputCycleSlipFile;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox_OutputBufferCount;
        private System.Windows.Forms.CheckBox checkBoxIsOutputWetTrop;
        private System.Windows.Forms.CheckBox checkBoxIonoOutput;
        private System.Windows.Forms.CheckBox checkBox_outputResult;
        private System.Windows.Forms.CheckBox checkBox_outputEpochInfo;
        private System.Windows.Forms.CheckBox checkBox_outputSiteSat;
        private System.Windows.Forms.CheckBox checkBox_outputAdjust;
        private System.Windows.Forms.CheckBox checkBox_IsOutputSinex;
        private System.Windows.Forms.CheckBox checkBox_IsOutputSummery;
        private System.Windows.Forms.GroupBox groupBox_;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_IsOutputAdjustMatrix;
        private System.Windows.Forms.CheckBox checkBox_epochDops;
        private System.Windows.Forms.CheckBox checkBox_EpochCoord;
        private System.Windows.Forms.CheckBox checkBox_observation;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_outEpochInterval;
        private System.Windows.Forms.CheckBox checkBox_IsOpenReportWhenCompleted;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_IsOutputInGnsserFormat;
        private System.Windows.Forms.CheckBox checkBox_IsOutputJumpClockFile;
        private System.Windows.Forms.CheckBox checkBox_residual;
        private System.Windows.Forms.CheckBox checkBox_IsOutputObsEquation;
        private System.Windows.Forms.CheckBox checkBox_IsOutputEpochParamRms;
        private System.Windows.Forms.CheckBox checkBox_IsOutputEpochParam;
    }
}