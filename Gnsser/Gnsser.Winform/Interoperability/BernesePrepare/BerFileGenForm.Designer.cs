namespace Gnsser.Winform
{
    partial class BerFileGenForm
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
            this.button_setStaPath = new System.Windows.Forms.Button();
            this.button_setODirPath = new System.Windows.Forms.Button();
            this.textBox_outPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_dir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox_gen = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_setStaPath);
            this.groupBox1.Controls.Add(this.button_setODirPath);
            this.groupBox1.Controls.Add(this.textBox_outPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_dir);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(665, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // button_setStaPath
            // 
            this.button_setStaPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setStaPath.Location = new System.Drawing.Point(612, 45);
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
            this.button_setODirPath.Location = new System.Drawing.Point(612, 19);
            this.button_setODirPath.Name = "button_setODirPath";
            this.button_setODirPath.Size = new System.Drawing.Size(47, 23);
            this.button_setODirPath.TabIndex = 2;
            this.button_setODirPath.Text = "...";
            this.button_setODirPath.UseVisualStyleBackColor = true;
            this.button_setODirPath.Click += new System.EventHandler(this.button_setODirPath_Click);
            // 
            // textBox_outPath
            // 
            this.textBox_outPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_outPath.Location = new System.Drawing.Point(95, 48);
            this.textBox_outPath.Name = "textBox_outPath";
            this.textBox_outPath.Size = new System.Drawing.Size(511, 21);
            this.textBox_outPath.TabIndex = 1;
            this.textBox_outPath.Text = "C:\\EXAMPLE.ABB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "生成文件地址：|";
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
            this.label1.Location = new System.Drawing.Point(19, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "测站目录：";
            // 
            // button_run
            // 
            this.button_run.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_run.Location = new System.Drawing.Point(609, 100);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(69, 26);
            this.button_run.TabIndex = 1;
            this.button_run.Text = "生成";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "ABB文件(*.ABB)|*.ABB|所有文件|*.*";
            // 
            // textBox_gen
            // 
            this.textBox_gen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_gen.Location = new System.Drawing.Point(12, 132);
            this.textBox_gen.Multiline = true;
            this.textBox_gen.Name = "textBox_gen";
            this.textBox_gen.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_gen.Size = new System.Drawing.Size(660, 375);
            this.textBox_gen.TabIndex = 1;
            // 
            // BerFileGenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 519);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_gen);
            this.Controls.Add(this.button_run);
            this.Name = "BerAbbGenForm";
            this.Text = "ABB文件生成";
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
        private System.Windows.Forms.TextBox textBox_outPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_dir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox_gen;
    }
}