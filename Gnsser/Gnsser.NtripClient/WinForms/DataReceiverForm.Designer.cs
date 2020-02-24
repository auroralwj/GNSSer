namespace Gnsser.Ntrip.WinForms
{
    partial class DataReceiverForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataReceiverForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip_siteSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选择测站SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除所选DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource_site = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1_enableShowInfo = new System.Windows.Forms.CheckBox();
            this.checkBox_showData = new System.Windows.Forms.CheckBox();
            this.checkBox_showError = new System.Windows.Forms.CheckBox();
            this.checkBox_showWarn = new System.Windows.Forms.CheckBox();
            this.checkBox_debugModel = new System.Windows.Forms.CheckBox();
            this.label_notice = new System.Windows.Forms.Label();
            this.checkBox_saveRawData = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView_site = new System.Windows.Forms.DataGridView();
            this.CasterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mountpoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.identiﬁer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Network = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NavSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormatDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.format = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Generator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.fileOpenControl_nav = new Geo.Winform.Controls.FileOpenControl();
            this.singleSiteGnssSolverTypeSelectionControl1 = new Gnsser.Winform.SingleSiteGnssSolverTypeSelectionControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button_drawDxyz = new System.Windows.Forms.Button();
            this.paramVectorRenderControl1 = new Geo.Winform.Controls.ParamVectorRenderControl();
            this.label_info = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip_siteSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_site)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_site)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(673, 380);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 90;
            // 
            // contextMenuStrip_siteSelect
            // 
            this.contextMenuStrip_siteSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择测站SToolStripMenuItem,
            this.移除所选DToolStripMenuItem});
            this.contextMenuStrip_siteSelect.Name = "contextMenuStrip_siteSelect";
            this.contextMenuStrip_siteSelect.Size = new System.Drawing.Size(142, 48);
            // 
            // 选择测站SToolStripMenuItem
            // 
            this.选择测站SToolStripMenuItem.Name = "选择测站SToolStripMenuItem";
            this.选择测站SToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.选择测站SToolStripMenuItem.Text = "选择测站(&S)";
            this.选择测站SToolStripMenuItem.Click += new System.EventHandler(this.选择测站SToolStripMenuItem_Click);
            // 
            // 移除所选DToolStripMenuItem
            // 
            this.移除所选DToolStripMenuItem.Name = "移除所选DToolStripMenuItem";
            this.移除所选DToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.移除所选DToolStripMenuItem.Text = "移除所选(&D)";
            this.移除所选DToolStripMenuItem.Click += new System.EventHandler(this.移除所选DToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(673, 177);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBoxControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(665, 151);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "操作信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label_info);
            this.splitContainer2.Panel2.Controls.Add(this.button_stop);
            this.splitContainer2.Panel2.Controls.Add(this.button_start);
            this.splitContainer2.Size = new System.Drawing.Size(673, 199);
            this.splitContainer2.SplitterDistance = 157;
            this.splitContainer2.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(673, 157);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(665, 131);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.checkBox_saveRawData);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(659, 125);
            this.groupBox1.TabIndex = 89;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通用设置";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.checkBox1_enableShowInfo);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_showData);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_showError);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_showWarn);
            this.flowLayoutPanel1.Controls.Add(this.checkBox_debugModel);
            this.flowLayoutPanel1.Controls.Add(this.label_notice);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 98);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(653, 24);
            this.flowLayoutPanel1.TabIndex = 87;
            // 
            // checkBox1_enableShowInfo
            // 
            this.checkBox1_enableShowInfo.AutoSize = true;
            this.checkBox1_enableShowInfo.Checked = true;
            this.checkBox1_enableShowInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1_enableShowInfo.Location = new System.Drawing.Point(3, 3);
            this.checkBox1_enableShowInfo.Name = "checkBox1_enableShowInfo";
            this.checkBox1_enableShowInfo.Size = new System.Drawing.Size(72, 16);
            this.checkBox1_enableShowInfo.TabIndex = 1;
            this.checkBox1_enableShowInfo.Text = "显示信息";
            this.checkBox1_enableShowInfo.UseVisualStyleBackColor = true;
            this.checkBox1_enableShowInfo.CheckedChanged += new System.EventHandler(this.checkBox1_enableShowInfo_CheckedChanged);
            // 
            // checkBox_showData
            // 
            this.checkBox_showData.AutoSize = true;
            this.checkBox_showData.Checked = true;
            this.checkBox_showData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showData.Location = new System.Drawing.Point(81, 3);
            this.checkBox_showData.Name = "checkBox_showData";
            this.checkBox_showData.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showData.TabIndex = 3;
            this.checkBox_showData.Text = "显示数据";
            this.checkBox_showData.UseVisualStyleBackColor = true;
            this.checkBox_showData.CheckedChanged += new System.EventHandler(this.checkBox_showData_CheckedChanged);
            // 
            // checkBox_showError
            // 
            this.checkBox_showError.AutoSize = true;
            this.checkBox_showError.Location = new System.Drawing.Point(159, 3);
            this.checkBox_showError.Name = "checkBox_showError";
            this.checkBox_showError.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showError.TabIndex = 2;
            this.checkBox_showError.Text = "显示错误";
            this.checkBox_showError.UseVisualStyleBackColor = true;
            this.checkBox_showError.CheckedChanged += new System.EventHandler(this.checkBox_showError_CheckedChanged);
            // 
            // checkBox_showWarn
            // 
            this.checkBox_showWarn.AutoSize = true;
            this.checkBox_showWarn.Location = new System.Drawing.Point(237, 3);
            this.checkBox_showWarn.Name = "checkBox_showWarn";
            this.checkBox_showWarn.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showWarn.TabIndex = 2;
            this.checkBox_showWarn.Text = "显示警告";
            this.checkBox_showWarn.UseVisualStyleBackColor = true;
            this.checkBox_showWarn.CheckedChanged += new System.EventHandler(this.checkBox_showWarn_CheckedChanged);
            // 
            // checkBox_debugModel
            // 
            this.checkBox_debugModel.AutoSize = true;
            this.checkBox_debugModel.Location = new System.Drawing.Point(315, 3);
            this.checkBox_debugModel.Name = "checkBox_debugModel";
            this.checkBox_debugModel.Size = new System.Drawing.Size(72, 16);
            this.checkBox_debugModel.TabIndex = 2;
            this.checkBox_debugModel.Text = "启用调试";
            this.checkBox_debugModel.UseVisualStyleBackColor = true;
            this.checkBox_debugModel.CheckedChanged += new System.EventHandler(this.checkBox_debugModel_CheckedChanged);
            // 
            // label_notice
            // 
            this.label_notice.AutoSize = true;
            this.label_notice.Location = new System.Drawing.Point(393, 0);
            this.label_notice.Name = "label_notice";
            this.label_notice.Size = new System.Drawing.Size(29, 12);
            this.label_notice.TabIndex = 0;
            this.label_notice.Text = "通知";
            // 
            // checkBox_saveRawData
            // 
            this.checkBox_saveRawData.AutoSize = true;
            this.checkBox_saveRawData.Checked = true;
            this.checkBox_saveRawData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_saveRawData.Location = new System.Drawing.Point(27, 20);
            this.checkBox_saveRawData.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_saveRawData.Name = "checkBox_saveRawData";
            this.checkBox_saveRawData.Size = new System.Drawing.Size(96, 16);
            this.checkBox_saveRawData.TabIndex = 80;
            this.checkBox_saveRawData.Text = "保存原始数据";
            this.checkBox_saveRawData.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "本地目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(21, 41);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(369, 22);
            this.directorySelectionControl1.TabIndex = 85;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView_site);
            this.tabPage4.Controls.Add(this.bindingNavigator1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(665, 131);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "测站";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView_site
            // 
            this.dataGridView_site.AllowUserToAddRows = false;
            this.dataGridView_site.AllowUserToOrderColumns = true;
            this.dataGridView_site.AutoGenerateColumns = false;
            this.dataGridView_site.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_site.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CasterName,
            this.mountpoint,
            this.identiﬁer,
            this.Network,
            this.Country,
            this.NavSystem,
            this.FormatDetails,
            this.latitude,
            this.longitude,
            this.format,
            this.Generator});
            this.dataGridView_site.ContextMenuStrip = this.contextMenuStrip_siteSelect;
            this.dataGridView_site.DataSource = this.bindingSource_site;
            this.dataGridView_site.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_site.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_site.Name = "dataGridView_site";
            this.dataGridView_site.RowTemplate.Height = 23;
            this.dataGridView_site.Size = new System.Drawing.Size(659, 100);
            this.dataGridView_site.TabIndex = 0;
            // 
            // CasterName
            // 
            this.CasterName.DataPropertyName = "CasterName";
            this.CasterName.HeaderText = "CasterName";
            this.CasterName.Name = "CasterName";
            // 
            // mountpoint
            // 
            this.mountpoint.DataPropertyName = "Mountpoint";
            this.mountpoint.HeaderText = "Mountpoint";
            this.mountpoint.Name = "mountpoint";
            // 
            // identiﬁer
            // 
            this.identiﬁer.DataPropertyName = "Identiﬁer";
            this.identiﬁer.HeaderText = "Identiﬁer";
            this.identiﬁer.Name = "identiﬁer";
            // 
            // Network
            // 
            this.Network.DataPropertyName = "Network";
            this.Network.HeaderText = "Network";
            this.Network.Name = "Network";
            // 
            // Country
            // 
            this.Country.DataPropertyName = "Country";
            this.Country.HeaderText = "Country";
            this.Country.Name = "Country";
            // 
            // NavSystem
            // 
            this.NavSystem.DataPropertyName = "NavSystem";
            this.NavSystem.HeaderText = "NavSystem";
            this.NavSystem.Name = "NavSystem";
            // 
            // FormatDetails
            // 
            this.FormatDetails.DataPropertyName = "FormatDetails";
            this.FormatDetails.HeaderText = "FormatDetails";
            this.FormatDetails.Name = "FormatDetails";
            // 
            // latitude
            // 
            this.latitude.DataPropertyName = "Latitude";
            this.latitude.HeaderText = "Latitude";
            this.latitude.Name = "latitude";
            // 
            // longitude
            // 
            this.longitude.DataPropertyName = "Longitude";
            this.longitude.HeaderText = "Longitude";
            this.longitude.Name = "longitude";
            // 
            // format
            // 
            this.format.DataPropertyName = "Format";
            this.format.HeaderText = "Format";
            this.format.Name = "format";
            // 
            // Generator
            // 
            this.Generator.DataPropertyName = "Generator";
            this.Generator.HeaderText = "Generator";
            this.Generator.Name = "Generator";
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.bindingSource_site;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigator1.Location = new System.Drawing.Point(3, 103);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(659, 25);
            this.bindingNavigator1.TabIndex = 1;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 22);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "删除";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.fileOpenControl_nav);
            this.tabPage2.Controls.Add(this.singleSiteGnssSolverTypeSelectionControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(665, 131);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "定位设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_nav
            // 
            this.fileOpenControl_nav.AllowDrop = true;
            this.fileOpenControl_nav.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fileOpenControl_nav.FilePath = "";
            this.fileOpenControl_nav.FilePathes = new string[0];
            this.fileOpenControl_nav.Filter = "Rinex导航、星历文件|*.*N;*.*P;*.*R;*.*G;*.SP3;*.EPH|所有文件|*.*";
            this.fileOpenControl_nav.FirstPath = "";
            this.fileOpenControl_nav.IsMultiSelect = false;
            this.fileOpenControl_nav.LabelName = "星历(若无将等待下载)：";
            this.fileOpenControl_nav.Location = new System.Drawing.Point(3, 67);
            this.fileOpenControl_nav.Name = "fileOpenControl_nav";
            this.fileOpenControl_nav.Size = new System.Drawing.Size(659, 22);
            this.fileOpenControl_nav.TabIndex = 88;
            // 
            // singleSiteGnssSolverTypeSelectionControl1
            // 
            this.singleSiteGnssSolverTypeSelectionControl1.CurrentdType = Gnsser.SingleSiteGnssSolverType.无电离层组合PPP;
            this.singleSiteGnssSolverTypeSelectionControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.singleSiteGnssSolverTypeSelectionControl1.Location = new System.Drawing.Point(3, 89);
            this.singleSiteGnssSolverTypeSelectionControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.singleSiteGnssSolverTypeSelectionControl1.Name = "singleSiteGnssSolverTypeSelectionControl1";
            this.singleSiteGnssSolverTypeSelectionControl1.Size = new System.Drawing.Size(659, 39);
            this.singleSiteGnssSolverTypeSelectionControl1.TabIndex = 13;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.paramVectorRenderControl1);
            this.tabPage5.Controls.Add(this.button_drawDxyz);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(665, 131);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "定位结果显示";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button_drawDxyz
            // 
            this.button_drawDxyz.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_drawDxyz.Location = new System.Drawing.Point(576, 3);
            this.button_drawDxyz.Name = "button_drawDxyz";
            this.button_drawDxyz.Size = new System.Drawing.Size(86, 125);
            this.button_drawDxyz.TabIndex = 17;
            this.button_drawDxyz.Text = "绘坐标偏差图";
            this.button_drawDxyz.UseVisualStyleBackColor = true;
            this.button_drawDxyz.Click += new System.EventHandler(this.button_drawDxyz_Click);
            // 
            // paramVectorRenderControl1
            // 
            this.paramVectorRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramVectorRenderControl1.Location = new System.Drawing.Point(3, 3);
            this.paramVectorRenderControl1.Margin = new System.Windows.Forms.Padding(2);
            this.paramVectorRenderControl1.Name = "paramVectorRenderControl1";
            this.paramVectorRenderControl1.Size = new System.Drawing.Size(573, 125);
            this.paramVectorRenderControl1.TabIndex = 16;
            this.paramVectorRenderControl1.Load += new System.EventHandler(this.paramVectorRenderControl1_Load);
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(7, 7);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(29, 12);
            this.label_info.TabIndex = 1;
            this.label_info.Text = "信息";
            // 
            // button_stop
            // 
            this.button_stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_stop.Location = new System.Drawing.Point(586, 5);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 32);
            this.button_stop.TabIndex = 0;
            this.button_stop.Text = "停止";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Location = new System.Drawing.Point(495, 5);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 32);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "启动";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(659, 145);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // DataReceiverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 380);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DataReceiverForm";
            this.Text = "Ntrip数据接收器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataReceiverForm_FormClosing);
            this.Load += new System.EventHandler(this.DataReceiverForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip_siteSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_site)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_site)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_saveRawData;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.DataGridView dataGridView_site;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_siteSelect;
        private System.Windows.Forms.ToolStripMenuItem 选择测站SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移除所选DToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource_site;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_nav;
        private Winform.SingleSiteGnssSolverTypeSelectionControl singleSiteGnssSolverTypeSelectionControl1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button_drawDxyz;
        private Geo.Winform.Controls.ParamVectorRenderControl paramVectorRenderControl1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        protected System.Windows.Forms.CheckBox checkBox1_enableShowInfo;
        private System.Windows.Forms.CheckBox checkBox_showData;
        private System.Windows.Forms.CheckBox checkBox_showError;
        private System.Windows.Forms.CheckBox checkBox_showWarn;
        private System.Windows.Forms.CheckBox checkBox_debugModel;
        protected System.Windows.Forms.Label label_notice;
        private System.Windows.Forms.DataGridViewTextBoxColumn CasterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn mountpoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn identiﬁer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Network;
        private System.Windows.Forms.DataGridViewTextBoxColumn Country;
        private System.Windows.Forms.DataGridViewTextBoxColumn NavSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormatDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn longitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn format;
        private System.Windows.Forms.DataGridViewTextBoxColumn Generator;
    }
}