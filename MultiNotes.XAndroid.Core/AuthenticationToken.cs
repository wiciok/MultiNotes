using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

using MultiNotes.Model;

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998

namespace MultiNotes.XAndroid.Core
{
    public class AuthenticationToken
    {
        public async Task<string> GetAuthenticationToken(AuthenticationRecord record)
        {
            const string apiUrl = "http://217.61.4.233:8080/MultiNotes.Server/api/auth/";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiUrl));
            request.ContentType = "application/json";
            request.Method = "POST";
            
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(record);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
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
            catch (WebException)
            {
                // TODO: do something!
                return "";
            }
        }
    }
}