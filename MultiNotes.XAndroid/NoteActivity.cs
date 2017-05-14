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
        ParentActivity = typeof(MainActivity), 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class NoteActivity : DefaultActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ActivityNote);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.ToolbarNote);
            SetActionBar(toolbar);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuNote, menu);
            return base.OnCreateOptionsMenu(menu);
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