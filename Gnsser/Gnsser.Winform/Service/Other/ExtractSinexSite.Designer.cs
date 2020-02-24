namespace Gnsser.Winform.Other
{
    partial class ExtractSinexSite
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_SinexFile = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_SinexFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ObsFilePath = new System.Windows.Forms.TextBox();
            this.button_ObsFilePath = new System.Windows.Forms.Button();
            this.button_extract = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sinex文件：";
            // 
            // textBox_SinexFile
            // 
            this.textBox_SinexFile.Location = new System.Drawing.Point(116, 29);
            this.textBox_SinexFile.Name = "textBox_SinexFile";
            this.textBox_SinexFile.Size = new System.Drawing.Size(344, 21);
            this.textBox_SinexFile.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "SINEX文件|*.*SNX;*.*snx|所有文件|*.*";
            // 
            // button_SinexFile
            // 
            this.button_SinexFile.Location = new System.Drawing.Point(498, 27);
            this.button_SinexFile.Name = "button_SinexFile";
            this.button_SinexFile.Size = new System.Drawing.Size(75, 23);
            this.button_SinexFile.TabIndex = 2;
            this.button_SinexFile.Text = "…";
            this.button_SinexFile.UseVisualStyleBackColor = true;
            this.button_SinexFile.Click += new System.EventHandler(this.button_SinexFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "O文件目录：";
            // 
            // textBox_ObsFilePath
            // 
            this.textBox_ObsFilePath.Location = new System.Drawing.Point(116, 98);
            this.textBox_ObsFilePath.Name = "textBox_ObsFilePath";
            this.textBox_ObsFilePath.Size = new System.Drawing.Size(344, 21);
            this.textBox_ObsFilePath.TabIndex = 4;
            // 
            // button_ObsFilePath
            // 
            this.button_ObsFilePath.Location = new System.Drawing.Point(498, 95);
            this.button_ObsFilePath.Name = "button_ObsFilePath";
            this.button_ObsFilePath.Size = new System.Drawing.Size(75, 23);
            this.button_ObsFilePath.TabIndex = 5;
            this.button_ObsFilePath.Text = "…";
            this.button_ObsFilePath.UseVisualStyleBackColor = true;
            this.button_ObsFilePath.Click += new System.EventHandler(this.button_ObsFilePath_Click);
            // 
            // button_extract
            // 
            this.button_extract.Location = new System.Drawing.Point(498, 148);
            this.button_extract.Name = "button_extract";
            this.button_extract.Size = new System.Drawing.Size(75, 23);
            this.button_extract.TabIndex = 6;
            this.button_extract.Text = "筛选";
            this.button_extract.UseVisualStyleBackColor = true;
            this.button_extract.Click += new System.EventHandler(this.button_extract_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(116, 148);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(344, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "处理进度：";
            // 
            // ExtractSinexSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 183);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_extract);
            this.Controls.Add(this.button_ObsFilePath);
            this.Controls.Add(this.textBox_ObsFilePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_SinexFile);
            this.Controls.Add(this.textBox_SinexFile);
            this.Controls.Add(this.label1);
            this.Name = "ExtractSinexSite";
            this.Text = "ExtractSinexSite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_SinexFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_SinexFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ObsFilePath;
        private System.Windows.Forms.Button button_ObsFilePath;
        private System.Windows.Forms.Button button_extract;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
    }
}