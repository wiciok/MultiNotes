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

using MultiNotes.Model;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Model.Base
{
    public interface IAuthorizationEngine
    {

        bool SignedIn { get; }

        User User { get; }

        Task<bool> SignIn(string username, string password);

        void SignOut();

    }
}
