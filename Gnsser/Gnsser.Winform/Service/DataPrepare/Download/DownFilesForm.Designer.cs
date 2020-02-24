namespace Gnsser.Winform
{
    partial class DownFilesForm
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_fileurls = new System.Windows.Forms.TextBox();
            this.button_download = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button_setFilePath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_filePath = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2_localPath = new System.Windows.Forms.TextBox();
            this.button_setLocalPath = new System.Windows.Forms.Button();
            this.button_readFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button_cancel = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox_fileurls);
            this.groupBox3.Location = new System.Drawing.Point(9, 56);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(573, 114);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "文件地址";
            // 
            // textBox_fileurls
            // 
            this.textBox_fileurls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_fileurls.Location = new System.Drawing.Point(3, 17);
            this.textBox_fileurls.Multiline = true;
            this.textBox_fileurls.Name = "textBox_fileurls";
            this.textBox_fileurls.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_fileurls.Size = new System.Drawing.Size(567, 94);
            this.textBox_fileurls.TabIndex = 2;
            // 
            // button_download
            // 
            this.button_download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_download.Location = new System.Drawing.Point(429, 176);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(75, 29);
            this.button_download.TabIndex = 5;
            this.button_download.Text = "下载";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 175);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(420, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // button_setFilePath
            // 
            this.button_setFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setFilePath.Location = new System.Drawing.Point(483, 2);
            this.button_setFilePath.Name = "button_setFilePath";
            this.button_setFilePath.Size = new System.Drawing.Size(48, 23);
            this.button_setFilePath.TabIndex = 7;
            this.button_setFilePath.Text = "...";
            this.button_setFilePath.UseVisualStyleBackColor = true;
            this.button_setFilePath.Click += new System.EventHandler(this.button_setFilePath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "地址文件：";
            // 
            // textBox_filePath
            // 
            this.textBox_filePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_filePath.Location = new System.Drawing.Point(72, 2);
            this.textBox_filePath.Name = "textBox_filePath";
            this.textBox_filePath.Size = new System.Drawing.Size(405, 21);
            this.textBox_filePath.TabIndex = 9;
            this.textBox_filePath.Text = ".\\Data\\SiteUrl.txt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "存储位置：";
            // 
            // textBox2_localPath
            // 
            this.textBox2_localPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2_localPath.Location = new System.Drawing.Point(72, 29);
            this.textBox2_localPath.Name = "textBox2_localPath";
            this.textBox2_localPath.Size = new System.Drawing.Size(405, 21);
            this.textBox2_localPath.TabIndex = 9;
            this.textBox2_localPath.Text = "C:\\Downloads";
            // 
            // button_setLocalPath
            // 
            this.button_setLocalPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setLocalPath.Location = new System.Drawing.Point(483, 30);
            this.button_setLocalPath.Name = "button_setLocalPath";
            this.button_setLocalPath.Size = new System.Drawing.Size(48, 23);
            this.button_setLocalPath.TabIndex = 7;
            this.button_setLocalPath.Text = "...";
            this.button_setLocalPath.UseVisualStyleBackColor = true;
            this.button_setLocalPath.Click += new System.EventHandler(this.button1_placeToSave_Click);
            // 
            // button_readFile
            // 
            this.button_readFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readFile.Location = new System.Drawing.Point(537, 2);
            this.button_readFile.Name = "button_readFile";
            this.button_readFile.Size = new System.Drawing.Size(53, 23);
            this.button_readFile.TabIndex = 7;
            this.button_readFile.Text = "读取";
            this.button_readFile.UseVisualStyleBackColor = true;
            this.button_readFile.Click += new System.EventHandler(this.button_readFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox_result);
            this.groupBox1.Location = new System.Drawing.Point(9, 199);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 129);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "执行结果";
            // 
            // textBox_result
            // 
            this.textBox_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_result.Location = new System.Drawing.Point(3, 17);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(576, 109);
            this.textBox_result.TabIndex = 2;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(504, 176);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 29);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // DownFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 330);
            this.Controls.Add(this.textBox2_localPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_filePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_setLocalPath);
            this.Controls.Add(this.button_readFile);
            this.Controls.Add(this.button_setFilePath);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "DownFilesForm";
            this.Text = "批量文件下载";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownFilesForm_FormClosing);
            this.Load += new System.EventHandler(this.DownFilesForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_fileurls;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button_setFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_filePath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2_localPath;
        private System.Windows.Forms.Button button_setLocalPath;
        private System.Windows.Forms.Button button_readFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_result;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button_cancel;

    }
}