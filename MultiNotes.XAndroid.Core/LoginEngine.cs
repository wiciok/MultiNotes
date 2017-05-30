using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using MultiNotes.Model;

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998

namespace MultiNotes.XAndroid.Core
{
    public class LoginEngine : ILoginEngine
    {
        public bool IsLoginSuccessful { get { return User != null; } }
        public string LoginMessage { get; private set; }
        public User User { get; private set; }
        public string Token { get; private set; }


        public LoginEngine()
        {
            LoginMessage = "";
            User = null;
            Token = "";
        }


        public async Task Login(string username, string password, bool hashed)
        {
            AuthenticationToken tokenApi = new AuthenticationToken();
            string token = await tokenApi.GetAuthenticationToken(new AuthenticationRecord()
            {
                Email = username,
                PasswordHash = hashed ? password : Encryption.Sha256(password)
            });
            token = token.Replace("\"", "");
            if (token == "")
            {
                User = null;
                LoginMessage = "Username or password is incorrect.";
                return;
            }
            Token = token;
            User = await new UserApi().GetUser(token, username);
            if (User == null)
            {
                LoginMessage = "An unknown exception occured.";
            }
        }
    }
}
