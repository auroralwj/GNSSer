namespace Geo.Winform
{
    partial class CheckSameFileForm
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
            this.button_setPathA = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_pathB = new System.Windows.Forms.TextBox();
            this.button_setPathB = new System.Windows.Forms.Button();
            this.textBox_pathA = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_compareDir = new System.Windows.Forms.Button();
            this.button_compareFile = new System.Windows.Forms.Button();
            this.checkBox_copyDir1ToDir2 = new System.Windows.Forms.CheckBox();
            this.checkBox_copyFile1ToFile2 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_setPathA
            // 
            this.button_setPathA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setPathA.Location = new System.Drawing.Point(478, 20);
            this.button_setPathA.Name = "button_setPathA";
            this.button_setPathA.Size = new System.Drawing.Size(42, 23);
            this.button_setPathA.TabIndex = 0;
            this.button_setPathA.Text = "...";
            this.button_setPathA.UseVisualStyleBackColor = true;
            this.button_setPathA.Click += new System.EventHandler(this.button_setPathA_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "目录1：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBox_pathB);
            this.groupBox1.Controls.Add(this.button_setPathB);
            this.groupBox1.Controls.Add(this.textBox_pathA);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button_setPathA);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 83);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息：";
            // 
            // textBox_pathB
            // 
            this.textBox_pathB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pathB.Location = new System.Drawing.Point(53, 47);
            this.textBox_pathB.Name = "textBox_pathB";
            this.textBox_pathB.Size = new System.Drawing.Size(419, 21);
            this.textBox_pathB.TabIndex = 2;
            this.textBox_pathB.Text = "E:\\[Projects]SZ";
            // 
            // button_setPathB
            // 
            this.button_setPathB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setPathB.Location = new System.Drawing.Point(478, 47);
            this.button_setPathB.Name = "button_setPathB";
            this.button_setPathB.Size = new System.Drawing.Size(42, 23);
            this.button_setPathB.TabIndex = 0;
            this.button_setPathB.Text = "...";
            this.button_setPathB.UseVisualStyleBackColor = true;
            this.button_setPathB.Click += new System.EventHandler(this.button_setPathB_Click);
            // 
            // textBox_pathA
            // 
            this.textBox_pathA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_pathA.Location = new System.Drawing.Point(53, 20);
            this.textBox_pathA.Name = "textBox_pathA";
            this.textBox_pathA.Size = new System.Drawing.Size(419, 21);
            this.textBox_pathA.TabIndex = 2;
            this.textBox_pathA.Text = "E:\\[Projects]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "目录2：";
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.Location = new System.Drawing.Point(13, 154);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(525, 254);
            this.textBox_result.TabIndex = 3;
            // 
            // button_compareDir
            // 
            this.button_compareDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_compareDir.Location = new System.Drawing.Point(459, 97);
            this.button_compareDir.Name = "button_compareDir";
            this.button_compareDir.Size = new System.Drawing.Size(79, 23);
            this.button_compareDir.TabIndex = 4;
            this.button_compareDir.Text = "比较文件夹";
            this.button_compareDir.UseVisualStyleBackColor = true;
            this.button_compareDir.Click += new System.EventHandler(this.button_compareDir_Click);
            // 
            // button_compareFile
            // 
            this.button_compareFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_compareFile.Location = new System.Drawing.Point(459, 128);
            this.button_compareFile.Name = "button_compareFile";
            this.button_compareFile.Size = new System.Drawing.Size(79, 23);
            this.button_compareFile.TabIndex = 4;
            this.button_compareFile.Text = "比较文件";
            this.button_compareFile.UseVisualStyleBackColor = true;
            this.button_compareFile.Click += new System.EventHandler(this.button_compareFile_Click);
            // 
            // checkBox_copyDir1ToDir2
            // 
            this.checkBox_copyDir1ToDir2.AutoSize = true;
            this.checkBox_copyDir1ToDir2.Location = new System.Drawing.Point(20, 101);
            this.checkBox_copyDir1ToDir2.Name = "checkBox_copyDir1ToDir2";
            this.checkBox_copyDir1ToDir2.Size = new System.Drawing.Size(216, 16);
            this.checkBox_copyDir1ToDir2.TabIndex = 5;
            this.checkBox_copyDir1ToDir2.Text = "将目录1中的不同子目录复制到目录2";
            this.checkBox_copyDir1ToDir2.UseVisualStyleBackColor = true;
            // 
            // checkBox_copyFile1ToFile2
            // 
            this.checkBox_copyFile1ToFile2.AutoSize = true;
            this.checkBox_copyFile1ToFile2.Location = new System.Drawing.Point(20, 128);
            this.checkBox_copyFile1ToFile2.Name = "checkBox_copyFile1ToFile2";
            this.checkBox_copyFile1ToFile2.Size = new System.Drawing.Size(204, 16);
            this.checkBox_copyFile1ToFile2.TabIndex = 5;
            this.checkBox_copyFile1ToFile2.Text = "将目录1中的不同文件复制到目录2";
            this.checkBox_copyFile1ToFile2.UseVisualStyleBackColor = true;
            // 
            // CheckSameFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 420);
            this.Controls.Add(this.checkBox_copyFile1ToFile2);
            this.Controls.Add(this.checkBox_copyDir1ToDir2);
            this.Controls.Add(this.button_compareFile);
            this.Controls.Add(this.button_compareDir);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.groupBox1);
            this.Name = "CheckSameFileForm";
            this.Text = "文件比较";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_setPathA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_pathB;
        private System.Windows.Forms.Button button_setPathB;
        private System.Windows.Forms.TextBox textBox_pathA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_compareDir;
        private System.Windows.Forms.Button button_compareFile;
        private System.Windows.Forms.CheckBox checkBox_copyDir1ToDir2;
        private System.Windows.Forms.CheckBox checkBox_copyFile1ToFile2;
    }
}

