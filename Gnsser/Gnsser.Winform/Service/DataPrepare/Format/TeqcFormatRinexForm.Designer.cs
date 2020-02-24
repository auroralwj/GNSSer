namespace Gnsser.Winform
{
    partial class TeqcFormatRinexForm
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
            this.checkBox_delOrigin = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.button_setOutPath = new System.Windows.Forms.Button();
            this.textBox_savePath = new System.Windows.Forms.TextBox();
            this.checkBox_subIncluded = new System.Windows.Forms.CheckBox();
            this.textBox_inDirPath = new System.Windows.Forms.TextBox();
            this.button_setInDirPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_info = new System.Windows.Forms.Label();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.checkBox_output = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBox_delOrigin);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.checkBox_subIncluded);
            this.groupBox1.Controls.Add(this.textBox_inDirPath);
            this.groupBox1.Controls.Add(this.button_setInDirPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // checkBox_delOrigin
            // 
            this.checkBox_delOrigin.AutoSize = true;
            this.checkBox_delOrigin.Location = new System.Drawing.Point(221, 83);
            this.checkBox_delOrigin.Name = "checkBox_delOrigin";
            this.checkBox_delOrigin.Size = new System.Drawing.Size(84, 16);
            this.checkBox_delOrigin.TabIndex = 4;
            this.checkBox_delOrigin.Text = "删除源文件";
            this.checkBox_delOrigin.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button_setOutPath);
            this.panel1.Controls.Add(this.textBox_savePath);
            this.panel1.Location = new System.Drawing.Point(20, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 30);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "输出文件夹：";
            // 
            // button_setOutPath
            // 
            this.button_setOutPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setOutPath.Location = new System.Drawing.Point(574, 0);
            this.button_setOutPath.Name = "button_setOutPath";
            this.button_setOutPath.Size = new System.Drawing.Size(55, 23);
            this.button_setOutPath.TabIndex = 1;
            this.button_setOutPath.Text = "...";
            this.button_setOutPath.UseVisualStyleBackColor = true;
            this.button_setOutPath.Click += new System.EventHandler(this.button_setOutPath_Click);
            // 
            // textBox_savePath
            // 
            this.textBox_savePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_savePath.Location = new System.Drawing.Point(81, 3);
            this.textBox_savePath.Name = "textBox_savePath";
            this.textBox_savePath.Size = new System.Drawing.Size(487, 21);
            this.textBox_savePath.TabIndex = 1;
            this.textBox_savePath.Text = "D:\\Temp\\Formated";
            // 
            // checkBox_subIncluded
            // 
            this.checkBox_subIncluded.AutoSize = true;
            this.checkBox_subIncluded.Location = new System.Drawing.Point(101, 84);
            this.checkBox_subIncluded.Name = "checkBox_subIncluded";
            this.checkBox_subIncluded.Size = new System.Drawing.Size(96, 16);
            this.checkBox_subIncluded.TabIndex = 2;
            this.checkBox_subIncluded.Text = "包含子文件夹";
            this.checkBox_subIncluded.UseVisualStyleBackColor = true;
            // 
            // textBox_inDirPath
            // 
            this.textBox_inDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_inDirPath.Location = new System.Drawing.Point(101, 21);
            this.textBox_inDirPath.Name = "textBox_inDirPath";
            this.textBox_inDirPath.Size = new System.Drawing.Size(485, 21);
            this.textBox_inDirPath.TabIndex = 1;
            this.textBox_inDirPath.Text = "D:\\Temp";
            this.textBox_inDirPath.TextChanged += new System.EventHandler(this.textBox_inDirPath_TextChanged);
            // 
            // button_setInDirPath
            // 
            this.button_setInDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setInDirPath.Location = new System.Drawing.Point(597, 19);
            this.button_setInDirPath.Name = "button_setInDirPath";
            this.button_setInDirPath.Size = new System.Drawing.Size(55, 23);
            this.button_setInDirPath.TabIndex = 1;
            this.button_setInDirPath.Text = "...";
            this.button_setInDirPath.UseVisualStyleBackColor = true;
            this.button_setInDirPath.Click += new System.EventHandler(this.button_setInDirPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入文件夹：";
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(530, 124);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 36);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "开始";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(613, 124);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 36);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "关闭";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 127);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(510, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // label_info
            // 
            this.label_info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(12, 155);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(29, 12);
            this.label_info.TabIndex = 4;
            this.label_info.Text = "信息";
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.BackColor = System.Drawing.Color.White;
            this.textBox_result.ForeColor = System.Drawing.Color.RoyalBlue;
            this.textBox_result.Location = new System.Drawing.Point(14, 195);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(674, 285);
            this.textBox_result.TabIndex = 5;
            this.textBox_result.Text = "信息输出";
            // 
            // checkBox_output
            // 
            this.checkBox_output.AutoSize = true;
            this.checkBox_output.Checked = true;
            this.checkBox_output.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_output.Location = new System.Drawing.Point(14, 173);
            this.checkBox_output.Name = "checkBox_output";
            this.checkBox_output.Size = new System.Drawing.Size(96, 16);
            this.checkBox_output.TabIndex = 6;
            this.checkBox_output.Text = "同步输出结果";
            this.checkBox_output.UseVisualStyleBackColor = true;
            // 
            // TeqcFormatRinexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 492);
            this.Controls.Add(this.checkBox_output);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.label_info);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "TeqcFormatRinexForm";
            this.Text = "Teqc格式化Rinex";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_setInDirPath;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_inDirPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox_subIncluded;
        private System.Windows.Forms.TextBox textBox_savePath;
        private System.Windows.Forms.Button button_setOutPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.CheckBox checkBox_output;
        private System.Windows.Forms.CheckBox checkBox_delOrigin;
    }
}