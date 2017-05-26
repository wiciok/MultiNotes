using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiNotes.Core;
using MultiNotes.Model;

namespace MultiNotes.Server.Services
{
    //todo: mozna dorobic zapezpieczenia zeby tokeny wygasały z czasem
    //mozna zamienic passwordresetrecord na tuple

    public class PasswordResetRecord
    {
        public PasswordResetRecord(User user, string token)
        {
            this.User = user;
            this.Token = token;
        }
        public User User { get; private set; }
        public string Token { get; private set; }
    }

    public class PasswordResetService //singleton
    {
        private PasswordResetService()
        {
            RecordList = new List<PasswordResetRecord>();
        }

        private static PasswordResetService _instance;
        public static PasswordResetService Instance => _instance ?? (_instance = new PasswordResetService());

        public List<PasswordResetRecord> RecordList { get; private set; }


        public void SendEmailWithToken(string email)
        {
            string token = RandomTokenGenerator.GenerateUniqueToken();

            IMailingService mailingService = new MailingService();

            string serverAddress = ServerAddressService.ServerAddress;
            string msg = "Twój link do resetu hasła to: <a href = 'http://" + serverAddress +
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
            string newPassword = RandomTokenGenerator.GenerateRandomString(newPasswordLength);

            record.User.PasswordHash = Encryption.Sha256(newPassword);
            UnitOfWork.Instance.UsersRepository.UpdateUser(record.User.Id, record.User);

            RecordList.Remove(record);

            return newPassword;
        }
    }
}