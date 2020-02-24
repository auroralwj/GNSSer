using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.WinTools.Net
{
    /// <summary>
    /// email测试
    /// </summary>
    public partial class EmailSenderForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmailSenderForm()
        {
            InitializeComponent();
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            string mailTo = this.textBox_mailTo.Text.Trim();
            string mailFrom = this.textBox_mailFrom.Text.Trim();
            string subject = this.textBox_subject.Text.Trim();
            string content = this.textBox_content.Text.Trim();
            int port = int.Parse(this.textBox_port.Text);
            string smtpHost = this.comboBox_smtphosts.Text.Trim();
            string password = this.textBox_password.Text.Trim();
            string [] attachments = null;
            if (this.textBox_attachments.Lines.Length > 0)
                attachments = this.textBox_attachments.Lines;
            var result = Geo.Utils.EmailSender.SendMail(mailTo, mailFrom,password, subject, content, smtpHost, port, attachments);

            MessageBox.Show(result +  "执行完毕！");
        }

        private void button_addAtachments_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> lines = new List<string>();
                if (this.textBox_attachments.Lines.Length > 0)
                    lines.AddRange(this.textBox_attachments.Lines);
                lines.AddRange( openFileDialog1.FileNames);
                this.textBox_attachments.Lines = lines.ToArray();
            }
        }

        private void button_clearAttachments_Click(object sender, EventArgs e)
        {
            this.textBox_attachments.Text = "";
        }
    }
}
