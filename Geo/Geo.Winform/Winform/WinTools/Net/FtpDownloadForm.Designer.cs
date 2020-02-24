namespace Geo.WinTools.Net
{
    partial class FtpDownloadForm
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
            this.button1_down = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1_placeToSave = new System.Windows.Forms.Button();
            this.textBox2_localPath = new System.Windows.Forms.TextBox();
            this.textBox1_uri = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_filePath = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1_down
            // 
            this.button1_down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1_down.Location = new System.Drawing.Point(521, 169);
            this.button1_down.Name = "button1_down";
            this.button1_down.Size = new System.Drawing.Size(75, 23);
            this.button1_down.TabIndex = 3;
            this.button1_down.Text = "下载";
            this.button1_down.UseVisualStyleBackColor = true;
            this.button1_down.Click += new System.EventHandler(this.button1_down_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1_placeToSave);
            this.groupBox1.Controls.Add(this.textBox2_localPath);
            this.groupBox1.Controls.Add(this.textBox_password);
            this.groupBox1.Controls.Add(this.textBox_filePath);
            this.groupBox1.Controls.Add(this.textBox_userName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox1_uri);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 151);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入";
            // 
            // button1_placeToSave
            // 
            this.button1_placeToSave.Location = new System.Drawing.Point(495, 109);
            this.button1_placeToSave.Name = "button1_placeToSave";
            this.button1_placeToSave.Size = new System.Drawing.Size(75, 23);
            this.button1_placeToSave.TabIndex = 2;
            this.button1_placeToSave.Text = "...";
            this.button1_placeToSave.UseVisualStyleBackColor = true;
            this.button1_placeToSave.Click += new System.EventHandler(this.button1_placeToSave_Click);
            // 
            // textBox2_localPath
            // 
            this.textBox2_localPath.Location = new System.Drawing.Point(107, 111);
            this.textBox2_localPath.Name = "textBox2_localPath";
            this.textBox2_localPath.Size = new System.Drawing.Size(380, 21);
            this.textBox2_localPath.TabIndex = 1;
            // 
            // textBox1_uri
            // 
            this.textBox1_uri.Location = new System.Drawing.Point(107, 20);
            this.textBox1_uri.Name = "textBox1_uri";
            this.textBox1_uri.Size = new System.Drawing.Size(446, 21);
            this.textBox1_uri.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 114);
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
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTP服务器地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户名：";
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(107, 50);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(113, 21);
            this.textBox_userName.TabIndex = 1;
            this.textBox_userName.Text = "Anonymous";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(240, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "密码：";
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(288, 50);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(199, 21);
            this.textBox_password.TabIndex = 1;
            this.textBox_password.Text = "User@";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "ftp文件路径：";
            // 
            // textBox_filePath
            // 
            this.textBox_filePath.Location = new System.Drawing.Point(107, 77);
            this.textBox_filePath.Name = "textBox_filePath";
            this.textBox_filePath.Size = new System.Drawing.Size(380, 21);
            this.textBox_filePath.TabIndex = 1;
            // 
            // FtpDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 204);
            this.Controls.Add(this.button1_down);
            this.Controls.Add(this.groupBox1);
            this.Name = "FtpDownloadForm";
            this.Text = "FTP下载";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1_down;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1_placeToSave;
        private System.Windows.Forms.TextBox textBox2_localPath;
        private System.Windows.Forms.TextBox textBox1_uri;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_filePath;
        private System.Windows.Forms.Label label5;
    }
}