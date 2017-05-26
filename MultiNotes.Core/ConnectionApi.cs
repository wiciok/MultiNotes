using System;
using System.Net.Http;
using System.Net.Http.Headers;
namespace MultiNotes.Core
{
    public class ConnectionApi//singleton???
    {
        public static HttpClient httpClient = new HttpClient();
        private static int port = 63252;
        private static bool configured = false;
        public static void configure()
        {
            if (!configured)
            {
                httpClient.BaseAddress = new Uri("http://localhost:" + port);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                configured = true;
            }
        }
    }
}