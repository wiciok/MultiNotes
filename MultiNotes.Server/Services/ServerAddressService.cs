using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiNotes.Server.Services
{
    public class ServerAddressService
    {
        public static string ServerAddress
        {
            get
            {
                return "localhost";
                //return "http://217.61.4.233:8080/MultiNotes.Server";
            }
        }
    }
}