namespace Gnsser.Winform
{
    partial class Sp3FilemaintainForm
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
            this.fileOpenControl1 = new Geo.Winform.Controls.FileOpenControl();
            this.button_run = new System.Windows.Forms.Button();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.checkBox_moveNotIgs = new System.Windows.Forms.CheckBox();
            this.checkBox_parse = new System.Windows.Forms.CheckBox();
            this.checkBox_moveZ = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // fileOpenControl1
            // 
            this.fileOpenControl1.AllowDrop = true;
            this.fileOpenControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenControl1.FilePath = "";
            this.fileOpenControl1.FilePathes = new string[0];
            this.fileOpenControl1.Filter = "sp3|*.sp3|文本文件|*.txt|所有文件|*.*";
            this.fileOpenControl1.FirstPath = "";
            this.fileOpenControl1.IsMultiSelect = true;
            this.fileOpenControl1.LabelName = "星历文件/夹：";
            this.fileOpenControl1.Location = new System.Drawing.Point(24, 12);
            this.fileOpenControl1.Name = "fileOpenControl1";
            this.fileOpenControl1.Size = new System.Drawing.Size(745, 51);
            this.fileOpenControl1.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(694, 192);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 48);
            this.button_run.TabIndex = 5;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(67, 206);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(584, 34);
            this.progressBarComponent1.TabIndex = 7;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "失败文件夹：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(24, 98);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(745, 22);
            this.directorySelectionControl1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "请确保没有其它文件访问该文件";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // checkBox_moveNotIgs
            // 
            this.checkBox_moveNotIgs.AutoSize = true;
            this.checkBox_moveNotIgs.Location = new System.Drawing.Point(105, 138);
            this.checkBox_moveNotIgs.Name = "checkBox_moveNotIgs";
            this.checkBox_moveNotIgs.Size = new System.Drawing.Size(138, 16);
            this.checkBox_moveNotIgs.TabIndex = 10;
            this.checkBox_moveNotIgs.Text = "移走非IGS开头的星历";
            this.checkBox_moveNotIgs.UseVisualStyleBackColor = true;
            // 
            // checkBox_parse
            // 
            this.checkBox_parse.AutoSize = true;
            this.checkBox_parse.Checked = true;
            this.checkBox_parse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_parse.Location = new System.Drawing.Point(260, 138);
            this.checkBox_parse.Name = "checkBox_parse";
            this.checkBox_parse.Size = new System.Drawing.Size(72, 16);
            this.checkBox_parse.TabIndex = 11;
            this.checkBox_parse.Text = "解析内容";
            this.checkBox_parse.UseVisualStyleBackColor = true;
            // 
            // checkBox_moveZ
            // 
            this.checkBox_moveZ.AutoSize = true;
            this.checkBox_moveZ.Checked = true;
            this.checkBox_moveZ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_moveZ.Location = new System.Drawing.Point(363, 138);
            this.checkBox_moveZ.Name = "checkBox_moveZ";
            this.checkBox_moveZ.Size = new System.Drawing.Size(180, 16);
            this.checkBox_moveZ.TabIndex = 11;
            this.checkBox_moveZ.Text = "解析成功后，移走.Z压缩文件";
            this.checkBox_moveZ.UseVisualStyleBackColor = true;
            // 
            // Sp3FilemaintainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkBox_moveZ);
            this.Controls.Add(this.checkBox_parse);
            this.Controls.Add(this.checkBox_moveNotIgs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directorySelectionControl1);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.fileOpenControl1);
            this.Name = "Sp3FilemaintainForm";
            this.Text = "星历维护器";
            this.Load += new System.EventHandler(this.FilemaintainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Geo.Winform.Controls.FileOpenControl fileOpenControl1;
        private System.Windows.Forms.Button button_run;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBox_moveNotIgs;
        private System.Windows.Forms.CheckBox checkBox_parse;
        private System.Windows.Forms.CheckBox checkBox_moveZ;
    }
}