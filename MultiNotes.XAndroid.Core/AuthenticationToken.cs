using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;

using Newtonsoft.Json;

using MultiNotes.Model;

// Disable warning: Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS1998

namespace MultiNotes.XAndroid.Core
{
    public class AuthenticationToken
    {
        private string token;
        private bool networkConnectionError;

        public AuthenticationToken()
        {
            networkConnectionError = false;
        }

        
        public string GetAuthenticationToken(AuthenticationRecord record)
        {
            LoadAuthenticationToken(record);
            if (networkConnectionError)
            {
                throw new WebApiClientException(WebApiClientError.InternetConnectionError);
            }
            return token;
        }


        public async void LoadAuthenticationToken(AuthenticationRecord record)
        {
            string apiUrl = Constants.ApiUrlBase + "api/auth/";
            networkConnectionError = false;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiUrl));
            request.ContentType = "application/json";
            request.Method = "POST";

            try
            {
                StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
                string json = JsonConvert.SerializeObject(record);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                // Somehow this doesn't work
                // using (WebResponse response = await request.GetResponseAsync())
                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();

                token = new StreamReader(stream).ReadToEnd().Replace("\"", "");
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                {
                    networkConnectionError = true;
                }
                // TODO: do something!
                token = "";
            }
        }
    }
}