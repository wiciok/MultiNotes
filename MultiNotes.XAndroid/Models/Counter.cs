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

namespace MultiNotes.XAndroid.Models
{
    public class Counter
    {
        private static int value = 0;
        public static string Next { get { return (++value).ToString(); } }
    }
}
