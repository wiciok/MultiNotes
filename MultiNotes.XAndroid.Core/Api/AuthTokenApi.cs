using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;

using Newtonsoft.Json;

using MultiNotes.Model;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Core.Api
{
    internal class AuthTokenApi : IAuthTokenApi
    {

        /// <exception cref="WebApiClientException"></exception>
        public string GetAuthToken(AuthenticationRecord record)
        {
            return GetAsyncTaskImpl(record);
        }


        /// <exception cref="WebApiClientException"></exception>
        private string GetAsyncTaskImpl(AuthenticationRecord record)
        {
            string apiUrl = Constants.ApiUrlBase + "api/auth/";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiUrl));
            request.ContentType = "application/json";
            request.Method = "POST";

            try
            {
                request.Timeout = 5000;

                StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
                string json = JsonConvert.SerializeObject(record);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                // Somehow this doesn't work
                // using (WebResponse response = await request.GetResponseAsync())
                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();

                return new StreamReader(stream).ReadToEnd().Replace("\"", "");
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure
                    || e.Status == WebExceptionStatus.Timeout)
                {
                    throw new WebApiClientException(WebApiClientError.InternetConnectionError);
                }
                // TODO: do something!
                return "";
            }
        }
    }
}