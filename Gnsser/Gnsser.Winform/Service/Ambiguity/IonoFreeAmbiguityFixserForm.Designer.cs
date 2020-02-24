namespace Gnsser.Winform
{
    partial class IonoFreeAmbiguityFixserForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.fileOpenControl_mwWide = new Geo.Winform.Controls.FileOpenControl();
            this.fileOpenControl_ionoFreeFloat = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.namedFloatControl_maxIntFloatDiffer = new Geo.Winform.Controls.NamedFloatControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 157;
            this.splitContainer1.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button_run);
            this.splitContainer2.Size = new System.Drawing.Size(800, 157);
            this.splitContainer2.SplitterDistance = 113;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(765, 100);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fileOpenControl_mwWide);
            this.tabPage1.Controls.Add(this.fileOpenControl_ionoFreeFloat);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(757, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_mwWide
            // 
            this.fileOpenControl_mwWide.AllowDrop = true;
            this.fileOpenControl_mwWide.FilePath = "";
            this.fileOpenControl_mwWide.FilePathes = new string[0];
            this.fileOpenControl_mwWide.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_mwWide.FirstPath = "";
            this.fileOpenControl_mwWide.IsMultiSelect = false;
            this.fileOpenControl_mwWide.LabelName = "MW宽项：";
            this.fileOpenControl_mwWide.Location = new System.Drawing.Point(18, 44);
            this.fileOpenControl_mwWide.Name = "fileOpenControl_mwWide";
            this.fileOpenControl_mwWide.Size = new System.Drawing.Size(667, 22);
            this.fileOpenControl_mwWide.TabIndex = 1;
            // 
            // fileOpenControl_ionoFreeFloat
            // 
            this.fileOpenControl_ionoFreeFloat.AllowDrop = true;
            this.fileOpenControl_ionoFreeFloat.FilePath = "";
            this.fileOpenControl_ionoFreeFloat.FilePathes = new string[0];
            this.fileOpenControl_ionoFreeFloat.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_ionoFreeFloat.FirstPath = "";
            this.fileOpenControl_ionoFreeFloat.IsMultiSelect = false;
            this.fileOpenControl_ionoFreeFloat.LabelName = "无电离层组合模糊度浮点解：";
            this.fileOpenControl_ionoFreeFloat.Location = new System.Drawing.Point(18, 15);
            this.fileOpenControl_ionoFreeFloat.Name = "fileOpenControl_ionoFreeFloat";
            this.fileOpenControl_ionoFreeFloat.Size = new System.Drawing.Size(667, 22);
            this.fileOpenControl_ionoFreeFloat.TabIndex = 0;
            this.fileOpenControl_ionoFreeFloat.FilePathSetted += new System.EventHandler(this.fileOpenControl_ionoFreeFloat_FilePathSetted);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.directorySelectionControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(757, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "输出选项";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(28, 6);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(488, 22);
            this.directorySelectionControl1.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Location = new System.Drawing.Point(713, 3);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 23);
            this.button_run.TabIndex = 0;
            this.button_run.Text = "计算";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(3, 12);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(794, 274);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(786, 248);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "结果";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(786, 248);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "表格";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(780, 242);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.namedFloatControl_maxIntFloatDiffer);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(757, 74);
            this.tabPage8.TabIndex = 3;
            this.tabPage8.Text = "设置";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // namedFloatControl_maxIntFloatDiffer
            // 
            this.namedFloatControl_maxIntFloatDiffer.Location = new System.Drawing.Point(24, 19);
            this.namedFloatControl_maxIntFloatDiffer.Name = "namedFloatControl_maxIntFloatDiffer";
            this.namedFloatControl_maxIntFloatDiffer.Size = new System.Drawing.Size(140, 23);
            this.namedFloatControl_maxIntFloatDiffer.TabIndex = 0;
            this.namedFloatControl_maxIntFloatDiffer.Title = "名称：";
            this.namedFloatControl_maxIntFloatDiffer.Value = 0.4D;
            // 
            // IonoFreeAmbiguityFixserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "IonoFreeAmbiguityFixserForm";
            this.Text = "模糊度计算器";
            this.Load += new System.EventHandler(this.IonoFreeAmbiguityFixserForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_mwWide;
        private Geo.Winform.Controls.FileOpenControl fileOpenControl_ionoFreeFloat;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TabPage tabPage2;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private Geo.Winform.ObjectTableControl objectTableControl1;
        private System.Windows.Forms.TabPage tabPage8;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_maxIntFloatDiffer;
    }
}