using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core.Api
{
    public class RegisterApi : IRegisterApi
    {
        public bool IsRegisterSuccessful { get; private set; }
        public string RegisterMessage { get; private set; }

        public RegisterApi()
        {
            IsRegisterSuccessful = false;
            RegisterMessage = "";
        }

        public async Task Register(string username, string password)
        {
            await Task.Run(() => { RegisterImpl(username, password); });
        }


        private async void RegisterImpl(string username, string password)
        {
            string bsonId = await new UniqueIdApi().GetUniqueId();
            // Somehow we get a quoted string (ex. "a12...4ff", instead of a12...4ff)
            bsonId = bsonId.Replace("\"", "");
            if (bsonId.Length == 0)
            {
                IsRegisterSuccessful = false;
                RegisterMessage = "An exception occured while fetching unique bson id from sever.";
                return;
            }

            string apiUrl = Constants.ApiUrlBase + "api/user/";

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
