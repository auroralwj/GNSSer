//2016.01.12, czs, edit in 洪庆，增加功能，设置默认stmp

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail; 
using System.Net.Mime;

namespace Geo.Utils
{
    /// <summary>
    /// 邮件发送器
    /// </summary>
    public class EmailSender
    {
        public static string SendMail(List<string> toMails, string fromMail, string displayName, string password, string subject, string text, string html, string[] attachmentLocalPathes, string smtpHost = "smtp.gnsser.com", int port = 25)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();
                foreach (var item in toMails)
	           {
                mailMsg.To.Add(new MailAddress(item));
		 
                }
                mailMsg.From = new MailAddress(fromMail, displayName);
                // 邮件主题
                mailMsg.Subject = subject;
                // 邮件正文内容
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // 添加附件 

                foreach (var file in attachmentLocalPathes)
                {
                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                    mailMsg.Attachments.Add(data);
                    
                }
                //邮件推送的SMTP地址和端口
                SmtpClient smtpClient = new SmtpClient(smtpHost, port);
                // 使用SMTP用户名和密码进行验证
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(fromMail,password);
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "0" + ex.Message;
            }
            return "1";
        }


        /// <summary>
        /// 发送一个普通文字邮件。返回1则成功，否则为失败。 
        /// <param name="toMail">收件人邮箱地址</param>		
        /// <param name="fromMail">发件人邮箱地址</param>	
        /// <param name="password">发件人邮箱密码</param>	
        /// <param name="subject">邮件标题</param>		
        /// <param name="Body">邮件正文</param>	
        /// <param name="smtpHost">邮件服务地址</param>	
        /// <param name="port">端口</param>	
        /// <param name="attachmentLocalPathes">附件地址</param>	
        /// <param name="Bcc">单个密送</param>		
        /// <param name="Cc">单个抄送</param>
        /// <returns>返回1则成功，否则为失败。</returns>		
        /// </summary>
        public static string SendMail(string toMail,
            string fromMail, string password, string subject,
            string Body, string smtpHost = "smtp.gnsser.com", int port = 25,
            string[] attachmentLocalPathes = null, string Cc = "", string Bcc = "")
        {
            List<string> toMails = new List<string>();
            List<string> ccs = new List<string>();
            List<string> bccs = new List<string>();

            toMails.Add(toMail);
            if (!String.IsNullOrEmpty(Cc)) ccs.Add(Cc);
            if (!String.IsNullOrEmpty(Bcc)) ccs.Add(Bcc);

            return SendMail(toMails,fromMail, password, subject, Body, smtpHost, port, attachmentLocalPathes, ccs = null, bccs=null);
        }

        /// <summary>
        /// 发送一个普通文字邮件。返回1则成功，否则为失败。 
        /// <param name="toMails">收件人邮箱地址</param>		
        /// <param name="fromMail">发件人邮箱地址</param>	
        /// <param name="password">发件人邮箱密码</param>	
        /// <param name="subject">邮件标题</param>		
        /// <param name="Body">邮件正文</param>	
        /// <param name="smtpHost">邮件服务地址</param>	
        /// <param name="port">端口</param>	
        /// <param name="attachmentLocalPathes">附件地址</param>	
        /// <param name="bccs">密送</param>		
        /// <param name="ccs">抄送</param>
        /// <returns>返回1则成功，否则为失败。</returns>		
        /// </summary>
        public static string SendMail(List<string> toMails, string fromMail, string password,
            string subject, string Body, string smtpHost = "smtp.gnsser.com", int port = 25,
            string[] attachmentLocalPathes = null, List<string> ccs = null, List<string> bccs = null, bool IsBodyHtml = true)
        {
            SmtpClient smtpClient = new SmtpClient(smtpHost, port);
            MailMessage message = new MailMessage
            {
                From = new MailAddress(fromMail)
            };
            foreach (var item in toMails)
            {
                message.To.Add(item);
            }
            if (ccs!= null)
            foreach (var item in ccs)
            {
                message.CC.Add(item);
            } 
            if (bccs != null)
            foreach (var item in bccs)
            {
                message.Bcc.Add(item);
            }

            message.Body = Body;
            message.Subject = subject;
            message.IsBodyHtml = IsBodyHtml;
            //client.UseDefaultCredentials = true;
            //定义邮件正文，主题的编码方式
            message.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            message.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            message.Priority = MailPriority.High;

            smtpClient.Credentials = new System.Net.NetworkCredential(fromMail, password);//填写登录邮箱的用户名及密码

            if (attachmentLocalPathes != null)
            {
                foreach (var item in attachmentLocalPathes)
                {
                    Attachment data = new Attachment(item);
                    message.Attachments.Add(data);
                }
            }

            try
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(message);

                System.Threading.Thread.Sleep(300);//间隔1s发送，避免太频繁而失败

                message.Dispose();
                return "1";
        }
            catch (Exception exception)
            {
                new Geo.IO.Log(typeof(EmailSender)).Error("邮件发送失败：" + exception.Message);
                return ("0" + exception.Message);
            }
}
    }
}