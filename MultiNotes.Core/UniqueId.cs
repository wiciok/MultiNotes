using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System;

namespace MultiNotes.Core
{
    public static class UniqueId
    {
        public static async Task<string> GetUniqueBsonId(HttpClient httpClient)
        {
            string product;
            HttpResponseMessage response;
            try
            {
                response = httpClient.GetAsync("api/id/").Result;
            }
            catch (Exception e)
            {
                return null;
            }
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
