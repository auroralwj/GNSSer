namespace Gnsser.Winform
{
    partial class DataSourceOptionPage
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
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.checkBox1IsLengthPhaseValue = new System.Windows.Forms.CheckBox();
            this.checkBox_IsRangeValueRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsPhaseValueRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsDopplerShiftRequired = new System.Windows.Forms.CheckBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox_isEphemerisFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_ispreciseEphemerisFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_isClockFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsAntennaFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsVMF1FileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsDCBFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsOceanLoadingFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsErpFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSatInfoFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSatStateFileRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsSiteCoordServiceRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_stationInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_igsIonoFile = new System.Windows.Forms.CheckBox();
            this.checkBox_Isgpt2File1DegreeRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsP2C2Enabled = new System.Windows.Forms.CheckBox();
            this.checkBox1IsEnableNgaEphemerisSource = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox1DownloadingSurplusIgsProduct = new System.Windows.Forms.CheckBox();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.button_commonDatasource = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_datasource.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
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
            this.tabPage_datasource.Controls.Add(this.groupBox7);
            this.tabPage_datasource.Controls.Add(this.groupBox13);
            this.tabPage_datasource.Controls.Add(this.groupBox5);
            this.tabPage_datasource.Controls.Add(this.multiGnssSystemSelectControl1);
            this.tabPage_datasource.Controls.Add(this.button_commonDatasource);
            this.tabPage_datasource.Location = new System.Drawing.Point(4, 22);
            this.tabPage_datasource.Name = "tabPage_datasource";
            this.tabPage_datasource.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_datasource.Size = new System.Drawing.Size(678, 452);
            this.tabPage_datasource.TabIndex = 1;
            this.tabPage_datasource.Text = "数据源";
            this.tabPage_datasource.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.checkBox1IsLengthPhaseValue);
            this.groupBox7.Controls.Add(this.checkBox_IsRangeValueRequired);
            this.groupBox7.Controls.Add(this.checkBox_IsPhaseValueRequired);
            this.groupBox7.Controls.Add(this.checkBox_IsDopplerShiftRequired);
            this.groupBox7.Location = new System.Drawing.Point(5, 113);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(456, 46);
            this.groupBox7.TabIndex = 65;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "基本观测量";
            // 
            // checkBox1IsLengthPhaseValue
            // 
            this.checkBox1IsLengthPhaseValue.AutoSize = true;
            this.checkBox1IsLengthPhaseValue.Location = new System.Drawing.Point(322, 20);
            this.checkBox1IsLengthPhaseValue.Name = "checkBox1IsLengthPhaseValue";
            this.checkBox1IsLengthPhaseValue.Size = new System.Drawing.Size(108, 16);
            this.checkBox1IsLengthPhaseValue.TabIndex = 47;
            this.checkBox1IsLengthPhaseValue.Text = "载波为距离非周";
            this.checkBox1IsLengthPhaseValue.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsRangeValueRequired
            // 
            this.checkBox_IsRangeValueRequired.AutoSize = true;
            this.checkBox_IsRangeValueRequired.Location = new System.Drawing.Point(12, 20);
            this.checkBox_IsRangeValueRequired.Name = "checkBox_IsRangeValueRequired";
            this.checkBox_IsRangeValueRequired.Size = new System.Drawing.Size(84, 16);
            this.checkBox_IsRangeValueRequired.TabIndex = 46;
            this.checkBox_IsRangeValueRequired.Text = "需要伪距值";
            this.checkBox_IsRangeValueRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsPhaseValueRequired
            // 
            this.checkBox_IsPhaseValueRequired.AutoSize = true;
            this.checkBox_IsPhaseValueRequired.Location = new System.Drawing.Point(102, 20);
            this.checkBox_IsPhaseValueRequired.Name = "checkBox_IsPhaseValueRequired";
            this.checkBox_IsPhaseValueRequired.Size = new System.Drawing.Size(84, 16);
            this.checkBox_IsPhaseValueRequired.TabIndex = 46;
            this.checkBox_IsPhaseValueRequired.Text = "需要相位值";
            this.checkBox_IsPhaseValueRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsDopplerShiftRequired
            // 
            this.checkBox_IsDopplerShiftRequired.AutoSize = true;
            this.checkBox_IsDopplerShiftRequired.Location = new System.Drawing.Point(186, 20);
            this.checkBox_IsDopplerShiftRequired.Name = "checkBox_IsDopplerShiftRequired";
            this.checkBox_IsDopplerShiftRequired.Size = new System.Drawing.Size(120, 16);
            this.checkBox_IsDopplerShiftRequired.TabIndex = 46;
            this.checkBox_IsDopplerShiftRequired.Text = "需要多普勒频率值";
            this.checkBox_IsDopplerShiftRequired.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.flowLayoutPanel1);
            this.groupBox13.Location = new System.Drawing.Point(8, 177);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(660, 97);
            this.groupBox13.TabIndex = 64;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "自动数据源选项";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox_isEphemerisFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_ispreciseEphemerisFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_isClockFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsAntennaFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsVMF1FileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsDCBFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsOceanLoadingFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsErpFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsSatInfoFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsSatStateFileRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsSiteCoordServiceRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_stationInfo);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_igsIonoFile);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_Isgpt2File1DegreeRequired);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_IsP2C2Enabled);
            this.flowLayoutPanel1.Controls.Add(this.checkBox1IsEnableNgaEphemerisSource);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(654, 77);
            this.flowLayoutPanel1.TabIndex = 48;
            // 
            // checkBox_isEphemerisFileRequired
            // 
            this.checkBox_isEphemerisFileRequired.AutoSize = true;
            this.checkBox_isEphemerisFileRequired.Location = new System.Drawing.Point(3, 3);
            this.checkBox_isEphemerisFileRequired.Name = "checkBox_isEphemerisFileRequired";
            this.checkBox_isEphemerisFileRequired.Size = new System.Drawing.Size(72, 16);
            this.checkBox_isEphemerisFileRequired.TabIndex = 63;
            this.checkBox_isEphemerisFileRequired.Text = "星历文件";
            this.checkBox_isEphemerisFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_ispreciseEphemerisFileRequired
            // 
            this.checkBox_ispreciseEphemerisFileRequired.AutoSize = true;
            this.checkBox_ispreciseEphemerisFileRequired.Location = new System.Drawing.Point(81, 3);
            this.checkBox_ispreciseEphemerisFileRequired.Name = "checkBox_ispreciseEphemerisFileRequired";
            this.checkBox_ispreciseEphemerisFileRequired.Size = new System.Drawing.Size(72, 16);
            this.checkBox_ispreciseEphemerisFileRequired.TabIndex = 63;
            this.checkBox_ispreciseEphemerisFileRequired.Text = "精密星历";
            this.checkBox_ispreciseEphemerisFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_isClockFileRequired
            // 
            this.checkBox_isClockFileRequired.AutoSize = true;
            this.checkBox_isClockFileRequired.Location = new System.Drawing.Point(159, 3);
            this.checkBox_isClockFileRequired.Name = "checkBox_isClockFileRequired";
            this.checkBox_isClockFileRequired.Size = new System.Drawing.Size(72, 16);
            this.checkBox_isClockFileRequired.TabIndex = 63;
            this.checkBox_isClockFileRequired.Text = "精密钟差";
            this.checkBox_isClockFileRequired.UseVisualStyleBackColor = true;
            this.checkBox_isClockFileRequired.CheckedChanged += new System.EventHandler(this.checkBox_isClockFileRequired_CheckedChanged);
            // 
            // checkBox_IsAntennaFileRequired
            // 
            this.checkBox_IsAntennaFileRequired.AutoSize = true;
            this.checkBox_IsAntennaFileRequired.Location = new System.Drawing.Point(237, 3);
            this.checkBox_IsAntennaFileRequired.Name = "checkBox_IsAntennaFileRequired";
            this.checkBox_IsAntennaFileRequired.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsAntennaFileRequired.TabIndex = 46;
            this.checkBox_IsAntennaFileRequired.Text = "天线文件";
            this.checkBox_IsAntennaFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsVMF1FileRequired
            // 
            this.checkBox_IsVMF1FileRequired.AutoSize = true;
            this.checkBox_IsVMF1FileRequired.Location = new System.Drawing.Point(315, 3);
            this.checkBox_IsVMF1FileRequired.Name = "checkBox_IsVMF1FileRequired";
            this.checkBox_IsVMF1FileRequired.Size = new System.Drawing.Size(72, 16);
            this.checkBox_IsVMF1FileRequired.TabIndex = 47;
            this.checkBox_IsVMF1FileRequired.Text = "VMF1文件";
            this.checkBox_IsVMF1FileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsDCBFileRequired
            // 
            this.checkBox_IsDCBFileRequired.AutoSize = true;
            this.checkBox_IsDCBFileRequired.Location = new System.Drawing.Point(393, 3);
            this.checkBox_IsDCBFileRequired.Name = "checkBox_IsDCBFileRequired";
            this.checkBox_IsDCBFileRequired.Size = new System.Drawing.Size(66, 16);
            this.checkBox_IsDCBFileRequired.TabIndex = 47;
            this.checkBox_IsDCBFileRequired.Text = "DCB文件";
            this.checkBox_IsDCBFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsOceanLoadingFileRequired
            // 
            this.checkBox_IsOceanLoadingFileRequired.AutoSize = true;
            this.checkBox_IsOceanLoadingFileRequired.Location = new System.Drawing.Point(465, 3);
            this.checkBox_IsOceanLoadingFileRequired.Name = "checkBox_IsOceanLoadingFileRequired";
            this.checkBox_IsOceanLoadingFileRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsOceanLoadingFileRequired.TabIndex = 47;
            this.checkBox_IsOceanLoadingFileRequired.Text = "海洋潮汐文件";
            this.checkBox_IsOceanLoadingFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsErpFileRequired
            // 
            this.checkBox_IsErpFileRequired.AutoSize = true;
            this.checkBox_IsErpFileRequired.Location = new System.Drawing.Point(567, 3);
            this.checkBox_IsErpFileRequired.Name = "checkBox_IsErpFileRequired";
            this.checkBox_IsErpFileRequired.Size = new System.Drawing.Size(66, 16);
            this.checkBox_IsErpFileRequired.TabIndex = 47;
            this.checkBox_IsErpFileRequired.Text = "ERP文件";
            this.checkBox_IsErpFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSatInfoFileRequired
            // 
            this.checkBox_IsSatInfoFileRequired.AutoSize = true;
            this.checkBox_IsSatInfoFileRequired.Location = new System.Drawing.Point(3, 25);
            this.checkBox_IsSatInfoFileRequired.Name = "checkBox_IsSatInfoFileRequired";
            this.checkBox_IsSatInfoFileRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsSatInfoFileRequired.TabIndex = 47;
            this.checkBox_IsSatInfoFileRequired.Text = "卫星信息文件";
            this.checkBox_IsSatInfoFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSatStateFileRequired
            // 
            this.checkBox_IsSatStateFileRequired.AutoSize = true;
            this.checkBox_IsSatStateFileRequired.Location = new System.Drawing.Point(105, 25);
            this.checkBox_IsSatStateFileRequired.Name = "checkBox_IsSatStateFileRequired";
            this.checkBox_IsSatStateFileRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsSatStateFileRequired.TabIndex = 47;
            this.checkBox_IsSatStateFileRequired.Text = "卫星状态文件";
            this.checkBox_IsSatStateFileRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsSiteCoordServiceRequired
            // 
            this.checkBox_IsSiteCoordServiceRequired.AutoSize = true;
            this.checkBox_IsSiteCoordServiceRequired.Location = new System.Drawing.Point(207, 25);
            this.checkBox_IsSiteCoordServiceRequired.Name = "checkBox_IsSiteCoordServiceRequired";
            this.checkBox_IsSiteCoordServiceRequired.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsSiteCoordServiceRequired.TabIndex = 47;
            this.checkBox_IsSiteCoordServiceRequired.Text = "测站坐标文件";
            this.checkBox_IsSiteCoordServiceRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_stationInfo
            // 
            this.checkBox_stationInfo.AutoSize = true;
            this.checkBox_stationInfo.Location = new System.Drawing.Point(309, 25);
            this.checkBox_stationInfo.Name = "checkBox_stationInfo";
            this.checkBox_stationInfo.Size = new System.Drawing.Size(96, 16);
            this.checkBox_stationInfo.TabIndex = 47;
            this.checkBox_stationInfo.Text = "测站信息文件";
            this.checkBox_stationInfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_igsIonoFile
            // 
            this.checkBox_igsIonoFile.AutoSize = true;
            this.checkBox_igsIonoFile.Location = new System.Drawing.Point(411, 25);
            this.checkBox_igsIonoFile.Name = "checkBox_igsIonoFile";
            this.checkBox_igsIonoFile.Size = new System.Drawing.Size(102, 16);
            this.checkBox_igsIonoFile.TabIndex = 47;
            this.checkBox_igsIonoFile.Text = "IGS电离层文件";
            this.checkBox_igsIonoFile.UseVisualStyleBackColor = true;
            // 
            // checkBox_Isgpt2File1DegreeRequired
            // 
            this.checkBox_Isgpt2File1DegreeRequired.AutoSize = true;
            this.checkBox_Isgpt2File1DegreeRequired.Location = new System.Drawing.Point(519, 25);
            this.checkBox_Isgpt2File1DegreeRequired.Name = "checkBox_Isgpt2File1DegreeRequired";
            this.checkBox_Isgpt2File1DegreeRequired.Size = new System.Drawing.Size(90, 16);
            this.checkBox_Isgpt2File1DegreeRequired.TabIndex = 64;
            this.checkBox_Isgpt2File1DegreeRequired.Text = "GPT2文件1度";
            this.checkBox_Isgpt2File1DegreeRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsP2C2Enabled
            // 
            this.checkBox_IsP2C2Enabled.AutoSize = true;
            this.checkBox_IsP2C2Enabled.Location = new System.Drawing.Point(3, 47);
            this.checkBox_IsP2C2Enabled.Name = "checkBox_IsP2C2Enabled";
            this.checkBox_IsP2C2Enabled.Size = new System.Drawing.Size(84, 16);
            this.checkBox_IsP2C2Enabled.TabIndex = 64;
            this.checkBox_IsP2C2Enabled.Text = "P2C2数据源";
            this.checkBox_IsP2C2Enabled.UseVisualStyleBackColor = true;
            // 
            // checkBox1IsEnableNgaEphemerisSource
            // 
            this.checkBox1IsEnableNgaEphemerisSource.AutoSize = true;
            this.checkBox1IsEnableNgaEphemerisSource.Location = new System.Drawing.Point(93, 47);
            this.checkBox1IsEnableNgaEphemerisSource.Name = "checkBox1IsEnableNgaEphemerisSource";
            this.checkBox1IsEnableNgaEphemerisSource.Size = new System.Drawing.Size(162, 16);
            this.checkBox1IsEnableNgaEphemerisSource.TabIndex = 64;
            this.checkBox1IsEnableNgaEphemerisSource.Text = "NGA预报星历（实时应急）";
            this.checkBox1IsEnableNgaEphemerisSource.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox1DownloadingSurplusIgsProduct);
            this.groupBox5.Location = new System.Drawing.Point(5, 53);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(278, 45);
            this.groupBox5.TabIndex = 64;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "数据源获取";
            // 
            // checkBox1DownloadingSurplusIgsProduct
            // 
            this.checkBox1DownloadingSurplusIgsProduct.AutoSize = true;
            this.checkBox1DownloadingSurplusIgsProduct.Location = new System.Drawing.Point(3, 20);
            this.checkBox1DownloadingSurplusIgsProduct.Name = "checkBox1DownloadingSurplusIgsProduct";
            this.checkBox1DownloadingSurplusIgsProduct.Size = new System.Drawing.Size(114, 16);
            this.checkBox1DownloadingSurplusIgsProduct.TabIndex = 46;
            this.checkBox1DownloadingSurplusIgsProduct.Text = "下载多余IGS产品";
            this.checkBox1DownloadingSurplusIgsProduct.UseVisualStyleBackColor = true;
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(4, 5);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(657, 44);
            this.multiGnssSystemSelectControl1.TabIndex = 0;
            // 
            // button_commonDatasource
            // 
            this.button_commonDatasource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_commonDatasource.Location = new System.Drawing.Point(589, 53);
            this.button_commonDatasource.Name = "button_commonDatasource";
            this.button_commonDatasource.Size = new System.Drawing.Size(75, 38);
            this.button_commonDatasource.TabIndex = 0;
            this.button_commonDatasource.Text = "公共数据源";
            this.button_commonDatasource.UseVisualStyleBackColor = true;
            this.button_commonDatasource.Click += new System.EventHandler(this.button_commonDatasource_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_datasource);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(686, 478);
            this.tabControl1.TabIndex = 1;
            // 
            // DataSourceOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "DataSourceOptionPage";
            this.Size = new System.Drawing.Size(686, 478);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabPage_datasource.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_datasource;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox checkBox_IsRangeValueRequired;
        private System.Windows.Forms.CheckBox checkBox_IsPhaseValueRequired;
        private System.Windows.Forms.CheckBox checkBox_IsDopplerShiftRequired;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBox_isEphemerisFileRequired;
        private System.Windows.Forms.CheckBox checkBox_ispreciseEphemerisFileRequired;
        private System.Windows.Forms.CheckBox checkBox_isClockFileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsAntennaFileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsVMF1FileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsDCBFileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsOceanLoadingFileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsErpFileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSatInfoFileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSatStateFileRequired;
        private System.Windows.Forms.CheckBox checkBox_IsSiteCoordServiceRequired;
        private System.Windows.Forms.CheckBox checkBox_stationInfo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox1DownloadingSurplusIgsProduct;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.Windows.Forms.Button button_commonDatasource;
        private System.Windows.Forms.CheckBox checkBox1IsLengthPhaseValue;
        private System.Windows.Forms.CheckBox checkBox_igsIonoFile;
        private System.Windows.Forms.CheckBox checkBox_Isgpt2File1DegreeRequired;
        private System.Windows.Forms.CheckBox checkBox_IsP2C2Enabled;
        private System.Windows.Forms.CheckBox checkBox1IsEnableNgaEphemerisSource;
    }
}