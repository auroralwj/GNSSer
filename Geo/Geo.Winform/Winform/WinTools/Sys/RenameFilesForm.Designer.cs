namespace Geo.WinTools.Sys
{
    partial class RenameFilesForm
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
            this.button_renameSuffix = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_applyToSubDirs = new System.Windows.Forms.CheckBox();
            this.button_setdirpath = new System.Windows.Forms.Button();
            this.textBox_dirPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_sourseSuffix = new System.Windows.Forms.ComboBox();
            this.comboBox_targetSuffix = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_renameName = new System.Windows.Forms.Button();
            this.label_example = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_filePrefixName = new System.Windows.Forms.TextBox();
            this.comboBox_spaceMark = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_process_info = new System.Windows.Forms.Label();
            this.checkBox_replaceExist = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_renameSuffix
            // 
            this.button_renameSuffix.Location = new System.Drawing.Point(521, 16);
            this.button_renameSuffix.Name = "button_renameSuffix";
            this.button_renameSuffix.Size = new System.Drawing.Size(75, 23);
            this.button_renameSuffix.TabIndex = 0;
            this.button_renameSuffix.Text = "执行";
            this.button_renameSuffix.UseVisualStyleBackColor = true;
            this.button_renameSuffix.Click += new System.EventHandler(this.button_RenameSuffix_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_replaceExist);
            this.groupBox1.Controls.Add(this.checkBox_applyToSubDirs);
            this.groupBox1.Controls.Add(this.button_setdirpath);
            this.groupBox1.Controls.Add(this.textBox_dirPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(608, 82);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // checkBox_applyToSubDirs
            // 
            this.checkBox_applyToSubDirs.AutoSize = true;
            this.checkBox_applyToSubDirs.Location = new System.Drawing.Point(84, 47);
            this.checkBox_applyToSubDirs.Name = "checkBox_applyToSubDirs";
            this.checkBox_applyToSubDirs.Size = new System.Drawing.Size(108, 16);
            this.checkBox_applyToSubDirs.TabIndex = 3;
            this.checkBox_applyToSubDirs.Text = "应用于子文件夹";
            this.checkBox_applyToSubDirs.UseVisualStyleBackColor = true;
            // 
            // button_setdirpath
            // 
            this.button_setdirpath.Location = new System.Drawing.Point(564, 20);
            this.button_setdirpath.Name = "button_setdirpath";
            this.button_setdirpath.Size = new System.Drawing.Size(38, 23);
            this.button_setdirpath.TabIndex = 2;
            this.button_setdirpath.Text = "...";
            this.button_setdirpath.UseVisualStyleBackColor = true;
            this.button_setdirpath.Click += new System.EventHandler(this.button_setdirpath_Click);
            // 
            // textBox_dirPath
            // 
            this.textBox_dirPath.Location = new System.Drawing.Point(84, 20);
            this.textBox_dirPath.Name = "textBox_dirPath";
            this.textBox_dirPath.Size = new System.Drawing.Size(474, 21);
            this.textBox_dirPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件夹：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "改名后的文件";
            // 
            // comboBox_sourseSuffix
            // 
            this.comboBox_sourseSuffix.FormattingEnabled = true;
            this.comboBox_sourseSuffix.Items.AddRange(new object[] {
            ".jpg",
            ".png",
            ".txt",
            ".pdf"});
            this.comboBox_sourseSuffix.Location = new System.Drawing.Point(127, 17);
            this.comboBox_sourseSuffix.Name = "comboBox_sourseSuffix";
            this.comboBox_sourseSuffix.Size = new System.Drawing.Size(52, 20);
            this.comboBox_sourseSuffix.TabIndex = 3;
            this.comboBox_sourseSuffix.Text = ".png";
            // 
            // comboBox_targetSuffix
            // 
            this.comboBox_targetSuffix.FormattingEnabled = true;
            this.comboBox_targetSuffix.Items.AddRange(new object[] {
            ".jpg",
            ".png",
            ".txt",
            ".pdf"});
            this.comboBox_targetSuffix.Location = new System.Drawing.Point(215, 17);
            this.comboBox_targetSuffix.Name = "comboBox_targetSuffix";
            this.comboBox_targetSuffix.Size = new System.Drawing.Size(47, 20);
            this.comboBox_targetSuffix.TabIndex = 4;
            this.comboBox_targetSuffix.Text = ".jpg";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "改为";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBox_targetSuffix);
            this.groupBox2.Controls.Add(this.comboBox_sourseSuffix);
            this.groupBox2.Controls.Add(this.button_renameSuffix);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(602, 44);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "修改后缀名";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "将后缀名";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_renameName);
            this.groupBox3.Controls.Add(this.label_example);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.textBox_filePrefixName);
            this.groupBox3.Controls.Add(this.comboBox_spaceMark);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(12, 167);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(602, 66);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "修改文件名";
            // 
            // button_renameName
            // 
            this.button_renameName.Location = new System.Drawing.Point(521, 14);
            this.button_renameName.Name = "button_renameName";
            this.button_renameName.Size = new System.Drawing.Size(75, 23);
            this.button_renameName.TabIndex = 7;
            this.button_renameName.Text = "执行";
            this.button_renameName.UseVisualStyleBackColor = true;
            this.button_renameName.Click += new System.EventHandler(this.button_renameName_Click);
            // 
            // label_example
            // 
            this.label_example.AutoSize = true;
            this.label_example.Location = new System.Drawing.Point(25, 42);
            this.label_example.Name = "label_example";
            this.label_example.Size = new System.Drawing.Size(29, 12);
            this.label_example.TabIndex = 6;
            this.label_example.Text = "例子";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(281, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "数字";
            // 
            // textBox_filePrefixName
            // 
            this.textBox_filePrefixName.Location = new System.Drawing.Point(108, 15);
            this.textBox_filePrefixName.Name = "textBox_filePrefixName";
            this.textBox_filePrefixName.Size = new System.Drawing.Size(67, 21);
            this.textBox_filePrefixName.TabIndex = 1;
            this.textBox_filePrefixName.TextChanged += new System.EventHandler(this.filePrefixName_TextChanged);
            // 
            // comboBox_spaceMark
            // 
            this.comboBox_spaceMark.FormattingEnabled = true;
            this.comboBox_spaceMark.Items.AddRange(new object[] {
            "-",
            "-"});
            this.comboBox_spaceMark.Location = new System.Drawing.Point(228, 15);
            this.comboBox_spaceMark.Name = "comboBox_spaceMark";
            this.comboBox_spaceMark.Size = new System.Drawing.Size(47, 20);
            this.comboBox_spaceMark.TabIndex = 4;
            this.comboBox_spaceMark.Text = "_";
            this.comboBox_spaceMark.TextChanged += new System.EventHandler(this.filePrefixName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "间隔符";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 276);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(608, 16);
            this.progressBar1.TabIndex = 4;
            // 
            // label_process_info
            // 
            this.label_process_info.AutoSize = true;
            this.label_process_info.Location = new System.Drawing.Point(12, 257);
            this.label_process_info.Name = "label_process_info";
            this.label_process_info.Size = new System.Drawing.Size(53, 12);
            this.label_process_info.TabIndex = 5;
            this.label_process_info.Text = "当前进程";
            // 
            // checkBox_replaceExist
            // 
            this.checkBox_replaceExist.AutoSize = true;
            this.checkBox_replaceExist.Location = new System.Drawing.Point(228, 47);
            this.checkBox_replaceExist.Name = "checkBox_replaceExist";
            this.checkBox_replaceExist.Size = new System.Drawing.Size(108, 16);
            this.checkBox_replaceExist.TabIndex = 3;
            this.checkBox_replaceExist.Text = "替换已存在文件";
            this.checkBox_replaceExist.UseVisualStyleBackColor = true;
            // 
            // RenameFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 304);
            this.Controls.Add(this.label_process_info);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RenameFilesForm";
            this.ShowIcon = false;
            this.Text = "批量命名文件";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_renameSuffix;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_setdirpath;
        private System.Windows.Forms.TextBox textBox_dirPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_sourseSuffix;
        private System.Windows.Forms.ComboBox comboBox_targetSuffix;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_filePrefixName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_spaceMark;
        private System.Windows.Forms.Label label_example;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_renameName;
        private System.Windows.Forms.CheckBox checkBox_applyToSubDirs;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_process_info;
        private System.Windows.Forms.CheckBox checkBox_replaceExist;
    }
}