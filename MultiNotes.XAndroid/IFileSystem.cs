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
using Java.IO;
using MultiNotes.Core;

namespace MultiNotes.XAndroid
{
    public interface IFileSystem
    {
        File FilesDir { get; set; }

        List<Note> GetSavedNotes();
        void SaveNotes(List<Note> notes);
    }
}