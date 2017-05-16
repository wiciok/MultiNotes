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

namespace MultiNotes.XAndroid
{
    [Activity(MainLauncher = false,
        ParentActivity = typeof(AccountActivity),
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/AppTheme.NoActionBar")]
    public class ChangePasswordActivity : DefaultActivity
    {
        
        private EditText oldPasswordEditText;
        private EditText passwordEditText;
        private EditText repeatPasswordEditText;
        private Button changePasswordButton;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_change_password);

            oldPasswordEditText = FindViewById<EditText>(Resource.Id.change_password_edit_text_old_password);
            passwordEditText = FindViewById<EditText>(Resource.Id.change_password_edit_text_password);
            repeatPasswordEditText = FindViewById<EditText>(Resource.Id.change_password_edit_text_repeat_password);
            changePasswordButton = FindViewById<Button>(Resource.Id.change_password_button_change_password);

            changePasswordButton.Click += ChangePasswordButtonOnClick;
        }

        private void ChangePasswordButtonOnClick(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Click!", ToastLength.Short).Show();
        }
    }
}