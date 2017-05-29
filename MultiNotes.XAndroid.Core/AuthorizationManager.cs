using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public class AuthorizationManager
    {
        private static readonly object syncRoot = new object();
        private static AuthorizationManager instance = null;

        public static AuthorizationManager Instance { get { return GetSingleton(); } }

        private static AuthorizationManager GetSingleton()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new AuthorizationManager();
                    }
                }
            }
            return instance;
        }
        

        private AuthorizationManager()
        {
        }


        private string[] GetFileData()
        {
            return System.IO.File.ReadAllLines(Constants.AuthenticationRecordFile);
        }


        public User User { get { return GetUser(); } }

        public bool IsUserSigned { get { return User != null; } }

        private User GetUser()
        {
            string[] data = GetFileData();
            if (data.Length != 5)
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


        public string Token { get { return GetToken(); } }

        private string GetToken()
        {
            string[] data = GetFileData();
            if (data.Length != 5)
            {
                return "";
            }
            return data[4];
        }
    }
}