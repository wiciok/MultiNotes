using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MultiNotes.Model;

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998 

namespace MultiNotes.XAndroid.Core
{
    public class UserApi : IUserApi
    {
        public async Task<User> GetUser(string token, string username)
        {
            string apiUrl = Constants.ApiUrlBase + "api/user/{0}/{1}";

            User user;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token, Encryption.Base64Encode(username))));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        user = JsonConvert.DeserializeObject<User>(new StreamReader(stream).ReadToEnd());
                        return user;
                    }
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}
