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
        public UserMethod(HttpClient httpClient2)
        {
            httpClient = httpClient2;
        }
        public async Task registerAsync(string email, string password)
        {
            string BsonId = await getUniqueBsonId();
            string passwordHash = Encryption.Sha256(password);
            User newUser = new User()
            { Id = BsonId, Email = "a", Name = "a", Surname = "a", PasswordHash = "b" };

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/user", newUser);
            response.EnsureSuccessStatusCode();
        }

        private static async Task<string> getUniqueBsonId()
        {
            string product = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/id/");
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<string>();
            }

            return product;
        }

    }
}
