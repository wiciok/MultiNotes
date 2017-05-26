using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MultiNotes.Core
{
    class ConnectionApi//singleton???
    {
        public static HttpClient httpClient = new HttpClient();
        private static int port = 80;
        public static void configure()
        {
            httpClient.BaseAddress = new Uri("http://217.61.4.233:8080/MultiNotes.Server/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
