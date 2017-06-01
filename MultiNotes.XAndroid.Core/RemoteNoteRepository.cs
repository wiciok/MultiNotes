using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MultiNotes.Model;
using MultiNotes.XAndroid.Core.Api;

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


        /// <exception cref="WebApiClientException"></exception>
        public List<Note> GetAllNotes()
        {
            LoadNotes();
            return remoteNotes;
        }


        /// <exception cref="WebApiClientException"></exception>
        private void LoadNotes()
        {
            string apiUrl = Constants.ApiUrlBase + "api/note/{0}";
            
            User signedUser = new Authorization().User;

            LoadToken();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token)));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.NoContent)
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
                object notes = JsonConvert.DeserializeObject(json);
                if (notes is JArray jArray)
                {
                    remoteNotes = jArray.ToObject<List<Note>>()
                        .Where(g => g.OwnerId == signedUser.Id).ToList();
                }
                // remoteNotes = tmpList
                //     .Select(JsonConvert.DeserializeObject<Note>)
                //     .Where(a => a.OwnerId == signedUser.Id)
                //     .ToList();

                Success = true;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                {
                    throw new WebApiClientException(WebApiClientError.InternetConnectionError);
                }
                Success = false;
                remoteNotes = null;
            }
        }

        /// <exception cref="WebApiClientException"></exception>
        private async void LoadToken()
        {
            User signedUser = new Authorization().User;
            AuthTokenApi tokenManager = new AuthTokenApi();

            // This method throws WebApiClientException
            token = await tokenManager.GetAuthToken(new AuthenticationRecord()
            {
                Email = signedUser.EmailAddress,
                PasswordHash = signedUser.PasswordHash
            });
        }


        /// <exception cref="WebApiClientException"></exception>
        public void AddNote(Note note)
        {
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
                if (e.Status == WebExceptionStatus.ConnectFailure)
                {
                    throw new WebApiClientException(WebApiClientError.InternetConnectionError);
                }
                Success = false;
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