namespace Gnsser.Winform
{
    partial class ObsPeriodDividerForm
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
            this.fileOpenControl1_fiels = new Geo.Winform.Controls.FileOpenControl();
            this.richTextBoxControl_result = new Geo.Winform.Controls.RichTextBoxControl();
            this.button_run = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.namedFloatControl_periodSpanMinutes = new Geo.Winform.Controls.NamedFloatControl();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.namedStringControl_subDirectory = new Geo.Winform.Controls.NamedStringControl();
            this.namedStringControl_netName = new Geo.Winform.Controls.NamedStringControl();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_moveto = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // fileOpenControl1_fiels
            // 
            this.fileOpenControl1_fiels.AllowDrop = true;
            this.fileOpenControl1_fiels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1_fiels.FilePath = "";
            this.fileOpenControl1_fiels.FilePathes = new string[0];
            this.fileOpenControl1_fiels.Filter = "文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1_fiels.FirstPath = "";
            this.fileOpenControl1_fiels.IsMultiSelect = true;
            this.fileOpenControl1_fiels.LabelName = "文件：";
            this.fileOpenControl1_fiels.Location = new System.Drawing.Point(12, 12);
            this.fileOpenControl1_fiels.Name = "fileOpenControl1_fiels";
            this.fileOpenControl1_fiels.Size = new System.Drawing.Size(602, 64);
            this.fileOpenControl1_fiels.TabIndex = 0;
            this.fileOpenControl1_fiels.FilePathSetted += new System.EventHandler(this.fileOpenControl1_fiels_FilePathSetted);
            // 
            // richTextBoxControl_result
            // 
            this.richTextBoxControl_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxControl_result.Location = new System.Drawing.Point(12, 179);
            this.richTextBoxControl_result.MaxAppendLineCount = 5000;
            this.richTextBoxControl_result.Name = "richTextBoxControl_result";
            this.richTextBoxControl_result.Size = new System.Drawing.Size(602, 232);
            this.richTextBoxControl_result.TabIndex = 1;
            this.richTextBoxControl_result.Text = "";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(539, 142);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 31);
            this.button_run.TabIndex = 2;
            this.button_run.Text = "运行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "用于区别不同时段";
            // 
            // namedFloatControl_periodSpanMinutes
            // 
            this.namedFloatControl_periodSpanMinutes.Location = new System.Drawing.Point(12, 150);
            this.namedFloatControl_periodSpanMinutes.Name = "namedFloatControl_periodSpanMinutes";
            this.namedFloatControl_periodSpanMinutes.Size = new System.Drawing.Size(191, 23);
            this.namedFloatControl_periodSpanMinutes.TabIndex = 5;
            this.namedFloatControl_periodSpanMinutes.Title = "最小公共时段(分)：";
            this.namedFloatControl_periodSpanMinutes.Value = 15D;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出(工程)主目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(12, 88);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(602, 22);
            this.directorySelectionControl1.TabIndex = 7;
            // 
            // namedStringControl_subDirectory
            // 
            this.namedStringControl_subDirectory.Location = new System.Drawing.Point(216, 116);
            this.namedStringControl_subDirectory.Name = "namedStringControl_subDirectory";
            this.namedStringControl_subDirectory.Size = new System.Drawing.Size(217, 23);
            this.namedStringControl_subDirectory.TabIndex = 8;
            this.namedStringControl_subDirectory.Title = "子(原始数据)目录：";
            // 
            // namedStringControl_netName
            // 
            this.namedStringControl_netName.Location = new System.Drawing.Point(12, 116);
            this.namedStringControl_netName.Name = "namedStringControl_netName";
            this.namedStringControl_netName.Size = new System.Drawing.Size(198, 23);
            this.namedStringControl_netName.TabIndex = 13;
            this.namedStringControl_netName.Title = "时段网(工程)前缀：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(440, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "空则无子目录";
            // 
            // checkBox_moveto
            // 
            this.checkBox_moveto.AutoSize = true;
            this.checkBox_moveto.Location = new System.Drawing.Point(329, 157);
            this.checkBox_moveto.Name = "checkBox_moveto";
            this.checkBox_moveto.Size = new System.Drawing.Size(168, 16);
            this.checkBox_moveto.TabIndex = 15;
            this.checkBox_moveto.Text = "移动到工程目录，默认复制";
            this.checkBox_moveto.UseVisualStyleBackColor = true;
            // 
            // ObsPeriodDividerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 413);
            this.Controls.Add(this.checkBox_moveto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.namedStringControl_netName);
            this.Controls.Add(this.namedStringControl_subDirectory);
            this.Controls.Add(this.directorySelectionControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.namedFloatControl_periodSpanMinutes);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.richTextBoxControl_result);
            this.Controls.Add(this.fileOpenControl1_fiels);
            this.Name = "ObsPeriodDividerForm";
            this.Text = "ObsPeriodDividerForm";
            this.Load += new System.EventHandler(this.ObsPeriodDividerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl1_fiels;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_result;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.Label label2;
        private Geo.Winform.Controls.NamedFloatControl namedFloatControl_periodSpanMinutes;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_subDirectory;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_netName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_moveto;
    }
}