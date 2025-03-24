
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
namespace ReserveBiteee.Service
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                string fromEmail = _configuration["EmailSettings:FromEmail"];
                string smtpHost = _configuration["EmailSettings:SmtpHost"];
                int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                string smtpUser = _configuration["EmailSettings:SmtpUser"];
                string smtpPass = _configuration["EmailSettings:SmtpPass"];

                using (MailMessage mail = new MailMessage())
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;

                    smtpClient.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Sending Error: " + ex.Message);
            }
        }

    }
}

