using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace QAWebsite.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            //var emailMessage = new MimeMessage("QAWebsite341@gmail.com", email,subject,message);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("QAWebsite", "QAWebsite341@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(email, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html")
            {
                Text = message
            };

            using (SmtpClient client = new SmtpClient()) {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("qawebsite341@gmail.com", "!Qaz2wsx");
                client.Send(emailMessage);
                client.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
