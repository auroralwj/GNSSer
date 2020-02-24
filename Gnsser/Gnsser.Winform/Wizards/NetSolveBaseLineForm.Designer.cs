namespace Gnsser.Winform
{
    partial class NetSolveBaseLineForm
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
            this.splitContainer_content = new System.Windows.Forms.SplitContainer();
            this.splitContainer_leftSide = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listBox_site = new System.Windows.Forms.ListBox();
            this.contextMenuStrip_site = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导入文件IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.打开所在目录OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开当前文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.查看编辑所选测站时段EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看编辑所有测站时段AToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复原始文件RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复所有测站原始文件SToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.格式化当前测站FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.采用上一设置格式化当前测站TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.格式化所有测站AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.采用上一设置格式所有测站MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pPP计算并更新当前头文件PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.所有文件PPP并更新头文件MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.查看当前测站PPP收敛图VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开当前测站PPP结果RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.查看所有测站PPP收敛图AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.地图显示测站SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看所有测站时段图PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看卫星高度角HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingSource_site = new System.Windows.Forms.BindingSource(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.label_siteInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_showSitePeriod = new System.Windows.Forms.Button();
            this.button1_import = new System.Windows.Forms.Button();
            this.button_mapShowSites = new System.Windows.Forms.Button();
            this.tabPage_netSolve = new System.Windows.Forms.TabPage();
            this.treeView_netLine = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_treeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.全部展开EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部关闭CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.显示所有残差图RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看所有收敛图CToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel8 = new System.Windows.Forms.Panel();
            this.button_netOption = new System.Windows.Forms.Button();
            this.button_solveCurrentNet = new System.Windows.Forms.Button();
            this.button_solveAllNet = new System.Windows.Forms.Button();
            this.attributeBox1 = new Geo.Winform.AttributeBox();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.tabControlContentTables = new System.Windows.Forms.TabControl();
            this.tabPage_currentLine = new System.Windows.Forms.TabPage();
            this.tabControl_currentLineChart = new System.Windows.Forms.TabControl();
            this.tabPage_currentLineResidual = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.commonChartControl_currentResidual = new Geo.Winform.CommonChartControl();
            this.commonChartControl_convergenceA = new Geo.Winform.CommonChartControl();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button_showCurentLine = new System.Windows.Forms.Button();
            this.tabPage_currentBaseLineInfo = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_baselineInfo = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage_currentLineCloure = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.objectTableControl_currentLineErrors = new Geo.Winform.ObjectTableControl();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl_reverseError = new Geo.Winform.ObjectTableControl();
            this.tabPage_lineResult = new System.Windows.Forms.TabPage();
            this.objectTableControl_baselineResult = new Geo.Winform.ObjectTableControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_showAllLineeOnMap = new System.Windows.Forms.Button();
            this.button_saveAllAsLgoAsc = new System.Windows.Forms.Button();
            this.button_saveAllAsGNSSerFile = new System.Windows.Forms.Button();
            this.tabPage_totalClosureError = new System.Windows.Forms.TabPage();
            this.objectTableControl_closureErrors = new Geo.Winform.ObjectTableControl();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button_viewSelectedTriPathes = new System.Windows.Forms.Button();
            this.tabPage_independtLines = new System.Windows.Forms.TabPage();
            this.splitContainer_independentLine = new System.Windows.Forms.SplitContainer();
            this.button_showIndeLineOnMap = new System.Windows.Forms.Button();
            this.button_runIndependentLine = new System.Windows.Forms.Button();
            this.button_saveIndeToLeoAsc = new System.Windows.Forms.Button();
            this.enumRadioControl_selectLineType = new Geo.Winform.EnumRadioControl();
            this.button_saveIndeBaselineFile = new System.Windows.Forms.Button();
            this.objectTableControl_independentLine = new Geo.Winform.ObjectTableControl();
            this.openFileDialog1_rinexOFile = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.namedStringControl_projName = new Geo.Winform.Controls.NamedStringControl();
            this.checkBox_enableNet = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl1_projDir = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage6_baselineSolver = new System.Windows.Forms.TabPage();
            this.enumRadioControl_obsType = new Geo.Winform.EnumRadioControl();
            this.parallelConfigControl1 = new Geo.Winform.Controls.ParallelConfigControl();
            this.backgroundWorker_netSolve = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip_netSite = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.以此站为基准网解NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.查看参数文件PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开残差文件OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.查看残差图SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看收敛图LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.查看所有残差图RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看所有收敛图CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.展开EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.收起CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_content)).BeginInit();
            this.splitContainer_content.Panel1.SuspendLayout();
            this.splitContainer_content.Panel2.SuspendLayout();
            this.splitContainer_content.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_leftSide)).BeginInit();
            this.splitContainer_leftSide.Panel1.SuspendLayout();
            this.splitContainer_leftSide.Panel2.SuspendLayout();
            this.splitContainer_leftSide.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip_site.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_site)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage_netSolve.SuspendLayout();
            this.contextMenuStrip_treeView.SuspendLayout();
            this.panel8.SuspendLayout();
            this.tabControlContentTables.SuspendLayout();
            this.tabPage_currentLine.SuspendLayout();
            this.tabControl_currentLineChart.SuspendLayout();
            this.tabPage_currentLineResidual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabPage_currentBaseLineInfo.SuspendLayout();
            this.tabPage_currentLineCloure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage_lineResult.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage_totalClosureError.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabPage_independtLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_independentLine)).BeginInit();
            this.splitContainer_independentLine.Panel1.SuspendLayout();
            this.splitContainer_independentLine.Panel2.SuspendLayout();
            this.splitContainer_independentLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage6_baselineSolver.SuspendLayout();
            this.contextMenuStrip_netSite.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer_content.Panel1.Controls.Add(this.splitContainer_leftSide);
            // 
            // splitContainer_content.Panel2
            // 
            this.splitContainer_content.Panel2.Controls.Add(this.tabControlContentTables);
            this.splitContainer_content.Size = new System.Drawing.Size(836, 456);
            this.splitContainer_content.SplitterDistance = 252;
            this.splitContainer_content.TabIndex = 0;
            // 
            // splitContainer_leftSide
            // 
            this.splitContainer_leftSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_leftSide.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_leftSide.Name = "splitContainer_leftSide";
            this.splitContainer_leftSide.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_leftSide.Panel1
            // 
            this.splitContainer_leftSide.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer_leftSide.Panel2
            // 
            this.splitContainer_leftSide.Panel2.Controls.Add(this.attributeBox1);
            this.splitContainer_leftSide.Panel2.Controls.Add(this.progressBarComponent1);
            this.splitContainer_leftSide.Size = new System.Drawing.Size(252, 456);
            this.splitContainer_leftSide.SplitterDistance = 301;
            this.splitContainer_leftSide.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage_netSolve);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(252, 301);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBox_site);
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(244, 275);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "测站";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox_site
            // 
            this.listBox_site.ContextMenuStrip = this.contextMenuStrip_site;
            this.listBox_site.DataSource = this.bindingSource_site;
            this.listBox_site.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_site.FormattingEnabled = true;
            this.listBox_site.ItemHeight = 12;
            this.listBox_site.Location = new System.Drawing.Point(3, 33);
            this.listBox_site.Name = "listBox_site";
            this.listBox_site.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_site.Size = new System.Drawing.Size(238, 223);
            this.listBox_site.TabIndex = 1;
            this.listBox_site.SelectedIndexChanged += new System.EventHandler(this.listBox_site_SelectedIndexChanged);
            this.listBox_site.DoubleClick += new System.EventHandler(this.listBox_site_DoubleClick);
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
            this.移除ToolStripMenuItem,
            this.清空ToolStripMenuItem,
            this.toolStripSeparator5,
            this.查看编辑所选测站时段EToolStripMenuItem,
            this.恢复原始文件RToolStripMenuItem,
            this.格式化当前测站FToolStripMenuItem,
            this.pPP计算并更新当前头文件PToolStripMenuItem,
            this.toolStripSeparator2,
            this.地图显示测站SToolStripMenuItem,
            this.查看所有测站时段图PToolStripMenuItem,
            this.查看卫星高度角HToolStripMenuItem,
            this.toolStripSeparator6});
            this.contextMenuStrip_site.Name = "contextMenuStrip1";
            this.contextMenuStrip_site.Size = new System.Drawing.Size(233, 298);
            // 
            // 导入文件IToolStripMenuItem
            // 
            this.导入文件IToolStripMenuItem.Name = "导入文件IToolStripMenuItem";
            this.导入文件IToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.导入文件IToolStripMenuItem.Text = "导入文件(&I)";
            this.导入文件IToolStripMenuItem.Click += new System.EventHandler(this.导入文件IToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(229, 6);
            // 
            // 打开所在目录OToolStripMenuItem
            // 
            this.打开所在目录OToolStripMenuItem.Name = "打开所在目录OToolStripMenuItem";
            this.打开所在目录OToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.打开所在目录OToolStripMenuItem.Text = "打开所在目录(&O)";
            this.打开所在目录OToolStripMenuItem.Click += new System.EventHandler(this.打开所在目录OToolStripMenuItem_Click);
            // 
            // 打开当前文件FToolStripMenuItem
            // 
            this.打开当前文件FToolStripMenuItem.Name = "打开当前文件FToolStripMenuItem";
            this.打开当前文件FToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.打开当前文件FToolStripMenuItem.Text = "打开当前文件(&F)";
            this.打开当前文件FToolStripMenuItem.Click += new System.EventHandler(this.打开当前文件FToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(229, 6);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.移除ToolStripMenuItem.Text = "移除所选测站(&R)";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.清空ToolStripMenuItem.Text = "清空(&C)";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.清空ToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(229, 6);
            // 
            // 查看编辑所选测站时段EToolStripMenuItem
            // 
            this.查看编辑所选测站时段EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看编辑所有测站时段AToolStripMenuItem1});
            this.查看编辑所选测站时段EToolStripMenuItem.Name = "查看编辑所选测站时段EToolStripMenuItem";
            this.查看编辑所选测站时段EToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.查看编辑所选测站时段EToolStripMenuItem.Text = "查看/编辑 当前测站时段(&E)";
            this.查看编辑所选测站时段EToolStripMenuItem.Click += new System.EventHandler(this.查看编辑所选测站时段EToolStripMenuItem_Click);
            // 
            // 查看编辑所有测站时段AToolStripMenuItem1
            // 
            this.查看编辑所有测站时段AToolStripMenuItem1.Name = "查看编辑所有测站时段AToolStripMenuItem1";
            this.查看编辑所有测站时段AToolStripMenuItem1.Size = new System.Drawing.Size(221, 22);
            this.查看编辑所有测站时段AToolStripMenuItem1.Text = "查看/编辑 所有测站时段(&A)";
            this.查看编辑所有测站时段AToolStripMenuItem1.Click += new System.EventHandler(this.查看编辑所有测站时段AToolStripMenuItem_Click);
            // 
            // 恢复原始文件RToolStripMenuItem
            // 
            this.恢复原始文件RToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.恢复所有测站原始文件SToolStripMenuItem1});
            this.恢复原始文件RToolStripMenuItem.Name = "恢复原始文件RToolStripMenuItem";
            this.恢复原始文件RToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.恢复原始文件RToolStripMenuItem.Text = "恢复当前测站原始文件(&R)";
            this.恢复原始文件RToolStripMenuItem.Click += new System.EventHandler(this.恢复原始文件RToolStripMenuItem_Click);
            // 
            // 恢复所有测站原始文件SToolStripMenuItem1
            // 
            this.恢复所有测站原始文件SToolStripMenuItem1.Name = "恢复所有测站原始文件SToolStripMenuItem1";
            this.恢复所有测站原始文件SToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.恢复所有测站原始文件SToolStripMenuItem1.Text = "恢复所有测站原始文件(&S)";
            this.恢复所有测站原始文件SToolStripMenuItem1.Click += new System.EventHandler(this.恢复所有测站原始文件SToolStripMenuItem_Click);
            // 
            // 格式化当前测站FToolStripMenuItem
            // 
            this.格式化当前测站FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.采用上一设置格式化当前测站TToolStripMenuItem,
            this.toolStripSeparator12,
            this.格式化所有测站AToolStripMenuItem,
            this.采用上一设置格式所有测站MToolStripMenuItem});
            this.格式化当前测站FToolStripMenuItem.Name = "格式化当前测站FToolStripMenuItem";
            this.格式化当前测站FToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.格式化当前测站FToolStripMenuItem.Text = "格式化 当前测站(&F)";
            this.格式化当前测站FToolStripMenuItem.Click += new System.EventHandler(this.格式化当前测站FToolStripMenuItem_Click);
            // 
            // 采用上一设置格式化当前测站TToolStripMenuItem
            // 
            this.采用上一设置格式化当前测站TToolStripMenuItem.Name = "采用上一设置格式化当前测站TToolStripMenuItem";
            this.采用上一设置格式化当前测站TToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.采用上一设置格式化当前测站TToolStripMenuItem.Text = "采用上一设置格式化当前测站(&T)";
            this.采用上一设置格式化当前测站TToolStripMenuItem.Click += new System.EventHandler(this.采用上一设置格式化当前测站TToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(244, 6);
            // 
            // 格式化所有测站AToolStripMenuItem
            // 
            this.格式化所有测站AToolStripMenuItem.Name = "格式化所有测站AToolStripMenuItem";
            this.格式化所有测站AToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.格式化所有测站AToolStripMenuItem.Text = "格式化所有测站(&A)";
            this.格式化所有测站AToolStripMenuItem.Click += new System.EventHandler(this.格式化所有测站AToolStripMenuItem_Click);
            // 
            // 采用上一设置格式所有测站MToolStripMenuItem
            // 
            this.采用上一设置格式所有测站MToolStripMenuItem.Name = "采用上一设置格式所有测站MToolStripMenuItem";
            this.采用上一设置格式所有测站MToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.采用上一设置格式所有测站MToolStripMenuItem.Text = "采用上一设置格式所有测站(&M)";
            this.采用上一设置格式所有测站MToolStripMenuItem.Click += new System.EventHandler(this.采用上一设置格式所有测站MToolStripMenuItem_Click);
            // 
            // pPP计算并更新当前头文件PToolStripMenuItem
            // 
            this.pPP计算并更新当前头文件PToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.所有文件PPP并更新头文件MToolStripMenuItem,
            this.toolStripSeparator14,
            this.查看当前测站PPP收敛图VToolStripMenuItem,
            this.打开当前测站PPP结果RToolStripMenuItem,
            this.toolStripSeparator15,
            this.查看所有测站PPP收敛图AToolStripMenuItem});
            this.pPP计算并更新当前头文件PToolStripMenuItem.Name = "pPP计算并更新当前头文件PToolStripMenuItem";
            this.pPP计算并更新当前头文件PToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.pPP计算并更新当前头文件PToolStripMenuItem.Text = "PPP计算并更新当前头文件(&P)";
            this.pPP计算并更新当前头文件PToolStripMenuItem.Click += new System.EventHandler(this.pPP计算并更新当前头文件PToolStripMenuItem_Click);
            // 
            // 所有文件PPP并更新头文件MToolStripMenuItem
            // 
            this.所有文件PPP并更新头文件MToolStripMenuItem.Name = "所有文件PPP并更新头文件MToolStripMenuItem";
            this.所有文件PPP并更新头文件MToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.所有文件PPP并更新头文件MToolStripMenuItem.Text = "所有文件PPP并更新头文件(&M)";
            this.所有文件PPP并更新头文件MToolStripMenuItem.Click += new System.EventHandler(this.所有文件PPP并更新头文件MToolStripMenuItem_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(234, 6);
            // 
            // 查看当前测站PPP收敛图VToolStripMenuItem
            // 
            this.查看当前测站PPP收敛图VToolStripMenuItem.Name = "查看当前测站PPP收敛图VToolStripMenuItem";
            this.查看当前测站PPP收敛图VToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.查看当前测站PPP收敛图VToolStripMenuItem.Text = "查看当前测站PPP收敛图(&V)";
            this.查看当前测站PPP收敛图VToolStripMenuItem.Click += new System.EventHandler(this.查看当前测站PPP收敛图VToolStripMenuItem_Click);
            // 
            // 打开当前测站PPP结果RToolStripMenuItem
            // 
            this.打开当前测站PPP结果RToolStripMenuItem.Name = "打开当前测站PPP结果RToolStripMenuItem";
            this.打开当前测站PPP结果RToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.打开当前测站PPP结果RToolStripMenuItem.Text = "打开当前测站PPP结果(&R)";
            this.打开当前测站PPP结果RToolStripMenuItem.Click += new System.EventHandler(this.打开当前测站PPP结果RToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(234, 6);
            // 
            // 查看所有测站PPP收敛图AToolStripMenuItem
            // 
            this.查看所有测站PPP收敛图AToolStripMenuItem.Name = "查看所有测站PPP收敛图AToolStripMenuItem";
            this.查看所有测站PPP收敛图AToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.查看所有测站PPP收敛图AToolStripMenuItem.Text = "查看所有测站PPP收敛图(&A)";
            this.查看所有测站PPP收敛图AToolStripMenuItem.Click += new System.EventHandler(this.查看所有测站PPP收敛图AToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(229, 6);
            // 
            // 地图显示测站SToolStripMenuItem
            // 
            this.地图显示测站SToolStripMenuItem.Name = "地图显示测站SToolStripMenuItem";
            this.地图显示测站SToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.地图显示测站SToolStripMenuItem.Text = "地图显示测站(&S)";
            this.地图显示测站SToolStripMenuItem.Click += new System.EventHandler(this.地图显示测站SToolStripMenuItem_Click);
            // 
            // 查看所有测站时段图PToolStripMenuItem
            // 
            this.查看所有测站时段图PToolStripMenuItem.Name = "查看所有测站时段图PToolStripMenuItem";
            this.查看所有测站时段图PToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.查看所有测站时段图PToolStripMenuItem.Text = "查看所有测站共同时段图(&P)";
            this.查看所有测站时段图PToolStripMenuItem.Click += new System.EventHandler(this.查看所有测站时段图PToolStripMenuItem_Click);
            // 
            // 查看卫星高度角HToolStripMenuItem
            // 
            this.查看卫星高度角HToolStripMenuItem.Name = "查看卫星高度角HToolStripMenuItem";
            this.查看卫星高度角HToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.查看卫星高度角HToolStripMenuItem.Text = "查看卫星高度角(&H)";
            this.查看卫星高度角HToolStripMenuItem.Click += new System.EventHandler(this.查看卫星高度角HToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(229, 6);
            // 
            // bindingSource_site
            // 
            this.bindingSource_site.CurrentChanged += new System.EventHandler(this.bindingSource_site_CurrentChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label_siteInfo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 256);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(238, 16);
            this.panel4.TabIndex = 3;
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
            this.panel3.Size = new System.Drawing.Size(238, 30);
            this.panel3.TabIndex = 2;
            // 
            // button_showSitePeriod
            // 
            this.button_showSitePeriod.Location = new System.Drawing.Point(146, 3);
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
            this.button_mapShowSites.Location = new System.Drawing.Point(75, 3);
            this.button_mapShowSites.Name = "button_mapShowSites";
            this.button_mapShowSites.Size = new System.Drawing.Size(65, 23);
            this.button_mapShowSites.TabIndex = 9;
            this.button_mapShowSites.Text = "地图显示";
            this.button_mapShowSites.UseVisualStyleBackColor = true;
            this.button_mapShowSites.Click += new System.EventHandler(this.地图显示测站SToolStripMenuItem_Click);
            // 
            // tabPage_netSolve
            // 
            this.tabPage_netSolve.Controls.Add(this.treeView_netLine);
            this.tabPage_netSolve.Controls.Add(this.panel8);
            this.tabPage_netSolve.Location = new System.Drawing.Point(4, 22);
            this.tabPage_netSolve.Name = "tabPage_netSolve";
            this.tabPage_netSolve.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_netSolve.Size = new System.Drawing.Size(244, 275);
            this.tabPage_netSolve.TabIndex = 3;
            this.tabPage_netSolve.Text = "整网解算";
            this.tabPage_netSolve.UseVisualStyleBackColor = true;
            // 
            // treeView_netLine
            // 
            this.treeView_netLine.ContextMenuStrip = this.contextMenuStrip_treeView;
            this.treeView_netLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_netLine.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView_netLine.Location = new System.Drawing.Point(3, 33);
            this.treeView_netLine.Name = "treeView_netLine";
            this.treeView_netLine.Size = new System.Drawing.Size(238, 239);
            this.treeView_netLine.TabIndex = 0;
            this.treeView_netLine.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_netLine_DrawNode);
            this.treeView_netLine.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_netLine_AfterSelect);
            // 
            // contextMenuStrip_treeView
            // 
            this.contextMenuStrip_treeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全部展开EToolStripMenuItem,
            this.全部关闭CToolStripMenuItem,
            this.toolStripSeparator3,
            this.显示所有残差图RToolStripMenuItem,
            this.查看所有收敛图CToolStripMenuItem1});
            this.contextMenuStrip_treeView.Name = "contextMenuStrip_treeView";
            this.contextMenuStrip_treeView.Size = new System.Drawing.Size(177, 98);
            // 
            // 全部展开EToolStripMenuItem
            // 
            this.全部展开EToolStripMenuItem.Name = "全部展开EToolStripMenuItem";
            this.全部展开EToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.全部展开EToolStripMenuItem.Text = "全部展开(&E)";
            this.全部展开EToolStripMenuItem.Click += new System.EventHandler(this.全部展开EToolStripMenuItem_Click);
            // 
            // 全部关闭CToolStripMenuItem
            // 
            this.全部关闭CToolStripMenuItem.Name = "全部关闭CToolStripMenuItem";
            this.全部关闭CToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.全部关闭CToolStripMenuItem.Text = "全部收起(&C)";
            this.全部关闭CToolStripMenuItem.Click += new System.EventHandler(this.全部收起CToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(173, 6);
            // 
            // 显示所有残差图RToolStripMenuItem
            // 
            this.显示所有残差图RToolStripMenuItem.Name = "显示所有残差图RToolStripMenuItem";
            this.显示所有残差图RToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.显示所有残差图RToolStripMenuItem.Text = "显示所有残差图(&R)";
            this.显示所有残差图RToolStripMenuItem.Click += new System.EventHandler(this.查看所有残差图RToolStripMenuItem_Click);
            // 
            // 查看所有收敛图CToolStripMenuItem1
            // 
            this.查看所有收敛图CToolStripMenuItem1.Name = "查看所有收敛图CToolStripMenuItem1";
            this.查看所有收敛图CToolStripMenuItem1.Size = new System.Drawing.Size(176, 22);
            this.查看所有收敛图CToolStripMenuItem1.Text = "查看所有收敛图(&C)";
            this.查看所有收敛图CToolStripMenuItem1.Click += new System.EventHandler(this.查看所有收敛图CToolStripMenuItem_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.button_netOption);
            this.panel8.Controls.Add(this.button_solveCurrentNet);
            this.panel8.Controls.Add(this.button_solveAllNet);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(238, 30);
            this.panel8.TabIndex = 4;
            // 
            // button_netOption
            // 
            this.button_netOption.Location = new System.Drawing.Point(0, 4);
            this.button_netOption.Name = "button_netOption";
            this.button_netOption.Size = new System.Drawing.Size(71, 23);
            this.button_netOption.TabIndex = 4;
            this.button_netOption.Text = "选项设置";
            this.button_netOption.UseVisualStyleBackColor = true;
            this.button_netOption.Click += new System.EventHandler(this.button_netOption_Click);
            // 
            // button_solveCurrentNet
            // 
            this.button_solveCurrentNet.Location = new System.Drawing.Point(77, 4);
            this.button_solveCurrentNet.Name = "button_solveCurrentNet";
            this.button_solveCurrentNet.Size = new System.Drawing.Size(72, 23);
            this.button_solveCurrentNet.TabIndex = 3;
            this.button_solveCurrentNet.Text = "计算当前";
            this.button_solveCurrentNet.UseVisualStyleBackColor = true;
            this.button_solveCurrentNet.Click += new System.EventHandler(this.button_solveCurrentNet_Click);
            // 
            // button_solveAllNet
            // 
            this.button_solveAllNet.Location = new System.Drawing.Point(158, 4);
            this.button_solveAllNet.Name = "button_solveAllNet";
            this.button_solveAllNet.Size = new System.Drawing.Size(67, 23);
            this.button_solveAllNet.TabIndex = 3;
            this.button_solveAllNet.Text = "计算所有";
            this.button_solveAllNet.UseVisualStyleBackColor = true;
            this.button_solveAllNet.Click += new System.EventHandler(this.button_solveAllNet_Click);
            // 
            // attributeBox1
            // 
            this.attributeBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attributeBox1.Location = new System.Drawing.Point(0, 0);
            this.attributeBox1.Margin = new System.Windows.Forms.Padding(5);
            this.attributeBox1.Name = "attributeBox1";
            this.attributeBox1.Size = new System.Drawing.Size(252, 123);
            this.attributeBox1.TabIndex = 0;
            this.attributeBox1.Tilte = "属性";
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarComponent1.IsBackwardProcess = false;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(0, 123);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(252, 28);
            this.progressBarComponent1.TabIndex = 2;
            // 
            // tabControlContentTables
            // 
            this.tabControlContentTables.Controls.Add(this.tabPage_currentLine);
            this.tabControlContentTables.Controls.Add(this.tabPage_lineResult);
            this.tabControlContentTables.Controls.Add(this.tabPage_totalClosureError);
            this.tabControlContentTables.Controls.Add(this.tabPage_independtLines);
            this.tabControlContentTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlContentTables.Location = new System.Drawing.Point(0, 0);
            this.tabControlContentTables.Name = "tabControlContentTables";
            this.tabControlContentTables.SelectedIndex = 0;
            this.tabControlContentTables.Size = new System.Drawing.Size(580, 456);
            this.tabControlContentTables.TabIndex = 0;
            // 
            // tabPage_currentLine
            // 
            this.tabPage_currentLine.Controls.Add(this.tabControl_currentLineChart);
            this.tabPage_currentLine.Location = new System.Drawing.Point(4, 22);
            this.tabPage_currentLine.Name = "tabPage_currentLine";
            this.tabPage_currentLine.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_currentLine.Size = new System.Drawing.Size(572, 430);
            this.tabPage_currentLine.TabIndex = 4;
            this.tabPage_currentLine.Text = "当前基线";
            this.tabPage_currentLine.UseVisualStyleBackColor = true;
            // 
            // tabControl_currentLineChart
            // 
            this.tabControl_currentLineChart.Controls.Add(this.tabPage_currentLineResidual);
            this.tabControl_currentLineChart.Controls.Add(this.tabPage_currentBaseLineInfo);
            this.tabControl_currentLineChart.Controls.Add(this.tabPage_currentLineCloure);
            this.tabControl_currentLineChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_currentLineChart.Location = new System.Drawing.Point(3, 3);
            this.tabControl_currentLineChart.Name = "tabControl_currentLineChart";
            this.tabControl_currentLineChart.SelectedIndex = 0;
            this.tabControl_currentLineChart.Size = new System.Drawing.Size(566, 424);
            this.tabControl_currentLineChart.TabIndex = 2;
            // 
            // tabPage_currentLineResidual
            // 
            this.tabPage_currentLineResidual.Controls.Add(this.splitContainer4);
            this.tabPage_currentLineResidual.Controls.Add(this.panel6);
            this.tabPage_currentLineResidual.Location = new System.Drawing.Point(4, 22);
            this.tabPage_currentLineResidual.Name = "tabPage_currentLineResidual";
            this.tabPage_currentLineResidual.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_currentLineResidual.Size = new System.Drawing.Size(558, 398);
            this.tabPage_currentLineResidual.TabIndex = 0;
            this.tabPage_currentLineResidual.Text = "当前基线绘图";
            this.tabPage_currentLineResidual.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 29);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.commonChartControl_currentResidual);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.commonChartControl_convergenceA);
            this.splitContainer4.Size = new System.Drawing.Size(552, 366);
            this.splitContainer4.SplitterDistance = 179;
            this.splitContainer4.TabIndex = 2;
            // 
            // commonChartControl_currentResidual
            // 
            this.commonChartControl_currentResidual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commonChartControl_currentResidual.Location = new System.Drawing.Point(0, 0);
            this.commonChartControl_currentResidual.Name = "commonChartControl_currentResidual";
            this.commonChartControl_currentResidual.Points = null;
            this.commonChartControl_currentResidual.Size = new System.Drawing.Size(552, 179);
            this.commonChartControl_currentResidual.TabIndex = 1;
            // 
            // commonChartControl_convergenceA
            // 
            this.commonChartControl_convergenceA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commonChartControl_convergenceA.Location = new System.Drawing.Point(0, 0);
            this.commonChartControl_convergenceA.Name = "commonChartControl_convergenceA";
            this.commonChartControl_convergenceA.Points = null;
            this.commonChartControl_convergenceA.Size = new System.Drawing.Size(552, 183);
            this.commonChartControl_convergenceA.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button_showCurentLine);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(552, 26);
            this.panel6.TabIndex = 0;
            // 
            // button_showCurentLine
            // 
            this.button_showCurentLine.Location = new System.Drawing.Point(3, 3);
            this.button_showCurentLine.Name = "button_showCurentLine";
            this.button_showCurentLine.Size = new System.Drawing.Size(119, 23);
            this.button_showCurentLine.TabIndex = 9;
            this.button_showCurentLine.Text = "地图查看当前基线";
            this.button_showCurentLine.UseVisualStyleBackColor = true;
            this.button_showCurentLine.Click += new System.EventHandler(this.button_showCurentLine_Click);
            // 
            // tabPage_currentBaseLineInfo
            // 
            this.tabPage_currentBaseLineInfo.Controls.Add(this.richTextBoxControl_baselineInfo);
            this.tabPage_currentBaseLineInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPage_currentBaseLineInfo.Name = "tabPage_currentBaseLineInfo";
            this.tabPage_currentBaseLineInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_currentBaseLineInfo.Size = new System.Drawing.Size(558, 398);
            this.tabPage_currentBaseLineInfo.TabIndex = 3;
            this.tabPage_currentBaseLineInfo.Text = "数值结果";
            this.tabPage_currentBaseLineInfo.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_baselineInfo
            // 
            this.richTextBoxControl_baselineInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_baselineInfo.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_baselineInfo.MaxAppendLineCount = 5000;
            this.richTextBoxControl_baselineInfo.Name = "richTextBoxControl_baselineInfo";
            this.richTextBoxControl_baselineInfo.Size = new System.Drawing.Size(552, 392);
            this.richTextBoxControl_baselineInfo.TabIndex = 0;
            this.richTextBoxControl_baselineInfo.Text = "";
            // 
            // tabPage_currentLineCloure
            // 
            this.tabPage_currentLineCloure.Controls.Add(this.splitContainer1);
            this.tabPage_currentLineCloure.Location = new System.Drawing.Point(4, 22);
            this.tabPage_currentLineCloure.Name = "tabPage_currentLineCloure";
            this.tabPage_currentLineCloure.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_currentLineCloure.Size = new System.Drawing.Size(558, 398);
            this.tabPage_currentLineCloure.TabIndex = 2;
            this.tabPage_currentLineCloure.Text = "当前基线同步环闭合差";
            this.tabPage_currentLineCloure.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl4);
            this.splitContainer1.Size = new System.Drawing.Size(552, 392);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.TabIndex = 3;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(552, 215);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.objectTableControl_currentLineErrors);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(544, 189);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "三角形闭合差";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_currentLineErrors
            // 
            this.objectTableControl_currentLineErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_currentLineErrors.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_currentLineErrors.Name = "objectTableControl_currentLineErrors";
            this.objectTableControl_currentLineErrors.Size = new System.Drawing.Size(538, 183);
            this.objectTableControl_currentLineErrors.TabIndex = 2;
            this.objectTableControl_currentLineErrors.TableObjectStorage = null;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage4);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(0, 0);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(552, 173);
            this.tabControl4.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl_reverseError);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(544, 147);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "正反向基线较差";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_reverseError
            // 
            this.objectTableControl_reverseError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_reverseError.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl_reverseError.Name = "objectTableControl_reverseError";
            this.objectTableControl_reverseError.Size = new System.Drawing.Size(538, 141);
            this.objectTableControl_reverseError.TabIndex = 3;
            this.objectTableControl_reverseError.TableObjectStorage = null;
            // 
            // tabPage_lineResult
            // 
            this.tabPage_lineResult.Controls.Add(this.objectTableControl_baselineResult);
            this.tabPage_lineResult.Controls.Add(this.panel1);
            this.tabPage_lineResult.Location = new System.Drawing.Point(4, 22);
            this.tabPage_lineResult.Name = "tabPage_lineResult";
            this.tabPage_lineResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_lineResult.Size = new System.Drawing.Size(572, 430);
            this.tabPage_lineResult.TabIndex = 1;
            this.tabPage_lineResult.Text = "所有基线结果";
            this.tabPage_lineResult.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_baselineResult
            // 
            this.objectTableControl_baselineResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_baselineResult.Location = new System.Drawing.Point(3, 34);
            this.objectTableControl_baselineResult.Name = "objectTableControl_baselineResult";
            this.objectTableControl_baselineResult.Size = new System.Drawing.Size(566, 393);
            this.objectTableControl_baselineResult.TabIndex = 0;
            this.objectTableControl_baselineResult.TableObjectStorage = null;
            this.objectTableControl_baselineResult.Load += new System.EventHandler(this.objectTableControl_baselineResult_Load);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_showAllLineeOnMap);
            this.panel1.Controls.Add(this.button_saveAllAsLgoAsc);
            this.panel1.Controls.Add(this.button_saveAllAsGNSSerFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 31);
            this.panel1.TabIndex = 1;
            // 
            // button_showAllLineeOnMap
            // 
            this.button_showAllLineeOnMap.Location = new System.Drawing.Point(0, 3);
            this.button_showAllLineeOnMap.Name = "button_showAllLineeOnMap";
            this.button_showAllLineeOnMap.Size = new System.Drawing.Size(119, 23);
            this.button_showAllLineeOnMap.TabIndex = 8;
            this.button_showAllLineeOnMap.Text = "地图查看所有基线";
            this.button_showAllLineeOnMap.UseVisualStyleBackColor = true;
            this.button_showAllLineeOnMap.Click += new System.EventHandler(this.button_showAllLineeOnMap_Click);
            // 
            // button_saveAllAsLgoAsc
            // 
            this.button_saveAllAsLgoAsc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveAllAsLgoAsc.Location = new System.Drawing.Point(432, 3);
            this.button_saveAllAsLgoAsc.Name = "button_saveAllAsLgoAsc";
            this.button_saveAllAsLgoAsc.Size = new System.Drawing.Size(129, 23);
            this.button_saveAllAsLgoAsc.TabIndex = 6;
            this.button_saveAllAsLgoAsc.Text = "另存为LGO基线文件";
            this.button_saveAllAsLgoAsc.UseVisualStyleBackColor = true;
            this.button_saveAllAsLgoAsc.Click += new System.EventHandler(this.button_saveAllAsLgoAsc_Click);
            // 
            // button_saveAllAsGNSSerFile
            // 
            this.button_saveAllAsGNSSerFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveAllAsGNSSerFile.Location = new System.Drawing.Point(278, 3);
            this.button_saveAllAsGNSSerFile.Name = "button_saveAllAsGNSSerFile";
            this.button_saveAllAsGNSSerFile.Size = new System.Drawing.Size(148, 23);
            this.button_saveAllAsGNSSerFile.TabIndex = 7;
            this.button_saveAllAsGNSSerFile.Text = "另存为GNSSer基线文件";
            this.button_saveAllAsGNSSerFile.UseVisualStyleBackColor = true;
            this.button_saveAllAsGNSSerFile.Click += new System.EventHandler(this.button_saveAllAsGNSSerFile_Click);
            // 
            // tabPage_totalClosureError
            // 
            this.tabPage_totalClosureError.Controls.Add(this.objectTableControl_closureErrors);
            this.tabPage_totalClosureError.Controls.Add(this.panel7);
            this.tabPage_totalClosureError.Location = new System.Drawing.Point(4, 22);
            this.tabPage_totalClosureError.Name = "tabPage_totalClosureError";
            this.tabPage_totalClosureError.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_totalClosureError.Size = new System.Drawing.Size(572, 430);
            this.tabPage_totalClosureError.TabIndex = 2;
            this.tabPage_totalClosureError.Text = "所有同步环闭合差";
            this.tabPage_totalClosureError.UseVisualStyleBackColor = true;
            // 
            // objectTableControl_closureErrors
            // 
            this.objectTableControl_closureErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_closureErrors.Location = new System.Drawing.Point(3, 29);
            this.objectTableControl_closureErrors.Name = "objectTableControl_closureErrors";
            this.objectTableControl_closureErrors.Size = new System.Drawing.Size(566, 398);
            this.objectTableControl_closureErrors.TabIndex = 1;
            this.objectTableControl_closureErrors.TableObjectStorage = null;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.button_viewSelectedTriPathes);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(566, 26);
            this.panel7.TabIndex = 2;
            // 
            // button_viewSelectedTriPathes
            // 
            this.button_viewSelectedTriPathes.Location = new System.Drawing.Point(4, 0);
            this.button_viewSelectedTriPathes.Name = "button_viewSelectedTriPathes";
            this.button_viewSelectedTriPathes.Size = new System.Drawing.Size(153, 23);
            this.button_viewSelectedTriPathes.TabIndex = 9;
            this.button_viewSelectedTriPathes.Text = "地图查看所选闭合路径";
            this.button_viewSelectedTriPathes.UseVisualStyleBackColor = true;
            this.button_viewSelectedTriPathes.Click += new System.EventHandler(this.button_viewSelectedTriPathes_Click);
            // 
            // tabPage_independtLines
            // 
            this.tabPage_independtLines.Controls.Add(this.splitContainer_independentLine);
            this.tabPage_independtLines.Location = new System.Drawing.Point(4, 22);
            this.tabPage_independtLines.Name = "tabPage_independtLines";
            this.tabPage_independtLines.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_independtLines.Size = new System.Drawing.Size(572, 430);
            this.tabPage_independtLines.TabIndex = 3;
            this.tabPage_independtLines.Text = "独立基线选择与输出";
            this.tabPage_independtLines.UseVisualStyleBackColor = true;
            // 
            // splitContainer_independentLine
            // 
            this.splitContainer_independentLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_independentLine.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_independentLine.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_independentLine.Name = "splitContainer_independentLine";
            this.splitContainer_independentLine.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_independentLine.Panel1
            // 
            this.splitContainer_independentLine.Panel1.Controls.Add(this.button_showIndeLineOnMap);
            this.splitContainer_independentLine.Panel1.Controls.Add(this.button_runIndependentLine);
            this.splitContainer_independentLine.Panel1.Controls.Add(this.button_saveIndeToLeoAsc);
            this.splitContainer_independentLine.Panel1.Controls.Add(this.enumRadioControl_selectLineType);
            this.splitContainer_independentLine.Panel1.Controls.Add(this.button_saveIndeBaselineFile);
            // 
            // splitContainer_independentLine.Panel2
            // 
            this.splitContainer_independentLine.Panel2.Controls.Add(this.objectTableControl_independentLine);
            this.splitContainer_independentLine.Size = new System.Drawing.Size(566, 424);
            this.splitContainer_independentLine.SplitterDistance = 87;
            this.splitContainer_independentLine.TabIndex = 7;
            // 
            // button_showIndeLineOnMap
            // 
            this.button_showIndeLineOnMap.Location = new System.Drawing.Point(3, 3);
            this.button_showIndeLineOnMap.Name = "button_showIndeLineOnMap";
            this.button_showIndeLineOnMap.Size = new System.Drawing.Size(119, 23);
            this.button_showIndeLineOnMap.TabIndex = 9;
            this.button_showIndeLineOnMap.Text = "地图查看独立基线";
            this.button_showIndeLineOnMap.UseVisualStyleBackColor = true;
            this.button_showIndeLineOnMap.Click += new System.EventHandler(this.button_showIndeLineOnMap_Click);
            // 
            // button_runIndependentLine
            // 
            this.button_runIndependentLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_runIndependentLine.Location = new System.Drawing.Point(441, 45);
            this.button_runIndependentLine.Name = "button_runIndependentLine";
            this.button_runIndependentLine.Size = new System.Drawing.Size(113, 32);
            this.button_runIndependentLine.TabIndex = 7;
            this.button_runIndependentLine.Text = "选择独立基线";
            this.button_runIndependentLine.UseVisualStyleBackColor = true;
            this.button_runIndependentLine.Click += new System.EventHandler(this.button_runIndependentLine_Click);
            // 
            // button_saveIndeToLeoAsc
            // 
            this.button_saveIndeToLeoAsc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveIndeToLeoAsc.Location = new System.Drawing.Point(380, 3);
            this.button_saveIndeToLeoAsc.Name = "button_saveIndeToLeoAsc";
            this.button_saveIndeToLeoAsc.Size = new System.Drawing.Size(174, 23);
            this.button_saveIndeToLeoAsc.TabIndex = 5;
            this.button_saveIndeToLeoAsc.Text = "独立基线另存为LGO基线文件";
            this.button_saveIndeToLeoAsc.UseVisualStyleBackColor = true;
            this.button_saveIndeToLeoAsc.Click += new System.EventHandler(this.button_saveIndeToLeoAsc_Click);
            // 
            // enumRadioControl_selectLineType
            // 
            this.enumRadioControl_selectLineType.IsReady = false;
            this.enumRadioControl_selectLineType.Location = new System.Drawing.Point(3, 32);
            this.enumRadioControl_selectLineType.Name = "enumRadioControl_selectLineType";
            this.enumRadioControl_selectLineType.Size = new System.Drawing.Size(411, 45);
            this.enumRadioControl_selectLineType.TabIndex = 6;
            this.enumRadioControl_selectLineType.Title = "独立基线选择算法";
            // 
            // button_saveIndeBaselineFile
            // 
            this.button_saveIndeBaselineFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveIndeBaselineFile.Location = new System.Drawing.Point(190, 3);
            this.button_saveIndeBaselineFile.Name = "button_saveIndeBaselineFile";
            this.button_saveIndeBaselineFile.Size = new System.Drawing.Size(184, 23);
            this.button_saveIndeBaselineFile.TabIndex = 5;
            this.button_saveIndeBaselineFile.Text = "独立基线另存为GNSSer基线文件";
            this.button_saveIndeBaselineFile.UseVisualStyleBackColor = true;
            this.button_saveIndeBaselineFile.Click += new System.EventHandler(this.button_saveIndeBaselineFile_Click);
            // 
            // objectTableControl_independentLine
            // 
            this.objectTableControl_independentLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl_independentLine.Location = new System.Drawing.Point(0, 0);
            this.objectTableControl_independentLine.Name = "objectTableControl_independentLine";
            this.objectTableControl_independentLine.Size = new System.Drawing.Size(566, 333);
            this.objectTableControl_independentLine.TabIndex = 2;
            this.objectTableControl_independentLine.TableObjectStorage = null;
            // 
            // openFileDialog1_rinexOFile
            // 
            this.openFileDialog1_rinexOFile.Filter = "Rinex 观测文件|*.*o";
            this.openFileDialog1_rinexOFile.Multiselect = true;
            this.openFileDialog1_rinexOFile.Title = "请选择O文件";
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
            this.splitContainer_main.Size = new System.Drawing.Size(836, 556);
            this.splitContainer_main.SplitterDistance = 96;
            this.splitContainer_main.TabIndex = 1;
            // 
            // tabControl3
            // 
            this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl3.Controls.Add(this.tabPage1);
            this.tabControl3.Controls.Add(this.tabPage6_baselineSolver);
            this.tabControl3.Location = new System.Drawing.Point(4, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(832, 90);
            this.tabControl3.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.namedStringControl_projName);
            this.tabPage1.Controls.Add(this.checkBox_enableNet);
            this.tabPage1.Controls.Add(this.directorySelectionControl1_projDir);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(824, 64);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "通用设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // namedStringControl_projName
            // 
            this.namedStringControl_projName.Location = new System.Drawing.Point(30, 33);
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
            this.checkBox_enableNet.Location = new System.Drawing.Point(278, 33);
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
            this.directorySelectionControl1_projDir.Size = new System.Drawing.Size(815, 22);
            this.directorySelectionControl1_projDir.TabIndex = 4;
            // 
            // tabPage6_baselineSolver
            // 
            this.tabPage6_baselineSolver.Controls.Add(this.enumRadioControl_obsType);
            this.tabPage6_baselineSolver.Controls.Add(this.parallelConfigControl1);
            this.tabPage6_baselineSolver.Location = new System.Drawing.Point(4, 22);
            this.tabPage6_baselineSolver.Name = "tabPage6_baselineSolver";
            this.tabPage6_baselineSolver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6_baselineSolver.Size = new System.Drawing.Size(824, 64);
            this.tabPage6_baselineSolver.TabIndex = 1;
            this.tabPage6_baselineSolver.Text = "计算算法";
            this.tabPage6_baselineSolver.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl_obsType
            // 
            this.enumRadioControl_obsType.Dock = System.Windows.Forms.DockStyle.Left;
            this.enumRadioControl_obsType.IsReady = false;
            this.enumRadioControl_obsType.Location = new System.Drawing.Point(3, 3);
            this.enumRadioControl_obsType.Margin = new System.Windows.Forms.Padding(4);
            this.enumRadioControl_obsType.Name = "enumRadioControl_obsType";
            this.enumRadioControl_obsType.Size = new System.Drawing.Size(299, 58);
            this.enumRadioControl_obsType.TabIndex = 61;
            this.enumRadioControl_obsType.Title = "观测值选项(双频默认采用无电离层组合)";
            // 
            // parallelConfigControl1
            // 
            this.parallelConfigControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.parallelConfigControl1.EnableParallel = true;
            this.parallelConfigControl1.Location = new System.Drawing.Point(642, 3);
            this.parallelConfigControl1.Margin = new System.Windows.Forms.Padding(2);
            this.parallelConfigControl1.Name = "parallelConfigControl1";
            this.parallelConfigControl1.Size = new System.Drawing.Size(179, 58);
            this.parallelConfigControl1.TabIndex = 62;
            // 
            // backgroundWorker_netSolve
            // 
            this.backgroundWorker_netSolve.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_netSolve_DoWork);
            this.backgroundWorker_netSolve.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_netSolve_RunWorkerCompleted);
            // 
            // contextMenuStrip_netSite
            // 
            this.contextMenuStrip_netSite.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.以此站为基准网解NToolStripMenuItem,
            this.toolStripSeparator4,
            this.查看参数文件PToolStripMenuItem,
            this.打开残差文件OToolStripMenuItem,
            this.toolStripSeparator10,
            this.查看残差图SToolStripMenuItem,
            this.查看收敛图LToolStripMenuItem,
            this.toolStripSeparator8,
            this.查看所有残差图RToolStripMenuItem,
            this.查看所有收敛图CToolStripMenuItem,
            this.toolStripSeparator7,
            this.展开EToolStripMenuItem,
            this.收起CToolStripMenuItem});
            this.contextMenuStrip_netSite.Name = "contextMenuStrip_netSite";
            this.contextMenuStrip_netSite.Size = new System.Drawing.Size(191, 226);
            // 
            // 以此站为基准网解NToolStripMenuItem
            // 
            this.以此站为基准网解NToolStripMenuItem.Name = "以此站为基准网解NToolStripMenuItem";
            this.以此站为基准网解NToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.以此站为基准网解NToolStripMenuItem.Text = "以此站为基准网解(&N)";
            this.以此站为基准网解NToolStripMenuItem.Click += new System.EventHandler(this.button_solveCurrentNet_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(187, 6);
            // 
            // 查看参数文件PToolStripMenuItem
            // 
            this.查看参数文件PToolStripMenuItem.Name = "查看参数文件PToolStripMenuItem";
            this.查看参数文件PToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.查看参数文件PToolStripMenuItem.Text = "查看参数文件(&P)";
            this.查看参数文件PToolStripMenuItem.Click += new System.EventHandler(this.查看参数文件PToolStripMenuItem_Click);
            // 
            // 打开残差文件OToolStripMenuItem
            // 
            this.打开残差文件OToolStripMenuItem.Name = "打开残差文件OToolStripMenuItem";
            this.打开残差文件OToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.打开残差文件OToolStripMenuItem.Text = "查看残差文件(&O)";
            this.打开残差文件OToolStripMenuItem.Click += new System.EventHandler(this.打开残差文件OToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(187, 6);
            // 
            // 查看残差图SToolStripMenuItem
            // 
            this.查看残差图SToolStripMenuItem.Name = "查看残差图SToolStripMenuItem";
            this.查看残差图SToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.查看残差图SToolStripMenuItem.Text = "查看残差图(&S)";
            this.查看残差图SToolStripMenuItem.Click += new System.EventHandler(this.查看残差图SToolStripMenuItem_Click);
            // 
            // 查看收敛图LToolStripMenuItem
            // 
            this.查看收敛图LToolStripMenuItem.Name = "查看收敛图LToolStripMenuItem";
            this.查看收敛图LToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.查看收敛图LToolStripMenuItem.Text = "查看参数收敛图(&L)";
            this.查看收敛图LToolStripMenuItem.Click += new System.EventHandler(this.查看收敛图LToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(187, 6);
            // 
            // 查看所有残差图RToolStripMenuItem
            // 
            this.查看所有残差图RToolStripMenuItem.Name = "查看所有残差图RToolStripMenuItem";
            this.查看所有残差图RToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.查看所有残差图RToolStripMenuItem.Text = "查看所有残差图(&R)";
            this.查看所有残差图RToolStripMenuItem.Click += new System.EventHandler(this.查看所有残差图RToolStripMenuItem_Click);
            // 
            // 查看所有收敛图CToolStripMenuItem
            // 
            this.查看所有收敛图CToolStripMenuItem.Name = "查看所有收敛图CToolStripMenuItem";
            this.查看所有收敛图CToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.查看所有收敛图CToolStripMenuItem.Text = "查看所有收敛图(&C)";
            this.查看所有收敛图CToolStripMenuItem.Click += new System.EventHandler(this.查看所有收敛图CToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(187, 6);
            // 
            // 展开EToolStripMenuItem
            // 
            this.展开EToolStripMenuItem.Name = "展开EToolStripMenuItem";
            this.展开EToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.展开EToolStripMenuItem.Text = "展开(&E)";
            this.展开EToolStripMenuItem.Click += new System.EventHandler(this.展开EToolStripMenuItem_Click);
            // 
            // 收起CToolStripMenuItem
            // 
            this.收起CToolStripMenuItem.Name = "收起CToolStripMenuItem";
            this.收起CToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.收起CToolStripMenuItem.Text = "收起(&C)";
            this.收起CToolStripMenuItem.Click += new System.EventHandler(this.收起CToolStripMenuItem_Click);
            // 
            // NetSolveBaseLineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 556);
            this.Controls.Add(this.splitContainer_main);
            this.Name = "NetSolveBaseLineForm";
            this.Text = "同步网计算器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseLineNetSolverForm_FormClosing);
            this.Load += new System.EventHandler(this.BaseLineNetSolverForm_Load);
            this.splitContainer_content.Panel1.ResumeLayout(false);
            this.splitContainer_content.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_content)).EndInit();
            this.splitContainer_content.ResumeLayout(false);
            this.splitContainer_leftSide.Panel1.ResumeLayout(false);
            this.splitContainer_leftSide.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_leftSide)).EndInit();
            this.splitContainer_leftSide.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.contextMenuStrip_site.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_site)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabPage_netSolve.ResumeLayout(false);
            this.contextMenuStrip_treeView.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.tabControlContentTables.ResumeLayout(false);
            this.tabPage_currentLine.ResumeLayout(false);
            this.tabControl_currentLineChart.ResumeLayout(false);
            this.tabPage_currentLineResidual.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.tabPage_currentBaseLineInfo.ResumeLayout(false);
            this.tabPage_currentLineCloure.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage_lineResult.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabPage_totalClosureError.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tabPage_independtLines.ResumeLayout(false);
            this.splitContainer_independentLine.Panel1.ResumeLayout(false);
            this.splitContainer_independentLine.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_independentLine)).EndInit();
            this.splitContainer_independentLine.ResumeLayout(false);
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage6_baselineSolver.ResumeLayout(false);
            this.contextMenuStrip_netSite.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_content;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_site;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1_rinexOFile;
        private System.Windows.Forms.ToolStripMenuItem 导入文件IToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListBox listBox_site;
        private System.Windows.Forms.BindingSource bindingSource_site;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.TabControl tabControlContentTables;
        private System.Windows.Forms.TabPage tabPage_lineResult;
        private System.Windows.Forms.TabPage tabPage_totalClosureError;
        private Geo.Winform.ObjectTableControl objectTableControl_baselineResult;
        private System.Windows.Forms.SplitContainer splitContainer_leftSide;
        private Geo.Winform.AttributeBox attributeBox1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.ObjectTableControl objectTableControl_closureErrors;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem 查看编辑所选测站时段EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开所在目录OToolStripMenuItem;
        private System.Windows.Forms.Button button_saveIndeToLeoAsc;
        private System.Windows.Forms.Button button_saveIndeBaselineFile;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage_independtLines;
        private Geo.Winform.EnumRadioControl enumRadioControl_selectLineType;
        private System.Windows.Forms.SplitContainer splitContainer_independentLine;
        private Geo.Winform.ObjectTableControl objectTableControl_independentLine;
        private System.Windows.Forms.Button button_runIndependentLine;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_saveAllAsLgoAsc;
        private System.Windows.Forms.Button button_saveAllAsGNSSerFile;
        private System.Windows.Forms.Button button_showAllLineeOnMap;
        private System.Windows.Forms.Button button_showIndeLineOnMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 地图显示测站SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看所有测站时段图PToolStripMenuItem;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1_projDir;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_mapShowSites;
        private System.Windows.Forms.Button button_showSitePeriod;
        private System.Windows.Forms.Button button1_import;
        private System.Windows.Forms.ToolStripMenuItem 查看卫星高度角HToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage6_baselineSolver;
        private System.Windows.Forms.CheckBox checkBox_enableNet;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_projName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem 打开当前文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 恢复原始文件RToolStripMenuItem;
        private Geo.Winform.Controls.ParallelConfigControl parallelConfigControl1;
        private System.Windows.Forms.ToolStripMenuItem 查看编辑所有测站时段AToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 恢复所有测站原始文件SToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 格式化当前测站FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 格式化所有测站AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 采用上一设置格式所有测站MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 采用上一设置格式化当前测站TToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private Geo.Winform.EnumRadioControl enumRadioControl_obsType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem pPP计算并更新当前头文件PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 所有文件PPP并更新头文件MToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem 查看当前测站PPP收敛图VToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开当前测站PPP结果RToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem 查看所有测站PPP收敛图AToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage_currentLine;
        private Geo.Winform.ObjectTableControl objectTableControl_currentLineErrors;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label_siteInfo;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button button_showCurentLine;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button button_viewSelectedTriPathes;
        private System.Windows.Forms.TabControl tabControl_currentLineChart;
        private System.Windows.Forms.TabPage tabPage_currentLineResidual;
        private System.Windows.Forms.TabPage tabPage_currentLineCloure;
        private System.Windows.Forms.TabPage tabPage_netSolve;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private Geo.Winform.CommonChartControl commonChartControl_currentResidual;
        private System.Windows.Forms.TabPage tabPage_currentBaseLineInfo;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_baselineInfo;
        private System.Windows.Forms.TreeView treeView_netLine;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button button_solveCurrentNet;
        private System.Windows.Forms.Button button_solveAllNet;
        private System.ComponentModel.BackgroundWorker backgroundWorker_netSolve;
        private System.Windows.Forms.Button button_netOption;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_netSite;
        private System.Windows.Forms.ToolStripMenuItem 以此站为基准网解NToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看所有残差图RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看所有收敛图CToolStripMenuItem;
        private Geo.Winform.CommonChartControl commonChartControl_convergenceA;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_treeView;
        private System.Windows.Forms.ToolStripMenuItem 全部展开EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部关闭CToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 显示所有残差图RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看所有收敛图CToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem 查看残差图SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看收敛图LToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem 展开EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 收起CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看参数文件PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开残差文件OToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tabPage4;
        private Geo.Winform.ObjectTableControl objectTableControl_reverseError;
    }
}