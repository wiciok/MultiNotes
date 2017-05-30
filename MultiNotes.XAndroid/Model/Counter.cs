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

namespace MultiNotes.XAndroid.Model
{
    public class Counter
    {
        private static Counter defaultCounter = new Counter();
        public static Counter Default
        {
            get { return defaultCounter; }
        }


        private int value = 0;

        public int Next
        {
            get { return ++value; }
        }

        public int Current
        {
            get { return value; }
        }

    }
}
