using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998

namespace MultiNotes.XAndroid.Core.Api
{
    public class UniqueIdApi : IUniqueIdApi
    {
        /// <exception cref="WebApiClientException"></exception>
        public string GetUniqueId()
        {
            return GetUniqueIdImpl();
        }

        /// <exception cref="WebApiClientException"></exception>
        private string GetUniqueIdImpl()
        {
            string apiUrl = Constants.ApiUrlBase + "api/id/";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiUrl));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                // Somehow this doesn't work
                // using (WebResponse response = await request.GetResponseAsync())
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                return new StreamReader(stream).ReadToEnd().Replace("\"", "");
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                {
                    throw new WebApiClientException(WebApiClientError.InternetConnectionError);
                }
                // TODO: do something!
                return "";
            }
        }
    }
}
