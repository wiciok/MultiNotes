using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using MultiNotes.Model;
using System;

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998 

namespace MultiNotes.Core
{
    public class AuthenticationToken
    {
        private static HttpClient _httpClient;
        public AuthenticationToken(HttpClient httpClient2)
        {
            _httpClient = httpClient2;
        }
        public async Task<string> PostAuthRecordAsync(AuthenticationRecord authRecord) //zwraca token w postaci stringa
        {
            HttpResponseMessage response;
            try
            {
                response = _httpClient.PostAsJsonAsync("api/auth", authRecord).Result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            string token;
            if (response.StatusCode== HttpStatusCode.OK)
            {
                token= await response.Content.ReadAsAsync<string>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //Unauthorized,InternalServerError
            }
            return token;
        }
    }
}
