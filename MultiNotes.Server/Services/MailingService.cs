using System.Net.Mail;
using System.Text;

namespace MultiNotes.Server.Services
{
    public interface IMailingService
    {
        void SendEmail(string text, string subject, string targetEmailAdress);
    }

    public class MailingService : IMailingService
    {
        private readonly SmtpClient _client;

        public MailingService()
        {
            _client = new SmtpClient
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Timeout = 2000,
                Host = "smtp.wp.pl",
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential("projektor-test@wp.pl", "K9XyxRk9pzgeZrLUln7H")
            };
        }

        public void SendEmail(string text, string subject, string targetEmailAddress)
        {
            MailMessage mail = new MailMessage(new MailAddress("projektor-test@wp.pl"),
                new MailAddress(targetEmailAddress))
            {
                SubjectEncoding = Encoding.UTF8,
                Subject = subject,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                Body = text
            };
            _client.Send(mail);
        }
    }
}
