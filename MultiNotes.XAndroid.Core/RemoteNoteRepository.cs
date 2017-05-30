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

        public List<Note> GetAllNotes()
        {
            LoadNotes();
            return remoteNotes;
        }


        private async void LoadNotes()
        {
            string apiUrl = Constants.ApiUrlBase + "api/note/{0}";

            // ILoginEngine loginEngine = new LoginEngine();
            User signedUser = AuthorizationManager.Instance.User;
            // loginEngine.Login(signedUser.EmailAddress, signedUser.PasswordHash, true);
            // 
            // if (loginEngine.Token == "" || loginEngine.User == null)
            // {
            //     return null;
            // }

            AuthenticationToken tokenManager = new AuthenticationToken();
            string token = await tokenManager.GetAuthenticationToken(new AuthenticationRecord()
            {
                Email = signedUser.EmailAddress,
                PasswordHash = signedUser.PasswordHash
            });

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token)));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        string json = new StreamReader(stream).ReadToEnd();
                        List<string> tmpList = json.Split('}').ToList();
                        for (int i = 0; i < tmpList.Count; ++i)
                        {
                            tmpList[i] += "}";
                        }
                        tmpList.RemoveAt(tmpList.Count - 1);
                        remoteNotes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
                        remoteNotes = remoteNotes.Where(a => a.OwnerId == signedUser.Id).ToList();
                    }
                }
            }
            catch (WebException e)
            {
                string a = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                remoteNotes = new List<Note>();
            }
        }


        public void AddNote(Note note)
        {

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