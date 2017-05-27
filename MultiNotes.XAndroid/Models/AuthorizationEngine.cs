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
using MultiNotes.Model;
using MultiNotes.XAndroid.Models.Base;

namespace MultiNotes.XAndroid.Models
{
    public class AuthorizationEngine : IAuthorizationEngine
    {
        private static IAuthorizationEngine instance = null;
        private static object syncRoot = new object();
        public static IAuthorizationEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new AuthorizationEngine();
                        }
                    }
                }
                return instance;
            }
        }


        // Non-static part starts here
        
        private UserHeader userHeader;
        private bool signedIn;


        private AuthorizationEngine()
        {
            userHeader = UserHeader.Empty;
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

        public UserHeader UserHeader
        {
            get { return userHeader; }
        }

        
        public bool SignIn(string username, string password)
        {
            userHeader = new UserHeader("1", "example@multinotes.pl");
            signedIn = true;
            return signedIn;
        }

        
        public void SignOut()
        {
            signedIn = false;
        }
    }
}
