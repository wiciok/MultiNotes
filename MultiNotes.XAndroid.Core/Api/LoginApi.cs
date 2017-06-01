using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core.Api
{
    public class LoginApi : ILoginApi
    {
        public bool Success { get { return User != null; } }
        public string Message { get; private set; }
        public User User { get; private set; }
        public string Token { get; private set; }


        public LoginApi()
        {
            Message = "";
            User = null;
            Token = "";
        }


        /// <exception cref="WebApiClientException"></exception>
        public async Task Login(string username, string password, bool hashed = false)
        {
            AuthTokenApi tokenApi = new AuthTokenApi();
            string token = await tokenApi.GetAuthToken(new AuthenticationRecord()
            {
                Email = username,
                PasswordHash = hashed ? password : Encryption.Sha256(password)
            });
            token = token.Replace("\"", "");
            if (token == "")
            {
                User = null;
                Message = "Username or password is incorrect.";
                return;
            }
            Token = token;
            User = await new UserApi().GetUser(token, username);
            if (User == null)
            {
                Message = "An unknown exception occured.";
            }
        }
    }
}
