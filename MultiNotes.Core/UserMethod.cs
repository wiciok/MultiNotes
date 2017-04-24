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
            if (response.StatusCode == HttpStatusCode.Created)
            {
                string[] lines = { email, passwordHash };
                System.IO.File.WriteAllLines("plik.txt", lines);
                preparedAuthenticationRecord();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Conflict- loginow,InternalServerError
            }
        }
        public void login(string email, string password)
        {
            string[] lines = { email, Encryption.Sha256(password) };
            System.IO.File.WriteAllLines("plik.txt", lines);
            preparedAuthenticationRecord();
        }

        public static async Task<User> getUserInfo(string token,string login)
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


        public async Task<string> getUniqueBsonId()
        {
            string product = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/id/");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                product = await response.Content.ReadAsAsync<string>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }

            return product;
        }

        public async Task deleteAccount()
        {
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            var user = getUserInfo(token, Record.Login);
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

        public async Task editAccount()
        {
            string token = await authenticationToken.PostAuthRecordAsync(Record);
            var user = getUserInfo(token, Record.Login);
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
