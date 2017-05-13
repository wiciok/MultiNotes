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
using Newtonsoft.Json;

namespace MultiNotes.Core
{
    public class NoteMethod
    {
        private static HttpClient httpClient;
        private string path = "notes.txt";
        public NoteMethod(HttpClient httpClient2)
        {
            httpClient = httpClient2;
        }

        public async void AddNoteToFile(Note note)
        {
            if (!File.Exists(path))
            {
                //var tmpList = new List<Note>();
                //tmpList.Add(note);
                var json = new JavaScriptSerializer().Serialize(note);
                File.WriteAllText(path, json);
            }
            else
            {
                //var tmpList = new List<Note>();
                //.Add(note);
                var json = new JavaScriptSerializer().Serialize(note);
                File.AppendAllText(path, json);
            }
        }
        public async void AddNoteToDatabase(Note note,string token)
        {
            //zapis notatki do bazy danych        
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/note/"+token, note);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpResponseException(response.StatusCode);
                //unauthorized,Forbidden,InternalServerError
            }
        }

        public async void GetAllNotesFromDatabase(Note note, string token)
        {
            //pobranie wszystkich notatek danego uzytkownika      
            IEnumerable<Note> product = null;
            HttpResponseMessage response = await httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<IEnumerable<Note>>();
            }
        }



        public async void testujemy()
        {
            var a = GetAllNotesFromFile("5909a1977a48172c8049e25c");
            var bb = GetNoteFromFile("591739a77a48173e1cc59b17", "5909a1977a48172c8049e25c");
            //DeleteNoteFromFile("591739a77a48173e1cc59b17");
            int b = 99;
        }

        public List<Note> GetAllNotesFromFile(string userId)
        {
            if (File.Exists(path))
            {
                List<Note> listNotes = new List<Note>();
                string json = File.ReadAllText(path);
                List<string>tmpList = json.Split('}').ToList();
                for (int i=0;i< tmpList.Count;++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                foreach (var x in tmpList)
                {
                    listNotes.Add(new JavaScriptSerializer().Deserialize<Note>(x));
                }
                return listNotes.Where(a=>a.OwnerId== userId).ToList();
            }
            else
            {
                return null;
            }

        }

        public Note GetNoteFromFile(string id, string userId)
        {
            if (File.Exists(path))
            {
                List<Note> listNotes = new List<Note>();
                string json = File.ReadAllText(path);
                List<string> tmpList = json.Split('}').ToList();
                for (int i = 0; i < tmpList.Count; ++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                foreach (var x in tmpList)
                {
                    listNotes.Add(new JavaScriptSerializer().Deserialize<Note>(x));
                }
                return listNotes.Where(a=>a.Id==id && a.OwnerId==userId).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public void DeleteNoteFromFile(string id, string userId)
        {
            if (File.Exists(path))
            {
                List<Note> listNotes = new List<Note>();
                string json = File.ReadAllText(path);
                List<string> tmpList = json.Split('}').ToList();
                for (int i = 0; i < tmpList.Count; ++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                foreach (var x in tmpList)
                {
                    listNotes.Add(new JavaScriptSerializer().Deserialize<Note>(x));
                }
                Note toDelte = listNotes.Where(a => a.Id == id && a.OwnerId== userId).FirstOrDefault();
                listNotes.Remove(toDelte);
                File.WriteAllText(path, "");
                foreach (var x in listNotes)
                {
                    var jsonNote = new JavaScriptSerializer().Serialize(x);
                    File.AppendAllText(path, json);
                }
            }
        }

        public void UpdateNoteFromFile(string id,string  userId,Note newNote)
        {
            if (File.Exists(path))
            {
                List<Note> listNotes = new List<Note>();
                string json = File.ReadAllText(path);
                List<string> tmpList = json.Split('}').ToList();
                for (int i = 0; i < tmpList.Count; ++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                foreach (var x in tmpList)
                {
                    listNotes.Add(new JavaScriptSerializer().Deserialize<Note>(x));
                }
                Note toDelete = listNotes.Where(a => a.Id == id && a.OwnerId==userId).FirstOrDefault();
                listNotes.Remove(toDelete);

                toDelete.Content = newNote.Content;
                toDelete.LastChangeTimestamp = newNote.LastChangeTimestamp;

                listNotes.Add(toDelete);

                File.WriteAllText(path, "");
                foreach (var x in listNotes)
                {
                    var jsonNote = new JavaScriptSerializer().Serialize(x);
                    File.AppendAllText(path, json);
                }
            }
        }


    }
}
