//2018.11.22, czs, create in ryd, 邮件发送类的封装

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;


namespace Geo
{

     

    /// <summary>
    /// 邮件发送
    /// </summary> 
    public class SMTPEmailSender
    { 
        //If your smtp server wants authentication,use it 
        public SMTPEmailSender(String smtpServer, String user, String password)
        {
            mailMessage = new MailMessage();
            smtpClient = new SmtpClient();
            smtpClient.Host = smtpServer;
            smtpClient.Port = 25;
            smtpClient.Credentials = new NetworkCredential(user, password);
        }

        //If your smtp server doesn't want authentication,use it 
        public SMTPEmailSender(String smtpServer)
        {
            mailMessage = new MailMessage();
            smtpClient = new SmtpClient(smtpServer);
        }

        public String Subject
        {
            get
            {
                return mailMessage.Subject;
            }

            set
            {
                mailMessage.Subject = value;
            }
        }

        //get/set the email's content 
        public String Content
        {
            get
            {
                return mailMessage.Body;
            }
            set
            {
                mailMessage.Body = value;
            }
        }

        public String From
        {
            get
            {
                return mailMessage.From.Address;
            }

            set
            {
                mailMessage.From = new MailAddress(value);
            }
        }

        public void AddReceiver(String email)
        {
            mailMessage.To.Add(email);
        }

        public void Send()
        {
            smtpClient.Send(mailMessage);
        }

        public void AddAttachment(String filename)
        {
            mailMessage.Attachments.Add(new Attachment(filename));
        }

        private MailMessage mailMessage;
        private SmtpClient smtpClient;






       public void Test()
        {
            try
            {
                SMTPEmailSender sender = new SMTPEmailSender("mail.longdayinfo.com", "upcodechina@longdayinfo.com", "12345");
                sender.From = "upcodechina@longdayinfo.com";
                sender.AddReceiver("csfreebird@gmail.com");
                sender.Subject = "This is a test";
                sender.Content = "hi,beijing team";
                sender.AddAttachment("C://aa.jpg");
                sender.AddAttachment("C://aaa.csv");
                sender.Send();
                System.Console.Out.WriteLine("send ok");
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine(e.Source + e.Message);
            }
        }

    }
}