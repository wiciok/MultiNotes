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

        public string Content { get; set; }

        public DateTime CreateTimestamp { get; }

        public DateTime LastChangeTimestamp { get; set; }
    }


    public class Program
    {
        private static HttpClient httpClient = new HttpClient();
        private static int port;

        private static string ToString(Note note)
        {
            return note.Id + " \"" + note.Content + "\" " + note.LastChangeTimestamp;
        }

        public static int Main(string[] args)
        {
            // Pobranie portu - przydatne, bo nie trzeba będzie zmieniać kodu przy 
            // każdorazowym uruchomieniu programu.
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

            // Wywołanie metody, która wykona wszystkie wymagane przez nas operacje.

            RunAsync().Wait();

            // Wciśnij enter by zakończyć :)
            Console.ReadLine();
            return 0;
        }

        private static async Task RunAsync()
        {
            httpClient.BaseAddress = new Uri("http://localhost:" + port);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Note note = new Note() { Id = "10", Content = "Zjedz śniadanie!", LastChangeTimestamp = DateTime.Now };

            // Operacja POST - wysłanie notatki na serwer.
            await PostProductAsync(note);

            // Operacja GET - pobieram notatkę wcześniej wstawioną na serwer.
            Note product = await GetProductAsync("api/simplynote/" + note.Id);
            Console.WriteLine(ToString(product));
        }

        // Metoda wykonująca operację GET - pobiera notatkę z podanego adresu API.
        private static async Task<Note> GetProductAsync(string path)
        {
            Note product = null;
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Note>();
            }
            return product;
        }

        // Metoda wykonująca operację POST - wysyła notatke na serwer.
        private static async Task<object> PostProductAsync(Note note)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/simplynote", note);
            response.EnsureSuccessStatusCode();
            return null;

        }
    }
}
