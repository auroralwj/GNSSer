namespace Gnsser.Winform
{
    partial class SingleSitePointPositionSolverForm
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
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage_common = new System.Windows.Forms.TabPage();
            this.namedFloatControl_periodSpanMinutes = new Geo.Winform.Controls.NamedFloatControl();
            this.namedStringControl_projName = new Geo.Winform.Controls.NamedStringControl();
            this.checkBox_enableNet = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl1_projDir = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage6_baselineSolver = new System.Windows.Forms.TabPage();
            this.multiSolverOptionControl1 = new Gnsser.Winform.Controls.MultiSolverOptionControl();
            this.enumRadioControl_obsType = new Geo.Winform.EnumRadioControl();
            this.parallelConfigControl1 = new Geo.Winform.Controls.ParallelConfigControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.namedIntControl_removeFirstEpochCount = new Geo.Winform.Controls.NamedIntControl();
            this.splitContainer_content = new System.Windows.Forms.SplitContainer();
            this.splitContainer_leftTree = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage_site = new System.Windows.Forms.TabPage();
            this.treeView_sites = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_site = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导入文件IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.打开所在目录OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开当前文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.展开收缩测站EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.查看编辑所选测站时段EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看编辑所有测站时段AToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复原始文件RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复所有测站原始文件SToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.地图显示测站SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看所有测站时段图PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看卫星高度角HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.pPP计算并更新头文件PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pPP计算并更新所选头文件SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.所有文件PPP并更新头文件MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.查看当前测站PPP收敛图VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开当前测站PPP结果RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.查看所有测站PPP收敛图AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label_siteInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_showSitePeriod = new System.Windows.Forms.Button();
            this.button1_import = new System.Windows.Forms.Button();
            this.button_mapShowSites = new System.Windows.Forms.Button();
            this.tabPage_period = new System.Windows.Forms.TabPage();
            this.treeView_periods = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_periods = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.展开收缩时段EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.移除所选时段RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_periods = new System.Windows.Forms.Label();
            this.tabPage_lineList = new System.Windows.Forms.TabPage();
            this.listBox_vector = new System.Windows.Forms.ListBox();
            this.bindingSource_allLines = new System.Windows.Forms.BindingSource(this.components);
            this.panel9 = new System.Windows.Forms.Panel();
            this.button_solveCurrentLine = new System.Windows.Forms.Button();
            this.button_runAllBaseLine = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_listAll = new System.Windows.Forms.Label();
            this.attributeBox1 = new Geo.Winform.AttributeBox();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.tabControl_leftContent = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl_currentText = new System.Windows.Forms.TabControl();
            this.tabPage_currentText = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_currentLine = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage_chart = new System.Windows.Forms.TabPage();
            this.splitContainer_chart = new System.Windows.Forms.SplitContainer();
            this.commonChartControl_currentResidual = new Geo.Winform.CommonChartControl();
            this.commonChartControl_currentParamConvergence = new Geo.Winform.CommonChartControl();
            this.tabPage_synClosure = new System.Windows.Forms.TabPage();
            this.objectTableControl_currentResidual = new Geo.Winform.ObjectTableControl();
            this.tabPage_repeatError = new System.Windows.Forms.TabPage();
            this.objectTableControl_currentRepeatError = new Geo.Winform.ObjectTableControl();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.objectTableControl_positionLineResult = new Geo.Winform.ObjectTableControl();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button_saveAllAsGNSSerFile = new System.Windows.Forms.Button();
            this.openFileDialog1_rinexOFile = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.checkBox_replaceApproxCoordWhenPPP = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage_common.SuspendLayout();
            this.tabPage6_baselineSolver.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_content)).BeginInit();
            this.splitContainer_content.Panel1.SuspendLayout();
            this.splitContainer_content.Panel2.SuspendLayout();
            this.splitContainer_content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_leftTree)).BeginInit();
            this.splitContainer_leftTree.Panel1.SuspendLayout();
            this.splitContainer_leftTree.Panel2.SuspendLayout();
            this.splitContainer_leftTree.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage_site.SuspendLayout();
            this.contextMenuStrip_site.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage_period.SuspendLayout();
            this.contextMenuStrip_periods.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage_lineList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_allLines)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl_leftContent.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl_currentText.SuspendLayout();
            this.tabPage_currentText.SuspendLayout();
            this.tabPage_chart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_chart)).BeginInit();
            this.splitContainer_chart.Panel1.SuspendLayout();
            this.splitContainer_chart.Panel2.SuspendLayout();
            this.splitContainer_chart.SuspendLayout();
            this.tabPage_synClosure.SuspendLayout();
            this.tabPage_repeatError.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer_main.Panel1.Controls.Add(this.tabControl3);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.splitContainer_content);
            this.splitContainer_main.Size = new System.Drawing.Size(808, 548);
            this.splitContainer_main.SplitterDistance = 94;
            this.splitContainer_main.TabIndex = 0;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage_common);
            this.tabControl3.Controls.Add(this.tabPage6_baselineSolver);
            this.tabControl3.Controls.Add(this.tabPage7);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(808, 94);
            this.tabControl3.TabIndex = 7;
            // 
            // tabPage_common
            // 
            this.tabPage_common.Controls.Add(this.namedFloatControl_periodSpanMinutes);
            this.tabPage_common.Controls.Add(this.namedStringControl_projName);
            this.tabPage_common.Controls.Add(this.checkBox_enableNet);
            this.tabPage_common.Controls.Add(this.directorySelectionControl1_projDir);
            this.tabPage_common.Location = new System.Drawing.Point(4, 22);
            this.tabPage_common.Name = "tabPage_common";
            this.tabPage_common.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_common.Size = new System.Drawing.Size(800, 68);
            this.tabPage_common.TabIndex = 0;
            this.tabPage_common.Text = "通用设置";
            this.tabPage_common.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_periodSpanMinutes
            // 
            this.namedFloatControl_periodSpanMinutes.Location = new System.Drawing.Point(264, 34);
            this.namedFloatControl_periodSpanMinutes.Name = "namedFloatControl_periodSpanMinutes";
            this.namedFloatControl_periodSpanMinutes.Size = new System.Drawing.Size(172, 23);
            this.namedFloatControl_periodSpanMinutes.TabIndex = 58;
            this.namedFloatControl_periodSpanMinutes.Title = "最小公共时段(分)：";
            this.namedFloatControl_periodSpanMinutes.Value = 10D;
            // 
            // namedStringControl_projName
            // 
            this.namedStringControl_projName.Location = new System.Drawing.Point(30, 34);
            this.namedStringControl_projName.Name = "namedStringControl_projName";
            this.namedStringControl_projName.Size = new System.Drawing.Size(217, 23);
            this.namedStringControl_projName.TabIndex = 56;
            this.namedStringControl_projName.Title = "工程名称：";
            // 
            // checkBox_enableNet
            // 
            this.checkBox_enableNet.AutoSize = true;
            this.checkBox_enableNet.Checked = true;
            this.checkBox_enableNet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enableNet.Location = new System.Drawing.Point(477, 34);
            this.checkBox_enableNet.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_enableNet.Name = "checkBox_enableNet";
            this.checkBox_enableNet.Size = new System.Drawing.Size(96, 16);
            this.checkBox_enableNet.TabIndex = 55;
            this.checkBox_enableNet.Text = "允许访问网络";
            this.checkBox_enableNet.UseVisualStyleBackColor = true;
            this.checkBox_enableNet.CheckedChanged += new System.EventHandler(this.checkBox_enableNet_CheckedChanged);
            // 
            // directorySelectionControl1_projDir
            // 
            this.directorySelectionControl1_projDir.AllowDrop = true;
            this.directorySelectionControl1_projDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1_projDir.IsAddOrReplase = false;
            this.directorySelectionControl1_projDir.IsMultiPathes = false;
            this.directorySelectionControl1_projDir.LabelName = "当前工程目录：";
            this.directorySelectionControl1_projDir.Location = new System.Drawing.Point(8, 6);
            this.directorySelectionControl1_projDir.Name = "directorySelectionControl1_projDir";
            this.directorySelectionControl1_projDir.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1_projDir.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1_projDir.Size = new System.Drawing.Size(786, 22);
            this.directorySelectionControl1_projDir.TabIndex = 4;
            // 
            // tabPage6_baselineSolver
            // 
            this.tabPage6_baselineSolver.Controls.Add(this.multiSolverOptionControl1);
            this.tabPage6_baselineSolver.Controls.Add(this.enumRadioControl_obsType);
            this.tabPage6_baselineSolver.Controls.Add(this.parallelConfigControl1);
            this.tabPage6_baselineSolver.Location = new System.Drawing.Point(4, 22);
            this.tabPage6_baselineSolver.Name = "tabPage6_baselineSolver";
            this.tabPage6_baselineSolver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6_baselineSolver.Size = new System.Drawing.Size(800, 68);
            this.tabPage6_baselineSolver.TabIndex = 1;
            this.tabPage6_baselineSolver.Text = "算法详细设置";
            this.tabPage6_baselineSolver.UseVisualStyleBackColor = true;
            // 
            // multiSolverOptionControl1
            // 
            this.multiSolverOptionControl1.Location = new System.Drawing.Point(0, 0);
            this.multiSolverOptionControl1.Name = "multiSolverOptionControl1";
            this.multiSolverOptionControl1.Size = new System.Drawing.Size(394, 68);
            this.multiSolverOptionControl1.TabIndex = 63;
            // 
            // enumRadioControl_obsType
            // 
            this.enumRadioControl_obsType.IsReady = false;
            this.enumRadioControl_obsType.Location = new System.Drawing.Point(401, 4);
            this.enumRadioControl_obsType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_obsType.Name = "enumRadioControl_obsType";
            this.enumRadioControl_obsType.Size = new System.Drawing.Size(211, 65);
            this.enumRadioControl_obsType.TabIndex = 61;
            this.enumRadioControl_obsType.Title = "观测值选项(单频短基线有效)";
            // 
            // parallelConfigControl1
            // 
            this.parallelConfigControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.parallelConfigControl1.EnableParallel = true;
            this.parallelConfigControl1.Location = new System.Drawing.Point(618, 3);
            this.parallelConfigControl1.Margin = new System.Windows.Forms.Padding(2);
            this.parallelConfigControl1.Name = "parallelConfigControl1";
            this.parallelConfigControl1.Size = new System.Drawing.Size(179, 62);
            this.parallelConfigControl1.TabIndex = 62;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.checkBox_replaceApproxCoordWhenPPP);
            this.tabPage7.Controls.Add(this.label2);
            this.tabPage7.Controls.Add(this.namedIntControl_removeFirstEpochCount);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(800, 68);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "其它";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 12);
            this.label2.TabIndex = 59;
            this.label2.Text = "注意：颜色为最佳同步环结果";
            // 
            // namedIntControl_removeFirstEpochCount
            // 
            this.namedIntControl_removeFirstEpochCount.Location = new System.Drawing.Point(3, 15);
            this.namedIntControl_removeFirstEpochCount.Name = "namedIntControl_removeFirstEpochCount";
            this.namedIntControl_removeFirstEpochCount.Size = new System.Drawing.Size(209, 23);
            this.namedIntControl_removeFirstEpochCount.TabIndex = 58;
            this.namedIntControl_removeFirstEpochCount.Title = "收敛参数绘图的起始历元：";
            this.namedIntControl_removeFirstEpochCount.Value = 60;
            // 
            // splitContainer_content
            // 
            this.splitContainer_content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_content.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_content.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_content.Name = "splitContainer_content";
            // 
            // splitContainer_content.Panel1
            // 
            this.splitContainer_content.Panel1.Controls.Add(this.splitContainer_leftTree);
            // 
            // splitContainer_content.Panel2
            // 
            this.splitContainer_content.Panel2.Controls.Add(this.tabControl_leftContent);
            this.splitContainer_content.Size = new System.Drawing.Size(808, 450);
            this.splitContainer_content.SplitterDistance = 234;
            this.splitContainer_content.TabIndex = 0;
            // 
            // splitContainer_leftTree
            // 
            this.splitContainer_leftTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_leftTree.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer_leftTree.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_leftTree.Name = "splitContainer_leftTree";
            this.splitContainer_leftTree.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_leftTree.Panel1
            // 
            this.splitContainer_leftTree.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer_leftTree.Panel2
            // 
            this.splitContainer_leftTree.Panel2.Controls.Add(this.attributeBox1);
            this.splitContainer_leftTree.Panel2.Controls.Add(this.progressBarComponent1);
            this.splitContainer_leftTree.Size = new System.Drawing.Size(234, 450);
            this.splitContainer_leftTree.SplitterDistance = 277;
            this.splitContainer_leftTree.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage_site);
            this.tabControl2.Controls.Add(this.tabPage_period);
            this.tabControl2.Controls.Add(this.tabPage_lineList);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(234, 277);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage_site
            // 
            this.tabPage_site.Controls.Add(this.treeView_sites);
            this.tabPage_site.Controls.Add(this.panel4);
            this.tabPage_site.Controls.Add(this.panel3);
            this.tabPage_site.Location = new System.Drawing.Point(4, 22);
            this.tabPage_site.Name = "tabPage_site";
            this.tabPage_site.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_site.Size = new System.Drawing.Size(226, 251);
            this.tabPage_site.TabIndex = 0;
            this.tabPage_site.Text = "测站";
            this.tabPage_site.UseVisualStyleBackColor = true;
            // 
            // treeView_sites
            // 
            this.treeView_sites.ContextMenuStrip = this.contextMenuStrip_site;
            this.treeView_sites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_sites.Location = new System.Drawing.Point(3, 33);
            this.treeView_sites.Name = "treeView_sites";
            this.treeView_sites.Size = new System.Drawing.Size(220, 199);
            this.treeView_sites.TabIndex = 0;
            this.treeView_sites.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_sites_AfterSelect);
            this.treeView_sites.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_sites_MouseDoubleClick);
            // 
            // contextMenuStrip_site
            // 
            this.contextMenuStrip_site.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_site.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入文件IToolStripMenuItem,
            this.toolStripSeparator9,
            this.打开所在目录OToolStripMenuItem,
            this.打开当前文件FToolStripMenuItem,
            this.toolStripSeparator1,
            this.展开收缩测站EToolStripMenuItem,
            this.toolStripSeparator17,
            this.查看编辑所选测站时段EToolStripMenuItem,
            this.恢复原始文件RToolStripMenuItem,
            this.toolStripSeparator2,
            this.地图显示测站SToolStripMenuItem,
            this.查看所有测站时段图PToolStripMenuItem,
            this.查看卫星高度角HToolStripMenuItem,
            this.toolStripSeparator6,
            this.pPP计算并更新头文件PToolStripMenuItem,
            this.toolStripSeparator5,
            this.移除ToolStripMenuItem,
            this.清空ToolStripMenuItem});
            this.contextMenuStrip_site.Name = "contextMenuStrip1";
            this.contextMenuStrip_site.Size = new System.Drawing.Size(224, 304);
            // 
            // 导入文件IToolStripMenuItem
            // 
            this.导入文件IToolStripMenuItem.Name = "导入文件IToolStripMenuItem";
            this.导入文件IToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.导入文件IToolStripMenuItem.Text = "导入文件(&I)";
            this.导入文件IToolStripMenuItem.Click += new System.EventHandler(this.导入文件IToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(220, 6);
            // 
            // 打开所在目录OToolStripMenuItem
            // 
            this.打开所在目录OToolStripMenuItem.Name = "打开所在目录OToolStripMenuItem";
            this.打开所在目录OToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.打开所在目录OToolStripMenuItem.Text = "打开所在目录(&O)";
            this.打开所在目录OToolStripMenuItem.Click += new System.EventHandler(this.打开所在目录OToolStripMenuItem_Click);
            // 
            // 打开当前文件FToolStripMenuItem
            // 
            this.打开当前文件FToolStripMenuItem.Name = "打开当前文件FToolStripMenuItem";
            this.打开当前文件FToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.打开当前文件FToolStripMenuItem.Text = "打开当前文件(&F)";
            this.打开当前文件FToolStripMenuItem.Click += new System.EventHandler(this.打开当前文件FToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // 展开收缩测站EToolStripMenuItem
            // 
            this.展开收缩测站EToolStripMenuItem.Name = "展开收缩测站EToolStripMenuItem";
            this.展开收缩测站EToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.展开收缩测站EToolStripMenuItem.Text = "展开/收缩测站(&E)";
            this.展开收缩测站EToolStripMenuItem.Click += new System.EventHandler(this.展开收缩测站EToolStripMenuItem_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(220, 6);
            // 
            // 查看编辑所选测站时段EToolStripMenuItem
            // 
            this.查看编辑所选测站时段EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看编辑所有测站时段AToolStripMenuItem1});
            this.查看编辑所选测站时段EToolStripMenuItem.Name = "查看编辑所选测站时段EToolStripMenuItem";
            this.查看编辑所选测站时段EToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.查看编辑所选测站时段EToolStripMenuItem.Text = "查看/编辑 当前测站时段(&E)";
            this.查看编辑所选测站时段EToolStripMenuItem.Click += new System.EventHandler(this.查看编辑所选测站时段EToolStripMenuItem_Click);
            // 
            // 查看编辑所有测站时段AToolStripMenuItem1
            // 
            this.查看编辑所有测站时段AToolStripMenuItem1.Name = "查看编辑所有测站时段AToolStripMenuItem1";
            this.查看编辑所有测站时段AToolStripMenuItem1.Size = new System.Drawing.Size(221, 22);
            this.查看编辑所有测站时段AToolStripMenuItem1.Text = "查看/编辑 所有测站时段(&A)";
            this.查看编辑所有测站时段AToolStripMenuItem1.Click += new System.EventHandler(this.查看编辑所有测站时段AToolStripMenuItem1_Click);
            // 
            // 恢复原始文件RToolStripMenuItem
            // 
            this.恢复原始文件RToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.恢复所有测站原始文件SToolStripMenuItem1});
            this.恢复原始文件RToolStripMenuItem.Name = "恢复原始文件RToolStripMenuItem";
            this.恢复原始文件RToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.恢复原始文件RToolStripMenuItem.Text = "恢复当前测站原始文件(&R)";
            this.恢复原始文件RToolStripMenuItem.Click += new System.EventHandler(this.恢复原始文件RToolStripMenuItem_Click);
            // 
            // 恢复所有测站原始文件SToolStripMenuItem1
            // 
            this.恢复所有测站原始文件SToolStripMenuItem1.Name = "恢复所有测站原始文件SToolStripMenuItem1";
            this.恢复所有测站原始文件SToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.恢复所有测站原始文件SToolStripMenuItem1.Text = "恢复所有测站原始文件(&S)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(220, 6);
            // 
            // 地图显示测站SToolStripMenuItem
            // 
            this.地图显示测站SToolStripMenuItem.Name = "地图显示测站SToolStripMenuItem";
            this.地图显示测站SToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.地图显示测站SToolStripMenuItem.Text = "地图显示测站(&S)";
            this.地图显示测站SToolStripMenuItem.Click += new System.EventHandler(this.地图显示测站SToolStripMenuItem_Click);
            // 
            // 查看所有测站时段图PToolStripMenuItem
            // 
            this.查看所有测站时段图PToolStripMenuItem.Name = "查看所有测站时段图PToolStripMenuItem";
            this.查看所有测站时段图PToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.查看所有测站时段图PToolStripMenuItem.Text = "查看所有测站共同时段图(&P)";
            this.查看所有测站时段图PToolStripMenuItem.Click += new System.EventHandler(this.查看所有测站时段图PToolStripMenuItem_Click);
            // 
            // 查看卫星高度角HToolStripMenuItem
            // 
            this.查看卫星高度角HToolStripMenuItem.Name = "查看卫星高度角HToolStripMenuItem";
            this.查看卫星高度角HToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.查看卫星高度角HToolStripMenuItem.Text = "查看卫星高度角(&H)";
            this.查看卫星高度角HToolStripMenuItem.Click += new System.EventHandler(this.查看卫星高度角HToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(220, 6);
            // 
            // pPP计算并更新头文件PToolStripMenuItem
            // 
            this.pPP计算并更新头文件PToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pPP计算并更新所选头文件SToolStripMenuItem,
            this.所有文件PPP并更新头文件MToolStripMenuItem,
            this.toolStripSeparator14,
            this.查看当前测站PPP收敛图VToolStripMenuItem,
            this.打开当前测站PPP结果RToolStripMenuItem,
            this.toolStripSeparator15,
            this.查看所有测站PPP收敛图AToolStripMenuItem});
            this.pPP计算并更新头文件PToolStripMenuItem.Name = "pPP计算并更新头文件PToolStripMenuItem";
            this.pPP计算并更新头文件PToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.pPP计算并更新头文件PToolStripMenuItem.Text = "PPP计算并更新头文件(&P)";
            // 
            // pPP计算并更新所选头文件SToolStripMenuItem
            // 
            this.pPP计算并更新所选头文件SToolStripMenuItem.Name = "pPP计算并更新所选头文件SToolStripMenuItem";
            this.pPP计算并更新所选头文件SToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.pPP计算并更新所选头文件SToolStripMenuItem.Text = "PPP更新当前头文件(&S)";
            this.pPP计算并更新所选头文件SToolStripMenuItem.Click += new System.EventHandler(this.pPP计算并更新所选头文件SToolStripMenuItem_Click);
            // 
            // 所有文件PPP并更新头文件MToolStripMenuItem
            // 
            this.所有文件PPP并更新头文件MToolStripMenuItem.Name = "所有文件PPP并更新头文件MToolStripMenuItem";
            this.所有文件PPP并更新头文件MToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.所有文件PPP并更新头文件MToolStripMenuItem.Text = "PPP更新所有头文件(&M)";
            this.所有文件PPP并更新头文件MToolStripMenuItem.Click += new System.EventHandler(this.所有文件PPP并更新头文件MToolStripMenuItem_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(218, 6);
            // 
            // 查看当前测站PPP收敛图VToolStripMenuItem
            // 
            this.查看当前测站PPP收敛图VToolStripMenuItem.Name = "查看当前测站PPP收敛图VToolStripMenuItem";
            this.查看当前测站PPP收敛图VToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.查看当前测站PPP收敛图VToolStripMenuItem.Text = "查看当前测站PPP收敛图(&V)";
            this.查看当前测站PPP收敛图VToolStripMenuItem.Click += new System.EventHandler(this.查看当前测站PPP收敛图VToolStripMenuItem_Click);
            // 
            // 打开当前测站PPP结果RToolStripMenuItem
            // 
            this.打开当前测站PPP结果RToolStripMenuItem.Name = "打开当前测站PPP结果RToolStripMenuItem";
            this.打开当前测站PPP结果RToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.打开当前测站PPP结果RToolStripMenuItem.Text = "打开当前测站PPP结果(&R)";
            this.打开当前测站PPP结果RToolStripMenuItem.Click += new System.EventHandler(this.打开当前测站PPP结果RToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(218, 6);
            // 
            // 查看所有测站PPP收敛图AToolStripMenuItem
            // 
            this.查看所有测站PPP收敛图AToolStripMenuItem.Name = "查看所有测站PPP收敛图AToolStripMenuItem";
            this.查看所有测站PPP收敛图AToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.查看所有测站PPP收敛图AToolStripMenuItem.Text = "查看所有测站PPP收敛图(&A)";
            this.查看所有测站PPP收敛图AToolStripMenuItem.Click += new System.EventHandler(this.查看所有测站PPP收敛图AToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(220, 6);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.移除ToolStripMenuItem.Text = "移除所选测站(&R)";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.清空ToolStripMenuItem.Text = "清空(&C)";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.清空ToolStripMenuItem_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label_siteInfo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 232);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(220, 16);
            this.panel4.TabIndex = 4;
            // 
            // label_siteInfo
            // 
            this.label_siteInfo.AutoSize = true;
            this.label_siteInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_siteInfo.Location = new System.Drawing.Point(0, 0);
            this.label_siteInfo.Name = "label_siteInfo";
            this.label_siteInfo.Size = new System.Drawing.Size(53, 12);
            this.label_siteInfo.TabIndex = 0;
            this.label_siteInfo.Text = "测站信息";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_showSitePeriod);
            this.panel3.Controls.Add(this.button1_import);
            this.panel3.Controls.Add(this.button_mapShowSites);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(220, 30);
            this.panel3.TabIndex = 3;
            // 
            // button_showSitePeriod
            // 
            this.button_showSitePeriod.Location = new System.Drawing.Point(142, 3);
            this.button_showSitePeriod.Name = "button_showSitePeriod";
            this.button_showSitePeriod.Size = new System.Drawing.Size(75, 23);
            this.button_showSitePeriod.TabIndex = 9;
            this.button_showSitePeriod.Text = "时段图";
            this.button_showSitePeriod.UseVisualStyleBackColor = true;
            this.button_showSitePeriod.Click += new System.EventHandler(this.查看所有测站时段图PToolStripMenuItem_Click);
            // 
            // button1_import
            // 
            this.button1_import.Location = new System.Drawing.Point(3, 3);
            this.button1_import.Name = "button1_import";
            this.button1_import.Size = new System.Drawing.Size(66, 23);
            this.button1_import.TabIndex = 9;
            this.button1_import.Text = "导入文件";
            this.button1_import.UseVisualStyleBackColor = true;
            this.button1_import.Click += new System.EventHandler(this.导入文件IToolStripMenuItem_Click);
            // 
            // button_mapShowSites
            // 
            this.button_mapShowSites.Location = new System.Drawing.Point(73, 3);
            this.button_mapShowSites.Name = "button_mapShowSites";
            this.button_mapShowSites.Size = new System.Drawing.Size(65, 23);
            this.button_mapShowSites.TabIndex = 9;
            this.button_mapShowSites.Text = "地图显示";
            this.button_mapShowSites.UseVisualStyleBackColor = true;
            this.button_mapShowSites.Click += new System.EventHandler(this.地图显示测站SToolStripMenuItem_Click);
            // 
            // tabPage_period
            // 
            this.tabPage_period.Controls.Add(this.treeView_periods);
            this.tabPage_period.Controls.Add(this.panel2);
            this.tabPage_period.Location = new System.Drawing.Point(4, 22);
            this.tabPage_period.Name = "tabPage_period";
            this.tabPage_period.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_period.Size = new System.Drawing.Size(226, 251);
            this.tabPage_period.TabIndex = 2;
            this.tabPage_period.Text = "时段";
            this.tabPage_period.UseVisualStyleBackColor = true;
            // 
            // treeView_periods
            // 
            this.treeView_periods.ContextMenuStrip = this.contextMenuStrip_periods;
            this.treeView_periods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_periods.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView_periods.Location = new System.Drawing.Point(3, 3);
            this.treeView_periods.Name = "treeView_periods";
            this.treeView_periods.Size = new System.Drawing.Size(220, 229);
            this.treeView_periods.TabIndex = 7;
            this.treeView_periods.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_periods_DrawNode);
            this.treeView_periods.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_periods_AfterSelect);
            // 
            // contextMenuStrip_periods
            // 
            this.contextMenuStrip_periods.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.展开收缩时段EToolStripMenuItem,
            this.toolStripSeparator11,
            this.toolStripSeparator12,
            this.移除所选时段RToolStripMenuItem});
            this.contextMenuStrip_periods.Name = "contextMenuStrip_periods";
            this.contextMenuStrip_periods.Size = new System.Drawing.Size(169, 60);
            // 
            // 展开收缩时段EToolStripMenuItem
            // 
            this.展开收缩时段EToolStripMenuItem.Name = "展开收缩时段EToolStripMenuItem";
            this.展开收缩时段EToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.展开收缩时段EToolStripMenuItem.Text = "展开/收缩时段(&E)";
            this.展开收缩时段EToolStripMenuItem.Click += new System.EventHandler(this.展开收缩时段EToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(165, 6);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(165, 6);
            // 
            // 移除所选时段RToolStripMenuItem
            // 
            this.移除所选时段RToolStripMenuItem.Name = "移除所选时段RToolStripMenuItem";
            this.移除所选时段RToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.移除所选时段RToolStripMenuItem.Text = "移除所选时段(&R)";
            this.移除所选时段RToolStripMenuItem.Click += new System.EventHandler(this.移除所选时段RToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_periods);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 232);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(220, 16);
            this.panel2.TabIndex = 8;
            // 
            // label_periods
            // 
            this.label_periods.AutoSize = true;
            this.label_periods.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_periods.Location = new System.Drawing.Point(0, 0);
            this.label_periods.Name = "label_periods";
            this.label_periods.Size = new System.Drawing.Size(53, 12);
            this.label_periods.TabIndex = 0;
            this.label_periods.Text = "基线信息";
            // 
            // tabPage_lineList
            // 
            this.tabPage_lineList.Controls.Add(this.listBox_vector);
            this.tabPage_lineList.Controls.Add(this.panel9);
            this.tabPage_lineList.Controls.Add(this.panel1);
            this.tabPage_lineList.Location = new System.Drawing.Point(4, 22);
            this.tabPage_lineList.Name = "tabPage_lineList";
            this.tabPage_lineList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_lineList.Size = new System.Drawing.Size(226, 251);
            this.tabPage_lineList.TabIndex = 3;
            this.tabPage_lineList.Text = "列表";
            this.tabPage_lineList.UseVisualStyleBackColor = true;
            // 
            // listBox_vector
            // 
            this.listBox_vector.DataSource = this.bindingSource_allLines;
            this.listBox_vector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_vector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox_vector.FormattingEnabled = true;
            this.listBox_vector.ItemHeight = 12;
            this.listBox_vector.Location = new System.Drawing.Point(3, 33);
            this.listBox_vector.Name = "listBox_vector";
            this.listBox_vector.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_vector.Size = new System.Drawing.Size(220, 194);
            this.listBox_vector.TabIndex = 3;
            this.listBox_vector.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_vector_DrawItem);
            this.listBox_vector.SelectedIndexChanged += new System.EventHandler(this.listBox_vector_SelectedIndexChanged);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.button_solveCurrentLine);
            this.panel9.Controls.Add(this.button_runAllBaseLine);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(220, 30);
            this.panel9.TabIndex = 6;
            // 
            // button_solveCurrentLine
            // 
            this.button_solveCurrentLine.Location = new System.Drawing.Point(1, 3);
            this.button_solveCurrentLine.Name = "button_solveCurrentLine";
            this.button_solveCurrentLine.Size = new System.Drawing.Size(72, 23);
            this.button_solveCurrentLine.TabIndex = 3;
            this.button_solveCurrentLine.Text = "计算所选";
            this.button_solveCurrentLine.UseVisualStyleBackColor = true;
            this.button_solveCurrentLine.Click += new System.EventHandler(this.button_solveCurrentLine_Click);
            // 
            // button_runAllBaseLine
            // 
            this.button_runAllBaseLine.Location = new System.Drawing.Point(79, 3);
            this.button_runAllBaseLine.Name = "button_runAllBaseLine";
            this.button_runAllBaseLine.Size = new System.Drawing.Size(67, 23);
            this.button_runAllBaseLine.TabIndex = 3;
            this.button_runAllBaseLine.Text = "计算所有";
            this.button_runAllBaseLine.UseVisualStyleBackColor = true;
            this.button_runAllBaseLine.Click += new System.EventHandler(this.button_runAllBaseLine_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_listAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 227);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 21);
            this.panel1.TabIndex = 5;
            // 
            // label_listAll
            // 
            this.label_listAll.AutoSize = true;
            this.label_listAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_listAll.Location = new System.Drawing.Point(0, 0);
            this.label_listAll.Name = "label_listAll";
            this.label_listAll.Size = new System.Drawing.Size(29, 12);
            this.label_listAll.TabIndex = 0;
            this.label_listAll.Text = "列表";
            // 
            // attributeBox1
            // 
            this.attributeBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeBox1.Location = new System.Drawing.Point(0, 0);
            this.attributeBox1.Margin = new System.Windows.Forms.Padding(5);
            this.attributeBox1.Name = "attributeBox1";
            this.attributeBox1.Size = new System.Drawing.Size(234, 141);
            this.attributeBox1.TabIndex = 1;
            this.attributeBox1.Tilte = "属性";
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(0, 141);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(234, 28);
            this.progressBarComponent1.TabIndex = 3;
            // 
            // tabControl_leftContent
            // 
            this.tabControl_leftContent.Controls.Add(this.tabPage1);
            this.tabControl_leftContent.Controls.Add(this.tabPage2);
            this.tabControl_leftContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_leftContent.Location = new System.Drawing.Point(0, 0);
            this.tabControl_leftContent.Name = "tabControl_leftContent";
            this.tabControl_leftContent.SelectedIndex = 0;
            this.tabControl_leftContent.Size = new System.Drawing.Size(570, 450);
            this.tabControl_leftContent.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl_currentText);
            this.tabPage1.Controls.Add(this.panel6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(562, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "当前";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl_currentText
            // 
            this.tabControl_currentText.Controls.Add(this.tabPage_currentText);
            this.tabControl_currentText.Controls.Add(this.tabPage_chart);
            this.tabControl_currentText.Controls.Add(this.tabPage_synClosure);
            this.tabControl_currentText.Controls.Add(this.tabPage_repeatError);
            this.tabControl_currentText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_currentText.Location = new System.Drawing.Point(3, 29);
            this.tabControl_currentText.Name = "tabControl_currentText";
            this.tabControl_currentText.SelectedIndex = 0;
            this.tabControl_currentText.Size = new System.Drawing.Size(556, 392);
            this.tabControl_currentText.TabIndex = 2;
            // 
            // tabPage_currentText
            // 
            this.tabPage_currentText.Controls.Add(this.richTextBoxControl_currentLine);
            this.tabPage_currentText.Location = new System.Drawing.Point(4, 22);
            this.tabPage_currentText.Name = "tabPage_currentText";
            this.tabPage_currentText.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_currentText.Size = new System.Drawing.Size(548, 366);
            this.tabPage_currentText.TabIndex = 4;
            this.tabPage_currentText.Text = "当前文本";
            this.tabPage_currentText.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_currentLine
            // 
            this.richTextBoxControl_currentLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_currentLine.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_currentLine.MaxAppendLineCount = 5000;
            this.richTextBoxControl_currentLine.Name = "richTextBoxControl_currentLine";
            this.richTextBoxControl_currentLine.Size = new System.Drawing.Size(542, 360);
            this.richTextBoxControl_currentLine.TabIndex = 2;
            this.richTextBoxControl_currentLine.Text = "";
            // 
            // tabPage_chart
            // 
            this.tabPage_chart.Controls.Add(this.splitContainer_chart);
            this.tabPage_chart.Location = new System.Drawing.Point(4, 22);
            this.tabPage_chart.Name = "tabPage_chart";
            this.tabPage_chart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_chart.Size = new System.Drawing.Size(548, 366);
            this.tabPage_chart.TabIndex = 0;
            this.tabPage_chart.Text = "当前绘图";
            this.tabPage_chart.UseVisualStyleBackColor = true;
            // 
            // splitContainer_chart
            // 
            this.splitContainer_chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_chart.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_chart.Name = "splitContainer_chart";
            this.splitContainer_chart.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_chart.Panel1
            // 
            this.splitContainer_chart.Panel1.Controls.Add(this.commonChartControl_currentResidual);
            // 
            // splitContainer_chart.Panel2
            // 
            this.splitContainer_chart.Panel2.Controls.Add(this.commonChartControl_currentParamConvergence);
            this.splitContainer_chart.Size = new System.Drawing.Size(542, 360);
            this.splitContainer_chart.SplitterDistance = 180;
            this.splitContainer_chart.TabIndex = 1;
            // 
            // commonChartControl_currentResidual
            // 
            this.commonChartControl_currentResidual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commonChartControl_currentResidual.Location = new System.Drawing.Point(0, 0);
            this.commonChartControl_currentResidual.Name = "commonChartControl_currentResidual";
            this.commonChartControl_currentResidual.Points = null;
            this.commonChartControl_currentResidual.Size = new System.Drawing.Size(542, 180);
            this.commonChartControl_currentResidual.TabIndex = 0;
            // 
            // commonChartControl_currentParamConvergence
            // 
            this.commonChartControl_currentParamConvergence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commonChartControl_currentParamConvergence.Location = new System.Drawing.Point(0, 0);
            this.commonChartControl_currentParamConvergence.Name = "commonChartControl_currentParamConvergence";
            this.commonChartControl_currentParamConvergence.Points = null;
            this.commonChartControl_currentParamConvergence.Size = new System.Drawing.Size(542, 176);
            this.commonChartControl_currentParamConvergence.TabIndex = 0;
            // 
            // tabPage_synClosure
            // 
            this.tabPage_synClosure.Controls.Add(this.objectTableControl_currentResidual);
            this.tabPage_synClosure.Location = new System.Drawing.Point(4, 22);
            this.tabPage_synClosure.Name = "tabPage_synClosure";
            this.tabPage_synClosure.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_synClosure.Size = new System.Drawing.Size(548, 366);
            this.tabPage_synClosure.TabIndex = 2;
            this.tabPage_synClosure.Text = "当前残差";
            this.tabPage_synClosure.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_currentResidual
            // 
            this.objectTableControl_currentResidual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_currentResidual.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_currentResidual.Name = "objectTableControl_currentResidual";
            this.objectTableControl_currentResidual.Size = new System.Drawing.Size(542, 360);
            this.objectTableControl_currentResidual.TabIndex = 2;
            this.objectTableControl_currentResidual.TableObjectStorage = null;
            // 
            // tabPage_repeatError
            // 
            this.tabPage_repeatError.Controls.Add(this.objectTableControl_currentRepeatError);
            this.tabPage_repeatError.Location = new System.Drawing.Point(4, 22);
            this.tabPage_repeatError.Name = "tabPage_repeatError";
            this.tabPage_repeatError.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_repeatError.Size = new System.Drawing.Size(548, 366);
            this.tabPage_repeatError.TabIndex = 3;
            this.tabPage_repeatError.Text = "当前参数";
            this.tabPage_repeatError.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_currentRepeatError
            // 
            this.objectTableControl_currentRepeatError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_currentRepeatError.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_currentRepeatError.Name = "objectTableControl_currentRepeatError";
            this.objectTableControl_currentRepeatError.Size = new System.Drawing.Size(542, 360);
            this.objectTableControl_currentRepeatError.TabIndex = 3;
            this.objectTableControl_currentRepeatError.TableObjectStorage = null;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(556, 26);
            this.panel6.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.objectTableControl_positionLineResult);
            this.tabPage2.Controls.Add(this.panel7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(562, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "所有结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_baselineResult
            // 
            this.objectTableControl_positionLineResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_positionLineResult.Location = new System.Drawing.Point(3, 34);
            this.objectTableControl_positionLineResult.Name = "objectTableControl_baselineResult";
            this.objectTableControl_positionLineResult.Size = new System.Drawing.Size(556, 387);
            this.objectTableControl_positionLineResult.TabIndex = 2;
            this.objectTableControl_positionLineResult.TableObjectStorage = null;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.button_saveAllAsGNSSerFile);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(556, 31);
            this.panel7.TabIndex = 3;
            // 
            // button_saveAllAsGNSSerFile
            // 
            this.button_saveAllAsGNSSerFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveAllAsGNSSerFile.Location = new System.Drawing.Point(403, 3);
            this.button_saveAllAsGNSSerFile.Name = "button_saveAllAsGNSSerFile";
            this.button_saveAllAsGNSSerFile.Size = new System.Drawing.Size(148, 23);
            this.button_saveAllAsGNSSerFile.TabIndex = 7;
            this.button_saveAllAsGNSSerFile.Text = "另存为GNSSer坐标文件";
            this.button_saveAllAsGNSSerFile.UseVisualStyleBackColor = true;
            this.button_saveAllAsGNSSerFile.Click += new System.EventHandler(this.button_saveAllAsGNSSerFile_Click);
            // 
            // openFileDialog1_rinexOFile
            // 
            this.openFileDialog1_rinexOFile.Filter = "Rinex 观测文件|*.*o";
            this.openFileDialog1_rinexOFile.Multiselect = true;
            this.openFileDialog1_rinexOFile.Title = "请选择O文件";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // checkBox_replaceApproxCoordWhenPPP
            // 
            this.checkBox_replaceApproxCoordWhenPPP.AutoSize = true;
            this.checkBox_replaceApproxCoordWhenPPP.Location = new System.Drawing.Point(241, 15);
            this.checkBox_replaceApproxCoordWhenPPP.Name = "checkBox_replaceApproxCoordWhenPPP";
            this.checkBox_replaceApproxCoordWhenPPP.Size = new System.Drawing.Size(156, 16);
            this.checkBox_replaceApproxCoordWhenPPP.TabIndex = 60;
            this.checkBox_replaceApproxCoordWhenPPP.Text = "计算后，坐标替换源文件";
            this.checkBox_replaceApproxCoordWhenPPP.UseVisualStyleBackColor = true;
            // 
            // SingleSitePointPositionSolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 548);
            this.Controls.Add(this.splitContainer_main);
            this.Name = "SingleSitePointPositionSolverForm";
            this.Text = "PPP计算新面板";
            this.Load += new System.EventHandler(this.MultiPeriodBaseLineSolverForm_Load);
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage_common.ResumeLayout(false);
            this.tabPage_common.PerformLayout();
            this.tabPage6_baselineSolver.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.splitContainer_content.Panel1.ResumeLayout(false);
            this.splitContainer_content.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_content)).EndInit();
            this.splitContainer_content.ResumeLayout(false);
            this.splitContainer_leftTree.Panel1.ResumeLayout(false);
            this.splitContainer_leftTree.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_leftTree)).EndInit();
            this.splitContainer_leftTree.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage_site.ResumeLayout(false);
            this.contextMenuStrip_site.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabPage_period.ResumeLayout(false);
            this.contextMenuStrip_periods.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage_lineList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_allLines)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl_leftContent.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl_currentText.ResumeLayout(false);
            this.tabPage_currentText.ResumeLayout(false);
            this.tabPage_chart.ResumeLayout(false);
            this.splitContainer_chart.Panel1.ResumeLayout(false);
            this.splitContainer_chart.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_chart)).EndInit();
            this.splitContainer_chart.ResumeLayout(false);
            this.tabPage_synClosure.ResumeLayout(false);
            this.tabPage_repeatError.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.SplitContainer splitContainer_content;
        private System.Windows.Forms.TreeView treeView_sites;
        private System.Windows.Forms.SplitContainer splitContainer_leftTree;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage_site;
        private Geo.Winform.AttributeBox attributeBox1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage_common;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_projName;
        private System.Windows.Forms.CheckBox checkBox_enableNet;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1_projDir;
        private System.Windows.Forms.TabPage tabPage6_baselineSolver;
        private Geo.Winform.Controls.ParallelConfigControl parallelConfigControl1;
        private System.Windows.Forms.TabPage tabPage_period;
        private System.Windows.Forms.TabPage tabPage_lineList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_showSitePeriod;
        private System.Windows.Forms.Button button1_import;
        private System.Windows.Forms.Button button_mapShowSites;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_site;
        private System.Windows.Forms.ToolStripMenuItem 导入文件IToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem 打开所在目录OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开当前文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem 查看编辑所选测站时段EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看编辑所有测站时段AToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 恢复原始文件RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 恢复所有测站原始文件SToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 地图显示测站SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看所有测站时段图PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看卫星高度角HToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.OpenFileDialog openFileDialog1_rinexOFile;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label_siteInfo;
        private System.Windows.Forms.ListBox listBox_vector;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_listAll;
        private System.Windows.Forms.TreeView treeView_periods;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label_periods;
        private System.Windows.Forms.BindingSource bindingSource_allLines;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_periods;
        private System.Windows.Forms.ToolStripMenuItem 展开收缩时段EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 展开收缩测站EToolStripMenuItem;
        private Controls.MultiSolverOptionControl multiSolverOptionControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.Controls.NamedIntControl namedIntControl_removeFirstEpochCount;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.ToolStripMenuItem 移除所选时段RToolStripMenuItem;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button button_solveCurrentLine;
        private System.Windows.Forms.Button button_runAllBaseLine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem pPP计算并更新头文件PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pPP计算并更新所选头文件SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 所有文件PPP并更新头文件MToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem 查看当前测站PPP收敛图VToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开当前测站PPP结果RToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem 查看所有测站PPP收敛图AToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl_leftContent;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl_currentText;
        private System.Windows.Forms.TabPage tabPage_currentText;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_currentLine;
        private System.Windows.Forms.TabPage tabPage_chart;
        private System.Windows.Forms.SplitContainer splitContainer_chart;
        private Geo.Winform.CommonChartControl commonChartControl_currentResidual;
        private Geo.Winform.CommonChartControl commonChartControl_currentParamConvergence;
        private System.Windows.Forms.TabPage tabPage_synClosure;
        private Geo.Winform.ObjectTableControl objectTableControl_currentResidual;
        private System.Windows.Forms.TabPage tabPage_repeatError;
        private Geo.Winform.ObjectTableControl objectTableControl_currentRepeatError;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.ObjectTableControl objectTableControl_positionLineResult;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button button_saveAllAsGNSSerFile;
        private Geo.Winform.EnumRadioControl enumRadioControl_obsType;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_periodSpanMinutes;
        private System.Windows.Forms.CheckBox checkBox_replaceApproxCoordWhenPPP;
    }
}