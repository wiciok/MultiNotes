﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MultiNotes.Model;
using MultiNotes.XAndroid.Core.Api;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Core
{
    internal class RemoteNoteRepository
    {
        private List<Note> remoteNotes;


        public RemoteNoteRepository()
        {
            // LoadNotes();
        }


        public bool Success { get; private set; }


        /// <exception cref="WebApiClientException"></exception>
        public List<Note> GetAllNotes(string token)
        {
            LoadNotes(token);
            return remoteNotes;
        }


        /// <exception cref="WebApiClientException"></exception>
        private void LoadNotes(string token)
        {
            string apiUrl = Constants.ApiUrlBase + "api/note/{0}";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token)));
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
                        .Where(g => g.OwnerId == new Authorization().UserId).ToList();
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
        /// <exception cref="UserNotSignedException"></exception>
        private string GetToken()
        {
            Authorization auth = new Authorization();
            AuthTokenApi tokenManager = new AuthTokenApi();

            if (auth.IsUserSigned)
            {
                throw new UserNotSignedException();
            }

            // This method throws WebApiClientException
            return tokenManager.GetAuthToken(new AuthenticationRecord()
            {
                Email = auth.UserEmailAddress,
                PasswordHash = auth.UserPasswordHash
            });
        }


        /// <exception cref="WebApiClientException"></exception>
        /// <exception cref="UserNotSignedException"></exception>
        public void AddNote(Note note, string token)
        {
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


        /// <exception cref="WebApiClientException"></exception>
        /// <exception cref="UserNotSignedException"></exception>
        public void UpdateNote(Note note, string token)
        {
            // The sam api as AddNote
            AddNote(note, token);
        }


        public void DeleteNote(Note note, string token)
        {
            string apiUrl = Constants.ApiUrlBase + "api/note/{0}/{1}";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token, note.Id)));
            request.Method = "DELETE";

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
                if (e.Response is HttpWebResponse response)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        // Do nothing
                        Success = true;
                    }
                }
                else
                {
                    if (e.Status == WebExceptionStatus.ConnectFailure)
                    {
                        throw new WebApiClientException(WebApiClientError.InternetConnectionError);
                    }
                    Success = false;
                }
            }
        }
    }
}