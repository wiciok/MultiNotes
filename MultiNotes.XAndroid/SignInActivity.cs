using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MultiNotes.XAndroid.Models;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(MainActivity)
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SignInActivity : DefaultActivity
    {
        Button signInButton;
        Button signUpButton;

        private IAuthorization authorization;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            authorization = new AuthorizationFactory().Create();

            // Create your application here.
            SetContentView(Resource.Layout.ActivitySignIn);

            signInButton = FindViewById<Button>(Resource.Id.SignIn_SignInButton);
            signUpButton = FindViewById<Button>(Resource.Id.SignIn_SignUpButton);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.ToolbarSignIn);
            SetActionBar(toolbar);

            signInButton.Click += SignInButtonOnClick;
            signUpButton.Click += SignUpButtonOnClick;
        }

        private void SignInButtonOnClick(object sender, EventArgs e)
        {
            if (authorization.SignIn("", ""))
            {
                Finish();
            }
        }

        private void SignUpButtonOnClick(object sender, EventArgs e)
        {
            StartActivity(typeof(SignUpActivity));
        }

        public override void OnBackPressed()
        {
            // Cannot allow to show MainActivity afrer back button was 
            // pressed. SignInActivity will close itself after 
            // authorization is successful. This method finishes this 
            // activity and all activities below (in this case below
            // will be MainActivity).
            FinishAffinity();
        }
    }
}
