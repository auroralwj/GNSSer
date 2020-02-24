namespace Gnsser.Winform
{
    partial class SelectRinexFileWizardPage
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
            this.fileOpenControl_o = new Geo.Winform.Controls.FileOpenControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_indicateEph = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_n = new Geo.Winform.Controls.FileOpenControl();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.checkBox_isMultiFile = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileOpenControl_o
            // 
            this.fileOpenControl_o.AllowDrop = true;
            this.fileOpenControl_o.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_o.FilePath = "";
            this.fileOpenControl_o.FilePathes = new string[0];
            this.fileOpenControl_o.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_o.FirstPath = "";
            this.fileOpenControl_o.IsMultiSelect = false;
            this.fileOpenControl_o.LabelName = "观测文件：";
            this.fileOpenControl_o.Location = new System.Drawing.Point(6, 32);
            this.fileOpenControl_o.Name = "fileOpenControl_o";
            this.fileOpenControl_o.Size = new System.Drawing.Size(506, 22);
            this.fileOpenControl_o.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_indicateEph);
            this.groupBox1.Controls.Add(this.fileOpenControl_n);
            this.groupBox1.Controls.Add(this.checkBox_isMultiFile);
            this.groupBox1.Controls.Add(this.fileOpenControl_o);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 203);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件输入";
            // 
            // checkBox_indicateEph
            // 
            this.checkBox_indicateEph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_indicateEph.AutoSize = true;
            this.checkBox_indicateEph.Location = new System.Drawing.Point(430, 139);
            this.checkBox_indicateEph.Name = "checkBox_indicateEph";
            this.checkBox_indicateEph.Size = new System.Drawing.Size(96, 16);
            this.checkBox_indicateEph.TabIndex = 4;
            this.checkBox_indicateEph.Text = "指定星历文件";
            this.checkBox_indicateEph.UseVisualStyleBackColor = true;
            this.checkBox_indicateEph.CheckedChanged += new System.EventHandler(this.checkBox_indicateEph_CheckedChanged);
            // 
            // fileOpenControl_n
            // 
            this.fileOpenControl_n.AllowDrop = true;
            this.fileOpenControl_n.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_n.FilePath = "";
            this.fileOpenControl_n.FilePathes = new string[0];
            this.fileOpenControl_n.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_n.FirstPath = "";
            this.fileOpenControl_n.IsMultiSelect = false;
            this.fileOpenControl_n.LabelName = "星历文件：";
            this.fileOpenControl_n.Location = new System.Drawing.Point(20, 161);
            this.fileOpenControl_n.Name = "fileOpenControl_n";
            this.fileOpenControl_n.Size = new System.Drawing.Size(492, 22);
            this.fileOpenControl_n.TabIndex = 3;
            this.fileOpenControl_n.Load += new System.EventHandler(this.fileOpenControl_n_Load);
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(3, 3);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(512, 22);
            this.directorySelectionControl1.TabIndex = 2;
            // 
            // checkBox_isMultiFile
            // 
            this.checkBox_isMultiFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_isMultiFile.AutoSize = true;
            this.checkBox_isMultiFile.Location = new System.Drawing.Point(430, 10);
            this.checkBox_isMultiFile.Name = "checkBox_isMultiFile";
            this.checkBox_isMultiFile.Size = new System.Drawing.Size(84, 16);
            this.checkBox_isMultiFile.TabIndex = 1;
            this.checkBox_isMultiFile.Text = "多观测文件";
            this.checkBox_isMultiFile.UseVisualStyleBackColor = true;
            this.checkBox_isMultiFile.CheckedChanged += new System.EventHandler(this.checkBox_isMultiFile_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer1.Panel2.Controls.Add(this.directorySelectionControl1);
            this.splitContainer1.Size = new System.Drawing.Size(532, 303);
            this.splitContainer1.SplitterDistance = 203;
            this.splitContainer1.TabIndex = 2;
            // 
            // SelectRinexFileWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SelectRinexFileWizardPage";
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

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_o;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_isMultiFile;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_n;
        private System.Windows.Forms.CheckBox checkBox_indicateEph;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
