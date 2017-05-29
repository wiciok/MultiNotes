using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MultiNotes.Model;

namespace MultiNotes.Core.Util
{
    class InternetConnection
    {
        public static async Task<bool> IsInternetConnectionAvailable()
        {
            var response = await ConnectionApi.HttpClient.GetAsync("api/id/");

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }
    }
}
