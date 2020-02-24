namespace Gnsser.Winform
{
    partial class IonoTrendCaculateForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_sat = new System.Windows.Forms.ComboBox();
            this.bindingSource_sat = new System.Windows.Forms.BindingSource(this.components);
            this.textBox_show = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_read = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_showL1Only = new System.Windows.Forms.CheckBox();
            this.checkBox_carrrierInLen = new System.Windows.Forms.CheckBox();
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.attributeBox1 = new Geo.Winform.AttributeBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.directorySelectionControlOutDir = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox_isWeighted = new System.Windows.Forms.CheckBox();
            this.checkBox_isApproved = new System.Windows.Forms.CheckBox();
            this.checkBox_showPoly = new System.Windows.Forms.CheckBox();
            this.enumRadioControl1 = new Geo.Winform.EnumRadioControl();
            this.checkBox_isShowL1Only = new System.Windows.Forms.CheckBox();
            this.namedFloatControl_satCutoff = new Geo.Winform.Controls.NamedFloatControl();
            this.namedIntControl_smoothWindow = new Geo.Winform.Controls.NamedIntControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button_CacuIonoFitDelta = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.enumRadioControl_fitType = new Geo.Winform.EnumRadioControl();
            this.button_diffOrderPolyIonoFit = new System.Windows.Forms.Button();
            this.namedIntControl_toOrder = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_fromOrder = new Geo.Winform.Controls.NamedIntControl();
            this.multiGnssSystemSelectControl1 = new Gnsser.Winform.Controls.MultiGnssSystemSelectControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.namedIntControl1_ionoFitEpochCount = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_deltaIonoOrder = new Geo.Winform.Controls.NamedIntControl();
            this.namedIntControl_bufferCount = new Geo.Winform.Controls.NamedIntControl();
            this.enumRadioControl_ionDifferType = new Geo.Winform.EnumRadioControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.checkBox_showTable = new System.Windows.Forms.CheckBox();
            this.checkBox_openChart = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_multiPath = new Geo.Winform.Controls.FileOpenControl();
            this.button_multiFile = new System.Windows.Forms.Button();
            this.button_MultiRun = new System.Windows.Forms.Button();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer_top = new System.Windows.Forms.SplitContainer();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.button_solveAllRate = new System.Windows.Forms.Button();
            this.button_ionoDifferByLL = new System.Windows.Forms.Button();
            this.button_p_L_div2 = new System.Windows.Forms.Button();
            this.button_smoothCurrent = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_obsInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sat)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_top)).BeginInit();
            this.splitContainer_top.Panel1.SuspendLayout();
            this.splitContainer_top.Panel2.SuspendLayout();
            this.splitContainer_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_obs
            // 
            this.openFileDialog_obs.Filter = "Rinex观测文件|*.*O|所有文件|*.*";
            this.openFileDialog_obs.Title = "请选择O文件";
            // 
            // button_view
            // 
            this.button_view.Location = new System.Drawing.Point(328, 45);
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
            this.label5.Location = new System.Drawing.Point(62, 48);
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
            this.comboBox_sat.Location = new System.Drawing.Point(108, 45);
            this.comboBox_sat.Name = "comboBox_sat";
            this.comboBox_sat.Size = new System.Drawing.Size(201, 20);
            this.comboBox_sat.TabIndex = 13;
            // 
            // textBox_show
            // 
            this.textBox_show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_show.Location = new System.Drawing.Point(0, 20);
            this.textBox_show.Multiline = true;
            this.textBox_show.Name = "textBox_show";
            this.textBox_show.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_show.Size = new System.Drawing.Size(233, 154);
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
            this.label2.Size = new System.Drawing.Size(233, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "信息";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(1045, 16);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(61, 22);
            this.button_read.TabIndex = 15;
            this.button_read.Text = "读入";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_showL1Only);
            this.groupBox1.Controls.Add(this.checkBox_carrrierInLen);
            this.groupBox1.Controls.Add(this.fileOpenControl1);
            this.groupBox1.Controls.Add(this.button_read);
            this.groupBox1.Controls.Add(this.button_view);
            this.groupBox1.Controls.Add(this.comboBox_sat);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1112, 71);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读取查看选项";
            // 
            // checkBox_showL1Only
            // 
            this.checkBox_showL1Only.AutoSize = true;
            this.checkBox_showL1Only.Location = new System.Drawing.Point(554, 48);
            this.checkBox_showL1Only.Name = "checkBox_showL1Only";
            this.checkBox_showL1Only.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showL1Only.TabIndex = 29;
            this.checkBox_showL1Only.Text = "只显示L1";
            this.checkBox_showL1Only.UseVisualStyleBackColor = true;
            // 
            // checkBox_carrrierInLen
            // 
            this.checkBox_carrrierInLen.AutoSize = true;
            this.checkBox_carrrierInLen.Location = new System.Drawing.Point(431, 49);
            this.checkBox_carrrierInLen.Name = "checkBox_carrrierInLen";
            this.checkBox_carrrierInLen.Size = new System.Drawing.Size(108, 16);
            this.checkBox_carrrierInLen.TabIndex = 29;
            this.checkBox_carrrierInLen.Text = "载波转换为距离";
            this.checkBox_carrrierInLen.UseVisualStyleBackColor = true;
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
            this.fileOpenControl1.Location = new System.Drawing.Point(26, 15);
            this.fileOpenControl1.Margin = new System.Windows.Forms.Padding(4);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(994, 23);
            this.fileOpenControl1.TabIndex = 28;
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
            this.splitContainer1.Size = new System.Drawing.Size(1137, 355);
            this.splitContainer1.SplitterDistance = 233;
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
            this.splitContainer2.Size = new System.Drawing.Size(233, 355);
            this.splitContainer2.SplitterDistance = 174;
            this.splitContainer2.TabIndex = 15;
            // 
            // attributeBox1
            // 
            this.attributeBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeBox1.Location = new System.Drawing.Point(0, 0);
            this.attributeBox1.Margin = new System.Windows.Forms.Padding(4);
            this.attributeBox1.Name = "attributeBox1";
            this.attributeBox1.Size = new System.Drawing.Size(233, 177);
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
            this.tabControl1.Size = new System.Drawing.Size(900, 355);
            this.tabControl1.TabIndex = 17;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(892, 329);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据内容";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(886, 323);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1137, 142);
            this.tabControl2.TabIndex = 22;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.directorySelectionControlOutDir);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1129, 116);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "观测文件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControlOutDir
            // 
            this.directorySelectionControlOutDir.AllowDrop = true;
            this.directorySelectionControlOutDir.IsMultiPathes = false;
            this.directorySelectionControlOutDir.LabelName = "输出目录：";
            this.directorySelectionControlOutDir.Location = new System.Drawing.Point(10, 79);
            this.directorySelectionControlOutDir.Name = "directorySelectionControlOutDir";
            this.directorySelectionControlOutDir.Path = "";
            this.directorySelectionControlOutDir.Pathes = new string[0];
            this.directorySelectionControlOutDir.Size = new System.Drawing.Size(591, 22);
            this.directorySelectionControlOutDir.TabIndex = 18;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox_isWeighted);
            this.tabPage3.Controls.Add(this.checkBox_isApproved);
            this.tabPage3.Controls.Add(this.checkBox_showPoly);
            this.tabPage3.Controls.Add(this.enumRadioControl1);
            this.tabPage3.Controls.Add(this.checkBox_isShowL1Only);
            this.tabPage3.Controls.Add(this.namedFloatControl_satCutoff);
            this.tabPage3.Controls.Add(this.namedIntControl_smoothWindow);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1129, 116);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "设置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox_isWeighted
            // 
            this.checkBox_isWeighted.AutoSize = true;
            this.checkBox_isWeighted.Checked = true;
            this.checkBox_isWeighted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isWeighted.Location = new System.Drawing.Point(235, 28);
            this.checkBox_isWeighted.Name = "checkBox_isWeighted";
            this.checkBox_isWeighted.Size = new System.Drawing.Size(144, 16);
            this.checkBox_isWeighted.TabIndex = 27;
            this.checkBox_isWeighted.Text = "是否加权，否则为推估";
            this.checkBox_isWeighted.UseVisualStyleBackColor = true;
            // 
            // checkBox_isApproved
            // 
            this.checkBox_isApproved.AutoSize = true;
            this.checkBox_isApproved.Checked = true;
            this.checkBox_isApproved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isApproved.Location = new System.Drawing.Point(235, 6);
            this.checkBox_isApproved.Name = "checkBox_isApproved";
            this.checkBox_isApproved.Size = new System.Drawing.Size(246, 16);
            this.checkBox_isApproved.TabIndex = 27;
            this.checkBox_isApproved.Text = "采用GNSSer改进算法，否则原始Hatch算法";
            this.checkBox_isApproved.UseVisualStyleBackColor = true;
            // 
            // checkBox_showPoly
            // 
            this.checkBox_showPoly.AutoSize = true;
            this.checkBox_showPoly.Checked = true;
            this.checkBox_showPoly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showPoly.Location = new System.Drawing.Point(121, 64);
            this.checkBox_showPoly.Name = "checkBox_showPoly";
            this.checkBox_showPoly.Size = new System.Drawing.Size(108, 16);
            this.checkBox_showPoly.TabIndex = 27;
            this.checkBox_showPoly.Text = "显示多项式结果";
            this.checkBox_showPoly.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl1
            // 
            this.enumRadioControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.enumRadioControl1.Location = new System.Drawing.Point(484, 6);
            this.enumRadioControl1.Name = "enumRadioControl1";
            this.enumRadioControl1.Size = new System.Drawing.Size(333, 96);
            this.enumRadioControl1.TabIndex = 0;
            this.enumRadioControl1.Title = "GNSSer 滑动窗口";
            // 
            // checkBox_isShowL1Only
            // 
            this.checkBox_isShowL1Only.AutoSize = true;
            this.checkBox_isShowL1Only.Checked = true;
            this.checkBox_isShowL1Only.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_isShowL1Only.Location = new System.Drawing.Point(19, 64);
            this.checkBox_isShowL1Only.Name = "checkBox_isShowL1Only";
            this.checkBox_isShowL1Only.Size = new System.Drawing.Size(96, 16);
            this.checkBox_isShowL1Only.TabIndex = 27;
            this.checkBox_isShowL1Only.Text = "只显示L1频率";
            this.checkBox_isShowL1Only.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_satCutoff
            // 
            this.namedFloatControl_satCutoff.Location = new System.Drawing.Point(19, 6);
            this.namedFloatControl_satCutoff.Name = "namedFloatControl_satCutoff";
            this.namedFloatControl_satCutoff.Size = new System.Drawing.Size(184, 23);
            this.namedFloatControl_satCutoff.TabIndex = 25;
            this.namedFloatControl_satCutoff.Title = "卫星高度截止角(°)：";
            this.namedFloatControl_satCutoff.Value = 15D;
            // 
            // namedIntControl_smoothWindow
            // 
            this.namedIntControl_smoothWindow.Location = new System.Drawing.Point(72, 35);
            this.namedIntControl_smoothWindow.Name = "namedIntControl_smoothWindow";
            this.namedIntControl_smoothWindow.Size = new System.Drawing.Size(131, 23);
            this.namedIntControl_smoothWindow.TabIndex = 24;
            this.namedIntControl_smoothWindow.Title = "平滑窗口：";
            this.namedIntControl_smoothWindow.Value = 120;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button_CacuIonoFitDelta);
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Controls.Add(this.multiGnssSystemSelectControl1);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.enumRadioControl_ionDifferType);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1129, 116);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "电离层变化改正设置";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button_CacuIonoFitDelta
            // 
            this.button_CacuIonoFitDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CacuIonoFitDelta.Location = new System.Drawing.Point(855, 9);
            this.button_CacuIonoFitDelta.Name = "button_CacuIonoFitDelta";
            this.button_CacuIonoFitDelta.Size = new System.Drawing.Size(116, 74);
            this.button_CacuIonoFitDelta.TabIndex = 59;
            this.button_CacuIonoFitDelta.Text = "拟合所有星电离层变化\r\n(依据改正类型,No为原始数据)";
            this.button_CacuIonoFitDelta.UseVisualStyleBackColor = true;
            this.button_CacuIonoFitDelta.Click += new System.EventHandler(this.button_CacuIonoDelta_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.enumRadioControl_fitType);
            this.groupBox3.Controls.Add(this.button_diffOrderPolyIonoFit);
            this.groupBox3.Controls.Add(this.namedIntControl_toOrder);
            this.groupBox3.Controls.Add(this.namedIntControl_fromOrder);
            this.groupBox3.Location = new System.Drawing.Point(494, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 95);
            this.groupBox3.TabIndex = 58;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "当前星多阶次拟合偏差";
            // 
            // enumRadioControl_fitType
            // 
            this.enumRadioControl_fitType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.enumRadioControl_fitType.Location = new System.Drawing.Point(6, 46);
            this.enumRadioControl_fitType.Name = "enumRadioControl_fitType";
            this.enumRadioControl_fitType.Size = new System.Drawing.Size(274, 48);
            this.enumRadioControl_fitType.TabIndex = 30;
            this.enumRadioControl_fitType.Title = "电离层变化改正类型";
            // 
            // button_diffOrderPolyIonoFit
            // 
            this.button_diffOrderPolyIonoFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_diffOrderPolyIonoFit.Location = new System.Drawing.Point(286, 20);
            this.button_diffOrderPolyIonoFit.Name = "button_diffOrderPolyIonoFit";
            this.button_diffOrderPolyIonoFit.Size = new System.Drawing.Size(63, 69);
            this.button_diffOrderPolyIonoFit.TabIndex = 29;
            this.button_diffOrderPolyIonoFit.Text = "计算";
            this.button_diffOrderPolyIonoFit.UseVisualStyleBackColor = true;
            this.button_diffOrderPolyIonoFit.Click += new System.EventHandler(this.button_diffOrderPolyIonoFit_Click);
            // 
            // namedIntControl_toOrder
            // 
            this.namedIntControl_toOrder.Location = new System.Drawing.Point(137, 20);
            this.namedIntControl_toOrder.Name = "namedIntControl_toOrder";
            this.namedIntControl_toOrder.Size = new System.Drawing.Size(115, 23);
            this.namedIntControl_toOrder.TabIndex = 28;
            this.namedIntControl_toOrder.Title = "结束阶次：";
            this.namedIntControl_toOrder.Value = 4;
            // 
            // namedIntControl_fromOrder
            // 
            this.namedIntControl_fromOrder.Location = new System.Drawing.Point(16, 20);
            this.namedIntControl_fromOrder.Name = "namedIntControl_fromOrder";
            this.namedIntControl_fromOrder.Size = new System.Drawing.Size(115, 23);
            this.namedIntControl_fromOrder.TabIndex = 28;
            this.namedIntControl_fromOrder.Title = "起始阶次：";
            this.namedIntControl_fromOrder.Value = 1;
            // 
            // multiGnssSystemSelectControl1
            // 
            this.multiGnssSystemSelectControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.multiGnssSystemSelectControl1.Location = new System.Drawing.Point(976, 3);
            this.multiGnssSystemSelectControl1.Margin = new System.Windows.Forms.Padding(2);
            this.multiGnssSystemSelectControl1.Name = "multiGnssSystemSelectControl1";
            this.multiGnssSystemSelectControl1.Size = new System.Drawing.Size(150, 110);
            this.multiGnssSystemSelectControl1.TabIndex = 57;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.namedIntControl1_ionoFitEpochCount);
            this.groupBox2.Controls.Add(this.namedIntControl_deltaIonoOrder);
            this.groupBox2.Controls.Add(this.namedIntControl_bufferCount);
            this.groupBox2.Location = new System.Drawing.Point(290, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(198, 104);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电离层拟合选项";
            // 
            // namedIntControl1_ionoFitEpochCount
            // 
            this.namedIntControl1_ionoFitEpochCount.Location = new System.Drawing.Point(33, 17);
            this.namedIntControl1_ionoFitEpochCount.Name = "namedIntControl1_ionoFitEpochCount";
            this.namedIntControl1_ionoFitEpochCount.Size = new System.Drawing.Size(151, 23);
            this.namedIntControl1_ionoFitEpochCount.TabIndex = 28;
            this.namedIntControl1_ionoFitEpochCount.Title = "电离层拟合窗口：";
            this.namedIntControl1_ionoFitEpochCount.Value = 15;
            // 
            // namedIntControl_deltaIonoOrder
            // 
            this.namedIntControl_deltaIonoOrder.Location = new System.Drawing.Point(6, 69);
            this.namedIntControl_deltaIonoOrder.Name = "namedIntControl_deltaIonoOrder";
            this.namedIntControl_deltaIonoOrder.Size = new System.Drawing.Size(178, 23);
            this.namedIntControl_deltaIonoOrder.TabIndex = 24;
            this.namedIntControl_deltaIonoOrder.Title = "电离层变化拟合阶次：";
            this.namedIntControl_deltaIonoOrder.Value = 1;
            // 
            // namedIntControl_bufferCount
            // 
            this.namedIntControl_bufferCount.Location = new System.Drawing.Point(6, 43);
            this.namedIntControl_bufferCount.Name = "namedIntControl_bufferCount";
            this.namedIntControl_bufferCount.Size = new System.Drawing.Size(178, 23);
            this.namedIntControl_bufferCount.TabIndex = 24;
            this.namedIntControl_bufferCount.Title = "缓存大小(用于拟合)：";
            this.namedIntControl_bufferCount.Value = 10;
            // 
            // enumRadioControl_ionDifferType
            // 
            this.enumRadioControl_ionDifferType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.enumRadioControl_ionDifferType.Location = new System.Drawing.Point(3, 6);
            this.enumRadioControl_ionDifferType.Name = "enumRadioControl_ionDifferType";
            this.enumRadioControl_ionDifferType.Size = new System.Drawing.Size(281, 104);
            this.enumRadioControl_ionDifferType.TabIndex = 0;
            this.enumRadioControl_ionDifferType.Title = "电离层变化改正类型";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.checkBox_showTable);
            this.tabPage5.Controls.Add(this.checkBox_openChart);
            this.tabPage5.Controls.Add(this.fileOpenControl_multiPath);
            this.tabPage5.Controls.Add(this.button_multiFile);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1129, 116);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "批量计算";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // checkBox_showTable
            // 
            this.checkBox_showTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_showTable.AutoSize = true;
            this.checkBox_showTable.Location = new System.Drawing.Point(1032, 57);
            this.checkBox_showTable.Name = "checkBox_showTable";
            this.checkBox_showTable.Size = new System.Drawing.Size(72, 16);
            this.checkBox_showTable.TabIndex = 30;
            this.checkBox_showTable.Text = "弹出表格";
            this.checkBox_showTable.UseVisualStyleBackColor = true;
            // 
            // checkBox_openChart
            // 
            this.checkBox_openChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_openChart.AutoSize = true;
            this.checkBox_openChart.Location = new System.Drawing.Point(957, 57);
            this.checkBox_openChart.Name = "checkBox_openChart";
            this.checkBox_openChart.Size = new System.Drawing.Size(72, 16);
            this.checkBox_openChart.TabIndex = 30;
            this.checkBox_openChart.Text = "弹出绘图";
            this.checkBox_openChart.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_multiPath
            // 
            this.fileOpenControl_multiPath.AllowDrop = true;
            this.fileOpenControl_multiPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_multiPath.FilePath = "";
            this.fileOpenControl_multiPath.FilePathes = new string[0];
            this.fileOpenControl_multiPath.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_multiPath.FirstPath = "";
            this.fileOpenControl_multiPath.IsMultiSelect = true;
            this.fileOpenControl_multiPath.LabelName = "文件：";
            this.fileOpenControl_multiPath.Location = new System.Drawing.Point(6, 6);
            this.fileOpenControl_multiPath.Name = "fileOpenControl_multiPath";
            this.fileOpenControl_multiPath.Size = new System.Drawing.Size(912, 78);
            this.fileOpenControl_multiPath.TabIndex = 0;
            // 
            // button_multiFile
            // 
            this.button_multiFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_multiFile.Location = new System.Drawing.Point(957, 6);
            this.button_multiFile.Name = "button_multiFile";
            this.button_multiFile.Size = new System.Drawing.Size(164, 36);
            this.button_multiFile.TabIndex = 1;
            this.button_multiFile.Text = "所有站的电离层变化";
            this.button_multiFile.UseVisualStyleBackColor = true;
            this.button_multiFile.Click += new System.EventHandler(this.button_multiFile_Click);
            // 
            // button_MultiRun
            // 
            this.button_MultiRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_MultiRun.Location = new System.Drawing.Point(1047, 4);
            this.button_MultiRun.Name = "button_MultiRun";
            this.button_MultiRun.Size = new System.Drawing.Size(75, 36);
            this.button_MultiRun.TabIndex = 1;
            this.button_MultiRun.Text = "计算所有星";
            this.button_MultiRun.UseVisualStyleBackColor = true;
            this.button_MultiRun.Click += new System.EventHandler(this.button_viewObs_Click);
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_main.Name = "splitContainer_main";
            this.splitContainer_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.splitContainer_top);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer_main.Size = new System.Drawing.Size(1137, 593);
            this.splitContainer_main.SplitterDistance = 234;
            this.splitContainer_main.TabIndex = 23;
            // 
            // splitContainer_top
            // 
            this.splitContainer_top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_top.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_top.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_top.Name = "splitContainer_top";
            this.splitContainer_top.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_top.Panel1
            // 
            this.splitContainer_top.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer_top.Panel2
            // 
            this.splitContainer_top.Panel2.Controls.Add(this.progressBarComponent1);
            this.splitContainer_top.Panel2.Controls.Add(this.button_solveAllRate);
            this.splitContainer_top.Panel2.Controls.Add(this.button_ionoDifferByLL);
            this.splitContainer_top.Panel2.Controls.Add(this.button_p_L_div2);
            this.splitContainer_top.Panel2.Controls.Add(this.button_smoothCurrent);
            this.splitContainer_top.Panel2.Controls.Add(this.button_MultiRun);
            this.splitContainer_top.Size = new System.Drawing.Size(1137, 234);
            this.splitContainer_top.SplitterDistance = 142;
            this.splitContainer_top.TabIndex = 0;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(12, 7);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(731, 34);
            this.progressBarComponent1.TabIndex = 2;
            // 
            // button_solveAllRate
            // 
            this.button_solveAllRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_solveAllRate.Location = new System.Drawing.Point(944, 47);
            this.button_solveAllRate.Name = "button_solveAllRate";
            this.button_solveAllRate.Size = new System.Drawing.Size(164, 36);
            this.button_solveAllRate.TabIndex = 1;
            this.button_solveAllRate.Text = "所有星电离层精确变化";
            this.button_solveAllRate.UseVisualStyleBackColor = true;
            this.button_solveAllRate.Click += new System.EventHandler(this.button_solveAllRate_Click);
            // 
            // button_ionoDifferByLL
            // 
            this.button_ionoDifferByLL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ionoDifferByLL.Location = new System.Drawing.Point(758, 49);
            this.button_ionoDifferByLL.Name = "button_ionoDifferByLL";
            this.button_ionoDifferByLL.Size = new System.Drawing.Size(164, 36);
            this.button_ionoDifferByLL.TabIndex = 1;
            this.button_ionoDifferByLL.Text = "当前星电离层精确变化";
            this.button_ionoDifferByLL.UseVisualStyleBackColor = true;
            this.button_ionoDifferByLL.Click += new System.EventHandler(this.button_ionoDifferByLL_Click);
            // 
            // button_p_L_div2
            // 
            this.button_p_L_div2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_p_L_div2.Location = new System.Drawing.Point(758, 7);
            this.button_p_L_div2.Name = "button_p_L_div2";
            this.button_p_L_div2.Size = new System.Drawing.Size(164, 36);
            this.button_p_L_div2.TabIndex = 1;
            this.button_p_L_div2.Text = "当前星多法求电离层变化";
            this.button_p_L_div2.UseVisualStyleBackColor = true;
            this.button_p_L_div2.Click += new System.EventHandler(this.button_p_L_div2_Click);
            // 
            // button_smoothCurrent
            // 
            this.button_smoothCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_smoothCurrent.Location = new System.Drawing.Point(944, 5);
            this.button_smoothCurrent.Name = "button_smoothCurrent";
            this.button_smoothCurrent.Size = new System.Drawing.Size(75, 36);
            this.button_smoothCurrent.TabIndex = 1;
            this.button_smoothCurrent.Text = "计算当前星";
            this.button_smoothCurrent.UseVisualStyleBackColor = true;
            this.button_smoothCurrent.Click += new System.EventHandler(this.button_smoothCurrent_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // IonoTrendCaculateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 593);
            this.Controls.Add(this.splitContainer_main);
            this.Name = "IonoTrendCaculateForm";
            this.Text = "载波相位平滑伪距";
            this.Load += new System.EventHandler(this.ObsFileViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_obsInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_sat)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.splitContainer_top.Panel1.ResumeLayout(false);
            this.splitContainer_top.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_top)).EndInit();
            this.splitContainer_top.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog_obs;
        private System.Windows.Forms.BindingSource bindingSource_obsInfo;
        private System.Windows.Forms.Button button_view;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_sat;
        private System.Windows.Forms.BindingSource bindingSource_sat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_show;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.AttributeBox attributeBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button_MultiRun;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.SplitContainer splitContainer_top;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        protected Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_smoothCurrent;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_smoothWindow;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_satCutoff;
        private System.Windows.Forms.CheckBox checkBox_isShowL1Only;
        private System.Windows.Forms.CheckBox checkBox_isWeighted;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_deltaIonoOrder;
        private System.Windows.Forms.CheckBox checkBox_isApproved;
        private System.Windows.Forms.CheckBox checkBox_showPoly;
        private System.Windows.Forms.TabPage tabPage4;
        private Geo.Winform.EnumRadioControl enumRadioControl1;
        private System.Windows.Forms.CheckBox checkBox_carrrierInLen;
        private System.Windows.Forms.CheckBox checkBox_showL1Only;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_bufferCount;
        private Geo.Winform.Controls.NamedIntControl namedIntControl1_ionoFitEpochCount;
        private Geo.Winform.EnumRadioControl enumRadioControl_ionDifferType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_p_L_div2;
        private System.Windows.Forms.Button button_ionoDifferByLL;
        private System.Windows.Forms.Button button_solveAllRate;
        private System.Windows.Forms.TabPage tabPage5;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_multiPath;
        private System.Windows.Forms.Button button_multiFile;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControlOutDir;
        private Controls.MultiGnssSystemSelectControl multiGnssSystemSelectControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBox_openChart;
        private System.Windows.Forms.CheckBox checkBox_showTable;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_diffOrderPolyIonoFit;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_toOrder;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_fromOrder;
        private System.Windows.Forms.Button button_CacuIonoFitDelta;
        private Geo.Winform.EnumRadioControl enumRadioControl_fitType;
    }
}

