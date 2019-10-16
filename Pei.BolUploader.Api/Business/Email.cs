using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Pei.BolUploader.Api.Business
{
    public class Email
    {
        public static SmtpClient client = null;
        static Email() { 
            client = new SmtpClient(smtpClientAddress, 25);
            client.SendCompleted += (sender, e) => {

            };
        }
        static string smtpClientAddress = "mailman.supreme.com";
        
        public static void Send(List<string> to, string body)
        {
            MailMessage msg = new MailMessage();
            foreach (string t in to)
            {
                msg.To.Add(t);
            }
            msg.Subject = "[TEST]BOL UPLOAD NOTIFICATION";
            msg.Body = body;
            msg.IsBodyHtml = true;
            
            client.SendMailAsync(msg);
            
        }
    }
}
