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

namespace MultiNotes.XAndroid.Core
{
    public class LocalUniqueIdService : IUniqueIdService
    {
        private static readonly object syncRoot = new object();

        public string GetUniqueId()
        {
            lock (syncRoot)
            {
                int currentId;
                if (!System.IO.File.Exists(Constants.LocalIdFile))
                {
                    currentId = 0;
                }
                else
                {
                    try
                    {
                        currentId = int.Parse(System.IO.File.ReadAllText(Constants.LocalIdFile));
                    }
                    catch
                    {
                        currentId = 0;
                    }
                }
                System.IO.File.WriteAllText(Constants.LocalIdFile, (++currentId).ToString());
                return currentId.ToString();
            }
        }
    }
}
