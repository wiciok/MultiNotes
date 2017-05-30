using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MultiNotes.Model;
using System.Net;
using System.IO;

using Newtonsoft.Json;

namespace MultiNotes.XAndroid.Core
{
    public class RemoteNoteRepository : INoteRepository
    {

        public List<Note> GetAllNotes()
        {
            const string apiUrl = "http://217.61.4.233:8080/MultiNotes.Server/api/note/{0}";
            List<Note> notes;

            ILoginEngine loginEngine = new LoginEngine();
            User signedUser = AuthorizationManager.Instance.User;
            loginEngine.Login(signedUser.EmailAddress, signedUser.PasswordHash, true);

            if (loginEngine.Token == "" || loginEngine.User == null)
            {
                return null;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, loginEngine.Token)));
            request.ContentType = "application/json";
            request.Method = "GET";

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
                    notes = tmpList.Select(JsonConvert.DeserializeObject<Note>).ToList();
                    return notes.Where(a => a.OwnerId == signedUser.Id).ToList();
                }
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