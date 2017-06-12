using System;
using System.Net;
using System.Threading.Tasks;

namespace MultiNotes.Core.Util
{
    public class InternetConnection
    {
        public static async Task<bool> IsInternetConnectionAvailable()
        {
            try
            {
                var response = await ConnectionApi.HttpClient.GetAsync("api/id/");

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
