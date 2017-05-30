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

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using System.Threading;
using MultiNotes.XAndroid.Core;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class AccountActivity : MultiNotesBaseActivity
    {

        //private IAccountModel model;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_account);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            // Set up local variable components
            TextView emailAddressTextView = FindViewById<TextView>(Resource.Id.text_view_user_email);

            Button changePasswordButton = FindViewById<Button>(Resource.Id.button_change_password);
            Button signOutButton = FindViewById<Button>(Resource.Id.button_sign_out);
            Button settingsButton = FindViewById<Button>(Resource.Id.button_settings);

            EnableSupportToolbarHomeMenu();

            emailAddressTextView.Text = AuthorizationManager.Instance.User.EmailAddress;

            changePasswordButton.Click += ChangePasswordButtonOnClick;
            signOutButton.Click += SignOutButtonOnClick;
            settingsButton.Click += SettingsButtonOnClick;

            //model = new AccountModel();
        }

        public override bool OnOptionsItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Android.Resource.Id.Home:
                    return MenuHomeOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }
        

        private void ChangePasswordButtonOnClick(object sender, EventArgs e)
        {
            StartActivity(typeof(ChangePasswordActivity));
        }

        
        private void SignOutButtonOnClick(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                XUserMethod methods = new XUserMethod();
                methods.Logout();
                RunOnUiThread(() => Finish());
            })).Start();
        }


        private void SettingsButtonOnClick(object sender, EventArgs e)
        {
            StartActivity(typeof(SettingsActivity));
        }

    }
}
