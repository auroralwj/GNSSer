namespace Gnsser.Winform
{
    partial class DecompactRinexForm
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

        #region Windows Form Designer generated obsCodeode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCodeode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.directorySelectionControl2 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.checkBox_delete_sourse = new System.Windows.Forms.CheckBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.textBox_max_thread_count = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_convert = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.namedStringControl_extension = new Geo.Winform.Controls.NamedStringControl();
            this.checkBox_subDirIncluded = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.namedStringControl_extension);
            this.groupBox1.Controls.Add(this.directorySelectionControl2);
            this.groupBox1.Controls.Add(this.checkBox_subDirIncluded);
            this.groupBox1.Controls.Add(this.checkBox_delete_sourse);
            this.groupBox1.Controls.Add(this.directorySelectionControl1);
            this.groupBox1.Controls.Add(this.textBox_max_thread_count);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 188);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // directorySelectionControl2
            // 
            this.directorySelectionControl2.AllowDrop = true;
            this.directorySelectionControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl2.IsMultiPathes = false;
            this.directorySelectionControl2.LabelName = "O文件目录：";
            this.directorySelectionControl2.Location = new System.Drawing.Point(9, 50);
            this.directorySelectionControl2.Name = "directorySelectionControl2";
            this.directorySelectionControl2.Path = "";
            this.directorySelectionControl2.Pathes = new string[0];
            this.directorySelectionControl2.Size = new System.Drawing.Size(561, 22);
            this.directorySelectionControl2.TabIndex = 5;
            // 
            // checkBox_delete_sourse
            // 
            this.checkBox_delete_sourse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_delete_sourse.AutoSize = true;
            this.checkBox_delete_sourse.Location = new System.Drawing.Point(49, 161);
            this.checkBox_delete_sourse.Name = "checkBox_delete_sourse";
            this.checkBox_delete_sourse.Size = new System.Drawing.Size(84, 16);
            this.checkBox_delete_sourse.TabIndex = 7;
            this.checkBox_delete_sourse.Text = "删除源文件";
            this.checkBox_delete_sourse.UseVisualStyleBackColor = true;
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "D文件目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(9, 21);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(561, 22);
            this.directorySelectionControl1.TabIndex = 4;
            // 
            // textBox_max_thread_count
            // 
            this.textBox_max_thread_count.Location = new System.Drawing.Point(89, 121);
            this.textBox_max_thread_count.Name = "textBox_max_thread_count";
            this.textBox_max_thread_count.Size = new System.Drawing.Size(44, 21);
            this.textBox_max_thread_count.TabIndex = 6;
            this.textBox_max_thread_count.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "最大线程数：";
            // 
            // button_convert
            // 
            this.button_convert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_convert.Location = new System.Drawing.Point(433, 207);
            this.button_convert.Name = "button_convert";
            this.button_convert.Size = new System.Drawing.Size(75, 29);
            this.button_convert.TabIndex = 1;
            this.button_convert.Text = "转换";
            this.button_convert.UseVisualStyleBackColor = true;
            this.button_convert.Click += new System.EventHandler(this.button_convert_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(514, 208);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 29);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "关闭";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(13, 207);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(414, 34);
            this.progressBarComponent1.TabIndex = 8;
            // 
            // namedStringControl_extension
            // 
            this.namedStringControl_extension.Location = new System.Drawing.Point(6, 83);
            this.namedStringControl_extension.Name = "namedStringControl_extension";
            this.namedStringControl_extension.Size = new System.Drawing.Size(217, 23);
            this.namedStringControl_extension.TabIndex = 8;
            this.namedStringControl_extension.Title = "压缩后缀名：";
            // 
            // checkBox_subDirIncluded
            // 
            this.checkBox_subDirIncluded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_subDirIncluded.AutoSize = true;
            this.checkBox_subDirIncluded.Checked = true;
            this.checkBox_subDirIncluded.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_subDirIncluded.Location = new System.Drawing.Point(275, 90);
            this.checkBox_subDirIncluded.Name = "checkBox_subDirIncluded";
            this.checkBox_subDirIncluded.Size = new System.Drawing.Size(84, 16);
            this.checkBox_subDirIncluded.TabIndex = 7;
            this.checkBox_subDirIncluded.Text = "包含子目录";
            this.checkBox_subDirIncluded.UseVisualStyleBackColor = true;
            // 
            // DecompactRinexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 249);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_convert);
            this.Controls.Add(this.groupBox1);
            this.Name = "DecompactRinexForm";
            this.Text = "转换 compact  Rinex";
            this.Load += new System.EventHandler(this.DecompactRinexForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_convert;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TextBox textBox_max_thread_count;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_delete_sourse;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl2;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private Geo.Winform.Controls.NamedStringControl namedStringControl_extension;
        private System.Windows.Forms.CheckBox checkBox_subDirIncluded;
    }
}