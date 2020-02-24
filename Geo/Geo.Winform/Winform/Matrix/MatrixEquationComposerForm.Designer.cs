namespace Geo.Winform
{
    partial class MatrixEquationComposerForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_multiEquation = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_filePath = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.button_saveAsBinary = new System.Windows.Forms.Button();
            this.button_saveAsText = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button_read = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_left = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_right = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
            this.checkBox_independentObsOfEach = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage4.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 112;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 112);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox_independentObsOfEach);
            this.tabPage1.Controls.Add(this.checkBox_multiEquation);
            this.tabPage1.Controls.Add(this.fileOpenControl_filePath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 86);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_multiEquation
            // 
            this.checkBox_multiEquation.AutoSize = true;
            this.checkBox_multiEquation.Location = new System.Drawing.Point(612, 15);
            this.checkBox_multiEquation.Name = "checkBox_multiEquation";
            this.checkBox_multiEquation.Size = new System.Drawing.Size(204, 16);
            this.checkBox_multiEquation.TabIndex = 1;
            this.checkBox_multiEquation.Text = "批量叠加(否则只加各文件第一个)";
            this.checkBox_multiEquation.UseVisualStyleBackColor = true;
            this.checkBox_multiEquation.CheckedChanged += new System.EventHandler(this.checkBox_singleFIle_CheckedChanged);
            // 
            // fileOpenControl_filePath
            // 
            this.fileOpenControl_filePath.AllowDrop = true;
            this.fileOpenControl_filePath.FilePath = "";
            this.fileOpenControl_filePath.FilePathes = new string[0];
            this.fileOpenControl_filePath.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_filePath.FirstPath = "";
            this.fileOpenControl_filePath.IsMultiSelect = true;
            this.fileOpenControl_filePath.LabelName = "文件路径：";
            this.fileOpenControl_filePath.Location = new System.Drawing.Point(8, 6);
            this.fileOpenControl_filePath.Name = "fileOpenControl_filePath";
            this.fileOpenControl_filePath.Size = new System.Drawing.Size(598, 77);
            this.fileOpenControl_filePath.TabIndex = 0;
            this.fileOpenControl_filePath.FilePathSetted += new System.EventHandler(this.fileOpenControl_filePath_FilePathSetted);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.directorySelectionControl1);
            this.tabPage2.Controls.Add(this.button_saveAsBinary);
            this.tabPage2.Controls.Add(this.button_saveAsText);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 86);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "输出";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(10, 13);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(488, 22);
            this.directorySelectionControl1.TabIndex = 3;
            // 
            // button_saveAsBinary
            // 
            this.button_saveAsBinary.Location = new System.Drawing.Point(87, 42);
            this.button_saveAsBinary.Name = "button_saveAsBinary";
            this.button_saveAsBinary.Size = new System.Drawing.Size(96, 23);
            this.button_saveAsBinary.TabIndex = 2;
            this.button_saveAsBinary.Text = "另存为二进制";
            this.button_saveAsBinary.UseVisualStyleBackColor = true;
            this.button_saveAsBinary.Click += new System.EventHandler(this.button_saveAsBinary_Click);
            // 
            // button_saveAsText
            // 
            this.button_saveAsText.Location = new System.Drawing.Point(6, 41);
            this.button_saveAsText.Name = "button_saveAsText";
            this.button_saveAsText.Size = new System.Drawing.Size(83, 23);
            this.button_saveAsText.TabIndex = 2;
            this.button_saveAsText.Text = "另存为文本";
            this.button_saveAsText.UseVisualStyleBackColor = true;
            this.button_saveAsText.Click += new System.EventHandler(this.button_saveAsText_Click);
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
            this.splitContainer2.Panel1.Controls.Add(this.button_read);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Size = new System.Drawing.Size(800, 334);
            this.splitContainer2.SplitterDistance = 55;
            this.splitContainer2.TabIndex = 0;
            // 
            // button_read
            // 
            this.button_read.Location = new System.Drawing.Point(654, 3);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(134, 38);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "叠加";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(800, 275);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tabControl3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 249);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "文本";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(786, 243);
            this.tabControl3.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.richTextBoxControl_left);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(778, 217);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "方程左边";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_left
            // 
            this.richTextBoxControl_left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_left.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_left.MaxAppendLineCount = 5000;
            this.richTextBoxControl_left.Name = "richTextBoxControl_left";
            this.richTextBoxControl_left.Size = new System.Drawing.Size(772, 211);
            this.richTextBoxControl_left.TabIndex = 1;
            this.richTextBoxControl_left.Text = "";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.richTextBoxControl_right);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(778, 217);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "方程右边";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_right
            // 
            this.richTextBoxControl_right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_right.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_right.MaxAppendLineCount = 5000;
            this.richTextBoxControl_right.Name = "richTextBoxControl_right";
            this.richTextBoxControl_right.Size = new System.Drawing.Size(772, 211);
            this.richTextBoxControl_right.TabIndex = 1;
            this.richTextBoxControl_right.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(792, 249);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "表格";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(786, 243);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // checkBox_independentObsOfEach
            // 
            this.checkBox_independentObsOfEach.AutoSize = true;
            this.checkBox_independentObsOfEach.Checked = true;
            this.checkBox_independentObsOfEach.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_independentObsOfEach.Location = new System.Drawing.Point(612, 52);
            this.checkBox_independentObsOfEach.Name = "checkBox_independentObsOfEach";
            this.checkBox_independentObsOfEach.Size = new System.Drawing.Size(96, 16);
            this.checkBox_independentObsOfEach.TabIndex = 1;
            this.checkBox_independentObsOfEach.Text = "每行独立观测";
            this.checkBox_independentObsOfEach.UseVisualStyleBackColor = true;
            this.checkBox_independentObsOfEach.CheckedChanged += new System.EventHandler(this.checkBox_singleFIle_CheckedChanged);
            // 
            // MatrixEquationComposerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MatrixEquationComposerForm";
            this.Text = "矩阵方程叠加器";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private ObjectTableControl objectTableControl1;
        private System.Windows.Forms.Button button_saveAsBinary;
        private System.Windows.Forms.Button button_saveAsText;
        private System.Windows.Forms.Button button_read;
        private Controls.FileOpenControl fileOpenControl_filePath;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage5;
        private Controls.RichTextBoxControl richTextBoxControl_left;
        private System.Windows.Forms.TabPage tabPage6;
        private Controls.RichTextBoxControl richTextBoxControl_right;
        private System.Windows.Forms.CheckBox checkBox_multiEquation;
        private Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.CheckBox checkBox_independentObsOfEach;
    }
}