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
using MultiNotes.XAndroid.Models;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class AccountActivity : MultiNotesBaseActivity
    {

        private IAccountModel model;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_account);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            // Set up local variable components
            Button changePasswordButton = FindViewById<Button>(Resource.Id.button_change_password);
            Button signOutButton = FindViewById<Button>(Resource.Id.button_sign_out);
            Button settingsButton = FindViewById<Button>(Resource.Id.button_settings);

            EnableSupportToolbarHomeMenu();

            changePasswordButton.Click += ChangePasswordButtonOnClick;
            signOutButton.Click += SignOutButtonOnClick;
            settingsButton.Click += SettingsButtonOnClick;

            model = new AccountModel();
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
            model.Authorization.SignOut();
            Finish();
        }


        private void SettingsButtonOnClick(object sender, EventArgs e)
        {
            StartActivity(typeof(SettingsActivity));
        }

    }
}
