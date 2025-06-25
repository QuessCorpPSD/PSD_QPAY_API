
using Microsoft.Extensions.Configuration;
using QPay.UI.Common;
using System.Net;
using System.Net.Mail;

namespace QPay.BAL.Repository
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<EmailSensStatus> SendEmailAsync(string toEmail, string subject, string body)
        {
            EmailSensStatus emailSensStatus = new EmailSensStatus();
            try
            {

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("psd-appmailer@qazmail.quesscorp.com");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Priority=MailPriority.High;
                mail.Body = body;

                // Configure the SMTP client
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "qazmail.quesscorp.com";       // e.g., smtp.gmail.com
                smtp.Port = 587;                      // Or 25, or 465 depending on your provider
                smtp.EnableSsl = true;                // Use SSL
                smtp.UseDefaultCredentials=false;
                smtp.Credentials = new NetworkCredential("psd-appmailer@qazmail.quesscorp.com", "JbV-2e[7Wpua5Z](!jfN@K");
                ServicePointManager.Expect100Continue = true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // Send the email
                await smtp.SendMailAsync(mail);
                emailSensStatus.StatusCode=200;
                emailSensStatus.Message="Email sent successfully";
            }
            catch(Exception ex)
            {
                emailSensStatus.StatusCode=200;
                emailSensStatus.Message="Email sent failes";
                emailSensStatus.ErrorMessage=ex.Message;
            }

            return emailSensStatus;


        }
    }

}   


 