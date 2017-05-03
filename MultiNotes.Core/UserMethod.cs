using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiNotes.Core;
using System.Net;
using System.Web.Http;

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
        public void PreparedAuthenticationRecord()
        {
            string[] lines = System.IO.File.ReadAllLines("plik.txt");
            Record.Login = lines[0];
            Record.PasswordHash = lines[1];
        }
        public async Task register(string email, string password)
        {
            string BsonId = null;

            BsonId = await UniqueId.GetUniqueBsonId(httpClient);
            string passwordHash = Encryption.Sha256(password);
            user.Id = BsonId;
            user.Login = email;        
            user.PasswordHash = passwordHash;

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/user", user);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                string[] lines = { email, passwordHash };
                System.IO.File.WriteAllLines("plik.txt", lines);
                PreparedAuthenticationRecord();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Conflict- loginow,InternalServerError
            }
        }
        public async Task login(string email, string password)
        {
            //logowanie do pliku
            string[] lines = { email, Encryption.Sha256(password) };
            System.IO.File.WriteAllLines("plik.txt", lines);
            PreparedAuthenticationRecord();

            //wypelnienie uzytkownika
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            user = await GetUserInfo(token, Record.Login);
        }

        public async Task<User> GetUserInfo(string token,string login)
        {
            User user = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/user/" + token + "/" + login);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError
            }
            return user;
        }

        public async Task DeleteAccount()
        {
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            var user = GetUserInfo(token, Record.Login);
            HttpResponseMessage response = await httpClient.DeleteAsync("api/user/" + token + "/" + user.Result.Id);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                ;
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError
            }
        }

        public async Task EditAccount()
        {
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            var user = GetUserInfo(token, Record.Login);
            HttpResponseMessage response = await httpClient.PutAsJsonAsync("api/user/"+token, user);

            if(response.StatusCode == HttpStatusCode.OK)
            {

            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Forbidden,Unauthorized,InternalServerError
            }
        }

    }
}
