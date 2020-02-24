namespace Geo.Winform
{
    partial class MultiMatrixEquationCalculateForm
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
            this.checkBox_enbaleFixParam = new System.Windows.Forms.CheckBox();
            this.fileOpenControl_fixedParam = new Geo.Winform.Controls.FileOpenControl();
            this.checkBox_singleFIle = new System.Windows.Forms.CheckBox();
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
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_normal = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.objectTableControl1 = new Geo.Winform.ObjectTableControl();
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
            this.tabPage7.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(908, 450);
            this.splitContainer1.SplitterDistance = 150;
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
            this.tabControl1.Size = new System.Drawing.Size(908, 150);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox_enbaleFixParam);
            this.tabPage1.Controls.Add(this.fileOpenControl_fixedParam);
            this.tabPage1.Controls.Add(this.checkBox_singleFIle);
            this.tabPage1.Controls.Add(this.fileOpenControl_filePath);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(900, 124);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_enbaleFixParam
            // 
            this.checkBox_enbaleFixParam.AutoSize = true;
            this.checkBox_enbaleFixParam.Location = new System.Drawing.Point(567, 95);
            this.checkBox_enbaleFixParam.Name = "checkBox_enbaleFixParam";
            this.checkBox_enbaleFixParam.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enbaleFixParam.TabIndex = 4;
            this.checkBox_enbaleFixParam.Text = "启用";
            this.checkBox_enbaleFixParam.UseVisualStyleBackColor = true;
            // 
            // fileOpenControl_fixedParam
            // 
            this.fileOpenControl_fixedParam.AllowDrop = true;
            this.fileOpenControl_fixedParam.FilePath = "";
            this.fileOpenControl_fixedParam.FilePathes = new string[0];
            this.fileOpenControl_fixedParam.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_fixedParam.FirstPath = "";
            this.fileOpenControl_fixedParam.IsMultiSelect = false;
            this.fileOpenControl_fixedParam.LabelName = "固定参数/模糊度文件路径：";
            this.fileOpenControl_fixedParam.Location = new System.Drawing.Point(8, 89);
            this.fileOpenControl_fixedParam.Name = "fileOpenControl_fixedParam";
            this.fileOpenControl_fixedParam.Size = new System.Drawing.Size(553, 22);
            this.fileOpenControl_fixedParam.TabIndex = 3;
            // 
            // checkBox_singleFIle
            // 
            this.checkBox_singleFIle.AutoSize = true;
            this.checkBox_singleFIle.Location = new System.Drawing.Point(576, 17);
            this.checkBox_singleFIle.Name = "checkBox_singleFIle";
            this.checkBox_singleFIle.Size = new System.Drawing.Size(216, 16);
            this.checkBox_singleFIle.TabIndex = 1;
            this.checkBox_singleFIle.Text = "单文件多方程，否则多文件第一方程";
            this.checkBox_singleFIle.UseVisualStyleBackColor = true;
            this.checkBox_singleFIle.CheckedChanged += new System.EventHandler(this.checkBox_singleFIle_CheckedChanged);
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
            this.fileOpenControl_filePath.Size = new System.Drawing.Size(553, 77);
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
            this.tabPage2.Size = new System.Drawing.Size(900, 124);
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
            this.directorySelectionControl1.Size = new System.Drawing.Size(660, 22);
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
            this.splitContainer2.Size = new System.Drawing.Size(908, 296);
            this.splitContainer2.SplitterDistance = 55;
            this.splitContainer2.TabIndex = 0;
            // 
            // button_read
            // 
            this.button_read.Location = new System.Drawing.Point(791, 3);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 38);
            this.button_read.TabIndex = 1;
            this.button_read.Text = "叠加求解";
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
            this.tabControl2.Size = new System.Drawing.Size(908, 237);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tabControl3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(900, 211);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "文本";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Controls.Add(this.tabPage7);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(894, 205);
            this.tabControl3.TabIndex = 1;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.richTextBoxControl_left);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(886, 179);
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
            this.richTextBoxControl_left.Size = new System.Drawing.Size(880, 173);
            this.richTextBoxControl_left.TabIndex = 1;
            this.richTextBoxControl_left.Text = "";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.richTextBoxControl_right);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(778, 179);
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
            this.richTextBoxControl_right.Size = new System.Drawing.Size(772, 173);
            this.richTextBoxControl_right.TabIndex = 1;
            this.richTextBoxControl_right.Text = "";
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.richTextBoxControl_normal);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(778, 179);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "法方程";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_normal
            // 
            this.richTextBoxControl_normal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_normal.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_normal.MaxAppendLineCount = 5000;
            this.richTextBoxControl_normal.Name = "richTextBoxControl_normal";
            this.richTextBoxControl_normal.Size = new System.Drawing.Size(772, 173);
            this.richTextBoxControl_normal.TabIndex = 2;
            this.richTextBoxControl_normal.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.objectTableControl1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(792, 211);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "表格";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // objectTableControl1
            // 
            this.objectTableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTableControl1.Location = new System.Drawing.Point(3, 3);
            this.objectTableControl1.Name = "objectTableControl1";
            this.objectTableControl1.Size = new System.Drawing.Size(786, 205);
            this.objectTableControl1.TabIndex = 0;
            this.objectTableControl1.TableObjectStorage = null;
            // 
            // MultiMatrixEquationCalculateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MultiMatrixEquationCalculateForm";
            this.Text = "多矩阵方程计算";
            this.Load += new System.EventHandler(this.MultiMatrixEquationCalculateForm_Load);
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
            this.tabPage7.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox checkBox_singleFIle;
        private Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.TabPage tabPage7;
        private Controls.RichTextBoxControl richTextBoxControl_normal;
        private System.Windows.Forms.CheckBox checkBox_enbaleFixParam;
        private Controls.FileOpenControl fileOpenControl_fixedParam;
    }
}