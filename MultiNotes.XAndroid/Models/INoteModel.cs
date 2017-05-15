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

using MultiNotes.Core;

namespace MultiNotes.XAndroid.Models
{
    public interface INoteModel
    {
        string NoteId { get; }
        string NoteContent { get; set; }

        void SaveChanges();
        void DeleteNote();
        void AddNote();
    }
}
