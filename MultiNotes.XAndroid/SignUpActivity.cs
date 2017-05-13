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
    [Activity(MainLauncher = false, ParentActivity = typeof(SignInActivity), ScreenOrientation = ScreenOrientation.Portrait)]
    public class SignUpActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here.
            SetContentView(Resource.Layout.SignUp);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.ToolbarSignUp);
            SetActionBar(toolbar);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
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
    }
}