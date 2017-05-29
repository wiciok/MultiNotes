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

namespace MultiNotes.XAndroid.Core
{
    public sealed class Constants
    {
        public static string AuthenticationRecordFile { get { return GetAuthRecordFile(); } }


        private static string GetAuthRecordFile()
        {
            return System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                "MultiNotes", "user.dat"
            );
        }

        private Constants()
        {
        }
    }
}