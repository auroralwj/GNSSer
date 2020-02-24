namespace Gnsser.Winform
{
    partial class ObsFileEditorForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_satEle = new System.Windows.Forms.Button();
            this.button_satPolar = new System.Windows.Forms.Button();
            this.button_showMap = new System.Windows.Forms.Button();
            this.checkBox_show1Only = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_sortPrn = new System.Windows.Forms.CheckBox();
            this.buttonViewOnChart = new System.Windows.Forms.Button();
            this.checkBox1ViewAllPhase = new System.Windows.Forms.CheckBox();
            this.button_saveTo = new System.Windows.Forms.Button();
            this.button_read = new System.Windows.Forms.Button();
            this.fileOpenControl_ofilePath = new Geo.Winform.Controls.FileOpenControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip_data = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除行RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.对象表中打开OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.contextMenuStrip_prn = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除此星DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除历元不全的卫星AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip_data.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip_prn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.button_satEle);
            this.splitContainer1.Panel1.Controls.Add(this.button_satPolar);
            this.splitContainer1.Panel1.Controls.Add(this.button_showMap);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox_show1Only);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.button_saveTo);
            this.splitContainer1.Panel1.Controls.Add(this.button_read);
            this.splitContainer1.Panel1.Controls.Add(this.fileOpenControl_ofilePath);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(906, 476);
            this.splitContainer1.SplitterDistance = 112;
            this.splitContainer1.TabIndex = 0;
            // 
            // button_satEle
            // 
            this.button_satEle.Location = new System.Drawing.Point(590, 62);
            this.button_satEle.Name = "button_satEle";
            this.button_satEle.Size = new System.Drawing.Size(75, 23);
            this.button_satEle.TabIndex = 32;
            this.button_satEle.Text = "卫星高度角";
            this.button_satEle.UseVisualStyleBackColor = true;
            this.button_satEle.Click += new System.EventHandler(this.button_satEle_Click);
            // 
            // button_satPolar
            // 
            this.button_satPolar.Location = new System.Drawing.Point(496, 61);
            this.button_satPolar.Name = "button_satPolar";
            this.button_satPolar.Size = new System.Drawing.Size(87, 23);
            this.button_satPolar.TabIndex = 31;
            this.button_satPolar.Text = "站星位置计算";
            this.button_satPolar.UseVisualStyleBackColor = true;
            this.button_satPolar.Click += new System.EventHandler(this.button_satPolar_Click);
            // 
            // button_showMap
            // 
            this.button_showMap.Location = new System.Drawing.Point(415, 61);
            this.button_showMap.Name = "button_showMap";
            this.button_showMap.Size = new System.Drawing.Size(75, 23);
            this.button_showMap.TabIndex = 30;
            this.button_showMap.Text = "地图查看";
            this.button_showMap.UseVisualStyleBackColor = true;
            this.button_showMap.Click += new System.EventHandler(this.button_showMap_Click);
            // 
            // checkBox_show1Only
            // 
            this.checkBox_show1Only.AutoSize = true;
            this.checkBox_show1Only.Location = new System.Drawing.Point(282, 71);
            this.checkBox_show1Only.Name = "checkBox_show1Only";
            this.checkBox_show1Only.Size = new System.Drawing.Size(108, 16);
            this.checkBox_show1Only.TabIndex = 29;
            this.checkBox_show1Only.Text = "只显示第一频率";
            this.checkBox_show1Only.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox_sortPrn);
            this.groupBox3.Controls.Add(this.buttonViewOnChart);
            this.groupBox3.Controls.Add(this.checkBox1ViewAllPhase);
            this.groupBox3.Location = new System.Drawing.Point(12, 50);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(264, 46);
            this.groupBox3.TabIndex = 25;
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
            // button_saveTo
            // 
            this.button_saveTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_saveTo.Location = new System.Drawing.Point(790, 61);
            this.button_saveTo.Name = "button_saveTo";
            this.button_saveTo.Size = new System.Drawing.Size(75, 23);
            this.button_saveTo.TabIndex = 2;
            this.button_saveTo.Text = "保存";
            this.button_saveTo.UseVisualStyleBackColor = true;
            this.button_saveTo.Click += new System.EventHandler(this.button_saveTo_Click);
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(790, 21);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // fileOpenControl_ofilePath
            // 
            this.fileOpenControl_ofilePath.AllowDrop = true;
            this.fileOpenControl_ofilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_ofilePath.FilePath = "";
            this.fileOpenControl_ofilePath.FilePathes = new string[0];
            this.fileOpenControl_ofilePath.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_ofilePath.FirstPath = "";
            this.fileOpenControl_ofilePath.IsMultiSelect = false;
            this.fileOpenControl_ofilePath.LabelName = "文件：";
            this.fileOpenControl_ofilePath.Location = new System.Drawing.Point(12, 22);
            this.fileOpenControl_ofilePath.Name = "fileOpenControl_ofilePath";
            this.fileOpenControl_ofilePath.Size = new System.Drawing.Size(772, 22);
            this.fileOpenControl_ofilePath.TabIndex = 0;
            this.fileOpenControl_ofilePath.FilePathSetted += new System.EventHandler(this.fileOpenControl_ofilePath_FilePathSetted);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(906, 360);
            this.splitContainer2.SplitterDistance = 234;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(234, 360);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(668, 360);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(660, 334);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据内容";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip_data;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(654, 303);
            this.dataGridView1.TabIndex = 0;
            // 
            // contextMenuStrip_data
            // 
            this.contextMenuStrip_data.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除行RToolStripMenuItem,
            this.对象表中打开OToolStripMenuItem});
            this.contextMenuStrip_data.Name = "contextMenuStrip1";
            this.contextMenuStrip_data.Size = new System.Drawing.Size(167, 48);
            // 
            // 删除行RToolStripMenuItem
            // 
            this.删除行RToolStripMenuItem.Name = "删除行RToolStripMenuItem";
            this.删除行RToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.删除行RToolStripMenuItem.Text = "删除行(&R)";
            this.删除行RToolStripMenuItem.Click += new System.EventHandler(this.删除行RToolStripMenuItem_Click);
            // 
            // 对象表中打开OToolStripMenuItem
            // 
            this.对象表中打开OToolStripMenuItem.Name = "对象表中打开OToolStripMenuItem";
            this.对象表中打开OToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.对象表中打开OToolStripMenuItem.Text = "对象表中打开(&O)";
            this.对象表中打开OToolStripMenuItem.Click += new System.EventHandler(this.对象表中打开OToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(654, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel1.Text = "Info";
            // 
            // contextMenuStrip_prn
            // 
            this.contextMenuStrip_prn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除此星DToolStripMenuItem,
            this.删除历元不全的卫星AToolStripMenuItem});
            this.contextMenuStrip_prn.Name = "contextMenuStrip_prn";
            this.contextMenuStrip_prn.Size = new System.Drawing.Size(201, 48);
            // 
            // 删除此星DToolStripMenuItem
            // 
            this.删除此星DToolStripMenuItem.Name = "删除此星DToolStripMenuItem";
            this.删除此星DToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.删除此星DToolStripMenuItem.Text = "删除此星(&D)";
            this.删除此星DToolStripMenuItem.Click += new System.EventHandler(this.删除此星DToolStripMenuItem_Click);
            // 
            // 删除历元不全的卫星AToolStripMenuItem
            // 
            this.删除历元不全的卫星AToolStripMenuItem.Name = "删除历元不全的卫星AToolStripMenuItem";
            this.删除历元不全的卫星AToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.删除历元不全的卫星AToolStripMenuItem.Text = "删除历元不全的卫星(&A)";
            this.删除历元不全的卫星AToolStripMenuItem.Click += new System.EventHandler(this.删除历元不全的卫星AToolStripMenuItem_Click);
            // 
            // ObsFileEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 476);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ObsFileEditorForm";
            this.Text = "ObsFileEditorForm";
            this.Load += new System.EventHandler(this.ObsFileEditorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip_data.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip_prn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button_read;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_ofilePath;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_data;
        private System.Windows.Forms.ToolStripMenuItem 删除行RToolStripMenuItem;
        private System.Windows.Forms.Button button_saveTo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_sortPrn;
        private System.Windows.Forms.Button buttonViewOnChart;
        private System.Windows.Forms.CheckBox checkBox1ViewAllPhase;
        private System.Windows.Forms.CheckBox checkBox_show1Only;
        private System.Windows.Forms.ToolStripMenuItem 对象表中打开OToolStripMenuItem;
        private System.Windows.Forms.Button button_showMap;
        private System.Windows.Forms.Button button_satPolar;
        private System.Windows.Forms.Button button_satEle;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_prn;
        private System.Windows.Forms.ToolStripMenuItem 删除此星DToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripMenuItem 删除历元不全的卫星AToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}