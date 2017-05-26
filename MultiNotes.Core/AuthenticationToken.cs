using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using MultiNotes.Model;

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
