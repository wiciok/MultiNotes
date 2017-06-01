using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MultiNotes.XAndroid.Core
{
    [Serializable]
    public class WebApiClientException : Exception
    {
        public WebApiClientError Error { get; private set; }


        public WebApiClientException(WebApiClientError error)
        {
            Error = error;
        }


        public WebApiClientException(string message, WebApiClientError error) : base(message)
        {
            Error = error;
        }


        public WebApiClientException(string message, 
                                     Exception innerException, 
                                     WebApiClientError error) : base(message, innerException)
        {
            Error = error;
        }


        protected WebApiClientException(SerializationInfo info, 
                                        StreamingContext context, 
                                        WebApiClientError error) : base(info, context)
        {
            Error = error;
        }
    }
}