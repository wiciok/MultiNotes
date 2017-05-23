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
        private SmtpClient client;

        public MailingService()
        {
            client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Timeout = 2000;
            client.Host = "smtp.wp.pl";
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("projektor-test@wp.pl", "K9XyxRk9pzgeZrLUln7H");
        }

        public void SendEmail(string text, string subject, string targetEmailAddress)
        {
            MailMessage mail = new MailMessage(new MailAddress("projektor-test@wp.pl"), new MailAddress(targetEmailAddress));
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = text;
            client.Send(mail);
        }
    }
}
