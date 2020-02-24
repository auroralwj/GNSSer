namespace Gnsser.Winform.Other
{
    partial class CombinationSatNL
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
            this.textBox_dir = new System.Windows.Forms.TextBox();
            this.button_SelectDir = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_OutputDir = new System.Windows.Forms.TextBox();
            this.button_SelectOutputPath = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_extract = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.button_delete = new System.Windows.Forms.Button();
            this.textBox_maxvalue = new System.Windows.Forms.TextBox();
            this.textBox_minvalue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_PRN = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "目标文件夹：";
            // 
            // textBox_dir
            // 
            this.textBox_dir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dir.Location = new System.Drawing.Point(112, 53);
            this.textBox_dir.Name = "textBox_dir";
            this.textBox_dir.Size = new System.Drawing.Size(380, 21);
            this.textBox_dir.TabIndex = 1;
            // 
            // button_SelectDir
            // 
            this.button_SelectDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SelectDir.Location = new System.Drawing.Point(520, 53);
            this.button_SelectDir.Name = "button_SelectDir";
            this.button_SelectDir.Size = new System.Drawing.Size(75, 23);
            this.button_SelectDir.TabIndex = 2;
            this.button_SelectDir.Text = "……";
            this.button_SelectDir.UseVisualStyleBackColor = true;
            this.button_SelectDir.Click += new System.EventHandler(this.button_SelectDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "输出目录：";
            // 
            // textBox_OutputDir
            // 
            this.textBox_OutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_OutputDir.Location = new System.Drawing.Point(112, 108);
            this.textBox_OutputDir.Name = "textBox_OutputDir";
            this.textBox_OutputDir.Size = new System.Drawing.Size(380, 21);
            this.textBox_OutputDir.TabIndex = 4;
            // 
            // button_SelectOutputPath
            // 
            this.button_SelectOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SelectOutputPath.Location = new System.Drawing.Point(520, 108);
            this.button_SelectOutputPath.Name = "button_SelectOutputPath";
            this.button_SelectOutputPath.Size = new System.Drawing.Size(75, 23);
            this.button_SelectOutputPath.TabIndex = 5;
            this.button_SelectOutputPath.Text = "……";
            this.button_SelectOutputPath.UseVisualStyleBackColor = true;
            this.button_SelectOutputPath.Click += new System.EventHandler(this.button_SelectOutputPath_Click);
            // 
            // button_extract
            // 
            this.button_extract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_extract.Location = new System.Drawing.Point(520, 170);
            this.button_extract.Name = "button_extract";
            this.button_extract.Size = new System.Drawing.Size(75, 23);
            this.button_extract.TabIndex = 6;
            this.button_extract.Text = "提取";
            this.button_extract.UseVisualStyleBackColor = true;
            this.button_extract.Click += new System.EventHandler(this.button_extract_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(112, 170);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(380, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "处理进度：";
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(520, 233);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(75, 23);
            this.button_delete.TabIndex = 13;
            this.button_delete.Text = "删除";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // textBox_maxvalue
            // 
            this.textBox_maxvalue.Location = new System.Drawing.Point(392, 235);
            this.textBox_maxvalue.Name = "textBox_maxvalue";
            this.textBox_maxvalue.Size = new System.Drawing.Size(100, 21);
            this.textBox_maxvalue.TabIndex = 12;
            // 
            // textBox_minvalue
            // 
            this.textBox_minvalue.Location = new System.Drawing.Point(234, 235);
            this.textBox_minvalue.Name = "textBox_minvalue";
            this.textBox_minvalue.Size = new System.Drawing.Size(100, 21);
            this.textBox_minvalue.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(351, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "Max：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(193, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Min：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "PRN";
            // 
            // textBox_PRN
            // 
            this.textBox_PRN.Location = new System.Drawing.Point(78, 235);
            this.textBox_PRN.Name = "textBox_PRN";
            this.textBox_PRN.Size = new System.Drawing.Size(100, 21);
            this.textBox_PRN.TabIndex = 15;
            // 
            // CombinationSatNL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 324);
            this.Controls.Add(this.textBox_PRN);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.textBox_maxvalue);
            this.Controls.Add(this.textBox_minvalue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_extract);
            this.Controls.Add(this.button_SelectOutputPath);
            this.Controls.Add(this.textBox_OutputDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_SelectDir);
            this.Controls.Add(this.textBox_dir);
            this.Controls.Add(this.label1);
            this.Name = "CombinationSatNL";
            this.Text = "CombinationSatNL";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_dir;
        private System.Windows.Forms.Button button_SelectDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_OutputDir;
        private System.Windows.Forms.Button button_SelectOutputPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Button button_extract;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.TextBox textBox_maxvalue;
        private System.Windows.Forms.TextBox textBox_minvalue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_PRN;
    }
}