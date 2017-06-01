using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core.Api
{
    public class UserApi : IUserApi
    {
        /// <exception cref="WebApiClientException"></exception>
        public User GetUser(string token, string username)
        {
            return GetUserImpl(token, username);
        }


        /// <exception cref="WebApiClientException"></exception>
        private User GetUserImpl(string token, string username)
        {
            string apiUrl = Constants.ApiUrlBase + "api/user/{0}/{1}";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest
                .Create(new Uri(string.Format(apiUrl, token, Encryption.Base64Encode(username))));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                return JsonConvert.DeserializeObject<User>(new StreamReader(stream).ReadToEnd());
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                {
                    throw new WebApiClientException(WebApiClientError.InternetConnectionError);
                }
                return null;
            }
        }
    }
}
