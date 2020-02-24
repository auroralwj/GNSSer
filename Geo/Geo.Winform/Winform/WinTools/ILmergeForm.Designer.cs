namespace Geo.WinTools
{
    partial class ILmergeForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_enableExe = new System.Windows.Forms.CheckBox();
            this.button_setoutputpath = new System.Windows.Forms.Button();
            this.button_setExePath = new System.Windows.Forms.Button();
            this.button_setDllDir = new System.Windows.Forms.Button();
            this.textBox1_outputpath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_exePath = new System.Windows.Forms.TextBox();
            this.textBox_dllDir = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_run = new System.Windows.Forms.Button();
            this.textBox_results = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_enableExe);
            this.groupBox1.Controls.Add(this.button_setoutputpath);
            this.groupBox1.Controls.Add(this.button_setExePath);
            this.groupBox1.Controls.Add(this.button_setDllDir);
            this.groupBox1.Controls.Add(this.textBox1_outputpath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_exePath);
            this.groupBox1.Controls.Add(this.textBox_dllDir);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // checkBox_enableExe
            // 
            this.checkBox_enableExe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_enableExe.AutoSize = true;
            this.checkBox_enableExe.Checked = true;
            this.checkBox_enableExe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_enableExe.Location = new System.Drawing.Point(454, 49);
            this.checkBox_enableExe.Name = "checkBox_enableExe";
            this.checkBox_enableExe.Size = new System.Drawing.Size(48, 16);
            this.checkBox_enableExe.TabIndex = 3;
            this.checkBox_enableExe.Text = "启用";
            this.checkBox_enableExe.UseVisualStyleBackColor = true;
            this.checkBox_enableExe.CheckedChanged += new System.EventHandler(this.checkBox_enableExe_CheckedChanged);
            // 
            // button_setoutputpath
            // 
            this.button_setoutputpath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setoutputpath.Location = new System.Drawing.Point(454, 69);
            this.button_setoutputpath.Name = "button_setoutputpath";
            this.button_setoutputpath.Size = new System.Drawing.Size(47, 23);
            this.button_setoutputpath.TabIndex = 2;
            this.button_setoutputpath.Text = "...";
            this.button_setoutputpath.UseVisualStyleBackColor = true;
            this.button_setoutputpath.Click += new System.EventHandler(this.button_setoutputpath_Click);
            // 
            // button_setExePath
            // 
            this.button_setExePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setExePath.Location = new System.Drawing.Point(391, 43);
            this.button_setExePath.Name = "button_setExePath";
            this.button_setExePath.Size = new System.Drawing.Size(47, 23);
            this.button_setExePath.TabIndex = 2;
            this.button_setExePath.Text = "...";
            this.button_setExePath.UseVisualStyleBackColor = true;
            this.button_setExePath.Click += new System.EventHandler(this.button_setExePath_Click);
            // 
            // button_setDllDir
            // 
            this.button_setDllDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setDllDir.Location = new System.Drawing.Point(454, 15);
            this.button_setDllDir.Name = "button_setDllDir";
            this.button_setDllDir.Size = new System.Drawing.Size(47, 23);
            this.button_setDllDir.TabIndex = 2;
            this.button_setDllDir.Text = "...";
            this.button_setDllDir.UseVisualStyleBackColor = true;
            this.button_setDllDir.Click += new System.EventHandler(this.button_setDllDir_Click);
            // 
            // textBox1_outputpath
            // 
            this.textBox1_outputpath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1_outputpath.Location = new System.Drawing.Point(91, 75);
            this.textBox1_outputpath.Name = "textBox1_outputpath";
            this.textBox1_outputpath.Size = new System.Drawing.Size(357, 21);
            this.textBox1_outputpath.TabIndex = 0;
            this.textBox1_outputpath.TextChanged += new System.EventHandler(this.textBox1_outputpath_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "输出文件：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "EXE文件：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "DLL目录：";
            // 
            // textBox_exePath
            // 
            this.textBox_exePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_exePath.Location = new System.Drawing.Point(91, 45);
            this.textBox_exePath.Name = "textBox_exePath";
            this.textBox_exePath.Size = new System.Drawing.Size(294, 21);
            this.textBox_exePath.TabIndex = 0;
            // 
            // textBox_dllDir
            // 
            this.textBox_dllDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dllDir.Location = new System.Drawing.Point(91, 18);
            this.textBox_dllDir.Name = "textBox_dllDir";
            this.textBox_dllDir.Size = new System.Drawing.Size(357, 21);
            this.textBox_dllDir.TabIndex = 0;
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(439, 125);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(75, 29);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "执行";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // textBox_results
            // 
            this.textBox_results.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_results.Location = new System.Drawing.Point(19, 155);
            this.textBox_results.Multiline = true;
            this.textBox_results.Name = "textBox_results";
            this.textBox_results.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_results.Size = new System.Drawing.Size(239, 163);
            this.textBox_results.TabIndex = 0;
            this.textBox_results.TextChanged += new System.EventHandler(this.textBox1_outputpath_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(275, 160);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(239, 163);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_outputpath_TextChanged);
            // 
            // ILmergeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 330);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox_results);
            this.Name = "ILmergeForm";
            this.Text = "ILmergeForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_setDllDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_dllDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_setExePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_exePath;
        private System.Windows.Forms.Button button_setoutputpath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1_outputpath;
        private System.Windows.Forms.Button button_run;
        private System.Windows.Forms.TextBox textBox_results;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkBox_enableExe;
        private System.Windows.Forms.TextBox textBox1;
    }
}