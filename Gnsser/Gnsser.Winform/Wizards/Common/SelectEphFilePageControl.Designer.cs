namespace Gnsser.Winform
{
    partial class SelectEphFilePageControl
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
            this.checkBox_isMultiFile = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.fileOpenControl_o.LabelName = "文件：";
            this.fileOpenControl_o.Location = new System.Drawing.Point(30, 59);
            this.fileOpenControl_o.Name = "fileOpenControl_o";
            this.fileOpenControl_o.Size = new System.Drawing.Size(482, 22);
            this.fileOpenControl_o.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_isMultiFile);
            this.groupBox1.Controls.Add(this.fileOpenControl_o);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 175);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件输入";
            // 
            // checkBox_isMultiFile
            // 
            this.checkBox_isMultiFile.AutoSize = true;
            this.checkBox_isMultiFile.Location = new System.Drawing.Point(429, 22);
            this.checkBox_isMultiFile.Name = "checkBox_isMultiFile";
            this.checkBox_isMultiFile.Size = new System.Drawing.Size(60, 16);
            this.checkBox_isMultiFile.TabIndex = 1;
            this.checkBox_isMultiFile.Text = "多文件";
            this.checkBox_isMultiFile.UseVisualStyleBackColor = true;
            this.checkBox_isMultiFile.CheckedChanged += new System.EventHandler(this.checkBox_isMultiFile_CheckedChanged);
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(19, 33);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(488, 22);
            this.directorySelectionControl1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.directorySelectionControl1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 175);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(532, 128);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "输出目录";
            // 
            // SelectFilePageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "SelectFilePageControl";
            this.Size = new System.Drawing.Size(532, 303);
            this.Load += new System.EventHandler(this.SelectFilePageControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl_o;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_isMultiFile;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
