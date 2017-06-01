using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MultiNotes.Model;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Core
{
    public class Authorization
    {
        public static readonly string GuestId = "-1";
        private static User StaticUser = null;

        public Authorization()
        {
            if (StaticUser == null)
            {
                lock (GuestId)
                {
                    ReloadUser();
                }
            }
        }


        private string[] GetFileData()
        {
            if (!System.IO.File.Exists(Constants.AuthenticationRecordFile))
            {
                System.IO.File.WriteAllText(Constants.AuthenticationRecordFile, "");
            }
            return System.IO.File.ReadAllLines(Constants.AuthenticationRecordFile);
        }


        public string UserId { get { return User != null ? User.Id : GuestId ; } }


        public User User { get { return StaticUser; } }


        public bool IsUserSigned { get { return User != null; } }


        public void ReloadUser()
        {
            lock (GuestId)
            {
                if (StaticUser == null)
                {
                    StaticUser = GetUser();
                }
            }
        }


        private User GetUser()
        {
            string[] data = GetFileData();
            if (data.Length != 4)
            {
                return null;
            }
            return new User()
            {
                Id = data[0],
                EmailAddress = data[1],
                PasswordHash = data[2],
                RegistrationTimestamp = DateTime.Parse(data[3])
            };
        }
        

        private bool userVerified;

        public bool Verify()
        {
            LoadVerificationUser();
            return userVerified;
        }
        

        private async void LoadVerificationUser()
        {
            userVerified = await new XUserMethod().Verify(User.EmailAddress, User.PasswordHash);
        }
    }
}