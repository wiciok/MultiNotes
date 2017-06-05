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
using Android.Net;

namespace MultiNotes.XAndroid
{
    public sealed class Utility
    {
        public static bool IsNetworkAvailable(Context context)
        {
            ConnectivityManager connectivityManager
                  = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            return activeNetworkInfo != null && activeNetworkInfo.IsConnected;
        }


        private Utility()
        {
        }
    }
}