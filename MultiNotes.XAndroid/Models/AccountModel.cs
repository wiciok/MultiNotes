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

namespace MultiNotes.XAndroid.Models
{
    public class AccountModel : IAccountModel
    {

        private IAuthorization authorization;


        public AccountModel()
        {
            authorization = Models.Authorization.Instance;
        }


        /**
         * Implements IAccountModel.Authorization
         */
        public IAuthorization Authorization
        {
            get { return authorization; }
        }

    }
}
