//#define DEBUG_CONFIGURATION
using ModernHttpClient;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MultiNotes.Core
{
    public class ConnectionApi//singleton???
    {
#if DEBUG_CONFIGURATION
        private const int Port = 63252;
        private const string DbAddress = "http://localhost:63252/";
#else
        //private const int Port = 80;
        private const string DbAddress = "http://217.61.4.233:8080/MultiNotes.Server/";
#endif

        public static HttpClient HttpClient = new HttpClient(new NativeMessageHandler());  
        private static bool _configured;
        public static void Configure()
        {
            if (_configured)
            {
                return;
            }

            HttpClient.BaseAddress = new Uri(DbAddress);// + Port);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _configured = true;
        }
    }
}