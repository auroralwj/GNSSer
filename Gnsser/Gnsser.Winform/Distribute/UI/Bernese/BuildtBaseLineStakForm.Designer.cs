namespace Gnsser.Winform
{
    partial class BuildtBaseLineStakForm
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
            this.button_build = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_read = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_baseLines = new System.Windows.Forms.TextBox();
            this.button_setBaseLinePath = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.textBox_baseLinePath = new System.Windows.Forms.TextBox();
            this.textBox_resultFtp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_campaign = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_readPointSite = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button_SetPointSitePath = new System.Windows.Forms.Button();
            this.textBox_SitePPPPath = new System.Windows.Forms.TextBox();
            this.button_ppp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_build
            // 
            this.button_build.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_build.Location = new System.Drawing.Point(691, 486);
            this.button_build.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_build.Name = "button_build";
            this.button_build.Size = new System.Drawing.Size(100, 29);
            this.button_build.TabIndex = 1;
            this.button_build.Text = "建立基线";
            this.button_build.UseVisualStyleBackColor = true;
            this.button_build.Click += new System.EventHandler(this.button_build_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_readPointSite);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.button_SetPointSitePath);
            this.groupBox1.Controls.Add(this.textBox_SitePPPPath);
            this.groupBox1.Controls.Add(this.button_read);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_baseLines);
            this.groupBox1.Controls.Add(this.button_setBaseLinePath);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.textBox_baseLinePath);
            this.groupBox1.Controls.Add(this.textBox_resultFtp);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_campaign);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(775, 452);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // button_read
            // 
            this.button_read.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_read.Location = new System.Drawing.Point(685, 135);
            this.button_read.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_read.Name = "button_read";
            this.button_read.Size = new System.Drawing.Size(73, 29);
            this.button_read.TabIndex = 10;
            this.button_read.Text = "读入";
            this.button_read.UseVisualStyleBackColor = true;
            this.button_read.Click += new System.EventHandler(this.button_read_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 141);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "基线文件地址：";
            // 
            // textBox_baseLines
            // 
            this.textBox_baseLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_baseLines.Location = new System.Drawing.Point(8, 221);
            this.textBox_baseLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_baseLines.Multiline = true;
            this.textBox_baseLines.Name = "textBox_baseLines";
            this.textBox_baseLines.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_baseLines.Size = new System.Drawing.Size(757, 223);
            this.textBox_baseLines.TabIndex = 2;
            // 
            // button_setBaseLinePath
            // 
            this.button_setBaseLinePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_setBaseLinePath.Location = new System.Drawing.Point(616, 132);
            this.button_setBaseLinePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_setBaseLinePath.Name = "button_setBaseLinePath";
            this.button_setBaseLinePath.Size = new System.Drawing.Size(56, 29);
            this.button_setBaseLinePath.TabIndex = 8;
            this.button_setBaseLinePath.Text = "...";
            this.button_setBaseLinePath.UseVisualStyleBackColor = true;
            this.button_setBaseLinePath.Click += new System.EventHandler(this.button_setBaseLinePath_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(124, 98);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(265, 25);
            this.dateTimePicker1.TabIndex = 4;
            this.dateTimePicker1.Value = new System.DateTime(2002, 5, 23, 0, 0, 0, 0);
            // 
            // textBox_baseLinePath
            // 
            this.textBox_baseLinePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_baseLinePath.Location = new System.Drawing.Point(124, 135);
            this.textBox_baseLinePath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_baseLinePath.Name = "textBox_baseLinePath";
            this.textBox_baseLinePath.Size = new System.Drawing.Size(483, 25);
            this.textBox_baseLinePath.TabIndex = 2;
            this.textBox_baseLinePath.Text = ".\\Data\\BaseLine.txt";
            // 
            // textBox_resultFtp
            // 
            this.textBox_resultFtp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_resultFtp.Location = new System.Drawing.Point(124, 60);
            this.textBox_resultFtp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_resultFtp.Name = "textBox_resultFtp";
            this.textBox_resultFtp.Size = new System.Drawing.Size(623, 25);
            this.textBox_resultFtp.TabIndex = 2;
            this.textBox_resultFtp.Text = "ftp://25.20.220.196/TestData/Result/";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 98);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "GPS 时间：";
            // 
            // textBox_campaign
            // 
            this.textBox_campaign.Location = new System.Drawing.Point(124, 18);
            this.textBox_campaign.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_campaign.Name = "textBox_campaign";
            this.textBox_campaign.Size = new System.Drawing.Size(219, 25);
            this.textBox_campaign.TabIndex = 1;
            this.textBox_campaign.Text = "EXAMPLE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 21);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "工程：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "结果FTP：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_readPointSite
            // 
            this.button_readPointSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readPointSite.Location = new System.Drawing.Point(686, 179);
            this.button_readPointSite.Margin = new System.Windows.Forms.Padding(4);
            this.button_readPointSite.Name = "button_readPointSite";
            this.button_readPointSite.Size = new System.Drawing.Size(73, 29);
            this.button_readPointSite.TabIndex = 14;
            this.button_readPointSite.Text = "读入";
            this.button_readPointSite.UseVisualStyleBackColor = true;
            this.button_readPointSite.Click += new System.EventHandler(this.button_readPointSite_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 185);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "测站文件地址：";
            // 
            // button_SetPointSitePath
            // 
            this.button_SetPointSitePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SetPointSitePath.Location = new System.Drawing.Point(617, 176);
            this.button_SetPointSitePath.Margin = new System.Windows.Forms.Padding(4);
            this.button_SetPointSitePath.Name = "button_SetPointSitePath";
            this.button_SetPointSitePath.Size = new System.Drawing.Size(56, 29);
            this.button_SetPointSitePath.TabIndex = 12;
            this.button_SetPointSitePath.Text = "...";
            this.button_SetPointSitePath.UseVisualStyleBackColor = true;
            this.button_SetPointSitePath.Click += new System.EventHandler(this.button_SetPointSitePath_Click);
            // 
            // textBox_SitePPPPath
            // 
            this.textBox_SitePPPPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SitePPPPath.Location = new System.Drawing.Point(125, 179);
            this.textBox_SitePPPPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_SitePPPPath.Name = "textBox_SitePPPPath";
            this.textBox_SitePPPPath.Size = new System.Drawing.Size(483, 25);
            this.textBox_SitePPPPath.TabIndex = 11;
            this.textBox_SitePPPPath.Text = ".\\Data\\Site.txt";
            // 
            // button_ppp
            // 
            this.button_ppp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ppp.Location = new System.Drawing.Point(552, 486);
            this.button_ppp.Margin = new System.Windows.Forms.Padding(4);
            this.button_ppp.Name = "button_ppp";
            this.button_ppp.Size = new System.Drawing.Size(100, 29);
            this.button_ppp.TabIndex = 3;
            this.button_ppp.Text = "建立PPP";
            this.button_ppp.UseVisualStyleBackColor = true;
            this.button_ppp.Click += new System.EventHandler(this.button_ppp_Click);
            // 
            // BuildtBaseLineStakForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 530);
            this.Controls.Add(this.button_ppp);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_build);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BuildtBaseLineStakForm";
            this.Text = "创建基线任务";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_build;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_setBaseLinePath;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox textBox_baseLinePath;
        private System.Windows.Forms.TextBox textBox_resultFtp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_campaign;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_read;
        private System.Windows.Forms.TextBox textBox_baseLines;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_readPointSite;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_SetPointSitePath;
        private System.Windows.Forms.TextBox textBox_SitePPPPath;
        private System.Windows.Forms.Button button_ppp;
    }
}