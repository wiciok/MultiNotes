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

namespace MultiNotes.Core
{
    public class NoteMethod
    {
        private static HttpClient httpClient;
        public NoteMethod(HttpClient httpClient2)
        {
            httpClient = httpClient2;
        }
        public async void AddNote(string userId, string text)
        {
            string BsonId = await UniqueId.GetUniqueBsonId(httpClient);
            Note note = new Note() { Id = BsonId, OwnerId = userId, Content = text, LastChangeTimestamp = DateTime.Now };

            //save note to file
            using (var fileStream = File.Open("notes.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter("notes.txt", true))
                {
                    sw.WriteLine(userId);
                    sw.WriteLine(text);
                }
            }
            //save note to database
        }


    }
}
