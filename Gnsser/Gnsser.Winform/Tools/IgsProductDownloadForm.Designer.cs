namespace Gnsser.Winform
{
    partial class IgsProductDownloadForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IgsProductDownloadForm));
            this.label1 = new System.Windows.Forms.Label();
            this.button_download = new System.Windows.Forms.Button();
            this.textBox_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_cancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_sourcenames = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timePeriodUserControl1 = new Gnsser.Winform.Controls.TimePeriodUserControl();
            this.namedStringControl_siteNames = new Geo.Winform.Controls.NamedStringControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton1IGS周解模板 = new System.Windows.Forms.RadioButton();
            this.radioButton2IGS日解模板 = new System.Windows.Forms.RadioButton();
            this.radioButton3IGMAS小时产品 = new System.Windows.Forms.RadioButton();
            this.radioButton4IGMAS日解产品 = new System.Windows.Forms.RadioButton();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.textBox_pathDirs = new System.Windows.Forms.TextBox();
            this.textBox_model = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox_productsType = new System.Windows.Forms.ComboBox();
            this.bindingSource_productTypes = new System.Windows.Forms.BindingSource(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_stepHour = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_allUrls = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_failedUrls = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_buildPathes = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage_setting = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button_checkLib = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_productTypes)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage_setting.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "路径模板：";
            // 
            // button_download
            // 
            this.button_download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_download.Location = new System.Drawing.Point(813, 247);
            this.button_download.Margin = new System.Windows.Forms.Padding(2);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(70, 32);
            this.button_download.TabIndex = 2;
            this.button_download.Text = "下载";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.Location = new System.Drawing.Point(4, 5);
            this.textBox_result.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_result.MaxAppendLineCount = 10000;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.Size = new System.Drawing.Size(1072, 167);
            this.textBox_result.TabIndex = 3;
            this.textBox_result.Text = "";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(9, 248);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(575, 31);
            this.progressBarComponent1.TabIndex = 5;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(889, 247);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(2);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(56, 32);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 108);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 24);
            this.label6.TabIndex = 0;
            this.label6.Text = "数据源\r\n{SourceName}：";
            // 
            // textBox_sourcenames
            // 
            this.textBox_sourcenames.Location = new System.Drawing.Point(109, 113);
            this.textBox_sourcenames.Name = "textBox_sourcenames";
            this.textBox_sourcenames.Size = new System.Drawing.Size(155, 21);
            this.textBox_sourcenames.TabIndex = 12;
            this.textBox_sourcenames.Text = "igs,wum,gbm,qzf,tum,com";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.timePeriodUserControl1);
            this.groupBox1.Controls.Add(this.namedStringControl_siteNames);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Controls.Add(this.textBox_pathDirs);
            this.groupBox1.Controls.Add(this.textBox_model);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.comboBox_productsType);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_stepHour);
            this.groupBox1.Controls.Add(this.textBox_sourcenames);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(932, 211);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // timePeriodUserControl1
            // 
            this.timePeriodUserControl1.Location = new System.Drawing.Point(284, 108);
            this.timePeriodUserControl1.Name = "timePeriodUserControl1";
            this.timePeriodUserControl1.Size = new System.Drawing.Size(523, 32);
            this.timePeriodUserControl1.TabIndex = 33;
            // 
            // namedStringControl_siteNames
            // 
            this.namedStringControl_siteNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_siteNames.Location = new System.Drawing.Point(504, 146);
            this.namedStringControl_siteNames.Name = "namedStringControl_siteNames";
            this.namedStringControl_siteNames.Size = new System.Drawing.Size(425, 23);
            this.namedStringControl_siteNames.TabIndex = 31;
            this.namedStringControl_siteNames.Title = "测站名称(O文件,以\",\"分隔)：";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.flowLayoutPanel1);
            this.groupBox5.Location = new System.Drawing.Point(700, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(227, 79);
            this.groupBox5.TabIndex = 30;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "常用模板";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radioButton1IGS周解模板);
            this.flowLayoutPanel1.Controls.Add(this.radioButton2IGS日解模板);
            this.flowLayoutPanel1.Controls.Add(this.radioButton3IGMAS小时产品);
            this.flowLayoutPanel1.Controls.Add(this.radioButton4IGMAS日解产品);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(221, 59);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // radioButton1IGS周解模板
            // 
            this.radioButton1IGS周解模板.AutoSize = true;
            this.radioButton1IGS周解模板.Location = new System.Drawing.Point(3, 3);
            this.radioButton1IGS周解模板.Name = "radioButton1IGS周解模板";
            this.radioButton1IGS周解模板.Size = new System.Drawing.Size(89, 16);
            this.radioButton1IGS周解模板.TabIndex = 0;
            this.radioButton1IGS周解模板.TabStop = true;
            this.radioButton1IGS周解模板.Text = "IGS周解模板";
            this.radioButton1IGS周解模板.UseVisualStyleBackColor = true;
            this.radioButton1IGS周解模板.CheckedChanged += new System.EventHandler(this.radioButton1IGS周解模板_CheckedChanged);
            // 
            // radioButton2IGS日解模板
            // 
            this.radioButton2IGS日解模板.AutoSize = true;
            this.radioButton2IGS日解模板.Location = new System.Drawing.Point(98, 3);
            this.radioButton2IGS日解模板.Name = "radioButton2IGS日解模板";
            this.radioButton2IGS日解模板.Size = new System.Drawing.Size(89, 16);
            this.radioButton2IGS日解模板.TabIndex = 0;
            this.radioButton2IGS日解模板.TabStop = true;
            this.radioButton2IGS日解模板.Text = "IGS日解模板";
            this.radioButton2IGS日解模板.UseVisualStyleBackColor = true;
            this.radioButton2IGS日解模板.CheckedChanged += new System.EventHandler(this.radioButton2IGS日解模板_CheckedChanged);
            // 
            // radioButton3IGMAS小时产品
            // 
            this.radioButton3IGMAS小时产品.AutoSize = true;
            this.radioButton3IGMAS小时产品.Location = new System.Drawing.Point(3, 25);
            this.radioButton3IGMAS小时产品.Name = "radioButton3IGMAS小时产品";
            this.radioButton3IGMAS小时产品.Size = new System.Drawing.Size(101, 16);
            this.radioButton3IGMAS小时产品.TabIndex = 0;
            this.radioButton3IGMAS小时产品.TabStop = true;
            this.radioButton3IGMAS小时产品.Text = "IGMAS小时产品";
            this.radioButton3IGMAS小时产品.UseVisualStyleBackColor = true;
            this.radioButton3IGMAS小时产品.CheckedChanged += new System.EventHandler(this.radioButton3IGMAS小时产品_CheckedChanged);
            // 
            // radioButton4IGMAS日解产品
            // 
            this.radioButton4IGMAS日解产品.AutoSize = true;
            this.radioButton4IGMAS日解产品.Location = new System.Drawing.Point(110, 25);
            this.radioButton4IGMAS日解产品.Name = "radioButton4IGMAS日解产品";
            this.radioButton4IGMAS日解产品.Size = new System.Drawing.Size(101, 16);
            this.radioButton4IGMAS日解产品.TabIndex = 0;
            this.radioButton4IGMAS日解产品.TabStop = true;
            this.radioButton4IGMAS日解产品.Text = "IGMAS日解产品";
            this.radioButton4IGMAS日解产品.UseVisualStyleBackColor = true;
            this.radioButton4IGMAS日解产品.CheckedChanged += new System.EventHandler(this.radioButton4IGMAS日解产品_CheckedChanged);
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "存储位置：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(21, 180);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(416, 22);
            this.directorySelectionControl1.TabIndex = 26;
            // 
            // textBox_pathDirs
            // 
            this.textBox_pathDirs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pathDirs.Location = new System.Drawing.Point(107, 44);
            this.textBox_pathDirs.Multiline = true;
            this.textBox_pathDirs.Name = "textBox_pathDirs";
            this.textBox_pathDirs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_pathDirs.Size = new System.Drawing.Size(574, 57);
            this.textBox_pathDirs.TabIndex = 24;
            // 
            // textBox_model
            // 
            this.textBox_model.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_model.Location = new System.Drawing.Point(107, 17);
            this.textBox_model.Name = "textBox_model";
            this.textBox_model.Size = new System.Drawing.Size(574, 21);
            this.textBox_model.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(296, 146);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 24);
            this.label3.TabIndex = 23;
            this.label3.Text = "时间步长(小时){Step}：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 146);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 24);
            this.label11.TabIndex = 23;
            this.label11.Text = "产品类型\r\n{ProductType}：";
            // 
            // comboBox_productsType
            // 
            this.comboBox_productsType.DataSource = this.bindingSource_productTypes;
            this.comboBox_productsType.FormattingEnabled = true;
            this.comboBox_productsType.Location = new System.Drawing.Point(111, 150);
            this.comboBox_productsType.Name = "comboBox_productsType";
            this.comboBox_productsType.Size = new System.Drawing.Size(84, 20);
            this.comboBox_productsType.TabIndex = 22;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 47);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 24);
            this.label10.TabIndex = 20;
            this.label10.Text = "路径目录\r\n{UrlDirectory}：";
            // 
            // textBox_stepHour
            // 
            this.textBox_stepHour.Location = new System.Drawing.Point(442, 146);
            this.textBox_stepHour.Name = "textBox_stepHour";
            this.textBox_stepHour.Size = new System.Drawing.Size(41, 21);
            this.textBox_stepHour.TabIndex = 12;
            this.textBox_stepHour.Text = "24";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(9, 298);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(933, 228);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(2, 16);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(929, 210);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_result);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(921, 184);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "信息提示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBoxControl_allUrls);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(921, 184);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "全部地址";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_allUrls
            // 
            this.richTextBoxControl_allUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_allUrls.Location = new System.Drawing.Point(2, 2);
            this.richTextBoxControl_allUrls.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxControl_allUrls.MaxAppendLineCount = 10000;
            this.richTextBoxControl_allUrls.Name = "richTextBoxControl_allUrls";
            this.richTextBoxControl_allUrls.Size = new System.Drawing.Size(917, 180);
            this.richTextBoxControl_allUrls.TabIndex = 0;
            this.richTextBoxControl_allUrls.Text = "";
            this.richTextBoxControl_allUrls.TextChanged += new System.EventHandler(this.richTextBoxControl_allUrls_TextChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl_failedUrls);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(921, 184);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "失败地址";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_failedUrls
            // 
            this.richTextBoxControl_failedUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_failedUrls.Location = new System.Drawing.Point(2, 2);
            this.richTextBoxControl_failedUrls.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxControl_failedUrls.MaxAppendLineCount = 10000;
            this.richTextBoxControl_failedUrls.Name = "richTextBoxControl_failedUrls";
            this.richTextBoxControl_failedUrls.Size = new System.Drawing.Size(917, 180);
            this.richTextBoxControl_failedUrls.TabIndex = 0;
            this.richTextBoxControl_failedUrls.Text = "";
            // 
            // button_buildPathes
            // 
            this.button_buildPathes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_buildPathes.Location = new System.Drawing.Point(600, 248);
            this.button_buildPathes.Margin = new System.Windows.Forms.Padding(2);
            this.button_buildPathes.Name = "button_buildPathes";
            this.button_buildPathes.Size = new System.Drawing.Size(76, 31);
            this.button_buildPathes.TabIndex = 17;
            this.button_buildPathes.Text = "生成地址";
            this.button_buildPathes.UseVisualStyleBackColor = true;
            this.button_buildPathes.Click += new System.EventHandler(this.button_buildPathes_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage_setting);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(0, -2);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(946, 243);
            this.tabControl2.TabIndex = 18;
            // 
            // tabPage_setting
            // 
            this.tabPage_setting.Controls.Add(this.groupBox1);
            this.tabPage_setting.Location = new System.Drawing.Point(4, 22);
            this.tabPage_setting.Name = "tabPage_setting";
            this.tabPage_setting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_setting.Size = new System.Drawing.Size(938, 217);
            this.tabPage_setting.TabIndex = 0;
            this.tabPage_setting.Text = "设置";
            this.tabPage_setting.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.textBox1);
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.groupBox3);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(938, 217);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "提示";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(3, 148);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(932, 66);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "{UrlDirectory}{SourceName}{Year}{SubYear}{Month}{Day}\r\n{Week}{DayOfWeek}{DayOfYea" +
    "r}{WeekOfYear}{ProductType}\r\n{Hour}{Minute}{Second}\r\n{BdsWeek}";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 104);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 19;
            this.label9.Text = "匹配变量：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(932, 95);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "星历源说明(参考)";
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(2, 16);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(928, 77);
            this.textBox2.TabIndex = 20;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // button_checkLib
            // 
            this.button_checkLib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_checkLib.Location = new System.Drawing.Point(695, 248);
            this.button_checkLib.Margin = new System.Windows.Forms.Padding(2);
            this.button_checkLib.Name = "button_checkLib";
            this.button_checkLib.Size = new System.Drawing.Size(107, 31);
            this.button_checkLib.TabIndex = 17;
            this.button_checkLib.Text = "检查本地库存";
            this.button_checkLib.UseVisualStyleBackColor = true;
            this.button_checkLib.Click += new System.EventHandler(this.button_checkLib_Click);
            // 
            // IgsProductDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 536);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.button_checkLib);
            this.Controls.Add(this.button_buildPathes);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.progressBarComponent1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "IgsProductDownloadForm";
            this.Text = "IGS产品下载器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownFilesForm_FormClosing);
            this.Load += new System.EventHandler(this.EphemerisDownloadForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_productTypes)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage_setting.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_download;
        private Geo.Winform.Controls.RichTextBoxControl textBox_result;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_sourcenames;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_allUrls;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_failedUrls;
        private System.Windows.Forms.Button button_buildPathes;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBox_productsType;
        private System.Windows.Forms.BindingSource bindingSource_productTypes;
        private System.Windows.Forms.TextBox textBox_pathDirs;
        private System.Windows.Forms.TextBox textBox_model;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage_setting;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_stepHour;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton1IGS周解模板;
        private System.Windows.Forms.RadioButton radioButton2IGS日解模板;
        private System.Windows.Forms.RadioButton radioButton3IGMAS小时产品;
        private System.Windows.Forms.RadioButton radioButton4IGMAS日解产品;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_siteNames;
        private System.Windows.Forms.Button button_checkLib;
        private Controls.TimePeriodUserControl timePeriodUserControl1;
    }
}