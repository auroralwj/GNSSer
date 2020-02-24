namespace Gnsser.Winform
{
    partial class OFileSelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OFileSelectorForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_setReceiverFromFile = new System.Windows.Forms.Button();
            this.button_getNameFromFiles = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_ant = new System.Windows.Forms.CheckBox();
            this.checkBox_receiverInclude = new System.Windows.Forms.CheckBox();
            this.checkBox_siteIncluded = new System.Windows.Forms.CheckBox();
            this.checkBox_satCount = new System.Windows.Forms.CheckBox();
            this.button_setReceiverPath = new System.Windows.Forms.Button();
            this.button_setStaPath = new System.Windows.Forms.Button();
            this.button_setODirPath = new System.Windows.Forms.Button();
            this.textBox_satCount = new System.Windows.Forms.TextBox();
            this.textBox_receivers = new System.Windows.Forms.TextBox();
            this.textBox_sitesIncluded = new System.Windows.Forms.TextBox();
            this.textBox_ant = new System.Windows.Forms.TextBox();
            this.textBox_receiverPath = new System.Windows.Forms.TextBox();
            this.textBox_selectedPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_dir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBarComponent1 = new Geo.Winform.Controls.ProgressBarComponent();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBox_remove = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_setReceiverFromFile);
            this.groupBox1.Controls.Add(this.button_getNameFromFiles);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox_ant);
            this.groupBox1.Controls.Add(this.checkBox_receiverInclude);
            this.groupBox1.Controls.Add(this.checkBox_siteIncluded);
            this.groupBox1.Controls.Add(this.checkBox_satCount);
            this.groupBox1.Controls.Add(this.button_setReceiverPath);
            this.groupBox1.Controls.Add(this.button_setStaPath);
            this.groupBox1.Controls.Add(this.button_setODirPath);
            this.groupBox1.Controls.Add(this.textBox_satCount);
            this.groupBox1.Controls.Add(this.textBox_receivers);
            this.groupBox1.Controls.Add(this.textBox_sitesIncluded);
            this.groupBox1.Controls.Add(this.textBox_ant);
            this.groupBox1.Controls.Add(this.textBox_receiverPath);
            this.groupBox1.Controls.Add(this.textBox_selectedPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_dir);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 364);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // button_setReceiverFromFile
            // 
            this.button_setReceiverFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setReceiverFromFile.Location = new System.Drawing.Point(603, 217);
            this.button_setReceiverFromFile.Name = "button_setReceiverFromFile";
            this.button_setReceiverFromFile.Size = new System.Drawing.Size(69, 29);
            this.button_setReceiverFromFile.TabIndex = 5;
            this.button_setReceiverFromFile.Text = "文件提取";
            this.button_setReceiverFromFile.UseVisualStyleBackColor = true;
            this.button_setReceiverFromFile.Click += new System.EventHandler(this.button_setReceiverFromFile_Click);
            // 
            // button_getNameFromFiles
            // 
            this.button_getNameFromFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getNameFromFiles.Location = new System.Drawing.Point(603, 138);
            this.button_getNameFromFiles.Name = "button_getNameFromFiles";
            this.button_getNameFromFiles.Size = new System.Drawing.Size(69, 29);
            this.button_getNameFromFiles.TabIndex = 5;
            this.button_getNameFromFiles.Text = "文件提取";
            this.button_getNameFromFiles.UseVisualStyleBackColor = true;
            this.button_getNameFromFiles.Click += new System.EventHandler(this.button_getNameFromFiles_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(603, 274);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "按行分割";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(348, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(269, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "文字分隔符可为：空格、逗号、分号、换行符等。";
            // 
            // checkBox_ant
            // 
            this.checkBox_ant.AutoSize = true;
            this.checkBox_ant.Location = new System.Drawing.Point(95, 77);
            this.checkBox_ant.Name = "checkBox_ant";
            this.checkBox_ant.Size = new System.Drawing.Size(84, 16);
            this.checkBox_ant.TabIndex = 3;
            this.checkBox_ant.Text = "天线关键字";
            this.checkBox_ant.UseVisualStyleBackColor = true;
            // 
            // checkBox_receiverInclude
            // 
            this.checkBox_receiverInclude.AutoSize = true;
            this.checkBox_receiverInclude.Location = new System.Drawing.Point(95, 222);
            this.checkBox_receiverInclude.Name = "checkBox_receiverInclude";
            this.checkBox_receiverInclude.Size = new System.Drawing.Size(84, 16);
            this.checkBox_receiverInclude.TabIndex = 3;
            this.checkBox_receiverInclude.Text = "包含接收机";
            this.checkBox_receiverInclude.UseVisualStyleBackColor = true;
            // 
            // checkBox_siteIncluded
            // 
            this.checkBox_siteIncluded.AutoSize = true;
            this.checkBox_siteIncluded.Location = new System.Drawing.Point(95, 140);
            this.checkBox_siteIncluded.Name = "checkBox_siteIncluded";
            this.checkBox_siteIncluded.Size = new System.Drawing.Size(72, 16);
            this.checkBox_siteIncluded.TabIndex = 3;
            this.checkBox_siteIncluded.Text = "包含站名";
            this.checkBox_siteIncluded.UseVisualStyleBackColor = true;
            // 
            // checkBox_satCount
            // 
            this.checkBox_satCount.AutoSize = true;
            this.checkBox_satCount.Location = new System.Drawing.Point(95, 107);
            this.checkBox_satCount.Name = "checkBox_satCount";
            this.checkBox_satCount.Size = new System.Drawing.Size(120, 16);
            this.checkBox_satCount.TabIndex = 3;
            this.checkBox_satCount.Text = "历元卫星数量大于";
            this.checkBox_satCount.UseVisualStyleBackColor = true;
            // 
            // button_setReceiverPath
            // 
            this.button_setReceiverPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setReceiverPath.Location = new System.Drawing.Point(540, 220);
            this.button_setReceiverPath.Name = "button_setReceiverPath";
            this.button_setReceiverPath.Size = new System.Drawing.Size(47, 23);
            this.button_setReceiverPath.TabIndex = 2;
            this.button_setReceiverPath.Text = "...";
            this.button_setReceiverPath.UseVisualStyleBackColor = true;
            this.button_setReceiverPath.Click += new System.EventHandler(this.button_setReceiverPath_Click);
            // 
            // button_setStaPath
            // 
            this.button_setStaPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setStaPath.Location = new System.Drawing.Point(625, 45);
            this.button_setStaPath.Name = "button_setStaPath";
            this.button_setStaPath.Size = new System.Drawing.Size(47, 23);
            this.button_setStaPath.TabIndex = 2;
            this.button_setStaPath.Text = "...";
            this.button_setStaPath.UseVisualStyleBackColor = true;
            this.button_setStaPath.Click += new System.EventHandler(this.button_setStaPath_Click);
            // 
            // button_setODirPath
            // 
            this.button_setODirPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setODirPath.Location = new System.Drawing.Point(625, 18);
            this.button_setODirPath.Name = "button_setODirPath";
            this.button_setODirPath.Size = new System.Drawing.Size(47, 23);
            this.button_setODirPath.TabIndex = 2;
            this.button_setODirPath.Text = "...";
            this.button_setODirPath.UseVisualStyleBackColor = true;
            this.button_setODirPath.Click += new System.EventHandler(this.button_setODirPath_Click);
            // 
            // textBox_satCount
            // 
            this.textBox_satCount.Location = new System.Drawing.Point(221, 105);
            this.textBox_satCount.Name = "textBox_satCount";
            this.textBox_satCount.Size = new System.Drawing.Size(108, 21);
            this.textBox_satCount.TabIndex = 1;
            this.textBox_satCount.Text = "15";
            // 
            // textBox_receivers
            // 
            this.textBox_receivers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_receivers.Location = new System.Drawing.Point(179, 246);
            this.textBox_receivers.Multiline = true;
            this.textBox_receivers.Name = "textBox_receivers";
            this.textBox_receivers.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_receivers.Size = new System.Drawing.Size(418, 54);
            this.textBox_receivers.TabIndex = 1;
            this.textBox_receivers.Text = "AOA BENCHMARK ACT\r\nAOA ICS-4000Z";
            // 
            // textBox_sitesIncluded
            // 
            this.textBox_sitesIncluded.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sitesIncluded.Location = new System.Drawing.Point(179, 138);
            this.textBox_sitesIncluded.Multiline = true;
            this.textBox_sitesIncluded.Name = "textBox_sitesIncluded";
            this.textBox_sitesIncluded.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_sitesIncluded.Size = new System.Drawing.Size(418, 78);
            this.textBox_sitesIncluded.TabIndex = 1;
            this.textBox_sitesIncluded.Text = resources.GetString("textBox_sitesIncluded.Text");
            // 
            // textBox_ant
            // 
            this.textBox_ant.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ant.Location = new System.Drawing.Point(221, 75);
            this.textBox_ant.Name = "textBox_ant";
            this.textBox_ant.Size = new System.Drawing.Size(385, 21);
            this.textBox_ant.TabIndex = 1;
            this.textBox_ant.Text = "DORNE";
            // 
            // textBox_receiverPath
            // 
            this.textBox_receiverPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_receiverPath.Location = new System.Drawing.Point(179, 222);
            this.textBox_receiverPath.Name = "textBox_receiverPath";
            this.textBox_receiverPath.Size = new System.Drawing.Size(355, 21);
            this.textBox_receiverPath.TabIndex = 1;
            this.textBox_receiverPath.Text = "C:\\GPSDATA\\EXAMPLE\\GEN\\RECEIVER";
            // 
            // textBox_selectedPath
            // 
            this.textBox_selectedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_selectedPath.Location = new System.Drawing.Point(95, 48);
            this.textBox_selectedPath.Name = "textBox_selectedPath";
            this.textBox_selectedPath.Size = new System.Drawing.Size(511, 21);
            this.textBox_selectedPath.TabIndex = 1;
            this.textBox_selectedPath.Text = "C:\\Downloads\\Selected";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "目标目录：";
            // 
            // textBox_dir
            // 
            this.textBox_dir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dir.Location = new System.Drawing.Point(95, 21);
            this.textBox_dir.Name = "textBox_dir";
            this.textBox_dir.Size = new System.Drawing.Size(511, 21);
            this.textBox_dir.TabIndex = 1;
            this.textBox_dir.Text = "C:\\Downloads";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "测站目录：";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(581, 410);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 32);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "执行筛选";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // progressBarComponent1
            // 
            this.progressBarComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarComponent1.Classifies = null;
            this.progressBarComponent1.ClassifyIndex = 0;
            this.progressBarComponent1.IsUsePercetageStep = false;
            this.progressBarComponent1.Location = new System.Drawing.Point(13, 383);
            this.progressBarComponent1.Name = "progressBarComponent1";
            this.progressBarComponent1.Size = new System.Drawing.Size(562, 59);
            this.progressBarComponent1.TabIndex = 3;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkBox_remove
            // 
            this.checkBox_remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_remove.AutoSize = true;
            this.checkBox_remove.Location = new System.Drawing.Point(581, 388);
            this.checkBox_remove.Name = "checkBox_remove";
            this.checkBox_remove.Size = new System.Drawing.Size(108, 16);
            this.checkBox_remove.TabIndex = 4;
            this.checkBox_remove.Text = "删除所选源文件";
            this.checkBox_remove.UseVisualStyleBackColor = true;
            // 
            // OFileSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 454);
            this.Controls.Add(this.checkBox_remove);
            this.Controls.Add(this.progressBarComponent1);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.groupBox1);
            this.Name = "OFileSelectorForm";
            this.Text = "本地观测文件选择器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.Button button_setStaPath;
        private System.Windows.Forms.Button button_setODirPath;
        private System.Windows.Forms.TextBox textBox_selectedPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_dir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox_ant;
        private Geo.Winform.Controls.ProgressBarComponent progressBarComponent1;
        private System.Windows.Forms.TextBox textBox_satCount;
        private System.Windows.Forms.CheckBox checkBox_satCount;
        private System.Windows.Forms.CheckBox checkBox_ant;
        private System.Windows.Forms.CheckBox checkBox_siteIncluded;
        private System.Windows.Forms.TextBox textBox_sitesIncluded;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_getNameFromFiles;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_setReceiverFromFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_receiverInclude;
        private System.Windows.Forms.TextBox textBox_receivers;
        private System.Windows.Forms.Button button_setReceiverPath;
        private System.Windows.Forms.TextBox textBox_receiverPath;
        private System.Windows.Forms.CheckBox checkBox_remove;
    }
}