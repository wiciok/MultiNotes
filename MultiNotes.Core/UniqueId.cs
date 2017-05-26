using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;

namespace MultiNotes.Core
{
    public class UniqueId
    {
        public static async Task<string> GetUniqueBsonId(HttpClient httpClient)
        {
            string product = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/id/");

            if (response.StatusCode == HttpStatusCode.Created)
            {
                product = await response.Content.ReadAsAsync<string>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //InternalServerError
            }
            return product;
        }
    }
}
