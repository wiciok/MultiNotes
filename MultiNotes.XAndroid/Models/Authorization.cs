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

using MultiNotes.Core;

namespace MultiNotes.XAndroid.Models
{
    public class Authorization : IAuthorization
    {
        private bool successful;

        public Authorization()
        {
            successful = false;
        }

        public bool Successful { get { return successful; } }

        public User User { get { return null; } }

        public bool SignIn(string username, string password)
        {
            successful = true;
            return successful;
        }
    }
}
