using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using System.IO;
using MultiNotes.Model;
using Newtonsoft.Json;

namespace MultiNotes.Core
{
    public class NoteMethod : INoteMethod
    {
        private static HttpClient _httpClient;
        private static readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MultiNotes", "notes.txt");

        public NoteMethod(HttpClient httpClient2)
        {
            _httpClient = httpClient2;
        }

        public void AddNoteToFile(Note note)
        {
            if (!File.Exists(Path))
            {
                var json = JsonConvert.SerializeObject(note);
                File.WriteAllText(Path, json);
            }
            else
            {
                var json = JsonConvert.SerializeObject(note);
                File.AppendAllText(Path, json);
            }
        }

        public async void AddNoteToDatabase(Note note, string token)
        {
            //zapis notatki do bazy danych        
            var response = await _httpClient.PostAsJsonAsync("api/note/" + token, note);
            {
                if (!(response.StatusCode.Equals(System.Net.HttpStatusCode.Created)) && !(response.StatusCode.Equals(System.Net.HttpStatusCode.OK)))
                {
                    throw new HttpResponseException(response.StatusCode);
                    //unauthorized,Forbidden,InternalServerError
                }
            }
        }

        public async Task<IEnumerable<Note>> GetAllNotesFromDatabase(string token)
        {
            //pobranie wszystkich notatek danego uzytkownika      
            IEnumerable<Note> allNotes;

            var response = await _httpClient.GetAsync("api/note/" + token);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                allNotes = await response.Content.ReadAsAsync<IEnumerable<Note>>();
            }
            else
            {
                if(response.StatusCode.Equals(System.Net.HttpStatusCode.NoContent))
                {
                    allNotes = new List<Note>();
                }
                else
                throw new HttpResponseException(response.StatusCode);
                //InternalServerError,Unauthorized
            }

            return allNotes;
        }
        public async Task<Note> GetNoteByIdFromDatabase(string token, string id)
        {
            //pobranie wszystkich notatek danego uzytkownika      
            Note newNote;

            var response = await _httpClient.GetAsync("api/note/" + token + "/" + id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                newNote = await response.Content.ReadAsAsync<Note>();
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
                //InternalServerError,Unauthorized,Forbidden,NotFound
            }
            return newNote;
        }

        public async Task<bool> DeleteNoteByIdFromDatabase(string token, string id)
        {
            var response = await _httpClient.DeleteAsync("api/note/" + token + "/" + id);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.NotFound:
                    return false;
                default:
                    throw new HttpResponseException(response.StatusCode);
            }
        }

        public async Task<bool> UpdateNoteByIdFromDatabase(string token, string id, Note newNote)
        {
            var response = await _httpClient.DeleteAsync("api/note/" + token + "/" + id);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    AddNoteToDatabase(newNote, token);
                    return true;
                case HttpStatusCode.NotFound:
                    return false;
                default:
                    throw new HttpResponseException(response.StatusCode);
            }
        }

        public List<Note> GetAllNotesFromFile(string userId)
        {
            if (File.Exists(Path))
            {
                var json = File.ReadAllText(Path);
                var tmpList = json.Split('}').ToList();
                for (var i = 0; i < tmpList.Count; ++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                var listNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
                return listNotes.Where(a => a.OwnerId == userId).ToList();
            }
            else
                return new List<Note>();
        }

        public Note GetNoteFromFile(string id, string userId)
        {
            if (File.Exists(Path))
            {
                var json = File.ReadAllText(Path);
                var tmpList = json.Split('}').ToList();
                for (int i = 0; i < tmpList.Count; ++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                var listNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
                return listNotes.FirstOrDefault(a => a.Id == id && a.OwnerId == userId);
            }
            else
                return null;
        }

        public void DeleteNoteFromFile(string id, string userId)
        {
            if (!File.Exists(Path))
                return;

            var json = File.ReadAllText(Path);
            var tmpList = json.Split('}').ToList();
            for (var i = 0; i < tmpList.Count; ++i)
            {
                tmpList[i] += "}";
            }
            tmpList.RemoveAt(tmpList.Count - 1);
            var listNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
            var toDelete = listNotes.FirstOrDefault(a => a.Id == id && a.OwnerId == userId);

            if (toDelete == null)
                return;
            listNotes.Remove(toDelete);
            File.WriteAllText(Path, "");
            foreach (var x in listNotes)
            {
                var jsonNote = JsonConvert.SerializeObject(x);
                File.AppendAllText(Path, json);
            }
        }

        public void UpdateNoteFromFile(string id, string userId, Note newNote)
        {
            if (!File.Exists(Path)) return;
            var json = File.ReadAllText(Path);
            var tmpList = json.Split('}').ToList();
            for (var i = 0; i < tmpList.Count; ++i)
            {
                tmpList[i] += "}";
            }
            tmpList.RemoveAt(tmpList.Count - 1);
            var listNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
            var toDelete = listNotes.FirstOrDefault(a => a.Id == id && a.OwnerId == userId);
            if (toDelete == null)
                return;
            {
                listNotes.Remove(toDelete);
                toDelete.Content = newNote.Content;
                listNotes.Add(toDelete);

                File.WriteAllText(Path, "");
                foreach (var x in listNotes)
                {
                    var jsonNote = JsonConvert.SerializeObject(x);
                    File.AppendAllText(Path, json);
                }
            }
        }

        public void CleanLocalNotes()
        {
            File.Delete(Path);
            File.OpenWrite(Path);
        }
    }
}
