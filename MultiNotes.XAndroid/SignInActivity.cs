using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MultiNotes.XAndroid.Core;

// May be needed some day
// using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(MainActivity),
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.SignInActivity")]
    public sealed class SignInActivity : MultiNotesBaseActivity
    {

        private EditText emailAddressEditText;
        private EditText passwordEditText;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sign_in);

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
            new Thread(new ThreadStart(() =>
            {
                XUserMethod methods = new XUserMethod();
                ProgressDialog progress = null;

                RunOnUiThread(() =>
                {
                    progress = ProgressDialog.Show(
                        this,
                        Resources.GetString(Resource.String.please_wait),
                        Resources.GetString(Resource.String.please_wait),
                        true,
                        false
                    );
                });

                methods.Login(emailAddressEditText.Text.Trim(), passwordEditText.Text);
                RunOnUiThread(() =>
                {
                    progress.Hide();
                    if (methods.IsLoginSuccessful)
                    {
                        new AlertDialog.Builder(this)
                            .SetTitle(Resource.String.message)
                            .SetMessage(Resource.String.login_successful)
                            .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { Finish(); })
                            .Create().Show();
                    }
                    else
                    {
                        new AlertDialog.Builder(this)
                            .SetTitle(Resource.String.error)
                            .SetMessage(Resource.String.login_failed)
                            .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { })
                            .Create().Show();
                    }
                });
            })).Start();
            
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
