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
    public class Registration : IRegistration
    {
        public bool IsRegisterSuccessful { get; private set; }
        public string RegisterMessage { get; private set; }

        public async Task Register(string username, string password)
        {
            string bsonId = await new UniqueId().GetUniqueBsonId();
            // Somehow we get a quoted string (ex. "a12...4ff", instead of a12...4ff)
            bsonId = bsonId.Replace("\"", "");

            const string apiUrl = "http://217.61.4.233:8080/MultiNotes.Server/api/user/";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiUrl));
            request.ContentType = "application/json";
            request.Method = "POST";

            User user = new User()
            {
                Id = bsonId,
                EmailAddress = username,
                PasswordHash = Encryption.Sha256(password),
                RegistrationTimestamp = DateTime.Now
            };

            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(user);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                IsRegisterSuccessful = true;
                RegisterMessage = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
            }
            catch (WebException e)
            {
                IsRegisterSuccessful = false;
                RegisterMessage = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
            }
        }
    }
}
