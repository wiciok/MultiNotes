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
    [Activity(MainLauncher = false,
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public sealed class AccountActivity : DefaultActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_account);
            SetActionBar(FindViewById<Toolbar>(Resource.Id.toolbar_account));

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Button changePasswordButton = FindViewById<Button>(Resource.Id.account_button_change_password);
            changePasswordButton.Click += delegate
            {
                ChangePasswordButtonOnClick();
            };
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


        private void ChangePasswordButtonOnClick()
        {
            // TODO: Widok zmiany hasła.
            Toast.MakeText(this, "Tu pojawi się widok zmiany hasła!", ToastLength.Short).Show();
        }

    }
}
