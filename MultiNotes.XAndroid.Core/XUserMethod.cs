using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

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

        public async Task Register(string username, string password)
        {
            IRegistration registration = new Registration();
            await registration.Register(username, password);
            IsRegisterSuccessful = registration.IsRegisterSuccessful;
            RegisterMessage = registration.RegisterMessage;
            // string token = await RegisterAutologin(username, password);
            // 
            // if (token != "")
            // {
            //     if (IsRegisterSuccessful)
            //     {
            //         try
            //         {
            //             System.IO.File.WriteAllLines(
            //                 Constants.AuthenticationRecordFile,
            //                 new string[]
            //                 {
            //                     UserSigned.Id,
            //                     UserSigned.EmailAddress,
            //                     UserSigned.PasswordHash,
            //                     UserSigned.RegistrationTimestamp.ToString()
            //                 }
            //             );
            //         }
            //         catch (Exception e)
            //         {
            //             string a = e.Message;
            //         }
            //     }
            // }
        }


        private async Task<string> RegisterAutologin(string username, string password)
        {
            ILoginEngine loginEngine = new LoginEngine();
            await loginEngine.Login(username, password);
            LoginMessage = loginEngine.LoginMessage;
            UserSigned = loginEngine.User;
            return loginEngine.Token;
        }


        public async Task Login(string username, string password)
        {
            ILoginEngine loginEngine = new LoginEngine();
            await loginEngine.Login(username, password);
            LoginMessage = loginEngine.LoginMessage;
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


        public async Task<bool> Verify(string username, string password)
        {
            ILoginEngine loginEngine = new LoginEngine();
            await loginEngine.Login(username, password, true);
            return loginEngine.User != null;
        }


        public void Logout()
        {
            System.IO.File.WriteAllLines(Constants.AuthenticationRecordFile, new string[] { "" });
        }


        public async Task<User> GetUser(string token, string username)
        {
            IUserApi userApi = new UserApi();
            return await userApi.GetUser(token, username);
        }
    }
}
