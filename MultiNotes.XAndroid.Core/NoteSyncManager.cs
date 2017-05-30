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

using MultiNotes.Model;

namespace MultiNotes.XAndroid.Core
{
    public class NoteSyncManager
    {
        public void Sync(INoteRepository remote, INoteRepository local)
        {
            List<Note> remoteList = remote.GetAllNotes();
            List<Note> localList = local.GetAllNotes();
        }
    }
}