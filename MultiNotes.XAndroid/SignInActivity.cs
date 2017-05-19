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

using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(MainActivity),
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class SignInActivity : MultiNotesBaseActivity
    {

        private EditText emailAddressEditText;
        private EditText passwordEditText;

        private IAuthorization model;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sign_in);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            model = Authorization.Instance;

            // Set up field components
            emailAddressEditText = FindViewById<EditText>(Resource.Id.edit_text_email_address);
            passwordEditText = FindViewById<EditText>(Resource.Id.edit_text_password);

            // Set up local variable components
            Button signInButton = FindViewById<Button>(Resource.Id.button_sign_in);
            Button signUpButton = FindViewById<Button>(Resource.Id.button_sign_up);

            signInButton.Click += SignInButtonOnClick;
            signUpButton.Click += SignUpButtonOnClick;
        }


        private void SignInButtonOnClick(object sender, EventArgs e)
        {
            if (model.SignIn(emailAddressEditText.Text, passwordEditText.Text))
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
