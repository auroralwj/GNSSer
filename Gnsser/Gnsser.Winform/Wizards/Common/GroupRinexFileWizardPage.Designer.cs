namespace Gnsser.Winform
{
    partial class GroupRinexFileWizardPage
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.namedFloatControl_periodSpanMinutes = new Geo.Winform.Controls.NamedFloatControl();
            this.fileOpenControl1_fiels = new Geo.Winform.Controls.FileOpenControl();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.namedStringControl_subDirectory = new Geo.Winform.Controls.NamedStringControl();
            this.namedStringControl_netName = new Geo.Winform.Controls.NamedStringControl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.namedFloatControl_periodSpanMinutes);
            this.groupBox1.Controls.Add(this.fileOpenControl1_fiels);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 203);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件输入";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "用于区别不同时段观测网";
            // 
            // namedFloatControl_periodSpanMinutes
            // 
            this.namedFloatControl_periodSpanMinutes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.namedFloatControl_periodSpanMinutes.Location = new System.Drawing.Point(6, 171);
            this.namedFloatControl_periodSpanMinutes.Name = "namedFloatControl_periodSpanMinutes";
            this.namedFloatControl_periodSpanMinutes.Size = new System.Drawing.Size(196, 23);
            this.namedFloatControl_periodSpanMinutes.TabIndex = 10;
            this.namedFloatControl_periodSpanMinutes.Title = "最小公共时段(分)：";
            this.namedFloatControl_periodSpanMinutes.Value = 20D;
            // 
            // fileOpenControl1_fiels
            // 
            this.fileOpenControl1_fiels.AllowDrop = true;
            this.fileOpenControl1_fiels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1_fiels.FilePath = "";
            this.fileOpenControl1_fiels.FilePathes = new string[0];
            this.fileOpenControl1_fiels.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1_fiels.FirstPath = "";
            this.fileOpenControl1_fiels.IsMultiSelect = true;
            this.fileOpenControl1_fiels.LabelName = "O文件：";
            this.fileOpenControl1_fiels.Location = new System.Drawing.Point(3, 20);
            this.fileOpenControl1_fiels.Name = "fileOpenControl1_fiels";
            this.fileOpenControl1_fiels.Size = new System.Drawing.Size(526, 143);
            this.fileOpenControl1_fiels.TabIndex = 9;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出(工程)主目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(16, 9);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(513, 22);
            this.directorySelectionControl1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.namedStringControl_netName);
            this.splitContainer1.Panel2.Controls.Add(this.namedStringControl_subDirectory);
            this.splitContainer1.Panel2.Controls.Add(this.directorySelectionControl1);
            this.splitContainer1.Size = new System.Drawing.Size(532, 303);
            this.splitContainer1.SplitterDistance = 203;
            this.splitContainer1.TabIndex = 2;
            // 
            // namedStringControl_subDirectory
            // 
            this.namedStringControl_subDirectory.Location = new System.Drawing.Point(16, 62);
            this.namedStringControl_subDirectory.Name = "namedStringControl_subDirectory";
            this.namedStringControl_subDirectory.Size = new System.Drawing.Size(198, 23);
            this.namedStringControl_subDirectory.TabIndex = 12;
            this.namedStringControl_subDirectory.Title = "子(原始数据)目录：";
            // 
            // namedStringControl_netName
            // 
            this.namedStringControl_netName.Location = new System.Drawing.Point(16, 35);
            this.namedStringControl_netName.Name = "namedStringControl_netName";
            this.namedStringControl_netName.Size = new System.Drawing.Size(198, 23);
            this.namedStringControl_netName.TabIndex = 12;
            this.namedStringControl_netName.Title = "时段网(工程)前缀：";
            // 
            // GroupRinexFileWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GroupRinexFileWizardPage";
            this.Size = new System.Drawing.Size(532, 303);
            this.Load += new System.EventHandler(this.SelectFilePageControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_subDirectory;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_periodSpanMinutes;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl1_fiels;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_netName;
    }
}
