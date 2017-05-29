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

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using MultiNotes.Core;
using System.Threading.Tasks;
using MultiNotes.XAndroid.Core;

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

        private async void SignUpButtonOnClick(object sender, EventArgs args)
        {
            string username = FindViewById<EditText>(Resource.Id.edit_text_email_address).Text;
            string password = FindViewById<EditText>(Resource.Id.edit_text_password).Text;
            string passwordRetype = FindViewById<EditText>(Resource.Id.edit_text_repeat_password).Text;

            if (username.Length == 0 || password.Length == 0)
            {
                new AlertDialog.Builder(this)
                    .SetTitle("Błąd")
                    .SetMessage("Proszę uzupełnić wszystkie pola.")
                    .SetPositiveButton("OK", delegate { })
                    .Show();
                return;
            }

            if (password.Length < 6)
            {
                new AlertDialog.Builder(this)
                    .SetTitle("Błąd")
                    .SetMessage("Hasło jest za krótkie.")
                    .SetPositiveButton("OK", delegate { })
                    .Show();
                return;
            }

            if (password != passwordRetype)
            {
                new AlertDialog.Builder(this)
                    .SetTitle("Błąd")
                    .SetMessage("Hasła się nie zgadzają.")
                    .SetPositiveButton("OK", delegate { })
                    .Show();
                return;
            }

            XUserMethod userMethod = new XUserMethod();
            await userMethod.Register(username, password);
            if (!userMethod.IsRegisterSuccessful)
            {
                new AlertDialog.Builder(this)
                       .SetTitle("Błąd")
                       .SetMessage("Podczas rejestracji wystąpił nieoczekiwany błąd.")
                       .SetPositiveButton("OK", delegate { })
                       .Show();
            }
        }
    }
}
