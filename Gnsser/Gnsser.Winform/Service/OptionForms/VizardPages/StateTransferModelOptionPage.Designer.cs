namespace Gnsser.Winform
{
    partial class StateTransferModelOptionPage
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsPromoteTransWhenResultValueBreak = new System.Windows.Forms.CheckBox();
            this.namedFloatControl_PhaseCovaProportionToRange = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl6StdDevOfWhiteNoiseModel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl5StdDevOfTropoRandomWalkModel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl4StdDevOfStaticTransferModel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl3StdDevOfIonoRandomWalkModel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1StdDevOfCycledPhaseModel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl1StdDevOfRandomWalkModel = new Geo.Winform.Controls.NamedFloatControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel = new Geo.Winform.Controls.NamedFloatControl();
            this.tabControl1.SuspendLayout();
            this.tabPage_output.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_output);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(936, 658);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_output
            // 
            this.tabPage_output.Controls.Add(this.groupBox5);
            this.tabPage_output.Controls.Add(this.namedFloatControl_PhaseCovaProportionToRange);
            this.tabPage_output.Controls.Add(this.namedFloatControl1StdDevOfSysTimeRandomWalkModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos);
            this.tabPage_output.Controls.Add(this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl6StdDevOfWhiteNoiseModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl5StdDevOfTropoRandomWalkModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl4StdDevOfStaticTransferModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl3StdDevOfIonoRandomWalkModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl1StdDevOfCycledPhaseModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl2StdDevOfPhaseAmbiguityModel);
            this.tabPage_output.Controls.Add(this.namedFloatControl1StdDevOfRandomWalkModel);
            this.tabPage_output.Location = new System.Drawing.Point(4, 25);
            this.tabPage_output.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_output.Name = "tabPage_output";
            this.tabPage_output.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_output.Size = new System.Drawing.Size(928, 629);
            this.tabPage_output.TabIndex = 5;
            this.tabPage_output.Text = "状态转移模型参数设置";
            this.tabPage_output.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox_IsPromoteTransWhenResultValueBreak);
            this.groupBox5.Location = new System.Drawing.Point(424, 42);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(281, 65);
            this.groupBox5.TabIndex = 70;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "结果控制";
            // 
            // checkBox_IsPromoteTransWhenResultValueBreak
            // 
            this.checkBox_IsPromoteTransWhenResultValueBreak.AutoSize = true;
            this.checkBox_IsPromoteTransWhenResultValueBreak.Location = new System.Drawing.Point(8, 28);
            this.checkBox_IsPromoteTransWhenResultValueBreak.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_IsPromoteTransWhenResultValueBreak.Name = "checkBox_IsPromoteTransWhenResultValueBreak";
            this.checkBox_IsPromoteTransWhenResultValueBreak.Size = new System.Drawing.Size(179, 19);
            this.checkBox_IsPromoteTransWhenResultValueBreak.TabIndex = 47;
            this.checkBox_IsPromoteTransWhenResultValueBreak.Text = "当结果值断裂是否升噪";
            this.checkBox_IsPromoteTransWhenResultValueBreak.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_PhaseCovaProportionToRange
            // 
            this.namedFloatControl_PhaseCovaProportionToRange.Location = new System.Drawing.Point(400, 152);
            this.namedFloatControl_PhaseCovaProportionToRange.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl_PhaseCovaProportionToRange.Name = "namedFloatControl_PhaseCovaProportionToRange";
            this.namedFloatControl_PhaseCovaProportionToRange.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl_PhaseCovaProportionToRange.TabIndex = 0;
            this.namedFloatControl_PhaseCovaProportionToRange.Title = "载波与伪距方差比：";
            this.namedFloatControl_PhaseCovaProportionToRange.Value = 0.1D;
            // 
            // namedFloatControl1StdDevOfSysTimeRandomWalkModel
            // 
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Location = new System.Drawing.Point(31, 367);
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Name = "namedFloatControl1StdDevOfSysTimeRandomWalkModel";
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.TabIndex = 0;
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Title = "多系统时间随机游走模型：";
            this.namedFloatControl1StdDevOfSysTimeRandomWalkModel.Value = 0.1D;
            // 
            // namedFloatControl_rmsOfWhitenoiceOfDynamicPos
            // 
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Location = new System.Drawing.Point(31, 406);
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Name = "namedFloatControl_rmsOfWhitenoiceOfDynamicPos";
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.TabIndex = 0;
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Title = "动态定位白噪声模型：";
            this.namedFloatControl_rmsOfWhitenoiceOfDynamicPos.Value = 0.1D;
            // 
            // namedFloatControl6StdDevOfWhiteNoiseModel
            // 
            this.namedFloatControl6StdDevOfWhiteNoiseModel.Location = new System.Drawing.Point(31, 292);
            this.namedFloatControl6StdDevOfWhiteNoiseModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl6StdDevOfWhiteNoiseModel.Name = "namedFloatControl6StdDevOfWhiteNoiseModel";
            this.namedFloatControl6StdDevOfWhiteNoiseModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl6StdDevOfWhiteNoiseModel.TabIndex = 0;
            this.namedFloatControl6StdDevOfWhiteNoiseModel.Title = "接收机钟差白噪声模型：";
            this.namedFloatControl6StdDevOfWhiteNoiseModel.Value = 0.1D;
            // 
            // namedFloatControl5StdDevOfTropoRandomWalkModel
            // 
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.Location = new System.Drawing.Point(31, 245);
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.Name = "namedFloatControl5StdDevOfTropoRandomWalkModel";
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.TabIndex = 0;
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.Title = "对流层随机游走模型：";
            this.namedFloatControl5StdDevOfTropoRandomWalkModel.Value = 0.1D;
            // 
            // namedFloatControl4StdDevOfStaticTransferModel
            // 
            this.namedFloatControl4StdDevOfStaticTransferModel.Location = new System.Drawing.Point(31, 198);
            this.namedFloatControl4StdDevOfStaticTransferModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl4StdDevOfStaticTransferModel.Name = "namedFloatControl4StdDevOfStaticTransferModel";
            this.namedFloatControl4StdDevOfStaticTransferModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl4StdDevOfStaticTransferModel.TabIndex = 0;
            this.namedFloatControl4StdDevOfStaticTransferModel.Title = "静态模型：";
            this.namedFloatControl4StdDevOfStaticTransferModel.Value = 0.1D;
            // 
            // namedFloatControl3StdDevOfIonoRandomWalkModel
            // 
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.Location = new System.Drawing.Point(31, 150);
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.Name = "namedFloatControl3StdDevOfIonoRandomWalkModel";
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.TabIndex = 0;
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.Title = "电离层随机游走模型：";
            this.namedFloatControl3StdDevOfIonoRandomWalkModel.Value = 0.1D;
            // 
            // namedFloatControl1StdDevOfCycledPhaseModel
            // 
            this.namedFloatControl1StdDevOfCycledPhaseModel.Location = new System.Drawing.Point(31, 115);
            this.namedFloatControl1StdDevOfCycledPhaseModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl1StdDevOfCycledPhaseModel.Name = "namedFloatControl1StdDevOfCycledPhaseModel";
            this.namedFloatControl1StdDevOfCycledPhaseModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl1StdDevOfCycledPhaseModel.TabIndex = 0;
            this.namedFloatControl1StdDevOfCycledPhaseModel.Title = "载波相位周跳模型：";
            this.namedFloatControl1StdDevOfCycledPhaseModel.Value = 0.1D;
            // 
            // namedFloatControl2StdDevOfPhaseAmbiguityModel
            // 
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Location = new System.Drawing.Point(31, 79);
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Name = "namedFloatControl2StdDevOfPhaseAmbiguityModel";
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.TabIndex = 0;
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Title = "载波相位模型：";
            this.namedFloatControl2StdDevOfPhaseAmbiguityModel.Value = 0.1D;
            // 
            // namedFloatControl1StdDevOfRandomWalkModel
            // 
            this.namedFloatControl1StdDevOfRandomWalkModel.Location = new System.Drawing.Point(31, 31);
            this.namedFloatControl1StdDevOfRandomWalkModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.namedFloatControl1StdDevOfRandomWalkModel.Name = "namedFloatControl1StdDevOfRandomWalkModel";
            this.namedFloatControl1StdDevOfRandomWalkModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl1StdDevOfRandomWalkModel.TabIndex = 0;
            this.namedFloatControl1StdDevOfRandomWalkModel.Title = "随机游走模型：";
            this.namedFloatControl1StdDevOfRandomWalkModel.Value = 0.1D;
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
            // namedFloatControl_StdDevOfSatClockWhiteNoiseModel
            // 
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Location = new System.Drawing.Point(31, 331);
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Margin = new System.Windows.Forms.Padding(5);
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Name = "namedFloatControl_StdDevOfSatClockWhiteNoiseModel";
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Size = new System.Drawing.Size(356, 29);
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.TabIndex = 0;
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Title = "卫星钟差白噪声模型：";
            this.namedFloatControl_StdDevOfSatClockWhiteNoiseModel.Value = 0.1D;
            // 
            // StateTransferModelOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "StateTransferModelOptionPage";
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_output.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_output;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl6StdDevOfWhiteNoiseModel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl5StdDevOfTropoRandomWalkModel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl4StdDevOfStaticTransferModel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl3StdDevOfIonoRandomWalkModel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl2StdDevOfPhaseAmbiguityModel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1StdDevOfRandomWalkModel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1StdDevOfSysTimeRandomWalkModel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl1StdDevOfCycledPhaseModel;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_IsPromoteTransWhenResultValueBreak;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_PhaseCovaProportionToRange;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_rmsOfWhitenoiceOfDynamicPos;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_StdDevOfSatClockWhiteNoiseModel;
    }
}