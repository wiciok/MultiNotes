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
using Android.Content.PM;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false, ParentActivity = typeof(MainActivity), ScreenOrientation = ScreenOrientation.Portrait)]
    public class SignInActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignIn);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.ToolbarSignIn);
            SetActionBar(toolbar);

            Button moveToSignUpButton = (Button)FindViewById(Resource.Id.MoveToSignUpButton);
            moveToSignUpButton.Click += delegate
            {
                StartActivity(typeof(SignUpActivity));
            };
        }
    }
}
