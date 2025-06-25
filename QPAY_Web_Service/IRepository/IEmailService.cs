

using QPay.UI.Common;

namespace QPay.BAL.Repository
{
    public interface IEmailService
    {
        Task<EmailSensStatus> SendEmailAsync(string toEmail, string subject, string body);
    }
}
