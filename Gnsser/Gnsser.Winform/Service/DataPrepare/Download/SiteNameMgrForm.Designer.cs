namespace Gnsser.Winform
{
    partial class SiteNameMgrForm
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
            this.button_save = new System.Windows.Forms.Button();
            this.textBox_sites = new System.Windows.Forms.TextBox();
            this.button_read = new System.Windows.Forms.Button();
            this.button_setPath = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button_fromDir = new System.Windows.Forms.Button();
            this.button_trigerUpLow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(518, 92);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(117, 39);
            this.button_save.TabIndex = 14;
            this.button_save.Text = "保存到文件";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // textBox_sites
            // 
            this.textBox_sites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sites.Location = new System.Drawing.Point(93, 45);
            this.textBox_sites.Multiline = true;
            this.textBox_sites.Name = "textBox_sites";
            this.textBox_sites.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_sites.Size = new System.Drawing.Size(416, 396);
            this.textBox_sites.TabIndex = 13;
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(560, 10);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 12;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // button_setPath
            // 
            this.button_setPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setPath.Location = new System.Drawing.Point(515, 10);
            this.button_setPath.Name = "button_setPath";
            this.button_setPath.Size = new System.Drawing.Size(39, 23);
            this.button_setPath.TabIndex = 11;
            this.button_setPath.Text = "...";
            this.button_setPath.UseVisualStyleBackColor = true;
            this.button_setPath.Click += new System.EventHandler(this.button_setPath_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_path.Location = new System.Drawing.Point(93, 12);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(416, 21);
            this.textBox_path.TabIndex = 10;
            this.textBox_path.Text = ".\\Data\\Site.txt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "测站名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "测站名称地址：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_fromDir
            // 
            this.button_fromDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_fromDir.Location = new System.Drawing.Point(515, 203);
            this.button_fromDir.Name = "button_fromDir";
            this.button_fromDir.Size = new System.Drawing.Size(120, 39);
            this.button_fromDir.TabIndex = 15;
            this.button_fromDir.Text = "从文件提取名称";
            this.button_fromDir.UseVisualStyleBackColor = true;
            this.button_fromDir.Click += new System.EventHandler(this.button_fromDir_Click);
            // 
            // button_trigerUpLow
            // 
            this.button_trigerUpLow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_trigerUpLow.Location = new System.Drawing.Point(515, 357);
            this.button_trigerUpLow.Name = "button_trigerUpLow";
            this.button_trigerUpLow.Size = new System.Drawing.Size(120, 37);
            this.button_trigerUpLow.TabIndex = 16;
            this.button_trigerUpLow.Text = "转换为大写";
            this.button_trigerUpLow.UseVisualStyleBackColor = true;
            this.button_trigerUpLow.Click += new System.EventHandler(this.button_trigerUpLow_Click);
            // 
            // SiteNameMgrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 453);
            this.Controls.Add(this.button_trigerUpLow);
            this.Controls.Add(this.button_fromDir);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.textBox_sites);
            this.Controls.Add(this.button_read);
            this.Controls.Add(this.button_setPath);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SiteNameMgrForm";
            this.Text = "测站名称编辑器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.TextBox textBox_sites;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.Button button_setPath;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button_fromDir;
        private System.Windows.Forms.Button button_trigerUpLow;
    }
}