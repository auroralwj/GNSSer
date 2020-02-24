namespace Gnsser.Winform
{
    partial class CorrectionOptionPage
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
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.tabPage_datasource = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_IsObsCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsDcbCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsDcbOfP1P2Enabled = new System.Windows.Forms.CheckBox();
            this.checkBox_IsP1DcbToLcRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSmoothingRange = new System.Windows.Forms.CheckBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_IsApproxModelCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsRangeCorrectionsRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsFrequencyCorrectionsRequired = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_IsSiteCorrectionsRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOceanTideCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsPoleTideCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSolidTideCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_IsSatAntPcoCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsPhaseWindUpCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSatAntPvcCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_IsRecAntennaPcoCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsRecAntennaPcvRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsTropCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSatClockBiasCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsGravitationalDelayCorrectionRequired = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_ionoCorrection = new System.Windows.Forms.CheckBox();
            this.enumRadioControl_IonoSourceTypeForCorrection = new Geo.Winform.EnumRadioControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_datasource.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_clock
            // 
            this.openFileDialog_clock.FileName = "钟差文件";
            this.openFileDialog_clock.Filter = "Clock卫星钟差文件|*.clk;*.clk_30s|所有文件|*.*";
            // 
            // tabPage_datasource
            // 
            this.tabPage_datasource.Controls.Add(this.groupBox1);
            this.tabPage_datasource.Controls.Add(this.groupBox16);
            this.tabPage_datasource.Location = new System.Drawing.Point(4, 22);
            this.tabPage_datasource.Name = "tabPage_datasource";
            this.tabPage_datasource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_datasource.Size = new System.Drawing.Size(626, 470);
            this.tabPage_datasource.TabIndex = 1;
            this.tabPage_datasource.Text = "改正数";
            this.tabPage_datasource.UseVisualStyleBackColor = true;
            this.tabPage_datasource.Click += new System.EventHandler(this.tabPage_datasource_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel3);
            this.groupBox1.Location = new System.Drawing.Point(0, 366);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 98);
            this.groupBox1.TabIndex = 67;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "观测值改正";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.checkBox_IsObsCorrectionRequired);
            this.flowLayoutPanel3.Controls.Add(this.checkBox_IsDcbCorrectionRequired);
            this.flowLayoutPanel3.Controls.Add(this.checkBox_IsDcbOfP1P2Enabled);
            this.flowLayoutPanel3.Controls.Add(this.checkBox_IsP1DcbToLcRequired);
            this.flowLayoutPanel3.Controls.Add(this.checkBox_IsSmoothingRange);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(613, 78);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // checkBox_IsObsCorrectionRequired
            // 
            this.checkBox_IsObsCorrectionRequired.AutoSize = true;
            this.checkBox_IsObsCorrectionRequired.ForeColor = System.Drawing.Color.Red;
            this.checkBox_IsObsCorrectionRequired.Location = new System.Drawing.Point(3, 3);
            this.checkBox_IsObsCorrectionRequired.Name = "checkBox_IsObsCorrectionRequired";
            this.checkBox_IsObsCorrectionRequired.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsObsCorrectionRequired.TabIndex = 63;
            this.checkBox_IsObsCorrectionRequired.Text = "观测值改正总开关";
            this.checkBox_IsObsCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsDcbCorrectionRequired
            // 
            this.checkBox_IsDcbCorrectionRequired.AutoSize = true;
            this.checkBox_IsDcbCorrectionRequired.Location = new System.Drawing.Point(129, 3);
            this.checkBox_IsDcbCorrectionRequired.Name = "checkBox_IsDcbCorrectionRequired";
            this.checkBox_IsDcbCorrectionRequired.Size = new System.Drawing.Size(66, 16);
            this.checkBox_IsDcbCorrectionRequired.TabIndex = 63;
            this.checkBox_IsDcbCorrectionRequired.Text = "DCB改正";
            this.checkBox_IsDcbCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsDcbOfP1P2Enabled
            // 
            this.checkBox_IsDcbOfP1P2Enabled.AutoSize = true;
            this.checkBox_IsDcbOfP1P2Enabled.Location = new System.Drawing.Point(201, 3);
            this.checkBox_IsDcbOfP1P2Enabled.Name = "checkBox_IsDcbOfP1P2Enabled";
            this.checkBox_IsDcbOfP1P2Enabled.Size = new System.Drawing.Size(282, 16);
            this.checkBox_IsDcbOfP1P2Enabled.TabIndex = 47;
            this.checkBox_IsDcbOfP1P2Enabled.Text = "DCB P1P2无电离层改正(单频适用，来自DCB文件)";
            this.checkBox_IsDcbOfP1P2Enabled.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsP1DcbToLcRequired
            // 
            this.checkBox_IsP1DcbToLcRequired.AutoSize = true;
            this.checkBox_IsP1DcbToLcRequired.Location = new System.Drawing.Point(3, 25);
            this.checkBox_IsP1DcbToLcRequired.Name = "checkBox_IsP1DcbToLcRequired";
            this.checkBox_IsP1DcbToLcRequired.Size = new System.Drawing.Size(324, 16);
            this.checkBox_IsP1DcbToLcRequired.TabIndex = 47;
            this.checkBox_IsP1DcbToLcRequired.Text = "DCB P1P2无电离层改正(单频适用，来自格网电离层文件)";
            this.checkBox_IsP1DcbToLcRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSmoothingRange
            // 
            this.checkBox_IsSmoothingRange.AutoSize = true;
            this.checkBox_IsSmoothingRange.ForeColor = System.Drawing.Color.Black;
            this.checkBox_IsSmoothingRange.Location = new System.Drawing.Point(333, 25);
            this.checkBox_IsSmoothingRange.Name = "checkBox_IsSmoothingRange";
            this.checkBox_IsSmoothingRange.Size = new System.Drawing.Size(240, 16);
            this.checkBox_IsSmoothingRange.TabIndex = 64;
            this.checkBox_IsSmoothingRange.Text = "平滑伪距改正(与单独设置的总开关相同)";
            this.checkBox_IsSmoothingRange.UseVisualStyleBackColor = true;
            // 
            // groupBox16
            // 
            this.groupBox16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox16.Controls.Add(this.flowLayoutPanel2);
            this.groupBox16.Location = new System.Drawing.Point(5, 6);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(612, 354);
            this.groupBox16.TabIndex = 65;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "站星近似值改正";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.checkBox_IsApproxModelCorrectionRequired);
            this.flowLayoutPanel2.Controls.Add(this.checkBox_IsRangeCorrectionsRequired);
            this.flowLayoutPanel2.Controls.Add(this.checkBox_IsFrequencyCorrectionsRequired);
            this.flowLayoutPanel2.Controls.Add(this.groupBox2);
            this.flowLayoutPanel2.Controls.Add(this.groupBox4);
            this.flowLayoutPanel2.Controls.Add(this.groupBox5);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(606, 334);
            this.flowLayoutPanel2.TabIndex = 48;
            // 
            // checkBox_IsApproxModelCorrectionRequired
            // 
            this.checkBox_IsApproxModelCorrectionRequired.AutoSize = true;
            this.checkBox_IsApproxModelCorrectionRequired.ForeColor = System.Drawing.Color.Red;
            this.checkBox_IsApproxModelCorrectionRequired.Location = new System.Drawing.Point(3, 3);
            this.checkBox_IsApproxModelCorrectionRequired.Name = "checkBox_IsApproxModelCorrectionRequired";
            this.checkBox_IsApproxModelCorrectionRequired.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsApproxModelCorrectionRequired.TabIndex = 63;
            this.checkBox_IsApproxModelCorrectionRequired.Text = "近似值改正总开关";
            this.checkBox_IsApproxModelCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsRangeCorrectionsRequired
            // 
            this.checkBox_IsRangeCorrectionsRequired.AutoSize = true;
            this.checkBox_IsRangeCorrectionsRequired.Location = new System.Drawing.Point(129, 3);
            this.checkBox_IsRangeCorrectionsRequired.Name = "checkBox_IsRangeCorrectionsRequired";
            this.checkBox_IsRangeCorrectionsRequired.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsRangeCorrectionsRequired.TabIndex = 47;
            this.checkBox_IsRangeCorrectionsRequired.Text = "伪距改正";
            this.checkBox_IsRangeCorrectionsRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsFrequencyCorrectionsRequired
            // 
            this.checkBox_IsFrequencyCorrectionsRequired.AutoSize = true;
            this.checkBox_IsFrequencyCorrectionsRequired.Location = new System.Drawing.Point(207, 3);
            this.checkBox_IsFrequencyCorrectionsRequired.Name = "checkBox_IsFrequencyCorrectionsRequired";
            this.checkBox_IsFrequencyCorrectionsRequired.Size = new System.Drawing.Size(228, 16);
            this.checkBox_IsFrequencyCorrectionsRequired.TabIndex = 47;
            this.checkBox_IsFrequencyCorrectionsRequired.Text = "频率改正开关(与频率有关，需要天线)";
            this.checkBox_IsFrequencyCorrectionsRequired.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(3, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 48);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测站改正";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsSiteCorrectionsRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsReceiverAntSiteBiasCorrectionRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsOceanTideCorrectionRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsPoleTideCorrectionRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsSolidTideCorrectionRequired);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(594, 28);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // checkBox_IsSiteCorrectionsRequired
            // 
            this.checkBox_IsSiteCorrectionsRequired.AutoSize = true;
            this.checkBox_IsSiteCorrectionsRequired.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkBox_IsSiteCorrectionsRequired.Location = new System.Drawing.Point(3, 3);
            this.checkBox_IsSiteCorrectionsRequired.Name = "checkBox_IsSiteCorrectionsRequired";
            this.checkBox_IsSiteCorrectionsRequired.Size = new System.Drawing.Size(108, 16);
            this.checkBox_IsSiteCorrectionsRequired.TabIndex = 47;
            this.checkBox_IsSiteCorrectionsRequired.Text = "测站改正总开关";
            this.checkBox_IsSiteCorrectionsRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsReceiverAntSiteBiasCorrectionRequired
            // 
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired.AutoSize = true;
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired.Location = new System.Drawing.Point(117, 3);
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired.Name = "checkBox_IsReceiverAntSiteBiasCorrectionRequired";
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired.Size = new System.Drawing.Size(126, 16);
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired.TabIndex = 47;
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired.Text = "接收机天线ARP改正";
            this.checkBox_IsReceiverAntSiteBiasCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOceanTideCorrectionRequired
            // 
            this.checkBox_IsOceanTideCorrectionRequired.AutoSize = true;
            this.checkBox_IsOceanTideCorrectionRequired.Location = new System.Drawing.Point(249, 3);
            this.checkBox_IsOceanTideCorrectionRequired.Name = "checkBox_IsOceanTideCorrectionRequired";
            this.checkBox_IsOceanTideCorrectionRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsOceanTideCorrectionRequired.TabIndex = 46;
            this.checkBox_IsOceanTideCorrectionRequired.Text = "海洋潮汐改正";
            this.checkBox_IsOceanTideCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsPoleTideCorrectionRequired
            // 
            this.checkBox_IsPoleTideCorrectionRequired.AutoSize = true;
            this.checkBox_IsPoleTideCorrectionRequired.Location = new System.Drawing.Point(351, 3);
            this.checkBox_IsPoleTideCorrectionRequired.Name = "checkBox_IsPoleTideCorrectionRequired";
            this.checkBox_IsPoleTideCorrectionRequired.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsPoleTideCorrectionRequired.TabIndex = 47;
            this.checkBox_IsPoleTideCorrectionRequired.Text = "极潮改正";
            this.checkBox_IsPoleTideCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSolidTideCorrectionRequired
            // 
            this.checkBox_IsSolidTideCorrectionRequired.AutoSize = true;
            this.checkBox_IsSolidTideCorrectionRequired.Location = new System.Drawing.Point(429, 3);
            this.checkBox_IsSolidTideCorrectionRequired.Name = "checkBox_IsSolidTideCorrectionRequired";
            this.checkBox_IsSolidTideCorrectionRequired.Size = new System.Drawing.Size(84, 16);
            this.checkBox_IsSolidTideCorrectionRequired.TabIndex = 47;
            this.checkBox_IsSolidTideCorrectionRequired.Text = "固体潮改正";
            this.checkBox_IsSolidTideCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.flowLayoutPanel5);
            this.groupBox4.Location = new System.Drawing.Point(3, 79);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(597, 59);
            this.groupBox4.TabIndex = 67;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "卫星改正";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Controls.Add(this.checkBox_IsSatAntPcoCorrectionRequired);
            this.flowLayoutPanel5.Controls.Add(this.checkBox_IsPhaseWindUpCorrectionRequired);
            this.flowLayoutPanel5.Controls.Add(this.checkBox_IsSatAntPvcCorrectionRequired);
            this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(591, 39);
            this.flowLayoutPanel5.TabIndex = 0;
            // 
            // checkBox_IsSatAntPcoCorrectionRequired
            // 
            this.checkBox_IsSatAntPcoCorrectionRequired.AutoSize = true;
            this.checkBox_IsSatAntPcoCorrectionRequired.Location = new System.Drawing.Point(3, 3);
            this.checkBox_IsSatAntPcoCorrectionRequired.Name = "checkBox_IsSatAntPcoCorrectionRequired";
            this.checkBox_IsSatAntPcoCorrectionRequired.Size = new System.Drawing.Size(114, 16);
            this.checkBox_IsSatAntPcoCorrectionRequired.TabIndex = 47;
            this.checkBox_IsSatAntPcoCorrectionRequired.Text = "卫星天线PCO改正";
            this.checkBox_IsSatAntPcoCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsPhaseWindUpCorrectionRequired
            // 
            this.checkBox_IsPhaseWindUpCorrectionRequired.AutoSize = true;
            this.checkBox_IsPhaseWindUpCorrectionRequired.Location = new System.Drawing.Point(123, 3);
            this.checkBox_IsPhaseWindUpCorrectionRequired.Name = "checkBox_IsPhaseWindUpCorrectionRequired";
            this.checkBox_IsPhaseWindUpCorrectionRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsPhaseWindUpCorrectionRequired.TabIndex = 47;
            this.checkBox_IsPhaseWindUpCorrectionRequired.Text = "相位缠绕改正";
            this.checkBox_IsPhaseWindUpCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSatAntPvcCorrectionRequired
            // 
            this.checkBox_IsSatAntPvcCorrectionRequired.AutoSize = true;
            this.checkBox_IsSatAntPvcCorrectionRequired.Location = new System.Drawing.Point(225, 3);
            this.checkBox_IsSatAntPvcCorrectionRequired.Name = "checkBox_IsSatAntPvcCorrectionRequired";
            this.checkBox_IsSatAntPvcCorrectionRequired.Size = new System.Drawing.Size(114, 16);
            this.checkBox_IsSatAntPvcCorrectionRequired.TabIndex = 47;
            this.checkBox_IsSatAntPvcCorrectionRequired.Text = "卫星天线PVC改正";
            this.checkBox_IsSatAntPvcCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.flowLayoutPanel6);
            this.groupBox5.Location = new System.Drawing.Point(3, 144);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(597, 187);
            this.groupBox5.TabIndex = 67;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "站星距离改正";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.Controls.Add(this.checkBox_IsRecAntennaPcoCorrectionRequired);
            this.flowLayoutPanel6.Controls.Add(this.checkBox_IsRecAntennaPcvRequired);
            this.flowLayoutPanel6.Controls.Add(this.checkBox_IsTropCorrectionRequired);
            this.flowLayoutPanel6.Controls.Add(this.checkBox_IsSatClockBiasCorrectionRequired);
            this.flowLayoutPanel6.Controls.Add(this.checkBox_IsGravitationalDelayCorrectionRequired);
            this.flowLayoutPanel6.Controls.Add(this.groupBox3);
            this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel6.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(591, 167);
            this.flowLayoutPanel6.TabIndex = 0;
            // 
            // checkBox_IsRecAntennaPcoCorrectionRequired
            // 
            this.checkBox_IsRecAntennaPcoCorrectionRequired.AutoSize = true;
            this.checkBox_IsRecAntennaPcoCorrectionRequired.Location = new System.Drawing.Point(3, 3);
            this.checkBox_IsRecAntennaPcoCorrectionRequired.Name = "checkBox_IsRecAntennaPcoCorrectionRequired";
            this.checkBox_IsRecAntennaPcoCorrectionRequired.Size = new System.Drawing.Size(210, 16);
            this.checkBox_IsRecAntennaPcoCorrectionRequired.TabIndex = 47;
            this.checkBox_IsRecAntennaPcoCorrectionRequired.Text = "接收机天线PCO改正(需要天线文件)";
            this.checkBox_IsRecAntennaPcoCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsRecAntennaPcvRequired
            // 
            this.checkBox_IsRecAntennaPcvRequired.AutoSize = true;
            this.checkBox_IsRecAntennaPcvRequired.Location = new System.Drawing.Point(219, 3);
            this.checkBox_IsRecAntennaPcvRequired.Name = "checkBox_IsRecAntennaPcvRequired";
            this.checkBox_IsRecAntennaPcvRequired.Size = new System.Drawing.Size(210, 16);
            this.checkBox_IsRecAntennaPcvRequired.TabIndex = 47;
            this.checkBox_IsRecAntennaPcvRequired.Text = "接收机天线PCV改正(需要天线文件)";
            this.checkBox_IsRecAntennaPcvRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsTropCorrectionRequired
            // 
            this.checkBox_IsTropCorrectionRequired.AutoSize = true;
            this.checkBox_IsTropCorrectionRequired.Location = new System.Drawing.Point(435, 3);
            this.checkBox_IsTropCorrectionRequired.Name = "checkBox_IsTropCorrectionRequired";
            this.checkBox_IsTropCorrectionRequired.Size = new System.Drawing.Size(84, 16);
            this.checkBox_IsTropCorrectionRequired.TabIndex = 47;
            this.checkBox_IsTropCorrectionRequired.Text = "对流层改正";
            this.checkBox_IsTropCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSatClockBiasCorrectionRequired
            // 
            this.checkBox_IsSatClockBiasCorrectionRequired.AutoSize = true;
            this.checkBox_IsSatClockBiasCorrectionRequired.Location = new System.Drawing.Point(3, 25);
            this.checkBox_IsSatClockBiasCorrectionRequired.Name = "checkBox_IsSatClockBiasCorrectionRequired";
            this.checkBox_IsSatClockBiasCorrectionRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsSatClockBiasCorrectionRequired.TabIndex = 47;
            this.checkBox_IsSatClockBiasCorrectionRequired.Text = "卫星钟差改正";
            this.checkBox_IsSatClockBiasCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsGravitationalDelayCorrectionRequired
            // 
            this.checkBox_IsGravitationalDelayCorrectionRequired.AutoSize = true;
            this.checkBox_IsGravitationalDelayCorrectionRequired.Location = new System.Drawing.Point(105, 25);
            this.checkBox_IsGravitationalDelayCorrectionRequired.Name = "checkBox_IsGravitationalDelayCorrectionRequired";
            this.checkBox_IsGravitationalDelayCorrectionRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsGravitationalDelayCorrectionRequired.TabIndex = 47;
            this.checkBox_IsGravitationalDelayCorrectionRequired.Text = "重力延迟改正";
            this.checkBox_IsGravitationalDelayCorrectionRequired.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.flowLayoutPanel4);
            this.groupBox3.Location = new System.Drawing.Point(3, 47);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(585, 117);
            this.groupBox3.TabIndex = 66;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "电离层改正";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.checkBox_ionoCorrection);
            this.flowLayoutPanel4.Controls.Add(this.enumRadioControl_IonoSourceTypeForCorrection);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(579, 97);
            this.flowLayoutPanel4.TabIndex = 0;
            // 
            // checkBox_ionoCorrection
            // 
            this.checkBox_ionoCorrection.AutoSize = true;
            this.checkBox_ionoCorrection.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkBox_ionoCorrection.Location = new System.Drawing.Point(3, 3);
            this.checkBox_ionoCorrection.Name = "checkBox_ionoCorrection";
            this.checkBox_ionoCorrection.Size = new System.Drawing.Size(120, 16);
            this.checkBox_ionoCorrection.TabIndex = 64;
            this.checkBox_ionoCorrection.Text = "电离层改正总开关";
            this.checkBox_ionoCorrection.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_IonoSourceTypeForCorrection
            // 
            this.enumRadioControl_IonoSourceTypeForCorrection.Dock = System.Windows.Forms.DockStyle.Top;
            this.enumRadioControl_IonoSourceTypeForCorrection.Location = new System.Drawing.Point(3, 25);
            this.enumRadioControl_IonoSourceTypeForCorrection.Name = "enumRadioControl_IonoSourceTypeForCorrection";
            this.enumRadioControl_IonoSourceTypeForCorrection.Size = new System.Drawing.Size(558, 69);
            this.enumRadioControl_IonoSourceTypeForCorrection.TabIndex = 65;
            this.enumRadioControl_IonoSourceTypeForCorrection.Title = "电离层改正模型";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_datasource);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(634, 496);
            this.tabControl1.TabIndex = 1;
            // 
            // CorrectionOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CorrectionOptionPage";
            this.Size = new System.Drawing.Size(634, 496);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabPage_datasource.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_datasource;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.CheckBox checkBox_IsObsCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsApproxModelCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsDcbCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsOceanTideCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsReceiverAntSiteBiasCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSolidTideCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsPoleTideCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSatClockBiasCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsTropCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsGravitationalDelayCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSatAntPcoCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsRecAntennaPcoCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsRecAntennaPcvRequired;
        private System.Windows.Forms.CheckBox checkBox_IsPhaseWindUpCorrectionRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSiteCorrectionsRequired;
        private System.Windows.Forms.CheckBox checkBox_IsRangeCorrectionsRequired;
        private System.Windows.Forms.CheckBox checkBox_IsFrequencyCorrectionsRequired;
        private System.Windows.Forms.CheckBox checkBox_ionoCorrection;
        private System.Windows.Forms.CheckBox checkBox_IsP1DcbToLcRequired;
        private System.Windows.Forms.CheckBox checkBox_IsDcbOfP1P2Enabled;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.CheckBox checkBox_IsSatAntPvcCorrectionRequired;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private System.Windows.Forms.CheckBox checkBox_IsSmoothingRange;
        private Geo.Winform.EnumRadioControl enumRadioControl_IonoSourceTypeForCorrection;
    }
}