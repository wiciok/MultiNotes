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

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(AccountActivity),
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public class ChangePasswordActivity : MultiNotesBaseActivity
    {

        private EditText oldPasswordEditText;
        private EditText passwordEditText;
        private EditText repeatPasswordEditText;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_change_password);
            SetSupportActionBar(FindViewById<SupportToolbar>(Resource.Id.toolbar));

            // Set up field components
            oldPasswordEditText = FindViewById<EditText>(Resource.Id.edit_text_old_password);
            passwordEditText = FindViewById<EditText>(Resource.Id.edit_text_password);
            repeatPasswordEditText = FindViewById<EditText>(Resource.Id.edit_text_repeat_password);

            // Set up local variable components
            Button changePasswordButton = FindViewById<Button>(Resource.Id.button_change_password);

            EnableSupportToolbarHomeMenu();

            changePasswordButton.Click += ChangePasswordButtonOnClick;
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
            Toast.MakeText(this, "Click!", ToastLength.Short).Show();
            Finish();
        }
    }
}
