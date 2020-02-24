namespace Geo.Winform
{
    partial class CommonAdjusterForm
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
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.fileOpenControl_file = new Geo.Winform.Controls.FileOpenControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.enumRadioControl1AdjustType = new Geo.Winform.EnumRadioControl();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_run = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView_param = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView_rms = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_param)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rms)).BeginInit();
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
            this.splitContainer_main.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer_main.Size = new System.Drawing.Size(658, 400);
            this.splitContainer_main.SplitterDistance = 186;
            this.splitContainer_main.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.progressBarComponent1);
            this.splitContainer2.Panel2.Controls.Add(this.button_cancel);
            this.splitContainer2.Panel2.Controls.Add(this.button_run);
            this.splitContainer2.Size = new System.Drawing.Size(658, 186);
            this.splitContainer2.SplitterDistance = 113;
            this.splitContainer2.TabIndex = 1;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(658, 113);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.directorySelectionControl1);
            this.tabPage3.Controls.Add(this.fileOpenControl_file);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(650, 87);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "输入";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "输出目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(18, 50);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(598, 22);
            this.directorySelectionControl1.TabIndex = 1;
            // 
            // fileOpenControl_file
            // 
            this.fileOpenControl_file.AllowDrop = true;
            this.fileOpenControl_file.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl_file.FilePath = "";
            this.fileOpenControl_file.FilePathes = new string[0];
            this.fileOpenControl_file.Filter = "GNSSer文本表文件(*.txt.xls)|*.txt.xls|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl_file.FirstPath = "";
            this.fileOpenControl_file.IsMultiSelect = false;
            this.fileOpenControl_file.LabelName = "平差文件：";
            this.fileOpenControl_file.Location = new System.Drawing.Point(18, 21);
            this.fileOpenControl_file.Name = "fileOpenControl_file";
            this.fileOpenControl_file.Size = new System.Drawing.Size(598, 22);
            this.fileOpenControl_file.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.enumRadioControl1AdjustType);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(650, 87);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "设置";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // enumRadioControl1AdjustType
            // 
            this.enumRadioControl1AdjustType.Dock = System.Windows.Forms.DockStyle.Top;
            this.enumRadioControl1AdjustType.Location = new System.Drawing.Point(3, 3);
            this.enumRadioControl1AdjustType.Name = "enumRadioControl1AdjustType";
            this.enumRadioControl1AdjustType.Size = new System.Drawing.Size(644, 79);
            this.enumRadioControl1AdjustType.TabIndex = 0;
            this.enumRadioControl1AdjustType.Title = "选项";
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(22, 2);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(458, 34);
            this.progressBarComponent1.TabIndex = 1;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(571, 3);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 34);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(486, 3);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 34);
            this.button_run.TabIndex = 0;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(658, 210);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView_param);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(650, 184);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "参数值";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView_param
            // 
            this.dataGridView_param.AllowUserToAddRows = false;
            this.dataGridView_param.AllowUserToDeleteRows = false;
            this.dataGridView_param.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_param.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_param.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_param.Name = "dataGridView_param";
            this.dataGridView_param.ReadOnly = true;
            this.dataGridView_param.RowTemplate.Height = 23;
            this.dataGridView_param.Size = new System.Drawing.Size(644, 178);
            this.dataGridView_param.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView_rms);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(650, 184);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "参数RMS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView_rms
            // 
            this.dataGridView_rms.AllowUserToAddRows = false;
            this.dataGridView_rms.AllowUserToDeleteRows = false;
            this.dataGridView_rms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_rms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_rms.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_rms.Name = "dataGridView_rms";
            this.dataGridView_rms.ReadOnly = true;
            this.dataGridView_rms.RowTemplate.Height = 23;
            this.dataGridView_rms.Size = new System.Drawing.Size(644, 178);
            this.dataGridView_rms.TabIndex = 0;
            // 
            // CommonAdjusterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(658, 400);
            this.Controls.Add(this.splitContainer_main);
            this.Name = "CommonAdjusterForm";
            this.Text = "通用平差器";
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_param)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rms)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_run;


        private System.Windows.Forms.SplitContainer splitContainer_main;
        #endregion
        private Controls.FileOpenControl fileOpenControl_file;
        private EnumRadioControl enumRadioControl1AdjustType;
        private Controls.DirectorySelectionControl directorySelectionControl1;
        private Controls.ProgressBarComponent progressBarComponent1;
        private System.Windows.Forms.DataGridView dataGridView_rms;
        private System.Windows.Forms.DataGridView dataGridView_param;
    }
}