﻿using System;
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
        public static readonly string GuestEmailAddress = "guest@multinotes.pl";
        public static readonly string GuestPasswordHash = "###";
        public static DateTime GuestRegistrationTimestamp { get { return DateTime.Now; } }

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


        public bool IsUserSigned { get { return User != null; } }

        public string UserId { get { return User?.Id ?? GuestId; } }
        public string UserEmailAddress { get { return User?.EmailAddress ?? GuestEmailAddress; } }
        public string UserPasswordHash { get { return User?.PasswordHash ?? GuestPasswordHash; } }
        public DateTime UserRegistrationTimestamp { get { return User?.RegistrationTimestamp ?? GuestRegistrationTimestamp; } }

        public User User { get { return StaticUser; } }
        

        public void ReloadUser()
        {
            lock (GuestId)
            {
                StaticUser = GetUser();
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
        

        /// <exception cref="WebApiClientException"></exception>
        public bool Verify()
        {
            return new XUserMethod().Verify(User.EmailAddress, User.PasswordHash);
        }
    }
}
