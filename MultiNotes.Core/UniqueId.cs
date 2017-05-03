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
