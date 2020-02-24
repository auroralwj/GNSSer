namespace Gnsser.Winform
{
    partial class SingleFileEphemerisServiceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleFileEphemerisServiceForm));
            this.openFileDialog_sp3 = new System.Windows.Forms.OpenFileDialog();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.button_toExcel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_prn = new System.Windows.Forms.ComboBox();
            this.bindingSource_prn = new System.Windows.Forms.BindingSource(this.components);
            this.button_show = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_interval = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_read = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileOpenControl_eph = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_inter = new System.Windows.Forms.Button();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.button_showOnMap = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnXYZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGeoCoord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnXyzDot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClockBias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClockDrift = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDriftRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_multSelect = new System.Windows.Forms.Button();
            this.arrayCheckBoxControl_prns = new Geo.Winform.ArrayCheckBoxControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.namedFloatControl_angleCut = new Geo.Winform.Controls.NamedFloatControl();
            this.button_coordSet = new System.Windows.Forms.Button();
            this.namedStringControl_coord = new Geo.Winform.Controls.NamedStringControl();
            this.button_drawPhaseChart = new System.Windows.Forms.Button();
            this.timePeriodControl1 = new Geo.Winform.Controls.TimePeriodControl();
            this.button_interMulti = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prn)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_sp3
            // 
            this.openFileDialog_sp3.Filter = "Rinex导航、星历文件|*.*P;*.*N;*.*R;*.*G;*.SP3|所有文件|*.*";
            this.openFileDialog_sp3.Title = "请选择SP3文件";
            // 
            // button_toExcel
            // 
            this.button_toExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_toExcel.Location = new System.Drawing.Point(707, 21);
            this.button_toExcel.Name = "button_toExcel";
            this.button_toExcel.Size = new System.Drawing.Size(75, 28);
            this.button_toExcel.TabIndex = 23;
            this.button_toExcel.Text = "Excel导出";
            this.button_toExcel.UseVisualStyleBackColor = true;
            this.button_toExcel.Click += new System.EventHandler(this.button_toExcel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "PRN：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "历元";
            // 
            // comboBox_prn
            // 
            this.comboBox_prn.DataSource = this.bindingSource_prn;
            this.comboBox_prn.FormattingEnabled = true;
            this.comboBox_prn.Location = new System.Drawing.Point(55, 19);
            this.comboBox_prn.Name = "comboBox_prn";
            this.comboBox_prn.Size = new System.Drawing.Size(76, 20);
            this.comboBox_prn.TabIndex = 25;
            // 
            // button_show
            // 
            this.button_show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_show.Location = new System.Drawing.Point(621, 16);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(75, 23);
            this.button_show.TabIndex = 26;
            this.button_show.Text = "筛选";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.button_show_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 27;
            this.label6.Text = "间隔：";
            // 
            // textBox_interval
            // 
            this.textBox_interval.Location = new System.Drawing.Point(76, 14);
            this.textBox_interval.Name = "textBox_interval";
            this.textBox_interval.Size = new System.Drawing.Size(76, 21);
            this.textBox_interval.TabIndex = 28;
            this.textBox_interval.Text = "120.0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(158, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 27;
            this.label7.Text = "秒";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button_inter);
            this.groupBox2.Controls.Add(this.button_show);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBox_prn);
            this.groupBox2.Location = new System.Drawing.Point(6, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(816, 60);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "单星筛选";
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(774, 15);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 19;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.fileOpenControl_eph);
            this.groupBox1.Controls.Add(this.button_read);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(855, 44);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
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
            this.fileOpenControl_eph.LabelName = "星历文件：";
            this.fileOpenControl_eph.Location = new System.Drawing.Point(0, 13);
            this.fileOpenControl_eph.Name = "fileOpenControl_eph";
            this.fileOpenControl_eph.Size = new System.Drawing.Size(755, 22);
            this.fileOpenControl_eph.TabIndex = 64;
            this.fileOpenControl_eph.FilePathSetted += new System.EventHandler(this.fileOpenControl_eph_FilePathSetted);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.textBox_interval);
            this.groupBox3.Location = new System.Drawing.Point(439, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(217, 41);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "加工";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // button_inter
            // 
            this.button_inter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_inter.Location = new System.Drawing.Point(735, 16);
            this.button_inter.Name = "button_inter";
            this.button_inter.Size = new System.Drawing.Size(75, 23);
            this.button_inter.TabIndex = 30;
            this.button_inter.Text = "加密";
            this.button_inter.UseVisualStyleBackColor = true;
            this.button_inter.Click += new System.EventHandler(this.button_inter_Click);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 530);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(863, 27);
            this.bindingNavigator1.TabIndex = 33;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(32, 24);
            this.bindingNavigatorCountItem.Text = "/ {0}";
            this.bindingNavigatorCountItem.ToolTipText = "总项数";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(24, 24);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(24, 24);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 27);
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
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(24, 24);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(24, 24);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // button_showOnMap
            // 
            this.button_showOnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_showOnMap.Location = new System.Drawing.Point(788, 21);
            this.button_showOnMap.Name = "button_showOnMap";
            this.button_showOnMap.Size = new System.Drawing.Size(75, 28);
            this.button_showOnMap.TabIndex = 34;
            this.button_showOnMap.Text = "地图上显示";
            this.button_showOnMap.UseVisualStyleBackColor = true;
            this.button_showOnMap.Click += new System.EventHandler(this.button_showOnMap_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.ColumnPrn,
            this.ColumnXYZ,
            this.ColumnGeoCoord,
            this.ColumnXyzDot,
            this.ColumnClockBias,
            this.ColumnClockDrift,
            this.ColumnDriftRate});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(863, 261);
            this.dataGridView1.TabIndex = 35;
            // 
            // ColumnTime
            // 
            this.ColumnTime.DataPropertyName = "Time";
            this.ColumnTime.HeaderText = "历元";
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            this.ColumnTime.Width = 150;
            // 
            // ColumnPrn
            // 
            this.ColumnPrn.DataPropertyName = "PRN";
            this.ColumnPrn.HeaderText = "PRN";
            this.ColumnPrn.Name = "ColumnPrn";
            this.ColumnPrn.ReadOnly = true;
            this.ColumnPrn.Width = 40;
            // 
            // ColumnXYZ
            // 
            this.ColumnXYZ.DataPropertyName = "Xyz";
            this.ColumnXYZ.HeaderText = "空间直角坐标";
            this.ColumnXYZ.Name = "ColumnXYZ";
            this.ColumnXYZ.ReadOnly = true;
            this.ColumnXYZ.Width = 300;
            // 
            // ColumnGeoCoord
            // 
            this.ColumnGeoCoord.DataPropertyName = "GeoCoord";
            this.ColumnGeoCoord.HeaderText = "大地坐标";
            this.ColumnGeoCoord.Name = "ColumnGeoCoord";
            this.ColumnGeoCoord.ReadOnly = true;
            this.ColumnGeoCoord.Width = 250;
            // 
            // ColumnXyzDot
            // 
            this.ColumnXyzDot.DataPropertyName = "Xyzdot";
            this.ColumnXyzDot.HeaderText = "速度";
            this.ColumnXyzDot.Name = "ColumnXyzDot";
            this.ColumnXyzDot.ReadOnly = true;
            // 
            // ColumnClockBias
            // 
            this.ColumnClockBias.DataPropertyName = "ClockBias";
            this.ColumnClockBias.HeaderText = "钟差";
            this.ColumnClockBias.Name = "ColumnClockBias";
            this.ColumnClockBias.ReadOnly = true;
            // 
            // ColumnClockDrift
            // 
            this.ColumnClockDrift.DataPropertyName = "ClockDrift";
            this.ColumnClockDrift.HeaderText = "钟漂";
            this.ColumnClockDrift.Name = "ColumnClockDrift";
            this.ColumnClockDrift.ReadOnly = true;
            // 
            // ColumnDriftRate
            // 
            this.ColumnDriftRate.DataPropertyName = "DriftRate";
            this.ColumnDriftRate.HeaderText = "钟漂速度";
            this.ColumnDriftRate.Name = "ColumnDriftRate";
            this.ColumnDriftRate.ReadOnly = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(863, 175);
            this.tabControl1.TabIndex = 36;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(830, 135);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "单星筛选";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_interMulti);
            this.tabPage2.Controls.Add(this.button_multSelect);
            this.tabPage2.Controls.Add(this.arrayCheckBoxControl_prns);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(830, 135);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "多星筛选";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_multSelect
            // 
            this.button_multSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_multSelect.Location = new System.Drawing.Point(735, 16);
            this.button_multSelect.Name = "button_multSelect";
            this.button_multSelect.Size = new System.Drawing.Size(80, 32);
            this.button_multSelect.TabIndex = 1;
            this.button_multSelect.Text = "原始查看";
            this.button_multSelect.UseVisualStyleBackColor = true;
            this.button_multSelect.Click += new System.EventHandler(this.button_multSelect_Click);
            // 
            // arrayCheckBoxControl_prns
            // 
            this.arrayCheckBoxControl_prns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.arrayCheckBoxControl_prns.Location = new System.Drawing.Point(3, 0);
            this.arrayCheckBoxControl_prns.Name = "arrayCheckBoxControl_prns";
            this.arrayCheckBoxControl_prns.Size = new System.Drawing.Size(717, 132);
            this.arrayCheckBoxControl_prns.TabIndex = 0;
            this.arrayCheckBoxControl_prns.Title = "选项";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.namedFloatControl_angleCut);
            this.tabPage3.Controls.Add(this.button_coordSet);
            this.tabPage3.Controls.Add(this.namedStringControl_coord);
            this.tabPage3.Controls.Add(this.button_drawPhaseChart);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(855, 149);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "站星位置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_angleCut
            // 
            this.namedFloatControl_angleCut.Location = new System.Drawing.Point(3, 35);
            this.namedFloatControl_angleCut.Name = "namedFloatControl_angleCut";
            this.namedFloatControl_angleCut.Size = new System.Drawing.Size(157, 23);
            this.namedFloatControl_angleCut.TabIndex = 78;
            this.namedFloatControl_angleCut.Title = "高度截止角(度)：";
            this.namedFloatControl_angleCut.Value = 10D;
            // 
            // button_coordSet
            // 
            this.button_coordSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_coordSet.Location = new System.Drawing.Point(625, 6);
            this.button_coordSet.Name = "button_coordSet";
            this.button_coordSet.Size = new System.Drawing.Size(75, 23);
            this.button_coordSet.TabIndex = 77;
            this.button_coordSet.Text = "设置坐标";
            this.button_coordSet.UseVisualStyleBackColor = true;
            this.button_coordSet.Click += new System.EventHandler(this.button_coordSet_Click);
            // 
            // namedStringControl_coord
            // 
            this.namedStringControl_coord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.namedStringControl_coord.Location = new System.Drawing.Point(3, 6);
            this.namedStringControl_coord.Name = "namedStringControl_coord";
            this.namedStringControl_coord.Size = new System.Drawing.Size(616, 23);
            this.namedStringControl_coord.TabIndex = 76;
            this.namedStringControl_coord.Title = "测站坐标(XYZ)：";
            // 
            // button_drawPhaseChart
            // 
            this.button_drawPhaseChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawPhaseChart.Location = new System.Drawing.Point(726, 6);
            this.button_drawPhaseChart.Name = "button_drawPhaseChart";
            this.button_drawPhaseChart.Size = new System.Drawing.Size(75, 35);
            this.button_drawPhaseChart.TabIndex = 34;
            this.button_drawPhaseChart.Text = "绘制时段图";
            this.button_drawPhaseChart.UseVisualStyleBackColor = true;
            this.button_drawPhaseChart.Click += new System.EventHandler(this.button_drawPhaseChart_Click);
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(20, 49);
            this.timePeriodControl1.Margin = new System.Windows.Forms.Padding(2);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(414, 24);
            this.timePeriodControl1.TabIndex = 37;
            this.timePeriodControl1.TimeFrom = new System.DateTime(2018, 11, 23, 17, 13, 38, 175);
            this.timePeriodControl1.TimeStringFormat = "yyyy-MM-dd HH:mm:ss";
            this.timePeriodControl1.TimeTo = new System.DateTime(2018, 11, 23, 17, 13, 38, 183);
            this.timePeriodControl1.Title = "时段：";
            // 
            // button_interMulti
            // 
            this.button_interMulti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_interMulti.Location = new System.Drawing.Point(735, 60);
            this.button_interMulti.Name = "button_interMulti";
            this.button_interMulti.Size = new System.Drawing.Size(80, 33);
            this.button_interMulti.TabIndex = 1;
            this.button_interMulti.Text = "加密";
            this.button_interMulti.UseVisualStyleBackColor = true;
            this.button_interMulti.Click += new System.EventHandler(this.button_interMulti_Click);
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
            this.splitContainer1.Panel2.Controls.Add(this.button_toExcel);
            this.splitContainer1.Panel2.Controls.Add(this.button_showOnMap);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(863, 530);
            this.splitContainer1.SplitterDistance = 265;
            this.splitContainer1.TabIndex = 38;
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
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel1.Controls.Add(this.timePeriodControl1);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(863, 265);
            this.splitContainer2.SplitterDistance = 86;
            this.splitContainer2.TabIndex = 0;
            // 
            // SingleFileEphemerisServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 557);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "SingleFileEphemerisServiceForm";
            this.Text = "星历服务";
            this.Load += new System.EventHandler(this.SingleFileEphemerisServiceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_prn)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_sp3;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button button_toExcel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_prn;
        private System.Windows.Forms.Button button_show;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_interval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.Button button_showOnMap;
        private System.Windows.Forms.Button button_inter;
        private System.Windows.Forms.BindingSource bindingSource_prn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_eph;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnXYZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGeoCoord;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnXyzDot;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClockBias;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClockDrift;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDriftRate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_multSelect;
        private Geo.Winform.ArrayCheckBoxControl arrayCheckBoxControl_prns;
        private Geo.Winform.Controls.TimePeriodControl timePeriodControl1;
        private System.Windows.Forms.Button button_drawPhaseChart;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button_coordSet;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_coord;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_angleCut;
        private System.Windows.Forms.Button button_interMulti;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}