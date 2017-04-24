using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiNotes.Core;

namespace MultiNotes.SimplyClient
{
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
            /*// Pobranie portu - przydatne, bo nie trzeba będzie zmieniać kodu przy 
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
            }*/
            port = 63252;

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

            //todo: dorobić sensowne liczenie hasza
            User newUser = new User() { Id = await GetIdAsync(), Login = "test", Name = "test", Surname = "test", PasswordHash = "test" };


            string BsonId = await GetIdAsync();

            Note note = new Note() { Id = BsonId, OwnerId = newUser.Id, Content = "Zjedz śniadanie!", LastChangeTimestamp = DateTime.Now };


            //utworzenie usera
            await PostUserAsync(newUser);


            //wyslanie notatki - STARE API NOTE, tylko do testu, dopoki nie ma POSTa w tokenie!!!
            await PostNoteAsync(note);

            AuthenticationRecord authStructure = new AuthenticationRecord { Login = newUser.Login, PasswordHash = newUser.PasswordHash };
            //wysłanie authStructure i odebranie tokena w odpowiedzi
            string token = await PostAuthRecordAsync(authStructure);

            IEnumerable<Note> receivedNotesList = await GetTokenNotesListAsync("api/token/" + token);

            foreach (var el in receivedNotesList)
                Console.WriteLine(el.ToString());

            Console.WriteLine("-------teraz pojedyncze notki po id-----------");

            Note receivedNote = await GetTokenNoteAsync("api/token/" + token + "/" + note.Id);

            Console.WriteLine(receivedNote.ToString());
            /*// Operacja POST - wysłanie notatki na serwer.
            await PostNoteAsync(note);


            // Operacja GET - pobieram notatkę wcześniej wstawioną na serwer.
            Console.WriteLine("/api/note:");
            Note product = await GetNoteAsync("api/note/" + BsonId);
            Console.WriteLine(ToString(product));*/

        }

        // Metoda wykonująca operację GET - pobiera notatkę z podanego adresu API.
        private static async Task<Note> GetNoteAsync(string path)
        {
            Note product = null;
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Note>();
            }
            return product;
        }

        private static async Task<string> GetIdAsync()
        {
            string product = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/id/");
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<string>();
            }

            return product;
        }

        // Metoda wykonująca operację GET - pobiera notatkę z podanego adresu API.
        private static async Task<IEnumerable<Note>> GetTokenNotesListAsync(string path)
        {
            IEnumerable<Note> product = null;
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<IEnumerable<Note>>();
            }
            return product;
        }

        // Metoda wykonująca operację GET - pobiera notatkę z podanego adresu API.
        private static async Task<Note> GetTokenNoteAsync(string path)
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
        private static async Task<object> PostNoteAsync(Note note)
        {
            //todo: jesli bedziemy przechowywac referencje do wlasciciela w notatce to tutaj nie moze sie wysylac ta referencja
            //a moze sie nie wysyla domyslnie? trzeba sprawdzic
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/note", note);
            response.EnsureSuccessStatusCode();
            return null;
        }

        private static async Task<object> PostUserAsync(User user)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/user", user);
            response.EnsureSuccessStatusCode();
            return null;
        }

        private static async Task<string> PostAuthRecordAsync(AuthenticationRecord authRecord) //zwraca token w postaci stringa
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/auth", authRecord);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<string>();
        }
    }
}
