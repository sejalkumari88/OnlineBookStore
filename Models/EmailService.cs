using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace OnlineBookStore.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("yourgmail@gmail.com", "your-app-password"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("yourgmail@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
    }
}
