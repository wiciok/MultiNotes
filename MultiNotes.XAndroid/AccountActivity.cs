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
            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar_account));

            Button changePasswordButton = FindViewById<Button>(Resource.Id.account_button_change_password);
            Button signOutButton = FindViewById<Button>(Resource.Id.account_button_sign_out);
            Button settingsButton = FindViewById<Button>(Resource.Id.account_button_settings);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

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
