namespace Gnsser.Winform
{
    partial class ObsAndApproxOptionPage
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
            this.tabPage_caculator = new System.Windows.Forms.TabPage();
            this.enumRadioControl_ObsPhaseType = new Geo.Winform.EnumRadioControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_enableApriori = new System.Windows.Forms.CheckBox();
            this.namedArrayControl_apriori = new Geo.Winform.Controls.NamedArrayControl();
            this.namedArrayControl_rmsOfApriori = new Geo.Winform.Controls.NamedArrayControl();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.checkBox_isSmoothEpcohes = new System.Windows.Forms.CheckBox();
            this.checkBox_isRequireSameSats = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_MultiEpochSameSatCount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_multiEpochCount = new System.Windows.Forms.TextBox();
            this.satApproxDataTypeControl1 = new Gnsser.Winform.Controls.SatApproxDataTypeControl();
            this.satObsDataTypeControl1 = new Gnsser.Winform.Controls.SatObsDataTypeControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.enumRadioControl_BaseSiteSelectType = new Geo.Winform.EnumRadioControl();
            this.tabControl1.SuspendLayout();
            this.tabPage_caculator.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_caculator);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(653, 507);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_caculator
            // 
            this.tabPage_caculator.Controls.Add(this.enumRadioControl_BaseSiteSelectType);
            this.tabPage_caculator.Controls.Add(this.enumRadioControl_ObsPhaseType);
            this.tabPage_caculator.Controls.Add(this.groupBox1);
            this.tabPage_caculator.Controls.Add(this.groupBox14);
            this.tabPage_caculator.Controls.Add(this.satApproxDataTypeControl1);
            this.tabPage_caculator.Controls.Add(this.satObsDataTypeControl1);
            this.tabPage_caculator.Location = new System.Drawing.Point(4, 22);
            this.tabPage_caculator.Name = "tabPage_caculator";
            this.tabPage_caculator.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_caculator.Size = new System.Drawing.Size(645, 481);
            this.tabPage_caculator.TabIndex = 4;
            this.tabPage_caculator.Text = "观测值近似值";
            this.tabPage_caculator.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_ObsPhaseType
            // 
            this.enumRadioControl_ObsPhaseType.IsReady = false;
            this.enumRadioControl_ObsPhaseType.Location = new System.Drawing.Point(6, 218);
            this.enumRadioControl_ObsPhaseType.Name = "enumRadioControl_ObsPhaseType";
            this.enumRadioControl_ObsPhaseType.Size = new System.Drawing.Size(627, 42);
            this.enumRadioControl_ObsPhaseType.TabIndex = 74;
            this.enumRadioControl_ObsPhaseType.Title = "频率选项";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_enableApriori);
            this.groupBox1.Controls.Add(this.namedArrayControl_apriori);
            this.groupBox1.Controls.Add(this.namedArrayControl_rmsOfApriori);
            this.groupBox1.Location = new System.Drawing.Point(6, 395);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 69);
            this.groupBox1.TabIndex = 73;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "初始先验信息";
            // 
            // checkBox_enableApriori
            // 
            this.checkBox_enableApriori.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enableApriori.AutoSize = true;
            this.checkBox_enableApriori.Location = new System.Drawing.Point(570, 30);
            this.checkBox_enableApriori.Name = "checkBox_enableApriori";
            this.checkBox_enableApriori.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enableApriori.TabIndex = 47;
            this.checkBox_enableApriori.Text = "启用";
            this.checkBox_enableApriori.UseVisualStyleBackColor = true;
            // 
            // namedArrayControl_apriori
            // 
            this.namedArrayControl_apriori.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedArrayControl_apriori.Location = new System.Drawing.Point(15, 18);
            this.namedArrayControl_apriori.Margin = new System.Windows.Forms.Padding(4);
            this.namedArrayControl_apriori.Name = "namedArrayControl_apriori";
            this.namedArrayControl_apriori.Size = new System.Drawing.Size(535, 23);
            this.namedArrayControl_apriori.TabIndex = 71;
            this.namedArrayControl_apriori.Title = "初始先验值：";
            // 
            // namedArrayControl_rmsOfApriori
            // 
            this.namedArrayControl_rmsOfApriori.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedArrayControl_rmsOfApriori.Location = new System.Drawing.Point(15, 43);
            this.namedArrayControl_rmsOfApriori.Margin = new System.Windows.Forms.Padding(4);
            this.namedArrayControl_rmsOfApriori.Name = "namedArrayControl_rmsOfApriori";
            this.namedArrayControl_rmsOfApriori.Size = new System.Drawing.Size(535, 23);
            this.namedArrayControl_rmsOfApriori.TabIndex = 71;
            this.namedArrayControl_rmsOfApriori.Title = "初始先验值（RMS）：";
            // 
            // groupBox14
            // 
            this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox14.Controls.Add(this.checkBox_isSmoothEpcohes);
            this.groupBox14.Controls.Add(this.checkBox_isRequireSameSats);
            this.groupBox14.Controls.Add(this.label7);
            this.groupBox14.Controls.Add(this.textBox_MultiEpochSameSatCount);
            this.groupBox14.Controls.Add(this.label8);
            this.groupBox14.Controls.Add(this.textBox_multiEpochCount);
            this.groupBox14.Location = new System.Drawing.Point(6, 266);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(627, 72);
            this.groupBox14.TabIndex = 6;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "多历元对象";
            // 
            // checkBox_isSmoothEpcohes
            // 
            this.checkBox_isSmoothEpcohes.AutoSize = true;
            this.checkBox_isSmoothEpcohes.Location = new System.Drawing.Point(193, 39);
            this.checkBox_isSmoothEpcohes.Name = "checkBox_isSmoothEpcohes";
            this.checkBox_isSmoothEpcohes.Size = new System.Drawing.Size(180, 16);
            this.checkBox_isSmoothEpcohes.TabIndex = 47;
            this.checkBox_isSmoothEpcohes.Text = "逐历元，否则窗口无重复历元";
            this.checkBox_isSmoothEpcohes.UseVisualStyleBackColor = true;
            // 
            // checkBox_isRequireSameSats
            // 
            this.checkBox_isRequireSameSats.AutoSize = true;
            this.checkBox_isRequireSameSats.Location = new System.Drawing.Point(193, 17);
            this.checkBox_isRequireSameSats.Name = "checkBox_isRequireSameSats";
            this.checkBox_isRequireSameSats.Size = new System.Drawing.Size(96, 16);
            this.checkBox_isRequireSameSats.TabIndex = 47;
            this.checkBox_isRequireSameSats.Text = "需要共视卫星";
            this.checkBox_isRequireSameSats.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(65, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "历元数量：";
            // 
            // textBox_MultiEpochSameSatCount
            // 
            this.textBox_MultiEpochSameSatCount.Location = new System.Drawing.Point(134, 39);
            this.textBox_MultiEpochSameSatCount.Name = "textBox_MultiEpochSameSatCount";
            this.textBox_MultiEpochSameSatCount.Size = new System.Drawing.Size(53, 21);
            this.textBox_MultiEpochSameSatCount.TabIndex = 4;
            this.textBox_MultiEpochSameSatCount.Text = "5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "多历元共视卫星数量：";
            // 
            // textBox_multiEpochCount
            // 
            this.textBox_multiEpochCount.Location = new System.Drawing.Point(134, 12);
            this.textBox_multiEpochCount.Name = "textBox_multiEpochCount";
            this.textBox_multiEpochCount.Size = new System.Drawing.Size(53, 21);
            this.textBox_multiEpochCount.TabIndex = 4;
            this.textBox_multiEpochCount.Text = "4";
            // 
            // satApproxDataTypeControl1
            // 
            this.satApproxDataTypeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.satApproxDataTypeControl1.CurrentdType = Gnsser.SatApproxDataType.IonoFreeApproxPseudoRange;
            this.satApproxDataTypeControl1.Location = new System.Drawing.Point(2, 118);
            this.satApproxDataTypeControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.satApproxDataTypeControl1.Name = "satApproxDataTypeControl1";
            this.satApproxDataTypeControl1.Size = new System.Drawing.Size(631, 94);
            this.satApproxDataTypeControl1.TabIndex = 1;
            // 
            // satObsDataTypeControl1
            // 
            this.satObsDataTypeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.satObsDataTypeControl1.CurrentdType = Gnsser.SatObsDataType.AlignedIonoFreePhaseRange;
            this.satObsDataTypeControl1.Location = new System.Drawing.Point(2, 5);
            this.satObsDataTypeControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.satObsDataTypeControl1.Name = "satObsDataTypeControl1";
            this.satObsDataTypeControl1.Size = new System.Drawing.Size(631, 109);
            this.satObsDataTypeControl1.TabIndex = 0;
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
            // enumRadioControl_BaseSiteSelectType
            // 
            this.enumRadioControl_BaseSiteSelectType.IsReady = false;
            this.enumRadioControl_BaseSiteSelectType.Location = new System.Drawing.Point(6, 338);
            this.enumRadioControl_BaseSiteSelectType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_BaseSiteSelectType.Name = "enumRadioControl_BaseSiteSelectType";
            this.enumRadioControl_BaseSiteSelectType.Size = new System.Drawing.Size(627, 50);
            this.enumRadioControl_BaseSiteSelectType.TabIndex = 51;
            this.enumRadioControl_BaseSiteSelectType.Title = "基准测站选择方法";
            // 
            // ObsAndApproxOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ObsAndApproxOptionPage";
            this.Size = new System.Drawing.Size(653, 507);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_caculator.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private Controls.SatObsDataTypeControl satObsDataTypeControl1;
        private System.Windows.Forms.TabPage tabPage_caculator;
        private Controls.SatApproxDataTypeControl satApproxDataTypeControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.CheckBox checkBox_isRequireSameSats;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_MultiEpochSameSatCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_multiEpochCount;
        private System.Windows.Forms.CheckBox checkBox_isSmoothEpcohes;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_enableApriori;
        private Geo.Winform.Controls.NamedArrayControl namedArrayControl_apriori;
        private Geo.Winform.Controls.NamedArrayControl namedArrayControl_rmsOfApriori;
        private Geo.Winform.EnumRadioControl enumRadioControl_ObsPhaseType;
        private Geo.Winform.EnumRadioControl enumRadioControl_BaseSiteSelectType;
    }
}