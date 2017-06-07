using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;
using MultiNotes.XAndroid.Core.Api;

namespace MultiNotes.XAndroid.Core
{
    public class XUserMethod
    {
        public bool IsRegisterSuccessful { get; private set; }
        public string RegisterMessage { get; private set; }


        public bool IsLoginSuccessful { get { return UserSigned != null; } }
        public string LoginMessage { get; private set; }
        public User UserSigned { get; private set; }
        
        public XUserMethod()
        {
            IsRegisterSuccessful = false;
            UserSigned = null;

            string directoryName = System.IO.Path.GetDirectoryName(Constants.AuthenticationRecordFile);
            if (!System.IO.Directory.Exists(directoryName))
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }
        }

        /// <exception cref="WebApiClientException"></exception>
        public void Register(string username, string password)
        {
            IRegisterApi registration = new RegisterApi();
            registration.Register(username, password);
            IsRegisterSuccessful = registration.IsRegisterSuccessful;
            RegisterMessage = registration.RegisterMessage;
        }


        /// <exception cref="WebApiClientException"></exception>
        private string RegisterAutologin(string username, string password)
        {
            ILoginApi loginEngine = new LoginApi();
            loginEngine.Login(username, password);
            LoginMessage = loginEngine.Message;
            UserSigned = loginEngine.User;
            return loginEngine.Token;
        }


        /// <exception cref="WebApiClientException"></exception>
        public void Login(string username, string password)
        {
            ILoginApi loginEngine = new LoginApi();
            loginEngine.Login(username, password);
            LoginMessage = loginEngine.Message;
            UserSigned = loginEngine.User;
            if (IsLoginSuccessful)
            {
                try
                {
                    System.IO.File.WriteAllLines(
                        Constants.AuthenticationRecordFile,
                        new string[]
                        {
                        UserSigned.Id,
                        UserSigned.EmailAddress,
                        UserSigned.PasswordHash,
                        UserSigned.RegistrationTimestamp.ToString()
                        }
                    );
                }
                catch (Exception e)
                {
                    string a = e.Message;
                }
            }
        }


        /// <exception cref="WebApiClientException"></exception>
        public bool Verify(string username, string password)
        {
            ILoginApi loginEngine = new LoginApi();
            loginEngine.Login(username, password, true);
            return loginEngine.User != null;
        }


        public void Logout()
        {
            System.IO.File.WriteAllLines(Constants.AuthenticationRecordFile, new string[] { "" });
            new Authorization().ReloadUser();
        }


        /// <exception cref="WebApiClientException"></exception>
        public User GetUser(string token, string username)
        {
            return new UserApi().GetUser(token, username);
        }
    }
}
