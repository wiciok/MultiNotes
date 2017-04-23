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
        public UserMethod(HttpClient httpClient2)
        {
            httpClient = httpClient2;
            authenticationToken = new AuthenticationToken(httpClient);
            Record = new AuthenticationRecord();
        }
        public async Task registerAsync(string email, string password)
        {
            string BsonId = await getUniqueBsonId();
            string passwordHash = Encryption.Sha256(password);
            User newUser = new User(){ Id = BsonId, Email =email, Name = "", Surname = "", PasswordHash = passwordHash };

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/user", newUser);
            if (response.StatusCode.ToString() == "Created")
            {
                Record.UserId = BsonId;
                Record.PasswordHash = passwordHash;
            }
            else if(response.StatusCode.ToString() == "NotFound")
            {

            }
            else if(response.StatusCode.ToString() == "InternalServerError")
            {
                
            }
            else if (response.StatusCode.ToString() == "Conflict")
            {

            }
            else
            {

            }
            response.EnsureSuccessStatusCode();
        }
        private static async Task loginAsync(string email, string password)
        {
           //to kiedys musi powstac
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

        public async Task deleteAccount(User user)
        {
            AuthenticationRecord authStructure = new AuthenticationRecord { UserId = user.Id, PasswordHash = user.PasswordHash };
            string token= await authenticationToken.PostAuthRecordAsync(authStructure);
            HttpResponseMessage response = await httpClient.DeleteAsync(token);
        }
    }
}
