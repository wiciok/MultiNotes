using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MultiNotes.XAndroid.Core
{
    internal class LoginEngine : ILoginEngine
    {
        public bool IsLoginSuccessful { get; private set; }
        public string LoginMessage { get; private set; }


        public async Task Login(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
