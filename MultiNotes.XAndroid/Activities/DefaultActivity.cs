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

namespace MultiNotes.XAndroid.Activities
{
    // Root for all activities in this project.
    public class DefaultActivity : Activity
    {
        // Each override in this code will cause changes
        // in behavior of all activities in this project.

        protected virtual bool MenuHomeOnClick()
        {
            Finish();
            return true;
        }
    }
}
