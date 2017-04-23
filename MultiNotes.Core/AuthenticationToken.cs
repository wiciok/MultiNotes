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
    class AuthenticationToken
    {
        private static HttpClient httpClient;
        public AuthenticationToken(HttpClient httpClient2)
        {
            httpClient = httpClient2;
        }
        public async Task<string> PostAuthRecordAsync(AuthenticationRecord authRecord) //zwraca token w postaci stringa
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/auth", authRecord);
            string token = null;
            if (response.StatusCode.ToString() == "Created")
            {
                token= await response.Content.ReadAsAsync<string>();
            }
            else if (response.StatusCode.ToString() == "InternalServerError")
            {

            }
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                //co tutaj?
            }
            return token;
        }
    }
}
