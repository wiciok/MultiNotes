using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiNotes.Core;

namespace MultiNotes.Core
{
    public class UserMethod
    {
        private static HttpClient httpClient;
        private AuthenticationToken authenticationToken;
        public AuthenticationRecord Record { get; set; }
        public User user;
        public UserMethod(HttpClient httpClient2)
        {
            httpClient = httpClient2;
            authenticationToken = new AuthenticationToken(httpClient);
            Record = new AuthenticationRecord();
            user = new User();
        }
        public void preparedAuthenticationRecord()
        {
            string[] lines = System.IO.File.ReadAllLines("plik.txt");
            Record.Login = lines[0];
            Record.PasswordHash = lines[1];
        }
        public async Task registerAsync(string email, string password)
        {
            string BsonId = await getUniqueBsonId();
            string passwordHash = Encryption.Sha256(password);
            user.Id = BsonId;
            user.Login = email;
            user.PasswordHash = passwordHash;

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/user", user);
            if (response.StatusCode.ToString() == "Created")
            {
                string[] lines = { email, passwordHash};
                System.IO.File.WriteAllLines("plik.txt", lines);
                preparedAuthenticationRecord();
            }
            else if (response.StatusCode.ToString() == "NotFound")
            {
                //rzuc wyjatek
            }
            else if (response.StatusCode.ToString() == "InternalServerError")
            {
                //rzuc wyjatek
            }
            else if (response.StatusCode.ToString() == "Conflict")
            {
                //rzuc wyjatek
            }
            else
            {
                //rzuc wyjatek
            }
        }
        public void login(string email, string password)
        {
            string[] lines = { email, password };
            System.IO.File.WriteAllLines("plik.txt", lines);
            preparedAuthenticationRecord();
        }

        private static async Task<User> getUserInfo(string token,string login)
        {
            User user = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/token/" + token + "/" + login);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }


        private async Task<string> getUniqueBsonId()
        {
            string product = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/id/");
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<string>();
            }

            return product;
        }

        public async Task deleteAccount()
        {
            try
            {
                string token = await authenticationToken.PostAuthRecordAsync(Record);
                int id = getUserInfo(token, Record.Login).Id;
                HttpResponseMessage response = await httpClient.DeleteAsync("api/user/" + token + "/" + id);
            }
            catch(Exception kurwa)
            {
                int a = 8;
            }
        }
    }
}
