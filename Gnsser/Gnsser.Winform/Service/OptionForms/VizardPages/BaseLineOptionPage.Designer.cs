namespace Gnsser.Winform
{
    partial class BaseLineOptionPage
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
            this.fileOpenControl_baselineFile = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.namedStringControl_BaseSiteName = new Geo.Winform.Controls.NamedStringControl();
            this.enumRadioControl_BaseSiteSelectType = new Geo.Winform.EnumRadioControl();
            this.checkBox_IsBaseSiteRequried = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.namedFloatControl_maxShotBaseLine = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_MinDistanceOfLongBaseLine = new Geo.Winform.Controls.NamedFloatControl();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.namedFloatControl_fixedErrorVertical = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_verticalCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_fixedErrorLevel = new Geo.Winform.Controls.NamedFloatControl();
            this.namedFloatControl_levelCoefOfProprotion = new Geo.Winform.Controls.NamedFloatControl();
            this.enabledStringControl_IndicatedPrn = new Geo.Winform.Controls.EnabledStringControl();
            this.enumRadioControl_baseSatSelectionType = new Geo.Winform.EnumRadioControl();
            this.enumRadioControl_BaseLineSelectionType = new Geo.Winform.EnumRadioControl();
            this.openFileDialog_nav = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog_clock = new System.Windows.Forms.OpenFileDialog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.namedStringControl_ceterSiteName = new Geo.Winform.Controls.NamedStringControl();
            this.tabControl1.SuspendLayout();
            this.tabPage_receiver.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_receiver);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 478);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_receiver
            // 
            this.tabPage_receiver.Controls.Add(this.groupBox4);
            this.tabPage_receiver.Controls.Add(this.groupBox3);
            this.tabPage_receiver.Controls.Add(this.groupBox2);
            this.tabPage_receiver.Controls.Add(this.enabledStringControl_IndicatedPrn);
            this.tabPage_receiver.Controls.Add(this.enumRadioControl_baseSatSelectionType);
            this.tabPage_receiver.Location = new System.Drawing.Point(4, 22);
            this.tabPage_receiver.Name = "tabPage_receiver";
            this.tabPage_receiver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_receiver.Size = new System.Drawing.Size(586, 452);
            this.tabPage_receiver.TabIndex = 7;
            this.tabPage_receiver.Text = "基线";
            this.tabPage_receiver.UseVisualStyleBackColor = true;
            this.tabPage_receiver.Click += new System.EventHandler(this.TabPage_receiver_Click);
            // 
            // fileOpenControl_baselineFile
            // 
            this.fileOpenControl_baselineFile.AllowDrop = true;
            this.fileOpenControl_baselineFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_baselineFile.FilePath = "";
            this.fileOpenControl_baselineFile.FilePathes = new string[0];
            this.fileOpenControl_baselineFile.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_baselineFile.FirstPath = "";
            this.fileOpenControl_baselineFile.IsMultiSelect = false;
            this.fileOpenControl_baselineFile.LabelName = "外部基线文件：";
            this.fileOpenControl_baselineFile.Location = new System.Drawing.Point(0, 84);
            this.fileOpenControl_baselineFile.Name = "fileOpenControl_baselineFile";
            this.fileOpenControl_baselineFile.Size = new System.Drawing.Size(457, 22);
            this.fileOpenControl_baselineFile.TabIndex = 54;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.namedStringControl_BaseSiteName);
            this.groupBox3.Controls.Add(this.enumRadioControl_BaseSiteSelectType);
            this.groupBox3.Controls.Add(this.checkBox_IsBaseSiteRequried);
            this.groupBox3.Location = new System.Drawing.Point(6, 312);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(556, 100);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "基准站";
            // 
            // namedStringControl_BaseSiteName
            // 
            this.namedStringControl_BaseSiteName.Location = new System.Drawing.Point(284, 13);
            this.namedStringControl_BaseSiteName.Name = "namedStringControl_BaseSiteName";
            this.namedStringControl_BaseSiteName.Size = new System.Drawing.Size(255, 23);
            this.namedStringControl_BaseSiteName.TabIndex = 51;
            this.namedStringControl_BaseSiteName.Title = "手动指定基准测站名称：";
            // 
            // enumRadioControl_BaseSiteSelectType
            // 
            this.enumRadioControl_BaseSiteSelectType.IsReady = false;
            this.enumRadioControl_BaseSiteSelectType.Location = new System.Drawing.Point(6, 43);
            this.enumRadioControl_BaseSiteSelectType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_BaseSiteSelectType.Name = "enumRadioControl_BaseSiteSelectType";
            this.enumRadioControl_BaseSiteSelectType.Size = new System.Drawing.Size(543, 50);
            this.enumRadioControl_BaseSiteSelectType.TabIndex = 50;
            this.enumRadioControl_BaseSiteSelectType.Title = "基准测站选择方法";
            // 
            // checkBox_IsBaseSiteRequried
            // 
            this.checkBox_IsBaseSiteRequried.AutoSize = true;
            this.checkBox_IsBaseSiteRequried.Location = new System.Drawing.Point(6, 20);
            this.checkBox_IsBaseSiteRequried.Name = "checkBox_IsBaseSiteRequried";
            this.checkBox_IsBaseSiteRequried.Size = new System.Drawing.Size(108, 16);
            this.checkBox_IsBaseSiteRequried.TabIndex = 40;
            this.checkBox_IsBaseSiteRequried.Text = "是否需要基准站";
            this.checkBox_IsBaseSiteRequried.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.namedFloatControl_maxShotBaseLine);
            this.groupBox2.Controls.Add(this.namedFloatControl_MinDistanceOfLongBaseLine);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(6, 211);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(556, 95);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "基线长度";
            // 
            // namedFloatControl_maxShotBaseLine
            // 
            this.namedFloatControl_maxShotBaseLine.Location = new System.Drawing.Point(7, 21);
            this.namedFloatControl_maxShotBaseLine.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_maxShotBaseLine.Name = "namedFloatControl_maxShotBaseLine";
            this.namedFloatControl_maxShotBaseLine.Size = new System.Drawing.Size(199, 23);
            this.namedFloatControl_maxShotBaseLine.TabIndex = 39;
            this.namedFloatControl_maxShotBaseLine.Title = "最长短基线(m)：";
            this.namedFloatControl_maxShotBaseLine.Value = 0.1D;
            // 
            // namedFloatControl_MinDistanceOfLongBaseLine
            // 
            this.namedFloatControl_MinDistanceOfLongBaseLine.Location = new System.Drawing.Point(7, 47);
            this.namedFloatControl_MinDistanceOfLongBaseLine.Margin = new System.Windows.Forms.Padding(4);
            this.namedFloatControl_MinDistanceOfLongBaseLine.Name = "namedFloatControl_MinDistanceOfLongBaseLine";
            this.namedFloatControl_MinDistanceOfLongBaseLine.Size = new System.Drawing.Size(199, 23);
            this.namedFloatControl_MinDistanceOfLongBaseLine.TabIndex = 39;
            this.namedFloatControl_MinDistanceOfLongBaseLine.Title = "最短长基线(m)：";
            this.namedFloatControl_MinDistanceOfLongBaseLine.Value = 0.1D;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 12);
            this.label4.TabIndex = 50;
            this.label4.Text = "大于此，则认为是长基线";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(212, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 50;
            this.label3.Text = "小于此，则认为是短基线";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(383, 12);
            this.label5.TabIndex = 50;
            this.label5.Text = "大于“最长段基线”，而小于“最短长基线”，则认为是 “中长基线”";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.namedFloatControl_fixedErrorVertical);
            this.groupBox1.Controls.Add(this.namedFloatControl_verticalCoefOfProprotion);
            this.groupBox1.Controls.Add(this.namedFloatControl_fixedErrorLevel);
            this.groupBox1.Controls.Add(this.namedFloatControl_levelCoefOfProprotion);
            this.groupBox1.Location = new System.Drawing.Point(11, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(680, 104);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "接收机标称精度";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(446, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "注意：ppm 为百万分之一";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(653, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "按照 GB/T 18314-2009，三边同步环闭合差应满足：Wx <= √3 / 5 σ，B、C级复测基线长度较差应满足：Wx <= 2 √2 σ";
            // 
            // namedFloatControl_fixedErrorVertical
            // 
            this.namedFloatControl_fixedErrorVertical.Location = new System.Drawing.Point(223, 20);
            this.namedFloatControl_fixedErrorVertical.Name = "namedFloatControl_fixedErrorVertical";
            this.namedFloatControl_fixedErrorVertical.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorVertical.TabIndex = 11;
            this.namedFloatControl_fixedErrorVertical.Title = "垂直固定误差(mm)：";
            this.namedFloatControl_fixedErrorVertical.Value = 10D;
            // 
            // namedFloatControl_verticalCoefOfProprotion
            // 
            this.namedFloatControl_verticalCoefOfProprotion.Location = new System.Drawing.Point(223, 46);
            this.namedFloatControl_verticalCoefOfProprotion.Name = "namedFloatControl_verticalCoefOfProprotion";
            this.namedFloatControl_verticalCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_verticalCoefOfProprotion.TabIndex = 9;
            this.namedFloatControl_verticalCoefOfProprotion.Title = "垂直比例系数(ppm)：";
            this.namedFloatControl_verticalCoefOfProprotion.Value = 1D;
            // 
            // namedFloatControl_fixedErrorLevel
            // 
            this.namedFloatControl_fixedErrorLevel.Location = new System.Drawing.Point(4, 20);
            this.namedFloatControl_fixedErrorLevel.Name = "namedFloatControl_fixedErrorLevel";
            this.namedFloatControl_fixedErrorLevel.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_fixedErrorLevel.TabIndex = 13;
            this.namedFloatControl_fixedErrorLevel.Title = "水平固定误差(mm)：";
            this.namedFloatControl_fixedErrorLevel.Value = 5D;
            // 
            // namedFloatControl_levelCoefOfProprotion
            // 
            this.namedFloatControl_levelCoefOfProprotion.Location = new System.Drawing.Point(4, 46);
            this.namedFloatControl_levelCoefOfProprotion.Name = "namedFloatControl_levelCoefOfProprotion";
            this.namedFloatControl_levelCoefOfProprotion.Size = new System.Drawing.Size(203, 23);
            this.namedFloatControl_levelCoefOfProprotion.TabIndex = 12;
            this.namedFloatControl_levelCoefOfProprotion.Title = "水平比例系数(ppm)：";
            this.namedFloatControl_levelCoefOfProprotion.Value = 1D;
            // 
            // enabledStringControl_IndicatedPrn
            // 
            this.enabledStringControl_IndicatedPrn.Location = new System.Drawing.Point(6, 176);
            this.enabledStringControl_IndicatedPrn.Margin = new System.Windows.Forms.Padding(4);
            this.enabledStringControl_IndicatedPrn.Name = "enabledStringControl_IndicatedPrn";
            this.enabledStringControl_IndicatedPrn.Size = new System.Drawing.Size(334, 23);
            this.enabledStringControl_IndicatedPrn.TabIndex = 49;
            this.enabledStringControl_IndicatedPrn.Title = "手动指定基准星(比自动选星算法优先)：";
            // 
            // enumRadioControl_baseSatSelectionType
            // 
            this.enumRadioControl_baseSatSelectionType.IsReady = false;
            this.enumRadioControl_baseSatSelectionType.Location = new System.Drawing.Point(3, 125);
            this.enumRadioControl_baseSatSelectionType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_baseSatSelectionType.Name = "enumRadioControl_baseSatSelectionType";
            this.enumRadioControl_baseSatSelectionType.Size = new System.Drawing.Size(583, 41);
            this.enumRadioControl_baseSatSelectionType.TabIndex = 38;
            this.enumRadioControl_baseSatSelectionType.Title = "基准星选择方法";
            // 
            // enumRadioControl_BaseLineSelectionType
            // 
            this.enumRadioControl_BaseLineSelectionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_BaseLineSelectionType.IsReady = false;
            this.enumRadioControl_BaseLineSelectionType.Location = new System.Drawing.Point(7, 13);
            this.enumRadioControl_BaseLineSelectionType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_BaseLineSelectionType.Name = "enumRadioControl_BaseLineSelectionType";
            this.enumRadioControl_BaseLineSelectionType.Size = new System.Drawing.Size(563, 39);
            this.enumRadioControl_BaseLineSelectionType.TabIndex = 38;
            this.enumRadioControl_BaseLineSelectionType.Title = "基线选择方法";
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(586, 452);
            this.tabPage1.TabIndex = 8;
            this.tabPage1.Text = "接收机精度";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.namedStringControl_ceterSiteName);
            this.groupBox4.Controls.Add(this.fileOpenControl_baselineFile);
            this.groupBox4.Controls.Add(this.enumRadioControl_BaseLineSelectionType);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(574, 112);
            this.groupBox4.TabIndex = 55;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "基线选择";
            // 
            // namedStringControl_ceterSiteName
            // 
            this.namedStringControl_ceterSiteName.Location = new System.Drawing.Point(7, 59);
            this.namedStringControl_ceterSiteName.Name = "namedStringControl_ceterSiteName";
            this.namedStringControl_ceterSiteName.Size = new System.Drawing.Size(217, 23);
            this.namedStringControl_ceterSiteName.TabIndex = 56;
            this.namedStringControl_ceterSiteName.Title = "中心站名称：";
            // 
            // BaseLineOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "BaseLineOptionPage";
            this.Size = new System.Drawing.Size(594, 478);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_receiver.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_nav;
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_receiver;
        private Geo.Winform.EnumRadioControl enumRadioControl_BaseLineSelectionType;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_MinDistanceOfLongBaseLine;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxShotBaseLine;
        private Geo.Winform.EnumRadioControl enumRadioControl_baseSatSelectionType;
        private Geo.Winform.Controls.EnabledStringControl enabledStringControl_IndicatedPrn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_IsBaseSiteRequried;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorVertical;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_verticalCoefOfProprotion;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_fixedErrorLevel;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_levelCoefOfProprotion;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Geo.Winform.EnumRadioControl enumRadioControl_BaseSiteSelectType;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_BaseSiteName;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_baselineFile;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_ceterSiteName;
    }
}