namespace Gnsser.Winform
{
    partial class BuildAndDownFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuildAndDownFileForm));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_login = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1_placeToSave = new System.Windows.Forms.Button();
            this.textBox2_localPath = new System.Windows.Forms.TextBox();
            this.textBox1_uri = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_sites = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.button_download = new System.Windows.Forms.Button();
            this.button_gen_urls = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBox_login);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.button1_placeToSave);
            this.groupBox2.Controls.Add(this.textBox2_localPath);
            this.groupBox2.Controls.Add(this.textBox1_uri);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(689, 111);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "位置信息";
            // 
            // checkBox_login
            // 
            this.checkBox_login.AutoSize = true;
            this.checkBox_login.Location = new System.Drawing.Point(6, 55);
            this.checkBox_login.Name = "checkBox_login";
            this.checkBox_login.Size = new System.Drawing.Size(48, 16);
            this.checkBox_login.TabIndex = 5;
            this.checkBox_login.Text = "登录";
            this.checkBox_login.UseVisualStyleBackColor = true;
            this.checkBox_login.CheckedChanged += new System.EventHandler(this.checkBox_login_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox_password);
            this.panel1.Controls.Add(this.textBox_userName);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(90, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(463, 28);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(251, 3);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(199, 21);
            this.textBox_password.TabIndex = 4;
            this.textBox_password.Text = "User@";
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(70, 3);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(113, 21);
            this.textBox_userName.TabIndex = 5;
            this.textBox_userName.Text = "Anonymous";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "用户名：";
            // 
            // button1_placeToSave
            // 
            this.button1_placeToSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1_placeToSave.Location = new System.Drawing.Point(611, 82);
            this.button1_placeToSave.Name = "button1_placeToSave";
            this.button1_placeToSave.Size = new System.Drawing.Size(75, 23);
            this.button1_placeToSave.TabIndex = 2;
            this.button1_placeToSave.Text = "...";
            this.button1_placeToSave.UseVisualStyleBackColor = true;
            this.button1_placeToSave.Click += new System.EventHandler(this.button1_placeToSave_Click);
            // 
            // textBox2_localPath
            // 
            this.textBox2_localPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2_localPath.Location = new System.Drawing.Point(90, 84);
            this.textBox2_localPath.Name = "textBox2_localPath";
            this.textBox2_localPath.Size = new System.Drawing.Size(500, 21);
            this.textBox2_localPath.TabIndex = 1;
            this.textBox2_localPath.Text = "C:\\Download\\";
            // 
            // textBox1_uri
            // 
            this.textBox1_uri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1_uri.Location = new System.Drawing.Point(90, 20);
            this.textBox1_uri.Name = "textBox1_uri";
            this.textBox1_uri.Size = new System.Drawing.Size(593, 21);
            this.textBox1_uri.TabIndex = 1;
            this.textBox1_uri.Text = "ftp://25.20.220.196/";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "本地存储位置：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器根目录：";
            // 
            // textBox_sites
            // 
            this.textBox_sites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sites.Location = new System.Drawing.Point(79, 75);
            this.textBox_sites.Multiline = true;
            this.textBox_sites.Name = "textBox_sites";
            this.textBox_sites.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_sites.Size = new System.Drawing.Size(604, 77);
            this.textBox_sites.TabIndex = 2;
            this.textBox_sites.Text = resources.GetString("textBox_sites.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBox1.Location = new System.Drawing.Point(12, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(689, 154);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件信息";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 6;
            this.label9.Text = "文件命名规则：";
            // 
            // button_extractSiteNames
            // 
            this.button_extractSiteNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_extractSiteNames.Location = new System.Drawing.Point(603, 46);
            this.button_extractSiteNames.Name = "button_extractSiteNames";
            this.button_extractSiteNames.Size = new System.Drawing.Size(75, 23);
            this.button_extractSiteNames.TabIndex = 5;
            this.button_extractSiteNames.Text = "提取代号";
            this.button_extractSiteNames.UseVisualStyleBackColor = true;
            this.button_extractSiteNames.Click += new System.EventHandler(this.button_extractSiteNames_Click);
            // 
            // textBox_toDay
            // 
            this.textBox_toDay.Location = new System.Drawing.Point(295, 48);
            this.textBox_toDay.Name = "textBox_toDay";
            this.textBox_toDay.Size = new System.Drawing.Size(44, 21);
            this.textBox_toDay.TabIndex = 4;
            this.textBox_toDay.Text = "2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(261, 52);
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
            this.textBox_fileNameRule.Size = new System.Drawing.Size(575, 21);
            this.textBox_fileNameRule.TabIndex = 4;
            this.textBox_fileNameRule.Text = "{YEAR}/{DAY_OF_YEAR_3}/{NAME}{DAY_OF_YEAR_3}0.{YEAR_2}d.Z";
            // 
            // textBox_year
            // 
            this.textBox_year.Location = new System.Drawing.Point(79, 48);
            this.textBox_year.Name = "textBox_year";
            this.textBox_year.Size = new System.Drawing.Size(44, 21);
            this.textBox_year.TabIndex = 4;
            this.textBox_year.Text = "2013";
            // 
            // textBox_fromDay
            // 
            this.textBox_fromDay.Location = new System.Drawing.Point(202, 48);
            this.textBox_fromDay.Name = "textBox_fromDay";
            this.textBox_fromDay.Size = new System.Drawing.Size(44, 21);
            this.textBox_fromDay.TabIndex = 4;
            this.textBox_fromDay.Text = "2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "年：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "站点代号：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(143, 52);
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
            this.groupBox3.Location = new System.Drawing.Point(9, 328);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(692, 178);
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
            this.textBox_fileurls.Size = new System.Drawing.Size(686, 158);
            this.textBox_fileurls.TabIndex = 2;
            // 
            // button_download
            // 
            this.button_download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_download.Location = new System.Drawing.Point(621, 282);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(75, 40);
            this.button_download.TabIndex = 5;
            this.button_download.Text = "下载";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // button_gen_urls
            // 
            this.button_gen_urls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_gen_urls.Location = new System.Drawing.Point(540, 282);
            this.button_gen_urls.Name = "button_gen_urls";
            this.button_gen_urls.Size = new System.Drawing.Size(75, 40);
            this.button_gen_urls.TabIndex = 5;
            this.button_gen_urls.Text = "地址生成";
            this.button_gen_urls.UseVisualStyleBackColor = true;
            this.button_gen_urls.Click += new System.EventHandler(this.button_gen_urls_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(18, 282);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(516, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // DownGnssForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 518);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_gen_urls);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "DownGnssForm";
            this.Text = "地址生成与下载";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_login;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1_placeToSave;
        private System.Windows.Forms.TextBox textBox2_localPath;
        private System.Windows.Forms.TextBox textBox1_uri;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_sites;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_fileurls;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_gen_urls;
        private System.Windows.Forms.ProgressBar progressBar1;
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

    }
}