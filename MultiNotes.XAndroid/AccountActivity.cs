﻿using System;
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
    [Activity(MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class AccountActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here.
            SetContentView(Resource.Layout.Account);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.ToolbarAccount);
            SetActionBar(toolbar);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Button changePasswordButton = (Button)FindViewById(Resource.Id.Account_ChangePasswordButton);
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
                    return MenuHomeItemOnClick();

                default:
                    return base.OnOptionsItemSelected(menuItem);
            }
        }

        private bool MenuHomeItemOnClick()
        {
            Finish();
            return true;
        }

        private void ChangePasswordButtonOnClick()
        {
            // TODO: Widok zmiany hasła.
            Toast.MakeText(this, "Tu pojawi się widok zmiany hasła!", ToastLength.Short).Show();
        }
    }
}