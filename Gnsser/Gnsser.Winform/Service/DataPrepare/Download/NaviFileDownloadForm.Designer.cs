namespace Gnsser.Winform
{
    partial class NaviFileDownloadForm
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
            this.components = new System.ComponentModel.Container();
            this.checkBox_login = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1_uriModel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_download = new System.Windows.Forms.Button();
            this.button_gen_urls = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton1cddis = new System.Windows.Forms.RadioButton();
            this.radioButton2gnsswhu = new System.Windows.Forms.RadioButton();
            this.radioButton_cddisNFile = new System.Windows.Forms.RadioButton();
            this.directorySelectionControl1 = new Geo.Winform.Controls.DirectorySelectionControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl1 = new Geo.Winform.Controls.RichTextBoxControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.richTextBoxControl_failedUrls = new Geo.Winform.Controls.RichTextBoxControl();
            this.timePeriodControl1 = new Gnsser.Winform.Controls.TimePeriodUserControl();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_login
            // 
            this.checkBox_login.AutoSize = true;
            this.checkBox_login.Location = new System.Drawing.Point(6, 12);
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
            this.panel1.Location = new System.Drawing.Point(90, 5);
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
            // textBox1_uriModel
            // 
            this.textBox1_uriModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1_uriModel.Location = new System.Drawing.Point(75, 6);
            this.textBox1_uriModel.Name = "textBox1_uriModel";
            this.textBox1_uriModel.Size = new System.Drawing.Size(858, 21);
            this.textBox1_uriModel.TabIndex = 1;
            this.textBox1_uriModel.Text = "ftp://igs.gnsswhu.cn/pub/gnss/BRDC_v3/{Year}/{DayOfYear}/brdm{DayOfYear}0.{SubYea" +
    "r}p.Z";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地址模板：";
            // 
            // button_download
            // 
            this.button_download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_download.Location = new System.Drawing.Point(863, 172);
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
            this.button_gen_urls.Location = new System.Drawing.Point(782, 172);
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
            this.progressBar1.Location = new System.Drawing.Point(19, 172);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(757, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(948, 150);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.timePeriodControl1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.directorySelectionControl1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBox1_uriModel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(940, 124);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(570, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 44);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据模板";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radioButton1cddis);
            this.flowLayoutPanel1.Controls.Add(this.radioButton2gnsswhu);
            this.flowLayoutPanel1.Controls.Add(this.radioButton_cddisNFile);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(346, 24);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // radioButton1cddis
            // 
            this.radioButton1cddis.AutoSize = true;
            this.radioButton1cddis.Location = new System.Drawing.Point(3, 3);
            this.radioButton1cddis.Name = "radioButton1cddis";
            this.radioButton1cddis.Size = new System.Drawing.Size(83, 16);
            this.radioButton1cddis.TabIndex = 9;
            this.radioButton1cddis.TabStop = true;
            this.radioButton1cddis.Text = "cddisN文件";
            this.radioButton1cddis.UseVisualStyleBackColor = true;
            this.radioButton1cddis.CheckedChanged += new System.EventHandler(this.radioButton1cddis_CheckedChanged);
            // 
            // radioButton2gnsswhu
            // 
            this.radioButton2gnsswhu.AutoSize = true;
            this.radioButton2gnsswhu.Location = new System.Drawing.Point(92, 3);
            this.radioButton2gnsswhu.Name = "radioButton2gnsswhu";
            this.radioButton2gnsswhu.Size = new System.Drawing.Size(95, 16);
            this.radioButton2gnsswhu.TabIndex = 9;
            this.radioButton2gnsswhu.TabStop = true;
            this.radioButton2gnsswhu.Text = "gnsswhuN文件";
            this.radioButton2gnsswhu.UseVisualStyleBackColor = true;
            this.radioButton2gnsswhu.CheckedChanged += new System.EventHandler(this.radioButton2gnsswhu_CheckedChanged);
            // 
            // radioButton_cddisNFile
            // 
            this.radioButton_cddisNFile.AutoSize = true;
            this.radioButton_cddisNFile.Location = new System.Drawing.Point(193, 3);
            this.radioButton_cddisNFile.Name = "radioButton_cddisNFile";
            this.radioButton_cddisNFile.Size = new System.Drawing.Size(89, 16);
            this.radioButton_cddisNFile.TabIndex = 9;
            this.radioButton_cddisNFile.TabStop = true;
            this.radioButton_cddisNFile.Text = "ceddisN文件";
            this.radioButton_cddisNFile.UseVisualStyleBackColor = true;
            this.radioButton_cddisNFile.CheckedChanged += new System.EventHandler(this.radioButton_cddisNFile_CheckedChanged);
            // 
            // directorySelectionControl1
            // 
            this.directorySelectionControl1.AllowDrop = true;
            this.directorySelectionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directorySelectionControl1.IsAddOrReplase = false;
            this.directorySelectionControl1.IsMultiPathes = false;
            this.directorySelectionControl1.LabelName = "本地存储位置：";
            this.directorySelectionControl1.Location = new System.Drawing.Point(22, 87);
            this.directorySelectionControl1.Name = "directorySelectionControl1";
            this.directorySelectionControl1.Path = "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp";
            this.directorySelectionControl1.Pathes = new string[] {
        "D:\\ProgramFiles\\Microsoft Visual Studio\\2017\\Professional\\Common7\\IDE\\Temp"};
            this.directorySelectionControl1.Size = new System.Drawing.Size(906, 22);
            this.directorySelectionControl1.TabIndex = 8;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox_login);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(940, 124);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "高级设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(19, 218);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(920, 295);
            this.tabControl2.TabIndex = 8;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBoxControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(912, 269);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "生成地址";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl1
            // 
            this.richTextBoxControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl1.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl1.MaxAppendLineCount = 5000;
            this.richTextBoxControl1.Name = "richTextBoxControl1";
            this.richTextBoxControl1.Size = new System.Drawing.Size(906, 263);
            this.richTextBoxControl1.TabIndex = 0;
            this.richTextBoxControl1.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.richTextBoxControl_failedUrls);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(912, 269);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "失败地址";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // richTextBoxControl_failedUrls
            // 
            this.richTextBoxControl_failedUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxControl_failedUrls.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxControl_failedUrls.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxControl_failedUrls.MaxAppendLineCount = 10000;
            this.richTextBoxControl_failedUrls.Name = "richTextBoxControl_failedUrls";
            this.richTextBoxControl_failedUrls.Size = new System.Drawing.Size(906, 263);
            this.richTextBoxControl_failedUrls.TabIndex = 1;
            this.richTextBoxControl_failedUrls.Text = "";
            // 
            // timePeriodControl1
            // 
            this.timePeriodControl1.Location = new System.Drawing.Point(18, 45);
            this.timePeriodControl1.Name = "timePeriodControl1";
            this.timePeriodControl1.Size = new System.Drawing.Size(492, 32);
            this.timePeriodControl1.TabIndex = 11;
            // 
            // NaviFileDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 518);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_gen_urls);
            this.Controls.Add(this.button_download);
            this.Name = "NaviFileDownloadForm";
            this.Text = "导航文件生成与下载";
            this.Load += new System.EventHandler(this.NaviFileDownloadForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_login;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1_uriModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_gen_urls;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Geo.Winform.Controls.DirectorySelectionControl directorySelectionControl1;
        private System.Windows.Forms.RadioButton radioButton2gnsswhu;
        private System.Windows.Forms.RadioButton radioButton1cddis;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private Geo.Winform.Controls.RichTextBoxControl richTextBoxControl_failedUrls;
        private System.Windows.Forms.RadioButton radioButton_cddisNFile;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Controls.TimePeriodUserControl timePeriodControl1;
    }
}