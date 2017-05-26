#define DEBUG_CONFIGURATION
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MultiNotes.Core
{
    public class ConnectionApi//singleton???
    {
#if DEBUG_CONFIGURATION
        private static int port = 63252;
        private static string DBAddress = "http://localhost:";
#else
        private static int port = 80;
        private static string DBAddress = "http://217.61.4.233:8080/MultiNotes.Server/";
#endif

        public static HttpClient httpClient = new HttpClient();  
        private static bool configured = false;
        public static void configure()
        {
            if (!configured)
            {
                httpClient.BaseAddress = new Uri(DBAddress + port);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                configured = true;
            }
        }
    }
}