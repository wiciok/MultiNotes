using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public class RemoteNoteRepository : INoteRepository
    {
        private List<Note> remoteNotes;
        private string token;


        public RemoteNoteRepository()
        {
            // LoadNotes();
        }


        public bool Success { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }


        public List<Note> GetAllNotes()
        {
            LoadNotes();
            return remoteNotes;
        }


        private void LoadNotes()
        {
            string apiUrl = Constants.ApiUrlBase + "api/note/{0}";
            
            User signedUser = AuthorizationManager.Instance.User;

            LoadToken();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token)));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                HttpStatusCode = response.StatusCode;

                if (HttpStatusCode == HttpStatusCode.NoContent)
                {
                    remoteNotes = new List<Note>();
                    Success = true;
                    return;
                }

                Stream stream = response.GetResponseStream();
                string json = new StreamReader(stream).ReadToEnd();
                List<string> tmpList = json.Split('}').ToList();
                for (int i = 0; i < tmpList.Count; ++i)
                {
                    tmpList[i] += "}";
                }
                tmpList.RemoveAt(tmpList.Count - 1);
                remoteNotes = tmpList
                    .Select(JsonConvert.DeserializeObject<Note>)
                    .Where(a => a.OwnerId == signedUser.Id)
                    .ToList();
                Success = true;
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse response)
                {
                    HttpStatusCode = response.StatusCode;
                }
                else
                {
                    HttpStatusCode = 0;
                }
                Success = false;
                remoteNotes = null;
            }
        }


        private async void LoadToken()
        {
            User signedUser = AuthorizationManager.Instance.User;
            AuthenticationToken tokenManager = new AuthenticationToken();
            token = await tokenManager.GetAuthenticationToken(new AuthenticationRecord()
            {
                Email = signedUser.EmailAddress,
                PasswordHash = signedUser.PasswordHash
            });
        }


        public void AddNote(Note note)
        {
            PrepareNoteToAdd(ref note);
            LoadToken();

            string apiUrl = Constants.ApiUrlBase + "api/note/{0}";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token)));
            request.ContentType = "application/json";
            request.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(note);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                Success = true;
            }
            catch (WebException e)
            {
                Success = false;
                if (e.Response is HttpWebResponse response)
                {
                    HttpStatusCode = response.StatusCode;
                }
                else
                {
                    HttpStatusCode = 0;
                }
            }
        }


        private void PrepareNoteToAdd(ref Note note)
        {
            if (note.OwnerId == null)
            {
                note.OwnerId = AuthorizationManager.Instance.User.Id;
            }
            if (note.Id == null)
            {
                note.Id = new UniqueIdService().GetUniqueId();
            }
            if (note.LastChangeTimestamp == null)
            {
                note.LastChangeTimestamp = DateTime.Now;
            }
            if (note.CreateTimestamp == null)
            {
                note.CreateTimestamp = DateTime.Now;
                note.LastChangeTimestamp = DateTime.Now;
            }
        }


        public void UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }


        public void DeleteNote(Note note)
        {
            throw new NotImplementedException();
        }

    }
}