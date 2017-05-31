using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998 

namespace MultiNotes.XAndroid.Core
{
    public class UniqueIdService : IUniqueIdService
    {
        private static readonly object syncRoot = new object();
        public string GetUniqueId()
        {
            lock (syncRoot)
            {
                return FetchUniqueIdFromApi();
            }
        }

        private string FetchUniqueIdFromApi()
        {
            string apiUrl = Constants.ApiUrlBase + "api/id/";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiUrl));
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                // Somehow this doesn't work
                // using (WebResponse response = await request.GetResponseAsync())
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        return new StreamReader(stream).ReadToEnd().Replace("\"", "");
                    }
                }
            }
            catch (WebException)
            {
                // TODO: do something!
                return "";
            }
        }
    }
}
