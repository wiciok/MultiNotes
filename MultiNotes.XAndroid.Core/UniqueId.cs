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

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998 

namespace MultiNotes.XAndroid.Core
{
    public class UniqueId
    {
        public async Task<string> GetUniqueBsonId()
        {
            const string apiUrl = "http://217.61.4.233:8080/MultiNotes.Server/api/id/";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiUrl));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Somehow this doesn't work
            // using (WebResponse response = await request.GetResponseAsync())

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    return new StreamReader(stream).ReadToEnd();
                }
            }
        }
    }
}
