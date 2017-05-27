using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using MultiNotes.Model;

namespace MultiNotes.Core
{
    class AuthenticationToken
    {
        private static HttpClient _httpClient;
        public AuthenticationToken(HttpClient httpClient2)
        {
            _httpClient = httpClient2;
        }
        public async Task<string> PostAuthRecordAsync(AuthenticationRecord authRecord) //zwraca token w postaci stringa
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth", authRecord);
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
