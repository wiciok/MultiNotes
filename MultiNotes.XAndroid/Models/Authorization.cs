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

        
        public bool SignedIn
        {
            get { return signedIn; }
        }

        
        public User User
        {
            get { return null; }
        }

        
        public bool SignIn(string username, string password)
        {
            signedIn = true;
            return signedIn;
        }

        
        public void SignOut()
        {
            signedIn = false;
        }
    }
}
