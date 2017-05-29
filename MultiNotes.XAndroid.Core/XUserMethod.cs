using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public class XUserMethod
    {
        public bool IsRegisterSuccessful { get; private set; }
        public string RegisterMessage { get; private set; }


        public bool IsLoginSuccessful { get; private set; }
        public string LoginMessage { get; private set; }

        public XUserMethod()
        {
            IsRegisterSuccessful = false;
        }

        public async Task Register(string username, string password)
        {
            IRegistration registration = new Registration();
            await registration.Register(username, password);
            IsRegisterSuccessful = registration.IsRegisterSuccessful;
            RegisterMessage = registration.RegisterMessage;
        }


        public async Task Login(string username, string password)
        {
            ILoginEngine loginEngine = new LoginEngine();
            await loginEngine.Login(username, password);
            IsLoginSuccessful = loginEngine.IsLoginSuccessful;
            LoginMessage = loginEngine.LoginMessage;
        }
    }
}