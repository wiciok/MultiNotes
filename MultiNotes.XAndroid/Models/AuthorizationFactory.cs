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
    public class AuthorizationFactory : IAuthorizationFactory
    {
        private static IAuthorization instance = null;
        private static readonly object syncRoot = new object();

        public IAuthorization Create()
        {
            // Double check to avoid expensive lock 
            // operation when not needed
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Authorization();
                    }
                }
            }
            return instance;
        }
    }
}