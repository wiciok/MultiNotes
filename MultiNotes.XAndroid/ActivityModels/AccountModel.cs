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
using MultiNotes.XAndroid.Models;
using MultiNotes.XAndroid.Models.Base;

namespace MultiNotes.XAndroid.ActivityModels
{
    public class AccountModel : IAccountModel
    {

        private IAuthorizationEngine authEngine;


        public AccountModel()
        {
            authEngine = AuthorizationEngine.Instance;
        }


        public void SignOut()
        {
            authEngine.SignOut();
        }
    }
}
