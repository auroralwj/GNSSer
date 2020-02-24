namespace Gnsser.Winform
{
    partial class FileUrlGenForm
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
            this.textBox_sites = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_read = new System.Windows.Forms.Button();
            this.button_setPath = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button_extractSiteNames = new System.Windows.Forms.Button();
            this.textBox_toDay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_fileNameRule = new System.Windows.Forms.TextBox();
            this.textBox_year = new System.Windows.Forms.TextBox();
            this.textBox_fromDay = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_fileurls = new System.Windows.Forms.TextBox();
            this.button_gen_urls = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_savePath = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_urlPath = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_sites
            // 
            this.textBox_sites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sites.Location = new System.Drawing.Point(103, 116);
            this.textBox_sites.Multiline = true;
            this.textBox_sites.Name = "textBox_sites";
            this.textBox_sites.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_sites.Size = new System.Drawing.Size(528, 103);
            this.textBox_sites.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_read);
            this.groupBox1.Controls.Add(this.button_setPath);
            this.groupBox1.Controls.Add(this.textBox_path);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.button_extractSiteNames);
            this.groupBox1.Controls.Add(this.textBox_toDay);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_fileNameRule);
            this.groupBox1.Controls.Add(this.textBox_year);
            this.groupBox1.Controls.Add(this.textBox_fromDay);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_sites);
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(637, 221);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(551, 82);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(75, 23);
            this.button_read.TabIndex = 16;
            this.button_read.Text = "读取";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // button_setPath
            // 
            this.button_setPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setPath.Location = new System.Drawing.Point(506, 82);
            this.button_setPath.Name = "button_setPath";
            this.button_setPath.Size = new System.Drawing.Size(39, 23);
            this.button_setPath.TabIndex = 15;
            this.button_setPath.Text = "...";
            this.button_setPath.UseVisualStyleBackColor = true;
            this.button_setPath.Click += new System.EventHandler(this.button_setPath_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_path.Location = new System.Drawing.Point(105, 82);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(395, 21);
            this.textBox_path.TabIndex = 14;
            this.textBox_path.Text = ".\\Data\\Site.txt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "测站名称地址：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "文件命名规则：";
            // 
            // button_extractSiteNames
            // 
            this.button_extractSiteNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_extractSiteNames.Location = new System.Drawing.Point(516, 53);
            this.button_extractSiteNames.Name = "button_extractSiteNames";
            this.button_extractSiteNames.Size = new System.Drawing.Size(110, 23);
            this.button_extractSiteNames.TabIndex = 5;
            this.button_extractSiteNames.Text = "从文件提取代号";
            this.button_extractSiteNames.UseVisualStyleBackColor = true;
            this.button_extractSiteNames.Click += new System.EventHandler(this.button_extractSiteNames_Click);
            // 
            // textBox_toDay
            // 
            this.textBox_toDay.Location = new System.Drawing.Point(321, 43);
            this.textBox_toDay.Name = "textBox_toDay";
            this.textBox_toDay.Size = new System.Drawing.Size(44, 21);
            this.textBox_toDay.TabIndex = 4;
            this.textBox_toDay.Text = "2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(287, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "到";
            // 
            // textBox_fileNameRule
            // 
            this.textBox_fileNameRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fileNameRule.Location = new System.Drawing.Point(103, 14);
            this.textBox_fileNameRule.Name = "textBox_fileNameRule";
            this.textBox_fileNameRule.Size = new System.Drawing.Size(523, 21);
            this.textBox_fileNameRule.TabIndex = 4;
            this.textBox_fileNameRule.Text = "ftp://127.0.0.1/Gnss/{YEAR}/{DAY_OF_YEAR_3}/{NAME}{DAY_OF_YEAR_3}0.{YEAR_2}d.Z";
            // 
            // textBox_year
            // 
            this.textBox_year.Location = new System.Drawing.Point(105, 43);
            this.textBox_year.Name = "textBox_year";
            this.textBox_year.Size = new System.Drawing.Size(44, 21);
            this.textBox_year.TabIndex = 4;
            this.textBox_year.Text = "2013";
            // 
            // textBox_fromDay
            // 
            this.textBox_fromDay.Location = new System.Drawing.Point(228, 43);
            this.textBox_fromDay.Name = "textBox_fromDay";
            this.textBox_fromDay.Size = new System.Drawing.Size(44, 21);
            this.textBox_fromDay.TabIndex = 4;
            this.textBox_fromDay.Text = "2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(74, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "年：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "站点代号：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "年积日：";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox_fileurls);
            this.groupBox3.Location = new System.Drawing.Point(9, 300);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(634, 187);
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
            this.textBox_fileurls.Size = new System.Drawing.Size(628, 167);
            this.textBox_fileurls.TabIndex = 2;
            // 
            // button_gen_urls
            // 
            this.button_gen_urls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_gen_urls.Location = new System.Drawing.Point(557, 228);
            this.button_gen_urls.Name = "button_gen_urls";
            this.button_gen_urls.Size = new System.Drawing.Size(75, 31);
            this.button_gen_urls.TabIndex = 5;
            this.button_gen_urls.Text = "地址生成";
            this.button_gen_urls.UseVisualStyleBackColor = true;
            this.button_gen_urls.Click += new System.EventHandler(this.button_gen_urls_Click);
            // 
            // button_savePath
            // 
            this.button_savePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_savePath.Location = new System.Drawing.Point(557, 267);
            this.button_savePath.Name = "button_savePath";
            this.button_savePath.Size = new System.Drawing.Size(75, 31);
            this.button_savePath.TabIndex = 5;
            this.button_savePath.Text = "存储到文件";
            this.button_savePath.UseVisualStyleBackColor = true;
            this.button_savePath.Click += new System.EventHandler(this.button_savePath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "测站URL地址：";
            // 
            // textBox_urlPath
            // 
            this.textBox_urlPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_urlPath.Location = new System.Drawing.Point(139, 273);
            this.textBox_urlPath.Name = "textBox_urlPath";
            this.textBox_urlPath.Size = new System.Drawing.Size(395, 21);
            this.textBox_urlPath.TabIndex = 14;
            this.textBox_urlPath.Text = ".\\Data\\SiteUrl.txt";
            // 
            // FileUrlGenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 499);
            this.Controls.Add(this.button_savePath);
            this.Controls.Add(this.button_gen_urls);
            this.Controls.Add(this.textBox_urlPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "FileUrlGenForm";
            this.Text = "GNSS文件地址生成器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_sites;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_fileurls;
        private System.Windows.Forms.Button button_gen_urls;
        private System.Windows.Forms.TextBox textBox_toDay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_fromDay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_extractSiteNames;
        private System.Windows.Forms.TextBox textBox_year;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_fileNameRule;
        private System.Windows.Forms.Button button_savePath;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.Button button_setPath;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_urlPath;

    }
}