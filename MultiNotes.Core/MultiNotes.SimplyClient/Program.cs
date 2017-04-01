using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MultiNotes.SimplyClient
{
    public class Note
    {
        public string Id { get; set; }

        // Tutaj byc moze bedzie referencja do wlasciciela notatki.

        // Na razie niepotrzebne.
        // public string OwnerId { get; set; }

        public string Content { get; set; }
        public DateTime CreateTimestamp { get; }
        public DateTime LastChangeTimestamp { get; set; }

        public override string ToString()
        {
            return Id + " \"" + Content + "\" " + LastChangeTimestamp;
        }
    }


    public class Program
    {
        private static HttpClient httpClient = new HttpClient();
        private static int port;

        public static int Main(string[] args)
        {
            try
            {
                Console.Write("Port: ");
                port = int.Parse(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
                return 1;
            }

            RunAsync().Wait();

            return 0;
        }

        private static async Task RunAsync()
        {
            httpClient.BaseAddress = new Uri("http://localhost:" + port);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Note product = await GetProductAsync(httpClient.BaseAddress.ToString() + "api/simplynote/1");

            Console.WriteLine(product);

            Console.ReadLine();
        }

        static async Task<Note> GetProductAsync(string path)
        {
            Note product = null;
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Note>();
            }
            return product;
        }
    }
}
