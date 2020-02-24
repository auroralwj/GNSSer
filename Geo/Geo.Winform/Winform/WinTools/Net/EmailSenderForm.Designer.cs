namespace Geo.WinTools.Net
{
    partial class EmailSenderForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_mailTo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_mailFrom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_subject = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_content = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_attachments = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_addAtachments = new System.Windows.Forms.Button();
            this.button_clearAttachments = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_smtphosts = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBox_smtphosts);
            this.groupBox1.Controls.Add(this.textBox_port);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button_send);
            this.groupBox1.Controls.Add(this.textBox_password);
            this.groupBox1.Controls.Add(this.textBox_mailFrom);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_mailTo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(905, 129);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "SMTP服务器：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(394, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口：";
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(456, 21);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(57, 25);
            this.textBox_port.TabIndex = 3;
            this.textBox_port.Text = "25";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "发送到：";
            // 
            // textBox_mailTo
            // 
            this.textBox_mailTo.Location = new System.Drawing.Point(121, 53);
            this.textBox_mailTo.Name = "textBox_mailTo";
            this.textBox_mailTo.Size = new System.Drawing.Size(255, 25);
            this.textBox_mailTo.TabIndex = 1;
            this.textBox_mailTo.Text = "czsgeo@126.com";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "发送自：";
            // 
            // textBox_mailFrom
            // 
            this.textBox_mailFrom.Location = new System.Drawing.Point(121, 87);
            this.textBox_mailFrom.Name = "textBox_mailFrom";
            this.textBox_mailFrom.Size = new System.Drawing.Size(255, 25);
            this.textBox_mailFrom.TabIndex = 1;
            this.textBox_mailFrom.Text = "service@gnsser.com";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "主题：";
            // 
            // textBox_subject
            // 
            this.textBox_subject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_subject.Location = new System.Drawing.Point(134, 159);
            this.textBox_subject.Name = "textBox_subject";
            this.textBox_subject.Size = new System.Drawing.Size(663, 25);
            this.textBox_subject.TabIndex = 1;
            this.textBox_subject.Text = "这只是一个测试";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(61, 279);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "内容：";
            // 
            // textBox_content
            // 
            this.textBox_content.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_content.Location = new System.Drawing.Point(134, 279);
            this.textBox_content.Multiline = true;
            this.textBox_content.Name = "textBox_content";
            this.textBox_content.Size = new System.Drawing.Size(784, 199);
            this.textBox_content.TabIndex = 1;
            this.textBox_content.Text = "这是测试信件请不要回复。";
            // 
            // button_send
            // 
            this.button_send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_send.Location = new System.Drawing.Point(790, 35);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(97, 57);
            this.button_send.TabIndex = 4;
            this.button_send.Text = "发送";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "附件：";
            // 
            // textBox_attachments
            // 
            this.textBox_attachments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_attachments.Location = new System.Drawing.Point(134, 201);
            this.textBox_attachments.Multiline = true;
            this.textBox_attachments.Name = "textBox_attachments";
            this.textBox_attachments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_attachments.Size = new System.Drawing.Size(681, 72);
            this.textBox_attachments.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_addAtachments
            // 
            this.button_addAtachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addAtachments.Location = new System.Drawing.Point(821, 201);
            this.button_addAtachments.Name = "button_addAtachments";
            this.button_addAtachments.Size = new System.Drawing.Size(97, 36);
            this.button_addAtachments.TabIndex = 4;
            this.button_addAtachments.Text = "添加附件";
            this.button_addAtachments.UseVisualStyleBackColor = true;
            this.button_addAtachments.Click += new System.EventHandler(this.button_addAtachments_Click);
            // 
            // button_clearAttachments
            // 
            this.button_clearAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearAttachments.Location = new System.Drawing.Point(821, 237);
            this.button_clearAttachments.Name = "button_clearAttachments";
            this.button_clearAttachments.Size = new System.Drawing.Size(97, 36);
            this.button_clearAttachments.TabIndex = 4;
            this.button_clearAttachments.Text = "清空";
            this.button_clearAttachments.UseVisualStyleBackColor = true;
            this.button_clearAttachments.Click += new System.EventHandler(this.button_clearAttachments_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(456, 90);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(169, 25);
            this.textBox_password.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(394, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "密码：";
            // 
            // comboBox_smtphosts
            // 
            this.comboBox_smtphosts.FormattingEnabled = true;
            this.comboBox_smtphosts.Items.AddRange(new object[] {
            "gnsser.com",
            "smtp.163.com",
            "smtp.126.com",
            "smtp.qq.com"});
            this.comboBox_smtphosts.Location = new System.Drawing.Point(121, 21);
            this.comboBox_smtphosts.Name = "comboBox_smtphosts";
            this.comboBox_smtphosts.Size = new System.Drawing.Size(254, 23);
            this.comboBox_smtphosts.TabIndex = 5;
            this.comboBox_smtphosts.Text = "gnsser.com";
            // 
            // EmailSenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 490);
            this.Controls.Add(this.button_clearAttachments);
            this.Controls.Add(this.button_addAtachments);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_content);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_attachments);
            this.Controls.Add(this.textBox_subject);
            this.Controls.Add(this.label5);
            this.Name = "EmailSenderForm";
            this.Text = "邮件发送器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_mailFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_mailTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_subject;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_content;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_attachments;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_addAtachments;
        private System.Windows.Forms.Button button_clearAttachments;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.ComboBox comboBox_smtphosts;
    }
}