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
using MultiNotes.XAndroid.Model.Base;
using System.Net.Http;
using System.Threading.Tasks;

namespace MultiNotes.XAndroid.Model
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
        
        private bool signedIn;


        private AuthorizationEngine()
        {
            signedIn = false;
        }

        
        public bool SignedIn
        {
            get { return User != null; }
        }

        
        public User User
        {
            get { return null; }
        }

        
        public async Task<bool> SignIn(string username, string password)
        {
            //UserMethod userMethods = new UserMethod(ConnectionApi.HttpClient);

            //try
            //{
            //    await userMethods.Login(username, password);
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            //return signedIn;
            return false;
        }

        
        public void SignOut()
        {
            signedIn = false;
        }
    }
}
