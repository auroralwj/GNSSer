namespace Gnsser.Winform
{
    partial class IndicateDataSourceOptionPage
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
            this.groupBox_otherSource = new System.Windows.Forms.GroupBox();
            this.fileOpenControl_coordPath = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_indicateCoordfile = new System.Windows.Forms.CheckBox();
            this.checkBox_indicateStainfo = new System.Windows.Forms.CheckBox();
            this.checkBox_IsGnsserFcbOfDcbRequired = new System.Windows.Forms.CheckBox();
            this.checkBox_IsTropAugmentEnabled = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_fcbOfDcb = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_TropAugmentFilePath = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_stainfo = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fileOpenControl_galEph = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_gloEph = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_galEph = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_bdsEph = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_gloEph = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_eph = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_bdsEph = new System.Windows.Forms.CheckBox();
            this.checkBox_enableClockFile = new System.Windows.Forms.CheckBox();
            this.checkBox_IsUseUniqueEphemerisFile = new System.Windows.Forms.CheckBox();
            this.checkBox_setEphemerisFile = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_clk = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox_ionoSource = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fileOpenControl_ion = new Geo.Winform.Controls.FileOpenControl();
            this.checkBoxIsIndicateGridIono = new System.Windows.Forms.CheckBox();
            this.checkBox1IsGnsserEpochIonoFileRequired = new System.Windows.Forms.CheckBox();
            this.fileOpenControl1GnsserEpochIonoParamFilePath = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_ionoParamCorrection = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_navIonoModel = new Geo.Winform.Controls.FileOpenControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_datasource.SuspendLayout();
            this.groupBox_otherSource.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox_ionoSource.SuspendLayout();
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
            this.tabPage_datasource.Controls.Add(this.groupBox_otherSource);
            this.tabPage_datasource.Controls.Add(this.groupBox3);
            this.tabPage_datasource.Controls.Add(this.groupBox_ionoSource);
            this.tabPage_datasource.Location = new System.Drawing.Point(4, 25);
            this.tabPage_datasource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_datasource.Name = "tabPage_datasource";
            this.tabPage_datasource.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage_datasource.Size = new System.Drawing.Size(907, 569);
            this.tabPage_datasource.TabIndex = 1;
            this.tabPage_datasource.Text = "手动数据源选项(比自动数据源优先)";
            this.tabPage_datasource.UseVisualStyleBackColor = true;
            // 
            // groupBox_otherSource
            // 
            this.groupBox_otherSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_otherSource.Controls.Add(this.fileOpenControl_coordPath);
            this.groupBox_otherSource.Controls.Add(this.checkBox_indicateCoordfile);
            this.groupBox_otherSource.Controls.Add(this.checkBox_indicateStainfo);
            this.groupBox_otherSource.Controls.Add(this.checkBox_IsGnsserFcbOfDcbRequired);
            this.groupBox_otherSource.Controls.Add(this.checkBox_IsTropAugmentEnabled);
            this.groupBox_otherSource.Controls.Add(this.fileOpenControl_fcbOfDcb);
            this.groupBox_otherSource.Controls.Add(this.fileOpenControl_TropAugmentFilePath);
            this.groupBox_otherSource.Controls.Add(this.fileOpenControl_stainfo);
            this.groupBox_otherSource.Location = new System.Drawing.Point(8, 219);
            this.groupBox_otherSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_otherSource.Name = "groupBox_otherSource";
            this.groupBox_otherSource.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_otherSource.Size = new System.Drawing.Size(884, 180);
            this.groupBox_otherSource.TabIndex = 69;
            this.groupBox_otherSource.TabStop = false;
            this.groupBox_otherSource.Text = "其它数据源";
            // 
            // fileOpenControl_coordPath
            // 
            this.fileOpenControl_coordPath.AllowDrop = true;
            this.fileOpenControl_coordPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_coordPath.FilePath = "";
            this.fileOpenControl_coordPath.FilePathes = new string[0];
            this.fileOpenControl_coordPath.Filter = "坐标文件|*.snx;*.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_coordPath.FirstPath = "";
            this.fileOpenControl_coordPath.IsMultiSelect = false;
            this.fileOpenControl_coordPath.LabelName = "已知坐标文件：";
            this.fileOpenControl_coordPath.Location = new System.Drawing.Point(5, 25);
            this.fileOpenControl_coordPath.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_coordPath.Name = "fileOpenControl_coordPath";
            this.fileOpenControl_coordPath.Size = new System.Drawing.Size(739, 28);
            this.fileOpenControl_coordPath.TabIndex = 63;
            // 
            // checkBox_indicateCoordfile
            // 
            this.checkBox_indicateCoordfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_indicateCoordfile.AutoSize = true;
            this.checkBox_indicateCoordfile.Location = new System.Drawing.Point(756, 26);
            this.checkBox_indicateCoordfile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_indicateCoordfile.Name = "checkBox_indicateCoordfile";
            this.checkBox_indicateCoordfile.Size = new System.Drawing.Size(119, 19);
            this.checkBox_indicateCoordfile.TabIndex = 64;
            this.checkBox_indicateCoordfile.Text = "指定坐标文件";
            this.checkBox_indicateCoordfile.UseVisualStyleBackColor = true;
            // 
            // checkBox_indicateStainfo
            // 
            this.checkBox_indicateStainfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_indicateStainfo.AutoSize = true;
            this.checkBox_indicateStainfo.Location = new System.Drawing.Point(756, 61);
            this.checkBox_indicateStainfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_indicateStainfo.Name = "checkBox_indicateStainfo";
            this.checkBox_indicateStainfo.Size = new System.Drawing.Size(119, 19);
            this.checkBox_indicateStainfo.TabIndex = 64;
            this.checkBox_indicateStainfo.Text = "指定测站信息";
            this.checkBox_indicateStainfo.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsGnsserFcbOfDcbRequired
            // 
            this.checkBox_IsGnsserFcbOfDcbRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_IsGnsserFcbOfDcbRequired.AutoSize = true;
            this.checkBox_IsGnsserFcbOfDcbRequired.Location = new System.Drawing.Point(761, 130);
            this.checkBox_IsGnsserFcbOfDcbRequired.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_IsGnsserFcbOfDcbRequired.Name = "checkBox_IsGnsserFcbOfDcbRequired";
            this.checkBox_IsGnsserFcbOfDcbRequired.Size = new System.Drawing.Size(59, 19);
            this.checkBox_IsGnsserFcbOfDcbRequired.TabIndex = 64;
            this.checkBox_IsGnsserFcbOfDcbRequired.Text = "启用";
            this.checkBox_IsGnsserFcbOfDcbRequired.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsTropAugmentEnabled
            // 
            this.checkBox_IsTropAugmentEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_IsTropAugmentEnabled.AutoSize = true;
            this.checkBox_IsTropAugmentEnabled.Location = new System.Drawing.Point(756, 95);
            this.checkBox_IsTropAugmentEnabled.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_IsTropAugmentEnabled.Name = "checkBox_IsTropAugmentEnabled";
            this.checkBox_IsTropAugmentEnabled.Size = new System.Drawing.Size(59, 19);
            this.checkBox_IsTropAugmentEnabled.TabIndex = 64;
            this.checkBox_IsTropAugmentEnabled.Text = "启用";
            this.checkBox_IsTropAugmentEnabled.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_fcbOfDcb
            // 
            this.fileOpenControl_fcbOfDcb.AllowDrop = true;
            this.fileOpenControl_fcbOfDcb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_fcbOfDcb.FilePath = "";
            this.fileOpenControl_fcbOfDcb.FilePathes = new string[0];
            this.fileOpenControl_fcbOfDcb.Filter = "GNSSer FCB文件|*.fcb.txt.xls|所有文件|*.*";
            this.fileOpenControl_fcbOfDcb.FirstPath = "";
            this.fileOpenControl_fcbOfDcb.IsMultiSelect = false;
            this.fileOpenControl_fcbOfDcb.LabelName = "GNSSer FCB文件：";
            this.fileOpenControl_fcbOfDcb.Location = new System.Drawing.Point(7, 128);
            this.fileOpenControl_fcbOfDcb.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_fcbOfDcb.Name = "fileOpenControl_fcbOfDcb";
            this.fileOpenControl_fcbOfDcb.Size = new System.Drawing.Size(743, 28);
            this.fileOpenControl_fcbOfDcb.TabIndex = 63;
            // 
            // fileOpenControl_TropAugmentFilePath
            // 
            this.fileOpenControl_TropAugmentFilePath.AllowDrop = true;
            this.fileOpenControl_TropAugmentFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_TropAugmentFilePath.FilePath = "";
            this.fileOpenControl_TropAugmentFilePath.FilePathes = new string[0];
            this.fileOpenControl_TropAugmentFilePath.Filter = "对流层增强文件|*.txt|文本文件|*.troaug|所有文件|*.*";
            this.fileOpenControl_TropAugmentFilePath.FirstPath = "";
            this.fileOpenControl_TropAugmentFilePath.IsMultiSelect = false;
            this.fileOpenControl_TropAugmentFilePath.LabelName = "对流层增强文件：";
            this.fileOpenControl_TropAugmentFilePath.Location = new System.Drawing.Point(1, 92);
            this.fileOpenControl_TropAugmentFilePath.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_TropAugmentFilePath.Name = "fileOpenControl_TropAugmentFilePath";
            this.fileOpenControl_TropAugmentFilePath.Size = new System.Drawing.Size(743, 28);
            this.fileOpenControl_TropAugmentFilePath.TabIndex = 63;
            // 
            // fileOpenControl_stainfo
            // 
            this.fileOpenControl_stainfo.AllowDrop = true;
            this.fileOpenControl_stainfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_stainfo.FilePath = "";
            this.fileOpenControl_stainfo.FilePathes = new string[0];
            this.fileOpenControl_stainfo.Filter = "测站信息文件|*.stainfo|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_stainfo.FirstPath = "";
            this.fileOpenControl_stainfo.IsMultiSelect = false;
            this.fileOpenControl_stainfo.LabelName = "测站信息文件：";
            this.fileOpenControl_stainfo.Location = new System.Drawing.Point(5, 58);
            this.fileOpenControl_stainfo.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_stainfo.Name = "fileOpenControl_stainfo";
            this.fileOpenControl_stainfo.Size = new System.Drawing.Size(739, 28);
            this.fileOpenControl_stainfo.TabIndex = 63;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.fileOpenControl_galEph);
            this.groupBox3.Controls.Add(this.fileOpenControl_gloEph);
            this.groupBox3.Controls.Add(this.checkBox_galEph);
            this.groupBox3.Controls.Add(this.fileOpenControl_bdsEph);
            this.groupBox3.Controls.Add(this.checkBox_gloEph);
            this.groupBox3.Controls.Add(this.fileOpenControl_eph);
            this.groupBox3.Controls.Add(this.checkBox_bdsEph);
            this.groupBox3.Controls.Add(this.checkBox_enableClockFile);
            this.groupBox3.Controls.Add(this.checkBox_IsUseUniqueEphemerisFile);
            this.groupBox3.Controls.Add(this.checkBox_setEphemerisFile);
            this.groupBox3.Controls.Add(this.fileOpenControl_clk);
            this.groupBox3.Location = new System.Drawing.Point(8, 8);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(888, 204);
            this.groupBox3.TabIndex = 68;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "星历钟差";
            // 
            // fileOpenControl_galEph
            // 
            this.fileOpenControl_galEph.AllowDrop = true;
            this.fileOpenControl_galEph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_galEph.FilePath = "";
            this.fileOpenControl_galEph.FilePathes = new string[0];
            this.fileOpenControl_galEph.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_galEph.FirstPath = "";
            this.fileOpenControl_galEph.IsMultiSelect = false;
            this.fileOpenControl_galEph.LabelName = "GAL星历文件：";
            this.fileOpenControl_galEph.Location = new System.Drawing.Point(3, 165);
            this.fileOpenControl_galEph.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_galEph.Name = "fileOpenControl_galEph";
            this.fileOpenControl_galEph.Size = new System.Drawing.Size(775, 28);
            this.fileOpenControl_galEph.TabIndex = 63;
            // 
            // fileOpenControl_gloEph
            // 
            this.fileOpenControl_gloEph.AllowDrop = true;
            this.fileOpenControl_gloEph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_gloEph.FilePath = "";
            this.fileOpenControl_gloEph.FilePathes = new string[0];
            this.fileOpenControl_gloEph.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_gloEph.FirstPath = "";
            this.fileOpenControl_gloEph.IsMultiSelect = false;
            this.fileOpenControl_gloEph.LabelName = "GLO星历文件：";
            this.fileOpenControl_gloEph.Location = new System.Drawing.Point(3, 130);
            this.fileOpenControl_gloEph.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_gloEph.Name = "fileOpenControl_gloEph";
            this.fileOpenControl_gloEph.Size = new System.Drawing.Size(775, 28);
            this.fileOpenControl_gloEph.TabIndex = 63;
            // 
            // checkBox_galEph
            // 
            this.checkBox_galEph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_galEph.AutoSize = true;
            this.checkBox_galEph.Location = new System.Drawing.Point(793, 170);
            this.checkBox_galEph.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_galEph.Name = "checkBox_galEph";
            this.checkBox_galEph.Size = new System.Drawing.Size(89, 19);
            this.checkBox_galEph.TabIndex = 62;
            this.checkBox_galEph.Text = "指定星历";
            this.checkBox_galEph.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_bdsEph
            // 
            this.fileOpenControl_bdsEph.AllowDrop = true;
            this.fileOpenControl_bdsEph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_bdsEph.FilePath = "";
            this.fileOpenControl_bdsEph.FilePathes = new string[0];
            this.fileOpenControl_bdsEph.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_bdsEph.FirstPath = "";
            this.fileOpenControl_bdsEph.IsMultiSelect = false;
            this.fileOpenControl_bdsEph.LabelName = "BDS星历文件：";
            this.fileOpenControl_bdsEph.Location = new System.Drawing.Point(8, 95);
            this.fileOpenControl_bdsEph.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_bdsEph.Name = "fileOpenControl_bdsEph";
            this.fileOpenControl_bdsEph.Size = new System.Drawing.Size(768, 28);
            this.fileOpenControl_bdsEph.TabIndex = 63;
            // 
            // checkBox_gloEph
            // 
            this.checkBox_gloEph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_gloEph.AutoSize = true;
            this.checkBox_gloEph.Location = new System.Drawing.Point(793, 135);
            this.checkBox_gloEph.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_gloEph.Name = "checkBox_gloEph";
            this.checkBox_gloEph.Size = new System.Drawing.Size(89, 19);
            this.checkBox_gloEph.TabIndex = 62;
            this.checkBox_gloEph.Text = "指定星历";
            this.checkBox_gloEph.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_eph
            // 
            this.fileOpenControl_eph.AllowDrop = true;
            this.fileOpenControl_eph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_eph.FilePath = "";
            this.fileOpenControl_eph.FilePathes = new string[0];
            this.fileOpenControl_eph.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_eph.FirstPath = "";
            this.fileOpenControl_eph.IsMultiSelect = false;
            this.fileOpenControl_eph.LabelName = "统一星历文件（GPS）：";
            this.fileOpenControl_eph.Location = new System.Drawing.Point(8, 24);
            this.fileOpenControl_eph.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_eph.Name = "fileOpenControl_eph";
            this.fileOpenControl_eph.Size = new System.Drawing.Size(567, 28);
            this.fileOpenControl_eph.TabIndex = 63;
            // 
            // checkBox_bdsEph
            // 
            this.checkBox_bdsEph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_bdsEph.AutoSize = true;
            this.checkBox_bdsEph.Location = new System.Drawing.Point(793, 100);
            this.checkBox_bdsEph.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_bdsEph.Name = "checkBox_bdsEph";
            this.checkBox_bdsEph.Size = new System.Drawing.Size(89, 19);
            this.checkBox_bdsEph.TabIndex = 62;
            this.checkBox_bdsEph.Text = "指定星历";
            this.checkBox_bdsEph.UseVisualStyleBackColor = true;
            // 
            // checkBox_enableClockFile
            // 
            this.checkBox_enableClockFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enableClockFile.AutoSize = true;
            this.checkBox_enableClockFile.Location = new System.Drawing.Point(793, 61);
            this.checkBox_enableClockFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_enableClockFile.Name = "checkBox_enableClockFile";
            this.checkBox_enableClockFile.Size = new System.Drawing.Size(89, 19);
            this.checkBox_enableClockFile.TabIndex = 61;
            this.checkBox_enableClockFile.Text = "指定钟差";
            this.checkBox_enableClockFile.UseVisualStyleBackColor = true;
            // 
            // checkBox_IsUseUniqueEphemerisFile
            // 
            this.checkBox_IsUseUniqueEphemerisFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_IsUseUniqueEphemerisFile.AutoSize = true;
            this.checkBox_IsUseUniqueEphemerisFile.Location = new System.Drawing.Point(718, 25);
            this.checkBox_IsUseUniqueEphemerisFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_IsUseUniqueEphemerisFile.Name = "checkBox_IsUseUniqueEphemerisFile";
            this.checkBox_IsUseUniqueEphemerisFile.Size = new System.Drawing.Size(164, 19);
            this.checkBox_IsUseUniqueEphemerisFile.TabIndex = 62;
            this.checkBox_IsUseUniqueEphemerisFile.Text = "所有系统采用此星历";
            this.checkBox_IsUseUniqueEphemerisFile.UseVisualStyleBackColor = true;
            this.checkBox_IsUseUniqueEphemerisFile.CheckedChanged += new System.EventHandler(this.checkBox_IsUseUniqueEphemerisFile_CheckedChanged);
            // 
            // checkBox_setEphemerisFile
            // 
            this.checkBox_setEphemerisFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_setEphemerisFile.AutoSize = true;
            this.checkBox_setEphemerisFile.Location = new System.Drawing.Point(590, 25);
            this.checkBox_setEphemerisFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_setEphemerisFile.Name = "checkBox_setEphemerisFile";
            this.checkBox_setEphemerisFile.Size = new System.Drawing.Size(89, 19);
            this.checkBox_setEphemerisFile.TabIndex = 62;
            this.checkBox_setEphemerisFile.Text = "指定星历";
            this.checkBox_setEphemerisFile.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_clk
            // 
            this.fileOpenControl_clk.AllowDrop = true;
            this.fileOpenControl_clk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_clk.FilePath = "";
            this.fileOpenControl_clk.FilePathes = new string[0];
            this.fileOpenControl_clk.Filter = "Clock卫星钟差文件|*.clk;*.clk_30s|所有文件|*.*";
            this.fileOpenControl_clk.FirstPath = "";
            this.fileOpenControl_clk.IsMultiSelect = false;
            this.fileOpenControl_clk.LabelName = "钟差文件：";
            this.fileOpenControl_clk.Location = new System.Drawing.Point(8, 60);
            this.fileOpenControl_clk.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_clk.Name = "fileOpenControl_clk";
            this.fileOpenControl_clk.Size = new System.Drawing.Size(769, 28);
            this.fileOpenControl_clk.TabIndex = 63;
            // 
            // groupBox_ionoSource
            // 
            this.groupBox_ionoSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_ionoSource.Controls.Add(this.label1);
            this.groupBox_ionoSource.Controls.Add(this.fileOpenControl_ion);
            this.groupBox_ionoSource.Controls.Add(this.checkBoxIsIndicateGridIono);
            this.groupBox_ionoSource.Controls.Add(this.checkBox1IsGnsserEpochIonoFileRequired);
            this.groupBox_ionoSource.Controls.Add(this.fileOpenControl1GnsserEpochIonoParamFilePath);
            this.groupBox_ionoSource.Controls.Add(this.checkBox_ionoParamCorrection);
            this.groupBox_ionoSource.Controls.Add(this.fileOpenControl_navIonoModel);
            this.groupBox_ionoSource.Location = new System.Drawing.Point(11, 406);
            this.groupBox_ionoSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_ionoSource.Name = "groupBox_ionoSource";
            this.groupBox_ionoSource.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_ionoSource.Size = new System.Drawing.Size(885, 151);
            this.groupBox_ionoSource.TabIndex = 65;
            this.groupBox_ionoSource.TabStop = false;
            this.groupBox_ionoSource.Text = " 电离层数据源";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 125);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(392, 15);
            this.label1.TabIndex = 65;
            this.label1.Text = "电离层只需一次，优先顺序：历元参数>IGS格网>导航文件";
            // 
            // fileOpenControl_ion
            // 
            this.fileOpenControl_ion.AllowDrop = true;
            this.fileOpenControl_ion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_ion.FilePath = "";
            this.fileOpenControl_ion.FilePathes = new string[0];
            this.fileOpenControl_ion.Filter = "电离层文件|*.??i|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_ion.FirstPath = "";
            this.fileOpenControl_ion.IsMultiSelect = false;
            this.fileOpenControl_ion.LabelName = "IGS格网电离层：";
            this.fileOpenControl_ion.Location = new System.Drawing.Point(9, 21);
            this.fileOpenControl_ion.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_ion.Name = "fileOpenControl_ion";
            this.fileOpenControl_ion.Size = new System.Drawing.Size(800, 28);
            this.fileOpenControl_ion.TabIndex = 63;
            // 
            // checkBoxIsIndicateGridIono
            // 
            this.checkBoxIsIndicateGridIono.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxIsIndicateGridIono.AutoSize = true;
            this.checkBoxIsIndicateGridIono.Location = new System.Drawing.Point(824, 30);
            this.checkBoxIsIndicateGridIono.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxIsIndicateGridIono.Name = "checkBoxIsIndicateGridIono";
            this.checkBoxIsIndicateGridIono.Size = new System.Drawing.Size(59, 19);
            this.checkBoxIsIndicateGridIono.TabIndex = 64;
            this.checkBoxIsIndicateGridIono.Text = "指定";
            this.checkBoxIsIndicateGridIono.UseVisualStyleBackColor = true;
            // 
            // checkBox1IsGnsserEpochIonoFileRequired
            // 
            this.checkBox1IsGnsserEpochIonoFileRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1IsGnsserEpochIonoFileRequired.AutoSize = true;
            this.checkBox1IsGnsserEpochIonoFileRequired.Location = new System.Drawing.Point(824, 62);
            this.checkBox1IsGnsserEpochIonoFileRequired.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1IsGnsserEpochIonoFileRequired.Name = "checkBox1IsGnsserEpochIonoFileRequired";
            this.checkBox1IsGnsserEpochIonoFileRequired.Size = new System.Drawing.Size(59, 19);
            this.checkBox1IsGnsserEpochIonoFileRequired.TabIndex = 64;
            this.checkBox1IsGnsserEpochIonoFileRequired.Text = "启用";
            this.checkBox1IsGnsserEpochIonoFileRequired.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl1GnsserEpochIonoParamFilePath
            // 
            this.fileOpenControl1GnsserEpochIonoParamFilePath.AllowDrop = true;
            this.fileOpenControl1GnsserEpochIonoParamFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1GnsserEpochIonoParamFilePath.FilePath = "";
            this.fileOpenControl1GnsserEpochIonoParamFilePath.FilePathes = new string[0];
            this.fileOpenControl1GnsserEpochIonoParamFilePath.Filter = "历元电离层文件|*.txt.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1GnsserEpochIonoParamFilePath.FirstPath = "";
            this.fileOpenControl1GnsserEpochIonoParamFilePath.IsMultiSelect = false;
            this.fileOpenControl1GnsserEpochIonoParamFilePath.LabelName = "GNSSer历元电离层参数：";
            this.fileOpenControl1GnsserEpochIonoParamFilePath.Location = new System.Drawing.Point(4, 60);
            this.fileOpenControl1GnsserEpochIonoParamFilePath.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl1GnsserEpochIonoParamFilePath.Name = "fileOpenControl1GnsserEpochIonoParamFilePath";
            this.fileOpenControl1GnsserEpochIonoParamFilePath.Size = new System.Drawing.Size(805, 28);
            this.fileOpenControl1GnsserEpochIonoParamFilePath.TabIndex = 63;
            // 
            // checkBox_ionoParamCorrection
            // 
            this.checkBox_ionoParamCorrection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_ionoParamCorrection.AutoSize = true;
            this.checkBox_ionoParamCorrection.Location = new System.Drawing.Point(824, 100);
            this.checkBox_ionoParamCorrection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_ionoParamCorrection.Name = "checkBox_ionoParamCorrection";
            this.checkBox_ionoParamCorrection.Size = new System.Drawing.Size(59, 19);
            this.checkBox_ionoParamCorrection.TabIndex = 47;
            this.checkBox_ionoParamCorrection.Text = "启用";
            this.checkBox_ionoParamCorrection.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_navIonoModel
            // 
            this.fileOpenControl_navIonoModel.AllowDrop = true;
            this.fileOpenControl_navIonoModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_navIonoModel.FilePath = "";
            this.fileOpenControl_navIonoModel.FilePathes = new string[0];
            this.fileOpenControl_navIonoModel.Filter = "导航文件|*.??n;*.??p|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_navIonoModel.FirstPath = "";
            this.fileOpenControl_navIonoModel.IsMultiSelect = false;
            this.fileOpenControl_navIonoModel.LabelName = "电离层模型(导航文件)：";
            this.fileOpenControl_navIonoModel.Location = new System.Drawing.Point(5, 92);
            this.fileOpenControl_navIonoModel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileOpenControl_navIonoModel.Name = "fileOpenControl_navIonoModel";
            this.fileOpenControl_navIonoModel.Size = new System.Drawing.Size(804, 28);
            this.fileOpenControl_navIonoModel.TabIndex = 63;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_datasource);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(915, 598);
            this.tabControl1.TabIndex = 1;
            // 
            // IndicateDataSourceOptionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "IndicateDataSourceOptionPage";
            this.Size = new System.Drawing.Size(915, 598);
            this.Load += new System.EventHandler(this.GnssOptionForm_Load);
            this.tabPage_datasource.ResumeLayout(false);
            this.groupBox_otherSource.ResumeLayout(false);
            this.groupBox_otherSource.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox_ionoSource.ResumeLayout(false);
            this.groupBox_ionoSource.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_clock;
        private System.Windows.Forms.TabPage tabPage_datasource;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox checkBox_indicateStainfo;
        private System.Windows.Forms.CheckBox checkBox_indicateCoordfile;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_stainfo;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_clk;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_eph;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_coordPath;
        private System.Windows.Forms.CheckBox checkBox_enableClockFile;
        private System.Windows.Forms.CheckBox checkBox_setEphemerisFile;
        private System.Windows.Forms.CheckBox checkBox_ionoParamCorrection;
        private System.Windows.Forms.CheckBox checkBoxIsIndicateGridIono;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_ion;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1GnsserEpochIonoParamFilePath;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_navIonoModel;
        private System.Windows.Forms.CheckBox checkBox1IsGnsserEpochIonoFileRequired;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox_ionoSource;
        private System.Windows.Forms.CheckBox checkBox_IsTropAugmentEnabled;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_TropAugmentFilePath;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox_otherSource;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_galEph;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_gloEph;
        private System.Windows.Forms.CheckBox checkBox_galEph;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_bdsEph;
        private System.Windows.Forms.CheckBox checkBox_gloEph;
        private System.Windows.Forms.CheckBox checkBox_bdsEph;
        private System.Windows.Forms.CheckBox checkBox_IsUseUniqueEphemerisFile;
        private System.Windows.Forms.CheckBox checkBox_IsGnsserFcbOfDcbRequired;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_fcbOfDcb;
    }
}