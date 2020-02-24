namespace Gnsser.Winform
{
    partial class ObsFileViewerForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openFileDialog_obs = new System.Windows.Forms.OpenFileDialog();
            this.bindingSource_obsInfo = new System.Windows.Forms.BindingSource(this.components);
            this.button_view = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_sat = new System.Windows.Forms.ComboBox();
            this.bindingSource_sat = new System.Windows.Forms.BindingSource(this.components);
            this.textBox_show = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_read = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_selectDraw = new System.Windows.Forms.Button();
            this.button_drawTableView = new System.Windows.Forms.Button();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_outputOneTable = new System.Windows.Forms.Button();
            this.button＿viewAll = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_sortPrn = new System.Windows.Forms.CheckBox();
            this.buttonViewOnChart = new System.Windows.Forms.Button();
            this.checkBox1ViewAllPhase = new System.Windows.Forms.CheckBox();
            this.button_viewPeriodOnMap = new System.Windows.Forms.Button();
            this.button_toExcel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.attributeBox1 = new Geo.Winform.AttributeBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.button_toRinexV3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_maxPercentage = new System.Windows.Forms.TextBox();
            this.button_slim = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button_toRinexV2 = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox_isApproved = new System.Windows.Forms.CheckBox();
            this.checkBox_ionoFree = new System.Windows.Forms.CheckBox();
            this.namedIntControl_smoothWindow = new Geo.Winform.Controls.NamedIntControl();
            this.button_smoothRange = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button_exportEpochLine = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button_viewObs = new System.Windows.Forms.Button();
            this.satObsDataTypeControl1 = new Gnsser.Winform.Controls.SatObsDataTypeControl();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.checkBox_show1Only = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_obsInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sat)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件|*.*O|所有文件|*.*";
            this.openFileDialog_obs.Title = "请选择O文件";
            // 
            // button_view
            // 
            this.button_view.Location = new System.Drawing.Point(565, 61);
            this.button_view.Name = "button_view";
            this.button_view.Size = new System.Drawing.Size(75, 25);
            this.button_view.TabIndex = 9;
            this.button_view.Text = "查看数据";
            this.button_view.UseVisualStyleBackColor = true;
            this.button_view.Click += new System.EventHandler(this.button_view_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(448, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "卫星：";
            // 
            // comboBox_sat
            // 
            this.comboBox_sat.DataSource = this.bindingSource_sat;
            this.comboBox_sat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_sat.FormattingEnabled = true;
            this.comboBox_sat.Location = new System.Drawing.Point(495, 66);
            this.comboBox_sat.Name = "comboBox_sat";
            this.comboBox_sat.Size = new System.Drawing.Size(64, 20);
            this.comboBox_sat.TabIndex = 13;
            // 
            // textBox_show
            // 
            this.textBox_show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_show.Location = new System.Drawing.Point(0, 20);
            this.textBox_show.Multiline = true;
            this.textBox_show.Name = "textBox_show";
            this.textBox_show.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_show.Size = new System.Drawing.Size(189, 196);
            this.textBox_show.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "信息";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(810, 21);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(61, 22);
            this.button_read.TabIndex = 15;
            this.button_read.Text = "读入";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_show1Only);
            this.groupBox1.Controls.Add(this.button_selectDraw);
            this.groupBox1.Controls.Add(this.button_drawTableView);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Controls.Add(this.button_outputOneTable);
            this.groupBox1.Controls.Add(this.button＿viewAll);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.button_viewPeriodOnMap);
            this.groupBox1.Controls.Add(this.button_read);
            this.groupBox1.Controls.Add(this.button_view);
            this.groupBox1.Controls.Add(this.comboBox_sat);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(891, 98);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读取查看选项";
            // 
            // button_selectDraw
            // 
            this.button_selectDraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectDraw.Location = new System.Drawing.Point(813, 63);
            this.button_selectDraw.Name = "button_selectDraw";
            this.button_selectDraw.Size = new System.Drawing.Size(75, 23);
            this.button_selectDraw.TabIndex = 27;
            this.button_selectDraw.Text = "选择绘制";
            this.button_selectDraw.UseVisualStyleBackColor = true;
            this.button_selectDraw.Click += new System.EventHandler(this.button_selectDraw_Click);
            // 
            // button_drawTableView
            // 
            this.button_drawTableView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_drawTableView.Location = new System.Drawing.Point(729, 62);
            this.button_drawTableView.Name = "button_drawTableView";
            this.button_drawTableView.Size = new System.Drawing.Size(75, 23);
            this.button_drawTableView.TabIndex = 27;
            this.button_drawTableView.Text = "绘表数据";
            this.button_drawTableView.UseVisualStyleBackColor = true;
            this.button_drawTableView.Click += new System.EventHandler(this.button_drawTableView_Click);
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "Rinex观测文件(*.*O;*.rnx)|;*.rnx;*.*O|表格文件o.txt.xls|*.??o.txt.xls|RINEX压缩文件(*.crx;*.c" +
    "rx.gz;*.*D;*.*D.Z)|*.crx;*.crx.gz;*.*D.Z;*.*D|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = false;
            this.fileOpenControl1.LabelName = "输入文件：";
            this.fileOpenControl1.Location = new System.Drawing.Point(7, 20);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(787, 23);
            this.fileOpenControl1.TabIndex = 26;
            // 
            // button_outputOneTable
            // 
            this.button_outputOneTable.Location = new System.Drawing.Point(5, 75);
            this.button_outputOneTable.Name = "button_outputOneTable";
            this.button_outputOneTable.Size = new System.Drawing.Size(75, 23);
            this.button_outputOneTable.TabIndex = 25;
            this.button_outputOneTable.Text = "导出一表";
            this.button_outputOneTable.UseVisualStyleBackColor = true;
            this.button_outputOneTable.Click += new System.EventHandler(this.button_outputOneTable_Click);
            // 
            // button＿viewAll
            // 
            this.button＿viewAll.Location = new System.Drawing.Point(5, 50);
            this.button＿viewAll.Name = "button＿viewAll";
            this.button＿viewAll.Size = new System.Drawing.Size(75, 23);
            this.button＿viewAll.TabIndex = 25;
            this.button＿viewAll.Text = "一表看完";
            this.button＿viewAll.UseVisualStyleBackColor = true;
            this.button＿viewAll.Click += new System.EventHandler(this.button＿viewAll_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox_sortPrn);
            this.groupBox3.Controls.Add(this.buttonViewOnChart);
            this.groupBox3.Controls.Add(this.checkBox1ViewAllPhase);
            this.groupBox3.Location = new System.Drawing.Point(179, 46);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(264, 46);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "时段查看";
            // 
            // checkBox_sortPrn
            // 
            this.checkBox_sortPrn.AutoSize = true;
            this.checkBox_sortPrn.Location = new System.Drawing.Point(8, 20);
            this.checkBox_sortPrn.Name = "checkBox_sortPrn";
            this.checkBox_sortPrn.Size = new System.Drawing.Size(72, 16);
            this.checkBox_sortPrn.TabIndex = 23;
            this.checkBox_sortPrn.Text = "卫星排序";
            this.checkBox_sortPrn.UseVisualStyleBackColor = true;
            // 
            // buttonViewOnChart
            // 
            this.buttonViewOnChart.Location = new System.Drawing.Point(188, 17);
            this.buttonViewOnChart.Name = "buttonViewOnChart";
            this.buttonViewOnChart.Size = new System.Drawing.Size(69, 23);
            this.buttonViewOnChart.TabIndex = 21;
            this.buttonViewOnChart.Text = "时段查看";
            this.buttonViewOnChart.UseVisualStyleBackColor = true;
            this.buttonViewOnChart.Click += new System.EventHandler(this.buttonViewOnChart_Click);
            // 
            // checkBox1ViewAllPhase
            // 
            this.checkBox1ViewAllPhase.AutoSize = true;
            this.checkBox1ViewAllPhase.Location = new System.Drawing.Point(86, 20);
            this.checkBox1ViewAllPhase.Name = "checkBox1ViewAllPhase";
            this.checkBox1ViewAllPhase.Size = new System.Drawing.Size(96, 16);
            this.checkBox1ViewAllPhase.TabIndex = 22;
            this.checkBox1ViewAllPhase.Text = "查看所有载波";
            this.checkBox1ViewAllPhase.UseVisualStyleBackColor = true;
            // 
            // button_viewPeriodOnMap
            // 
            this.button_viewPeriodOnMap.Location = new System.Drawing.Point(86, 50);
            this.button_viewPeriodOnMap.Name = "button_viewPeriodOnMap";
            this.button_viewPeriodOnMap.Size = new System.Drawing.Size(86, 23);
            this.button_viewPeriodOnMap.TabIndex = 21;
            this.button_viewPeriodOnMap.Text = "地图时段查看";
            this.button_viewPeriodOnMap.UseVisualStyleBackColor = true;
            this.button_viewPeriodOnMap.Click += new System.EventHandler(this.button_viewPeriodOnMap_Click);
            // 
            // button_toExcel
            // 
            this.button_toExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_toExcel.Location = new System.Drawing.Point(785, 6);
            this.button_toExcel.Name = "button_toExcel";
            this.button_toExcel.Size = new System.Drawing.Size(75, 25);
            this.button_toExcel.TabIndex = 18;
            this.button_toExcel.Text = "Excel导出";
            this.button_toExcel.UseVisualStyleBackColor = true;
            this.button_toExcel.Click += new System.EventHandler(this.button_toExcel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(914, 438);
            this.splitContainer1.SplitterDistance = 189;
            this.splitContainer1.TabIndex = 19;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.textBox_show);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.attributeBox1);
            this.splitContainer2.Size = new System.Drawing.Size(189, 438);
            this.splitContainer2.SplitterDistance = 216;
            this.splitContainer2.TabIndex = 15;
            // 
            // attributeBox1
            // 
            this.attributeBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeBox1.Location = new System.Drawing.Point(0, 0);
            this.attributeBox1.Margin = new System.Windows.Forms.Padding(4);
            this.attributeBox1.Name = "attributeBox1";
            this.attributeBox1.Size = new System.Drawing.Size(189, 218);
            this.attributeBox1.TabIndex = 0;
            this.attributeBox1.Tilte = "属性";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(721, 438);
            this.tabControl1.TabIndex = 17;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(713, 412);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据内容";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(707, 406);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // button_toRinexV3
            // 
            this.button_toRinexV3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_toRinexV3.Location = new System.Drawing.Point(693, 6);
            this.button_toRinexV3.Name = "button_toRinexV3";
            this.button_toRinexV3.Size = new System.Drawing.Size(86, 25);
            this.button_toRinexV3.TabIndex = 18;
            this.button_toRinexV3.Text = "RinexV3导出";
            this.button_toRinexV3.UseVisualStyleBackColor = true;
            this.button_toRinexV3.Click += new System.EventHandler(this.button_toRinexV3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_maxPercentage);
            this.groupBox2.Controls.Add(this.button_slim);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(5, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(374, 40);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "编辑";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "% 的观测值";
            // 
            // textBox_maxPercentage
            // 
            this.textBox_maxPercentage.Location = new System.Drawing.Point(151, 14);
            this.textBox_maxPercentage.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_maxPercentage.Name = "textBox_maxPercentage";
            this.textBox_maxPercentage.Size = new System.Drawing.Size(52, 21);
            this.textBox_maxPercentage.TabIndex = 2;
            this.textBox_maxPercentage.Text = "5";
            // 
            // button_slim
            // 
            this.button_slim.Location = new System.Drawing.Point(291, 11);
            this.button_slim.Margin = new System.Windows.Forms.Padding(2);
            this.button_slim.Name = "button_slim";
            this.button_slim.Size = new System.Drawing.Size(77, 24);
            this.button_slim.TabIndex = 1;
            this.button_slim.Text = "瘦身";
            this.button_slim.UseVisualStyleBackColor = true;
            this.button_slim.Click += new System.EventHandler(this.button_slim_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "剔除观测量出勤率少于：";
            // 
            // button_toRinexV2
            // 
            this.button_toRinexV2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_toRinexV2.Location = new System.Drawing.Point(601, 6);
            this.button_toRinexV2.Name = "button_toRinexV2";
            this.button_toRinexV2.Size = new System.Drawing.Size(86, 25);
            this.button_toRinexV2.TabIndex = 18;
            this.button_toRinexV2.Text = "RinexV2导出";
            this.button_toRinexV2.UseVisualStyleBackColor = true;
            this.button_toRinexV2.Click += new System.EventHandler(this.button_toRinexV2_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(8, 8);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(905, 130);
            this.tabControl2.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(897, 104);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "观测文件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox_isApproved);
            this.tabPage3.Controls.Add(this.checkBox_ionoFree);
            this.tabPage3.Controls.Add(this.namedIntControl_smoothWindow);
            this.tabPage3.Controls.Add(this.button_smoothRange);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(866, 104);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "数据处理";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox_isApproved
            // 
            this.checkBox_isApproved.AutoSize = true;
            this.checkBox_isApproved.Checked = true;
            this.checkBox_isApproved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isApproved.Location = new System.Drawing.Point(427, 68);
            this.checkBox_isApproved.Name = "checkBox_isApproved";
            this.checkBox_isApproved.Size = new System.Drawing.Size(246, 16);
            this.checkBox_isApproved.TabIndex = 28;
            this.checkBox_isApproved.Text = "采用GNSSer改进算法，否则原始Hatch算法";
            this.checkBox_isApproved.UseVisualStyleBackColor = true;
            // 
            // checkBox_ionoFree
            // 
            this.checkBox_ionoFree.AutoSize = true;
            this.checkBox_ionoFree.Location = new System.Drawing.Point(172, 65);
            this.checkBox_ionoFree.Name = "checkBox_ionoFree";
            this.checkBox_ionoFree.Size = new System.Drawing.Size(120, 16);
            this.checkBox_ionoFree.TabIndex = 23;
            this.checkBox_ionoFree.Text = "双频消电离层组合";
            this.checkBox_ionoFree.UseVisualStyleBackColor = true;
            // 
            // namedIntControl_smoothWindow
            // 
            this.namedIntControl_smoothWindow.Location = new System.Drawing.Point(6, 61);
            this.namedIntControl_smoothWindow.Name = "namedIntControl_smoothWindow";
            this.namedIntControl_smoothWindow.Size = new System.Drawing.Size(143, 23);
            this.namedIntControl_smoothWindow.TabIndex = 22;
            this.namedIntControl_smoothWindow.Title = "伪距平滑窗口：";
            this.namedIntControl_smoothWindow.Value = 10;
            // 
            // button_smoothRange
            // 
            this.button_smoothRange.Location = new System.Drawing.Point(296, 65);
            this.button_smoothRange.Name = "button_smoothRange";
            this.button_smoothRange.Size = new System.Drawing.Size(103, 23);
            this.button_smoothRange.TabIndex = 21;
            this.button_smoothRange.Text = "载波平滑伪距";
            this.button_smoothRange.UseVisualStyleBackColor = true;
            this.button_smoothRange.Click += new System.EventHandler(this.button_smoothRange_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button_exportEpochLine);
            this.tabPage4.Controls.Add(this.button_toExcel);
            this.tabPage4.Controls.Add(this.button_toRinexV3);
            this.tabPage4.Controls.Add(this.button_toRinexV2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(866, 104);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "数据导出";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button_exportEpochLine
            // 
            this.button_exportEpochLine.Location = new System.Drawing.Point(19, 37);
            this.button_exportEpochLine.Name = "button_exportEpochLine";
            this.button_exportEpochLine.Size = new System.Drawing.Size(75, 23);
            this.button_exportEpochLine.TabIndex = 19;
            this.button_exportEpochLine.Text = "导出行数据";
            this.button_exportEpochLine.UseVisualStyleBackColor = true;
            this.button_exportEpochLine.Click += new System.EventHandler(this.button_exportEpochLine_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.button_viewObs);
            this.tabPage5.Controls.Add(this.satObsDataTypeControl1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(866, 104);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "数据类型";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button_viewObs
            // 
            this.button_viewObs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_viewObs.Location = new System.Drawing.Point(785, 6);
            this.button_viewObs.Name = "button_viewObs";
            this.button_viewObs.Size = new System.Drawing.Size(75, 23);
            this.button_viewObs.TabIndex = 1;
            this.button_viewObs.Text = "查看数据";
            this.button_viewObs.UseVisualStyleBackColor = true;
            this.button_viewObs.Click += new System.EventHandler(this.button_viewObs_Click);
            // 
            // satObsDataTypeControl1
            // 
            this.satObsDataTypeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.satObsDataTypeControl1.CurrentdType = Gnsser.SatObsDataType.AlignedIonoFreePhaseRange;
            this.satObsDataTypeControl1.Location = new System.Drawing.Point(5, 5);
            this.satObsDataTypeControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.satObsDataTypeControl1.Name = "satObsDataTypeControl1";
            this.satObsDataTypeControl1.Size = new System.Drawing.Size(754, 97);
            this.satObsDataTypeControl1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer3.Size = new System.Drawing.Size(914, 582);
            this.splitContainer3.SplitterDistance = 140;
            this.splitContainer3.TabIndex = 23;
            // 
            // checkBox_show1Only
            // 
            this.checkBox_show1Only.AutoSize = true;
            this.checkBox_show1Only.Location = new System.Drawing.Point(647, 68);
            this.checkBox_show1Only.Name = "checkBox_show1Only";
            this.checkBox_show1Only.Size = new System.Drawing.Size(108, 16);
            this.checkBox_show1Only.TabIndex = 28;
            this.checkBox_show1Only.Text = "只显示第一频率";
            this.checkBox_show1Only.UseVisualStyleBackColor = true;
            // 
            // ObsFileViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 582);
            this.Controls.Add(this.splitContainer3);
            this.Name = "ObsFileViewerForm";
            this.Text = "单个观测信息(o文件查看器)";
            this.Load += new System.EventHandler(this.ObsFileViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_obsInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sat)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.BindingSource bindingSource_obsInfo;
        private System.Windows.Forms.Button button_view;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_sat;
        private System.Windows.Forms.BindingSource bindingSource_sat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_show;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_toExcel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.AttributeBox attributeBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_toRinexV3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_maxPercentage;
        private System.Windows.Forms.Button button_slim;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_viewPeriodOnMap;
        private System.Windows.Forms.Button button_toRinexV2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button_viewObs;
        private Controls.SatObsDataTypeControl satObsDataTypeControl1;
        private System.Windows.Forms.Button buttonViewOnChart;
        private System.Windows.Forms.CheckBox checkBox1ViewAllPhase;
        private System.Windows.Forms.CheckBox checkBox_sortPrn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button＿viewAll;
        private System.Windows.Forms.SplitContainer splitContainer3;
        protected Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_outputOneTable;
        private System.Windows.Forms.Button button_drawTableView;
        private System.Windows.Forms.Button button_selectDraw;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.Button button_exportEpochLine;
        private System.Windows.Forms.Button button_smoothRange;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_smoothWindow;
        private System.Windows.Forms.CheckBox checkBox_ionoFree;
        private System.Windows.Forms.CheckBox checkBox_isApproved;
        private System.Windows.Forms.CheckBox checkBox_show1Only;
    }
}

