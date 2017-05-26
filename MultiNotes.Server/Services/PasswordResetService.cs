using System.Collections.Generic;
using System.Linq;
using MultiNotes.Model;

namespace MultiNotes.Server.Services
{
    //todo: mozna dorobic zapezpieczenia zeby tokeny wygasały z czasem
    //mozna zamienic passwordresetrecord na tuple

    public class PasswordResetRecord
    {
        public PasswordResetRecord(User user, string token)
        {
            User = user;
            Token = token;
        }
        public User User { get; }
        public string Token { get; }
    }

    public class PasswordResetService //singleton
    {
        private PasswordResetService()
        {
            RecordList = new List<PasswordResetRecord>();
        }

        private static PasswordResetService _instance;
        public static PasswordResetService Instance => _instance ?? (_instance = new PasswordResetService());

        public List<PasswordResetRecord> RecordList { get; }


        public void SendEmailWithToken(string email)
        {
            var token = RandomTokenGenerator.GenerateUniqueToken();

            IMailingService mailingService = new MailingService();

            var serverAddress = ServerAddressService.ServerAddress;
            var msg = "Twój link do resetu hasła to: <a href = 'http://" + serverAddress +
                "/ResetPassword/Reset/" + token + "'>http://" + serverAddress + "/ResetPassword/Reset/" + token + "</a>";

            mailingService.SendEmail(msg, "MultiNotes - Password Reset", email);
            RecordList.Add(new PasswordResetRecord(UnitOfWork.Instance.UsersRepository.GetUserByEmail(email), token));
        }

        public string ResetUserPassword(string token)
        {
            PasswordResetRecord record;
            if ((record = RecordList.SingleOrDefault(x => x.Token == token)) == null)
                return null;

            const int newPasswordLength = 20;
            var newPassword = RandomTokenGenerator.GenerateRandomString(newPasswordLength);

            record.User.PasswordHash = Encryption.Sha256(newPassword);
            UnitOfWork.Instance.UsersRepository.UpdateUser(record.User.Id, record.User);

            RecordList.Remove(record);

            return newPassword;
        }
    }
}