namespace Gnsser.Winform
{
    partial class DataPrepareForm
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

        #region Windows Form Designer generated obsCode

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the obsCode editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton_local = new System.Windows.Forms.RadioButton();
            this.radioButton_remote = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton_up = new System.Windows.Forms.RadioButton();
            this.radioButton_low = new System.Windows.Forms.RadioButton();
            this.checkBox_ignoreError = new System.Windows.Forms.CheckBox();
            this.checkBox_formatOfile = new System.Windows.Forms.CheckBox();
            this.checkBox_teqcFormat = new System.Windows.Forms.CheckBox();
            this.checkBox_delMidFile = new System.Windows.Forms.CheckBox();
            this.checkBox_delOriFile = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_urlfilePath = new System.Windows.Forms.TextBox();
            this.button_setFilePath = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.radioButton_up);
            this.groupBox1.Controls.Add(this.radioButton_low);
            this.groupBox1.Controls.Add(this.checkBox_ignoreError);
            this.groupBox1.Controls.Add(this.checkBox_formatOfile);
            this.groupBox1.Controls.Add(this.checkBox_teqcFormat);
            this.groupBox1.Controls.Add(this.checkBox_delMidFile);
            this.groupBox1.Controls.Add(this.checkBox_delOriFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(627, 176);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directorySelectionControl1.IsMultiPathes = true;
            this.directorySelectionControl1.LabelName = "结果目录：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(0, 0);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "";
            this.directorySelectionControl1.Pathes = new string[0];
            this.directorySelectionControl1.Size = new System.Drawing.Size(298, 100);
            this.directorySelectionControl1.TabIndex = 26;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton_local);
            this.panel1.Controls.Add(this.radioButton_remote);
            this.panel1.Location = new System.Drawing.Point(3, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(84, 47);
            this.panel1.TabIndex = 25;
            // 
            // radioButton_local
            // 
            this.radioButton_local.AutoSize = true;
            this.radioButton_local.Location = new System.Drawing.Point(9, 25);
            this.radioButton_local.Name = "radioButton_local";
            this.radioButton_local.Size = new System.Drawing.Size(71, 16);
            this.radioButton_local.TabIndex = 21;
            this.radioButton_local.Text = "本地目录";
            this.radioButton_local.UseVisualStyleBackColor = true;
            // 
            // radioButton_remote
            // 
            this.radioButton_remote.AutoSize = true;
            this.radioButton_remote.Checked = true;
            this.radioButton_remote.Location = new System.Drawing.Point(9, 3);
            this.radioButton_remote.Name = "radioButton_remote";
            this.radioButton_remote.Size = new System.Drawing.Size(71, 16);
            this.radioButton_remote.TabIndex = 20;
            this.radioButton_remote.TabStop = true;
            this.radioButton_remote.Text = "远程文件";
            this.radioButton_remote.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(6, 130);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(162, 16);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "是否需要解压（Z-D文件）";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // radioButton_up
            // 
            this.radioButton_up.AutoSize = true;
            this.radioButton_up.Location = new System.Drawing.Point(275, 152);
            this.radioButton_up.Name = "radioButton_up";
            this.radioButton_up.Size = new System.Drawing.Size(95, 16);
            this.radioButton_up.TabIndex = 21;
            this.radioButton_up.TabStop = true;
            this.radioButton_up.Text = "名称改为大写";
            this.radioButton_up.UseVisualStyleBackColor = true;
            // 
            // radioButton_low
            // 
            this.radioButton_low.AutoSize = true;
            this.radioButton_low.Location = new System.Drawing.Point(174, 152);
            this.radioButton_low.Name = "radioButton_low";
            this.radioButton_low.Size = new System.Drawing.Size(95, 16);
            this.radioButton_low.TabIndex = 20;
            this.radioButton_low.TabStop = true;
            this.radioButton_low.Text = "名称改为小写";
            this.radioButton_low.UseVisualStyleBackColor = true;
            // 
            // checkBox_ignoreError
            // 
            this.checkBox_ignoreError.AutoSize = true;
            this.checkBox_ignoreError.Checked = true;
            this.checkBox_ignoreError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ignoreError.Location = new System.Drawing.Point(10, 156);
            this.checkBox_ignoreError.Name = "checkBox_ignoreError";
            this.checkBox_ignoreError.Size = new System.Drawing.Size(96, 16);
            this.checkBox_ignoreError.TabIndex = 17;
            this.checkBox_ignoreError.Text = "忽略执行错误";
            this.checkBox_ignoreError.UseVisualStyleBackColor = true;
            // 
            // checkBox_formatOfile
            // 
            this.checkBox_formatOfile.AutoSize = true;
            this.checkBox_formatOfile.Checked = true;
            this.checkBox_formatOfile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_formatOfile.Location = new System.Drawing.Point(483, 127);
            this.checkBox_formatOfile.Name = "checkBox_formatOfile";
            this.checkBox_formatOfile.Size = new System.Drawing.Size(108, 16);
            this.checkBox_formatOfile.TabIndex = 19;
            this.checkBox_formatOfile.Text = "格式化观测文件";
            this.checkBox_formatOfile.UseVisualStyleBackColor = true;
            // 
            // checkBox_teqcFormat
            // 
            this.checkBox_teqcFormat.AutoSize = true;
            this.checkBox_teqcFormat.Location = new System.Drawing.Point(393, 127);
            this.checkBox_teqcFormat.Name = "checkBox_teqcFormat";
            this.checkBox_teqcFormat.Size = new System.Drawing.Size(84, 16);
            this.checkBox_teqcFormat.TabIndex = 19;
            this.checkBox_teqcFormat.Text = "TEQC格式化";
            this.checkBox_teqcFormat.UseVisualStyleBackColor = true;
            // 
            // checkBox_delMidFile
            // 
            this.checkBox_delMidFile.AutoSize = true;
            this.checkBox_delMidFile.Checked = true;
            this.checkBox_delMidFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_delMidFile.Location = new System.Drawing.Point(283, 127);
            this.checkBox_delMidFile.Name = "checkBox_delMidFile";
            this.checkBox_delMidFile.Size = new System.Drawing.Size(96, 16);
            this.checkBox_delMidFile.TabIndex = 18;
            this.checkBox_delMidFile.Text = "删除中间文件";
            this.checkBox_delMidFile.UseVisualStyleBackColor = true;
            // 
            // checkBox_delOriFile
            // 
            this.checkBox_delOriFile.AutoSize = true;
            this.checkBox_delOriFile.Checked = true;
            this.checkBox_delOriFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_delOriFile.Location = new System.Drawing.Point(173, 127);
            this.checkBox_delOriFile.Name = "checkBox_delOriFile";
            this.checkBox_delOriFile.Size = new System.Drawing.Size(96, 16);
            this.checkBox_delOriFile.TabIndex = 17;
            this.checkBox_delOriFile.Text = "删除原始文件";
            this.checkBox_delOriFile.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "文件或目录：";
            // 
            // textBox_urlfilePath
            // 
            this.textBox_urlfilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_urlfilePath.Location = new System.Drawing.Point(96, 7);
            this.textBox_urlfilePath.Multiline = true;
            this.textBox_urlfilePath.Name = "textBox_urlfilePath";
            this.textBox_urlfilePath.Size = new System.Drawing.Size(170, 88);
            this.textBox_urlfilePath.TabIndex = 16;
            this.textBox_urlfilePath.Text = ".\\Data\\SiteUrl.txt";
            // 
            // button_setFilePath
            // 
            this.button_setFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setFilePath.Location = new System.Drawing.Point(272, 7);
            this.button_setFilePath.Name = "button_setFilePath";
            this.button_setFilePath.Size = new System.Drawing.Size(34, 90);
            this.button_setFilePath.TabIndex = 12;
            this.button_setFilePath.Text = "...";
            this.button_setFilePath.UseVisualStyleBackColor = true;
            this.button_setFilePath.Click += new System.EventHandler(this.button_setFilePath_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(564, 189);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 34);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(481, 189);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 34);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox_result);
            this.groupBox2.Location = new System.Drawing.Point(10, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(628, 167);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "执行结果";
            // 
            // textBox_result
            // 
            this.textBox_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_result.Location = new System.Drawing.Point(3, 17);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(622, 147);
            this.textBox_result.TabIndex = 2;
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 1;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(13, 186);
            this.progressBarComponent1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(465, 36);
            this.progressBarComponent1.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(7, 20);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_urlfilePath);
            this.splitContainer1.Panel1.Controls.Add(this.button_setFilePath);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.directorySelectionControl1);
            this.splitContainer1.Size = new System.Drawing.Size(614, 100);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.TabIndex = 6;
            // 
            // DataPrepareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 398);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.groupBox1);
            this.Name = "DataPrepareForm";
            this.Text = "Gnss数据准备";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownFilesForm_FormClosing);
            this.Load += new System.EventHandler(this.DataPrepareForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_urlfilePath;
        private System.Windows.Forms.Button button_setFilePath;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.CheckBox checkBox_teqcFormat;
        private System.Windows.Forms.CheckBox checkBox_delMidFile;
        private System.Windows.Forms.CheckBox checkBox_delOriFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.CheckBox checkBox_ignoreError;
        private System.Windows.Forms.RadioButton radioButton_up;
        private System.Windows.Forms.RadioButton radioButton_low;
        private System.Windows.Forms.CheckBox checkBox_formatOfile;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_local;
        private System.Windows.Forms.RadioButton radioButton_remote;
        private System.Windows.Forms.Label label1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}