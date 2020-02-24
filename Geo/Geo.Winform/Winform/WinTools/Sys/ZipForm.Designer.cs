namespace Geo.WinTools.Sys
{
    partial class ZipForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_SetDestZip = new System.Windows.Forms.Button();
            this.button_compress = new System.Windows.Forms.Button();
            this.button_setSourseDirPath = new System.Windows.Forms.Button();
            this.textBox_destZipDir = new System.Windows.Forms.TextBox();
            this.textBox_sourseDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_setDestDir = new System.Windows.Forms.Button();
            this.button_decomress = new System.Windows.Forms.Button();
            this.button_setSourseZip = new System.Windows.Forms.Button();
            this.textBox_destDir = new System.Windows.Forms.TextBox();
            this.textBox_sourseZip = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox_deleSouse = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button_mulit_decompress = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox_source_dir = new System.Windows.Forms.TextBox();
            this.textBox_dest_dir = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_enablePass = new System.Windows.Forms.CheckBox();
            this.textBox_pass = new System.Windows.Forms.TextBox();
            this.checkBox_ignoreError = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_SetDestZip
            // 
            this.button_SetDestZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SetDestZip.Location = new System.Drawing.Point(410, 40);
            this.button_SetDestZip.Name = "button_SetDestZip";
            this.button_SetDestZip.Size = new System.Drawing.Size(32, 23);
            this.button_SetDestZip.TabIndex = 6;
            this.button_SetDestZip.Text = "...";
            this.button_SetDestZip.UseVisualStyleBackColor = true;
            this.button_SetDestZip.Click += new System.EventHandler(this.button_SetDestZip_Click);
            // 
            // button_compress
            // 
            this.button_compress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_compress.Location = new System.Drawing.Point(459, 38);
            this.button_compress.Name = "button_compress";
            this.button_compress.Size = new System.Drawing.Size(75, 23);
            this.button_compress.TabIndex = 5;
            this.button_compress.Text = "压缩";
            this.button_compress.UseVisualStyleBackColor = true;
            this.button_compress.Click += new System.EventHandler(this.button_comress_Click);
            // 
            // button_setSourseDirPath
            // 
            this.button_setSourseDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setSourseDirPath.Location = new System.Drawing.Point(409, 14);
            this.button_setSourseDirPath.Name = "button_setSourseDirPath";
            this.button_setSourseDirPath.Size = new System.Drawing.Size(33, 23);
            this.button_setSourseDirPath.TabIndex = 4;
            this.button_setSourseDirPath.Text = "...";
            this.button_setSourseDirPath.UseVisualStyleBackColor = true;
            this.button_setSourseDirPath.Click += new System.EventHandler(this.button_setSourseDirPath_Click);
            // 
            // textBox_destZipDir
            // 
            this.textBox_destZipDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_destZipDir.Location = new System.Drawing.Point(75, 40);
            this.textBox_destZipDir.Name = "textBox_destZipDir";
            this.textBox_destZipDir.Size = new System.Drawing.Size(328, 21);
            this.textBox_destZipDir.TabIndex = 3;
            // 
            // textBox_sourseDir
            // 
            this.textBox_sourseDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sourseDir.Location = new System.Drawing.Point(75, 14);
            this.textBox_sourseDir.Name = "textBox_sourseDir";
            this.textBox_sourseDir.Size = new System.Drawing.Size(328, 21);
            this.textBox_sourseDir.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "目标：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "源目录：";
            // 
            // button_setDestDir
            // 
            this.button_setDestDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setDestDir.Location = new System.Drawing.Point(401, 40);
            this.button_setDestDir.Name = "button_setDestDir";
            this.button_setDestDir.Size = new System.Drawing.Size(32, 23);
            this.button_setDestDir.TabIndex = 6;
            this.button_setDestDir.Text = "...";
            this.button_setDestDir.UseVisualStyleBackColor = true;
            this.button_setDestDir.Click += new System.EventHandler(this.button_setDestDir_Click);
            // 
            // button_decomress
            // 
            this.button_decomress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_decomress.Location = new System.Drawing.Point(449, 41);
            this.button_decomress.Name = "button_decomress";
            this.button_decomress.Size = new System.Drawing.Size(75, 23);
            this.button_decomress.TabIndex = 5;
            this.button_decomress.Text = "解压";
            this.button_decomress.UseVisualStyleBackColor = true;
            this.button_decomress.Click += new System.EventHandler(this.button_decompress_Click);
            // 
            // button_setSourseZip
            // 
            this.button_setSourseZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setSourseZip.Location = new System.Drawing.Point(399, 17);
            this.button_setSourseZip.Name = "button_setSourseZip";
            this.button_setSourseZip.Size = new System.Drawing.Size(33, 23);
            this.button_setSourseZip.TabIndex = 4;
            this.button_setSourseZip.Text = "...";
            this.button_setSourseZip.UseVisualStyleBackColor = true;
            this.button_setSourseZip.Click += new System.EventHandler(this.button_setSourseZip_Click);
            // 
            // textBox_destDir
            // 
            this.textBox_destDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_destDir.Location = new System.Drawing.Point(65, 43);
            this.textBox_destDir.Name = "textBox_destDir";
            this.textBox_destDir.Size = new System.Drawing.Size(328, 21);
            this.textBox_destDir.TabIndex = 3;
            // 
            // textBox_sourseZip
            // 
            this.textBox_sourseZip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sourseZip.Location = new System.Drawing.Point(65, 17);
            this.textBox_sourseZip.Name = "textBox_sourseZip";
            this.textBox_sourseZip.Size = new System.Drawing.Size(328, 21);
            this.textBox_sourseZip.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "目标：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "源文件：";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Zip 文件|*.zip";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Zip 文件|*.zip;*.z|所有文件|*.*";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(556, 114);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox_deleSouse);
            this.tabPage3.Controls.Add(this.progressBar1);
            this.tabPage3.Controls.Add(this.button_mulit_decompress);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.textBox_source_dir);
            this.tabPage3.Controls.Add(this.textBox_dest_dir);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(548, 88);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "批量解压";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox_deleSouse
            // 
            this.checkBox_deleSouse.AutoSize = true;
            this.checkBox_deleSouse.Location = new System.Drawing.Point(456, 66);
            this.checkBox_deleSouse.Name = "checkBox_deleSouse";
            this.checkBox_deleSouse.Size = new System.Drawing.Size(84, 16);
            this.checkBox_deleSouse.TabIndex = 15;
            this.checkBox_deleSouse.Text = "删除原文件";
            this.checkBox_deleSouse.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(13, 67);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(423, 10);
            this.progressBar1.TabIndex = 14;
            // 
            // button_mulit_decompress
            // 
            this.button_mulit_decompress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_mulit_decompress.Location = new System.Drawing.Point(456, 38);
            this.button_mulit_decompress.Name = "button_mulit_decompress";
            this.button_mulit_decompress.Size = new System.Drawing.Size(75, 23);
            this.button_mulit_decompress.TabIndex = 13;
            this.button_mulit_decompress.Text = "解压";
            this.button_mulit_decompress.UseVisualStyleBackColor = true;
            this.button_mulit_decompress.Click += new System.EventHandler(this.button_mulit_decompress_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(405, 38);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_setDestDir_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "源目录：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "目标：";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(404, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button_setSourseDirPath_Click);
            // 
            // textBox_source_dir
            // 
            this.textBox_source_dir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_source_dir.Location = new System.Drawing.Point(70, 12);
            this.textBox_source_dir.Name = "textBox_source_dir";
            this.textBox_source_dir.Size = new System.Drawing.Size(328, 21);
            this.textBox_source_dir.TabIndex = 9;
            // 
            // textBox_dest_dir
            // 
            this.textBox_dest_dir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dest_dir.Location = new System.Drawing.Point(70, 38);
            this.textBox_dest_dir.Name = "textBox_dest_dir";
            this.textBox_dest_dir.Size = new System.Drawing.Size(328, 21);
            this.textBox_dest_dir.TabIndex = 10;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button_setDestDir);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.button_decomress);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.button_setSourseZip);
            this.tabPage2.Controls.Add(this.textBox_sourseZip);
            this.tabPage2.Controls.Add(this.textBox_destDir);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(548, 88);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "解压缩";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button_SetDestZip);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button_compress);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.button_setSourseDirPath);
            this.tabPage1.Controls.Add(this.textBox_sourseDir);
            this.tabPage1.Controls.Add(this.textBox_destZipDir);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(548, 88);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "压缩";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_enablePass
            // 
            this.checkBox_enablePass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_enablePass.AutoSize = true;
            this.checkBox_enablePass.Location = new System.Drawing.Point(48, 133);
            this.checkBox_enablePass.Name = "checkBox_enablePass";
            this.checkBox_enablePass.Size = new System.Drawing.Size(72, 16);
            this.checkBox_enablePass.TabIndex = 3;
            this.checkBox_enablePass.Text = "启用密码";
            this.checkBox_enablePass.UseVisualStyleBackColor = true;
            this.checkBox_enablePass.CheckedChanged += new System.EventHandler(this.checkBox_enablePass_CheckedChanged);
            // 
            // textBox_pass
            // 
            this.textBox_pass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_pass.Location = new System.Drawing.Point(126, 131);
            this.textBox_pass.Name = "textBox_pass";
            this.textBox_pass.Size = new System.Drawing.Size(100, 21);
            this.textBox_pass.TabIndex = 4;
            this.textBox_pass.Visible = false;
            // 
            // checkBox_ignoreError
            // 
            this.checkBox_ignoreError.AutoSize = true;
            this.checkBox_ignoreError.Location = new System.Drawing.Point(261, 135);
            this.checkBox_ignoreError.Name = "checkBox_ignoreError";
            this.checkBox_ignoreError.Size = new System.Drawing.Size(72, 16);
            this.checkBox_ignoreError.TabIndex = 5;
            this.checkBox_ignoreError.Text = "忽略错误";
            this.checkBox_ignoreError.UseVisualStyleBackColor = true;
            // 
            // ZipForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 160);
            this.Controls.Add(this.checkBox_ignoreError);
            this.Controls.Add(this.textBox_pass);
            this.Controls.Add(this.checkBox_enablePass);
            this.Controls.Add(this.tabControl1);
            this.Name = "ZipForm";
            this.Text = "压缩解压缩";
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_compress;
        private System.Windows.Forms.Button button_setSourseDirPath;
        private System.Windows.Forms.TextBox textBox_destZipDir;
        private System.Windows.Forms.TextBox textBox_sourseDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_decomress;
        private System.Windows.Forms.Button button_setSourseZip;
        private System.Windows.Forms.TextBox textBox_destDir;
        private System.Windows.Forms.TextBox textBox_sourseZip;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button_SetDestZip;
        private System.Windows.Forms.Button button_setDestDir;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBox_enablePass;
        private System.Windows.Forms.TextBox textBox_pass;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button_mulit_decompress;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_source_dir;
        private System.Windows.Forms.TextBox textBox_dest_dir;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox_deleSouse;
        private System.Windows.Forms.CheckBox checkBox_ignoreError;
    }
}