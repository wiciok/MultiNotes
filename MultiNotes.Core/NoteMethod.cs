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
using System.IO;
using System.Web.Script.Serialization;

namespace MultiNotes.Core
{
    public class NoteMethod
    {
        private static HttpClient httpClient;
        public NoteMethod(HttpClient httpClient2)
        {
            httpClient = httpClient2;
        }
        public async void AddNote(Note note,string token)
        {
            //zapis notatki do pliku
            string path = "notes.txt";
            if (!File.Exists(path))
            {
                var json = new JavaScriptSerializer().Serialize(note);
                File.WriteAllText(path, json);
            }
            else
            {
                var json = new JavaScriptSerializer().Serialize(note);
                File.AppendAllText(path, json);
            }

            //zapis notatki do bazy danych        
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/note/"+token, note);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpResponseException(response.StatusCode);
                //unauthorized,Forbidden,InternalServerError
            }
        }


    }
}
