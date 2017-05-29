﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            string token = await RegisterAutologin(username, password);

            if (token != "")
            {
                if (IsRegisterSuccessful)
                {
                    System.IO.File.WriteAllLines(
                        Constants.AuthenticationRecordFile,
                        new string[]
                        {
                            UserSigned.Id,
                            UserSigned.EmailAddress,
                            UserSigned.PasswordHash,
                            UserSigned.RegistrationTimestamp.ToString(),
                            token
                        }
                    );
                }
            }
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
                System.IO.File.WriteAllLines(
                    Constants.AuthenticationRecordFile,
                    new string[]
                    {
                        UserSigned.Id,
                        UserSigned.EmailAddress,
                        UserSigned.PasswordHash,
                        UserSigned.RegistrationTimestamp.ToString(),
                        loginEngine.Token
                    }
                );
            }
        }


        public async Task Logout()
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
