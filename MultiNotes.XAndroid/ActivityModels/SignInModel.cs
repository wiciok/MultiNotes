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

using MultiNotes.XAndroid.ActivityModels.Base;

namespace MultiNotes.XAndroid.ActivityModels
{
    public class SignInModel : ISignInModel
    {
        public bool SignIn(string username, string password)
        {
            return true;
        }
    }
}