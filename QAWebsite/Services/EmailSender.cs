using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace QAWebsite.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfigurationSection emailConfig;

        public EmailSender()
        {
            this.emailConfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build().GetSection("WebsiteEmailService");
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            //var emailMessage = new MimeMessage("QAWebsite341@gmail.com", email,subject,message);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailConfig["EmailerTitle"], emailConfig["EmailAddress"]));
            emailMessage.To.Add(new MailboxAddress(email, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html")
            {
                Text = message
            };

            using (SmtpClient client = new SmtpClient()) {
                client.Connect(emailConfig["GmailSMTPAddress"], Convert.ToInt32(emailConfig["GmailSMTPPort"]), false);
                client.Authenticate(emailConfig["EmailAddress"], emailConfig["EmailPassword"]);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
