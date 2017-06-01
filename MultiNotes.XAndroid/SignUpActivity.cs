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

using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(SignInActivity),
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class SignUpActivity : MultiNotesBaseActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sign_up);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            EnableSupportToolbarHomeMenu();

            Button signUpButton = FindViewById<Button>(Resource.Id.button_sign_up);

            signUpButton.Click += SignUpButtonOnClick;
        }

        private void SignUpButtonOnClick(object sender, EventArgs args)
        {
            new Thread(new ThreadStart(() =>
            {
                XUserMethod userMethod = new XUserMethod();
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

                string username = FindViewById<EditText>(Resource.Id.edit_text_email_address).Text;
                string password = FindViewById<EditText>(Resource.Id.edit_text_password).Text;
                string passwordRetype = FindViewById<EditText>(Resource.Id.edit_text_repeat_password).Text;

                if (username.Length == 0 || password.Length == 0)
                {
                    new AlertDialog.Builder(this)
                        .SetTitle(Resource.String.error)
                        .SetMessage(Resource.String.fill_all_fields)
                        .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { })
                        .Show();
                    return;
                }

                if (password.Length < 6)
                {
                    new AlertDialog.Builder(this)
                        .SetTitle(Resource.String.error)
                        .SetMessage(Resource.String.password_too_short)
                        .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { })
                        .Show();
                    return;
                }

                if (password != passwordRetype)
                {
                    new AlertDialog.Builder(this)
                        .SetTitle(Resource.String.error)
                        .SetMessage(Resource.String.passwords_dont_match)
                        .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { })
                        .Show();
                    return;
                }
                
                userMethod.Register(username, password);
                
                RunOnUiThread(() => 
                {
                    progress.Hide();
                    if (!userMethod.IsRegisterSuccessful)
                    {
                        new AlertDialog.Builder(this)
                            .SetTitle(Resource.String.error)
                            .SetMessage(Resource.String.register_failed)
                            .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { })
                            .Show();
                    }
                    else
                    {
                        new AlertDialog.Builder(this)
                            .SetTitle(Resource.String.message)
                            .SetMessage(Resource.String.register_successful)
                            .SetPositiveButton(Resource.String.confirm_dialog_ok, delegate { Finish(); })
                            .SetCancelable(false)
                            .Show();
                    }
                });

            })).Start();
            
        }
    }
}
