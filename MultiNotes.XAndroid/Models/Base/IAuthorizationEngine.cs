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

namespace MultiNotes.XAndroid.Models.Base
{
    public interface IAuthorizationEngine
    {

        bool SignedIn { get; }

        User User { get; }

        UserHeader UserHeader { get; }

        bool SignIn(string username, string password);

        void SignOut();

    }
}
