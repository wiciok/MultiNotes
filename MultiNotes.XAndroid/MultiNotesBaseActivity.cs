using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace MultiNotes.XAndroid
{
    // Root for all activities in this project.
    public abstract class MultiNotesBaseActivity : AppCompatActivity
    {
        // Each override in this code will cause changes
        // in behavior of all activities in this project.

        protected virtual bool MenuHomeOnClick()
        {
            Finish();
            return true;
        }


        protected void EnableToolbarHomeMenu()
        {
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
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

    }
}
