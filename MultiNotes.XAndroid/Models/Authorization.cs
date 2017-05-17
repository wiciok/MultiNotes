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
        private static IAuthorization instance = null;
        private static object syncRoot = new object();
        public static IAuthorization Instance
        {
            get
            {
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



        private bool signedIn;


        private Authorization()
        {
            signedIn = false;
        }


        /**
         * Implements IAuthorization.SignedIn { get; }
         */
        public bool SignedIn
        {
            get { return signedIn; }
        }


        /**
         * Implements IAuthorization.User { get; }
         */
        public User User
        {
            get { return null; }
        }


        /**
         * Implements IAuthorization.SignIn()
         */
        public bool SignIn(string username, string password)
        {
            signedIn = true;
            return signedIn;
        }


        /**
         * Implements IAuthorization.SignOut()
         */
        public void SignOut()
        {
            signedIn = false;
        }
    }
}
